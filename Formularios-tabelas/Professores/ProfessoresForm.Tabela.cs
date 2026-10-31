using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    public partial class ProfessoresForm : Form {
        private void BtnAdicionar_Click(object sender, EventArgs e) {
            AbrirCadastroProfessor(-1);
        }

        // Trata o clique na célula da tabela e identifica a ação solicitada
        private void AoClicarNaCelulaDaTabela(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (string.Equals(coluna, "colTurmas", StringComparison.Ordinal)) {
                MostrarTurmasDoProfessor(e.RowIndex);
                return;
            }
            if (!string.Equals(coluna, "colAcoes", StringComparison.Ordinal))
                return;

            int acao = ObterIndiceDaAcaoSobOMouse(e.ColumnIndex, e.RowIndex);
            if (acao == 0)
                AbrirCadastroProfessor(e.RowIndex);
            else if (acao == 1)
                ConfirmarExclusaoProfessor(e.RowIndex);
        }

        private void AoDesenharCelulaDaTabela(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colTurmas")
                return;

            e.PaintBackground(e.CellBounds, true);
            if (coluna == "colTurmas") {
                Rectangle lupa = VisualPadrao.ObterRetanguloAcaoCentralizada(e.CellBounds, 26);
                bool hover = e.RowIndex == linhaAcaoSobMouse && botaoAcaoSobMouse == 2;
                bool pressionado = e.RowIndex == linhaBotaoPressionado && botaoPressionado == 2;

                VisualPadrao.DesenharBotaoAcao(e.Graphics, lupa, false, false, hover, pressionado, "\uE721");
                e.Handled = true;
                return;
            }

            Rectangle[] botoes = VisualPadrao.ObterRetangulosDuasAcoes(e.CellBounds);
            for (int i = 0; i < botoes.Length; i++) {
                bool hover = e.RowIndex == linhaAcaoSobMouse && botaoAcaoSobMouse == i;
                bool pressionado = e.RowIndex == linhaBotaoPressionado && botaoPressionado == i;
                string icone = i == 0 ? "\uE104" : "\uE107";

                VisualPadrao.DesenharBotaoAcao(e.Graphics, botoes[i], i == 1, false, hover, pressionado, icone);
            }
            e.Handled = true;
        }

        private int ObterIndiceDaAcaoSobOMouse(int coluna, int linha) {
            if (coluna < 0 || linha < 0)
                return -1;
            Rectangle celula = dgvProfessores.GetCellDisplayRectangle(coluna, linha, false);
            Point mouse = dgvProfessores.PointToClient(Cursor.Position);
            Point local = new Point(mouse.X - celula.Left, mouse.Y - celula.Top);
            if (dgvProfessores.Columns[coluna].Name == "colTurmas")
                return VisualPadrao.ObterRetanguloAcaoCentralizada(new Rectangle(0, 0, celula.Width, celula.Height), 26).Contains(local) ? 2 : -1;
            Rectangle[] botoes = VisualPadrao.ObterRetangulosDuasAcoes(new Rectangle(0, 0, celula.Width, celula.Height));
            for (int i = 0; i < botoes.Length; i++)
                if (botoes[i].Contains(local))
                    return i;

            return -1;
        }

        private void AoMoverMouseNaCelulaDaTabela(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) {
                LimparRealceDasAcoes();
                return;
            }
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colTurmas") {
                LimparRealceDasAcoes();
                return;
            }
            int botao = ObterIndiceDaAcaoSobOMouse(e.ColumnIndex, e.RowIndex);
            if (linhaAcaoSobMouse == e.RowIndex && botaoAcaoSobMouse == botao)
                return;
            LimparRealceDasAcoes();
            linhaAcaoSobMouse = e.RowIndex; botaoAcaoSobMouse = botao;
            dgvProfessores.Cursor = botao >= 0
                ? Cursors.Hand
                : Cursors.Default;
            dgvProfessores.InvalidateCell(e.ColumnIndex, e.RowIndex);
        }

        private void AoSairDaCelulaDaTabela(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == linhaAcaoSobMouse && e.ColumnIndex >= 0 &&
                (dgvProfessores.Columns[e.ColumnIndex].Name == "colAcoes" || dgvProfessores.Columns[e.ColumnIndex].Name == "colTurmas")) LimparRealceDasAcoes();
        }

        private void AoPressionarMouseNaCelulaDaTabela(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colTurmas")
                return;
            int botao = ObterIndiceDaAcaoSobOMouse(e.ColumnIndex, e.RowIndex);
            if (botao < 0)
                return;
            linhaBotaoPressionado = e.RowIndex; botaoPressionado = botao;
            dgvProfessores.InvalidateCell(e.ColumnIndex, e.RowIndex);
            dgvProfessores.Cursor = Cursors.Hand;
        }

        private void AoSoltarMouseNaCelulaDaTabela(object sender, DataGridViewCellMouseEventArgs e) {
            int linha = linhaBotaoPressionado;
            int botao = botaoPressionado;
            linhaBotaoPressionado = -1; botaoPressionado = -1;
            if (linha >= 0 && linha < dgvProfessores.Rows.Count) {
                if (botao == 2 && dgvProfessores.Columns.Contains("colTurmas"))
                    dgvProfessores.InvalidateCell(dgvProfessores.Columns["colTurmas"].Index, linha);
                else if (dgvProfessores.Columns.Contains("colAcoes"))
                    dgvProfessores.InvalidateCell(dgvProfessores.Columns["colAcoes"].Index, linha);
            }
        }

        // Remove o destaque visual das ações da tabela
        private void LimparRealceDasAcoes() {
            int linha = linhaAcaoSobMouse;
            int botao = botaoAcaoSobMouse;
            linhaAcaoSobMouse = -1;
            botaoAcaoSobMouse = -1;
            dgvProfessores.Cursor = Cursors.Default;
            if (linha >= 0 && linha < dgvProfessores.Rows.Count) {
                string nomeColuna = botao == 2 ? "colTurmas" : "colAcoes";
                if (dgvProfessores.Columns.Contains(nomeColuna))
                    dgvProfessores.InvalidateCell(dgvProfessores.Columns[nomeColuna].Index, linha);
            }
        }
    }
}