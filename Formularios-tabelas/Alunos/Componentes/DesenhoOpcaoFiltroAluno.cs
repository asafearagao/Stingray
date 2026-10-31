using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal static class DesenhoOpcaoFiltroAluno
    {
        // Desenha a seta que indica a abertura da lista de opções
        public static void DesenharSeta(Button seta, PaintEventArgs argumentos)
        {
            Rectangle area = seta.ClientRectangle;
            if (area.Width < 8 || area.Height < 8)
                return;

            Color cor = area.Contains(seta.PointToClient(Cursor.Position))
                ? Color.FromArgb(37, 99, 235)
                : Color.FromArgb(100, 116, 139);

            argumentos.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            DesenharSeparador(seta, argumentos.Graphics);
            DesenharIndicadorDeAbertura(area, cor, argumentos.Graphics);
        }

        // Desenha um item da lista conforme seu estado de seleção
        public static void DesenharItem(ComboBox lista, DrawItemEventArgs argumentos)
        {
            if (lista == null || argumentos.Bounds.Width <= 0 || argumentos.Bounds.Height <= 0)
                return;

            bool selecionado = lista.DroppedDown && argumentos.Index >= 0 &&
                (argumentos.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color corFundo = selecionado ? Color.FromArgb(239, 246, 255) : Color.White;

            using (Brush pincel = new SolidBrush(corFundo))
                argumentos.Graphics.FillRectangle(pincel, argumentos.Bounds);

            string texto = ObterTextoDoItem(lista, argumentos.Index);
            TextRenderer.DrawText(
                argumentos.Graphics,
                texto,
                lista.Font,
                Rectangle.Inflate(argumentos.Bounds, -6, 0),
                VisualPadrao.TextoPrincipal,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        // Desenha a linha divisória entre o campo e o botão de seta
        private static void DesenharSeparador(Button seta, Graphics grafico)
        {
            using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                grafico.DrawLine(separador, 0, 1, 0, seta.ClientSize.Height - 2);
        }

        private static void DesenharIndicadorDeAbertura(Rectangle area, Color cor, Graphics grafico)
        {
            float centroX = area.Width / 2F;
            float centroY = area.Height / 2F;

            using (Pen caneta = new Pen(cor, 1.6F))
            {
                caneta.StartCap = LineCap.Round;
                caneta.EndCap = LineCap.Round;
                grafico.DrawLines(caneta, new PointF[]
                {
                    new PointF(centroX - 3.5F, centroY - 1.5F),
                    new PointF(centroX, centroY + 2F),
                    new PointF(centroX + 3.5F, centroY - 1.5F)
                });
            }
        }

        private static string ObterTextoDoItem(ComboBox lista, int indice)
        {
            if (indice >= 0 && indice < lista.Items.Count)
                return Convert.ToString(lista.Items[indice]);

            return lista.Text;
        }
    }
}