using System;

namespace Stingray.DadosTela
{

    // Transporta os dados de turma exibidos na linha de um aluno
    public class TurmaAlunoLinha
    {
        public int IdTurma { get; set; }
        public string Modalidade { get; set; }
        public string DiaSemana { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string NomeProfessor { get; set; }
        public decimal ValorMensal { get; set; }
    }
}
