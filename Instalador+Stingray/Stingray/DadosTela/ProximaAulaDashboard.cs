namespace Stingray.DadosTela
{

    // Transporta os dados de uma aula exibida no Dashboard
    public class ProximaAulaDashboard
    {
        public int IdTurma { get; set; }
        public string Modalidade { get; set; }
        public string DiaSemana { get; set; }
        public System.TimeSpan HorarioInicio { get; set; }
        public string NomeProfessor { get; set; }
        public System.DateTime DataHora { get; set; }
        public bool EhRemarcacao { get; set; }
        public System.DateTime DataOriginal { get; set; }
    }
}
