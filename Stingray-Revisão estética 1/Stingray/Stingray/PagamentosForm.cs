using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;

namespace Stingray {
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class PagamentosForm : System.Windows.Forms.Form {
        private readonly Color azul = Color.FromArgb(25, 86, 196);
        private readonly Color azulClaro = Color.FromArgb(239, 246, 255);
        private readonly Color textoEscuro = Color.FromArgb(55, 65, 81);
        private readonly Color textoPlaceholder = Color.FromArgb(148, 163, 184);

        private Panel popupPeriodo;
        private Panel campoDataInicial;
        private Panel campoDataFinal;
        private TextBox txtDataInicial;
        private TextBox txtDataFinal;
        private Button btnCalendarioInicial;
        private Button btnCalendarioFinal;
        private Button btnSetaTurma;
        private Button btnSetaStatus;
        private Panel turmaContentPanel;
        private Panel statusContentPanel;
        private Panel calendarioFlutuante;
        private Label lblMesCalendario;
        private Label lblHojeCalendario;
        private DateTime mesCalendarioVisualizado = DateTime.Today;
        private readonly List<Button> botoesDiasCalendario = new List<Button>();
        private readonly Font fonteDiaCalendario = new Font("Segoe UI", 7.8F, FontStyle.Regular);
        private readonly Font fonteDiaCalendarioHoje = new Font("Segoe UI Semibold", 7.8F, FontStyle.Regular);
        private Panel campoDataAtivo;
        private DateTime? dataInicialSelecionada;
        private DateTime? dataFinalSelecionada;
        private IMessageFilter filtroCliqueExterno;
        private bool placeholderAluno = true;
        private DateTime? dataInicialAplicada;
        private DateTime? dataFinalAplicada;

        private enum TipoIconeCalendario {
            Periodo,
            DataInicial,
            DataFinal
        }

        private sealed class CalendarDayButton : Button {
            protected override bool ShowFocusCues { get { return false; } }
        }

        public PagamentosForm() {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ConfigurarAparenciaFiltros();
            ConfigurarPosicaoBotaoNovoPagamento();

            if (cmbTurma.Items.Count > 0) cmbTurma.SelectedIndex = 0;
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;

            ConfigurarEventosPagamento();
            ConfigurarPopupPeriodo();
            PreencherPagamentosDeExemplo();
        }

        private void ConfigurarEventosPagamento() {
            btnCalendario.Click += delegate { AlternarPopupPeriodo(); };
            btnPesquisar.Click += delegate { AplicarPesquisaVisual(); };
            btnLimpar.Click += delegate { LimparFiltros(); };
            btnLimpar.MouseEnter += delegate {
                btnLimpar.ForeColor = Color.FromArgb(185, 28, 28);
                btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
                btnLimpar.BackColor = Color.FromArgb(254, 242, 242);
            };
            btnLimpar.MouseLeave += delegate {
                btnLimpar.ForeColor = textoEscuro;
                btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                btnLimpar.BackColor = Color.White;
            };
            btnLimpar.MouseDown += delegate {
                btnLimpar.ForeColor = Color.FromArgb(153, 27, 27);
                btnLimpar.BackColor = Color.FromArgb(254, 226, 226);
            };
            btnLimpar.MouseUp += delegate {
                if (btnLimpar.ClientRectangle.Contains(btnLimpar.PointToClient(Cursor.Position))) {
                    btnLimpar.ForeColor = Color.FromArgb(185, 28, 28);
                    btnLimpar.BackColor = Color.FromArgb(254, 242, 242);
                }
            };

            txtAluno.Enter += delegate {
                if (placeholderAluno) {
                    txtAluno.Clear();
                    txtAluno.ForeColor = textoEscuro;
                    placeholderAluno = false;
                }
            };
            txtAluno.Leave += delegate {
                if (string.IsNullOrWhiteSpace(txtAluno.Text)) {
                    txtAluno.Text = "Digite o nome do aluno...";
                    txtAluno.ForeColor = textoPlaceholder;
                    placeholderAluno = true;
                }
            };
            gridPayments.CellPainting += GridPayments_CellPainting;
            gridPayments.CellContentClick += GridPayments_CellContentClick;
        }
        private void ConfigurarPosicaoBotaoNovoPagamento() {
            if (headerPanel == null || btnNovoPagamento == null)
                return;

            btnNovoPagamento.Visible = true;
            if (!headerPanel.Controls.Contains(btnNovoPagamento))
                headerPanel.Controls.Add(btnNovoPagamento);

            btnNovoPagamento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovoPagamento.Size = new Size(194, 36);
            btnNovoPagamento.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnNovoPagamento.BringToFront();
            PosicionarBotaoNovoPagamento();
            headerPanel.Resize += delegate { PosicionarBotaoNovoPagamento(); };
        }

        private void PosicionarBotaoNovoPagamento() {
            if (headerPanel == null || btnNovoPagamento == null)
                return;

            int x = Math.Max(8, headerPanel.ClientSize.Width - btnNovoPagamento.Width - 10);
            int y = Math.Max(4, (headerPanel.ClientSize.Height - btnNovoPagamento.Height) / 2);
            btnNovoPagamento.Location = new Point(x, y);
            btnNovoPagamento.Visible = true;
        }

        private void ConfigurarAparenciaFiltros() {
            periodoField.BackColor = Color.White;
            alunoField.BackColor = Color.White;
            PrepararCampoSeletor(turmaField, cmbTurma, out turmaContentPanel);
            PrepararCampoSeletor(statusField, cmbStatus, out statusContentPanel);
            txtPeriodo.BackColor = Color.White;
            txtAluno.BackColor = Color.White;
            txtAluno.BorderStyle = BorderStyle.None;
            txtAluno.Dock = DockStyle.None;
            txtAluno.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPeriodo.Dock = DockStyle.None;
            txtPeriodo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnCalendario.Dock = DockStyle.None;
            btnCalendario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCalendario.AccessibleName = "Selecionar período completo";
            btnCalendario.AccessibleDescription = "Abre o seletor de data inicial e data final";
            ConfigurarBotaoIconeCalendario(btnCalendario, TipoIconeCalendario.Periodo);
            cmbTurma.Dock = DockStyle.None;
            cmbTurma.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbStatus.Dock = DockStyle.None;
            cmbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Mantém o valor selecionado com fundo branco.
            cmbTurma.DrawMode = DrawMode.OwnerDrawFixed;
            cmbStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTurma.ItemHeight = 18;
            cmbStatus.ItemHeight = 18;
            cmbTurma.DrawItem += ComboFiltro_DrawItem;
            cmbStatus.DrawItem += ComboFiltro_DrawItem;
            cmbTurma.DropDownClosed += delegate { cmbTurma.Invalidate(); turmaField.Invalidate(); };
            cmbStatus.DropDownClosed += delegate { cmbStatus.Invalidate(); statusField.Invalidate(); };

            CriarBotaoSetaCombo(turmaContentPanel, cmbTurma, true);
            CriarBotaoSetaCombo(statusContentPanel, cmbStatus, false);

            // Período e Aluno usam contorno próprio; os seletores usam externo
            periodoField.Paint += CampoFiltro_Paint;
            alunoField.Paint += CampoFiltro_Paint;
            cmbTurma.AutoSize = false;
            cmbStatus.AutoSize = false;
            cmbTurma.FlatStyle = FlatStyle.Flat;
            cmbStatus.FlatStyle = FlatStyle.Flat;

            periodoField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            alunoField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            turmaField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            statusField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            if (turmaContentPanel != null) turmaContentPanel.SizeChanged += delegate { PosicionarControlesFiltro(); };
            if (statusContentPanel != null) statusContentPanel.SizeChanged += delegate { PosicionarControlesFiltro(); };
            PosicionarControlesFiltro();
        }

        private void PrepararCampoSeletor(Panel campo, ComboBox combo, out Panel conteudo) {
            campo.Paint -= CampoFiltro_Paint;
            campo.Padding = new Padding(1);
            campo.BackColor = Color.FromArgb(203, 213, 225);

            conteudo = new Panel();
            conteudo.Name = campo.Name + "Content";
            conteudo.Dock = DockStyle.Fill;
            conteudo.Margin = new Padding(0);
            conteudo.Padding = new Padding(0);
            conteudo.BackColor = Color.White;
            campo.Controls.Add(conteudo);
            conteudo.BringToFront();

            combo.Parent = conteudo;
            combo.BackColor = Color.White;
        }

        private void CriarBotaoSetaCombo(Panel campo, ComboBox combo, bool turma) {
            Button seta = new Button();
            seta.Name = turma ? "btnSetaTurma" : "btnSetaStatus";
            seta.AccessibleName = turma ? "Abrir opções de turma" : "Abrir opções de status";
            seta.AccessibleDescription = "Abre a lista de opções";
            seta.Text = string.Empty;
            seta.TabStop = false;
            seta.Cursor = Cursors.Hand;
            seta.FlatStyle = FlatStyle.Flat;
            seta.FlatAppearance.BorderSize = 0;
            seta.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            seta.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            seta.BackColor = Color.White;
            seta.UseVisualStyleBackColor = false;
            seta.Padding = new Padding(0);
            seta.Margin = new Padding(0);
            seta.Paint += ComboSeta_Paint;
            seta.MouseEnter += delegate { seta.BackColor = Color.FromArgb(239, 246, 255); seta.Invalidate(); };
            seta.MouseLeave += delegate { seta.BackColor = Color.White; seta.Invalidate(); };
            seta.Click += delegate {
                if (combo == null || combo.IsDisposed) return;
                combo.Focus();
                combo.DroppedDown = true;
            };

            campo.Controls.Add(seta);
            seta.BringToFront();
            if (turma) btnSetaTurma = seta;
            else btnSetaStatus = seta;
        }

        private void ComboSeta_Paint(object sender, PaintEventArgs e) {
            Button seta = sender as Button;
            if (seta == null) return;

            Rectangle r = seta.ClientRectangle;
            if (r.Width < 8 || r.Height < 8) return;

            bool hover = r.Contains(seta.PointToClient(Cursor.Position));
            Color corSeta = hover ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                e.Graphics.DrawLine(separador, 0, 1, 0, r.Height - 2);

            float cx = r.Width / 2F;
            float cy = r.Height / 2F;
            using (Pen pen = new Pen(corSeta, 1.6F)) {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                e.Graphics.DrawLines(pen, new PointF[] {
                    new PointF(cx - 3.5F, cy - 1.5F),
                    new PointF(cx, cy + 2F),
                    new PointF(cx + 3.5F, cy - 1.5F)
                });
            }
        }

        private void ComboFiltro_DrawItem(object sender, DrawItemEventArgs e) {
            ComboBox combo = sender as ComboBox;
            if (combo == null || e.Bounds.Width <= 0 || e.Bounds.Height <= 0)
                return;

            bool itemSelecionadoNaLista = combo.DroppedDown && e.Index >= 0 &&
                (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color fundo = itemSelecionadoNaLista ? azul : Color.White;
            Color texto = itemSelecionadoNaLista ? Color.White : textoEscuro;

            using (Brush brushFundo = new SolidBrush(fundo))
                e.Graphics.FillRectangle(brushFundo, e.Bounds);

            string valor = e.Index >= 0 && e.Index < combo.Items.Count
                ? Convert.ToString(combo.Items[e.Index])
                : combo.Text;
            Rectangle areaTexto = Rectangle.Inflate(e.Bounds, -6, 0);
            TextRenderer.DrawText(e.Graphics, valor, combo.Font, areaTexto, texto,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        private void CampoFiltro_Paint(object sender, PaintEventArgs e) {
            Panel panel = sender as Panel;
            if (panel == null || panel.ClientSize.Width < 2 || panel.ClientSize.Height < 2)
                return;

            // Desenhar uma caneta em cortava metade do traço
            using (Pen pen = new Pen(Color.FromArgb(203, 213, 225), 1F)) {
                e.Graphics.DrawRectangle(pen, 0, 0,
                    Math.Max(0, panel.ClientSize.Width - 1),
                    Math.Max(0, panel.ClientSize.Height - 1));
            }
        }

        private void ConfigurarBotaoIconeCalendario(Button botao, TipoIconeCalendario tipo) {
            if (botao == null) return;
            botao.Tag = tipo;
            botao.Text = string.Empty;
            botao.Padding = new Padding(0);
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            botao.BackColor = Color.White;
            botao.UseVisualStyleBackColor = false;
            botao.Cursor = Cursors.Hand;
            botao.Paint += CalendarioButton_Paint;
            botao.MouseEnter += delegate { botao.Invalidate(); };
            botao.MouseLeave += delegate { botao.Invalidate(); };
        }

        private void CalendarioButton_Paint(object sender, PaintEventArgs e) {
            Button botao = sender as Button;
            if (botao == null) return;

            bool hover = botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position));
            Color cor = hover ? Color.FromArgb(29, 78, 216) : Color.FromArgb(55, 96, 160);
            TipoIconeCalendario tipo = botao.Tag is TipoIconeCalendario
                ? (TipoIconeCalendario) botao.Tag
                : TipoIconeCalendario.DataInicial;
            DesenharIconeCalendario(e.Graphics, botao.ClientRectangle, cor, tipo);
        }

        private static void DesenharIconeCalendario(Graphics graphics, Rectangle bounds, Color color, TipoIconeCalendario tipo) {
            float tamanho = Math.Min(18F, Math.Min(bounds.Width - 4F, bounds.Height - 4F));
            if (tamanho < 12F) return;

            float escala = tamanho / 18F;
            float x = bounds.Left + (bounds.Width - tamanho) / 2F;
            float y = bounds.Top + (bounds.Height - tamanho) / 2F;

            var oldSmoothing = graphics.SmoothingMode;
            var oldPixelOffset = graphics.PixelOffsetMode;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            try {
                using (Pen pen = new Pen(color, Math.Max(1F, 1.25F * escala)))
                using (Pen gridPen = new Pen(Color.FromArgb(115, color), Math.Max(0.85F, 0.95F * escala)))
                using (Brush dots = new SolidBrush(color))
                using (Brush headerFill = new SolidBrush(Color.FromArgb(235, 242, 255))) {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    gridPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    gridPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                    RectangleF body = new RectangleF(x + 2.7F * escala, y + 4.3F * escala,
                        12.6F * escala, 12.2F * escala);
                    using (var path = CriarRetanguloArredondado( Rectangle.Round(body), Math.Max(1.5F, 2F * escala))) {
                        graphics.DrawPath(pen, path);
                    }

                    graphics.FillRectangle(headerFill, x + 3.6F * escala, y + 5.2F * escala, 10.8F * escala, 2.5F * escala);
                    graphics.DrawLine(gridPen, x + 3.5F * escala, y + 8.4F * escala, x + 14.5F * escala, y + 8.4F * escala);

                    graphics.DrawLine(pen, x + 6.1F * escala, y + 2.2F * escala,
                        x + 6.1F * escala, y + 5.5F * escala);
                    graphics.DrawLine(pen, x + 11.9F * escala, y + 2.2F * escala,
                        x + 11.9F * escala, y + 5.5F * escala);

                    float d = 1.35F * escala;
                    float[,] dias = new float[,] {
                        { 5.4F, 10.2F }, { 9.0F, 10.2F },
                        { 12.0F, 10.2F }, { 5.4F, 13.1F },
                        { 9.0F, 13.1F }, { 12.0F, 13.1F }
                    };
                    for (int i = 0; i < dias.GetLength(0); i++)
                        graphics.FillEllipse(dots, x + dias[i, 0] * escala, y + dias[i, 1] * escala, d, d);
                }
            }
            finally {
                graphics.SmoothingMode = oldSmoothing;
                graphics.PixelOffsetMode = oldPixelOffset;
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath CriarRetanguloArredondado(Rectangle rect, float raio) {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            float diametro = raio * 2F;
            path.AddArc(rect.X, rect.Y, diametro, diametro, 180F, 90F);
            path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270F, 90F);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0F, 90F);
            path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90F, 90F);
            path.CloseFigure();
            return path;
        }

        private void PosicionarControlesFiltro() {
            if (periodoField != null && periodoField.ClientSize.Width > 30 && periodoField.ClientSize.Height > 4) {
                int h = periodoField.ClientSize.Height;
                int buttonWidth = 25;
                int buttonHeight = Math.Max(18, Math.Min(24, h - 4));
                btnCalendario.SetBounds(
                    periodoField.ClientSize.Width - buttonWidth - 2,
                    Math.Max(1, (h - buttonHeight) / 2), buttonWidth, buttonHeight);

                int textHeight = txtPeriodo.Height;
                txtPeriodo.SetBounds(6, Math.Max(0, (h - textHeight) / 2),
                    Math.Max(20, periodoField.ClientSize.Width - buttonWidth - 12), textHeight);
            }

            if (alunoField != null && txtAluno != null && alunoField.ClientSize.Width > 20 && alunoField.ClientSize.Height > 4) {
                int hAluno = alunoField.ClientSize.Height;
                int alturaTextoAluno = txtAluno.Height;
                txtAluno.SetBounds(6, Math.Max(0, (hAluno - alturaTextoAluno) / 2),
                    Math.Max(20, alunoField.ClientSize.Width - 12), alturaTextoAluno);
            }

            PosicionarComboNoCampo(turmaContentPanel, cmbTurma, btnSetaTurma);
            PosicionarComboNoCampo(statusContentPanel, cmbStatus, btnSetaStatus);
        }
        private static void PosicionarComboNoCampo(Panel campo, ComboBox combo, Button seta) {
            if (campo == null || combo == null || campo.ClientSize.Width < 30 || campo.ClientSize.Height < 12)
                return;

            const int larguraSeta = 24;
            int alturaDisponivel = Math.Max(18, campo.ClientSize.Height - 2);
            int altura = Math.Min(22, alturaDisponivel);
            int topo = Math.Max(0, (campo.ClientSize.Height - altura) / 2);
            combo.SetBounds(0, topo, campo.ClientSize.Width, altura);

            if (seta != null) {
                int alturaSeta = campo.ClientSize.Height;
                seta.SetBounds(Math.Max(0, campo.ClientSize.Width - larguraSeta),
                    0, larguraSeta, alturaSeta);
                seta.BringToFront();
            }
        }

        private void ConfigurarPopupPeriodo() {
            popupPeriodo = new Panel();
            popupPeriodo.Name = "popupPeriodoRuntime";
            popupPeriodo.Size = new Size(250, 194);
            popupPeriodo.BackColor = Color.FromArgb(226, 232, 240);
            popupPeriodo.BorderStyle = BorderStyle.None;
            popupPeriodo.Visible = false;
            popupPeriodo.Padding = new Padding(1);

            Panel popupContent = new Panel();
            popupContent.Name = "popupPeriodoContent";
            popupContent.Dock = DockStyle.Fill;
            popupContent.BackColor = Color.White;
            popupPeriodo.Controls.Add(popupContent);

            Label titulo = new Label();
            titulo.Text = "Selecionar período";
            titulo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            titulo.ForeColor = Color.FromArgb(17, 24, 39);
            titulo.BackColor = Color.Transparent;
            titulo.Location = new Point(10, 8);
            titulo.Size = new Size(190, 20);
            popupContent.Controls.Add(titulo);

            Label labelInicial = new Label();
            labelInicial.Text = "Data inicial";
            labelInicial.Font = new Font("Segoe UI", 7.5F);
            labelInicial.ForeColor = Color.FromArgb(100, 116, 139);
            labelInicial.Location = new Point(10, 38);
            labelInicial.Size = new Size(220, 15);
            popupContent.Controls.Add(labelInicial);

            campoDataInicial = CriarCampoData(new Point(10, 55), true);
            popupContent.Controls.Add(campoDataInicial);

            Label labelFinal = new Label();
            labelFinal.Text = "Data final";
            labelFinal.Font = new Font("Segoe UI", 7.5F);
            labelFinal.ForeColor = Color.FromArgb(100, 116, 139);
            labelFinal.Location = new Point(10, 87);
            labelFinal.Size = new Size(220, 15);
            popupContent.Controls.Add(labelFinal);

            campoDataFinal = CriarCampoData(new Point(10, 104), false);
            popupContent.Controls.Add(campoDataFinal);

            Button aplicar = new Button();
            aplicar.Text = "Aplicar período";
            aplicar.Location = new Point(10, 143);
            aplicar.Size = new Size(220, 31);
            aplicar.FlatStyle = FlatStyle.Flat;
            aplicar.FlatAppearance.BorderSize = 0;
            aplicar.BackColor = azul;
            aplicar.ForeColor = Color.White;
            aplicar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            aplicar.Cursor = Cursors.Hand;
            aplicar.Click += delegate { AplicarPeriodo(); };
            popupContent.Controls.Add(aplicar);

            paymentPagePanel.Controls.Add(popupPeriodo);
            popupPeriodo.BringToFront();

            CriarCalendarioFlutuante();
            paymentPagePanel.Controls.Add(calendarioFlutuante);
            calendarioFlutuante.BringToFront();

            filtroCliqueExterno = new FiltroFecharPopup(this, popupPeriodo, btnCalendario, calendarioFlutuante);
            Application.AddMessageFilter(filtroCliqueExterno);
        }

        private void CriarCalendarioFlutuante() {
            calendarioFlutuante = new Panel();
            calendarioFlutuante.Name = "calendarioDataPopup";
            calendarioFlutuante.Size = new Size(224, 196);
            calendarioFlutuante.BackColor = Color.White;
            calendarioFlutuante.BorderStyle = BorderStyle.None;
            calendarioFlutuante.Padding = new Padding(0);
            calendarioFlutuante.Visible = false;
            calendarioFlutuante.Paint += delegate(object sender, PaintEventArgs e) {
                using (Pen pen = new Pen(Color.FromArgb(203, 213, 225), 1F))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        Math.Max(0, calendarioFlutuante.ClientSize.Width - 1),
                        Math.Max(0, calendarioFlutuante.ClientSize.Height - 1));
            };

            Button anterior = CriarBotaoNavegacaoCalendario("‹", -1);
            anterior.Location = new Point(4, 3);
            calendarioFlutuante.Controls.Add(anterior);

            Button proximo = CriarBotaoNavegacaoCalendario("›", 1);
            proximo.Location = new Point(196, 3);
            calendarioFlutuante.Controls.Add(proximo);

            lblMesCalendario = new Label();
            lblMesCalendario.Name = "lblMesCalendario";
            lblMesCalendario.Location = new Point(28, 4);
            lblMesCalendario.Size = new Size(168, 21);
            lblMesCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblMesCalendario.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            lblMesCalendario.ForeColor = Color.FromArgb(55, 65, 81);
            lblMesCalendario.BackColor = Color.White;
            calendarioFlutuante.Controls.Add(lblMesCalendario);

            string[] diasSemana = { "dom", "seg", "ter", "qua", "qui", "sex", "sáb" };
            for (int coluna = 0; coluna < 7; coluna++) {
                Label diaSemana = new Label();
                diaSemana.Name = "lblDiaSemana" + coluna;
                diaSemana.Text = diasSemana[coluna];
                diaSemana.Location = new Point(5 + coluna * 30, 27);
                diaSemana.Size = new Size(29, 18);
                diaSemana.TextAlign = ContentAlignment.MiddleCenter;
                diaSemana.Font = new Font("Segoe UI", 7.2F, FontStyle.Regular);
                diaSemana.ForeColor = Color.FromArgb(71, 85, 105);
                diaSemana.BackColor = Color.White;
                calendarioFlutuante.Controls.Add(diaSemana);
            }

            botoesDiasCalendario.Clear();
            for (int indice = 0; indice < 42; indice++) {
                Button dia = new CalendarDayButton();
                dia.Name = "btnDiaCalendario" + indice;
                dia.Size = new Size(27, 20);
                dia.Location = new Point(6 + (indice % 7) * 30, 45 + (indice / 7) * 20);
                dia.Margin = new Padding(0);
                dia.Padding = new Padding(0);
                dia.TabStop = false;
                dia.FlatStyle = FlatStyle.Flat;
                dia.FlatAppearance.BorderSize = 0;
                dia.FlatAppearance.MouseOverBackColor = azul;
                dia.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
                dia.UseVisualStyleBackColor = false;
                dia.BackColor = Color.White;
                dia.Font = fonteDiaCalendario;
                dia.ForeColor = textoEscuro;
                dia.Cursor = Cursors.Hand;
                dia.Click += delegate {
                    if (dia.Tag is DateTime)
                        SelecionarDataCalendario((DateTime)dia.Tag);
                };
                dia.MouseEnter += delegate { EstilizarDiaCalendario(dia, true); };
                dia.MouseMove += delegate { EstilizarDiaCalendario(dia, true); };
                dia.MouseLeave += delegate { EstilizarDiaCalendario(dia, false); };
                botoesDiasCalendario.Add(dia);
                calendarioFlutuante.Controls.Add(dia);
            }

            lblHojeCalendario = new Label();
            lblHojeCalendario.Name = "lblHojeCalendario";
            // Reserva uma faixa para o rodapé do calendário
            lblHojeCalendario.Location = new Point(5, 171);
            lblHojeCalendario.Size = new Size(214, 20);
            lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");
            lblHojeCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblHojeCalendario.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105);
            lblHojeCalendario.BackColor = Color.White;
            lblHojeCalendario.Cursor = Cursors.Hand;
            lblHojeCalendario.TabStop = false;
            lblHojeCalendario.MouseEnter += delegate {
                lblHojeCalendario.ForeColor = azul;
                lblHojeCalendario.BackColor = azulClaro;
            };
            lblHojeCalendario.MouseLeave += delegate {
                lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105);
                lblHojeCalendario.BackColor = Color.White;
            };
            lblHojeCalendario.Click += delegate {
                if (campoDataAtivo != null)
                    SelecionarDataCalendario(DateTime.Today);
            };
            calendarioFlutuante.Controls.Add(lblHojeCalendario);

            mesCalendarioVisualizado = DateTime.Today;
            AtualizarDiasCalendario();
        }

        private Button CriarBotaoNavegacaoCalendario(string simbolo, int meses) {
            Button botao = new Button();
            botao.Text = simbolo;
            botao.Size = new Size(22, 22);
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            botao.UseVisualStyleBackColor = false;
            botao.BackColor = Color.White;
            botao.ForeColor = Color.FromArgb(71, 85, 105);
            botao.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            botao.Padding = new Padding(0);
            botao.Margin = new Padding(0);
            botao.TabStop = false;
            botao.Cursor = Cursors.Hand;
            botao.Click += delegate {
                mesCalendarioVisualizado = mesCalendarioVisualizado.AddMonths(meses);
                AtualizarDiasCalendario();
            };
            return botao;
        }

        private void AtualizarDiasCalendario() {
            if (lblMesCalendario != null)
                lblMesCalendario.Text = mesCalendarioVisualizado.ToString("MMMM 'de' yyyy", new CultureInfo("pt-BR"));
            if (lblHojeCalendario != null)
                lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");

            DateTime primeiroDia = new DateTime(mesCalendarioVisualizado.Year, mesCalendarioVisualizado.Month, 1);
            DateTime inicioGrade = primeiroDia.AddDays(-(int)primeiroDia.DayOfWeek);
            for (int i = 0; i < botoesDiasCalendario.Count; i++) {
                Button dia = botoesDiasCalendario[i];
                dia.Tag = inicioGrade.AddDays(i).Date;
                dia.Text = ((DateTime)dia.Tag).Day.ToString(CultureInfo.InvariantCulture);
                EstilizarDiaCalendario(dia, false);
            }
            if (calendarioFlutuante != null) calendarioFlutuante.Invalidate(true);
        }

        private void EstilizarDiaCalendario(Button botao, bool hover) {
            if (botao == null || !(botao.Tag is DateTime)) return;
            DateTime data = ((DateTime)botao.Tag).Date;
            bool hoje = data == DateTime.Today;
            DateTime? selecionada = campoDataAtivo == campoDataInicial
                ? dataInicialSelecionada : (campoDataAtivo == campoDataFinal ? dataFinalSelecionada : null);
            bool escolhida = selecionada.HasValue && selecionada.Value.Date == data;

            botao.FlatAppearance.BorderSize = 0;
            botao.Font = hoje ? fonteDiaCalendarioHoje : fonteDiaCalendario;
            if (hover) {
                botao.BackColor = azul;
                botao.ForeColor = Color.White;
                botao.Invalidate();
                return;
            }

            if (escolhida) {
                botao.BackColor = azulClaro;
                botao.ForeColor = azul;
                botao.FlatAppearance.BorderSize = 1;
                botao.FlatAppearance.BorderColor = Color.FromArgb(147, 197, 253);
            } else {
                botao.BackColor = Color.White;
                // O dia atual e o dia sob o mouse têm estilos distintos
                botao.ForeColor = data.Month != mesCalendarioVisualizado.Month || data.Year != mesCalendarioVisualizado.Year
                    ? Color.FromArgb(148, 163, 184)
                    : textoEscuro;
            }
            botao.Invalidate();
        }
        
        private void SelecionarDataCalendario(DateTime data) {
            data = data.Date;
            if (campoDataAtivo == campoDataInicial) {
                dataInicialSelecionada = data;
                if (txtDataInicial != null) {
                    txtDataInicial.Text = data.ToString("dd/MM/yyyy");
                    txtDataInicial.ForeColor = textoEscuro;
                }
            } else if (campoDataAtivo == campoDataFinal) {
                dataFinalSelecionada = data;
                if (txtDataFinal != null) {
                    txtDataFinal.Text = data.ToString("dd/MM/yyyy");
                    txtDataFinal.ForeColor = textoEscuro;
                }
            }

            calendarioFlutuante.Visible = false;
            campoDataAtivo = null;
        }

        private Panel CriarCampoData(Point location, bool dataInicial) {
            Panel campo = new Panel();
            campo.Name = dataInicial ? "campoDataInicial" : "campoDataFinal";
            campo.Location = location;
            campo.Size = new Size(220, 28);
            campo.BackColor = Color.White;
            campo.BorderStyle = BorderStyle.None;
            campo.Paint += CampoFiltro_Paint;

            TextBox texto = new TextBox();
            texto.Name = dataInicial ? "txtDataInicial" : "txtDataFinal";
            texto.ReadOnly = true;
            texto.BorderStyle = BorderStyle.None;
            texto.BackColor = Color.White;
            texto.ForeColor = textoEscuro;
            texto.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            texto.TextAlign = HorizontalAlignment.Left;
            texto.Location = new Point(8, 6);
            texto.Size = new Size(166, 16);
            texto.TabStop = false;
            DateTime? dataSelecionada = dataInicial ? dataInicialSelecionada : dataFinalSelecionada;
            texto.Text = dataSelecionada.HasValue ? dataSelecionada.Value.ToString("dd/MM/yyyy") : "Selecione uma data";
            texto.ForeColor = dataSelecionada.HasValue ? textoEscuro : textoPlaceholder;

            Button botaoCalendario = new Button();
            botaoCalendario.Name = dataInicial ? "btnCalendarioDataInicial" : "btnCalendarioDataFinal";
            botaoCalendario.Location = new Point(188, 1);
            botaoCalendario.Size = new Size(28, 26);
            ConfigurarBotaoIconeCalendario(botaoCalendario, TipoIconeCalendario.DataInicial);
            botaoCalendario.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            botaoCalendario.AccessibleName = dataInicial ? "Abrir calendário da data inicial" : "Abrir calendário da data final";
            botaoCalendario.Click += delegate { AlternarCalendarioData(campo, dataInicial); };

            campo.Controls.Add(texto);
            campo.Controls.Add(botaoCalendario);

            if (dataInicial) {
                txtDataInicial = texto;
                btnCalendarioInicial = botaoCalendario;
            } else {
                txtDataFinal = texto;
                btnCalendarioFinal = botaoCalendario;
            }

            return campo;
        }

        private void AlternarCalendarioData(Panel campo, bool dataInicial) {
            if (calendarioFlutuante == null || campo == null) return;
            if (calendarioFlutuante.Visible && campoDataAtivo == campo) {
                calendarioFlutuante.Visible = false;
                campoDataAtivo = null;
                return;
            }

            campoDataAtivo = campo;
            DateTime data = (dataInicial ? dataInicialSelecionada : dataFinalSelecionada) ?? DateTime.Today;
            mesCalendarioVisualizado = new DateTime(data.Year, data.Month, 1);
            AtualizarDiasCalendario();

            Point abaixoNaTela = campo.PointToScreen(new Point(0, campo.Height + 3));
            Point posicao = paymentPagePanel.PointToClient(abaixoNaTela);
            int margem = 4;
            if (posicao.X + calendarioFlutuante.Width > paymentPagePanel.ClientSize.Width - margem)
                posicao.X = paymentPagePanel.ClientSize.Width - calendarioFlutuante.Width - margem;
            if (posicao.Y + calendarioFlutuante.Height > paymentPagePanel.ClientSize.Height - margem) {
                Point acimaNaTela = campo.PointToScreen(new Point(0, -calendarioFlutuante.Height - 3));
                posicao = paymentPagePanel.PointToClient(acimaNaTela);
            }
            posicao.X = Math.Max(margem, posicao.X);
            posicao.Y = Math.Max(margem, posicao.Y);
            calendarioFlutuante.Location = posicao;
            calendarioFlutuante.Visible = true;
            calendarioFlutuante.BringToFront();
        }

        private void AlternarPopupPeriodo() {
            if (popupPeriodo == null) return;
            if (popupPeriodo.Visible) {
                FecharPopupPeriodo();
                return;
            }

            Rectangle bounds = periodoField.RectangleToScreen(periodoField.ClientRectangle);
            Point posicao = paymentPagePanel.PointToClient(new Point(bounds.Left, bounds.Bottom + 4));
            int x = Math.Max(8, Math.Min(posicao.X, paymentPagePanel.ClientSize.Width - popupPeriodo.Width - 8));
            int y = Math.Max(8, Math.Min(posicao.Y, paymentPagePanel.ClientSize.Height - popupPeriodo.Height - 8));
            popupPeriodo.Location = new Point(x, y);

            // Restaura o último intervalo aplicado, se houver
            if (dataInicialAplicada.HasValue && dataFinalAplicada.HasValue) {
                dataInicialSelecionada = dataInicialAplicada.Value;
                dataFinalSelecionada = dataFinalAplicada.Value;
            }
            if (txtDataInicial != null) {
                txtDataInicial.Text = dataInicialSelecionada.HasValue ? dataInicialSelecionada.Value.ToString("dd/MM/yyyy") : "Selecione uma data";
                txtDataInicial.ForeColor = dataInicialSelecionada.HasValue ? textoEscuro : textoPlaceholder;
            }
            if (txtDataFinal != null) {
                txtDataFinal.Text = dataFinalSelecionada.HasValue ? dataFinalSelecionada.Value.ToString("dd/MM/yyyy") : "Selecione uma data";
                txtDataFinal.ForeColor = dataFinalSelecionada.HasValue ? textoEscuro : textoPlaceholder;
            }

            popupPeriodo.Visible = true;
            popupPeriodo.BringToFront();
        }

        private void AplicarPeriodo() {
            if (!dataInicialSelecionada.HasValue || !dataFinalSelecionada.HasValue) {
                MessageBox.Show(this,
                    "Selecione a data inicial e a data final antes de aplicar o período.",
                    "Período incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                popupPeriodo.Visible = true;
                popupPeriodo.BringToFront();
                return;
            }

            DateTime inicial = dataInicialSelecionada.Value.Date;
            DateTime final = dataFinalSelecionada.Value.Date;
            if (final < inicial) {
                MessageBox.Show(this,
                    "A data final não pode ser anterior à data inicial. Corrija as datas e tente novamente.",
                    "Erro ao aplicar período", MessageBoxButtons.OK, MessageBoxIcon.Error);
                popupPeriodo.Visible = true;
                popupPeriodo.BringToFront();
                return;
            }

            dataInicialAplicada = inicial;
            dataFinalAplicada = final;
            txtPeriodo.Text = inicial.ToString("dd/MM/yyyy") + " - " + final.ToString("dd/MM/yyyy");
            txtPeriodo.ForeColor = textoEscuro;
            FecharPopupPeriodo();
        }

        public void FecharSeletores() {
            FecharPopupPeriodo();
        }

        private void FecharPopupPeriodo() {
            if (calendarioFlutuante != null) calendarioFlutuante.Visible = false;
            campoDataAtivo = null;
            if (popupPeriodo != null) popupPeriodo.Visible = false;
        }

        private void LimparFiltros() {
            txtAluno.Text = "Digite o nome do aluno...";
            txtAluno.ForeColor = textoPlaceholder;
            placeholderAluno = true;
            if (cmbTurma.Items.Count > 0) cmbTurma.SelectedIndex = 0;
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
            txtPeriodo.Text = "Selecione o período";
            txtPeriodo.ForeColor = textoPlaceholder;
            dataInicialAplicada = null;
            dataFinalAplicada = null;
            dataInicialSelecionada = null;
            dataFinalSelecionada = null;
            if (txtDataInicial != null) {
                txtDataInicial.Text = "Selecione uma data";
                txtDataInicial.ForeColor = textoPlaceholder;
            }
            if (txtDataFinal != null) {
                txtDataFinal.Text = "Selecione uma data";
                txtDataFinal.ForeColor = textoPlaceholder;
            }
            FecharPopupPeriodo();
        }

        private void AplicarPesquisaVisual() {
            //  Integração com o banco dados será feita depois
        }

        private void PreencherPagamentosDeExemplo() {
            gridPayments.Rows.Add("20/05/2024", "Maria Silva", "20/05/2024", "Hidroginástica", "Mai/2024", "Dinheiro", "180,00", "Pago", "");
            gridPayments.Rows.Add("18/05/2024", "João Pedro", "18/05/2024", "Natação Iniciante", "Mai/2024", "PIX", "180,00", "Pago", "");
            gridPayments.Rows.Add("15/05/2024", "Ana Clara", "22/05/2024", "Natação Intermediário", "Mai/2024", "Cartão de Débito", "180,00", "Pago", "");
            gridPayments.Rows.Add("14/05/2024", "Pedro Henrique", "21/05/2024", "Natação Avançado", "Mai/2024", "Dinheiro", "180,00", "Pago", "");
            gridPayments.Rows.Add("10/05/2024", "Laura Souza", "17/05/2024", "Natação Adulto", "Mai/2024", "PIX", "180,00", "Pago", "");
            gridPayments.Rows.Add("07/05/2024", "Gabriel Lima", "19/05/2024", "Natação Master", "Abr/Mai 2024", "Boleto Bancário", "360,00", "Pago", "");
            gridPayments.Rows.Add("05/05/2024", "Beatriz Oliveira", "16/05/2024", "Natação Competição", "Mai/2024", "Cartão de Crédito", "180,00", "Pago", "");
            gridPayments.Rows.Add("02/05/2024", "Miguel Ferreira", "15/05/2024", "Natação Iniciante", "Mai/2024", "Dinheiro", "180,00", "Pendente", "");
            gridPayments.Rows.Add("30/04/2024", "Sofia Martins", "14/05/2024", "Hidroginástica", "Abr/2024", "PIX", "180,00", "Pago", "");
            gridPayments.Rows.Add("28/04/2024", "Rafael Santos", "08/06/2024", "Natação Competição", "Abr/2024", "Boleto Bancário", "180,00", "Atrasado", "");
        }

        private void GridPayments_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == colStatus.Index) {
                e.PaintBackground(e.CellBounds, true);
                string status = Convert.ToString(e.FormattedValue);
                Color background = Color.FromArgb(232, 247, 237);
                Color foreground = Color.FromArgb(38, 149, 74);
                int width = 38;
                if (status == "Pendente") {
                    background = Color.FromArgb(255, 244, 217);
                    foreground = Color.FromArgb(196, 122, 0);
                    width = 54;
                } else if (status == "Atrasado") {
                    background = Color.FromArgb(254, 228, 226);
                    foreground = Color.FromArgb(217, 45, 32);
                    width = 49;
                }
                Rectangle pill = new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - width) / 2,
                    e.CellBounds.Top + (e.CellBounds.Height - 22) / 2, width, 22);
                using (Brush brush = new SolidBrush(background)) e.Graphics.FillRectangle(brush, pill);
                using (Font statusFont = new Font("Segoe UI", 7.5F))
                    TextRenderer.DrawText(e.Graphics, status, statusFont, pill, foreground,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                e.Handled = true;
            } else if (e.ColumnIndex == colAcoes.Index) {
                e.PaintBackground(e.CellBounds, true);

                const int actionSize = 26;
                const int actionGap = 7;
                int groupWidth = actionSize * 2 + actionGap;
                int startX = e.CellBounds.Left + Math.Max(2, (e.CellBounds.Width - groupWidth) / 2);
                int startY = e.CellBounds.Top + Math.Max(2, (e.CellBounds.Height - actionSize) / 2);
                Rectangle edit = new Rectangle(startX, startY, actionSize, actionSize);
                Rectangle delete = new Rectangle(edit.Right + actionGap, startY, actionSize, actionSize);

                using (Brush editFill = new SolidBrush(Color.FromArgb(247, 250, 255)))
                    e.Graphics.FillRectangle(editFill, edit);
                using (Pen editPen = new Pen(Color.FromArgb(219, 234, 254)))
                    e.Graphics.DrawRectangle(editPen, edit);
                using (Brush deleteFill = new SolidBrush(Color.FromArgb(255, 248, 248)))
                    e.Graphics.FillRectangle(deleteFill, delete);
                using (Pen deletePen = new Pen(Color.FromArgb(254, 226, 226)))
                    e.Graphics.DrawRectangle(deletePen, delete);

                using (Font actionFont = new Font("Segoe MDL2 Assets", 10F)) {
                    TextRenderer.DrawText(e.Graphics, "\uE104", actionFont, edit,
                        Color.FromArgb(25, 86, 196), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, "\uE107", actionFont, delete,
                        Color.FromArgb(220, 38, 38), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                }
                e.Handled = true;
            }
        }

        private void GridPayments_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex != colAcoes.Index) return;
            // As ações de edição e exclusão serão feitas depois
        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            if (filtroCliqueExterno != null) {
                Application.RemoveMessageFilter(filtroCliqueExterno);
                filtroCliqueExterno = null;
            }
            base.OnFormClosed(e);
        }

        private sealed class FiltroFecharPopup : IMessageFilter {
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_NCLBUTTONDOWN = 0x00A1;
            private readonly Form form;
            private readonly Control popup;
            private readonly Control acionador;
            private readonly Control popupSecundario;

            public FiltroFecharPopup(Form form, Control popup, Control acionador, Control popupSecundario) {
                this.form = form;
                this.popup = popup;
                this.acionador = acionador;
                this.popupSecundario = popupSecundario;
            }

            public bool PreFilterMessage(ref Message m) {
                if ((m.Msg != WM_LBUTTONDOWN && m.Msg != WM_NCLBUTTONDOWN) || popup == null || !popup.Visible)
                    return false;

                Control alvo = Control.FromHandle(m.HWnd);
                if (alvo != null && alvo.FindForm() != form) return false;
                if (alvo == null) {
                    if (!form.Bounds.Contains(Cursor.Position)) {
                        if (popupSecundario != null) popupSecundario.Visible = false;
                        popup.Visible = false;
                    }
                    return false;
                }

                if (EhFilhoDe(alvo, popupSecundario)) return false;
                if (EhFilhoDe(alvo, popup) || EhFilhoDe(alvo, acionador)) {
                    if (popupSecundario != null && popupSecundario.Visible && !EhFilhoDe(alvo, popupSecundario))
                        popupSecundario.Visible = false;
                    return false;
                }

                if (popupSecundario != null) popupSecundario.Visible = false;
                popup.Visible = false;
                return false;
            }

            private static bool EhFilhoDe(Control controle, Control pai) {
                if (controle == null || pai == null) return false;
                Control atual = controle;
                while (atual != null) {
                    if (atual == pai) return true;
                    atual = atual.Parent;
                }
                return false;
            }
        }
    }
}