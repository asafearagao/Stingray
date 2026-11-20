using System;
using System.Collections.Generic;  // Listas
using MySqlConnector;              // Conexão com o MySQL
using Stingray.BancoDeDados;
using Stingray.Entidades;

namespace Stingray.Repositorios
{

    // Executa as consultas e alterações de professores no banco de dados
    public class ProfessorRepositorio : RepositorioMySqlBase
    {
        private readonly PessoaRepositorio pessoaRepositorio = new PessoaRepositorio();

        // Consulta os dados do professor e das turmas vinculadas
        private const string SqlProfessor = @"
            SELECT pr.id_professor, pr.cref, p.nome, p.data_nascimento, p.cpf, p.rg,
                   p.email, p.id_endereco, e.numero, e.cep, e.complemento,
                   tel.numero_telefone AS telefone, turmas.lista AS turmas
            FROM professor pr
            INNER JOIN pessoa p ON p.id_pessoa = pr.id_professor
            INNER JOIN endereco e ON e.id_endereco = p.id_endereco
            LEFT JOIN (
                SELECT id_pessoa, MIN(id_telefone) AS id_telefone
                FROM telefone GROUP BY id_pessoa
            ) telefone_principal ON telefone_principal.id_pessoa = p.id_pessoa
            LEFT JOIN telefone tel ON tel.id_telefone = telefone_principal.id_telefone
            LEFT JOIN (
                SELECT id_professor,
                       GROUP_CONCAT(DISTINCT CONCAT(
                           modalidade, ' — ', dia_semana, ' · ', TIME_FORMAT(horario_inicio, '%H:%i')
                       ) ORDER BY dia_semana, horario_inicio, modalidade SEPARATOR '|||') AS lista
                FROM turma GROUP BY id_professor
            ) turmas ON turmas.id_professor = pr.id_professor";

        public List<string> ListarNomes()
        {
            // Consulta SQL para obter os nomes dos professores
            const string sql = @"
                SELECT p.nome
                FROM professor pr
                INNER JOIN pessoa p ON p.id_pessoa = pr.id_professor
                ORDER BY p.nome;";

            // Executa a consulta e coloca os nomes em uma lista de strings
            return ConsultarLista(sql, null, leitor => LeitorMySql.Texto(leitor, "nome"));
        }

        public List<Professor> ListarTodos()
        {
            string sql = SqlProfessor + " ORDER BY p.nome;";
            return ConsultarLista(sql, null, MapearProfessor);
        }

        public Professor BuscarPorId(int idProfessor)
        {
            // Valida o ID do professor
            if (idProfessor <= 0)
                throw new ArgumentException("ID do professor inválido.", nameof(idProfessor));

            string sql = SqlProfessor + " WHERE pr.id_professor = @idProfessor LIMIT 1;";

            return ConsultarUnico(
                sql,
                comando => comando.Parameters.AddWithValue("@idProfessor", idProfessor),
                MapearProfessor);
        }

        public int Inserir(Professor professor)
        {
            // Consulta SQL para inserir os dados do professor
            const string sqlProfessor = @"
                INSERT INTO professor (id_professor, cref)
                VALUES (@idPessoa, @cref);";

            if (professor == null) // Valida se o professor não é nulo
                throw new ArgumentNullException(nameof(professor));

            return ExecutarTransacao<int>(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int idEndereco = pessoaRepositorio.InserirEndereco(conexao, transacao, professor.Endereco);
                int idPessoa = pessoaRepositorio.InserirPessoa(conexao, transacao, professor.Pessoa, idEndereco);

                using (MySqlCommand comando = new MySqlCommand(sqlProfessor, conexao, transacao))
                {
                    comando.Parameters.AddWithValue("@idPessoa", idPessoa);
                    comando.Parameters.AddWithValue("@cref", professor.Cref.Trim());

                    comando.ExecuteNonQuery(); // Executa o comando SQL
                }

                pessoaRepositorio.SincronizarTelefone(conexao, transacao, idPessoa, professor.Telefone);
                return idPessoa;
            });
        }

        public void Atualizar(Professor professor)
        {
            // Consulta SQL para atualizar os dados do professor
            const string sqlProfessor = @"
                UPDATE professor
                SET cref = @cref
                WHERE id_professor = @idProfessor;";

            if (professor == null) // Valida se o professor não é nulo
                throw new ArgumentNullException(nameof(professor));

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int idEndereco = pessoaRepositorio.ObterIdEndereco(
                    conexao, transacao, professor.IdProfessor);

                pessoaRepositorio.AtualizarEndereco(
                    conexao, transacao, professor.Endereco, idEndereco);

                pessoaRepositorio.AtualizarPessoa(
                    conexao, transacao, professor.Pessoa, professor.IdProfessor);

                using (MySqlCommand comando = new MySqlCommand(sqlProfessor, conexao, transacao))
                {
                    comando.Parameters.AddWithValue("@cref", professor.Cref.Trim());
                    comando.Parameters.AddWithValue("@idProfessor", professor.IdProfessor);

                    // Executa o comando SQL e verifica se algum registro foi atualizado
                    if (comando.ExecuteNonQuery() == 0 && !ProfessorExiste(conexao, transacao, professor.IdProfessor))
                        throw new InvalidOperationException("O professor não foi encontrado no banco de dados.");
                }

                // Sincroniza o telefone do professor nas tabelas relacionadas
                pessoaRepositorio.SincronizarTelefone(
                    conexao, transacao, professor.IdProfessor, professor.Telefone);
            });
        }

        public void Excluir(int idProfessor)
        {
            if (idProfessor <= 0) // Valida o ID do professor
                throw new ArgumentException("Informe um professor válido.", nameof(idProfessor));

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int idEndereco = ObterEnderecoDoProfessor(conexao, transacao, idProfessor);
                List<int> idsTurmas = new List<int>();
                using (MySqlCommand comando = new MySqlCommand(
                    // Consulta SQL para obter os IDs das turmas vinculadas ao professor
                    "SELECT id_turma FROM turma WHERE id_professor = @idProfessor FOR UPDATE;", conexao, transacao))
                {
                    comando.Parameters.AddWithValue("@idProfessor", idProfessor);

                    // Executa a consulta e adiciona os IDs das turmas à lista
                    using (MySqlDataReader leitor = comando.ExecuteReader())
                        while (leitor.Read()) idsTurmas.Add(Convert.ToInt32(leitor[0]));
                }

                foreach (int idTurma in idsTurmas)
                {
                    // Exclui as dependências da turma antes de excluir a própria turma
                    TurmaRepositorio.ExcluirDependenciasTurma(conexao, transacao, idTurma);
                    using (MySqlCommand comando = new MySqlCommand(
                        "DELETE FROM turma WHERE id_turma = @idTurma;", conexao, transacao))
                    {
                        comando.Parameters.AddWithValue("@idTurma", idTurma);

                        comando.ExecuteNonQuery(); // Executa o comando SQL para excluir a turma
                    }
                }

                // Exclui os registros relacionados ao professor
                pessoaRepositorio.ExcluirTelefones(conexao, transacao, idProfessor);
                ExecutarExclusao(conexao, transacao,
                    "DELETE FROM professor WHERE id_professor = @id;", idProfessor);
                pessoaRepositorio.ExcluirPessoa(conexao, transacao, idProfessor);
                pessoaRepositorio.ExcluirEndereco(conexao, transacao, idEndereco);
            });
        }

        private static Professor MapearProfessor(MySqlDataReader leitor)
        {
            Professor professor = new Professor
            {
                IdProfessor = LeitorMySql.Inteiro(leitor, "id_professor"),
                Cref = LeitorMySql.Texto(leitor, "cref")
            };

            professor.Pessoa = new Pessoa
            {
                IdPessoa = professor.IdProfessor,
                IdEndereco = LeitorMySql.Inteiro(leitor, "id_endereco"),
                Nome = LeitorMySql.Texto(leitor, "nome"),
                DataNascimento = LeitorMySql.Data(leitor, "data_nascimento"),
                Cpf = LeitorMySql.Texto(leitor, "cpf"),
                Rg = LeitorMySql.Texto(leitor, "rg"),
                Email = LeitorMySql.Texto(leitor, "email")
            };

            professor.Endereco = new Endereco
            {
                IdEndereco = professor.Pessoa.IdEndereco,
                Numero = LeitorMySql.Texto(leitor, "numero"),
                Cep = LeitorMySql.Texto(leitor, "cep"),
                Complemento = LeitorMySql.Texto(leitor, "complemento")
            };

            professor.Telefone = new Telefone
            {
                IdPessoa = professor.IdProfessor,
                NumeroTelefone = LeitorMySql.Texto(leitor, "telefone", string.Empty)
            };

            // Lê a lista de turmas vinculadas ao professor, separadas por "|||"
            string turmas = LeitorMySql.Texto(leitor, "turmas", string.Empty);
            professor.Turmas = string.IsNullOrWhiteSpace(turmas)
                ? new List<string>()
                : new List<string>(turmas.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries));

            return professor;
        }

        private static bool ProfessorExiste(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idProfessor)
        {
            // Consulta SQL para verificar se o professor existe no banco de dados
            const string sql = "SELECT EXISTS(SELECT 1 FROM professor WHERE id_professor = @idProfessor);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idProfessor", idProfessor);

                // Consulta e retorna true se o professor existir
                return Convert.ToInt32(comando.ExecuteScalar()) == 1;
            }
        }

        private static int ObterEnderecoDoProfessor(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idProfessor)
        {
            // Consulta SQL para obter o ID do endereço do professor
            const string sql = @"
                SELECT p.id_endereco
                FROM professor pr
                INNER JOIN pessoa p ON p.id_pessoa = pr.id_professor
                WHERE pr.id_professor = @idProfessor
                FOR UPDATE;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idProfessor", idProfessor);

                // Executa a consulta e obtém o resultado
                object resultado = comando.ExecuteScalar();

                // Se o resultado for nulo ou DBNull, é falha na consulta
                if (resultado == null || resultado == DBNull.Value)
                    throw new InvalidOperationException("O professor não foi encontrado no banco de dados.");

                return Convert.ToInt32(resultado);
            }
        }

        private static void ExecutarExclusao(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            string sql,
            int idProfessor)
        {
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@id", idProfessor);
                if (comando.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("O registro do professor não foi encontrado.");
            }
        }
    }
}
