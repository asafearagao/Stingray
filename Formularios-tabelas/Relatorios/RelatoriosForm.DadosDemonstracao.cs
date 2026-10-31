namespace Stingray
{
    public partial class RelatoriosForm
    {
        // Valores de demostração/teste
        private void CarregarDadosDemonstracao()
        {
            lblReceitaValor.Text = "R$ 23.450,00";
            lblPresencaValor.Text = "86%";
            lblAlunosValor.Text = "128";

            lblReceitaTotal.Text = "R$ 23.450,00";
            lblReceitaRecebida.Text = "R$ 21.830,00";
            lblReceitaPendente.Text = "R$ 1.620,00";

            graficoReceita.DefinirDados(
                new string[] { "Dez/23", "Jan/24", "Fev/24", "Mar/24", "Abr/24", "Mai/24" },
                new double[] { 15400, 17800, 19500, 21230, 19750, 23450 });

            graficoPresencaRelatorios.DefinirDados(
                new float[] { 86F, 8F, 6F },
                new string[] { "Presentes", "Ausentes", "Justificadas" },
                new string[] { "86%  (1.102)", "8%  (102)", "6%  (76)" },
                "Total: 1.280 presenças");
        }
    }
}