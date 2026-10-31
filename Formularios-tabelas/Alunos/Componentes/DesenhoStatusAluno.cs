using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal static class DesenhoStatusAluno
    {
        // Desenha o indicador visual de status da linha
        public static void Desenhar(DataGridViewCellPaintingEventArgs argumentos)
        {
            DesenharFundoESeparador(argumentos);

            string status = Convert.ToString(argumentos.FormattedValue) ?? string.Empty;
            AparenciaStatusAluno aparencia = ObterAparencia(status);
            Rectangle etiqueta = ObterAreaDaEtiqueta(argumentos.CellBounds, aparencia.Largura);

            using (Brush pincel = new SolidBrush(aparencia.CorFundo))
                argumentos.Graphics.FillRectangle(pincel, etiqueta);

            DesenharTexto(status, etiqueta, aparencia.CorTexto, argumentos.Graphics);
            argumentos.Handled = true;
        }

        private static void DesenharFundoESeparador(DataGridViewCellPaintingEventArgs argumentos)
        {
            using (Brush fundo = new SolidBrush(Color.White))
                argumentos.Graphics.FillRectangle(fundo, argumentos.CellBounds);

            using (Pen linha = new Pen(Color.FromArgb(235, 239, 245)))
            {
                argumentos.Graphics.DrawLine(
                    linha,
                    argumentos.CellBounds.Left,
                    argumentos.CellBounds.Bottom - 1,
                    argumentos.CellBounds.Right,
                    argumentos.CellBounds.Bottom - 1);
            }
        }

        private static AparenciaStatusAluno ObterAparencia(string status)
        {
            if (status == "Pausada")
                return new AparenciaStatusAluno(Color.FromArgb(255, 244, 217), Color.FromArgb(196, 122, 0), 54);

            if (status == "Cancelada")
                return new AparenciaStatusAluno(Color.FromArgb(254, 228, 226), Color.FromArgb(217, 45, 32), 58);

            return new AparenciaStatusAluno(Color.FromArgb(232, 247, 237), Color.FromArgb(38, 149, 74), 38);
        }

        private static Rectangle ObterAreaDaEtiqueta(Rectangle celula, int largura)
        {
            return new Rectangle(
                celula.Left + (celula.Width - largura) / 2,
                celula.Top + (celula.Height - 22) / 2,
                largura,
                22);
        }

        private static void DesenharTexto(string status, Rectangle etiqueta, Color cor, Graphics grafico)
        {
            using (Font fonte = new Font("Segoe UI", 7.5F))
            {
                TextRenderer.DrawText(
                    grafico,
                    status,
                    fonte,
                    etiqueta,
                    cor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }
    }

    internal sealed class AparenciaStatusAluno
    {
        public Color CorFundo { get; private set; }
        public Color CorTexto { get; private set; }
        public int Largura { get; private set; }

        // Armazena as cores e a largura utilizadas na apresentação do status do aluno
        public AparenciaStatusAluno(Color corFundo, Color corTexto, int largura)
        {
            CorFundo = corFundo;
            CorTexto = corTexto;
            Largura = largura;
        }
    }
}