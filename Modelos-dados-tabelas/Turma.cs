using System.Collections.Generic;

namespace Stingray.Modelos
{
    public class Turma
    {
        public Turma()
        {
            AlunosIds = new List<int>();
            PresentesIds = new List<int>();
        }

        // Propriedades que definem a turma e mantêm seus alunos e presentes
        public int Id { get; set; }
        public string DiaSemana { get; set; }
        public string Modalidade { get; set; }
        public string Horario { get; set; }
        public int ProfessorId { get; set; }
        public IList<int> AlunosIds { get; set; }
        public IList<int> PresentesIds { get; set; }
    }
}