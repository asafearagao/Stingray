using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using Stingray.BancoDeDados;
using Stingray.DadosTela;
using Stingray.Entidades;

namespace Stingray.Repositorios
{

    // Gerencia chamadas, ocorrências, reposições e regras de frequência
    public class FrequenciaRepositorio : RepositorioMySqlBase
    {

        public int ObterOuCriarOcorrencia(int idTurma, DateTime dataAula)
        {
            if (idTurma <= 0) // Verifica se o ID é válido
                throw new ArgumentException("Selecione uma turma válida.", nameof(idTurma));

            return ExecutarTransacao<int>(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                const string sql = @"
                    INSERT INTO ocorrencia_aula (data_aula, situacao, id_turma)
                    VALUES (@data, 'Pendente', @turma)
                    ON DUPLICATE KEY UPDATE id_ocorrencia = LAST_INSERT_ID(id_ocorrencia);";

                using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
                {
                    comando.Parameters.AddWithValue("@data", dataAula.Date);
                    comando.Parameters.AddWithValue("@turma", idTurma);
                    comando.ExecuteNonQuery();
                    return Convert.ToInt32(comando.LastInsertedId);
                }
            });
        }

        public bool EstaRemarcadaNaOrigem(int idTurma, DateTime data)
        {
            const string sql = @"SELECT COUNT(*) FROM remarcacao_aula WHERE id_turma = @turma AND data_origem = @data;";
            object valor = ConsultarValor(sql, comando =>
            {
                // Substituir palavra chave por variável
                comando.Parameters.AddWithValue("@turma", idTurma);
                comando.Parameters.AddWithValue("@data", data.Date);
            });
            return Convert.ToInt32(valor) > 0;
        }

        public bool EhDestinoRemarcacao(int idTurma, DateTime data)
        {
            const string sql = @"SELECT COUNT(*) FROM remarcacao_aula WHERE id_turma = @turma AND data_destino = @data;";
            object valor = ConsultarValor(sql, comando =>
            {
                // Substituindo palavra chave por variável
                comando.Parameters.AddWithValue("@turma", idTurma);
                comando.Parameters.AddWithValue("@data", data.Date);
            });
            return Convert.ToInt32(valor) > 0;
        }

        public List<int> ListarTurmasRemarcadasParaData(DateTime data)
        {
            const string sql = @"SELECT id_turma FROM remarcacao_aula WHERE data_destino = @data ORDER BY id_turma;";
            return ConsultarLista(sql,
                comando => comando.Parameters.AddWithValue("@data", data.Date),
                leitor => LeitorMySql.Inteiro(leitor, "id_turma"));
        }

        // Move a ocorrência para a nova data mantendo o vínculo com a origem
        public void RemarcarAula(int idTurma, DateTime dataOrigem, DateTime dataDestino)
        {
            if (idTurma <= 0) // Validar ID
                throw new ArgumentException("Selecione uma turma válida.", nameof(idTurma));

            // Remarcar para datas válidas
            if (dataOrigem.Date == dataDestino.Date)
                throw new InvalidOperationException("Escolha uma data diferente da aula original.");
            if (dataDestino.Date <= DateTime.Today)
                throw new InvalidOperationException("A remarcação deve ser feita para uma data futura.");

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                const string verificarSql = @"
                    SELECT COUNT(*) FROM frequencia f
                    INNER JOIN ocorrencia_aula o ON o.id_ocorrencia = f.id_ocorrencia
                        WHERE o.id_turma = @turma AND o.data_aula = @origem;";

                using (MySqlCommand verificar = new MySqlCommand(verificarSql, conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    verificar.Parameters.AddWithValue("@turma", idTurma);
                    verificar.Parameters.AddWithValue("@origem", dataOrigem.Date);

                    // Executa comando SQL e vê se aula foi dada
                    if (Convert.ToInt32(verificar.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("Esta aula já possui registros de frequência e não pode ser remarcada.");
                }

                const string conflitoSql = @"
                    SELECT COUNT(*) FROM remarcacao_aula
                    WHERE id_turma = @turma AND (data_origem = @origem OR data_destino = @destino);";

                using (MySqlCommand conflito = new MySqlCommand(conflitoSql, conexao, transacao))
                {
                    // Susbtituir palavra chave por variável
                    conflito.Parameters.AddWithValue("@turma", idTurma);
                    conflito.Parameters.AddWithValue("@origem", dataOrigem.Date);
                    conflito.Parameters.AddWithValue("@destino", dataDestino.Date);

                    // Executa comando SQL e vê se deu conflito com outras turmas
                    if (Convert.ToInt32(conflito.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("A aula de origem ou a data de destino já participa de outra remarcação.");
                }

                int idOrigem = ObterOuCriarOcorrenciaNaTransacao(conexao, transacao, idTurma, dataOrigem.Date);
                int idDestino = ObterOuCriarOcorrenciaNaTransacao(conexao, transacao, idTurma, dataDestino.Date);

                const string validarDestinoSql = "SELECT COUNT(*) FROM frequencia WHERE id_ocorrencia = @ocorrencia;";

                using (MySqlCommand validarDestino = new MySqlCommand(validarDestinoSql, conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    validarDestino.Parameters.AddWithValue("@ocorrencia", idDestino);

                    // Executa comando SQL e verifica se a aula foi dada
                    if (Convert.ToInt32(validarDestino.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("A data escolhida já possui registros de frequência para esta turma.");
                }

                const string inserirSql = @"
                    INSERT INTO remarcacao_aula (id_turma, data_origem, data_destino, data_geracao)
                    VALUES (@turma, @origem, @destino, @geracao);";

                using (MySqlCommand inserir = new MySqlCommand(inserirSql, conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    inserir.Parameters.AddWithValue("@turma", idTurma);
                    inserir.Parameters.AddWithValue("@origem", dataOrigem.Date);
                    inserir.Parameters.AddWithValue("@destino", dataDestino.Date);
                    inserir.Parameters.AddWithValue("@geracao", DateTime.Now);

                    inserir.ExecuteNonQuery(); // Executar comando SQL
                }

                using (MySqlCommand cancelarOrigem = new MySqlCommand(
                    "UPDATE ocorrencia_aula SET situacao = 'Cancelada' WHERE id_ocorrencia = @id;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    cancelarOrigem.Parameters.AddWithValue("@id", idOrigem);

                    cancelarOrigem.ExecuteNonQuery(); // Executar comando SQL
                }
            });
        }

        // Localiza ou cria a ocorrência dentro da transação atual
        private static int ObterOuCriarOcorrenciaNaTransacao(
            MySqlConnection conexao, MySqlTransaction transacao, int idTurma, DateTime data)
        {
            const string sql = @"                INSERT INTO ocorrencia_aula (data_aula, situacao, id_turma)                VALUES (@data, 'Pendente', @turma)                ON DUPLICATE KEY UPDATE id_ocorrencia = LAST_INSERT_ID(id_ocorrencia);";
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substituir palavra chave por variável
                comando.Parameters.AddWithValue("@data", data.Date);
                comando.Parameters.AddWithValue("@turma", idTurma);

                comando.ExecuteNonQuery(); // Executar comando SQL

                return Convert.ToInt32(comando.LastInsertedId);
            }
        }

        // Busca os alunos que devem aparecer na chamada
        public List<ParticipantePresenca> ListarParticipantes(int idTurma, int idOcorrencia)
        {
            const string sql = @"
                SELECT participantes.id_matricula, participantes.id_aluno, participantes.nome,
                       COALESCE(f.situacao, '') AS situacao,
                       COALESCE(f.observacao, '') AS observacao,
                       COALESCE(r_destino.id_reposicao, r_origem.id_reposicao, 0) AS id_reposicao,
                       CASE WHEN r_destino.id_reposicao IS NOT NULL
                                  AND r_destino.id_ocorrencia <> @ocorrencia THEN 1 ELSE 0 END AS eh_reposicao,
                       CASE WHEN r_origem.situacao = 'Pendente' AND r_origem.id_ocorrencia_destino IS NOT NULL
                            THEN 1 ELSE 0 END AS possui_reposicao_agendada,
                       COALESCE(r_destino.situacao, r_origem.situacao, '') AS situacao_reposicao
                FROM (
                    SELECT m.id_matricula, m.id_aluno, p.nome
                    FROM turma_matricula tm
                    INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                    INNER JOIN aluno a ON a.id_aluno = m.id_aluno
                    INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                    WHERE tm.id_turma = @turma
                      AND (m.situacao = 'Ativo' OR EXISTS (
                          SELECT 1 FROM frequencia historico
                          WHERE historico.id_matricula = m.id_matricula
                            AND historico.id_ocorrencia = @ocorrencia
                      ))

                    UNION

                    SELECT historico.id_matricula, m.id_aluno, p.nome
                    FROM frequencia historico
                    INNER JOIN matricula m ON m.id_matricula = historico.id_matricula
                    INNER JOIN aluno a ON a.id_aluno = m.id_aluno
                    INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                    WHERE historico.id_ocorrencia = @ocorrencia

                    UNION

                    SELECT r.id_matricula, m.id_aluno, p.nome
                    FROM reposicao r
                    INNER JOIN matricula m ON m.id_matricula = r.id_matricula
                    INNER JOIN aluno a ON a.id_aluno = m.id_aluno
                    INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                    WHERE r.id_ocorrencia_destino = @ocorrencia
                      AND r.situacao = 'Pendente'
                      AND r.id_matricula IS NOT NULL
                ) participantes
                LEFT JOIN frequencia f
                    ON f.id_matricula = participantes.id_matricula
                   AND f.id_ocorrencia = @ocorrencia
                LEFT JOIN reposicao r_destino
                    ON r_destino.id_matricula = participantes.id_matricula
                   AND (
                       r_destino.id_ocorrencia_destino = @ocorrencia
                       OR (f.id_reposicao = r_destino.id_reposicao
                           AND r_destino.id_ocorrencia <> @ocorrencia)
                   )
                   AND r_destino.situacao IN ('Pendente', 'Concluida')
                LEFT JOIN reposicao r_origem
                    ON r_origem.id_reposicao = (
                        SELECT MAX(r_historico.id_reposicao)
                        FROM reposicao r_historico
                        WHERE r_historico.id_matricula = participantes.id_matricula
                          AND r_historico.id_ocorrencia = @ocorrencia
                    )
                ORDER BY participantes.nome;";

            return ConsultarLista(sql, comando =>
            {
                // Substituir palavra chave por variável
                comando.Parameters.AddWithValue("@turma", idTurma);
                comando.Parameters.AddWithValue("@ocorrencia", idOcorrencia);
            }, leitor => new ParticipantePresenca
            {
                // variável = função.tipo(leitor, "nome da coluna", valor padrão)
                IdMatricula = LeitorMySql.Inteiro(leitor, "id_matricula"),
                IdAluno = LeitorMySql.Inteiro(leitor, "id_aluno"),
                IdOcorrencia = idOcorrencia,
                Nome = LeitorMySql.Texto(leitor, "nome"),
                Situacao = LeitorMySql.Texto(leitor, "situacao"),
                Observacao = LeitorMySql.Texto(leitor, "observacao"),
                IdReposicao = LeitorMySql.Inteiro(leitor, "id_reposicao"),
                EhReposicao = LeitorMySql.Inteiro(leitor, "eh_reposicao") == 1,
                PossuiReposicaoAgendada = LeitorMySql.Inteiro(leitor, "possui_reposicao_agendada") == 1,
                SituacaoReposicao = LeitorMySql.Texto(leitor, "situacao_reposicao")
            });
        }

        // Persiste as situações de todos os participantes da aula
        public void SalvarChamada(int idTurma, DateTime dataAula, bool aulaRealizada, IList<ParticipantePresenca> participantes)
        {
            if (participantes == null || participantes.Count == 0)
                throw new InvalidOperationException("Não há alunos para salvar nesta chamada.");

            if (dataAula.Date > DateTime.Today && participantes.Any(item => item.Situacao != "Justificado"))
                throw new InvalidOperationException("Em aulas futuras, somente justificativas podem ser registradas.");

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                int idOcorrencia = ObterOuCriarOcorrencia(conexao, transacao, idTurma, dataAula.Date);
                using (MySqlCommand bloquear = new MySqlCommand(
                    "SELECT situacao FROM ocorrencia_aula WHERE id_ocorrencia = @id FOR UPDATE;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    bloquear.Parameters.AddWithValue("@id", idOcorrencia);
                    string situacaoOcorrencia = Convert.ToString(bloquear.ExecuteScalar());

                    if (situacaoOcorrencia == "Cancelada")
                        throw new InvalidOperationException("Esta aula está cancelada e não pode receber registros de frequência.");
                }
                List<int> matriculasAtualizadas = new List<int>();

                foreach (ParticipantePresenca participante in participantes)
                {
                    ValidarSituacao(participante.Situacao);
                    ValidarParticipacao(conexao, transacao, idTurma, idOcorrencia, participante.IdMatricula);

                    int idReposicao;
                    if (participante.EhReposicao)
                    {
                        idReposicao = participante.IdReposicao;
                    }
                    else if (participante.Situacao == "Justificado")
                    {
                        idReposicao = ObterOuCriarReposicao(conexao, transacao, participante.IdMatricula, idOcorrencia, dataAula.Date);
                    }
                    else
                    {
                        if (ReposicaoJaConcluida(conexao, transacao, participante.IdMatricula, idOcorrencia))
                            throw new InvalidOperationException("A justificativa original deve permanecer registrada porque a reposição já foi concluída.");

                        idReposicao = ObterReposicaoPendente(conexao, transacao, participante.IdMatricula, idOcorrencia);
                    }

                    const string sqlFrequencia = @"
                        INSERT INTO frequencia (id_matricula, id_ocorrencia, situacao, observacao, id_reposicao)
                        VALUES (@matricula, @ocorrencia, @situacao, @observacao, @reposicao)
                        ON DUPLICATE KEY UPDATE
                            situacao = VALUES(situacao),
                            observacao = VALUES(observacao),
                            id_reposicao = VALUES(id_reposicao);";

                    using (MySqlCommand comando = new MySqlCommand(sqlFrequencia, conexao, transacao))
                    {
                        // Substituir palavra chave por variável
                        comando.Parameters.AddWithValue("@matricula", participante.IdMatricula);
                        comando.Parameters.AddWithValue("@ocorrencia", idOcorrencia);
                        comando.Parameters.AddWithValue("@situacao", participante.Situacao);
                        comando.Parameters.AddWithValue("@observacao", ValorOuNulo(participante.Observacao));
                        comando.Parameters.AddWithValue("@reposicao", idReposicao > 0 ? (object)idReposicao : DBNull.Value);

                        comando.ExecuteNonQuery(); // Executa comando SQL
                    }

                    if (participante.EhReposicao && idReposicao > 0)
                    {
                        if (participante.Situacao == "Presente")
                        {
                            using (MySqlCommand concluir = new MySqlCommand(@"
                                UPDATE reposicao
                                SET id_ocorrencia_destino = @ocorrencia,
                                    data_agendamento = @data,
                                    situacao = 'Concluida'
                                WHERE id_reposicao = @id AND situacao IN ('Pendente', 'Concluida');", conexao, transacao))
                            {
                                // Substituir palavra chave por variável
                                concluir.Parameters.AddWithValue("@ocorrencia", idOcorrencia);
                                concluir.Parameters.AddWithValue("@data", dataAula.Date);
                                concluir.Parameters.AddWithValue("@id", idReposicao);

                                concluir.ExecuteNonQuery(); // Executa comando SQL
                            }
                        }
                        else
                        {
                            // A tentativa não foi concluída; a origem continua pendente e pode ser reagendada
                            using (MySqlCommand liberarDestino = new MySqlCommand(@"
                                UPDATE reposicao
                                SET id_ocorrencia_destino = NULL,
                                    situacao = 'Pendente'
                                WHERE id_reposicao = @id AND situacao IN ('Pendente', 'Concluida');", conexao, transacao))
                            {
                                liberarDestino.Parameters.AddWithValue("@id", idReposicao);
                                liberarDestino.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (participante.Situacao != "Justificado" && idReposicao > 0)
                    {
                        using (MySqlCommand comando = new MySqlCommand(@"
                            UPDATE reposicao SET situacao = 'Cancelada'
                            WHERE id_reposicao = @id AND situacao = 'Pendente';", conexao, transacao))
                        {
                            // Substituir palavra chave por variável
                            comando.Parameters.AddWithValue("@id", idReposicao);

                            comando.ExecuteNonQuery(); // Executa comando SQL
                        }
                    }

                    matriculasAtualizadas.Add(participante.IdMatricula);
                }

                using (MySqlCommand comando = new MySqlCommand(@"
                    UPDATE ocorrencia_aula SET situacao = @situacao
                    WHERE id_ocorrencia = @id;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    comando.Parameters.AddWithValue("@situacao", aulaRealizada ? "Concluida" : "Pendente");
                    comando.Parameters.AddWithValue("@id", idOcorrencia);

                    comando.ExecuteNonQuery(); // Executa comando SQL
                }

                foreach (int idMatricula in matriculasAtualizadas.Distinct())
                    RecalcularInatividade(conexao, transacao, idMatricula);
            });
        }

        // Cria o registro de reposição e atualiza as ocorrências relacionadas
        public void AgendarReposicao(int idMatricula, int idOcorrenciaOriginal, int idTurmaDestino, DateTime dataDestino)
        {
            if (idMatricula <= 0 || idOcorrenciaOriginal <= 0 || idTurmaDestino <= 0)
                throw new ArgumentException("Os dados da reposição estão incompletos.");
            if (dataDestino.Date < DateTime.Today)
                throw new ArgumentException("A reposição deve ser agendada para hoje ou uma data futura.");

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                using (MySqlCommand verificar = new MySqlCommand(@"
                    SELECT situacao FROM frequencia
                    WHERE id_matricula = @matricula AND id_ocorrencia = @origem
                    FOR UPDATE;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    verificar.Parameters.AddWithValue("@matricula", idMatricula);
                    verificar.Parameters.AddWithValue("@origem", idOcorrenciaOriginal);

                    if (!string.Equals(Convert.ToString(verificar.ExecuteScalar()), "Justificado", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("A aula original precisa estar salva como justificada antes de agendar a reposição.");
                }

                int idReposicao = ObterReposicaoPendente(conexao, transacao, idMatricula, idOcorrenciaOriginal);
                if (idReposicao <= 0)
                    throw new InvalidOperationException("Não foi encontrada uma reposição pendente para esta justificativa.");

                int idDestino = ObterOuCriarOcorrencia(conexao, transacao, idTurmaDestino, dataDestino.Date);
                if (idDestino == idOcorrenciaOriginal)
                    throw new InvalidOperationException("A reposição deve ocorrer em uma aula diferente da aula original.");

                using (MySqlCommand bloquearDestino = new MySqlCommand(@"
                    SELECT situacao FROM ocorrencia_aula
                    WHERE id_ocorrencia = @destino FOR UPDATE;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    bloquearDestino.Parameters.AddWithValue("@destino", idDestino);

                    // Executa comando SQL
                    string situacaoDestino = Convert.ToString(bloquearDestino.ExecuteScalar());
                    if (situacaoDestino != "Pendente")
                        throw new InvalidOperationException("A reposição precisa ser agendada em uma aula ainda pendente.");
                }

                using (MySqlCommand verificarDestino = new MySqlCommand(@"
                    SELECT COUNT(*) FROM turma_matricula
                    WHERE id_turma = @turma AND id_matricula = @matricula;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    verificarDestino.Parameters.AddWithValue("@turma", idTurmaDestino);
                    verificarDestino.Parameters.AddWithValue("@matricula", idMatricula);

                    if (Convert.ToInt32(verificarDestino.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("O aluno já está matriculado na turma escolhida. Não é necessário registrar essa participação como reposição.");
                }

                using (MySqlCommand capacidade = new MySqlCommand(@"
                    SELECT t.capacidade,
                        (SELECT COUNT(DISTINCT tm.id_matricula)
                         FROM turma_matricula tm
                         INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                         WHERE tm.id_turma = t.id_turma AND m.situacao = 'Ativo')
                        +
                        (SELECT COUNT(DISTINCT r.id_matricula)
                         FROM reposicao r
                         WHERE r.id_ocorrencia_destino = @destino
                           AND r.situacao = 'Pendente'
                           AND r.id_reposicao <> @reposicao
                           AND NOT EXISTS (
                               SELECT 1 FROM turma_matricula tm
                               WHERE tm.id_turma = t.id_turma AND tm.id_matricula = r.id_matricula
                           )) AS participantes
                    FROM turma t WHERE t.id_turma = @turma;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    capacidade.Parameters.AddWithValue("@destino", idDestino);
                    capacidade.Parameters.AddWithValue("@turma", idTurmaDestino);
                    capacidade.Parameters.AddWithValue("@reposicao", idReposicao);

                    using (MySqlDataReader leitor = capacidade.ExecuteReader())
                    {
                        if (!leitor.Read())
                            throw new InvalidOperationException("A turma de destino não foi encontrada.");
                        int limite = Convert.ToInt32(leitor["capacidade"]);
                        int atuais = Convert.ToInt32(leitor["participantes"]);
                        if (atuais >= limite)
                            throw new InvalidOperationException("A aula de destino já atingiu a capacidade máxima.");
                    }
                }

                using (MySqlCommand duplicada = new MySqlCommand(@"
                    SELECT COUNT(*) FROM reposicao
                    WHERE id_matricula = @matricula
                      AND id_ocorrencia_destino = @destino
                      AND situacao = 'Pendente'
                      AND id_reposicao <> @id;", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    duplicada.Parameters.AddWithValue("@matricula", idMatricula);
                    duplicada.Parameters.AddWithValue("@destino", idDestino);
                    duplicada.Parameters.AddWithValue("@id", idReposicao);

                    if (Convert.ToInt32(duplicada.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("Este aluno já possui outra reposição agendada para essa aula.");
                }

                using (MySqlCommand atualizar = new MySqlCommand(@"
                    UPDATE reposicao
                    SET id_ocorrencia_destino = @destino,
                        data_agendamento = @data,
                        situacao = 'Pendente'
                    WHERE id_reposicao = @id AND situacao = 'Pendente';", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    atualizar.Parameters.AddWithValue("@destino", idDestino);
                    atualizar.Parameters.AddWithValue("@data", dataDestino.Date);
                    atualizar.Parameters.AddWithValue("@id", idReposicao);

                    if (atualizar.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("A reposição não pôde ser agendada. Atualize a chamada e tente novamente.");
                }
            });
        }

        // Atualiza as ocorrências que já foram concluídas
        public void AtualizarOcorrenciasEncerradas()
        {
            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                List<int> ocorrencias = new List<int>();
                const string sqlOcorrencias = @"
                    SELECT o.id_ocorrencia
                    FROM ocorrencia_aula o
                    INNER JOIN turma t ON t.id_turma = o.id_turma
                    WHERE o.situacao = 'Pendente'
                      AND (o.data_aula < CURDATE()
                           OR (o.data_aula = CURDATE() AND t.horario_fim <= CURTIME()))
                      AND EXISTS (
                          SELECT 1 FROM frequencia f
                          WHERE f.id_ocorrencia = o.id_ocorrencia
                            AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                      )
                      AND NOT EXISTS (
                          SELECT 1
                          FROM turma_matricula tm
                          INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                          WHERE tm.id_turma = o.id_turma AND m.situacao = 'Ativo'
                            AND NOT EXISTS (
                                SELECT 1 FROM frequencia f
                                WHERE f.id_matricula = m.id_matricula
                                  AND f.id_ocorrencia = o.id_ocorrencia
                                  AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                            )
                      )
                      AND NOT EXISTS (
                          SELECT 1 FROM reposicao r
                          WHERE r.id_ocorrencia_destino = o.id_ocorrencia
                            AND r.situacao = 'Pendente'
                            AND NOT EXISTS (
                                SELECT 1 FROM frequencia f
                                WHERE f.id_matricula = r.id_matricula
                                  AND f.id_ocorrencia = o.id_ocorrencia
                                  AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                            )
                      )
                    FOR UPDATE;";

                using (MySqlCommand consulta = new MySqlCommand(sqlOcorrencias, conexao, transacao))
                using (MySqlDataReader leitor = consulta.ExecuteReader())
                {
                    while (leitor.Read())
                        ocorrencias.Add(Convert.ToInt32(leitor["id_ocorrencia"]));
                }

                if (ocorrencias.Count == 0)
                    return;

                List<int> matriculas = new List<int>();
                string parametros = string.Join(",", ocorrencias.Select((id, indice) => "@ocorrencia" + indice));
                using (MySqlCommand atualizar = new MySqlCommand(
                    "UPDATE ocorrencia_aula SET situacao = 'Concluida' WHERE id_ocorrencia IN (" + parametros + ");",
                    conexao, transacao))
                {
                    // Substituir palavra chave por variável + i
                    for (int i = 0; i < ocorrencias.Count; i++)
                        atualizar.Parameters.AddWithValue("@ocorrencia" + i, ocorrencias[i]);

                    atualizar.ExecuteNonQuery();
                }

                using (MySqlCommand consulta = new MySqlCommand(
                    "SELECT DISTINCT id_matricula FROM frequencia WHERE id_ocorrencia IN (" + parametros + ");",
                    conexao, transacao))
                {
                    // Substituir palavra chave por variável + i
                    for (int i = 0; i < ocorrencias.Count; i++)
                        consulta.Parameters.AddWithValue("@ocorrencia" + i, ocorrencias[i]);

                    using (MySqlDataReader leitor = consulta.ExecuteReader())
                    {
                        while (leitor.Read())
                            matriculas.Add(Convert.ToInt32(leitor[0]));
                    }
                }

                foreach (int idMatricula in matriculas.Distinct())
                    RecalcularInatividade(conexao, transacao, idMatricula);
            });
        }

        // Calcula os indicadores de frequência do aluno
        public ResumoFrequenciaAluno ObterResumoAluno(int idAluno, int quantidadeAulas)
        {
            const string sql = @"
                SELECT f.situacao
                FROM frequencia f
                INNER JOIN ocorrencia_aula o ON o.id_ocorrencia = f.id_ocorrencia
                INNER JOIN matricula m ON m.id_matricula = f.id_matricula
                WHERE m.id_aluno = @aluno
                  AND o.situacao = 'Concluida'
                  AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                ORDER BY o.data_aula DESC, o.id_ocorrencia DESC
                LIMIT @limite;";

            List<string> situacoes = ConsultarLista(sql, comando =>
            {
                // Substituir palavra chave por variável
                comando.Parameters.AddWithValue("@aluno", idAluno);
                comando.Parameters.AddWithValue("@limite", quantidadeAulas);

            }, leitor => LeitorMySql.Texto(leitor, "situacao"));

            ResumoFrequenciaAluno resumo = new ResumoFrequenciaAluno
            {
                TotalAulas = situacoes.Count,
                Presentes = situacoes.Count(item => item == "Presente"),
                Ausentes = situacoes.Count(item => item == "Ausente"),
                Justificados = situacoes.Count(item => item == "Justificado")
            };
            resumo.PercentualFaltas = resumo.TotalAulas == 0 ? 0 : (decimal)resumo.Ausentes / resumo.TotalAulas * 100M;
            return resumo;
        }

        public ConfiguracaoFrequencia ObterConfiguracao()
        {
            const string sql = @"
                SELECT quantidade_aulas, percentual_faltas
                FROM configuracao_frequencia WHERE id_configuracao = 1;";

            return ConsultarUnico(sql, null, leitor => new ConfiguracaoFrequencia
            {
                QuantidadeAulas = LeitorMySql.Inteiro(leitor, "quantidade_aulas", 10),
                PercentualFaltas = Convert.ToDecimal(leitor["percentual_faltas"])
            }, new ConfiguracaoFrequencia { QuantidadeAulas = 10, PercentualFaltas = 30M });
        }

        public void SalvarConfiguracao(ConfiguracaoFrequencia configuracao)
        {
            if (configuracao == null)
                throw new ArgumentNullException(nameof(configuracao));

            if (configuracao.QuantidadeAulas < 1 || configuracao.QuantidadeAulas > 100)
                throw new ArgumentOutOfRangeException(nameof(configuracao.QuantidadeAulas), "A quantidade deve estar entre 1 e 100 aulas.");

            if (configuracao.PercentualFaltas < 1 || configuracao.PercentualFaltas > 100)
                throw new ArgumentOutOfRangeException(nameof(configuracao.PercentualFaltas), "O percentual deve estar entre 1 e 100.");

            ExecutarTransacao(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                using (MySqlCommand comando = new MySqlCommand(@"
                    INSERT INTO configuracao_frequencia
                        (id_configuracao, quantidade_aulas, percentual_faltas)
                    VALUES (1, @quantidade, @percentual)
                    ON DUPLICATE KEY UPDATE
                        quantidade_aulas = VALUES(quantidade_aulas),
                        percentual_faltas = VALUES(percentual_faltas);", conexao, transacao))
                {
                    // Substituir palavra chave por variável
                    comando.Parameters.AddWithValue("@quantidade", configuracao.QuantidadeAulas);
                    comando.Parameters.AddWithValue("@percentual", configuracao.PercentualFaltas);

                    comando.ExecuteNonQuery(); // Executa comando SQL
                }

                List<int> matriculas = new List<int>();
                using (MySqlCommand comando = new MySqlCommand(
                    "SELECT id_matricula FROM matricula WHERE situacao <> 'Cancelado';", conexao, transacao))
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                        matriculas.Add(Convert.ToInt32(leitor[0]));
                }

                foreach (int idMatricula in matriculas)
                    RecalcularInatividade(conexao, transacao, idMatricula);
            });
        }

        // Conta os alunos que atendem à regra de inatividade
        public int ContarAlunosInativos()
        {
            const string sql = "SELECT COUNT(*) FROM matricula WHERE situacao = 'Inativo' AND inativo_automatico = 1;";

            return Convert.ToInt32(ConsultarValor(sql));
        }

        // Conta as reposições que ainda estão em aberto
        public int ContarReposicoesPendentes()
        {
            const string sql = @"
                SELECT COUNT(*) FROM reposicao r
                INNER JOIN frequencia f ON f.id_matricula = r.id_matricula
                                       AND f.id_ocorrencia = r.id_ocorrencia
                                       AND f.situacao = 'Justificado'
                WHERE r.situacao = 'Pendente'
                  AND r.id_ocorrencia_destino IS NULL;";

            return Convert.ToInt32(ConsultarValor(sql));
        }

        // Lista os alunos classificados como inativos
        public List<AlunoInativoDashboard> ListarAlunosInativos(int limite)
        {
            const string sql = @"
                SELECT m.id_aluno, p.nome,
                       COALESCE(ROUND(SUM(CASE WHEN recentes.situacao = 'Ausente' THEN 1 ELSE 0 END) * 100.0 /
                           NULLIF(COUNT(recentes.situacao), 0), 1), 0) AS percentual_faltas
                FROM matricula m
                INNER JOIN aluno a ON a.id_aluno = m.id_aluno
                INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                CROSS JOIN configuracao_frequencia c
                LEFT JOIN (
                    SELECT f.id_matricula, f.situacao,
                           ROW_NUMBER() OVER (PARTITION BY f.id_matricula
                               ORDER BY o.data_aula DESC, o.id_ocorrencia DESC) AS ordem
                    FROM frequencia f
                    INNER JOIN ocorrencia_aula o ON o.id_ocorrencia = f.id_ocorrencia
                    INNER JOIN turma_matricula tm
                        ON tm.id_matricula = f.id_matricula AND tm.id_turma = o.id_turma
                    WHERE o.situacao = 'Concluida'
                      AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                      AND NOT EXISTS (
                          SELECT 1 FROM reposicao r
                          WHERE r.id_matricula = f.id_matricula
                            AND r.id_ocorrencia_destino = f.id_ocorrencia
                            AND r.situacao = 'Concluida'
                      )
                ) recentes ON recentes.id_matricula = m.id_matricula
                           AND recentes.ordem <= c.quantidade_aulas
                WHERE m.situacao = 'Inativo' AND m.inativo_automatico = 1
                GROUP BY m.id_matricula, m.id_aluno, p.nome
                ORDER BY p.nome LIMIT @limite;";

            return ConsultarLista(sql, comando => comando.Parameters.AddWithValue("@limite", limite), leitor => new AlunoInativoDashboard
            {
                // Variável = função.tipo(leitor, "nome da coluna", valor padrão)
                IdAluno = LeitorMySql.Inteiro(leitor, "id_aluno"),
                Nome = LeitorMySql.Texto(leitor, "nome"),

                PercentualFaltas = Convert.ToDecimal(leitor["percentual_faltas"])
            });
        }

        // Busca as reposições que ainda precisam ser realizadas
        public List<ReposicaoPendenteDashboard> ListarReposicoesPendentes(int limite)
        {
            const string sql = @"
                SELECT m.id_aluno, m.id_matricula, r.id_ocorrencia AS ocorrencia_original,
                       turma_origem.id_turma AS id_turma_origem,
                       destino.id_turma AS id_turma_destino,
                       destino.data_aula AS data_destino,
                       p.nome, turma_origem.modalidade AS modalidade_original,
                       origem.data_aula AS data_original,
                       CASE WHEN destino.id_ocorrencia IS NULL THEN 'A agendar'
                            ELSE CONCAT(turma_destino.modalidade, ' · ', DATE_FORMAT(destino.data_aula, '%d/%m/%Y'))
                       END AS destino
                FROM reposicao r
                INNER JOIN matricula m ON m.id_matricula = r.id_matricula
                INNER JOIN aluno a ON a.id_aluno = m.id_aluno
                INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                INNER JOIN ocorrencia_aula origem ON origem.id_ocorrencia = r.id_ocorrencia
                INNER JOIN turma turma_origem ON turma_origem.id_turma = origem.id_turma
                LEFT JOIN ocorrencia_aula destino ON destino.id_ocorrencia = r.id_ocorrencia_destino
                LEFT JOIN turma turma_destino ON turma_destino.id_turma = destino.id_turma
                INNER JOIN frequencia f ON f.id_matricula = r.id_matricula
                                        AND f.id_ocorrencia = r.id_ocorrencia
                                        AND f.situacao = 'Justificado'
                WHERE r.situacao = 'Pendente'
                  AND r.id_ocorrencia_destino IS NULL
                ORDER BY origem.data_aula, p.nome LIMIT @limite;";

            return ConsultarLista(sql, comando => comando.Parameters.AddWithValue("@limite", limite), leitor => new ReposicaoPendenteDashboard
            {
                // Variável = função.tipo(leitor, "nome da coluna", valor padrão)
                IdAluno = LeitorMySql.Inteiro(leitor, "id_aluno"),
                IdMatricula = LeitorMySql.Inteiro(leitor, "id_matricula"),
                IdOcorrenciaOriginal = LeitorMySql.Inteiro(leitor, "ocorrencia_original"),
                IdTurmaOrigem = LeitorMySql.Inteiro(leitor, "id_turma_origem"),
                IdTurmaDestino = LeitorMySql.Inteiro(leitor, "id_turma_destino"),
                DataDestino = leitor.IsDBNull(leitor.GetOrdinal("data_destino"))
                    ? DateTime.MinValue : LeitorMySql.Data(leitor, "data_destino"),
                Nome = LeitorMySql.Texto(leitor, "nome"),
                ModalidadeOriginal = LeitorMySql.Texto(leitor, "modalidade_original"),
                DataOriginal = LeitorMySql.Data(leitor, "data_original"),
                Destino = LeitorMySql.Texto(leitor, "destino")
            });
        }

        private static int ObterOuCriarOcorrencia(MySqlConnection conexao, MySqlTransaction transacao, int idTurma, DateTime data)
        {
            using (MySqlCommand comando = new MySqlCommand(@"
                INSERT INTO ocorrencia_aula (data_aula, situacao, id_turma)
                VALUES (@data, 'Pendente', @turma)
                ON DUPLICATE KEY UPDATE id_ocorrencia = LAST_INSERT_ID(id_ocorrencia);", conexao, transacao))
            {
                // Substitui palavras chave por variável
                comando.Parameters.AddWithValue("@data", data.Date);
                comando.Parameters.AddWithValue("@turma", idTurma);

                comando.ExecuteNonQuery(); // Executa comando SQL

                return Convert.ToInt32(comando.LastInsertedId);
            }
        }

        private static void ValidarParticipacao(MySqlConnection conexao, MySqlTransaction transacao,
            int idTurma, int idOcorrencia, int idMatricula)
        {
            using (MySqlCommand comando = new MySqlCommand(@"
                SELECT
                    EXISTS(SELECT 1 FROM turma_matricula tm
                           INNER JOIN matricula m ON m.id_matricula = tm.id_matricula
                           WHERE tm.id_turma = @turma AND tm.id_matricula = @matricula
                             AND (m.situacao = 'Ativo' OR EXISTS (
                                 SELECT 1 FROM frequencia historico
                                 WHERE historico.id_matricula = m.id_matricula
                                   AND historico.id_ocorrencia = @ocorrencia
                             )))
                    OR EXISTS(SELECT 1 FROM frequencia historico
                              WHERE historico.id_matricula = @matricula
                                AND historico.id_ocorrencia = @ocorrencia)
                    OR EXISTS(SELECT 1 FROM reposicao r
                              WHERE r.id_ocorrencia_destino = @ocorrencia
                                AND r.id_matricula = @matricula AND r.situacao = 'Pendente');", conexao, transacao))
            {
                // Substitui palavra chave por variável
                comando.Parameters.AddWithValue("@turma", idTurma);
                comando.Parameters.AddWithValue("@matricula", idMatricula);
                comando.Parameters.AddWithValue("@ocorrencia", idOcorrencia);

                if (Convert.ToInt32(comando.ExecuteScalar()) != 1)
                    throw new InvalidOperationException("O aluno não está matriculado nesta turma nem possui reposição agendada para esta aula.");
            }
        }

        private static int ObterOuCriarReposicao(MySqlConnection conexao, MySqlTransaction transacao,
            int idMatricula, int idOcorrencia, DateTime data)
        {
            using (MySqlCommand consulta = new MySqlCommand(@"
                SELECT id_reposicao, situacao, id_ocorrencia_destino
                FROM reposicao
                WHERE id_matricula = @matricula AND id_ocorrencia = @ocorrencia
                ORDER BY id_reposicao DESC LIMIT 1 FOR UPDATE;", conexao, transacao))
            {
                // Substitui palavra chave por variável
                consulta.Parameters.AddWithValue("@matricula", idMatricula);
                consulta.Parameters.AddWithValue("@ocorrencia", idOcorrencia);

                using (MySqlDataReader leitor = consulta.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        int id = Convert.ToInt32(leitor["id_reposicao"]);
                        string situacao = Convert.ToString(leitor["situacao"]);
                        bool temDestino = !leitor.IsDBNull(leitor.GetOrdinal("id_ocorrencia_destino"));
                        leitor.Close();

                        if (situacao == "Concluida")
                            throw new InvalidOperationException("Esta justificativa já possui uma reposição concluída.");
                        if (situacao == "Pendente")
                            return id;

                        if (!temDestino)
                        {
                            using (MySqlCommand reabrir = new MySqlCommand(@"
                                UPDATE reposicao SET situacao = 'Pendente', data_agendamento = @data
                                WHERE id_reposicao = @id;", conexao, transacao))
                            {
                                // Substitui palavra chave por variável
                                reabrir.Parameters.AddWithValue("@data", data.Date);
                                reabrir.Parameters.AddWithValue("@id", id);

                                reabrir.ExecuteNonQuery(); // Executa comando SQL
                            }
                            return id;
                        }
                    }
                }
            }

            using (MySqlCommand inserir = new MySqlCommand(@"
                INSERT INTO reposicao (data_agendamento, situacao, id_ocorrencia, id_matricula)
                VALUES (@data, 'Pendente', @ocorrencia, @matricula);", conexao, transacao))
            {
                // Substitui palavra chave por variável
                inserir.Parameters.AddWithValue("@data", data.Date);
                inserir.Parameters.AddWithValue("@ocorrencia", idOcorrencia);
                inserir.Parameters.AddWithValue("@matricula", idMatricula);

                inserir.ExecuteNonQuery(); // Executa comando SQL

                return Convert.ToInt32(inserir.LastInsertedId);
            }
        }

        private static bool ReposicaoJaConcluida(MySqlConnection conexao, MySqlTransaction transacao,
            int idMatricula, int idOcorrencia)
        {
            using (MySqlCommand comando = new MySqlCommand(@"
                SELECT COUNT(*) FROM reposicao
                WHERE id_matricula = @matricula AND id_ocorrencia = @ocorrencia
                  AND situacao = 'Concluida';", conexao, transacao))
            {
                // Substitui palavra chave por variável
                comando.Parameters.AddWithValue("@matricula", idMatricula);
                comando.Parameters.AddWithValue("@ocorrencia", idOcorrencia);

                return Convert.ToInt32(comando.ExecuteScalar()) > 0;
            }
        }

        private static int ObterReposicaoPendente(MySqlConnection conexao, MySqlTransaction transacao,
            int idMatricula, int idOcorrencia)
        {
            using (MySqlCommand comando = new MySqlCommand(@"
                SELECT id_reposicao FROM reposicao
                WHERE id_matricula = @matricula AND id_ocorrencia = @ocorrencia
                  AND situacao = 'Pendente'
                ORDER BY id_reposicao DESC LIMIT 1;", conexao, transacao))
            {
                // Substitui palavra chave por variável
                comando.Parameters.AddWithValue("@matricula", idMatricula);
                comando.Parameters.AddWithValue("@ocorrencia", idOcorrencia);

                object resultado = comando.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value ? 0 : Convert.ToInt32(resultado);
            }
        }

        // Atualiza a situação de atividade conforme o histórico de frequência
        private static void RecalcularInatividade(MySqlConnection conexao, MySqlTransaction transacao, int idMatricula)
        {
            int quantidade;
            decimal limite;
            using (MySqlCommand comando = new MySqlCommand(@"
                SELECT quantidade_aulas, percentual_faltas
                FROM configuracao_frequencia WHERE id_configuracao = 1;", conexao, transacao))
            using (MySqlDataReader leitor = comando.ExecuteReader())
            {
                if (!leitor.Read())
                    return;

                quantidade = Convert.ToInt32(leitor["quantidade_aulas"]);
                limite = Convert.ToDecimal(leitor["percentual_faltas"]);
            }

            int total = 0;
            int ausencias = 0;
            using (MySqlCommand comando = new MySqlCommand(@"
                SELECT f.situacao
                FROM frequencia f
                INNER JOIN ocorrencia_aula o ON o.id_ocorrencia = f.id_ocorrencia
                INNER JOIN turma_matricula tm ON tm.id_turma = o.id_turma AND tm.id_matricula = f.id_matricula
                WHERE f.id_matricula = @matricula
                  AND o.situacao = 'Concluida'
                  AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                  AND NOT EXISTS (
                      SELECT 1 FROM reposicao r
                      WHERE r.id_matricula = f.id_matricula
                        AND r.id_ocorrencia_destino = f.id_ocorrencia
                        AND r.situacao = 'Concluida'
                  )
                ORDER BY o.data_aula DESC, o.id_ocorrencia DESC
                LIMIT @limite;", conexao, transacao))
            {
                // Substitui palavra chave por variável
                comando.Parameters.AddWithValue("@matricula", idMatricula);
                comando.Parameters.AddWithValue("@limite", quantidade);

                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        total++;
                        if (Convert.ToString(leitor["situacao"]) == "Ausente")
                            ausencias++;
                    }
                }
            }

            string situacaoAtual;
            bool inativoAutomatico;
            using (MySqlCommand consulta = new MySqlCommand(@"
                SELECT situacao, inativo_automatico
                FROM matricula WHERE id_matricula = @matricula FOR UPDATE;", conexao, transacao))
            {
                consulta.Parameters.AddWithValue("@matricula", idMatricula);
                using (MySqlDataReader leitor = consulta.ExecuteReader())
                {
                    if (!leitor.Read())
                        return;

                    situacaoAtual = Convert.ToString(leitor["situacao"]);
                    inativoAutomatico = Convert.ToInt32(leitor["inativo_automatico"]) == 1;
                }
            }

            if (situacaoAtual == "Cancelado")
                return;

            if (total < quantidade)
            {
                if (situacaoAtual == "Inativo" && inativoAutomatico)
                {
                    using (MySqlCommand reativar = new MySqlCommand(@"
                        UPDATE matricula
                        SET situacao = 'Ativo', inativo_automatico = 0
                        WHERE id_matricula = @matricula;", conexao, transacao))
                    {
                        // Substitui palavra chave por variável
                        reativar.Parameters.AddWithValue("@matricula", idMatricula);

                        reativar.ExecuteNonQuery(); // Executa comando SQL
                    }
                }
                return;
            }

            decimal percentual = total == 0 ? 0 : (decimal)ausencias / total * 100M;
            if (situacaoAtual == "Ativo" && percentual >= limite)
            {
                using (MySqlCommand inativar = new MySqlCommand(@"
                    UPDATE matricula
                    SET situacao = 'Inativo', inativo_automatico = 1
                    WHERE id_matricula = @matricula AND situacao = 'Ativo';", conexao, transacao))
                {
                    // Substitui palavra chave por variável
                    inativar.Parameters.AddWithValue("@matricula", idMatricula);

                    inativar.ExecuteNonQuery(); // Executa comando SQL
                }
            }
            else if (situacaoAtual == "Inativo" && inativoAutomatico && percentual < limite)
            {
                using (MySqlCommand reativar = new MySqlCommand(@"
                    UPDATE matricula
                    SET situacao = 'Ativo', inativo_automatico = 0
                    WHERE id_matricula = @matricula;", conexao, transacao))
                {
                    // Substitui palavra chave por variável
                    reativar.Parameters.AddWithValue("@matricula", idMatricula);

                    reativar.ExecuteNonQuery(); // Executa comando SQL
                }
            }
        }

        private static void ValidarSituacao(string situacao)
        {
            if (situacao != "Presente" && situacao != "Ausente" && situacao != "Justificado")
                throw new ArgumentException("Selecione Presente, Ausente ou Justificado para cada aluno.");
        }
    }
}
