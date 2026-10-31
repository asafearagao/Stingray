using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    public partial class ConfiguracoesForm {
        private void ConfigurarAparencia() {
            cartaoWhatsApp.BorderStyle = BorderStyle.None;
            cartaoWhatsApp.Paint -= CartaoWhatsApp_Paint;
            cartaoWhatsApp.Paint += CartaoWhatsApp_Paint;
            iconeWhatsApp.Paint += IconeWhatsApp_Paint;
        }

        // Desenha o ícone alternativo do WhatsApp quando a imagem não está disponível
        private void IconeWhatsApp_Paint(object sender, PaintEventArgs e) {
            if (iconeWhatsApp.Image != null)
                return;

            Graphics grafico = e.Graphics;
            grafico.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(2, 2,
                Math.Max(1, iconeWhatsApp.Width - 4), Math.Max(1, iconeWhatsApp.Height - 4));

            using (SolidBrush fundo = new SolidBrush(Color.FromArgb(220, 252, 231)))
                grafico.FillEllipse(fundo, area);

            using (Pen telefone = new Pen(Color.FromArgb(22, 163, 74), 2.1F)) {
                telefone.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                telefone.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                telefone.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                float x = iconeWhatsApp.Width / 2F;
                float y = iconeWhatsApp.Height / 2F;
                grafico.DrawArc(telefone, x - 7F, y - 7F, 14F, 14F, 125F, 105F);
                grafico.DrawLine(telefone, x - 5.8F, y - 5.1F, x - 2.8F, y - 2.4F);
                grafico.DrawLine(telefone, x + 2.5F, y + 2.5F, x + 5.1F, y + 5.1F);
            }
        }

        private void CartaoWhatsApp_Paint(object sender, PaintEventArgs e) {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle area = cartaoWhatsApp.ClientRectangle;
            area.Width -= 1;
            area.Height -= 1;

            using (var caminho = new System.Drawing.Drawing2D.GraphicsPath()) {
                int raio = 8;
                caminho.AddArc(area.X, area.Y, raio, raio, 180, 90);
                caminho.AddArc(area.Right - raio, area.Y, raio, raio, 270, 90);
                caminho.AddArc(area.Right - raio, area.Bottom - raio, raio, raio, 0, 90);
                caminho.AddArc(area.X, area.Bottom - raio, raio, raio, 90, 90);
                caminho.CloseFigure();

                using (var caneta = new Pen(VisualPadrao.BordaCartao, 1F))
                    e.Graphics.DrawPath(caneta, caminho);
            }
        }

        private void PainelConteudo_Paint(object sender, PaintEventArgs e) {
            VisualPadrao.DesenharBordaCartao(e.Graphics, painelConteudo.ClientRectangle);
        }
    }
}