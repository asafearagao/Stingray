
using System.Drawing;
using System.Windows.Forms;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class AlunosGradeMouse
    {
        // Grade que recebe os eventos do mouse
        private readonly DataGridView grade;

        // Posição do mouse em relação a um elemento
        private int linhaSobOMouse = -1;
        private int botaoSobOMouse = -1;

        // Estado dos botões
        private int linhaPressionada = -1;
        private int botaoPressionado = -1;

        public AlunosGradeMouse(DataGridView grade) // Inicializa a grade utilizada
        {
            this.grade = grade;
        }

        public void Configurar() // Registra os eventos de interação do mouse com a grade
        {
            grade.CellMouseMove += AoMoverMouseSobreCelula;
            grade.CellMouseLeave += AoSairDaCelula;
            grade.CellMouseDown += AoPressionarBotaoDoMouse;
            grade.CellMouseUp += AoSoltarBotaoDoMouse;
        }
        // Verifica se o mouse está sobre o botão informado em determinada linha
        public bool MouseEstaSobre(int indiceLinha, int indiceBotao)
        {
            return indiceLinha == linhaSobOMouse && indiceBotao == botaoSobOMouse;
        }

        // Verifica se o botão informado está pressionado em determinada linha
        public bool BotaoEstaPressionado(int indiceLinha, int indiceBotao)
        {
            return indiceLinha == linhaPressionada && indiceBotao == botaoPressionado;
        }

        // Atualiza o estado do mouse quando ele se movimenta sobre uma célula
        private void AoMoverMouseSobreCelula(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            if (!EhColunaDeAcoes(argumentos.ColumnIndex) || argumentos.RowIndex < 0)
            {
                LimparEstadoDoMouse();
                return;
            }

            int indiceBotao = ObterIndiceDoBotao(argumentos.ColumnIndex, argumentos.RowIndex);
            if (linhaSobOMouse == argumentos.RowIndex && botaoSobOMouse == indiceBotao)
                return;

            LimparEstadoDoMouse();

            linhaSobOMouse = argumentos.RowIndex;
            botaoSobOMouse = indiceBotao;

            grade.Cursor = indiceBotao >= 0 ? Cursors.Hand : Cursors.Default;
            grade.InvalidateCell(argumentos.ColumnIndex, argumentos.RowIndex);
        }

        // Limpa o estado do mouse quando ele sai da célula
        private void AoSairDaCelula(object remetente, DataGridViewCellEventArgs argumentos)
        {
            if (argumentos.RowIndex == linhaSobOMouse && EhColunaDeAcoes(argumentos.ColumnIndex))
                LimparEstadoDoMouse();
        }

        // Registra o botão de ação pressionado pelo mouse
        private void AoPressionarBotaoDoMouse(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            if (!EhColunaDeAcoes(argumentos.ColumnIndex) || argumentos.RowIndex < 0)
                return;

            int indiceBotao = ObterIndiceDoBotao(argumentos.ColumnIndex, argumentos.RowIndex);
            if (indiceBotao < 0)
                return;

            linhaPressionada = argumentos.RowIndex;
            botaoPressionado = indiceBotao;

            grade.InvalidateCell(argumentos.ColumnIndex, argumentos.RowIndex);
            grade.Cursor = Cursors.Hand;
        }

        // Limpa o estado do botão de ação quando o mouse é solto
        private void AoSoltarBotaoDoMouse(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            int indiceLinha = linhaPressionada;

            linhaPressionada = -1;
            botaoPressionado = -1;

            if (EhLinhaValida(indiceLinha) && grade.Columns.Contains("colunaAcoes"))
                grade.InvalidateCell(grade.Columns["colunaAcoes"].Index, indiceLinha);
        }

        // Identifica qual botão recebeu a posição atual do mouse
        private int ObterIndiceDoBotao(int indiceColuna, int indiceLinha)
        {
            if (indiceColuna < 0 || indiceLinha < 0)
                return -1;

            Rectangle celula = grade.GetCellDisplayRectangle(indiceColuna, indiceLinha, false);
            Point posicaoDoMouse = grade.PointToClient(Cursor.Position);
            Point posicaoNaCelula = new Point(
                posicaoDoMouse.X - celula.Left,
                posicaoDoMouse.Y - celula.Top);

            return GeometriaBotoesAcaoAluno.ObterIndiceDoBotao(posicaoNaCelula, celula);
        }

        // Limpa o estado do mouse
        private void LimparEstadoDoMouse()
        {
            int linhaAnterior = linhaSobOMouse;

            linhaSobOMouse = -1;
            botaoSobOMouse = -1;

            grade.Cursor = Cursors.Default;

            if (EhLinhaValida(linhaAnterior) && grade.Columns.Contains("colunaAcoes"))
                grade.InvalidateCell(grade.Columns["colunaAcoes"].Index, linhaAnterior);
        }

        // Verifica se coluna de ações
        private bool EhColunaDeAcoes(int indiceColuna)
        {
            return indiceColuna >= 0 && indiceColuna < grade.Columns.Count &&
                grade.Columns[indiceColuna].Name == "colunaAcoes";
        }

        // Verifica se linha valida
        private bool EhLinhaValida(int indiceLinha)
        {
            return indiceLinha >= 0 && indiceLinha < grade.Rows.Count;
        }
    }
}