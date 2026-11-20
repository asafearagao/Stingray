using System;

namespace Stingray.Servicos
{

    // Define quais mensalidades pagas já ultrapassaram o prazo de retenção
    public static class RetencaoPagamentoRegra
    {

        // Calcula a data a partir da qual os pagamentos recebidos podem ser removidos
        public static DateTime ObterDataLimite(DateTime hoje)
        {
            if (hoje == DateTime.MinValue)
                throw new ArgumentException("Informe uma data válida.", nameof(hoje));

            return hoje.Date.AddMonths(-6);
        }

        // Preserva pendências e identifica somente pagamentos antigos já recebidos
        public static bool DeveExcluirPagamento(string situacao, DateTime dataPagamento, DateTime vencimento, DateTime hoje)
        {
            if (hoje == DateTime.MinValue || vencimento == DateTime.MinValue)
                return false;

            if (!string.Equals(situacao, "Pago", StringComparison.OrdinalIgnoreCase))
                return false;

            DateTime dataReferencia = dataPagamento == DateTime.MinValue
                ? vencimento.Date
                : dataPagamento.Date;

            return dataReferencia < ObterDataLimite(hoje);
        }
    }
}
