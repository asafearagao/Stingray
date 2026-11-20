namespace Stingray.DadosTela
{

    // Agrupa os campos retornados pela consulta de CEP
    public class ResultadoConsultaCep
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
    }
}
