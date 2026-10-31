using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray
{
    public partial class TurmasForm
    {
        private int linhaAcaoSobMouseTurmas = -1;
        private int botaoAcaoSobMouseTurmas = -1;
        private int linhaBotaoPressionadoTurmas = -1;
        private int botaoPressionadoTurmas = -1;

        // Registra os eventos de interação da tabela de turmas
        private void ConfigurarEventosTabela()
        {
            dgvTurmas.CellPainting += AoDesenharCelulaDaTabela;
            dgvTurmas.CellMouseClick += AoClicarNaCelulaDaTabela;
            dgvTurmas.CellMouseMove += AoMoverMouseNaCelulaDaTabela;
            dgvTurmas.CellMouseLeave += AoSairDaCelulaDaTabela;
            dgvTurmas.CellMouseDown += AoPressionarMouseNaCelulaDaTabela;
            dgvTurmas.CellMouseUp += AoSoltarMouseNaCelulaDaTabela;
        }

        private void AoDesenharCelulaDaTabela(object remetente, DataGridViewCellPaintingEventArgs argumentos)
        {
            if (argumentos.RowIndex < 0 || argumentos.ColumnIndex < 0)
                return;

            string nomeColuna = dgvTurmas.Columns[argumentos.ColumnIndex].Name;
            if (nomeColuna != "colAcoes" && nomeColuna != "colAlunos")
                return;

            argumentos.PaintBackground(argumentos.CellBounds, true);
            if (nomeColuna == "colAlunos")
            {
                DesenharBotaoDeAlunos(argumentos);
                argumentos.Handled = true;
                return;
            }

            DesenharBotoesDeAcao(argumentos);
            argumentos.Handled = true;
        }

        private void DesenharBotaoDeAlunos(DataGridViewCellPaintingEventArgs argumentos)
        {
            Rectangle areaLupa = VisualPadrao.ObterRetanguloAcaoCentralizada(argumentos.CellBounds, 26);
            bool mouseSobre = argumentos.RowIndex == linhaAcaoSobMouseTurmas && botaoAcaoSobMouseTurmas == 2;
            bool pressionado = argumentos.RowIndex == linhaBotaoPressionadoTurmas && botaoPressionadoTurmas == 2;

            VisualPadrao.DesenharBotaoAcao(
                argumentos.Graphics,
                areaLupa,
                false,
                false,
                mouseSobre,
                pressionado,
                "\uE721");
        }

        // Desenha os botões de edição e exclusão da turma
        private void DesenharBotoesDeAcao(DataGridViewCellPaintingEventArgs argumentos)
        {
            Rectangle[] botoes = VisualPadrao.ObterRetangulosDuasAcoes(argumentos.CellBounds);
            for (int indice = 0; indice < botoes.Length; indice++)
            {
                bool mouseSobre = argumentos.RowIndex == linhaAcaoSobMouseTurmas && indice == botaoAcaoSobMouseTurmas;
                bool pressionado = argumentos.RowIndex == linhaBotaoPressionadoTurmas && indice == botaoPressionadoTurmas;
                string simbolo = indice == 0 ? "\uE104" : "\uE107";

                VisualPadrao.DesenharBotaoAcao(
                    argumentos.Graphics,
                    botoes[indice],
                    indice == 1,
                    false,
                    mouseSobre,
                    pressionado,
                    simbolo);
            }
        }

        // Identifica qual ação o mouse está
        private int ObterIndiceDaAcaoSobOMouse(int coluna, int linha)
        {
            if (coluna < 0 || linha < 0)
                return -1;

            Rectangle celula = dgvTurmas.GetCellDisplayRectangle(coluna, linha, false);
            Point posicaoMouse = dgvTurmas.PointToClient(Cursor.Position);
            Point posicaoNaCelula = new Point(posicaoMouse.X - celula.Left, posicaoMouse.Y - celula.Top);

            if (dgvTurmas.Columns[coluna].Name == "colAlunos")
            {
                Rectangle lupa = VisualPadrao.ObterRetanguloAcaoCentralizada(new Rectangle(0, 0, celula.Width, celula.Height), 26);
                return lupa.Contains(posicaoNaCelula) ? 2 : -1;
            }

            Rectangle[] botoes = VisualPadrao.ObterRetangulosDuasAcoes(new Rectangle(0, 0, celula.Width, celula.Height));
            for (int indice = 0; indice < botoes.Length; indice++)
            {
                if (botoes[indice].Contains(posicaoNaCelula))
                    return indice;
            }

            return -1;
        }

        // Atualiza a ação em destaque conforme a posição do mouse na tabela
        private void AoMoverMouseNaCelulaDaTabela(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            if (argumentos.RowIndex < 0 || argumentos.ColumnIndex < 0)
            {
                LimparHoverAcoesTurmas();
                return;
            }

            string nomeColuna = dgvTurmas.Columns[argumentos.ColumnIndex].Name;
            if (nomeColuna != "colAcoes" && nomeColuna != "colAlunos")
            {
                LimparHoverAcoesTurmas();
                return;
            }

            int botao = ObterIndiceDaAcaoSobOMouse(argumentos.ColumnIndex, argumentos.RowIndex);
            if (linhaAcaoSobMouseTurmas == argumentos.RowIndex && botaoAcaoSobMouseTurmas == botao)
                return;

            LimparHoverAcoesTurmas();
            linhaAcaoSobMouseTurmas = argumentos.RowIndex;
            botaoAcaoSobMouseTurmas = botao;
            dgvTurmas.Cursor = botao >= 0 ? Cursors.Hand : Cursors.Default;
            dgvTurmas.InvalidateCell(argumentos.ColumnIndex, argumentos.RowIndex);
        }

        private void AoSairDaCelulaDaTabela(object remetente, DataGridViewCellEventArgs argumentos)
        {
            if (argumentos.RowIndex == linhaAcaoSobMouseTurmas && argumentos.ColumnIndex >= 0)
            {
                string nomeColuna = dgvTurmas.Columns[argumentos.ColumnIndex].Name;
                if (nomeColuna == "colAcoes" || nomeColuna == "colAlunos")
                    LimparHoverAcoesTurmas();
            }
        }

        private void AoPressionarMouseNaCelulaDaTabela(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            if (argumentos.RowIndex < 0 || argumentos.ColumnIndex < 0)
                return;

            string nomeColuna = dgvTurmas.Columns[argumentos.ColumnIndex].Name;
            if (nomeColuna != "colAcoes" && nomeColuna != "colAlunos")
                return;

            int botao = ObterIndiceDaAcaoSobOMouse(argumentos.ColumnIndex, argumentos.RowIndex);
            if (botao < 0)
                return;

            linhaBotaoPressionadoTurmas = argumentos.RowIndex;
            botaoPressionadoTurmas = botao;
            dgvTurmas.InvalidateCell(argumentos.ColumnIndex, argumentos.RowIndex);
            dgvTurmas.Cursor = Cursors.Hand;
        }

        private void AoSoltarMouseNaCelulaDaTabela(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            int linha = linhaBotaoPressionadoTurmas;
            int botao = botaoPressionadoTurmas;
            linhaBotaoPressionadoTurmas = -1;
            botaoPressionadoTurmas = -1;

            if (linha < 0 || linha >= dgvTurmas.Rows.Count)
                return;

            string nomeColuna = botao == 2 ? "colAlunos" : "colAcoes";
            if (dgvTurmas.Columns.Contains(nomeColuna))
                dgvTurmas.InvalidateCell(dgvTurmas.Columns[nomeColuna].Index, linha);
        }

        // Limpa hover ações turmas
        private void LimparHoverAcoesTurmas()
        {
            int linha = linhaAcaoSobMouseTurmas;
            int botao = botaoAcaoSobMouseTurmas;
            linhaAcaoSobMouseTurmas = -1;
            botaoAcaoSobMouseTurmas = -1;
            dgvTurmas.Cursor = Cursors.Default;

            if (linha < 0 || linha >= dgvTurmas.Rows.Count)
                return;

            string nomeColuna = botao == 2 ? "colAlunos" : "colAcoes";
            if (dgvTurmas.Columns.Contains(nomeColuna))
                dgvTurmas.InvalidateCell(dgvTurmas.Columns[nomeColuna].Index, linha);
        }

        // Trata o clique na célula da tabela e identifica a ação solicitada
        private void AoClicarNaCelulaDaTabela(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            if (argumentos.RowIndex < 0 || argumentos.ColumnIndex < 0)
                return;

            string nomeColuna = dgvTurmas.Columns[argumentos.ColumnIndex].Name;
            if (nomeColuna == "colAlunos")
            {
                GerirAlunosPresentes(argumentos.RowIndex);
                return;
            }

            if (nomeColuna != "colAcoes")
                return;

            int acao = ObterIndiceDaAcaoSobOMouse(argumentos.ColumnIndex, argumentos.RowIndex);
            if (acao == 0)
                AbrirCadastroTurma(argumentos.RowIndex);
            else if (acao == 1)
                ExcluirTurma(argumentos.RowIndex);
        }
    }
}