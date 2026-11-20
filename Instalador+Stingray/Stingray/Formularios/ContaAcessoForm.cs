using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.Servicos;

namespace Stingray.Formularios
{
    [DesignerCategory("Form")]

    // Permite alterar as credenciais da conta atualmente utilizada
    public partial class ContaAcessoForm : Form
    {
        private readonly bool primeiroAcesso;
        private readonly Usuario usuarioAtual;
        private readonly AutenticacaoServico autenticacao;

        public Usuario UsuarioAtualizado { get; private set; }

        // Construtor sem parâmetros para o Designer do Windows Forms
        public ContaAcessoForm() : this(true, null) { }

        public ContaAcessoForm(bool primeiroAcesso, Usuario usuarioAtual)
        {
            InitializeComponent();

            this.primeiroAcesso = primeiroAcesso;
            this.usuarioAtual = usuarioAtual;
            autenticacao = new AutenticacaoServico();

            ConfigurarTela();
            EstiloPopup.Aplicar(this);
        }

        // Configura a tela de acordo com o contexto (primeiro acesso ou alteração)
        private void ConfigurarTela()
        {
            if (primeiroAcesso)
            {
                lblTitulo.Text = "Primeiro acesso";
                lblDescricao.Text = "Crie o usuário e a senha que serão usados para entrar no Stingray.";
                lblSenhaAtual.Visible = false;
                txtSenhaAtual.Visible = false;

                // Sem senha atual no primeiro uso, os demais campos sobem um nível
                AjustarPosicao(lblNomeUsuario, 119);
                AjustarPosicao(txtNomeUsuario, 142);
                AjustarPosicao(lblNovaSenha, 204);
                AjustarPosicao(txtNovaSenha, 227);
                AjustarPosicao(lblConfirmarSenha, 289);
                AjustarPosicao(txtConfirmarSenha, 312);
                AjustarPosicao(lblOrientacao, 354);
                AjustarPosicao(btnSalvar, 408);
                AjustarPosicao(btnCancelar, 408);
                ClientSize = new System.Drawing.Size(540, 485);
                btnSalvar.Text = "Criar acesso";
            }
            else
            {
                if (usuarioAtual == null) // Valida se o usuário atual foi fornecido
                    throw new ArgumentNullException(nameof(usuarioAtual));

                lblTitulo.Text = "Alterar usuário e senha";
                lblDescricao.Text = "Confirme sua senha atual e informe as novas credenciais.";
                txtNomeUsuario.Text = usuarioAtual.NomeUsuario;
            }

            AcceptButton = btnSalvar;
            CancelButton = btnCancelar;
        }

        // Ajusta a posição vertical de um controle na tela
        private static void AjustarPosicao(Control controle, int topo)
        {
            controle.Top = topo;
        }

        // Valida as credenciais e solicita a alteração da conta
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nomeUsuario = txtNomeUsuario.Text.Trim();
            string novaSenha = txtNovaSenha.Text;
            string confirmacao = txtConfirmarSenha.Text;

            // Valida os campos obrigatórios e a confirmação da senha
            if (string.IsNullOrWhiteSpace(nomeUsuario))
            {
                MostrarAviso("Informe um nome de usuário.");
                txtNomeUsuario.Focus();
                return;
            }

            if (string.IsNullOrEmpty(novaSenha))
            {
                MostrarAviso("Informe uma senha.");
                txtNovaSenha.Focus();
                return;
            }

            if (!string.Equals(novaSenha, confirmacao, StringComparison.Ordinal))
            {
                MostrarAviso("A confirmação da senha não confere.");
                txtConfirmarSenha.Clear();
                txtConfirmarSenha.Focus();
                return;
            }

            try
            {
                // Cria a primeira conta ou altera as credenciais da conta atual
                if (primeiroAcesso)
                {
                    UsuarioAtualizado = autenticacao.CriarPrimeiroUsuario(nomeUsuario, novaSenha);
                    MessageBox.Show(
                        "A primeira conta foi criada. Agora entre no Stingray com essas credenciais.",
                        "Acesso configurado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    UsuarioAtualizado = autenticacao.AlterarCredenciais(
                        usuarioAtual,
                        txtSenhaAtual.Text,
                        nomeUsuario,
                        novaSenha);

                    MessageBox.Show(
                        "O nome de usuário e a senha foram atualizados.",
                        "Credenciais alteradas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(
                    erro.Message,
                    "Não foi possível salvar as credenciais",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Exibe uma mensagem de aviso para o usuário e explica o erro
        private void MostrarAviso(string mensagem)
        {
            MessageBox.Show(
                mensagem,
                "Verifique os dados",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
