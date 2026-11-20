using System;
using System.Collections.Generic;
using System.Linq;
using Stingray.DadosTela;
using Stingray.Entidades;
using Stingray.Repositorios;

namespace Stingray.Servicos
{

    // Aplica as regras de frequência, chamada e reposição
    public class FrequenciaServico
    {
        private readonly FrequenciaRepositorio repositorio = new FrequenciaRepositorio();
        private readonly TurmaRepositorio turmaRepositorio = new TurmaRepositorio();

        public List<Turma> ListarTurmasDaData(DateTime data)
        {
            List<Turma> turmas = turmaRepositorio.ListarTodos();
            List<int> destinos = repositorio.ListarTurmasRemarcadasParaData(data.Date);
            return turmas
                .Where(turma =>
                    (!repositorio.EstaRemarcadaNaOrigem(turma.IdTurma, data.Date) &&
                     TurmaOcorreNoDia(turma.DiaSemana, data.DayOfWeek)) ||
                    destinos.Contains(turma.IdTurma))
                .GroupBy(turma => turma.IdTurma)
                .Select(grupo => grupo.First())
                .OrderBy(turma => turma.HorarioInicio)
                .ThenBy(turma => turma.Modalidade)
                .ToList();
        }

        public int ObterOuCriarOcorrencia(int idTurma, DateTime data)
        {
            ValidarTurmaNaData(idTurma, data);
            return repositorio.ObterOuCriarOcorrencia(idTurma, data.Date);
        }

        // Para exibir a lista da chamada
        public List<ParticipantePresenca> ListarParticipantes(int idTurma, DateTime data)
        {
            int idOcorrencia = ObterOuCriarOcorrencia(idTurma, data);
            return repositorio.ListarParticipantes(idTurma, idOcorrencia);
        }

        public void SalvarChamada(int idTurma, DateTime data, IList<ParticipantePresenca> participantes)
        {
            ValidarTurmaNaData(idTurma, data);
            Turma turma = turmaRepositorio.ListarTodos().First(item => item.IdTurma == idTurma);
            bool aulaIniciada = data.Date < DateTime.Today ||
                (data.Date == DateTime.Today && DateTime.Now.TimeOfDay >= turma.HorarioInicio);

            if (!aulaIniciada && participantes.Any(item => item.Situacao != "Justificado"))
                throw new InvalidOperationException("Antes do início da aula, somente justificativas podem ser registradas.");

            bool aulaRealizada = data.Date < DateTime.Today ||
                (data.Date == DateTime.Today && DateTime.Now.TimeOfDay >= turma.HorarioFim);

            repositorio.SalvarChamada(idTurma, data.Date, aulaRealizada, participantes);
        }

        // Abrir a edição antecipadamente
        public bool PermiteJustificativasAntecipadas(int idTurma, DateTime data)
        {
            ValidarTurmaNaData(idTurma, data);
            Turma turma = turmaRepositorio.ListarTodos().First(item => item.IdTurma == idTurma);

            return data.Date > DateTime.Today ||
                (data.Date == DateTime.Today && DateTime.Now.TimeOfDay < turma.HorarioInicio);
        }

        // Não deixar agendar para o dia/horário passado
        public void AgendarReposicao(int idMatricula, int idOcorrenciaOriginal, int idTurmaDestino, DateTime dataDestino)
        {
            ValidarTurmaNaData(idTurmaDestino, dataDestino);
            Turma destino = turmaRepositorio.ListarTodos().First(item => item.IdTurma == idTurmaDestino);
            if (dataDestino.Date == DateTime.Today && DateTime.Now.TimeOfDay >= destino.HorarioInicio)
                throw new InvalidOperationException("A reposição deve ser agendada antes do início da aula de destino.");

            repositorio.AgendarReposicao(idMatricula, idOcorrenciaOriginal, idTurmaDestino, dataDestino.Date);
        }

        // Caso seja justificada ou queira remarcar a aula para todos os alunos
        public void RemarcarAula(int idTurma, DateTime dataOrigem, DateTime dataDestino)
        {
            Turma turma = turmaRepositorio.ListarTodos().FirstOrDefault(item => item.IdTurma == idTurma);
            if (turma == null)
                throw new InvalidOperationException("A turma selecionada não foi encontrada.");
            if (!TurmaOcorreNoDia(turma.DiaSemana, dataOrigem.DayOfWeek))
                throw new InvalidOperationException("A data de origem não corresponde ao dia regular desta turma.");
            repositorio.RemarcarAula(idTurma, dataOrigem.Date, dataDestino.Date);
        }

        public ResumoFrequenciaAluno ObterResumoAluno(int idAluno)
        {
            repositorio.AtualizarOcorrenciasEncerradas();
            ConfiguracaoFrequencia configuracao = repositorio.ObterConfiguracao();

            return repositorio.ObterResumoAluno(idAluno, configuracao.QuantidadeAulas);
        }

        // Qual porcentagem e aulas usar para definir inatividade
        public ConfiguracaoFrequencia ObterConfiguracao()
        {
            return repositorio.ObterConfiguracao();
        }

        // Atualizar critérios para inatividade
        public void SalvarConfiguracao(ConfiguracaoFrequencia configuracao)
        {
            repositorio.AtualizarOcorrenciasEncerradas();
            repositorio.SalvarConfiguracao(configuracao);
        }

        // Exibir no Dashboard
        public int ContarAlunosInativos()
        {
            return repositorio.ContarAlunosInativos();
        }

        // Exibir no Dashboard
        public int ContarReposicoesPendentes()
        {
            return repositorio.ContarReposicoesPendentes();
        }

        // Exibir no Dashboard
        public List<AlunoInativoDashboard> ListarAlunosInativos(int limite = 5)
        {
            repositorio.AtualizarOcorrenciasEncerradas();
            return repositorio.ListarAlunosInativos(limite);
        }

        // Exibir no Dashboard
        public List<ReposicaoPendenteDashboard> ListarReposicoesPendentes(int limite = 5)
        {
            return repositorio.ListarReposicoesPendentes(limite);
        }

        // A data escolhida deve ter uma turma nesta data
        private void ValidarTurmaNaData(int idTurma, DateTime data)
        {
            Turma turma = turmaRepositorio.ListarTodos().FirstOrDefault(item => item.IdTurma == idTurma);
            if (turma == null)
                throw new InvalidOperationException("A turma selecionada não foi encontrada.");
            bool dataRegular = TurmaOcorreNoDia(turma.DiaSemana, data.DayOfWeek) &&
                !repositorio.EstaRemarcadaNaOrigem(idTurma, data.Date);
            bool dataDeDestino = repositorio.EhDestinoRemarcacao(idTurma, data.Date);
            if (!dataRegular && !dataDeDestino)
                throw new InvalidOperationException("A data escolhida não corresponde a uma aula regular nem a uma remarcação desta turma.");
        }

        // Exibir junto ao nome da turma nas consultas de outras telas
        private static bool TurmaOcorreNoDia(string diaTexto, DayOfWeek dia)
        {
            string texto = (diaTexto ?? string.Empty).Trim().ToLowerInvariant();
            switch (dia)
            {
                case DayOfWeek.Sunday:     return texto.StartsWith("domingo");
                case DayOfWeek.Monday:     return texto.StartsWith("segunda");
                case DayOfWeek.Tuesday:    return texto.StartsWith("terça") || texto.StartsWith("terca");
                case DayOfWeek.Wednesday:  return texto.StartsWith("quarta");
                case DayOfWeek.Thursday:   return texto.StartsWith("quinta");
                case DayOfWeek.Friday:     return texto.StartsWith("sexta");
                case DayOfWeek.Saturday:   return texto.StartsWith("sábado") || texto.StartsWith("sabado");
                default: return false;
            }
        }
    }
}
