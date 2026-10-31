using System;
using System.Drawing;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal static class GeometriaBotoesAcaoAluno
    {
        private const int TamanhoBotao = 24;
        private const int EspacoEntreBotoes = 4;
        private const int QuantidadeDeBotoes = 3;

        public static Rectangle[] ObterRetangulos(Rectangle celula)
        {
            int larguraTotal = TamanhoBotao * QuantidadeDeBotoes +
                EspacoEntreBotoes * (QuantidadeDeBotoes - 1);
            int posicaoX = celula.Left + Math.Max(2, (celula.Width - larguraTotal) / 2);
            int posicaoY = celula.Top + Math.Max(2, (celula.Height - TamanhoBotao) / 2);

            return new Rectangle[]
            {
                new Rectangle(posicaoX, posicaoY, TamanhoBotao, TamanhoBotao),
                new Rectangle(posicaoX + TamanhoBotao + EspacoEntreBotoes, posicaoY, TamanhoBotao, TamanhoBotao),
                new Rectangle(posicaoX + (TamanhoBotao + EspacoEntreBotoes) * 2, posicaoY, TamanhoBotao, TamanhoBotao)
            };
        }

        // Identifica qual botão está na posição informada
        public static int ObterIndiceDoBotao(Point posicao, Rectangle celula)
        {
            Rectangle[] botoes = ObterRetangulos(new Rectangle(0, 0, celula.Width, celula.Height));
            for (int indice = 0; indice < botoes.Length; indice++)
            {
                if (botoes[indice].Contains(posicao))
                    return indice;
            }

            return -1;
        }
    }
}