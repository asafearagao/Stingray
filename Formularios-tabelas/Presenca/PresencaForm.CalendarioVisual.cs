using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray
{
    public partial class PresencaForm
    {
        private void CalendarioButton_Paint(object sender, PaintEventArgs e) {
            Button botao = sender as Button;
            if (botao == null)
                return;

            bool hover = botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position));
            Color cor = hover
                ? Color.FromArgb(29, 78, 216)
                : Color.FromArgb(55, 96, 160);
            DesenharIconeCalendario(e.Graphics, botao.ClientRectangle, cor);
        }

        private static void DesenharIconeCalendario(Graphics graphics, Rectangle bounds, Color color) {
            float tamanho = Math.Min(18F, Math.Min(bounds.Width - 4F, bounds.Height - 4F));
            if (tamanho < 12F) return;

            float escala = tamanho / 18F;
            float x = bounds.Left + (bounds.Width - tamanho) / 2F;
            float y = bounds.Top + (bounds.Height - tamanho) / 2F;
            SmoothingMode oldSmoothing = graphics.SmoothingMode;
            PixelOffsetMode oldPixelOffset = graphics.PixelOffsetMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            try {
                using (Pen pen = new Pen(color, Math.Max(1F, 1.25F * escala)))
                using (Pen gridPen = new Pen(Color.FromArgb(115, color), Math.Max(0.85F, 0.95F * escala)))
                using (Brush dots = new SolidBrush(color))
                using (Brush headerFill = new SolidBrush(Color.FromArgb(235, 242, 255))) {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    gridPen.StartCap = LineCap.Round;
                    gridPen.EndCap = LineCap.Round;

                    RectangleF body = new RectangleF(x + 2.7F * escala, y + 4.3F * escala, 12.6F * escala, 12.2F * escala);
                    using (GraphicsPath path = CriarRetanguloArredondado(Rectangle.Round(body), Math.Max(1.5F, 2F * escala)))
                        graphics.DrawPath(pen, path);

                    graphics.FillRectangle(headerFill, x + 3.6F * escala, y + 5.2F * escala, 10.8F * escala, 2.5F * escala);
                    graphics.DrawLine(gridPen, x + 3.5F * escala, y + 8.4F * escala, x + 14.5F * escala, y + 8.4F * escala);
                    graphics.DrawLine(pen, x + 6.1F * escala, y + 2.2F * escala, x + 6.1F * escala, y + 5.5F * escala);
                    graphics.DrawLine(pen, x + 11.9F * escala, y + 2.2F * escala, x + 11.9F * escala, y + 5.5F * escala);

                    float d = 1.35F * escala;
                    float[,] dias = new float[,] {
                        { 5.4F, 10.2F }, { 9.0F, 10.2F }, { 12.0F, 10.2F },
                        { 5.4F, 13.1F }, { 9.0F, 13.1F }, { 12.0F, 13.1F }
                    };
                    for (int i = 0; i < dias.GetLength(0); i++)
                        graphics.FillEllipse(dots, x + dias[i, 0] * escala, y + dias[i, 1] * escala, d, d);
                }

            } finally {
                graphics.SmoothingMode = oldSmoothing;
                graphics.PixelOffsetMode = oldPixelOffset;
            }
        }

        // Cria o caminho gráfico com cantos arredondados
        private static GraphicsPath CriarRetanguloArredondado(Rectangle rect, float raio) {
            GraphicsPath path = new GraphicsPath();
            float diametro = raio * 2F;
            path.AddArc(rect.X, rect.Y, diametro, diametro, 180F, 90F);
            path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270F, 90F);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0F, 90F);
            path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90F, 90F);
            path.CloseFigure();
            return path;
        }

    }
}