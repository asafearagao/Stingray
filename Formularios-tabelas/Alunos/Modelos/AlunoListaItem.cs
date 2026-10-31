using Stingray.Modelos;

namespace Stingray.Formularios.Alunos.Modelos
{
    internal sealed class AlunoListaItem
    {
        public Aluno Aluno { get; private set; }
        public string Turma { get; private set; }
        public string Professor { get; private set; }
        public string Plano { get; private set; }
        public string Status { get; private set; }

        // Inicializa um item da lista com aluno, turma, professor, plano e status
        public AlunoListaItem(Aluno aluno, string turma, string professor, string plano, string status)
        {
            Aluno = aluno;
            Turma = turma;
            Professor = professor;
            Plano = plano;
            Status = status;
        }
    }
}