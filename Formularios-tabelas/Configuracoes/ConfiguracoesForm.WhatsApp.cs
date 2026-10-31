using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stingray {
    public partial class ConfiguracoesForm {
        private async Task AtualizarEstadoWhatsAppAsync() {
            if (verificacaoEmAndamento || IsDisposed || !IsHandleCreated)
                return;

            verificacaoEmAndamento = true;
            try {
                if (!WhatsAppService.EstaAtivado()) {
                    AtualizarVisualEstado();
                    return;
                }

                btnAbrirWhatsApp.Visible = true;

                bool internet = await WhatsAppService.VerificarInternetAsync();
                if (!internet) {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                    // A queda da internet não altera o último estado de autenticação salvo
                    return;
                }

                bool conectadoAgora = await WhatsAppService.VerificarWhatsAppAsync();
                if (WhatsAppService.SessaoConfirmadaAgora && conectadoAgora) {
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "WhatsApp Web autenticado e pronto.";

                } else if (WhatsAppService.SessaoConfirmadaAgora && !conectadoAgora) {
                    // A verificação encontrou a tela de QR Code
                    lblEstadoWhatsApp.Text = "Desconectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(180, 83, 9);
                    lblDescricaoWhatsApp.Text = "Abra o navegador e leia o QR Code pelo celular.";

                } else if (WhatsAppService.ConexaoWhatsApp) {
                    // Preserva o estado salvo durante falhas
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "A sessão será verificada automaticamente.";

                } else {
                    lblEstadoWhatsApp.Text = "Verificando sessão...";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = "A página ainda está carregando ou aguardando autenticação.";
                }

            } catch {
                // Uma falha de consulta não equivale a logout
                if (WhatsAppService.ConexaoInternet) {
                    lblEstadoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                        ? "Conectado"
                        : "Não foi possível verificar";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                        ? "A sessão será verificada automaticamente."
                        : "Abra o WhatsApp Web para autenticar a sessão.";

                } else {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                }

                if (WhatsAppService.EstaAtivado())
                    btnAbrirWhatsApp.Visible = true;

            } finally {
                verificacaoEmAndamento = false;
            }
        }

        private void AtualizarVisualEstado() {
            if (!WhatsAppService.EstaAtivado()) {
                lblEstadoWhatsApp.Text = "Inativo";
                lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                lblDescricaoWhatsApp.Text = "A integração está desligada.";
                btnAbrirWhatsApp.Visible = false;
                return;
            }

            btnAbrirWhatsApp.Visible = true;

            if (!WhatsAppService.ConexaoInternet) {
                lblEstadoWhatsApp.Text = "Verificando conexão...";
                lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                lblDescricaoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                    ? "A sessão será verificada quando a internet voltar."
                    : "Verifique sua conexão com a internet.";

            } else if (!WhatsAppService.SessaoConfirmadaAgora && WhatsAppService.ConexaoWhatsApp) {
                lblEstadoWhatsApp.Text = "Conectado";
                lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                lblDescricaoWhatsApp.Text = "A sessão será verificada automaticamente.";

            } else {
                lblEstadoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                    ? "Conectado"
                    : "Verificando sessão...";
                lblEstadoWhatsApp.ForeColor = WhatsAppService.ConexaoWhatsApp
                    ? Color.FromArgb(22, 101, 52)
                    : Color.FromArgb(107, 114, 128);
                lblDescricaoWhatsApp.Text = "Abra o WhatsApp Web para acessar a sessão.";
            }
        }

        private async void BtnAbrirWhatsApp_Click(object sender, EventArgs e) {
            btnAbrirWhatsApp.Enabled = false;
            btnAbrirWhatsApp.Text = "Abrindo navegador...";

            try {
                bool abriu = await WhatsAppService.PrepararWhatsAppAsync();
                if (!abriu) {
                    MessageBox.Show(this,
                        "Não foi possível abrir o WhatsApp Web.\r\n\r\n" +
                        (String.IsNullOrWhiteSpace(WhatsAppService.UltimoErro)
                            ? "Nenhum detalhe de erro foi informado."
                            : WhatsAppService.UltimoErro),
                        "WhatsApp Web", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } catch (Exception ex) {
                MessageBox.Show(this, ex.ToString(), "WhatsApp Web",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            } finally {
                if (!IsDisposed) {
                    btnAbrirWhatsApp.Enabled = true;
                    btnAbrirWhatsApp.Text = "Abrir WhatsApp Web";
                    await AtualizarEstadoWhatsAppAsync();
                }
            }
        }
    }
}