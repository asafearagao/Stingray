using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stingray.Servicos;

namespace Stingray.Formularios
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Controla a autenticação e a entrada no sistema
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            EstiloPopup.AplicarControles(this);
            lblDescricao.Visible = false;
            Shown += delegate { txtUsuario.Focus(); };
            CarregarLogo();
            try { Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
        }

        private void CarregarLogo()
        {
            try
            {
                using (var fluxo = Assembly.GetExecutingAssembly().GetManifestResourceStream("Stingray.Assets.AquaFit.png"))
                {
                    if (fluxo != null)
                    {
                        using (Image imagem = Image.FromStream(fluxo))
                            picLogo.Image = new Bitmap(imagem);
                    }
                }
            }
            catch
            {
                lblTitulo.Text = "AquaFit";
            }
        }

        // Valida as credenciais e inicia a sessão do usuário
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("Preencha o usuário e a senha!");
                return;
            }

            try
            {
                // Testa a conexão com o banco de dados antes de autenticar
                var autenticacao = new AutenticacaoServico();
                string mensagemConexao;
                if (!autenticacao.TestarConexao(out mensagemConexao))
                {
                    MessageBox.Show(mensagemConexao + Environment.NewLine + Environment.NewLine +
                        "Confira se o serviço StingrayMySQL está em execução e se as credenciais do banco foram configuradas para esta instalação.",
                        "Banco de dados indisponível", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tenta autenticar
                var usuario = autenticacao.Autenticar(txtUsuario.Text, txtSenha.Text);
                if (usuario == null)
                {
                    MessageBox.Show("Usuário ou senha incorretos!", "Erro de autenticação",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DashboardForm dashboard = new DashboardForm(usuario);
                dashboard.FormClosed += (s, args) => this.Close();
                dashboard.Show();
                this.Hide();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um erro ao realizar o login: " + erro.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Alterna a visibilidade da senha no campo de entrada
        private bool senhaVisivel = false;

        // Alterna a visibilidade do campo de senha
        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;

            if (senhaVisivel)
            {
                txtSenha.PasswordChar = '\0';
                btnVisualizar.Text = "Ocultar";
            }
            else
            {
                txtSenha.PasswordChar = '•';
                btnVisualizar.Text = "Mostrar";
            }
        }
    }
}
