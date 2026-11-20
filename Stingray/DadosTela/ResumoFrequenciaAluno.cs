namespace Stingray.DadosTela
{

    // Representa o resumo de frequência de um aluno
    public class ResumoFrequenciaAluno
    {
        public int TotalAulas { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public int Justificados { get; set; }
        public decimal PercentualFaltas { get; set; }
    }
}
