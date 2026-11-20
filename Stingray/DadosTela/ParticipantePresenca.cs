namespace Stingray.DadosTela
{

    // Representa um aluno na chamada de uma aula
    public class ParticipantePresenca
    {
        public int IdMatricula { get; set; }
        public int IdAluno { get; set; }
        public int IdOcorrencia { get; set; }
        public int IdReposicao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
        public string Observacao { get; set; }
        public bool EhReposicao { get; set; }
        public bool PossuiReposicaoAgendada { get; set; }
        public string SituacaoReposicao { get; set; }
    }
}
