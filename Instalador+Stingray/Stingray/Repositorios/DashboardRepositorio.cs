using System.Collections.Generic; // Listas
using Stingray.BancoDeDados;
using Stingray.DadosTela;

namespace Stingray.Repositorios
{
    // Consulta a programação semanal exibida no Dashboard
    public class DashboardRepositorio : RepositorioMySqlBase
    {

        public List<ProximaAulaDashboard> ListarTurmasDaAgenda()
        {
            // Consulta SQL para obter as turmas com dia da semana e horário de início
            const string sql = @"
                SELECT t.id_turma, t.modalidade, t.dia_semana, t.horario_inicio, p.nome AS nome_professor
                FROM turma t
                INNER JOIN professor pr ON pr.id_professor = t.id_professor
                INNER JOIN pessoa p ON p.id_pessoa = pr.id_professor
                ORDER BY t.dia_semana, t.horario_inicio, t.modalidade;";

            return ConsultarLista(sql, null, leitor => new ProximaAulaDashboard
            {
                IdTurma = LeitorMySql.Inteiro(leitor, "id_turma"),
                Modalidade = LeitorMySql.Texto(leitor, "modalidade"),
                DiaSemana = LeitorMySql.Texto(leitor, "dia_semana"),
                HorarioInicio = LeitorMySql.Hora(leitor, "horario_inicio"),
                NomeProfessor = LeitorMySql.Texto(leitor, "nome_professor")
            });
        }

        public bool EstaRemarcadaNaOrigem(int idTurma, System.DateTime data)
        {
            const string sql = "SELECT COUNT(*) FROM remarcacao_aula WHERE id_turma = @turma AND data_origem = @data;";
            object valor = ConsultarValor(sql, comando =>
            {
                comando.Parameters.AddWithValue("@turma", idTurma);
                comando.Parameters.AddWithValue("@data", data.Date);
            });
            return System.Convert.ToInt32(valor) > 0;
        }

        public List<ProximaAulaDashboard> ListarRemarcacoesFuturas(System.DateTime aPartirDe)
        {
            const string sql = @"                SELECT r.id_turma, r.data_origem, r.data_destino, t.modalidade,                       t.horario_inicio, p.nome AS nome_professor                FROM remarcacao_aula r                INNER JOIN turma t ON t.id_turma = r.id_turma                INNER JOIN professor pr ON pr.id_professor = t.id_professor                INNER JOIN pessoa p ON p.id_pessoa = pr.id_professor                WHERE r.data_destino >= @data                ORDER BY r.data_destino, t.horario_inicio;";
            return ConsultarLista(sql, comando => comando.Parameters.AddWithValue("@data", aPartirDe.Date), leitor => new ProximaAulaDashboard
            {
                IdTurma = LeitorMySql.Inteiro(leitor, "id_turma"),
                Modalidade = LeitorMySql.Texto(leitor, "modalidade"),
                HorarioInicio = LeitorMySql.Hora(leitor, "horario_inicio"),
                NomeProfessor = LeitorMySql.Texto(leitor, "nome_professor"),
                DataOriginal = LeitorMySql.Data(leitor, "data_origem"),
                DataHora = LeitorMySql.Data(leitor, "data_destino").Date.Add(LeitorMySql.Hora(leitor, "horario_inicio")),
                EhRemarcacao = true
            });
        }

        public ResumoDashboard ObterResumo()
        {
            // Retorna a quantidade atual de alunos, professores e turmas cadastrados
            const string sql = @"
                SELECT (SELECT COUNT(*) FROM aluno) AS total_alunos,
                       (SELECT COUNT(*) FROM professor) AS total_professores,
                       (SELECT COUNT(*) FROM turma) AS total_turmas;";

            return ConsultarUnico(sql, null, leitor => new ResumoDashboard
            {
                TotalAlunos = LeitorMySql.Inteiro(leitor, "total_alunos"),
                TotalProfessores = LeitorMySql.Inteiro(leitor, "total_professores"),
                TotalTurmas = LeitorMySql.Inteiro(leitor, "total_turmas")
            }, new ResumoDashboard());
        }
    }
}
