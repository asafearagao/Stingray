using System;
using System.Collections.Generic;  // Listas
using System.Linq;                 // Ordenação
using MySqlConnector;              // Conexão com o MySQL
using Stingray.BancoDeDados;
using Stingray.Entidades;
using Stingray.DadosTela;

namespace Stingray.Repositorios
{

    // Gerencia turmas, participantes e vínculos de matrícula no banco
    public class TurmaRepositorio : RepositorioMySqlBase
    {

        public List<Turma> ListarTodos()
        {
            // Consulta SQL para obter todos os dados das turmas
            const string sql = @"
                SELECT t.id_turma, t.modalidade, t.dia_semana, t.horario_inicio, t.horario_fim,
                       t.capacidade, t.valor_mensal, t.id_professor, p.nome AS nome_professor,
                       (SELECT COUNT(DISTINCT m.id_aluno) FROM turma_matricula tm
                        INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                        WHERE tm.id_turma = t.id_turma AND m.situacao = 'Ativo') AS quantidade_participantes
                FROM turma t
                INNER JOIN professor pr ON pr.id_professor = t.id_professor
                INNER JOIN pessoa p ON p.id_pessoa = pr.id_professor
                ORDER BY t.dia_semana, t.horario_inicio, t.modalidade;";

            return ConsultarLista(sql, null, MapearTurma);
        }

        public int Inserir(Turma turma)
        {
            // Insere uma nova turma
            const string sql = @"
                INSERT INTO turma
                    (modalidade, dia_semana, horario_inicio, horario_fim, capacidade, valor_mensal, id_professor)
                VALUES
                    (@modalidade, @dia, @inicio, @fim, @capacidade, @valor, @professor);";

            // Abre a conexão, cria o comando e adiciona os parâmetros
            using (MySqlConnection conexao = Conexao.Abrir())
            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                AdicionarParametros(comando, turma);
                comando.ExecuteNonQuery();
                return Convert.ToInt32(comando.LastInsertedId);
            }
        }

        public void Atualizar(Turma turma)
        {
            // Atualiza os dados da turma
            const string sql = @"
                UPDATE turma
                SET modalidade = @modalidade, dia_semana = @dia, horario_inicio = @inicio,
                    horario_fim = @fim, capacidade = @capacidade, valor_mensal = @valor, id_professor = @professor
                WHERE id_turma = @id;";

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                if (!TurmaExiste(conexao, transacao, turma.IdTurma))
                    throw new InvalidOperationException("A turma não foi encontrada no banco de dados.");

                int participantesAtivos = ContarParticipantesAtivos(conexao, transacao, turma.IdTurma);
                if (participantesAtivos > turma.Capacidade)
                {
                    throw new InvalidOperationException(
                        "A nova capacidade é menor que a quantidade atual de participantes ativos.");
                }

                using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
                {
                    // Adiciona os parâmetros da turma ao comando
                    AdicionarParametros(comando, turma);
                    comando.Parameters.AddWithValue("@id", turma.IdTurma);

                    comando.ExecuteNonQuery(); // Executa o comando SQL
                }
            });
        }

        public void Excluir(int idTurma)
        {
            if (idTurma <= 0) // Valida o ID da turma
                throw new ArgumentException("Informe uma turma válida.", nameof(idTurma));

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                if (!TurmaExiste(conexao, transacao, idTurma))
                    throw new InvalidOperationException("A turma não foi encontrada no banco de dados.");

                ExcluirDependenciasTurma(conexao, transacao, idTurma);
                ExecutarComandoTransacao(conexao, transacao,
                    "DELETE FROM turma WHERE id_turma = @idTurma;", "@idTurma", idTurma);
            });
        }

        internal static void ExcluirDependenciasTurma(
            MySqlConnection conexao, MySqlTransaction transacao, int idTurma)
        {
            const string sqlFrequenciasRelacionadas = @"
                DELETE f FROM frequencia f
                WHERE EXISTS (
                    SELECT 1
                    FROM reposicao r
                    LEFT JOIN ocorrencia_aula origem ON origem.id_ocorrencia = r.id_ocorrencia
                    LEFT JOIN ocorrencia_aula destino ON destino.id_ocorrencia = r.id_ocorrencia_destino
                    WHERE r.id_reposicao = f.id_reposicao
                      AND (origem.id_turma = @idTurma OR destino.id_turma = @idTurma)
                );";
            ExecutarComandoTransacao(conexao, transacao, sqlFrequenciasRelacionadas, "@idTurma", idTurma);

            ExecutarComandoTransacao(conexao, transacao, @"
                DELETE f FROM frequencia f
                INNER JOIN ocorrencia_aula o ON o.id_ocorrencia = f.id_ocorrencia
                WHERE o.id_turma = @idTurma;", "@idTurma", idTurma);

            ExecutarComandoTransacao(conexao, transacao, @"
                DELETE r FROM reposicao r
                LEFT JOIN ocorrencia_aula origem ON origem.id_ocorrencia = r.id_ocorrencia
                LEFT JOIN ocorrencia_aula destino ON destino.id_ocorrencia = r.id_ocorrencia_destino
                WHERE origem.id_turma = @idTurma OR destino.id_turma = @idTurma;", "@idTurma", idTurma);

            ExecutarComandoTransacao(conexao, transacao,
                "DELETE FROM remarcacao_aula WHERE id_turma = @idTurma;", "@idTurma", idTurma);
            ExecutarComandoTransacao(conexao, transacao,
                "DELETE FROM ocorrencia_aula WHERE id_turma = @idTurma;", "@idTurma", idTurma);
            ExecutarComandoTransacao(conexao, transacao,
                "DELETE FROM turma_matricula WHERE id_turma = @idTurma;", "@idTurma", idTurma);
        }

        private static void ExecutarComandoTransacao(
            MySqlConnection conexao, MySqlTransaction transacao, string sql, string parametro, int valor)
        {
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue(parametro, valor);
                comando.ExecuteNonQuery();
            }
        }

        public List<ParticipanteTurma> ListarParticipantes(int idTurma)
        {
            // Listar os participantes da turma
            const string sql = @"
                SELECT m.id_matricula, a.id_aluno, p.nome,
                       CASE WHEN tm.id_matricula IS NULL THEN 0 ELSE 1 END AS selecionado
                FROM aluno a
                INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                INNER JOIN matricula m ON m.id_matricula = (
                    SELECT MAX(m2.id_matricula) FROM matricula m2
                    WHERE m2.id_aluno = a.id_aluno AND m2.situacao = 'Ativo'
                )
                LEFT JOIN turma_matricula tm ON tm.id_matricula = m.id_matricula AND tm.id_turma = @idTurma
                ORDER BY p.nome;";

            return ConsultarLista(sql,
                comando => comando.Parameters.AddWithValue("@idTurma", idTurma),
                leitor => new ParticipanteTurma
                {
                    IdMatricula = LeitorMySql.Inteiro(leitor, "id_matricula"),
                    IdAluno = LeitorMySql.Inteiro(leitor, "id_aluno"),
                    Nome = LeitorMySql.Texto(leitor, "nome"),
                    Selecionado = LeitorMySql.Inteiro(leitor, "selecionado") == 1
                });
        }

        public void AtualizarParticipantes(int idTurma, IList<int> idsMatricula)
        {
            if (idsMatricula == null) // Valida a lista de IDs de matrícula
                throw new ArgumentNullException(nameof(idsMatricula));

            List<int> idsUnicos = idsMatricula.Distinct().ToList();
            if (idsUnicos.Count != idsMatricula.Count)
                throw new ArgumentException("A lista contém matrículas repetidas.", nameof(idsMatricula));

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int capacidade = ObterCapacidade(conexao, transacao, idTurma);

                if (idsUnicos.Count > capacidade)
                    throw new InvalidOperationException("A quantidade de participantes ultrapassa a capacidade da turma.");

                ValidarMatriculasAtivas(conexao, transacao, idsUnicos);
                RemoverVinculosAtuais(conexao, transacao, idTurma);
                InserirVinculos(conexao, transacao, idTurma, idsUnicos);
            });
        }

        private static Turma MapearTurma(MySqlDataReader leitor)
        {
            return new Turma
            {
                IdTurma = LeitorMySql.Inteiro(leitor, "id_turma"),
                Modalidade = LeitorMySql.Texto(leitor, "modalidade"),
                DiaSemana = LeitorMySql.Texto(leitor, "dia_semana"),
                HorarioInicio = LeitorMySql.Hora(leitor, "horario_inicio"),
                HorarioFim = LeitorMySql.Hora(leitor, "horario_fim"),
                Capacidade = LeitorMySql.Inteiro(leitor, "capacidade"),
                ValorMensal = LeitorMySql.Decimal(leitor, "valor_mensal"),
                IdProfessor = LeitorMySql.Inteiro(leitor, "id_professor"),
                NomeProfessor = LeitorMySql.Texto(leitor, "nome_professor"),
                QuantidadeParticipantes = LeitorMySql.Inteiro(leitor, "quantidade_participantes")
            };
        }

        private static void AdicionarParametros(MySqlCommand comando, Turma turma)
        {
            comando.Parameters.AddWithValue("@modalidade", turma.Modalidade.Trim());
            comando.Parameters.AddWithValue("@dia", turma.DiaSemana.Trim());
            comando.Parameters.Add("@inicio", MySqlDbType.Time).Value = turma.HorarioInicio;
            comando.Parameters.Add("@fim", MySqlDbType.Time).Value = turma.HorarioFim;
            comando.Parameters.AddWithValue("@capacidade", turma.Capacidade);
            comando.Parameters.Add("@valor", MySqlDbType.Decimal).Value = turma.ValorMensal;
            comando.Parameters.AddWithValue("@professor", turma.IdProfessor);
        }

        private static bool TurmaExiste(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idTurma)
        {
            // Verificar se a turma existe no banco de dados
            const string sql = "SELECT id_turma FROM turma WHERE id_turma = @idTurma FOR UPDATE;";
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idTurma", idTurma);

                object resultado = comando.ExecuteScalar();
                return resultado != null && resultado != DBNull.Value;
            }
        }


        private static int ContarParticipantesAtivos(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idTurma)
        {
            // Conta os participantes ativos da turma
            const string sql = @"
                SELECT COUNT(DISTINCT m.id_aluno)
                FROM turma_matricula tm
                INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                WHERE tm.id_turma = @idTurma
                  AND m.situacao = 'Ativo';";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idTurma", idTurma);

                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        private static int ObterCapacidade(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idTurma)
        {
            // Consulta SQL para obter a capacidade da turma
            const string sql = "SELECT capacidade FROM turma WHERE id_turma = @idTurma FOR UPDATE;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idTurma", idTurma);

                // Executa a consulta e obtém o resultado
                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                    throw new InvalidOperationException("A turma não foi encontrada no banco de dados.");

                return Convert.ToInt32(resultado);
            }
        }

        private static void ValidarMatriculasAtivas(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            IList<int> idsMatricula)
        {
            if (idsMatricula.Count == 0) // Lista vazia não valida
                return;

            List<string> parametros = new List<string>();
            using (MySqlCommand comando = new MySqlCommand())
            {
                comando.Connection = conexao;
                comando.Transaction = transacao;

                for (int i = 0; i < idsMatricula.Count; i++)
                {
                    string nomeParametro = "@matricula" + i;
                    parametros.Add(nomeParametro);
                    comando.Parameters.AddWithValue(nomeParametro, idsMatricula[i]);
                }

                //  Verifica matriculas ativas
                comando.CommandText = @"
                    SELECT id_matricula
                    FROM matricula
                    WHERE situacao = 'Ativo'
                      AND id_matricula IN (" + string.Join(", ", parametros) + @")
                    FOR UPDATE;";

                int encontradas = 0;
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                        encontradas++;
                }

                if (encontradas != idsMatricula.Count)
                {
                    throw new InvalidOperationException(
                        "Um ou mais alunos selecionados não possuem matrícula ativa. Atualize a lista e tente novamente.");
                }
            }
        }

        private static void RemoverVinculosAtuais(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idTurma)
        {
            // Remove os vínculos atuais da turma com as matrículas
            const string sql = "DELETE FROM turma_matricula WHERE id_turma = @idTurma;";
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idTurma", idTurma);

                comando.ExecuteNonQuery(); // Executa o comando SQL
            }
        }

        private static void InserirVinculos(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idTurma,
            IList<int> idsMatricula)
        {
            // Insere os novos vínculos da turma com as matrículas
            const string sql = @"
                INSERT INTO turma_matricula (id_turma, id_matricula)
                VALUES (@idTurma, @idMatricula);";

            foreach (int idMatricula in idsMatricula)
            {
                using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
                {
                    comando.Parameters.AddWithValue("@idTurma", idTurma);
                    comando.Parameters.AddWithValue("@idMatricula", idMatricula);

                    comando.ExecuteNonQuery(); // Executa o comando SQL
                }
            }
        }
    }
}
