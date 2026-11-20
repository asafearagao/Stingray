using System;
using System.Windows.Forms;
using Stingray.Formularios;
using Stingray.Servicos;

namespace Stingray {

    // Inicializa os serviços e abre a tela inicial do Stingray
    internal static class Program {
        [STAThread]

        // Prepara os serviços essenciais e inicia a aplicação WinForms
        private static void Main()
        {
            // Configura a aparência padrão dos controles WinForms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try {

                var autenticacao = new AutenticacaoServico();
                string mensagemConexao;

                // Verifica a conexão com o banco de dados
                if (!autenticacao.TestarConexao(out mensagemConexao))
                {
                    MessageBox.Show(
                        mensagemConexao + Environment.NewLine + Environment.NewLine +
                        "Verifique se o serviço StingrayMySQL está em execução e se a credencial do banco foi configurada para esta conta do Windows.",
                        "Banco de dados indisponível",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                WhatsAppServico.Inicializar();
                Application.ApplicationExit += delegate
                {
                    try { WhatsAppServico.EncerrarAsync().GetAwaiter().GetResult(); }
                    catch { }
                };

                // Na primeira execução, cria a primeira conta
                if (!autenticacao.ExisteUsuarioCadastrado())
                {
                    using (var primeiroAcesso = new ContaAcessoForm(true, null))
                    {
                        if (primeiroAcesso.ShowDialog() != DialogResult.OK)
                            return;
                    }
                }

                Application.Run(new LoginForm());
            }
            catch (Exception erro) // Popup para erro das funções acima
            {
                MessageBox.Show(
                    "Não foi possível iniciar o Stingray." + Environment.NewLine + erro.Message,
                    "Falha na inicialização",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
