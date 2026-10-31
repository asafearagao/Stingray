namespace Stingray
{
    public partial class DashboardForm
    {
        private void CarregarDadosDemonstracao()
        {
            PreencherIndicadoresDemonstracao();
            PreencherAulasDemonstracao();
            PreencherPagamentosDemonstracao();
        }

        private void PreencherIndicadoresDemonstracao()
        {
            dashValorAlunos.Text = "9";
            dashValorProfessores.Text = "7";
            dashValorTurmas.Text = "7";
        }

        private void PreencherAulasDemonstracao()
        {
            dashAulaHora1.Text = "08:00";
            dashAulaNome1.Text = "Hidroginástica";
            dashAulaProfessor1.Text = "Prof. Lucas Almeida";

            dashAulaHora2.Text = "09:00";
            dashAulaNome2.Text = "Natação Iniciante";
            dashAulaProfessor2.Text = "Prof. Ana Paula Costa";

            dashAulaHora3.Text = "10:00";
            dashAulaNome3.Text = "Natação Intermediário";
            dashAulaProfessor3.Text = "Prof. Gabriel Silva";

            dashAulaHora4.Text = "14:00";
            dashAulaNome4.Text = "Natação Avançado";
            dashAulaProfessor4.Text = "Prof. Felipe Rocha";
        }

        private void PreencherPagamentosDemonstracao()
        {
            dashGradePagamentos.Rows.Add("Maria Silva", "10/05/2024", "R$ 180,00");
            dashGradePagamentos.Rows.Add("João Pedro", "12/05/2024", "R$ 180,00");
            dashGradePagamentos.Rows.Add("Ana Clara", "15/05/2024", "R$ 180,00");
            dashGradePagamentos.Rows.Add("Pedro Henrique", "18/05/2024", "R$ 180,00");
            dashGradePagamentos.Rows.Add("Laura Souza", "20/05/2024", "R$ 180,00");
            dashGradePagamentos.ClearSelection();
        }
    }
}