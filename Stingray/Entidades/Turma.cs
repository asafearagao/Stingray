using System;

namespace Stingray.Entidades
{

    // Representa uma turma e seus dados de cobrança e horário
    public class Turma
    {
        public int IdTurma { get; set; }
        public string Modalidade { get; set; }
        public string DiaSemana { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public int Capacidade { get; set; }
        public decimal ValorMensal { get; set; }
        public int IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        public int QuantidadeParticipantes { get; set; }

        public string DescricaoAgenda
        {
            get { return Modalidade + " · " + HorarioInicio.ToString(@"hh\:mm") + "–" + HorarioFim.ToString(@"hh\:mm"); }
        }

        public override string ToString()
        {
            return DescricaoAgenda;
        }
    }
}
