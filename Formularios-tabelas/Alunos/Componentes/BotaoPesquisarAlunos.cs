
using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class BotaoPesquisarAlunos
    {
        // Referência ao botão de pesquisa
        private readonly RoundedButton botao;

        // Evento quando o botão é clicado
        public event EventHandler Clicado;

        // Inicializa o componente e valida a referência do botão de pesquisa
        public BotaoPesquisarAlunos(RoundedButton botao)
        {
            if (botao == null)
                throw new ArgumentNullException(nameof(botao));

            this.botao = botao;
        }

        // Configura a aparência e o evento de clique do botão de pesquisa
        public void Configurar(bool emModoDesign)
        {
            ConfigurarAparencia();

            if (!emModoDesign)
                botao.Click += AoClicar;
        }

        private void ConfigurarAparencia() // Define as propriedades visuais padrão do botão
        {
            botao.CornerRadius = 5;
            botao.BackColor = VisualPadrao.Azul;
            botao.ForeColor = Color.White;
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            botao.Cursor = Cursors.Hand;
            botao.UseVisualStyleBackColor = false;
        }

        // Trata o clique
        private void AoClicar(object remetente, EventArgs argumentos)
        {
            Clicado?.Invoke(this, EventArgs.Empty);
        }
    }
}