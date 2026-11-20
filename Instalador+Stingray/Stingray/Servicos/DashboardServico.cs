using System;
using System.Collections.Generic;  // Listas
using System.Linq;                 // Ordenação
using Stingray.DadosTela;
using Stingray.Repositorios;

namespace Stingray.Servicos
{

    // Prepara os indicadores e as próximas aulas do Dashboard
    public class DashboardServico // Fornece as somas calculadas do banco de dados para o "resumo"
    {
        private readonly DashboardRepositorio repositorio;

        public DashboardServico() // Construtor
        {
            repositorio = new DashboardRepositorio();
        }

        public ResumoDashboard ObterResumo()  // Obtém o total de alunos, turmas e professores
        {
            return repositorio.ObterResumo();
        }

        // Calcula as próximas aulas usando o dia da semana o horário

        // Não dependendo da existência prévia da aula
        public List<ProximaAulaDashboard> ObterProximasAulas(int quantidade = 4)
        {
            if (quantidade <= 0)  // Se não tiver aulas cadastradas, não retorna nada
                return new List<ProximaAulaDashboard>();

            DateTime agora = DateTime.Now;  // Horário atual
            // Obtém as turmas, com dia da semana e horário de início
            List<ProximaAulaDashboard> turmas = repositorio.ListarTurmasDaAgenda();

            // Calcula a data e hora da próxima aula de cada turma
            foreach (ProximaAulaDashboard turma in turmas)
            {
                DayOfWeek dia;  // Converte o dia da semana em DayOfWeek

                // Se não conseguir converter, ignora a turma
                if (!TentarObterDiaSemana(turma.DiaSemana, out dia))
                    continue;

                // Calcula a data e hora da próxima aula
                int diasAteAula = ((int)dia - (int)agora.DayOfWeek + 7) % 7;

                // Se a aula for hoje, mas o horário já passou, considera a próxima semana
                DateTime proximaAula = agora.Date.AddDays(diasAteAula).Add(turma.HorarioInicio);

                if (proximaAula <= agora)
                    proximaAula = proximaAula.AddDays(7);

                int tentativas = 0;
                while (repositorio.EstaRemarcadaNaOrigem(turma.IdTurma, proximaAula.Date) && tentativas < 52)
                {
                    proximaAula = proximaAula.AddDays(7);
                    tentativas++;
                }

                turma.DataHora = proximaAula; // Armazena a data e hora da próxima aula
            }

            // Ordena as turma
            List<ProximaAulaDashboard> resultado = turmas
                .Where(turma => turma.DataHora > DateTime.MinValue)
                .ToList();
            resultado.AddRange(repositorio.ListarRemarcacoesFuturas(agora.Date)
                .Where(aula => aula.DataHora > agora));

            return resultado
                .OrderBy(turma => turma.DataHora)
                .Take(quantidade)
                .ToList();
        }

        // Converte o nome do dia da semana em DayOfWeek
        private static bool TentarObterDiaSemana(string texto, out DayOfWeek dia)
        {
            dia = DayOfWeek.Monday; // Inicializa o dia como segunda-feira, caso não converta

            if (string.IsNullOrWhiteSpace(texto)) // Texto vazio, não consegue converter
                return false;

            // Converte o texto para minúsculo e sem espaços no início ou fim
            string valor = texto.Trim().ToLowerInvariant();

            if (valor.StartsWith("segunda")) dia = DayOfWeek.Monday;
            else if (valor.StartsWith("terça") || valor.StartsWith("terca")) dia = DayOfWeek.Tuesday;
            else if (valor.StartsWith("quarta")) dia = DayOfWeek.Wednesday;
            else if (valor.StartsWith("quinta")) dia = DayOfWeek.Thursday;
            else if (valor.StartsWith("sexta")) dia = DayOfWeek.Friday;
            else if (valor.StartsWith("sábado") || valor.StartsWith("sabado")) dia = DayOfWeek.Saturday;
            else if (valor.StartsWith("domingo")) dia = DayOfWeek.Sunday;
            else return false;

            return true;
        }
    }
}
