using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace Stingray
{
    public partial class ConfiguracoesForm : Form
    {
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

        public ConfiguracoesForm()
        {
            InitializeComponent();
            painelConteudo.Paint += PainelConteudo_Paint;
            CriarCartaoWhatsApp();
        }

        /// <summary>
        /// Cria o cartão da integração WhatsApp na área de Configurações.
        /// O cartão ocupa aproximadamente metade da largura disponível, deixando
        /// espaço para futuras opções de configuração ao lado.
        /// </summary>
        private void CriarCartaoWhatsApp()
        {
            cartaoWhatsApp = new Panel();
            cartaoWhatsApp.Name = "cartaoWhatsApp";
            cartaoWhatsApp.Location = new Point(24, 28);
            cartaoWhatsApp.Size = new Size(Math.Max(360, (painelConteudo.ClientSize.Width - 72) / 2), 166);
            cartaoWhatsApp.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cartaoWhatsApp.BackColor = Color.White;
            cartaoWhatsApp.BorderStyle = BorderStyle.None;
            cartaoWhatsApp.Paint += (sender, args) =>
            {
                args.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle r = cartaoWhatsApp.ClientRectangle;
                r.Width -= 1; r.Height -= 1;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 16;
                    path.AddArc(r.X, r.Y, radius, radius, 180, 90);
                    path.AddArc(r.Right - radius, r.Y, radius, radius, 270, 90);
                    path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(r.X, r.Bottom - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    using (var pen = new Pen(Color.FromArgb(226, 228, 232), 1f))
                        args.Graphics.DrawPath(pen, path);
                }
            };
            painelConteudo.Controls.Add(cartaoWhatsApp);

            Label titulo = new Label();
            titulo.Text = "WhatsApp Web";
            titulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            titulo.ForeColor = Color.FromArgb(31, 41, 55);
            titulo.Location = new Point(62, 14);
            titulo.AutoSize = true;
            cartaoWhatsApp.Controls.Add(titulo);

            // A imagem do logotipo é obtida da Wikimedia Commons e armazenada em cache local.
            // Se o primeiro uso ocorrer sem internet, o temporizador tentará novamente depois.
            iconeWhatsApp = new PictureBox();
            iconeWhatsApp.Name = "iconeWhatsApp";
            iconeWhatsApp.Location = new Point(16, 12);
            iconeWhatsApp.Size = new Size(36, 36);
            iconeWhatsApp.BackColor = Color.Transparent;
            iconeWhatsApp.SizeMode = PictureBoxSizeMode.Zoom;
            cartaoWhatsApp.Controls.Add(iconeWhatsApp);
            _ = CarregarLogoWhatsAppAsync(iconeWhatsApp);

            // Interruptor azul/branco. O estado é carregado do configuracoes.json pelo serviço.
            chkWhatsAppAtivado = new ToggleSwitch();
            chkWhatsAppAtivado.Name = "chkWhatsAppAtivado";
            chkWhatsAppAtivado.Location = new Point(cartaoWhatsApp.Width - 66, 16);
            chkWhatsAppAtivado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkWhatsAppAtivado.Checked = WhatsAppService.EstaAtivado();
            chkWhatsAppAtivado.CheckedChanged += ChkWhatsAppAtivado_CheckedChanged;
            cartaoWhatsApp.Controls.Add(chkWhatsAppAtivado);

            lblEstadoWhatsApp = new Label();
            lblEstadoWhatsApp.Name = "lblEstadoWhatsApp";
            lblEstadoWhatsApp.Text = "Inativo";
            lblEstadoWhatsApp.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
            lblEstadoWhatsApp.Location = new Point(16, 64);
            lblEstadoWhatsApp.AutoSize = true;
            cartaoWhatsApp.Controls.Add(lblEstadoWhatsApp);

            lblDescricaoWhatsApp = new Label();
            lblDescricaoWhatsApp.Text = "Ative para verificar a conexão e o login.";
            lblDescricaoWhatsApp.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            lblDescricaoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
            lblDescricaoWhatsApp.Location = new Point(16, 88);
            lblDescricaoWhatsApp.AutoSize = true;
            cartaoWhatsApp.Controls.Add(lblDescricaoWhatsApp);

            // Único botão do cartão: abre ou traz o WhatsApp Web para a frente.
            // Ele permanece disponível enquanto a integração estiver ativada, inclusive
            // quando a internet cair ou quando for necessário ler novamente o QR Code.
            btnAbrirWhatsApp = new RoundedButton();
            btnAbrirWhatsApp.Name = "btnAbrirWhatsApp";
            btnAbrirWhatsApp.Text = "Abrir WhatsApp Web";
            btnAbrirWhatsApp.Location = new Point(16, 116);
            btnAbrirWhatsApp.Size = new Size(165, 36);
            btnAbrirWhatsApp.BackColor = Color.FromArgb(25, 86, 196);
            btnAbrirWhatsApp.ForeColor = Color.White;
            btnAbrirWhatsApp.FlatAppearance.BorderSize = 0;
            btnAbrirWhatsApp.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular);
            btnAbrirWhatsApp.CornerRadius = 7;
            btnAbrirWhatsApp.Cursor = Cursors.Hand;
            btnAbrirWhatsApp.Visible = false;
            btnAbrirWhatsApp.Click += BtnAbrirWhatsApp_Click;
            cartaoWhatsApp.Controls.Add(btnAbrirWhatsApp);

            // Atualiza o indicador periodicamente sem bloquear a interface.
            timerEstadoWhatsApp = new Timer();
            timerEstadoWhatsApp.Interval = 5000;
            timerEstadoWhatsApp.Tick += async delegate
            {
                await AtualizarEstadoWhatsAppAsync();

                // Se o app começou sem internet, tenta baixar a logo novamente no máximo
                // uma vez a cada 30 segundos, até conseguir guardá-la no cache local.
                if (iconeWhatsApp.Image == null && !carregandoLogo &&
                    DateTime.Now - ultimaTentativaLogo >= TimeSpan.FromSeconds(30))
                {
                    ultimaTentativaLogo = DateTime.Now;
                    await CarregarLogoWhatsAppAsync(iconeWhatsApp);
                }
            };
            timerEstadoWhatsApp.Start();

            AtualizarVisualEstado();
            painelConteudo.Resize += delegate
            {
                // Mantém o cartão com metade da largura da área de conteúdo.
                cartaoWhatsApp.Width = Math.Max(360, (painelConteudo.ClientSize.Width - 72) / 2);
                chkWhatsAppAtivado.Left = cartaoWhatsApp.Width - chkWhatsAppAtivado.Width - 18;
            };
            FormClosed += delegate
            {
                if (timerEstadoWhatsApp != null)
                {
                    timerEstadoWhatsApp.Stop();
                    timerEstadoWhatsApp.Dispose();
                }
            };
        }

        /// <summary>
        /// Guarda o estado liga/desliga no serviço e atualiza os textos visíveis.
        /// Desativar não apaga mensagens pendentes.
        /// </summary>
        private void ChkWhatsAppAtivado_CheckedChanged(object sender, EventArgs e)
        {
            WhatsAppService.DefinirAtivado(chkWhatsAppAtivado.Checked);
            AtualizarVisualEstado();
            if (chkWhatsAppAtivado.Checked)
                _ = AtualizarEstadoWhatsAppAsync();
        }

        /// <summary>
        /// Verifica internet e autenticação. Não envia nem adiciona mensagens à fila.
        /// </summary>
        private async System.Threading.Tasks.Task AtualizarEstadoWhatsAppAsync()
        {
            if (verificacaoEmAndamento || IsDisposed || !IsHandleCreated)
                return;

            verificacaoEmAndamento = true;
            try
            {
                if (!WhatsAppService.EstaAtivado())
                {
                    AtualizarVisualEstado();
                    return;
                }

                // Mantém o único botão de ação visível enquanto a integração estiver ativa.
                btnAbrirWhatsApp.Visible = true;

                bool internet = await WhatsAppService.VerificarInternetAsync();
                if (!internet)
                {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                    // Não alteramos o estado de autenticação salvo só porque a internet caiu.
                    return;
                }

                bool conectadoAgora = await WhatsAppService.VerificarWhatsAppAsync();
                if (WhatsAppService.SessaoConfirmadaAgora && conectadoAgora)
                {
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "WhatsApp Web autenticado e pronto.";
                }
                else if (WhatsAppService.SessaoConfirmadaAgora && !conectadoAgora)
                {
                    // A verificação encontrou claramente a tela de QR Code/login.
                    lblEstadoWhatsApp.Text = "Desconectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(180, 83, 9);
                    lblDescricaoWhatsApp.Text = "Abra o navegador e leia o QR Code pelo celular.";
                }
                else if (WhatsAppService.ConexaoWhatsApp)
                {
                    // Preserva o estado salvo durante falhas transitórias. A sessão é
                    // confirmada novamente pelo navegador assim que ele responder.
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "A sessão será verificada automaticamente.";
                }
                else
                {
                    lblEstadoWhatsApp.Text = "Verificando sessão...";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = "A página ainda está carregando ou aguardando autenticação.";
                }
            }
            catch
            {
                // Uma falha de consulta não equivale a logout. Preserva o estado salvo.
                if (WhatsAppService.ConexaoInternet)
                {
                    lblEstadoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                        ? "Conectado"
                        : "Não foi possível verificar";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                        ? "A sessão será verificada automaticamente."
                        : "Abra o WhatsApp Web para autenticar a sessão.";
                }
                else
                {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                }

                if (WhatsAppService.EstaAtivado())
                {
                    btnAbrirWhatsApp.Visible = true;
                }
            }
            finally
            {
                verificacaoEmAndamento = false;
            }
        }

        /// <summary>
        /// Renderiza o estado inicial ou inativo. A verificação de conexão só ocorre
        /// quando o usuário liga a integração.
        /// </summary>
        private void AtualizarVisualEstado()
        {
            if (!WhatsAppService.EstaAtivado())
            {
                lblEstadoWhatsApp.Text = "Inativo";
                lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                lblDescricaoWhatsApp.Text = "A integração está desligada.";
                btnAbrirWhatsApp.Visible = false;
            }
            else
            {
                btnAbrirWhatsApp.Visible = true;

                if (!WhatsAppService.ConexaoInternet)
                {
                    lblEstadoWhatsApp.Text = "Verificando conexão...";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = WhatsAppService.ConexaoWhatsApp
                        ? "A sessão será verificada quando a internet voltar."
                        : "Verifique sua conexão com a internet.";
                }
                else if (!WhatsAppService.SessaoConfirmadaAgora && WhatsAppService.ConexaoWhatsApp)
                {
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "A sessão será verificada automaticamente.";
                }
                else
                {
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

        /// <summary>
        /// Baixa a logo oficial do WhatsApp da Wikimedia Commons e a mantém em cache local.
        /// O download é feito apenas quando ainda não existe uma cópia. A imagem é carregada
        /// de um arquivo local para que o arquivo possa ser fechado/liberado com segurança.
        /// </summary>
        private async System.Threading.Tasks.Task CarregarLogoWhatsAppAsync(PictureBox pictureBox)
        {
            if (carregandoLogo || IsDisposed || pictureBox == null || pictureBox.IsDisposed)
                return;

            carregandoLogo = true;
            ultimaTentativaLogo = DateTime.Now;
            // Usa o arquivo PNG do logotipo publicado na Wikimedia Commons. São duas URLs
            // para tolerar diferenças de cache/CDN e firewall; não desenhamos uma substituição.
            string[] urlsLogo = new string[]
            {
                "https://thumb.wikimedia.org/wikipedia/commons/thumb/5/5e/WhatsApp_icon.png/250px-WhatsApp_icon.png?utm_campaign=index&utm_content=thumbnail&utm_source=commons.wikimedia.org",
                "https://upload.wikimedia.org/wikipedia/commons/5/5e/WhatsApp_icon.png"
            };
            string pastaCache = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Stingray", "Assets");
            string caminhoLogo = Path.Combine(pastaCache, "whatsapp-icon-v2.png");

            try
            {
                if (!File.Exists(caminhoLogo))
                {
                    // Compatibilidade com instalações do .NET Framework que não estão
                    // configuradas para negociar TLS automaticamente com o servidor HTTPS.
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    Directory.CreateDirectory(pastaCache);
                    byte[] bytes = null;
                    Exception erroDownload = null;
                    using (HttpClient cliente = new HttpClient())
                    {
                        cliente.Timeout = System.TimeSpan.FromSeconds(8);
                        cliente.DefaultRequestHeaders.UserAgent.ParseAdd("StingrayApp/1.0");
                        foreach (string urlLogo in urlsLogo)
                        {
                            try
                            {
                                bytes = await cliente.GetByteArrayAsync(urlLogo);
                                if (bytes != null && bytes.Length > 1000)
                                    break;
                                bytes = null;
                            }
                            catch (Exception ex)
                            {
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

                // Image.FromFile mantém o arquivo bloqueado; criar uma cópia em memória
                // permite que o cache possa ser atualizado/limpo depois sem erro de arquivo.
                using (Image imagem = Image.FromFile(caminhoLogo))
                {
                    pictureBox.Image = new Bitmap(imagem);
                }
            }
            catch
            {
                // Se o cache estiver corrompido, remova-o para permitir novo download depois.
                try { if (File.Exists(caminhoLogo)) File.Delete(caminhoLogo); } catch { }
                // Sem internet no primeiro uso, não desenhamos uma substituta; o timer tentará novamente.
                if (!IsDisposed && !pictureBox.IsDisposed)
                    pictureBox.Image = null;
            }
            finally
            {
                carregandoLogo = false;
            }
        }

        /// <summary>
        /// Abre ou traz para frente o WhatsApp Web. O botão continua disponível mesmo
        /// quando a sessão aparece como conectada ou quando a internet está indisponível.
        /// Esta função não envia nem adiciona mensagens à fila.
        /// </summary>
        private async void BtnAbrirWhatsApp_Click(object sender, EventArgs e)
        {
            btnAbrirWhatsApp.Enabled = false;
            btnAbrirWhatsApp.Text = "Abrindo navegador...";
            try
            {
                bool abriu = await WhatsAppService.PrepararWhatsAppAsync();
                if (!abriu)
                {
                    MessageBox.Show(this,
                        "Não foi possível abrir o WhatsApp Web.\r\n\r\n" +
                        (String.IsNullOrWhiteSpace(WhatsAppService.UltimoErro)
                            ? "Nenhum detalhe de erro foi informado."
                            : WhatsAppService.UltimoErro),
                        "WhatsApp Web", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "WhatsApp Web",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnAbrirWhatsApp.Enabled = true;
                    btnAbrirWhatsApp.Text = "Abrir WhatsApp Web";
                    await AtualizarEstadoWhatsAppAsync();
                }
            }
        }

        private void PainelConteudo_Paint(object sender, PaintEventArgs e)
        {
            Rectangle area = painelConteudo.ClientRectangle;
            if (area.Width <= 1 || area.Height <= 1)
                return;

            area.Width -= 1;
            area.Height -= 1;
            using (Pen borda = new Pen(Color.FromArgb(226, 232, 240)))
                e.Graphics.DrawRectangle(borda, area);
        }
    }
}
