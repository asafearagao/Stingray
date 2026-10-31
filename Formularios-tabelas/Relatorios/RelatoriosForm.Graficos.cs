using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray
{
    public partial class RelatoriosForm
    {
        private sealed class PainelGraficoPresenca : Panel
        {
            private readonly Color corPresentes = Color.FromArgb(22, 163, 74);
            private readonly Color corAusentes = Color.FromArgb(239, 68, 68);
            private readonly Color corJustificados = Color.FromArgb(245, 158, 11);
            private float[] valores = new float[0];
            private string[] titulos = new string[0];
            private string[] detalhes = new string[0];
            private string textoTotal = string.Empty;

            // Inicializa o painel de presença
            public PainelGraficoPresenca()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            // Define os dados utilizados na renderização
            public void DefinirDados(float[] percentuais, string[] nomes, string[] descricoes, string total)
            {
                valores = percentuais;
                titulos = nomes;
                detalhes = descricoes;
                textoTotal = total;
                Invalidate();
            }

            // Desenha o gráfico
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics grafico = e.Graphics;
                grafico.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                grafico.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int diametroGrafico = Math.Max(54, Math.Min(136, Math.Min(Height - 35, Width / 3)));
                int posicaoX = 15;
                int posicaoY = Math.Max(8, (Height - diametroGrafico) / 2 - 1);
                Rectangle circulo = new Rectangle(posicaoX, posicaoY, diametroGrafico, diametroGrafico);
                float anguloInicial = -90F;
                Color[] cores = { corPresentes, corAusentes, corJustificados };
                if (valores.Length == 0 || valores.Length > cores.Length ||
                    titulos.Length < valores.Length || detalhes.Length < valores.Length)
                    return;

                for (int i = 0; i < valores.Length; i++)
                {
                    float angulo = valores[i] / 100F * 360F;
                    using (SolidBrush pincel = new SolidBrush(cores[i]))
                        grafico.FillPie(pincel, circulo, anguloInicial, angulo);
                    anguloInicial += angulo;
                }

                int diametroCentro = (int)(diametroGrafico * 0.61F);
                Rectangle centro = new Rectangle(posicaoX + (diametroGrafico - diametroCentro) / 2,
                    posicaoY + (diametroGrafico - diametroCentro) / 2, diametroCentro, diametroCentro);
                using (SolidBrush pincel = new SolidBrush(Color.White))
                    grafico.FillEllipse(pincel, centro);

                using (Font fonte = new Font("Segoe UI Semibold", Math.Max(10F, diametroGrafico / 8.3F),
                    FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush pincel = new SolidBrush(corPresentes))
                {
                    string textoCentro = valores.Length > 0 ? valores[0].ToString("0") + "%" : string.Empty;
                    SizeF tamanhoTextoCentro = grafico.MeasureString(textoCentro, fonte);
                    grafico.DrawString(textoCentro, fonte, pincel,
                        centro.Left + (centro.Width - tamanhoTextoCentro.Width) / 2,
                        centro.Top + (centro.Height - tamanhoTextoCentro.Height) / 2 - 1);
                }

                int legendaX = posicaoX + diametroGrafico + 16;
                int larguraLegenda = Math.Max(100, Width - legendaX - 8);
                int quantidadeItens = Math.Max(1, valores.Length);
                int alturaItem = Math.Max(32, Math.Min(43, (Height - 20) / quantidadeItens));
                int yLegenda = Math.Max(10, (Height - alturaItem * quantidadeItens - 14) / 2);

                for (int i = 0; i < quantidadeItens; i++)
                {
                    DesenharItemLegenda(grafico, legendaX, yLegenda + alturaItem * i,
                        larguraLegenda, cores[i], titulos[i], detalhes[i]);
                }

                using (Font fonteTotal = new Font("Segoe UI", 7.2F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush pincel = new SolidBrush(VisualPadrao.TextoSecundario))
                    grafico.DrawString(textoTotal, fonteTotal, pincel, legendaX,
                        Math.Min(Height - 15, yLegenda + alturaItem * quantidadeItens + 1));
            }

            // Desenha a legenda do gráfico
            private void DesenharItemLegenda(Graphics grafico, int posicaoX, int posicaoY, int largura,
                Color cor, string titulo, string detalhe)
            {
                using (SolidBrush ponto = new SolidBrush(cor))
                    grafico.FillEllipse(ponto, posicaoX, posicaoY + 5, 6, 6);

                using (Font fonteTitulo = new Font("Segoe UI Semibold", 7.7F,
                    FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush pincelTitulo = new SolidBrush(Color.FromArgb(55, 65, 81)))
                    grafico.DrawString(titulo, fonteTitulo, pincelTitulo, posicaoX + 12, posicaoY);

                using (Font fonteDetalhe = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush pincelDetalhe = new SolidBrush(VisualPadrao.TextoSecundario))
                    grafico.DrawString(detalhe, fonteDetalhe, pincelDetalhe, posicaoX + 12, posicaoY + 15,
                        new StringFormat
                        {
                            Trimming = StringTrimming.EllipsisCharacter,
                            FormatFlags = StringFormatFlags.NoWrap
                        });
            }
        }

        private sealed class PainelGraficoReceita : Panel
        {
            private string[] meses = new string[0];
            private double[] valores = new double[0];

            public int QuantidadeMeses { get; set; }

            // Inicializa o painel gráfico de receita
            public PainelGraficoReceita()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
                QuantidadeMeses = 6;
            }

            // Define os dados utilizados na renderização do gráfico
            public void DefinirDados(string[] nomesMeses, double[] receitas)
            {
                meses = nomesMeses;
                valores = receitas;
                Invalidate();
            }

            // Desenha o controle
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics grafico = e.Graphics;
                grafico.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                grafico.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int margemEsquerda = 68;
                int margemDireita = 12;
                int margemSuperior = 27;
                int margemInferior = 32;
                int larguraGrafico = Math.Max(40, Width - margemEsquerda - margemDireita);
                int alturaGrafico = Math.Max(35, Height - margemSuperior - margemInferior);
                double maximo = 25000D;
                if (meses.Length == 0 || valores.Length < meses.Length)
                    return;

                using (Font fonteEixo = new Font("Segoe UI", 6.6F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font fonteValor = new Font("Segoe UI Semibold", 6.7F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font fonteMes = new Font("Segoe UI", 6.8F, FontStyle.Regular, GraphicsUnit.Point))
                using (Pen grade = new Pen(Color.FromArgb(235, 239, 244), 1F))
                using (SolidBrush pincelEixo = new SolidBrush(VisualPadrao.TextoSecundario))
                using (SolidBrush pincelValor = new SolidBrush(VisualPadrao.TextoPrincipal))
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        int posicaoY = margemSuperior + alturaGrafico - (int)Math.Round(alturaGrafico * i / 5D);
                        grafico.DrawLine(grade, margemEsquerda, posicaoY, margemEsquerda + larguraGrafico, posicaoY);
                        if (i == 0)
                            continue;

                        string textoEixo = "R$ " + (i * 5000).ToString("#,##0",
                            CultureInfo.GetCultureInfo("pt-BR"));
                        SizeF tamanhoTextoEixo = grafico.MeasureString(textoEixo, fonteEixo);
                        grafico.DrawString(textoEixo, fonteEixo, pincelEixo,
                            Math.Max(0, margemEsquerda - tamanhoTextoEixo.Width - 7), posicaoY - tamanhoTextoEixo.Height / 2F);
                    }

                    int quantidade = Math.Max(1, Math.Min(meses.Length, Math.Min(6, QuantidadeMeses)));
                    int inicioDados = meses.Length - quantidade;
                    float passo = larguraGrafico / (float)quantidade;
                    float larguraBarra = Math.Min(34F, passo * 0.52F);

                    for (int i = 0; i < quantidade; i++)
                    {
                        int indice = inicioDados + i;
                        float altura = (float)(valores[indice] / maximo * alturaGrafico);
                        float posicaoBarraX = margemEsquerda + passo * i + (passo - larguraBarra) / 2F;
                        float posicaoBarraY = margemSuperior + alturaGrafico - altura;
                        Color corBarra = indice == valores.Length - 1
                            ? Color.FromArgb(54, 99, 214)
                            : Color.FromArgb(158, 194, 244);

                        using (SolidBrush barra = new SolidBrush(corBarra))
                            grafico.FillRectangle(barra, posicaoBarraX, posicaoBarraY, larguraBarra, Math.Max(1, altura));

                        string rotuloValor = "R$ " + valores[indice].ToString("#,##0",
                            CultureInfo.GetCultureInfo("pt-BR"));
                        SizeF tamanhoValor = grafico.MeasureString(rotuloValor, fonteValor);
                        float posicaoValorX = Math.Max(margemEsquerda - 4,
                            Math.Min(posicaoBarraX + larguraBarra / 2F - tamanhoValor.Width / 2F,
                                margemEsquerda + larguraGrafico - tamanhoValor.Width));
                        grafico.DrawString(rotuloValor, fonteValor, pincelValor, posicaoValorX,
                            Math.Max(1, posicaoBarraY - tamanhoValor.Height - 2));

                        SizeF tamanhoMes = grafico.MeasureString(meses[indice], fonteMes);
                        grafico.DrawString(meses[indice], fonteMes, pincelEixo,
                            posicaoBarraX + larguraBarra / 2F - tamanhoMes.Width / 2F,
                            margemSuperior + alturaGrafico + 7);
                    }
                }
            }
        }
    }
}