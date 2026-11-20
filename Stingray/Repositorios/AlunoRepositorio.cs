using System;
using System.Collections.Generic;  // Listas
using MySqlConnector;              // Conexão com o MySQL
using Stingray.BancoDeDados;
using Stingray.Entidades;

namespace Stingray.Repositorios
{
    // Mantém as alterações dos dados relacionados na mesma transação
    public class AlunoRepositorio : RepositorioMySqlBase
    {
        private readonly PessoaRepositorio pessoaRepositorio = new PessoaRepositorio();

        // Consulta os dados de cadastro dos alunos
        private const string SqlListarAlunos = @"
            SELECT a.id_aluno, a.observacao, p.nome, p.data_nascimento, p.cpf, p.rg,
                   p.email, p.id_endereco, e.numero, e.cep, e.complemento,
                   tel.numero_telefone AS telefone,
                   CASE WHEN m.situacao IN ('Ativo', 'Inativo') THEN m.id_matricula ELSE NULL END AS id_matricula,
                   m.data_matricula, m.dia_vencimento, m.situacao, m.inativo_automatico,
                   vinculos.modalidades, vinculos.professores, vinculos.primeira_turma,
                    COALESCE(vinculos.quantidade_turmas, 0) AS quantidade_turmas,
                   COALESCE(vinculos.valor_plano, 0) AS valor_plano
            FROM aluno a
            INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
            INNER JOIN endereco e ON e.id_endereco = p.id_endereco
            LEFT JOIN (
                SELECT matricula_atual.* FROM matricula matricula_atual
                INNER JOIN (
                    SELECT id_aluno, MAX(id_matricula) AS id_matricula
                    FROM matricula GROUP BY id_aluno
                ) ultima ON ultima.id_matricula = matricula_atual.id_matricula
            ) m ON m.id_aluno = a.id_aluno
            LEFT JOIN (
                SELECT id_pessoa, MIN(id_telefone) AS id_telefone
                FROM telefone GROUP BY id_pessoa
            ) telefone_principal ON telefone_principal.id_pessoa = p.id_pessoa
            LEFT JOIN telefone tel ON tel.id_telefone = telefone_principal.id_telefone
            LEFT JOIN (
                SELECT tm.id_matricula,
                       GROUP_CONCAT(CONCAT(t.modalidade, ' · ', t.dia_semana, ' · ', TIME_FORMAT(t.horario_inicio, '%H:%i'), '–', TIME_FORMAT(t.horario_fim, '%H:%i'))
                           ORDER BY t.dia_semana, t.horario_inicio, t.modalidade SEPARATOR '||') AS modalidades,
                       SUBSTRING_INDEX(GROUP_CONCAT(CONCAT(t.modalidade, ' · ', t.dia_semana, ' · ', TIME_FORMAT(t.horario_inicio, '%H:%i'), '–', TIME_FORMAT(t.horario_fim, '%H:%i'))
                           ORDER BY t.dia_semana, t.horario_inicio, t.modalidade SEPARATOR '||'), '||', 1) AS primeira_turma,
                       COUNT(DISTINCT t.id_turma) AS quantidade_turmas,
                       COALESCE(SUM(t.valor_mensal), 0) AS valor_plano,
                       GROUP_CONCAT(DISTINCT pessoa_professor.nome ORDER BY pessoa_professor.nome SEPARATOR ', ') AS professores
                FROM (
                    SELECT DISTINCT id_matricula, id_turma
                    FROM turma_matricula
                ) tm
                INNER JOIN turma t ON t.id_turma = tm.id_turma
                LEFT JOIN professor pr ON pr.id_professor = t.id_professor
                LEFT JOIN pessoa pessoa_professor ON pessoa_professor.id_pessoa = pr.id_professor
                GROUP BY tm.id_matricula
            ) vinculos ON vinculos.id_matricula = m.id_matricula AND m.situacao IN ('Ativo', 'Inativo')
            ";

        private const string SqlCondicoesFiltros = @"
            WHERE
                (@idTurma = 0 AND @idProfessor = 0 OR EXISTS (
                    SELECT 1
                    FROM turma_matricula tm_filtro
                    INNER JOIN turma t_filtro ON t_filtro.id_turma = tm_filtro.id_turma
                    WHERE tm_filtro.id_matricula = m.id_matricula
                      AND m.situacao IN ('Ativo', 'Inativo')
                      AND (@idTurma = 0 OR t_filtro.id_turma = @idTurma)
                      AND (@idProfessor = 0 OR t_filtro.id_professor = @idProfessor)
                ))
                AND (@situacao = '' OR m.situacao = @situacao)
                AND (@busca = '' OR p.nome LIKE CONCAT('%', @busca, '%')
                     OR COALESCE(p.cpf, '') LIKE CONCAT('%', @busca, '%'))
            ORDER BY p.nome;";

        // Lista os alunos com seus dados de cadastro
        public List<Aluno> ListarTodos()
        {
            return ConsultarLista(SqlListarAlunos + " ORDER BY p.nome;", null, MapearAluno);
        }

        // Aplica os filtros na própria consulta
        public List<Aluno> ListarFiltrados(int idTurma, int idProfessor, string situacao, string busca)
        {
            string sql = SqlListarAlunos + SqlCondicoesFiltros;
            string filtroSituacao = string.IsNullOrWhiteSpace(situacao) ? string.Empty : situacao.Trim();
            string textoBusca = string.IsNullOrWhiteSpace(busca) ? string.Empty : busca.Trim();

            return ConsultarLista(sql, comando =>
            {
                comando.Parameters.AddWithValue("@idTurma", Math.Max(0, idTurma));
                comando.Parameters.AddWithValue("@idProfessor", Math.Max(0, idProfessor));
                comando.Parameters.AddWithValue("@situacao", filtroSituacao);
                comando.Parameters.AddWithValue("@busca", textoBusca);
            }, MapearAluno);
        }

        // Obtém as turmas vinculadas às matrículas do aluno
        public List<Stingray.DadosTela.TurmaAlunoLinha> ListarTurmasMatricula(int idMatricula)
        {
            if (idMatricula <= 0)
                return new List<Stingray.DadosTela.TurmaAlunoLinha>();

            const string sql = @"
                SELECT DISTINCT t.id_turma, t.modalidade, t.dia_semana, t.horario_inicio,
                       t.horario_fim, t.valor_mensal, p.nome AS nome_professor
                FROM turma_matricula tm
                INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                INNER JOIN turma t ON t.id_turma = tm.id_turma
                LEFT JOIN professor pr ON pr.id_professor = t.id_professor
                LEFT JOIN pessoa p ON p.id_pessoa = pr.id_professor
                WHERE tm.id_matricula = @matricula AND m.situacao IN ('Ativo', 'Inativo')
                ORDER BY t.dia_semana, t.horario_inicio, t.modalidade;";

            return ConsultarLista(sql,
                comando => comando.Parameters.AddWithValue("@matricula", idMatricula),
                leitor => new Stingray.DadosTela.TurmaAlunoLinha
                {
                    IdTurma = LeitorMySql.Inteiro(leitor, "id_turma"),
                    Modalidade = LeitorMySql.Texto(leitor, "modalidade"),
                    DiaSemana = LeitorMySql.Texto(leitor, "dia_semana"),
                    HorarioInicio = LeitorMySql.Hora(leitor, "horario_inicio"),
                    HorarioFim = LeitorMySql.Hora(leitor, "horario_fim"),
                    NomeProfessor = LeitorMySql.Texto(leitor, "nome_professor"),
                    ValorMensal = LeitorMySql.Decimal(leitor, "valor_mensal")
                });
        }

        // Grava um novo aluno e seus vínculos iniciais
        public int Inserir(Aluno aluno) // Caso falhe, nenhuma parte do cadastro é mantida
        {
            if (aluno == null) // Verifica se o aluno não é nulo
                throw new ArgumentNullException(nameof(aluno));
            ValidarDiaVencimento(aluno.DiaVencimento);

            return ExecutarTransacao<int>(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int idEndereco = pessoaRepositorio.InserirEndereco(conexao, transacao, aluno.Endereco);
                int idAluno = pessoaRepositorio.InserirPessoa(conexao, transacao, aluno.Pessoa, idEndereco);

                InserirRegistroAluno(conexao, transacao, idAluno, aluno.Observacao);
                InserirMatriculaInicial(conexao, transacao, idAluno, aluno.DiaVencimento);
                pessoaRepositorio.SincronizarTelefone(conexao, transacao, idAluno, aluno.Telefone);

                return idAluno;
            });
        }

        // Atualiza os dados e os vínculos do aluno existente
        public void Atualizar(Aluno aluno)
        {
            if (aluno == null) // Verifica se o aluno não é nulo
                throw new ArgumentNullException(nameof(aluno));
            ValidarDiaVencimento(aluno.DiaVencimento);

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                // Verifica se o aluno existe no banco de dados antes de atualizar
                if (!AlunoExiste(conexao, transacao, aluno.IdAluno))
                    throw new InvalidOperationException("O aluno não foi encontrado no banco de dados.");

                int idEndereco = pessoaRepositorio.ObterIdEndereco(conexao, transacao, aluno.IdAluno);
                pessoaRepositorio.AtualizarEndereco(conexao, transacao, aluno.Endereco, idEndereco);
                pessoaRepositorio.AtualizarPessoa(conexao, transacao, aluno.Pessoa, aluno.IdAluno);
                AtualizarObservacao(conexao, transacao, aluno.IdAluno, aluno.Observacao);
                AtualizarMatriculaAtiva(conexao, transacao, aluno);
                pessoaRepositorio.SincronizarTelefone(conexao, transacao, aluno.IdAluno, aluno.Telefone);
            });
        }

        // Exclui o aluno e os dados relacionados (responsável, frequência, mensalidade, pessoa)
        public void Excluir(int idAluno)
        {
            if (idAluno <= 0) // Valida o ID do aluno
                throw new ArgumentException("Informe um aluno válido.", nameof(idAluno));

            // Delete em cascata
            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int idEndereco = pessoaRepositorio.ObterIdEndereco(conexao, transacao, idAluno);

                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM aluno_responsavel WHERE id_aluno = @id OR id_responsavel = @id;", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE f FROM frequencia f INNER JOIN matricula m ON m.id_matricula = f.id_matricula WHERE m.id_aluno = @id;", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM reposicao WHERE id_matricula IN (SELECT id_matricula FROM matricula WHERE id_aluno = @id);", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM mensalidade WHERE id_matricula IN (SELECT id_matricula FROM matricula WHERE id_aluno = @id);", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM turma_matricula WHERE id_matricula IN (SELECT id_matricula FROM matricula WHERE id_aluno = @id);", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM matricula WHERE id_aluno = @id;", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM responsavel WHERE id_responsavel = @id;", idAluno);
                ExecutarExclusaoAluno(conexao, transacao,
                    "DELETE FROM aluno WHERE id_aluno = @id;", idAluno);
                pessoaRepositorio.ExcluirTelefones(conexao, transacao, idAluno);
                pessoaRepositorio.ExcluirPessoa(conexao, transacao, idAluno);
                pessoaRepositorio.ExcluirEndereco(conexao, transacao, idEndereco);
            });
        }

        // Recebe string e ID do aluno, executando o comando SQL
        private static void ExecutarExclusaoAluno(
            MySqlConnection conexao, MySqlTransaction transacao, string sql, int idAluno)
        {
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Adiciona o parâmetro do ID do aluno à consulta SQL
                comando.Parameters.AddWithValue("@id", idAluno);

                comando.ExecuteNonQuery();
            }
        }

        // Garante que o dia de vencimento esteja dentro do intervalo permitido
        private static void ValidarDiaVencimento(byte diaVencimento)
        {
            if (diaVencimento < 1 || diaVencimento > 28)
                throw new ArgumentOutOfRangeException(nameof(diaVencimento), "O dia de vencimento deve estar entre 1 e 28.");
        }

        // Converte os dados da consulta para a entidade Aluno
        private static Aluno MapearAluno(MySqlDataReader leitor)
        {
            // LeitorMySql é um leitor de dados do MySQL
            // que permite acessar os resultados de uma consulta SQL
            Aluno aluno = new Aluno
            {
                IdAluno = LeitorMySql.Inteiro(leitor, "id_aluno"),
                Observacao = LeitorMySql.Texto(leitor, "observacao"),
                DataMatricula = LeitorMySql.Data(leitor, "data_matricula"),
                DiaVencimento = LeitorMySql.Byte(leitor, "dia_vencimento", 10),
                Situacao = LeitorMySql.Texto(leitor, "situacao", "Sem matrícula"),
                InativoAutomatico = LeitorMySql.Inteiro(leitor, "inativo_automatico") == 1,
                IdMatricula = LeitorMySql.Inteiro(leitor, "id_matricula"),
                Modalidades = LeitorMySql.Texto(leitor, "modalidades", string.Empty),
                Professores = LeitorMySql.Texto(leitor, "professores", string.Empty),
                PrimeiraTurma = LeitorMySql.Texto(leitor, "primeira_turma", string.Empty),
                QuantidadeTurmas = LeitorMySql.Inteiro(leitor, "quantidade_turmas"),
                ValorPlano = LeitorMySql.Decimal(leitor, "valor_plano")
            };

            aluno.Pessoa = new Pessoa
            {
                IdPessoa = aluno.IdAluno,
                IdEndereco = LeitorMySql.Inteiro(leitor, "id_endereco"),
                Nome = LeitorMySql.Texto(leitor, "nome"),
                DataNascimento = LeitorMySql.Data(leitor, "data_nascimento"),
                Cpf = LeitorMySql.Texto(leitor, "cpf"),
                Rg = LeitorMySql.Texto(leitor, "rg"),
                Email = LeitorMySql.Texto(leitor, "email")
            };

            aluno.Endereco = new Endereco
            {
                IdEndereco = aluno.Pessoa.IdEndereco,
                Numero = LeitorMySql.Texto(leitor, "numero"),
                Cep = LeitorMySql.Texto(leitor, "cep"),
                Complemento = LeitorMySql.Texto(leitor, "complemento")
            };

            aluno.Telefone = new Telefone
            {
                IdPessoa = aluno.IdAluno,
                NumeroTelefone = LeitorMySql.Texto(leitor, "telefone", string.Empty)
            };

            return aluno;
        }

        private static bool AlunoExiste(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idAluno)
        {
            // Consulta SQL para verificar se o aluno existe no banco de dados
            const string sql = "SELECT EXISTS(SELECT 1 FROM aluno WHERE id_aluno = @idAluno);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substitui a palavra chave pelo ID do aluno no comando
                comando.Parameters.AddWithValue("@idAluno", idAluno);

                //Executa o comando SQL e retorna true se encontrar o aluno
                return Convert.ToInt32(comando.ExecuteScalar()) == 1;
            }
        }

        private static void InserirRegistroAluno(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idAluno,
            string observacao)
        {
            const string sql = @"
                INSERT INTO aluno (id_aluno, observacao)
                VALUES (@idAluno, @observacao);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substitui as palavras chave pelos valores do aluno no comando
                comando.Parameters.AddWithValue("@idAluno", idAluno);
                comando.Parameters.AddWithValue("@observacao", ValorOuNulo(observacao));

                comando.ExecuteNonQuery();
            }
        }

        private static void AtualizarObservacao(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idAluno,
            string observacao)
        {
            // Atualiza a observação do aluno no banco de dados
            const string sql = @"
                UPDATE aluno
                SET observacao = @observacao
                WHERE id_aluno = @idAluno;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substitui as palavras chave pelos valores do aluno no comando
                comando.Parameters.AddWithValue("@observacao", ValorOuNulo(observacao));
                comando.Parameters.AddWithValue("@idAluno", idAluno);

                comando.ExecuteNonQuery();
            }
        }

        // Cria a matrícula inicial associada ao aluno
        private static void InserirMatriculaInicial(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idAluno,
            byte diaVencimento)
        {
            // Insere a matrícula do aluno com a data de hoje e situação "Ativo"
            const string sql = @"
                INSERT INTO matricula (data_matricula, dia_vencimento, situacao, id_aluno)
                VALUES (@data, @dia, 'Ativo', @aluno);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substitui as palavras chave pelos valores do aluno no comando
                comando.Parameters.AddWithValue("@data", DateTime.Today);
                comando.Parameters.AddWithValue("@dia", diaVencimento);
                comando.Parameters.AddWithValue("@aluno", idAluno);

                comando.ExecuteNonQuery();
            }
        }

        // Atualiza o estado da matrícula e suas associações
        private static void AtualizarMatriculaAtiva(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            Aluno aluno)
        {
            // Consulta SQL para buscar a matrícula do aluno
            const string sqlBuscar = @"
                SELECT id_matricula FROM matricula
                WHERE id_aluno = @idAluno AND situacao <> 'Cancelado'
                ORDER BY id_matricula DESC LIMIT 1 FOR UPDATE;";

            object idMatricula;
            using (MySqlCommand comando = new MySqlCommand(sqlBuscar, conexao, transacao))
            {
                // Substitui a palavra chave pelo ID do aluno no comando
                comando.Parameters.AddWithValue("@idAluno", aluno.IdAluno);

                // Executa o comando SQL e retorna o ID da matrícula
                idMatricula = comando.ExecuteScalar();
            }

            // Verifica se encontrou uma matrícula para o aluno
            if (idMatricula == null || idMatricula == DBNull.Value)
            {
                InserirMatriculaInicial(conexao, transacao, aluno.IdAluno, aluno.DiaVencimento);
                return;
            }


            // Atualiza o dia de vencimento da matrícula do aluno
            const string sqlAtualizar = @"
                UPDATE matricula
                SET dia_vencimento = @dia
                WHERE id_matricula = @idMatricula;";

            using (MySqlCommand comando = new MySqlCommand(sqlAtualizar, conexao, transacao))
            {
                // Substitui as palavras chave pelos valores do aluno no comando
                comando.Parameters.AddWithValue("@dia", aluno.DiaVencimento);
                comando.Parameters.AddWithValue("@idMatricula", Convert.ToInt32(idMatricula));

                comando.ExecuteNonQuery();
            }
        }
    }
}
