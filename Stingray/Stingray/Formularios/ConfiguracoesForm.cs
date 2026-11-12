using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class ConfiguracoesForm : Form {
        private Panel cartaoWhatsApp;
        private ToggleSwitch chkWhatsAppAtivado;
        private Label lblEstadoWhatsApp;
        private Label lblDescricaoWhatsApp;
        private RoundedButton btnAbrirWhatsApp;
        private PictureBox iconeWhatsApp;
        private Timer timerEstadoWhatsApp;
        private bool verificacaoEmAndamento;
        private bool carregandoLogo;
        private DateTime ultimaTentativaLogo = DateTime.MinValue;

        public ConfiguracoesForm() {
            InitializeComponent();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            cartaoWhatsApp.BorderStyle = BorderStyle.None;
            cartaoWhatsApp.Paint -= CartaoWhatsApp_Paint;
            cartaoWhatsApp.Paint += CartaoWhatsApp_Paint;
            iconeWhatsApp.Paint += IconeWhatsApp_Paint;

            if (emDesignTime) {
                lblEstadoWhatsApp.Text = "Pré-visualização";
                lblDescricaoWhatsApp.Text = "A integração será verificada durante a execução do aplicativo.";
                btnAbrirWhatsApp.Visible = true;
                return;
            }

            btnAbrirWhatsApp.Visible = false;
            chkWhatsAppAtivado.Checked = WhatsAppService.EstaAtivado();
            chkWhatsAppAtivado.CheckedChanged += ChkWhatsAppAtivado_CheckedChanged;
            btnAbrirWhatsApp.Click += BtnAbrirWhatsApp_Click;
            _ = CarregarLogoWhatsAppAsync(iconeWhatsApp);

            timerEstadoWhatsApp = new Timer();
            timerEstadoWhatsApp.Interval = 5000;
            timerEstadoWhatsApp.Tick += async delegate {
                await AtualizarEstadoWhatsAppAsync();

                if (iconeWhatsApp.Image == null && !carregandoLogo &&
                    DateTime.Now - ultimaTentativaLogo >= TimeSpan.FromSeconds(30)) {
                    ultimaTentativaLogo = DateTime.Now;
                    await CarregarLogoWhatsAppAsync(iconeWhatsApp);
                }
            };
            timerEstadoWhatsApp.Start();
            AtualizarVisualEstado();

            painelConteudo.Resize += delegate {
                cartaoWhatsApp.Width = Math.Max(360, (painelConteudo.ClientSize.Width - 72) / 2);
                chkWhatsAppAtivado.Left = cartaoWhatsApp.Width - chkWhatsAppAtivado.Width - 18;
            };
            FormClosed += delegate {
                if (timerEstadoWhatsApp != null) {
                    timerEstadoWhatsApp.Stop();
                    timerEstadoWhatsApp.Dispose();
                }
            };
        }

        private void IconeWhatsApp_Paint(object sender, PaintEventArgs e) {
            if (iconeWhatsApp.Image != null)
                return;

            Graphics grafico = e.Graphics;
            grafico.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(2, 2, Math.Max(1, iconeWhatsApp.Width - 4), Math.Max(1, iconeWhatsApp.Height - 4));
            using (SolidBrush fundo = new SolidBrush(Color.FromArgb(220, 252, 231)))
                grafico.FillEllipse(fundo, area);

            using (Pen telefone = new Pen(Color.FromArgb(22, 163, 74), 2.1F)) {
                telefone.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                telefone.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                telefone.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                float x = iconeWhatsApp.Width / 2F;
                float y = iconeWhatsApp.Height / 2F;
                grafico.DrawArc(telefone, x - 7F, y - 7F, 14F, 14F, 125F, 105F);
                grafico.DrawLine(telefone, x - 5.8F, y - 5.1F, x - 2.8F, y - 2.4F);
                grafico.DrawLine(telefone, x + 2.5F, y + 2.5F, x + 5.1F, y + 5.1F);
            }
        }

        private void CartaoWhatsApp_Paint(object sender, PaintEventArgs e) {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle area = cartaoWhatsApp.ClientRectangle;
            area.Width -= 1;
            area.Height -= 1;
            using (var path = new System.Drawing.Drawing2D.GraphicsPath()) {
                int raio = 8;
                path.AddArc(area.X, area.Y, raio, raio, 180, 90);
                path.AddArc(area.Right - raio, area.Y, raio, raio, 270, 90);
                path.AddArc(area.Right - raio, area.Bottom - raio, raio, raio, 0, 90);
                path.AddArc(area.X, area.Bottom - raio, raio, raio, 90, 90);
                path.CloseFigure();
                using (var pen = new Pen(VisualPadrao.BordaCartao, 1F))
                    e.Graphics.DrawPath(pen, path);
            }
        }

        private void ChkWhatsAppAtivado_CheckedChanged(object sender, EventArgs e) {
            WhatsAppService.DefinirAtivado(chkWhatsAppAtivado.Checked);
            AtualizarVisualEstado();
            if (chkWhatsAppAtivado.Checked)
                _ = AtualizarEstadoWhatsAppAsync();
        }

        // Consulta a conexão sem alterar a fila de mensagens
        private async System.Threading.Tasks.Task AtualizarEstadoWhatsAppAsync() {
            if (verificacaoEmAndamento || IsDisposed || !IsHandleCreated)
                return;

            verificacaoEmAndamento = true;
            try {
                if (!WhatsAppService.EstaAtivado()) {
                    AtualizarVisualEstado();
                    return;
                }

                // Mantém o único botão de ação visível enquanto a integração estiver ativa
                btnAbrirWhatsApp.Visible = true;

                bool internet = await WhatsAppService.VerificarInternetAsync();
                if (!internet) {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                    // Não alteramos o estado de autenticação salvo só porque a internet caiu
                    return;
                }

                bool conectadoAgora = await WhatsAppService.VerificarWhatsAppAsync();
                if (WhatsAppService.SessaoConfirmadaAgora && conectadoAgora) {
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "WhatsApp Web autenticado e pronto.";

                } else if (WhatsAppService.SessaoConfirmadaAgora && !conectadoAgora) {
                    // A verificação encontrou claramente a tela de QR Code/login
                    lblEstadoWhatsApp.Text = "Desconectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(180, 83, 9);
                    lblDescricaoWhatsApp.Text = "Abra o navegador e leia o QR Code pelo celular.";

                } else if (WhatsAppService.ConexaoWhatsApp) {
                    // Preserva o estado salvo durante falhas transitórias. A sessão é
                    // confirmada novamente pelo navegador assim que ele responder
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "A sessão será verificada automaticamente.";

                } else {
                    lblEstadoWhatsApp.Text = "Verificando sessão...";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = "A página ainda está carregando ou aguardando autenticação.";
                }

            } catch {
                // Uma falha de consulta não equivale a logout. Preserva o estado salvo
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

        // Atualiza a aparência conforme o estado da integração
        private void AtualizarVisualEstado() {
            if (!WhatsAppService.EstaAtivado()) {
                lblEstadoWhatsApp.Text = "Inativo";
                lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                lblDescricaoWhatsApp.Text = "A integração está desligada.";
                btnAbrirWhatsApp.Visible = false;

            } else {
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
        }

        // Baixa e armazena localmente a logo do WhatsApp
        private async System.Threading.Tasks.Task CarregarLogoWhatsAppAsync(PictureBox pictureBox) {
            if (carregandoLogo || IsDisposed || pictureBox == null || pictureBox.IsDisposed)
                return;

            carregandoLogo = true;
            ultimaTentativaLogo = DateTime.Now;
            // Usa o arquivo PNG do logotipo publicado na Wikimedia Commons
            string[] urlsLogo = new string[] {
                "https://thumb.wikimedia.org/wikipedia/commons/thumb/5/5e/WhatsApp_icon.png/250px-WhatsApp_icon.png?utm_campaign=index&utm_content=thumbnail&utm_source=commons.wikimedia.org",
                "https://upload.wikimedia.org/wikipedia/commons/5/5e/WhatsApp_icon.png"
            };
            string pastaCache = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Stingray", "Assets");
            string caminhoLogo = Path.Combine(pastaCache, "whatsapp-icon-v2.png");

            try {
                if (!File.Exists(caminhoLogo)) {
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    Directory.CreateDirectory(pastaCache);
                    byte[] bytes = null;
                    Exception erroDownload = null;
                    using (HttpClient cliente = new HttpClient()) {
                        cliente.Timeout = System.TimeSpan.FromSeconds(8);
                        cliente.DefaultRequestHeaders.UserAgent.ParseAdd("StingrayApp/1.0");
                        foreach (string urlLogo in urlsLogo) {
                            try {
                                bytes = await cliente.GetByteArrayAsync(urlLogo);
                                if (bytes != null && bytes.Length > 1000)
                                    break;
                                bytes = null;

                            } catch (Exception ex) {
                                erroDownload = ex;
                            }
                        }
                    }

                    if (bytes == null)
                        throw new InvalidOperationException("Não foi possível baixar a imagem do logotipo do WhatsApp.", erroDownload);

                    Directory.CreateDirectory(pastaCache);
                    File.WriteAllBytes(caminhoLogo, bytes);
                }

                if (IsDisposed || pictureBox.IsDisposed)
                    return;

                using (Image imagem = Image.FromFile(caminhoLogo)) {
                    pictureBox.Image = new Bitmap(imagem);
                }

            } catch {
                // Se o cache estiver corrompido, remova-o para permitir novo download depois
                try {
                    if (File.Exists(caminhoLogo)) File.Delete(caminhoLogo);

                } catch {

                }
                // Sem internet no primeiro uso, não colocamos nada
                if (!IsDisposed && !pictureBox.IsDisposed)
                    pictureBox.Image = null;

            } finally {
                carregandoLogo = false;
            }
        }

        // Abre o WhatsApp Web sem enviar mensagens
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

        private void PainelConteudo_Paint(object sender, PaintEventArgs e) {
            VisualPadrao.DesenharBordaCartao(e.Graphics, painelConteudo.ClientRectangle);
        }
    }
}
