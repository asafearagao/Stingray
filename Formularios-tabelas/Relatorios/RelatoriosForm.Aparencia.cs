using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray
{
    public partial class RelatoriosForm
    {
        // Configura a aparência dos componentes da tela
        private void ConfigurarAparencia()
        {
            panelPeriodoRelatorio.BorderStyle = BorderStyle.None;
            panelPeriodoRelatorio.Paint += PeriodoRelatorio_Paint;
        }

        // Desenha a borda do campo de período
        private void PeriodoRelatorio_Paint(object sender, PaintEventArgs e)
        {
            Panel painel = sender as Panel;
            if (painel == null || painel.Width <= 1 || painel.Height <= 1)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, painel.Width - 1, painel.Height - 1);
            using (GraphicsPath caminho = CriarCaminhoArredondado(area, 5))
            using (SolidBrush fundo = new SolidBrush(Color.White))
            using (Pen borda = new Pen(VisualPadrao.BordaCampo, 1F))
            {
                e.Graphics.FillPath(fundo, caminho);
                e.Graphics.DrawPath(borda, caminho);
            }
        }

        // Desenha o calendário de seleção do período
        private void CalendarioPeriodo_Paint(object sender, PaintEventArgs e)
        {
            Button botao = sender as Button;
            if (botao == null)
                return;

            bool mouseSobreBotao = botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position));
            Color cor = mouseSobreBotao
                ? Color.FromArgb(29, 78, 216)
                : Color.FromArgb(55, 96, 160);
            Graphics grafico = e.Graphics;
            SmoothingMode suavizacaoAnterior = grafico.SmoothingMode;
            PixelOffsetMode alinhamentoAnterior = grafico.PixelOffsetMode;
            grafico.SmoothingMode = SmoothingMode.AntiAlias;
            grafico.PixelOffsetMode = PixelOffsetMode.HighQuality;

            try
            {
                float tamanho = Math.Min(18F,
                    Math.Min(botao.ClientSize.Width - 4F, botao.ClientSize.Height - 4F));
                if (tamanho < 12F)
                    return;

                float escala = tamanho / 18F;
                float posicaoX = (botao.ClientSize.Width - tamanho) / 2F;
                float posicaoY = (botao.ClientSize.Height - tamanho) / 2F;
                using (Pen caneta = new Pen(cor, Math.Max(1F, 1.25F * escala)))
                using (Pen linhasInternas = new Pen(Color.FromArgb(115, cor), Math.Max(0.85F, 0.95F * escala)))
                using (Brush pontos = new SolidBrush(cor))
                using (Brush cabecalho = new SolidBrush(Color.FromArgb(235, 242, 255)))
                {
                    caneta.StartCap = LineCap.Round;
                    caneta.EndCap = LineCap.Round;
                    linhasInternas.StartCap = LineCap.Round;
                    linhasInternas.EndCap = LineCap.Round;

                    RectangleF corpo = new RectangleF(
                        posicaoX + 2.7F * escala,
                        posicaoY + 4.3F * escala,
                        12.6F * escala,
                        12.2F * escala);
                    using (GraphicsPath caminho = CriarCaminhoArredondado(
                        Rectangle.Round(corpo), Math.Max(1, (int)(2F * escala))))
                    {
                        grafico.DrawPath(caneta, caminho);
                    }

                    grafico.FillRectangle(cabecalho,
                        posicaoX + 3.6F * escala, posicaoY + 5.2F * escala,
                        10.8F * escala, 2.5F * escala);
                    grafico.DrawLine(linhasInternas,
                        posicaoX + 3.5F * escala, posicaoY + 8.4F * escala,
                        posicaoX + 14.5F * escala, posicaoY + 8.4F * escala);
                    grafico.DrawLine(caneta,
                        posicaoX + 6.1F * escala, posicaoY + 2.2F * escala,
                        posicaoX + 6.1F * escala, posicaoY + 5.5F * escala);
                    grafico.DrawLine(caneta,
                        posicaoX + 11.9F * escala, posicaoY + 2.2F * escala,
                        posicaoX + 11.9F * escala, posicaoY + 5.5F * escala);

                    float[,] posicoesDias =
                    {
                        { 5.4F, 10.2F }, { 9.0F, 10.2F }, { 12.0F, 10.2F },
                        { 5.4F, 13.1F }, { 9.0F, 13.1F }, { 12.0F, 13.1F }
                    };
                    float diametroPonto = 1.35F * escala;
                    for (int i = 0; i < posicoesDias.GetLength(0); i++)
                    {
                        grafico.FillEllipse(pontos,
                            posicaoX + posicoesDias[i, 0] * escala,
                            posicaoY + posicoesDias[i, 1] * escala,
                            diametroPonto,
                            diametroPonto);
                    }
                }
            }
            finally
            {
                grafico.SmoothingMode = suavizacaoAnterior;
                grafico.PixelOffsetMode = alinhamentoAnterior;
            }
        }

        private sealed class PainelComBorda : Panel
        {
            public Color CorBorda { get; set; }
            public int RaioCanto { get; set; }

            // Desenho de borda do painel com cantos arredondados
            public PainelComBorda()
            {
                CorBorda = Color.FromArgb(229, 234, 241);
                RaioCanto = 7;
                DoubleBuffered = true;
                ResizeRedraw = true;
                Padding = new Padding(1);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0,
                    Math.Max(0, Width - 1), Math.Max(0, Height - 1));
                using (GraphicsPath caminho = CriarCaminhoArredondado(area, RaioCanto))
                using (Pen borda = new Pen(CorBorda, 1F))
                    e.Graphics.DrawPath(borda, caminho);
            }
        }

        private sealed class SeloIcone : Panel
        {
            public Color CorDestaque { get; set; }
            public Color CorFundo { get; set; }
            public string TipoIcone { get; set; }

            // Inicializa o componente utilizado para exibir um ícone
            public SeloIcone()
            {
                DoubleBuffered = true;
                CorDestaque = Color.FromArgb(22, 163, 74);
                CorFundo = Color.FromArgb(226, 248, 235);
                TipoIcone = "dinheiro";
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics grafico = e.Graphics;
                grafico.SmoothingMode = SmoothingMode.AntiAlias;
                float diametro = Math.Min(Width, Height) - 2;
                RectangleF circulo = new RectangleF(
                    (Width - diametro) / 2, (Height - diametro) / 2, diametro, diametro);

                using (SolidBrush pincelFundo = new SolidBrush(CorFundo))
                    grafico.FillEllipse(pincelFundo, circulo);

                using (Pen caneta = new Pen(CorDestaque, 1.7F))
                {
                    if (TipoIcone == "dinheiro")
                    {
                        RectangleF moeda = new RectangleF(
                            Width / 2F - 8, Height / 2F - 8, 16, 16);
                        grafico.DrawEllipse(caneta, moeda);
                        using (Font fonte = new Font("Segoe UI Semibold", 9F,
                            FontStyle.Regular, GraphicsUnit.Point))
                        using (SolidBrush pincel = new SolidBrush(CorDestaque))
                        {
                            grafico.DrawString("$", fonte, pincel,
                                new PointF(Width / 2F - 4.7F, Height / 2F - 7.5F));
                        }
                    }
                    else if (TipoIcone == "pessoas")
                    {
                        caneta.Width = 1.8F;
                        caneta.StartCap = LineCap.Round;
                        caneta.EndCap = LineCap.Round;
                        caneta.LineJoin = LineJoin.Round;
                        float centroX = Width / 2F + 0.5F;
                        float centroY = Height / 2F + 0.5F;

                        grafico.DrawEllipse(caneta,
                            centroX - 4F, centroY - 8.5F, 8F, 8F);

                        using (GraphicsPath ombros = new GraphicsPath())
                        {
                            ombros.StartFigure();
                            ombros.AddBezier(
                                new PointF(centroX - 9.5F, centroY + 7.5F),
                                new PointF(centroX - 9F, centroY + 2.5F),
                                new PointF(centroX - 5.5F, centroY + 1F),
                                new PointF(centroX, centroY + 1F));
                            ombros.AddBezier(
                                new PointF(centroX, centroY + 1F),
                                new PointF(centroX + 5.5F, centroY + 1F),
                                new PointF(centroX + 9F, centroY + 2.5F),
                                new PointF(centroX + 9.5F, centroY + 7.5F));
                            grafico.DrawPath(caneta, ombros);
                        }
                    }
                    else
                    {
                        PointF[] pontosChapeu =
                        {
                            new PointF(Width / 2F, Height / 2F - 10),
                            new PointF(Width / 2F + 10, Height / 2F - 4),
                            new PointF(Width / 2F, Height / 2F + 2),
                            new PointF(Width / 2F - 10, Height / 2F - 4)
                        };
                        grafico.DrawPolygon(caneta, pontosChapeu);
                        grafico.DrawLine(caneta,
                            Width / 2F + 7, Height / 2F - 1,
                            Width / 2F + 7, Height / 2F + 6);
                        grafico.DrawArc(caneta,
                            Width / 2F - 8, Height / 2F + 2, 16, 13, 185, 170);
                    }
                }
            }
        }

        // Cria o caminho com cantos arredondados
        private static GraphicsPath CriarCaminhoArredondado(Rectangle limites, int raio)
        {
            GraphicsPath caminho = new GraphicsPath();
            if (limites.Width <= 0 || limites.Height <= 0)
                return caminho;

            int diametro = Math.Max(1,
                Math.Min(raio * 2, Math.Min(limites.Width, limites.Height)));
            Rectangle arco = new Rectangle(limites.X, limites.Y, diametro, diametro);
            caminho.AddArc(arco, 180, 90);
            arco.X = limites.Right - diametro;
            caminho.AddArc(arco, 270, 90);
            arco.Y = limites.Bottom - diametro;
            caminho.AddArc(arco, 0, 90);
            arco.X = limites.X;
            caminho.AddArc(arco, 90, 90);
            caminho.CloseFigure();
            return caminho;
        }
    }
}