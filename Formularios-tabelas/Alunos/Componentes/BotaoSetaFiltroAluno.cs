using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class BotaoSetaFiltroAluno
    {
        private readonly Button botao;
        private readonly ComboBox lista;

        // Inicializa o botão que controla a abertura da lista de opções
        public BotaoSetaFiltroAluno(Button botao, ComboBox lista)
        {
            if (botao == null)
                throw new ArgumentNullException(nameof(botao));
            if (lista == null)
                throw new ArgumentNullException(nameof(lista));

            this.botao = botao;
            this.lista = lista;
        }

        // Configura a acessibilidade, a aparência e os eventos do botão de seta
        public void Configurar()
        {
            botao.AccessibleName = "Abrir opções";
            botao.AccessibleDescription = "Abre a lista de opções";
            botao.Cursor = Cursors.Hand;
            botao.Paint += AoDesenhar;
            botao.MouseEnter += AoEntrarComMouse;
            botao.MouseLeave += AoSairComMouse;
            botao.Click += AoClicar;
        }

        // Desenha a seta utilizada para indicar a abertura da lista
        private void AoDesenhar(object remetente, PaintEventArgs argumentos)
        {
            DesenhoOpcaoFiltroAluno.DesenharSeta(botao, argumentos);
        }

        private void AoEntrarComMouse(object remetente, EventArgs argumentos)
        {
            botao.BackColor = Color.FromArgb(239, 246, 255);
            botao.Invalidate();
        }

        private void AoSairComMouse(object remetente, EventArgs argumentos)
        {
            botao.BackColor = Color.White;
            botao.Invalidate();
        }

        private void AoClicar(object remetente, EventArgs argumentos)
        {
            if (lista.IsDisposed)
                return;

            lista.Focus();
            lista.DroppedDown = true;
        }
    }
}