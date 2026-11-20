using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.DadosTela;
using Stingray.Formularios;
using Stingray.Servicos;

namespace Stingray {
    [DesignerCategory("Form")]

    // Controla as preferências gerais, de frequência e do WhatsApp
    public partial class ConfiguracoesForm : Form {
        private Panel cartaoWhatsApp;
        private ToggleSwitch chkWhatsAppAtivado;
        private Label lblEstadoWhatsApp;
        private Label lblDescricaoWhatsApp;
        private RoundedButton btnAbrirWhatsApp;
        private PictureBox iconeWhatsApp;
        private Timer timerEstadoWhatsApp;
        private bool verificacaoEmAndamento;
        private readonly Usuario usuarioAtual;
        private Panel cartaoConta;
        private Panel cartaoFrequencia;
        private NumericUpDown numQuantidadeAulas;
        private NumericUpDown numPercentualFaltas;
        private Label lblResultadoFrequencia;
        private NumericUpDown numDiasCobrancaWhatsApp;

        public ConfiguracoesForm() : this(null) {
        }

        public ConfiguracoesForm(Usuario usuarioAtual) {
            this.usuarioAtual = usuarioAtual;
            InitializeComponent();
            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);
            CriarCartaoFrequencia(!emDesignTime);
            CriarConfiguracaoPrazoWhatsApp(!emDesignTime);
            cartaoWhatsApp.BorderStyle = BorderStyle.None;
            cartaoWhatsApp.Paint -= CartaoWhatsApp_Paint;
            cartaoWhatsApp.Paint += CartaoWhatsApp_Paint;

            iconeWhatsApp.Image = CarregarLogoWhatsApp();

            // Deixar placehouder durante edição no Visual Studio
            if (emDesignTime) {
                CriarCartaoConta(false);
                lblEstadoWhatsApp.Text = "Pré-visualização";
                lblDescricaoWhatsApp.Text = "A integração será verificada durante a execução do aplicativo.";
                btnAbrirWhatsApp.Visible = true;
                AjustarPosicaoCartoes();
                return;
            }

            if (usuarioAtual != null)
                CriarCartaoConta(true);

            btnAbrirWhatsApp.Visible = false;
            chkWhatsAppAtivado.Checked = WhatsAppServico.EstaAtivado();
            chkWhatsAppAtivado.CheckedChanged += ChkWhatsAppAtivado_CheckedChanged;
            btnAbrirWhatsApp.Click += BtnAbrirWhatsApp_Click;

            timerEstadoWhatsApp = new Timer();
            timerEstadoWhatsApp.Interval = 5000;
            timerEstadoWhatsApp.Tick += async delegate {
                await AtualizarEstadoWhatsAppAsync();
            };
            timerEstadoWhatsApp.Start();
            AtualizarVisualEstado();

            painelConteudo.Resize += delegate { AjustarPosicaoCartoes(); };
            AjustarPosicaoCartoes();
            FormClosed += delegate {
                if (timerEstadoWhatsApp != null) {
                    timerEstadoWhatsApp.Stop();
                    timerEstadoWhatsApp.Dispose();
                }
            };
        }


        // Carrega a imagem local usada na identificação do WhatsApp
        private static Image CarregarLogoWhatsApp() {
            const string nomeRecurso = "Stingray.Assets.WhatsAppLogo.png";
            var assembly = typeof(ConfiguracoesForm).Assembly;

            using (var fluxo = assembly.GetManifestResourceStream(nomeRecurso)) {
                if (fluxo == null) {
                    throw new InvalidOperationException(
                        "O recurso local do logotipo do WhatsApp não foi encontrado no executável: " + nomeRecurso);
                }

                using (var imagemOriginal = Image.FromStream(fluxo)) {
                    return new Bitmap(imagemOriginal);
                }
            }
        }

        // Mantém os cartões superiores lado a lado e a frequência em uma faixa própria
        private void AjustarPosicaoCartoes() {
            int larguraCartao = Math.Max(360, (painelConteudo.ClientSize.Width - 72) / 2);
            cartaoWhatsApp.Width = larguraCartao;
            chkWhatsAppAtivado.Left = cartaoWhatsApp.Width - chkWhatsAppAtivado.Width - 18;

            if (cartaoConta != null)
            {
                cartaoConta.Width = larguraCartao;
                cartaoConta.Left = cartaoWhatsApp.Right + 24;
            }

            if (cartaoFrequencia != null) {
                cartaoFrequencia.Width = Math.Max(440, painelConteudo.ClientSize.Width - 48);
                cartaoFrequencia.Left = 24;
            }
        }

        // Monta o controle para definir os dias de espera após o vencimento
        private void CriarConfiguracaoPrazoWhatsApp(bool carregarDoBanco)
        {
            cartaoWhatsApp.Size = new Size(440, 198);
            btnAbrirWhatsApp.Location = new Point(16, 154);
            btnAbrirWhatsApp.Size = new Size(165, 32);

            Label tituloPrazo = new Label
            {
                AutoSize = false,
                Location = new Point(16, 116),
                Size = new Size(226, 22),
                Text = "Cobrar após o vencimento",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            numDiasCobrancaWhatsApp = new NumericUpDown
            {
                Location = new Point(244, 111),
                Size = new Size(60, 27),
                Minimum = 1,
                Maximum = 30,
                Value = 4,
                Font = new Font("Segoe UI", 9F)
            };
            Label dias = new Label
            {
                AutoSize = true,
                Location = new Point(308, 116),
                Text = "dias",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            RoundedButton salvar = new RoundedButton
            {
                Text = "Salvar",
                Location = new Point(350, 110),
                Size = new Size(74, 29),
                CornerRadius = 5,
                BackColor = VisualPadrao.Azul,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8F),
                Cursor = Cursors.Hand
            };
            salvar.FlatAppearance.BorderSize = 0;
            if (carregarDoBanco)
                salvar.Click += SalvarPrazoWhatsApp_Click;

            cartaoWhatsApp.Controls.Add(tituloPrazo);
            cartaoWhatsApp.Controls.Add(numDiasCobrancaWhatsApp);
            cartaoWhatsApp.Controls.Add(dias);
            cartaoWhatsApp.Controls.Add(salvar);

            if (carregarDoBanco)
            {
                try
                {
                    int diasConfigurados = WhatsAppServico.ObterDiasAposVencimento();
                    numDiasCobrancaWhatsApp.Value = Math.Max(numDiasCobrancaWhatsApp.Minimum,
                        Math.Min(numDiasCobrancaWhatsApp.Maximum, diasConfigurados));
                }
                catch (Exception erro)
                {
                    numDiasCobrancaWhatsApp.Enabled = false;
                    salvar.Enabled = false;
                    lblDescricaoWhatsApp.Text = "Não foi possível carregar o prazo: " + erro.Message;
                }
            }
        }

        // Valida e persiste o prazo de cobrança configurado
        private void SalvarPrazoWhatsApp_Click(object sender, EventArgs e)
        {
            try
            {
                int dias = Convert.ToInt32(numDiasCobrancaWhatsApp.Value);
                WhatsAppServico.SalvarDiasAposVencimento(dias);
                MessageBox.Show(this,
                    "As cobranças automáticas serão enviadas após " + dias + " dia(s) do vencimento.",
                    "Prazo de cobrança", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception erro)
            {
                MessageBox.Show(this, "Não foi possível salvar o prazo de cobrança.\r\n\r\n" + erro.Message,
                    "Configuração do WhatsApp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriarCartaoConta(bool permitirAlteracao) {
            cartaoConta = new Panel {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(cartaoWhatsApp.Right + 24, 28),
                Size = new Size(440, 166),
                Name = "cartaoConta"
            };
            cartaoConta.Paint += delegate (object sender, PaintEventArgs e) {
                VisualPadrao.DesenharBordaCartao(e.Graphics, cartaoConta.ClientRectangle);
            };

            var lblTituloConta = new Label {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(18, 16),
                Text = "Conta do sistema"
            };

            var lblDescricaoConta = new Label {
                AutoSize = false,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(18, 55),
                Size = new Size(390, 34),
                Text = "Altere o nome de usuário e a senha usados para entrar no Stingray."
            };

            var btnAlterarConta = new RoundedButton {
                BackColor = VisualPadrao.Azul,
                CornerRadius = 7,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(18, 101),
                Size = new Size(205, 34),
                Text = "Alterar usuário e senha",
                UseVisualStyleBackColor = false
            };
            btnAlterarConta.FlatAppearance.BorderSize = 0;
            btnAlterarConta.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulHover;
            btnAlterarConta.FlatAppearance.MouseDownBackColor = VisualPadrao.AzulPressionado;
            if (permitirAlteracao)
                btnAlterarConta.Click += BtnAlterarConta_Click;

            cartaoConta.Controls.Add(lblTituloConta);
            cartaoConta.Controls.Add(lblDescricaoConta);
            cartaoConta.Controls.Add(btnAlterarConta);
            painelConteudo.Controls.Add(cartaoConta);
        }

        private void CriarCartaoFrequencia(bool carregarDoBanco)
        {
            cartaoFrequencia = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(24, 246),
                Size = new Size(Math.Max(440, painelConteudo.ClientSize.Width - 48), 238),
                Name = "cartaoFrequencia"
            };
            cartaoFrequencia.Paint += delegate (object sender, PaintEventArgs e) {
                VisualPadrao.DesenharBordaCartao(e.Graphics, cartaoFrequencia.ClientRectangle);
            };

            Label titulo = new Label
            {
                Text = "Configurações de Frequência e Inatividade",
                AutoSize = false,
                Location = new Point(18, 15),
                Size = new Size(400, 24),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55)
            };
            Label descricao = new Label
            {
                Text = "Defina a janela de aulas e o limite de faltas não justificadas.",
                AutoSize = false,
                Location = new Point(18, 43),
                Size = new Size(Math.Max(400, cartaoFrequencia.Width - 36), 28),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(107, 114, 128)
            };
            Label lblQuantidade = new Label
            {
                Text = "Últimas aulas consideradas",
                AutoSize = true,
                Location = new Point(18, 82),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            numQuantidadeAulas = new NumericUpDown
            {
                Location = new Point(18, 104),
                Size = new Size(190, 30),
                Minimum = 1,
                Maximum = 100,
                Value = 10,
                Font = new Font("Segoe UI", 9F)
            };
            Label lblPercentual = new Label
            {
                Text = "Percentual para inativação",
                AutoSize = true,
                Location = new Point(245, 82),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            numPercentualFaltas = new NumericUpDown
            {
                Location = new Point(245, 104),
                Size = new Size(160, 30),
                Minimum = 1,
                Maximum = 100,
                DecimalPlaces = 1,
                Increment = 0.5M,
                Value = 30M,
                Font = new Font("Segoe UI", 9F)
            };
            Label unidade = new Label
            {
                Text = "%",
                AutoSize = true,
                Location = new Point(412, 109),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            lblResultadoFrequencia = new Label
            {
                Text = "O percentual considera somente faltas não justificadas.",
                AutoSize = false,
                Location = new Point(18, 150),
                Size = new Size(Math.Max(400, cartaoFrequencia.Width - 36), 28),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(107, 114, 128)
            };
            RoundedButton salvar = new RoundedButton
            {
                Text = "Salvar configurações",
                Location = new Point(18, 190),
                Size = new Size(190, 36),
                CornerRadius = 6,
                BackColor = VisualPadrao.Azul,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8.5F),
                Cursor = Cursors.Hand
            };
            salvar.FlatAppearance.BorderSize = 0;
            salvar.Click += SalvarConfiguracoesFrequencia_Click;

            cartaoFrequencia.Controls.AddRange(new Control[] {
                titulo, descricao, lblQuantidade, numQuantidadeAulas,
                lblPercentual, numPercentualFaltas, unidade, lblResultadoFrequencia, salvar
            });
            painelConteudo.Controls.Add(cartaoFrequencia);

            if (carregarDoBanco)
            {
                try
                {
                    ConfiguracaoFrequencia configuracao = new FrequenciaServico().ObterConfiguracao();
                    numQuantidadeAulas.Value = Math.Max(numQuantidadeAulas.Minimum,
                        Math.Min(numQuantidadeAulas.Maximum, configuracao.QuantidadeAulas));
                    numPercentualFaltas.Value = Math.Max(numPercentualFaltas.Minimum,
                        Math.Min(numPercentualFaltas.Maximum, configuracao.PercentualFaltas));
                }
                catch (Exception erro)
                {
                    lblResultadoFrequencia.Text = "Não foi possível ler as configurações: " + erro.Message;
                    lblResultadoFrequencia.ForeColor = Color.FromArgb(185, 28, 28);
                }
            }
        }

        // Valida e salva as opções de cálculo da frequência
        private void SalvarConfiguracoesFrequencia_Click(object sender, EventArgs e)
        {
            try
            {
                new FrequenciaServico().SalvarConfiguracao(new ConfiguracaoFrequencia
                {
                    QuantidadeAulas = Convert.ToInt32(numQuantidadeAulas.Value),
                    PercentualFaltas = numPercentualFaltas.Value
                });
                lblResultadoFrequencia.Text = "Configurações salvas. O status das matrículas foi reavaliado.";
                lblResultadoFrequencia.ForeColor = Color.FromArgb(22, 101, 52);
                MessageBox.Show(this, "Configurações de frequência salvas.",
                    "Configurações", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception erro)
            {
                lblResultadoFrequencia.Text = "Não foi possível salvar as configurações.";
                lblResultadoFrequencia.ForeColor = Color.FromArgb(185, 28, 28);
                MessageBox.Show(this, erro.Message, "Configurações", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAlterarConta_Click(object sender, EventArgs e) {
            using (var formulario = new ContaAcessoForm(false, usuarioAtual)) {
                formulario.ShowDialog(FindForm());
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
            bool valorAnterior = !chkWhatsAppAtivado.Checked;
            try {
                WhatsAppServico.DefinirAtivado(chkWhatsAppAtivado.Checked);
                AtualizarVisualEstado();
                if (chkWhatsAppAtivado.Checked)
                    _ = AtualizarEstadoWhatsAppAsync();
            } catch (Exception erro) {
                chkWhatsAppAtivado.CheckedChanged -= ChkWhatsAppAtivado_CheckedChanged;
                chkWhatsAppAtivado.Checked = valorAnterior;
                chkWhatsAppAtivado.CheckedChanged += ChkWhatsAppAtivado_CheckedChanged;
                MessageBox.Show(this, "Não foi possível salvar o estado do WhatsApp no banco de dados.\r\n\r\n" + erro.Message,
                    "Configuração do WhatsApp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Consulta a conexão sem alterar a fila de mensagens
        private async System.Threading.Tasks.Task AtualizarEstadoWhatsAppAsync() {
            if (verificacaoEmAndamento || IsDisposed || !IsHandleCreated)
                return;

            verificacaoEmAndamento = true;
            try {
                if (!WhatsAppServico.EstaAtivado()) {
                    AtualizarVisualEstado();
                    return;
                }

                // Mantém o único botão de ação visível enquanto a integração estiver ativa
                btnAbrirWhatsApp.Visible = true;

                bool internet = await WhatsAppServico.VerificarInternetAsync();
                if (!internet) {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                    // Não alteramos o estado de autenticação salvo só porque a internet caiu
                    return;
                }

                bool conectadoAgora = await WhatsAppServico.VerificarWhatsAppAsync();
                if (WhatsAppServico.SessaoConfirmadaAgora && conectadoAgora) {
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "WhatsApp Web autenticado e pronto.";

                } else if (WhatsAppServico.SessaoConfirmadaAgora && !conectadoAgora) {
                    // A verificação encontrou claramente a tela de QR Code/login
                    lblEstadoWhatsApp.Text = "Desconectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(180, 83, 9);
                    lblDescricaoWhatsApp.Text = "Abra o navegador e leia o QR Code pelo celular.";

                } else if (WhatsAppServico.ConexaoWhatsApp) {
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
                if (WhatsAppServico.ConexaoInternet) {
                    lblEstadoWhatsApp.Text = WhatsAppServico.ConexaoWhatsApp
                        ? "Conectado"
                        : "Não foi possível verificar";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = WhatsAppServico.ConexaoWhatsApp
                        ? "A sessão será verificada automaticamente."
                        : "Abra o WhatsApp Web para autenticar a sessão.";

                } else {
                    lblEstadoWhatsApp.Text = "Sem internet";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(185, 28, 28);
                    lblDescricaoWhatsApp.Text = "Verifique sua conexão com a internet.";
                }

                if (WhatsAppServico.EstaAtivado())
                    btnAbrirWhatsApp.Visible = true;

            } finally {
                verificacaoEmAndamento = false;
            }
        }

        // Atualiza a aparência conforme o estado da integração
        private void AtualizarVisualEstado() {
            if (!WhatsAppServico.EstaAtivado()) {
                lblEstadoWhatsApp.Text = "Inativo";
                lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                lblDescricaoWhatsApp.Text = "A integração está desligada.";
                btnAbrirWhatsApp.Visible = false;

            } else {
                btnAbrirWhatsApp.Visible = true;

                if (!WhatsAppServico.ConexaoInternet) {
                    lblEstadoWhatsApp.Text = "Verificando conexão...";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = WhatsAppServico.ConexaoWhatsApp
                        ? "A sessão será verificada quando a internet voltar."
                        : "Verifique sua conexão com a internet.";

                } else if (!WhatsAppServico.SessaoConfirmadaAgora && WhatsAppServico.ConexaoWhatsApp) {
                    lblEstadoWhatsApp.Text = "Conectado";
                    lblEstadoWhatsApp.ForeColor = Color.FromArgb(22, 101, 52);
                    lblDescricaoWhatsApp.Text = "A sessão será verificada automaticamente.";

                } else {
                    lblEstadoWhatsApp.Text = WhatsAppServico.ConexaoWhatsApp
                        ? "Conectado"
                        : "Verificando sessão...";
                    lblEstadoWhatsApp.ForeColor = WhatsAppServico.ConexaoWhatsApp
                        ? Color.FromArgb(22, 101, 52)
                        : Color.FromArgb(107, 114, 128);
                    lblDescricaoWhatsApp.Text = "Abra o WhatsApp Web para acessar a sessão.";
                }
            }
        }

        // Abre o WhatsApp Web sem enviar mensagens
        private async void BtnAbrirWhatsApp_Click(object sender, EventArgs e) {
            btnAbrirWhatsApp.Enabled = false;
            btnAbrirWhatsApp.Text = "Abrindo navegador...";
            try {
                bool abriu = await WhatsAppServico.PrepararWhatsAppAsync();
                if (!abriu) {
                    MessageBox.Show(this,
                        "Não foi possível abrir o WhatsApp Web.\r\n\r\n" +
                        (String.IsNullOrWhiteSpace(WhatsAppServico.UltimoErro)
                            ? "Nenhum detalhe de erro foi informado."
                            : WhatsAppServico.UltimoErro),
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
