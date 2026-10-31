using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class CampoBuscaAluno
    {
        private const string TextoDeAjuda = "Digite o nome do aluno...";
        private readonly TextBox controle;

        public event EventHandler Alterado;

        // Inicializa o campo de busca
        public CampoBuscaAluno(TextBox controle)
        {
            if (controle == null)
                throw new ArgumentNullException(nameof(controle));

            this.controle = controle;
        }

        public string Texto
        {
            get { return controle.Text.Trim(); }
        }

        // Configura a aparência, o placeholder e o evento do campo de busca
        public void Configurar(bool emModoDesign)
        {
            ConfigurarAparencia();

            if (emModoDesign)
            {
                controle.Text = TextoDeAjuda;
                controle.ForeColor = VisualPadrao.TextoSecundario;
                return;
            }

            controle.Text = string.Empty;
            controle.ForeColor = VisualPadrao.TextoPrincipal;
            PlaceholderTexto.Aplicar(controle, TextoDeAjuda);
            controle.TextChanged += AoAlterarTexto;
        }

        // Limpa o texto do campo de busca
        public void Limpar()
        {
            controle.Clear();
        }

        private void ConfigurarAparencia()
        {
            controle.BorderStyle = BorderStyle.None;
            controle.AutoSize = false;
            controle.Dock = DockStyle.Fill;
            controle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            controle.BackColor = Color.White;
            controle.Margin = new Padding(0);
        }

        private void AoAlterarTexto(object remetente, EventArgs argumentos)
        {
            Alterado?.Invoke(this, EventArgs.Empty);
        }
    }
}