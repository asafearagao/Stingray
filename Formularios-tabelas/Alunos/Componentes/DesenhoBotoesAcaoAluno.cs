using System.Drawing;
using System.Windows.Forms;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal static class DesenhoBotoesAcaoAluno
    {
        private static readonly string[] Icones = { "\uE721", "\uE70F", "\uE74D" };

        // Desenha os botões de ação dentro da célula da grade
        public static void Desenhar(DataGridViewCellPaintingEventArgs argumentos, AlunosGradeInteracao interacao)
        {
            argumentos.PaintBackground(argumentos.CellBounds, true);

            Rectangle[] botoes = GeometriaBotoesAcaoAluno.ObterRetangulos(
                new Rectangle(0, 0, argumentos.CellBounds.Width, argumentos.CellBounds.Height));

            for (int indice = 0; indice < botoes.Length; indice++)
                DesenharBotao(argumentos, interacao, botoes[indice], indice);

            argumentos.Handled = true;
        }

        // Desenha um botão de ação considerando seu estado visual
        private static void DesenharBotao(
            DataGridViewCellPaintingEventArgs argumentos,
            AlunosGradeInteracao interacao,
            Rectangle area,
            int indice)
        {
            area.Offset(argumentos.CellBounds.Location);

            bool mouseSobre = interacao.MouseEstaSobre(argumentos.RowIndex, indice);
            bool pressionado = interacao.BotaoEstaPressionado(argumentos.RowIndex, indice);

            VisualPadrao.DesenharBotaoAcao(
                argumentos.Graphics,
                area,
                indice == 2,
                indice == 0,
                mouseSobre,
                pressionado,
                Icones[indice]);
        }

    }
}