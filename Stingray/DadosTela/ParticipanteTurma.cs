namespace Stingray.DadosTela
{

    // Representa o vínculo de um aluno com uma turma
    public class ParticipanteTurma
    {
        public int IdMatricula { get; set; }
        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public bool Selecionado { get; set; }
    }
}
