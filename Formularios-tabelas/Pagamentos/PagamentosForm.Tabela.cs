using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    public partial class PagamentosForm {
        // Desenha os elementos das células da tabela de pagamentos
        private void GridPayments_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == colStatus.Index) {
                e.PaintBackground(e.CellBounds, true);
                string status = Convert.ToString(e.FormattedValue);
                Color background = Color.FromArgb(232, 247, 237);
                Color foreground = Color.FromArgb(38, 149, 74);
                int width = 38;
                if (status == "Pendente") {
                    background = Color.FromArgb(255, 244, 217);
                    foreground = Color.FromArgb(196, 122, 0);
                    width = 54;

                } else if (status == "Atrasado") {
                    background = Color.FromArgb(254, 228, 226);
                    foreground = Color.FromArgb(217, 45, 32);
                    width = 49;
                }
                Rectangle pill = new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - width) / 2,
                    e.CellBounds.Top + (e.CellBounds.Height - 22) / 2, width, 22);
                using (Brush brush = new SolidBrush(background)) e.Graphics.FillRectangle(brush, pill);
                using (Font statusFont = new Font("Segoe UI", 7.5F))
                    TextRenderer.DrawText(e.Graphics, status, statusFont, pill, foreground,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                e.Handled = true;

            } else if (e.ColumnIndex == colAcoes.Index) {
                e.PaintBackground(e.CellBounds, true);

                Rectangle[] botoes = VisualPadrao.ObterRetangulosDuasAcoes(e.CellBounds);
                bool hoverEditar = e.RowIndex == linhaAcaoSobMousePg && botaoAcaoSobMousePg == 0;
                bool hoverExcluir = e.RowIndex == linhaAcaoSobMousePg && botaoAcaoSobMousePg == 1;

                VisualPadrao.DesenharBotaoAcao(e.Graphics, botoes[0], false, false, hoverEditar, false, "\uE104");
                VisualPadrao.DesenharBotaoAcao(e.Graphics, botoes[1], true, false, hoverExcluir, false, "\uE107");
                e.Handled = true;
            }
        }

        // Identifica qual ação executar pela posição do mouse
        private int IndiceAcaoPagamentoSobMouse(int coluna, int linha) {
            if (coluna < 0 || linha < 0)
                return -1;
            Rectangle celula = gridPayments.GetCellDisplayRectangle(coluna, linha, false);
            Point mouse = gridPayments.PointToClient(Cursor.Position);
            Point local = new Point(mouse.X - celula.Left, mouse.Y - celula.Top);
            Rectangle[] botoes = VisualPadrao.ObterRetangulosDuasAcoes(new Rectangle(0, 0, celula.Width, celula.Height));
            if (botoes[0].Contains(local))
                return 0;
            if (botoes[1].Contains(local))
                return 1;

            return -1;
        }

        private void GridPayments_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex != colAcoes.Index) {
                LimparHoverAcoesPagamento();
                return;
            }
            int indice = IndiceAcaoPagamentoSobMouse(e.ColumnIndex, e.RowIndex);
            if (linhaAcaoSobMousePg == e.RowIndex && botaoAcaoSobMousePg == indice)
                return;
            LimparHoverAcoesPagamento();
            linhaAcaoSobMousePg = e.RowIndex; botaoAcaoSobMousePg = indice;
            gridPayments.Cursor = indice >= 0
                ? Cursors.Hand
                : Cursors.Default;
            gridPayments.InvalidateCell(e.ColumnIndex, e.RowIndex);
        }

        private void GridPayments_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == linhaAcaoSobMousePg && e.ColumnIndex == colAcoes.Index)
                LimparHoverAcoesPagamento();
        }

        private void LimparHoverAcoesPagamento() {
            int linha = linhaAcaoSobMousePg;
            linhaAcaoSobMousePg = -1;
            botaoAcaoSobMousePg = -1;
            gridPayments.Cursor = Cursors.Default;
            if (linha >= 0 && linha < gridPayments.Rows.Count)
                gridPayments.InvalidateCell(colAcoes.Index, linha);
        }

        private void GridPayments_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex != colAcoes.Index)
                return;
            // As ações de edição e exclusão serão feitas depois
        }

    }
}