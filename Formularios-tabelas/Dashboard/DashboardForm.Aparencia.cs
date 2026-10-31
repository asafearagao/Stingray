using System.Drawing;
using System.Windows.Forms;

namespace Stingray
{
    public partial class DashboardForm
    {
        private void ConfigurarBordasDosCartoes()
        {
            Control[] cartoes =
            {
                dashCardAlunos,
                dashCardProfessores,
                dashCardTurmas,
                dashCardAulas,
                dashCardPagamentos
            };

            foreach (Control cartao in cartoes)
            {
                Panel painel = cartao as Panel;
                if (painel == null)
                    continue;

                painel.BorderStyle = BorderStyle.None;
                painel.BackColor = Color.White;

                if (painel == dashCardAlunos || painel == dashCardProfessores || painel == dashCardTurmas)
                    painel.Padding = new Padding(2);

                painel.Paint -= DashboardCard_Paint;
                painel.Paint += DashboardCard_Paint;
            }
        }

        // Desenha a borda do painel principal do dashboard
        private void DashboardContentPanel_Paint(object sender, PaintEventArgs e)
        {
            VisualPadrao.DesenharBordaCartao(e.Graphics, dashboardContentPanel.ClientRectangle);
        }

        // Desenha a borda do cartão do dashboard
        private void DashboardCard_Paint(object sender, PaintEventArgs e)
        {
            Control cartao = sender as Control;
            if (cartao == null)
                return;

            VisualPadrao.DesenharBordaCartao(e.Graphics, cartao.ClientRectangle);
        }

        private void DashAulasTabela_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row >= 3)
                return;

            using (Pen linha = new Pen(Color.FromArgb(229, 234, 241), 1F))
            {
                e.Graphics.DrawLine(
                    linha,
                    e.CellBounds.Left + 1,
                    e.CellBounds.Bottom - 1,
                    e.CellBounds.Right - 1,
                    e.CellBounds.Bottom - 1);
            }
        }
    }
}