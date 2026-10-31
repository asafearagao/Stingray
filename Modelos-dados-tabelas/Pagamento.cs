using System;

namespace Stingray.Modelos
{
    public class Pagamento
    {
        // Propriedades que representam os dados e a situação do pagamento
        public int Id { get; set; }
        public DateTime DataPagamento { get; set; }
        public int AlunoId { get; set; }
        public int TurmaId { get; set; }
        public string Competencia { get; set; }
        public string FormaPagamento { get; set; }
        public decimal Valor { get; set; }
        public string Situacao { get; set; }
    }
}