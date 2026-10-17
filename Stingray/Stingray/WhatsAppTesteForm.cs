using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    // Formulário temporário para testar cada operação do serviço.
    public class WhatsAppTesteForm : Form {
        private readonly TextBox txtTelefone;
        private readonly TextBox txtMensagem;
        private readonly TextBox txtLog;
        private readonly Label lblUltimaMensagem;
        private Guid? ultimaMensagem;

        public WhatsAppTesteForm() {
            Text = "Teste temporário - WhatsApp";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(760, 570);
            MinimumSize = new Size(700, 520);
            Font = new Font("Segoe UI", 9F);
            BackColor = Color.FromArgb(247, 249, 252);

            Label titulo = new Label();
            titulo.Text = "WhatsApp Web — testes temporários";
            titulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titulo.ForeColor = Color.FromArgb(17, 24, 39);
            titulo.Location = new Point(20, 18);
            titulo.AutoSize = true;
            Controls.Add(titulo);

            Label lblTelefone = new Label();
            lblTelefone.Text = "Telefone (país + DDD + número)";
            lblTelefone.Location = new Point(22, 66);
            lblTelefone.AutoSize = true;
            Controls.Add(lblTelefone);

            txtTelefone = new TextBox();
            txtTelefone.Location = new Point(22, 88);
            txtTelefone.Width = 300;
            txtTelefone.Text = "5511999999999";
            Controls.Add(txtTelefone);

            Label lblMensagem = new Label();
            lblMensagem.Text = "Mensagem para enviar ou adicionar à fila";
            lblMensagem.Location = new Point(22, 124);
            lblMensagem.AutoSize = true;
            Controls.Add(lblMensagem);

            txtMensagem = new TextBox();
            txtMensagem.Location = new Point(22, 146);
            txtMensagem.Size = new Size(700, 72);
            txtMensagem.Multiline = true;
            txtMensagem.ScrollBars = ScrollBars.Vertical;
            txtMensagem.Text = "Olá! Esta é uma mensagem de teste do Stingray.";
            Controls.Add(txtMensagem);

            FlowLayoutPanel botoes = new FlowLayoutPanel();
            botoes.Location = new Point(22, 232);
            botoes.Size = new Size(700, 92);
            botoes.WrapContents = true;
            botoes.AutoScroll = true;
            Controls.Add(botoes);

            Button btnAbrir = CriarBotao("1. Abrir WhatsApp / QR Code", 190);
            btnAbrir.Click += async delegate {
                Registrar("Abrindo WhatsApp Web...");
                bool abriu = await WhatsAppService.PrepararWhatsAppAsync();
                Registrar(abriu
                    ? "Navegador aberto. Se necessário, leia o QR Code."
                    : "Não foi possível abrir o navegador. Detalhes: " +
                        (String.IsNullOrWhiteSpace(WhatsAppService.UltimoErro)
                            ? "erro não informado"
                            : WhatsAppService.UltimoErro));
            };
            botoes.Controls.Add(btnAbrir);

            Button btnInternet = CriarBotao("2. Verificar internet", 140);
            btnInternet.Click += async delegate {
                bool online = await WhatsAppService.VerificarInternetAsync();
                Registrar(online ? "Internet: conectada." : "Internet: sem conexão. " +
                    WhatsAppService.UltimoErro);
            };
            botoes.Controls.Add(btnInternet);

            Button btnWhatsapp = CriarBotao("3. Verificar sessão", 140);
            btnWhatsapp.Click += async delegate {
                bool conectado = await WhatsAppService.VerificarWhatsAppAsync();
                Registrar(conectado
                    ? "WhatsApp: sessão conectada."
                    : "WhatsApp: sessão não autenticada ou navegador fechado. " +
                        (String.IsNullOrWhiteSpace(WhatsAppService.UltimoErro)
                            ? ""
                            : WhatsAppService.UltimoErro));
            };
            botoes.Controls.Add(btnWhatsapp);

            Button btnEnviar = CriarBotao("4. Enviar (verificar login)", 190);
            btnEnviar.Click += async delegate {
                btnEnviar.Enabled = false;
                try {
                    Registrar("Verificando se o WhatsApp está conectado...");
                    Guid? id = await WhatsAppService.EnviarMensagemAsync(
                        txtTelefone.Text, txtMensagem.Text);

                    if (id.HasValue) {
                        ultimaMensagem = id.Value;
                        lblUltimaMensagem.Text = "Último ID: " + ultimaMensagem.Value;
                        Registrar("WhatsApp conectado. Mensagem adicionada à fila de envio. ID: " + id.Value);
                    } else {
                        string erro = String.IsNullOrWhiteSpace(WhatsAppService.UltimoErro)
                            ? "Não foi possível enviar a mensagem."
                            : WhatsAppService.UltimoErro;
                        Registrar("Envio cancelado: " + erro);
                        MessageBox.Show(this, erro, "WhatsApp não conectado",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } catch (Exception ex) {
                    Registrar("Erro ao enviar: " + ex.Message);
                    MessageBox.Show(this, ex.Message, "Erro ao enviar mensagem",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally {
                    btnEnviar.Enabled = true;
                }
            };
            botoes.Controls.Add(btnEnviar);

            Button btnFila = CriarBotao("5. Só adicionar à fila", 155);
            btnFila.Click += delegate {
                try {
                    ultimaMensagem = WhatsAppService.EnfileirarMensagem(
                        txtTelefone.Text, txtMensagem.Text);
                    lblUltimaMensagem.Text = "Último ID: " + ultimaMensagem.Value;
                    Registrar("Mensagem adicionada à fila. Ela será enviada quando o WhatsApp estiver autenticado. ID: " + ultimaMensagem.Value);
                } catch (Exception ex) {
                    MessageBox.Show(this, ex.Message, "Não foi possível adicionar",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            botoes.Controls.Add(btnFila);

            Button btnEstado = CriarBotao("6. Consultar estado", 140);
            btnEstado.Click += delegate {
                if (!ultimaMensagem.HasValue) {
                    Registrar("Adicione uma mensagem à fila primeiro.");
                    return;
                }
                Registrar("Estado da última mensagem: " +
                    WhatsAppService.ConsultarEstado(ultimaMensagem.Value));
            };
            botoes.Controls.Add(btnEstado);

            Button btnEncerrar = CriarBotao("7. Encerrar serviço", 140);
            btnEncerrar.Click += async delegate {
                await WhatsAppService.EncerrarAsync();
                Registrar("Serviço encerrado e navegador fechado.");
            };
            botoes.Controls.Add(btnEncerrar);

            lblUltimaMensagem = new Label();
            lblUltimaMensagem.Text = "Último ID: nenhum";
            lblUltimaMensagem.Location = new Point(22, 330);
            lblUltimaMensagem.AutoSize = true;
            lblUltimaMensagem.ForeColor = Color.FromArgb(75, 85, 99);
            Controls.Add(lblUltimaMensagem);

            Label lblLog = new Label();
            lblLog.Text = "Log de teste";
            lblLog.Location = new Point(22, 356);
            lblLog.AutoSize = true;
            Controls.Add(lblLog);

            txtLog = new TextBox();
            txtLog.Location = new Point(22, 378);
            txtLog.Size = new Size(700, 135);
            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(txtLog);
        }

        private static Button CriarBotao(string texto, int largura) {
            Button botao = new Button();
            botao.Text = texto;
            botao.Width = largura;
            botao.Height = 34;
            botao.Margin = new Padding(0, 0, 8, 8);
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            botao.BackColor = Color.White;
            botao.ForeColor = Color.FromArgb(55, 65, 81);
            botao.Cursor = Cursors.Hand;
            return botao;
        }

        private void Registrar(string mensagem) {
            txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + "  " +
                mensagem + Environment.NewLine);
        }
    }
}