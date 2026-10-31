using System.Windows.Forms;

namespace Stingray {
    public partial class PagamentosForm {
        // Carrega os registros de demonstração/teste na tabela de pagamentos
        private void PreencherPagamentosDeExemplo() {
            gridPayments.Rows.Add("20/05/2024", "Maria Silva", "Hidroginástica", "Mai/2024", "Dinheiro", "180,00", "Pago", "");
            gridPayments.Rows.Add("18/05/2024", "João Pedro", "Natação Iniciante", "Mai/2024", "PIX", "180,00", "Pago", "");
            gridPayments.Rows.Add("15/05/2024", "Ana Clara", "Natação Intermediário", "Mai/2024", "Cartão de Débito", "180,00", "Pago", "");
            gridPayments.Rows.Add("14/05/2024", "Pedro Henrique", "Natação Avançado", "Mai/2024", "Dinheiro", "180,00", "Pago", "");
            gridPayments.Rows.Add("10/05/2024", "Laura Souza", "Natação Adulto", "Mai/2024", "PIX", "180,00", "Pago", "");
            gridPayments.Rows.Add("07/05/2024", "Gabriel Lima", "Natação Master", "Abr/Mai 2024", "Boleto Bancário", "360,00", "Pago", "");
            gridPayments.Rows.Add("05/05/2024", "Beatriz Oliveira", "Natação Competição", "Mai/2024", "Cartão de Crédito", "180,00", "Pago", "");
            gridPayments.Rows.Add("02/05/2024", "Miguel Ferreira", "Natação Iniciante", "Mai/2024", "Dinheiro", "180,00", "Pendente", "");
            gridPayments.Rows.Add("30/04/2024", "Sofia Martins", "Hidroginástica", "Abr/2024", "PIX", "180,00", "Pago", "");
            gridPayments.Rows.Add("28/04/2024", "Rafael Santos", "Natação Competição", "Abr/2024", "Boleto Bancário", "180,00", "Atrasado", "");
        }

    }
}