
using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class BotaoLimparFiltrosAlunos
    {
        // Referência ao botão que limpa os filtros
        private readonly RoundedButton botao;

        // Evento quando o botão é clicado
        public event EventHandler Clicado;

        // Inicializa o componente responsável pelo botão de limpeza dos filtros
        public BotaoLimparFiltrosAlunos(RoundedButton botao)
        {
            if (botao == null)
                throw new ArgumentNullException(nameof(botao));

            this.botao = botao;
        }

        public void Configurar(bool emModoDesign) // Configura a aparência e os eventos do botão
        {
            ConfigurarAparencia();
            RegistrarEventosDeAparencia();

            if (!emModoDesign)
                botao.Click += AoClicar;
        }

        private void ConfigurarAparencia() // Define as propriedades visuais padrão do botão
        {
            botao.CornerRadius = 5;
            botao.BackColor = Color.White;
            botao.ForeColor = VisualPadrao.TextoPrincipal;
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            botao.Cursor = Cursors.Hand;
            botao.UseVisualStyleBackColor = false;
        }

        private void RegistrarEventosDeAparencia() // Registra os eventos das alterações visuais do botão
        {
            botao.MouseEnter += AoEntrarComMouse;
            botao.MouseLeave += AoSairComMouse;
            botao.MouseDown += AoPressionar;
            botao.MouseUp += AoSoltar;
        }

        // Trata o clique
        private void AoClicar(object remetente, EventArgs argumentos)
        {
            Clicado?.Invoke(this, EventArgs.Empty);
        }

        // Trata a entrada do mouse em com mouse
        private void AoEntrarComMouse(object remetente, EventArgs argumentos)
        {
            botao.ForeColor = Color.FromArgb(185, 28, 28);
            botao.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
            botao.BackColor = Color.FromArgb(254, 242, 242);
        }

        // Trata a saída do mouse de com mouse
        private void AoSairComMouse(object remetente, EventArgs argumentos)
        {
            botao.ForeColor = VisualPadrao.TextoPrincipal;
            botao.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            botao.BackColor = Color.White;
        }

        // Trata o pressionamento
        private void AoPressionar(object remetente, MouseEventArgs argumentos)
        {
            botao.ForeColor = Color.FromArgb(153, 27, 27);
            botao.BackColor = Color.FromArgb(254, 226, 226);
        }

        // Trata a liberação
        private void AoSoltar(object remetente, MouseEventArgs argumentos)
        {
            if (!botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position)))
                return;

            botao.ForeColor = Color.FromArgb(185, 28, 28);
            botao.BackColor = Color.FromArgb(254, 242, 242);
        }
    }
}