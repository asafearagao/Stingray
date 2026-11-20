using System;

namespace Stingray.DadosTela
{

    // Reúne os dados de uma mensalidade exibida na tela de Pagamentos
    public class PagamentoLinha
    {
        public int IdMensalidade { get; set; }
        public int IdAluno { get; set; }
        public DateTime DataNascimentoAluno { get; set; }
        public string NomeResponsavel { get; set; }
        public string TelefoneContato { get; set; }
        public string TelefoneResponsavel { get; set; }
        public string NomeAluno { get; set; }
        public string Turmas { get; set; }
        public DateTime MesReferencia { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public string Situacao { get; set; }
    }
}
