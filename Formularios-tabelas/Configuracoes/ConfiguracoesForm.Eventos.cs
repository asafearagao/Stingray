using System;
using System.Windows.Forms;

namespace Stingray {
    public partial class ConfiguracoesForm {
        // Registra os eventos da tela e inicializa o temporizador de verificação
        private void ConfigurarEventos() {
            btnAbrirWhatsApp.Visible = false;
            chkWhatsAppAtivado.Checked = WhatsAppService.EstaAtivado();

            chkWhatsAppAtivado.CheckedChanged += ChkWhatsAppAtivado_CheckedChanged;
            btnAbrirWhatsApp.Click += BtnAbrirWhatsApp_Click;

            timerEstadoWhatsApp = new Timer();
            timerEstadoWhatsApp.Interval = 5000;
            timerEstadoWhatsApp.Tick += TimerEstadoWhatsApp_Tick;

            Shown += ConfiguracoesForm_Shown;
            painelConteudo.Resize += PainelConteudo_Resize;
            FormClosed += ConfiguracoesForm_FormClosed;

            AtualizarVisualEstado();
        }

        private async void ConfiguracoesForm_Shown(object sender, EventArgs e) {
            timerEstadoWhatsApp.Start();
            _ = CarregarLogoWhatsAppAsync(iconeWhatsApp);
            await AtualizarEstadoWhatsAppAsync();
        }

        // Atualiza periodicamente o estado da conexão
        private async void TimerEstadoWhatsApp_Tick(object sender, EventArgs e) {
            await AtualizarEstadoWhatsAppAsync();

            if (iconeWhatsApp.Image == null && !carregandoLogo &&
                DateTime.Now - ultimaTentativaLogo >= TimeSpan.FromSeconds(30)) {
                ultimaTentativaLogo = DateTime.Now;
                await CarregarLogoWhatsAppAsync(iconeWhatsApp);
            }
        }

        private void PainelConteudo_Resize(object sender, EventArgs e) {
            cartaoWhatsApp.Width = Math.Max(360, (painelConteudo.ClientSize.Width - 72) / 2);
            chkWhatsAppAtivado.Left = cartaoWhatsApp.Width - chkWhatsAppAtivado.Width - 18;
        }

        // Encerra o temporizador ao fechar a tela
        private void ConfiguracoesForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (timerEstadoWhatsApp == null)
                return;

            timerEstadoWhatsApp.Stop();
            timerEstadoWhatsApp.Dispose();
            timerEstadoWhatsApp = null;
        }

        // Ação após ativar ou desativar a integração com o WhatsApp
        private void ChkWhatsAppAtivado_CheckedChanged(object sender, EventArgs e) {
            WhatsAppService.DefinirAtivado(chkWhatsAppAtivado.Checked);
            AtualizarVisualEstado();

            if (chkWhatsAppAtivado.Checked)
                _ = AtualizarEstadoWhatsAppAsync();
        }
    }
}