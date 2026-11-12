using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class RelatoriosForm : Form {
        private static readonly Color verde = Color.FromArgb(22, 163, 74);
        private static readonly Color roxo = Color.FromArgb(147, 51, 234);

        private Label lblReceitaValor;
        private Label lblPresencaValor;
        private Label lblAlunosValor;
        private Label lblReceitaTotal;
        private Label lblReceitaRecebida;
        private Label lblReceitaPendente;
        private RevenueChartPanel graficoReceita;
        private AttendanceChartPanel graficoPresencaRelatorios;
        private ComboBox cmbPeriodoGrafico;
        private Label txtPeriodoRelatorio;
        private Control campoPeriodoRelatorio;
        private Panel popupPeriodoRelatorio;
        private DateTimePicker dataInicialRelatorio;
        private DateTimePicker dataFinalRelatorio;

        public RelatoriosForm() {
            InitializeComponent();
            campoPeriodoRelatorio = panelPeriodoRelatorio;
            panelPeriodoRelatorio.BorderStyle = BorderStyle.None;
            panelPeriodoRelatorio.Paint += PeriodoRelatorio_Paint;

            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            if (emDesignTime)
                return;

            ConfigurarPopupPeriodoRelatorio(painelConteudo);
            txtPeriodoRelatorio.Click += delegate { AlternarPopupPeriodoRelatorio(campoPeriodoRelatorio); };
            btnCalendarioPeriodoRelatorio.Click += delegate { AlternarPopupPeriodoRelatorio(campoPeriodoRelatorio); };
            cmbPeriodoGrafico.SelectedIndexChanged += delegate {
                if (graficoReceita != null) {
                    graficoReceita.QuantidadeMeses = cmbPeriodoGrafico.SelectedIndex == 1 ? 3 : 6;
                    graficoReceita.Invalidate();
                }
            };
        }

        private void PeriodoRelatorio_Paint(object sender, PaintEventArgs e) {
            Panel painel = sender as Panel;
            if (painel == null || painel.Width <= 1 || painel.Height <= 1)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, painel.Width - 1, painel.Height - 1);
            using (GraphicsPath caminho = CriarCaminhoArredondado(area, 5))
            using (SolidBrush fundo = new SolidBrush(Color.White))
            using (Pen borda = new Pen(VisualPadrao.BordaCampo, 1F)) {
                e.Graphics.FillPath(fundo, caminho);
                e.Graphics.DrawPath(borda, caminho);
            }
        }

        private void ConstruirRelatorio(bool emDesignTime) {
            BackColor = VisualPadrao.FundoPagina;
            painelConteudo.Controls.Clear();
            painelConteudo.BackColor = VisualPadrao.FundoPagina;
            painelConteudo.Padding = new Padding(0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Name = "layoutRelatorios";
            layout.Dock = DockStyle.Fill;
            layout.BackColor = VisualPadrao.FundoPagina;
            layout.Padding = new Padding(2, 0, 2, 2);
            layout.Margin = new Padding(0);
            layout.ColumnCount = 1;
            layout.RowCount = 4;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            layout.Controls.Add(CriarBarraSuperior(), 0, 0);
            layout.Controls.Add(CriarIndicadores(), 0, 1);
            layout.Controls.Add(CriarGraficos(), 0, 2);
            layout.Controls.Add(CriarResumoFinanceiro(), 0, 3);
            painelConteudo.Controls.Add(layout);
            if (!emDesignTime)
                ConfigurarPopupPeriodoRelatorio(painelConteudo);
        }

        private Control CriarBarraSuperior() {
            Panel barra = new Panel();
            barra.Dock = DockStyle.Fill;
            barra.BackColor = VisualPadrao.FundoPagina;
            barra.Margin = new Padding(0, 0, 0, 2);

            Label titulo = new Label();
            titulo.Text = "Visão geral";
            titulo.AutoSize = true;
            titulo.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = Color.FromArgb(30, 58, 138);
            titulo.Location = new Point(4, 7);
            barra.Controls.Add(titulo);

            FlowLayoutPanel acoes = new FlowLayoutPanel();
            acoes.Dock = DockStyle.Right;
            acoes.Width = 372;
            acoes.FlowDirection = FlowDirection.LeftToRight;
            acoes.WrapContents = false;
            acoes.Margin = new Padding(0);
            acoes.Padding = new Padding(0, 1, 0, 0);
            acoes.BackColor = VisualPadrao.FundoPagina;

            Label lblPeriodo = new Label();
            lblPeriodo.Text = "Período:";
            lblPeriodo.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblPeriodo.ForeColor = VisualPadrao.TextoSecundario;
            lblPeriodo.TextAlign = ContentAlignment.MiddleLeft;
            lblPeriodo.AutoSize = false;
            lblPeriodo.Size = new Size(58, 27);
            lblPeriodo.Margin = new Padding(0, 0, 4, 0);

            Panel campoPeriodo = new Panel();
            campoPeriodo.Name = "periodoFieldRelatorio";
            campoPeriodo.Size = new Size(142, 28);
            campoPeriodo.Margin = new Padding(0, 0, 8, 0);
            campoPeriodo.BackColor = Color.White;
            campoPeriodo.Paint += delegate(object sender, PaintEventArgs e) {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, campoPeriodo.Width - 1, campoPeriodo.Height - 1);
                using (GraphicsPath caminho = CriarCaminhoArredondado(area, 5))
                using (SolidBrush fundo = new SolidBrush(Color.White))
                using (Pen contorno = new Pen(Color.FromArgb(203, 213, 225), 1F)) {
                    e.Graphics.FillPath(fundo, caminho);
                    e.Graphics.DrawPath(contorno, caminho);
                }
            };
            txtPeriodoRelatorio = new Label();
            txtPeriodoRelatorio.Name = "txtPeriodoRelatorio";
            txtPeriodoRelatorio.Text = "Selecione o período";
            txtPeriodoRelatorio.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtPeriodoRelatorio.ForeColor = Color.FromArgb(148, 163, 184);
            txtPeriodoRelatorio.BackColor = Color.White;
            txtPeriodoRelatorio.TextAlign = ContentAlignment.MiddleLeft;
            txtPeriodoRelatorio.AutoEllipsis = true;
            txtPeriodoRelatorio.Location = new Point(8, 1);
            txtPeriodoRelatorio.Size = new Size(102, 26);
            txtPeriodoRelatorio.Cursor = Cursors.Hand;
            Button btnCalendarioPeriodo = new Button();
            btnCalendarioPeriodo.Name = "btnCalendarioPeriodoRelatorio";
            btnCalendarioPeriodo.Text = string.Empty;
            btnCalendarioPeriodo.Padding = new Padding(0);
            btnCalendarioPeriodo.Paint += CalendarioPeriodo_Paint;
            btnCalendarioPeriodo.MouseEnter += delegate { btnCalendarioPeriodo.Invalidate(); };
            btnCalendarioPeriodo.MouseLeave += delegate { btnCalendarioPeriodo.Invalidate(); };
            btnCalendarioPeriodo.ForeColor = Color.FromArgb(100, 116, 139);
            btnCalendarioPeriodo.BackColor = Color.White;
            btnCalendarioPeriodo.FlatStyle = FlatStyle.Flat;
            btnCalendarioPeriodo.FlatAppearance.BorderSize = 0;
            btnCalendarioPeriodo.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btnCalendarioPeriodo.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnCalendarioPeriodo.UseVisualStyleBackColor = false;
            btnCalendarioPeriodo.Size = new Size(27, 26);
            btnCalendarioPeriodo.Location = new Point(113, 1);
            btnCalendarioPeriodo.Cursor = Cursors.Hand;
            campoPeriodo.Controls.Add(txtPeriodoRelatorio);
            campoPeriodo.Controls.Add(btnCalendarioPeriodo);
            txtPeriodoRelatorio.Click += delegate { AlternarPopupPeriodoRelatorio(campoPeriodo); };
            btnCalendarioPeriodo.Click += delegate { AlternarPopupPeriodoRelatorio(campoPeriodo); };

            DownloadReportButton btnExportar = new DownloadReportButton();
            btnExportar.Name = "btnExportarRelatorio";
            btnExportar.Caption = "Exportar relatório";
            btnExportar.Text = string.Empty;
            btnExportar.AccessibleName = "Exportar relatório";
            btnExportar.CornerRadius = 5;
            btnExportar.Size = new Size(132, 28);
            btnExportar.Margin = new Padding(0, 0, 0, 0);
            btnExportar.Font = new Font("Segoe UI Semibold", 8.7F, FontStyle.Regular, GraphicsUnit.Point);
            btnExportar.BackColor = Color.White;
            btnExportar.ForeColor = VisualPadrao.TextoPrincipal;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.FlatAppearance.BorderSize = 1;
            btnExportar.FlatAppearance.BorderColor = VisualPadrao.BordaCartao;
            btnExportar.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 246, 250);
            btnExportar.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
            btnExportar.Cursor = Cursors.Hand;

            acoes.Controls.Add(lblPeriodo);
            acoes.Controls.Add(campoPeriodo);
            acoes.Controls.Add(btnExportar);
            barra.Controls.Add(acoes);
            campoPeriodoRelatorio = campoPeriodo;
            return barra;
        }

        private Control CriarIndicadores() {
            TableLayoutPanel indicadores = new TableLayoutPanel();
            indicadores.Dock = DockStyle.Fill;
            indicadores.BackColor = VisualPadrao.FundoPagina;
            indicadores.Margin = new Padding(0, 2, 0, 5);
            indicadores.Padding = new Padding(0);
            indicadores.ColumnCount = 3;
            indicadores.RowCount = 1;
            indicadores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            indicadores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            indicadores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));

            BorderPanel cartaoReceita = CriarIndicador("RECEITA DO MÊS", "R$ 23.450,00", verde, Color.FromArgb(226, 248, 235), "money");
            BorderPanel cartaoPresenca = CriarIndicador("PRESENÇA MÉDIA NO MÊS", "86%", Color.FromArgb(79, 70, 229), Color.FromArgb(235, 238, 255), "people");
            BorderPanel cartaoAlunos = CriarIndicador("ALUNOS ATIVOS", "128", roxo, Color.FromArgb(246, 233, 255), "student");

            lblReceitaValor = (Label)cartaoReceita.Controls.Find("valorIndicador", true)[0];
            lblPresencaValor = (Label)cartaoPresenca.Controls.Find("valorIndicador", true)[0];
            lblAlunosValor = (Label)cartaoAlunos.Controls.Find("valorIndicador", true)[0];

            cartaoReceita.Margin = new Padding(0, 0, 6, 0);
            cartaoPresenca.Margin = new Padding(3, 0, 3, 0);
            cartaoAlunos.Margin = new Padding(6, 0, 0, 0);
            indicadores.Controls.Add(cartaoReceita, 0, 0);
            indicadores.Controls.Add(cartaoPresenca, 1, 0);
            indicadores.Controls.Add(cartaoAlunos, 2, 0);
            return indicadores;
        }

        private BorderPanel CriarIndicador(string titulo, string valor, Color destaque, Color fundoIcone, string tipoIcone) {
            BorderPanel cartao = new BorderPanel();
            cartao.Dock = DockStyle.Fill;
            cartao.BackColor = Color.White;
            cartao.BorderColor = VisualPadrao.BordaCartao;
            cartao.CornerRadius = 9;
            cartao.Padding = new Padding(13, 8, 10, 8);

            TableLayoutPanel conteudo = new TableLayoutPanel();
            conteudo.Dock = DockStyle.Fill;
            conteudo.BackColor = Color.White;
            conteudo.Margin = new Padding(0);
            conteudo.ColumnCount = 2;
            conteudo.RowCount = 1;
            conteudo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 49F));
            conteudo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            IconBadge icone = new IconBadge();
            icone.Dock = DockStyle.None;
            icone.Size = new Size(39, 39);
            icone.Margin = new Padding(0);
            icone.AccentColor = destaque;
            icone.BadgeColor = fundoIcone;
            icone.IconType = tipoIcone;
            icone.Anchor = AnchorStyles.Left;

            TableLayoutPanel textos = new TableLayoutPanel();
            textos.Dock = DockStyle.Fill;
            textos.Margin = new Padding(0);
            textos.Padding = new Padding(0);
            textos.BackColor = Color.White;
            textos.ColumnCount = 1;
            textos.RowCount = 2;
            textos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            textos.RowStyles.Add(new RowStyle(SizeType.Percent, 46F));
            textos.RowStyles.Add(new RowStyle(SizeType.Percent, 54F));

            Label lblTituloIndicador = new Label();
            lblTituloIndicador.Text = titulo;
            lblTituloIndicador.Dock = DockStyle.Fill;
            lblTituloIndicador.Margin = new Padding(0, 0, 0, 1);
            lblTituloIndicador.TextAlign = ContentAlignment.BottomLeft;
            lblTituloIndicador.Font = new Font("Segoe UI Semibold", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            lblTituloIndicador.ForeColor = VisualPadrao.TextoSecundario;
            lblTituloIndicador.AutoEllipsis = true;

            Label lblValor = new Label();
            lblValor.Name = "valorIndicador";
            lblValor.Text = valor;
            lblValor.Dock = DockStyle.Fill;
            lblValor.Margin = new Padding(0, 0, 0, 0);
            lblValor.TextAlign = ContentAlignment.TopLeft;
            lblValor.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Regular, GraphicsUnit.Point);
            lblValor.ForeColor = destaque;
            lblValor.AutoEllipsis = true;

            textos.Controls.Add(lblTituloIndicador, 0, 0);
            textos.Controls.Add(lblValor, 0, 1);
            conteudo.Controls.Add(icone, 0, 0);
            conteudo.Controls.Add(textos, 1, 0);
            cartao.Controls.Add(conteudo);
            return cartao;
        }

        private Control CriarGraficos() {
            TableLayoutPanel graficos = new TableLayoutPanel();
            graficos.Dock = DockStyle.Fill;
            graficos.BackColor = VisualPadrao.FundoPagina;
            graficos.Margin = new Padding(0, 2, 0, 5);
            graficos.Padding = new Padding(0);
            graficos.ColumnCount = 2;
            graficos.RowCount = 1;
            graficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47F));
            graficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53F));

            BorderPanel cartaoPresenca = CriarCartaoGrafico("Percentual de Presença no Mês", false);
            cartaoPresenca.Margin = new Padding(0, 0, 6, 0);
            AttendanceChartPanel graficoPresenca = new AttendanceChartPanel();
            graficoPresenca.Dock = DockStyle.Fill;
            graficoPresenca.BackColor = Color.White;
            PrepararAreaGrafico(cartaoPresenca, graficoPresenca, false);

            BorderPanel cartaoReceita = CriarCartaoGrafico("Evolução da Receita (Últimos 6 meses)", true);
            cartaoReceita.Margin = new Padding(3, 0, 0, 0);
            graficoReceita = new RevenueChartPanel();
            graficoReceita.Dock = DockStyle.Fill;
            graficoReceita.BackColor = Color.White;
            PrepararAreaGrafico(cartaoReceita, graficoReceita, true);

            graficos.Controls.Add(cartaoPresenca, 0, 0);
            graficos.Controls.Add(cartaoReceita, 1, 0);
            return graficos;
        }

        private BorderPanel CriarCartaoGrafico(string titulo, bool comSeletor) {
            BorderPanel cartao = new BorderPanel();
            cartao.Dock = DockStyle.Fill;
            cartao.BackColor = Color.White;
            cartao.BorderColor = VisualPadrao.BordaCartao;
            cartao.CornerRadius = 9;
            cartao.Padding = new Padding(1);

            TableLayoutPanel estrutura = new TableLayoutPanel();
            estrutura.Name = comSeletor
                ? "estruturaGraficoReceita"
                : "estruturaGraficoPresenca";
            estrutura.Dock = DockStyle.Fill;
            estrutura.BackColor = Color.White;
            estrutura.Margin = new Padding(0);
            estrutura.ColumnCount = 1;
            estrutura.RowCount = 2;
            estrutura.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            estrutura.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            estrutura.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel cabecalho = new Panel();
            cabecalho.Dock = DockStyle.Fill;
            cabecalho.BackColor = Color.White;
            cabecalho.Padding = new Padding(12, 0, 10, 0);
            cabecalho.Margin = new Padding(0);

            Label lblTituloGrafico = new Label();
            lblTituloGrafico.Text = titulo;
            lblTituloGrafico.Dock = DockStyle.Fill;
            lblTituloGrafico.TextAlign = ContentAlignment.MiddleLeft;
            lblTituloGrafico.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblTituloGrafico.ForeColor = VisualPadrao.TextoPrincipal;
            lblTituloGrafico.AutoEllipsis = true;
            lblTituloGrafico.Margin = new Padding(0);
            cabecalho.Controls.Add(lblTituloGrafico);

            if (comSeletor) {
                lblTituloGrafico.Dock = DockStyle.None;
                lblTituloGrafico.AutoSize = false;
                lblTituloGrafico.Location = new Point(12, 0);
                lblTituloGrafico.Size = new Size(Math.Max(100, cabecalho.Width - 142), 34);
                lblTituloGrafico.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                Panel contornoSeletor = new Panel();
                contornoSeletor.Name = "contornoPeriodoGrafico";
                contornoSeletor.Size = new Size(120, 23);
                contornoSeletor.Location = new Point(-130, 5);
                contornoSeletor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                contornoSeletor.BackColor = Color.White;
                contornoSeletor.Margin = new Padding(0);
                contornoSeletor.Paint += delegate(object sender, PaintEventArgs e) {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle area = new Rectangle(0, 0, contornoSeletor.Width - 1, contornoSeletor.Height - 1);
                    using (GraphicsPath caminho = CriarCaminhoArredondado(area, 4))
                    using (Pen contorno = new Pen(VisualPadrao.BordaCartao, 1F))
                        e.Graphics.DrawPath(contorno, caminho);
                };

                cmbPeriodoGrafico = new ComboBox();
                cmbPeriodoGrafico.Name = "cmbPeriodoGrafico";
                cmbPeriodoGrafico.Dock = DockStyle.Fill;
                cmbPeriodoGrafico.Margin = new Padding(0);
                cmbPeriodoGrafico.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbPeriodoGrafico.FlatStyle = FlatStyle.Flat;
                cmbPeriodoGrafico.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
                cmbPeriodoGrafico.ForeColor = VisualPadrao.TextoPrincipal;
                cmbPeriodoGrafico.BackColor = Color.White;
                cmbPeriodoGrafico.Items.Add("Últimos 6 meses");
                cmbPeriodoGrafico.Items.Add("Últimos 3 meses");
                cmbPeriodoGrafico.SelectedIndex = 0;
                cmbPeriodoGrafico.SelectedIndexChanged += delegate {
                    if (graficoReceita != null) {
                        graficoReceita.QuantidadeMeses = cmbPeriodoGrafico.SelectedIndex == 1 ? 3 : 6;
                        graficoReceita.Invalidate();
                    }
                };
                // Recuo vertical para o ComboBox não cobrir as bordas superior e inferior
                contornoSeletor.Padding = new Padding(2, 2, 2, 2);
                contornoSeletor.Controls.Add(cmbPeriodoGrafico);
                cabecalho.Resize += delegate {
                    lblTituloGrafico.Width = Math.Max(100, cabecalho.ClientSize.Width - 142);
                    contornoSeletor.Left = cabecalho.ClientSize.Width - contornoSeletor.Width - 10;
                };
                cabecalho.Controls.Add(contornoSeletor);
            } else {
                Label info = new Label();
                info.Text = "ⓘ";
                info.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
                info.ForeColor = Color.FromArgb(148, 163, 184);
                info.AutoSize = true;
                info.Location = new Point(12 + TextRenderer.MeasureText(titulo, lblTituloGrafico.Font).Width + 5, 10);
                cabecalho.Controls.Add(info);
            }

            Panel areaGrafico = new Panel();
            areaGrafico.Name = comSeletor
                ? "areaGraficoReceita"
                : "areaGraficoPresenca";
            areaGrafico.Dock = DockStyle.Fill;
            areaGrafico.Margin = new Padding(0);
            areaGrafico.Padding = new Padding(0);
            areaGrafico.BackColor = Color.White;

            Panel divisor = new Panel();
            divisor.Dock = DockStyle.Top;
            divisor.Height = 1;
            divisor.BackColor = Color.FromArgb(245, 247, 250);
            divisor.Margin = new Padding(0);

            estrutura.Controls.Add(cabecalho, 0, 0);
            estrutura.Controls.Add(areaGrafico, 0, 1);
            cartao.Controls.Add(estrutura);
            cartao.Controls.Add(divisor);
            divisor.BringToFront();
            return cartao;
        }

        private void PrepararAreaGrafico(BorderPanel cartao, Control grafico, bool seletor) {
            string nomeArea = seletor
                ? "areaGraficoReceita"
                : "areaGraficoPresenca";
            Control[] areas = cartao.Controls.Find(nomeArea, true);
            if (areas.Length == 0)
                return;
            areas[0].Controls.Add(grafico);
            grafico.Dock = DockStyle.Fill;
            grafico.BackColor = Color.White;
        }

        private Control CriarResumoFinanceiro() {
            BorderPanel cartao = new BorderPanel();
            cartao.Dock = DockStyle.Fill;
            cartao.BackColor = Color.White;
            cartao.BorderColor = VisualPadrao.BordaCartao;
            cartao.CornerRadius = 9;
            cartao.Padding = new Padding(1);
            cartao.Margin = new Padding(0, 2, 0, 0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.White;
            layout.Margin = new Padding(0);
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label titulo = new Label();
            titulo.Text = "Resumo Financeiro";
            titulo.Dock = DockStyle.Fill;
            titulo.TextAlign = ContentAlignment.MiddleLeft;
            titulo.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = VisualPadrao.TextoPrincipal;
            titulo.Padding = new Padding(12, 0, 0, 0);
            titulo.Margin = new Padding(0);
            layout.Controls.Add(titulo, 0, 0);

            TableLayoutPanel tabela = new TableLayoutPanel();
            tabela.Dock = DockStyle.Fill;
            tabela.BackColor = Color.White;
            tabela.Margin = new Padding(0);
            tabela.Padding = new Padding(12, 0, 12, 8);
            tabela.ColumnCount = 3;
            tabela.RowCount = 2;
            tabela.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tabela.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tabela.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            tabela.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
            tabela.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));

            tabela.Controls.Add(CriarCelulaResumo("Receita Total (Mês)", "R$ 23.450,00", verde, false), 0, 0);
            tabela.Controls.Add(CriarCelulaResumo("Receitas Recebidas", "R$ 21.830,00", verde, true), 1, 0);
            tabela.Controls.Add(CriarCelulaResumo("Receitas Pendentes", "R$ 1.620,00", Color.FromArgb(217, 119, 6), true), 2, 0);
            tabela.SetRowSpan(tabela.Controls[0], 2);
            tabela.SetRowSpan(tabela.Controls[1], 2);
            tabela.SetRowSpan(tabela.Controls[2], 2);

            layout.Controls.Add(tabela, 0, 1);
            cartao.Controls.Add(layout);
            return cartao;
        }

        private Control CriarCelulaResumo(string titulo, string valor, Color cor, bool separadorEsquerdo) {
            Panel celula = new Panel();
            celula.Dock = DockStyle.Fill;
            celula.BackColor = Color.White;
            celula.Padding = new Padding(10, 4, 8, 4);
            celula.Margin = new Padding(0);
            celula.Paint += delegate(object sender, PaintEventArgs e) {
                if (separadorEsquerdo) {
                    using (Pen pen = new Pen(Color.FromArgb(238, 241, 245)))
                        e.Graphics.DrawLine(pen, 0, 5, 0, celula.Height - 5);
                }
            };

            Label lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 24;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitulo.ForeColor = VisualPadrao.TextoSecundario;
            lblTitulo.AutoEllipsis = true;

            Label lblValor = new Label();
            lblValor.Text = valor;
            lblValor.Dock = DockStyle.Top;
            lblValor.Height = 27;
            lblValor.TextAlign = ContentAlignment.TopLeft;
            lblValor.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblValor.ForeColor = cor;
            lblValor.AutoEllipsis = true;

            if (titulo == "Receita Total (Mês)")
                lblReceitaTotal = lblValor;
            else if (titulo == "Receitas Recebidas")
                lblReceitaRecebida = lblValor;
            else lblReceitaPendente = lblValor;

            celula.Controls.Add(lblValor);
            celula.Controls.Add(lblTitulo);
            return celula;
        }

        private void ConfigurarPopupPeriodoRelatorio(Control container) {
            BorderPanel popup = new BorderPanel();
            popupPeriodoRelatorio = popup;
            popup.Name = "popupPeriodoRelatorio";
            popup.Size = new Size(304, 162);
            popup.BackColor = Color.White;
            popup.BorderColor = VisualPadrao.BordaCartao;
            popup.CornerRadius = 9;
            popup.Padding = new Padding(1);
            popup.Visible = false;
            popup.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            Label titulo = new Label();
            titulo.Text = "Selecionar período";
            titulo.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = VisualPadrao.TextoPrincipal;
            titulo.Location = new Point(14, 9);
            titulo.Size = new Size(260, 21);
            popup.Controls.Add(titulo);

            Label subtitulo = new Label();
            subtitulo.Text = "Escolha as datas para consultar os relatórios.";
            subtitulo.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            subtitulo.ForeColor = VisualPadrao.TextoSecundario;
            subtitulo.Location = new Point(14, 30);
            subtitulo.Size = new Size(274, 17);
            popup.Controls.Add(subtitulo);

            Panel divisor = new Panel();
            divisor.BackColor = Color.FromArgb(241, 245, 249);
            divisor.Location = new Point(13, 52);
            divisor.Size = new Size(276, 1);
            popup.Controls.Add(divisor);

            Label inicial = new Label();
            inicial.Text = "Data inicial";
            inicial.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            inicial.ForeColor = VisualPadrao.TextoSecundario;
            inicial.Location = new Point(14, 61);
            inicial.Size = new Size(128, 15);
            popup.Controls.Add(inicial);

            Label final = new Label();
            final.Text = "Data final";
            final.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            final.ForeColor = VisualPadrao.TextoSecundario;
            final.Location = new Point(158, 61);
            final.Size = new Size(128, 15);
            popup.Controls.Add(final);

            dataInicialRelatorio = new DateTimePicker();
            dataInicialRelatorio.Name = "dataInicialRelatorio";
            dataInicialRelatorio.Format = DateTimePickerFormat.Short;
            dataInicialRelatorio.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            dataInicialRelatorio.Location = new Point(14, 78);
            dataInicialRelatorio.Size = new Size(128, 25);
            dataInicialRelatorio.CalendarForeColor = VisualPadrao.TextoPrincipal;
            dataInicialRelatorio.CalendarTitleBackColor = VisualPadrao.Azul;
            dataInicialRelatorio.CalendarTitleForeColor = Color.White;
            dataInicialRelatorio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            popup.Controls.Add(dataInicialRelatorio);

            dataFinalRelatorio = new DateTimePicker();
            dataFinalRelatorio.Name = "dataFinalRelatorio";
            dataFinalRelatorio.Format = DateTimePickerFormat.Short;
            dataFinalRelatorio.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            dataFinalRelatorio.Location = new Point(158, 78);
            dataFinalRelatorio.Size = new Size(128, 25);
            dataFinalRelatorio.CalendarForeColor = VisualPadrao.TextoPrincipal;
            dataFinalRelatorio.CalendarTitleBackColor = VisualPadrao.Azul;
            dataFinalRelatorio.CalendarTitleForeColor = Color.White;
            dataFinalRelatorio.Value = DateTime.Today;
            popup.Controls.Add(dataFinalRelatorio);

            Button cancelar = new Button();
            cancelar.Name = "btnCancelarPeriodoRelatorio";
            cancelar.Text = "Cancelar";
            cancelar.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            cancelar.FlatStyle = FlatStyle.Flat;
            cancelar.FlatAppearance.BorderColor = VisualPadrao.BordaCartao;
            cancelar.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
            cancelar.BackColor = Color.White;
            cancelar.ForeColor = VisualPadrao.TextoPrincipal;
            cancelar.UseVisualStyleBackColor = false;
            cancelar.Size = new Size(82, 28);
            cancelar.Location = new Point(112, 119);
            cancelar.Cursor = Cursors.Hand;
            cancelar.Click += delegate { popupPeriodoRelatorio.Visible = false; };
            popup.Controls.Add(cancelar);

            RoundedButton aplicar = new RoundedButton();
            aplicar.Name = "btnAplicarPeriodoRelatorio";
            aplicar.Text = "Aplicar período";
            aplicar.CornerRadius = 5;
            aplicar.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Regular, GraphicsUnit.Point);
            aplicar.BackColor = VisualPadrao.Azul;
            aplicar.ForeColor = Color.White;
            aplicar.Size = new Size(116, 28);
            aplicar.Location = new Point(200, 119);
            aplicar.Cursor = Cursors.Hand;
            aplicar.Click += delegate {
                if (dataFinalRelatorio.Value.Date < dataInicialRelatorio.Value.Date) {
                    MessageBox.Show(this, "A data final não pode ser anterior à data inicial.", "Período inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    popupPeriodoRelatorio.Visible = true;
                    popupPeriodoRelatorio.BringToFront();
                    return;
                }
                txtPeriodoRelatorio.Text = dataInicialRelatorio.Value.ToString("dd/MM/yyyy") + " - " + dataFinalRelatorio.Value.ToString("dd/MM/yyyy");
                txtPeriodoRelatorio.ForeColor = VisualPadrao.TextoPrincipal;
                popupPeriodoRelatorio.Visible = false;
            };
            popup.Controls.Add(aplicar);

            container.Controls.Add(popupPeriodoRelatorio);
            popupPeriodoRelatorio.BringToFront();
        }

        private void AlternarPopupPeriodoRelatorio(Control campoPeriodo) {
            if (popupPeriodoRelatorio == null || campoPeriodo == null)
                return;
            if (popupPeriodoRelatorio.Visible) {
                popupPeriodoRelatorio.Visible = false;
                return;
            }

            Rectangle campoNaTela = campoPeriodo.RectangleToScreen(campoPeriodo.ClientRectangle);
            Point abaixo = popupPeriodoRelatorio.Parent.PointToClient(new Point(campoNaTela.Left, campoNaTela.Bottom + 5));
            int margem = 8;
            int x = Math.Max(margem, Math.Min(abaixo.X,
                popupPeriodoRelatorio.Parent.ClientSize.Width - popupPeriodoRelatorio.Width - margem));
            int y = abaixo.Y;

            if (y + popupPeriodoRelatorio.Height > popupPeriodoRelatorio.Parent.ClientSize.Height - margem) {
                Point acima = popupPeriodoRelatorio.Parent.PointToClient(
                    new Point(campoNaTela.Left, campoNaTela.Top - popupPeriodoRelatorio.Height - 5));
                y = acima.Y;
            }
            y = Math.Max(margem, Math.Min(y,
                popupPeriodoRelatorio.Parent.ClientSize.Height - popupPeriodoRelatorio.Height - margem));

            popupPeriodoRelatorio.Location = new Point(x, y);
            popupPeriodoRelatorio.Visible = true;
            popupPeriodoRelatorio.BringToFront();
        }

        private void CalendarioPeriodo_Paint(object sender, PaintEventArgs e) {
            Button botao = sender as Button;
            if (botao == null)
                return;

            bool hover = botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position));
            Color cor = hover ? Color.FromArgb(29, 78, 216) : Color.FromArgb(55, 96, 160);
            Graphics g = e.Graphics;
            SmoothingMode suavizacaoAnterior = g.SmoothingMode;
            PixelOffsetMode pixelAnterior = g.PixelOffsetMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            try {
                float tamanho = Math.Min(18F, Math.Min(botao.ClientSize.Width - 4F, botao.ClientSize.Height - 4F));
                if (tamanho < 12F)
                    return;
                float escala = tamanho / 18F;
                float x = (botao.ClientSize.Width - tamanho) / 2F;
                float y = (botao.ClientSize.Height - tamanho) / 2F;
                using (Pen caneta = new Pen(cor, Math.Max(1F, 1.25F * escala)))
                using (Pen grade = new Pen(Color.FromArgb(115, cor), Math.Max(0.85F, 0.95F * escala)))
                using (Brush pontos = new SolidBrush(cor))
                using (Brush cabecalho = new SolidBrush(Color.FromArgb(235, 242, 255))) {
                    caneta.StartCap = LineCap.Round;
                    caneta.EndCap = LineCap.Round;
                    grade.StartCap = LineCap.Round;
                    grade.EndCap = LineCap.Round;
                    RectangleF corpo = new RectangleF(x + 2.7F * escala, y + 4.3F * escala,
                        12.6F * escala, 12.2F * escala);
                    using (GraphicsPath caminho = CriarCaminhoArredondado(Rectangle.Round(corpo), Math.Max(1, (int)(2F * escala))))
                        g.DrawPath(caneta, caminho);
                    g.FillRectangle(cabecalho, x + 3.6F * escala, y + 5.2F * escala, 10.8F * escala, 2.5F * escala);
                    g.DrawLine(grade, x + 3.5F * escala, y + 8.4F * escala, x + 14.5F * escala, y + 8.4F * escala);
                    g.DrawLine(caneta, x + 6.1F * escala, y + 2.2F * escala, x + 6.1F * escala, y + 5.5F * escala);
                    g.DrawLine(caneta, x + 11.9F * escala, y + 2.2F * escala, x + 11.9F * escala, y + 5.5F * escala);
                    float[,] dias = new float[,] {
                        { 5.4F, 10.2F }, { 9.0F, 10.2F }, { 12.0F, 10.2F },
                        { 5.4F, 13.1F }, { 9.0F, 13.1F }, { 12.0F, 13.1F }
                    };
                    float diametro = 1.35F * escala;
                    for (int i = 0; i < dias.GetLength(0); i++)
                        g.FillEllipse(pontos, x + dias[i, 0] * escala, y + dias[i, 1] * escala, diametro, diametro);
                }

            } finally {
                g.SmoothingMode = suavizacaoAnterior;
                g.PixelOffsetMode = pixelAnterior;
            }
        }

        private sealed class DownloadReportButton : RoundedButton {
            public string Caption {
                get;
                set;
            }

            public DownloadReportButton() {
                Caption = "Exportar relatório";
                Text = string.Empty;
                TextAlign = ContentAlignment.MiddleLeft;
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen seta = new Pen(ForeColor, 1.55F)) {
                    seta.StartCap = LineCap.Round;
                    seta.EndCap = LineCap.Round;
                    seta.LineJoin = LineJoin.Round;
                    // Ícone de download
                    g.DrawLine(seta, 17F, 7F, 17F, 14F);
                    g.DrawLine(seta, 13.5F, 10.8F, 17F, 14.2F);
                    g.DrawLine(seta, 17F, 14.2F, 20.5F, 10.8F);
                    g.DrawLine(seta, 12.8F, 20F, 21.2F, 20F);
                }

                Rectangle areaTexto = new Rectangle(30, 2, Math.Max(0, Width - 34), Math.Max(0, Height - 4));
                TextRenderer.DrawText(g, Caption, Font, areaTexto, ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            }
        }

        private sealed class BorderPanel : Panel {
            public Color BorderColor {
                get;
                set;
            }
            public int CornerRadius {
                get;
                set;
            }

            public BorderPanel() {
                BorderColor = Color.FromArgb(229, 234, 241);
                CornerRadius = 7;
                DoubleBuffered = true;
                ResizeRedraw = true;
                Padding = new Padding(1);
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, Math.Max(0, Width - 1), Math.Max(0, Height - 1));
                using (GraphicsPath path = CriarCaminhoArredondado(rect, CornerRadius))
                using (Pen pen = new Pen(BorderColor, 1F))
                    e.Graphics.DrawPath(pen, path);
            }
        }

        private sealed class IconBadge : Panel {
            public Color AccentColor {
                get;
                set;
            }
            public Color BadgeColor {
                get;
                set;
            }
            public string IconType {
                get;
                set;
            }

            public IconBadge() {
                DoubleBuffered = true;
                AccentColor = Color.FromArgb(22, 163, 74);
                BadgeColor = Color.FromArgb(226, 248, 235);
                IconType = "money";
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float d = Math.Min(Width, Height) - 2;
                RectangleF circle = new RectangleF((Width - d) / 2, (Height - d) / 2, d, d);
                using (SolidBrush brush = new SolidBrush(BadgeColor)) g.FillEllipse(brush, circle);
                using (Pen pen = new Pen(AccentColor, 1.7F)) {
                    if (IconType == "money") {
                        RectangleF coin = new RectangleF(Width / 2F - 8, Height / 2F - 8, 16, 16);
                        g.DrawEllipse(pen, coin);
                        using (Font font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point))
                        using (SolidBrush brush = new SolidBrush(AccentColor))
                            g.DrawString("$", font, brush, new PointF(Width / 2F - 4.7F, Height / 2F - 7.5F));

                    } else if (IconType == "people") {
                        // Ícone de presença
                        pen.Width = 1.8F;
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        pen.LineJoin = LineJoin.Round;
                        float cx = Width / 2F + 0.5F;
                        float cy = Height / 2F + 0.5F;

                        g.DrawEllipse(pen, cx - 4F, cy - 8.5F, 8F, 8F);

                        using (GraphicsPath ombros = new GraphicsPath()) {
                            ombros.StartFigure();
                            ombros.AddBezier(
                                new PointF(cx - 9.5F, cy + 7.5F),
                                new PointF(cx - 9F, cy + 2.5F),
                                new PointF(cx - 5.5F, cy + 1F),
                                new PointF(cx, cy + 1F));
                            ombros.AddBezier(
                                new PointF(cx, cy + 1F),
                                new PointF(cx + 5.5F, cy + 1F),
                                new PointF(cx + 9F, cy + 2.5F),
                                new PointF(cx + 9.5F, cy + 7.5F));
                            g.DrawPath(pen, ombros);
                        }

                    } else {
                        PointF[] cap = new PointF[] {
                            new PointF(Width / 2F, Height / 2F - 10),
                            new PointF(Width / 2F + 10, Height / 2F - 4),
                            new PointF(Width / 2F, Height / 2F + 2),
                            new PointF(Width / 2F - 10, Height / 2F - 4)
                        };
                        g.DrawPolygon(pen, cap);
                        g.DrawLine(pen, Width / 2F + 7, Height / 2F - 1, Width / 2F + 7, Height / 2F + 6);
                        g.DrawArc(pen, Width / 2F - 8, Height / 2F + 2, 16, 13, 185, 170);
                    }
                }
            }
        }

        private sealed class AttendanceChartPanel : Panel {
            private readonly Color corPresentes = Color.FromArgb(22, 163, 74);
            private readonly Color corAusentes = Color.FromArgb(239, 68, 68);
            private readonly Color corJustificados = Color.FromArgb(245, 158, 11);

            public AttendanceChartPanel() {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int area = Math.Max(54, Math.Min(136, Math.Min(Height - 35, Width / 3)));
                int x = 15;
                int y = Math.Max(8, (Height - area) / 2 - 1);
                Rectangle circulo = new Rectangle(x, y, area, area);
                float inicio = -90F;
                float[] valores = new float[] { 86F, 8F, 6F };
                Color[] cores = new Color[] { corPresentes, corAusentes, corJustificados };

                for (int i = 0; i < valores.Length; i++) {
                    float angulo = valores[i] / 100F * 360F;
                    using (SolidBrush brush = new SolidBrush(cores[i]))
                        g.FillPie(brush, circulo, inicio, angulo);
                    inicio += angulo;
                }

                int interior = (int)(area * 0.61F);
                Rectangle centro = new Rectangle(x + (area - interior) / 2, y + (area - interior) / 2, interior, interior);
                using (SolidBrush brush = new SolidBrush(Color.White)) g.FillEllipse(brush, centro);
                using (Font fonte = new Font("Segoe UI Semibold", Math.Max(10F, area / 8.3F), FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush brush = new SolidBrush(corPresentes)) {
                    string textoCentro = "86%";
                    SizeF tam = g.MeasureString(textoCentro, fonte);
                    g.DrawString(textoCentro, fonte, brush, centro.Left + (centro.Width - tam.Width) / 2, centro.Top + (centro.Height - tam.Height) / 2 - 1);
                }

                int legendaX = x + area + 16;
                int larguraLegenda = Math.Max(100, Width - legendaX - 8);
                int alturaItem = Math.Max(32, Math.Min(43, (Height - 20) / 3));
                int yLegenda = Math.Max(10, (Height - alturaItem * 3 - 14) / 2);
                DesenharItemLegenda(g, legendaX, yLegenda, larguraLegenda, corPresentes, "Presentes", "86%  (1.102)");
                DesenharItemLegenda(g, legendaX, yLegenda + alturaItem, larguraLegenda, corAusentes, "Ausentes", "8%  (102)");
                DesenharItemLegenda(g, legendaX, yLegenda + alturaItem * 2, larguraLegenda, corJustificados, "Justificadas", "6%  (76)");
                using (Font fonteTotal = new Font("Segoe UI", 7.2F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush brush = new SolidBrush(VisualPadrao.TextoSecundario))
                    g.DrawString("Total: 1.280 presenças", fonteTotal, brush, legendaX, Math.Min(Height - 15, yLegenda + alturaItem * 3 + 1));
            }

            private void DesenharItemLegenda(Graphics g, int x, int y, int largura, Color cor, string titulo, string detalhe) {
                using (SolidBrush ponto = new SolidBrush(cor)) g.FillEllipse(ponto, x, y + 5, 6, 6);
                using (Font fonteTitulo = new Font("Segoe UI Semibold", 7.7F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush brushTitulo = new SolidBrush(Color.FromArgb(55, 65, 81)))
                    g.DrawString(titulo, fonteTitulo, brushTitulo, x + 12, y);
                using (Font fonteDetalhe = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush brushDetalhe = new SolidBrush(VisualPadrao.TextoSecundario))
                    g.DrawString(detalhe, fonteDetalhe, brushDetalhe, x + 12, y + 15, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap });
            }
        }

        private sealed class RevenueChartPanel : Panel {
            private readonly string[] meses = new string[] {
                "Dez/23", "Jan/24", "Fev/24", "Mar/24", "Abr/24", "Mai/24"
            };
            private readonly double[] valores = new double[] {
                15400, 17800, 19500, 21230, 19750, 23450
            };
            public int QuantidadeMeses {
                get;
                set;
            }

            public RevenueChartPanel() {
                DoubleBuffered = true;
                ResizeRedraw = true;
                QuantidadeMeses = 6;
            }

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int left = 68;
                int right = 12;
                int top = 27;
                int bottom = 32;
                int larguraGrafico = Math.Max(40, Width - left - right);
                int alturaGrafico = Math.Max(35, Height - top - bottom);
                double maximo = 25000D;
                using (Font fonteEixo = new Font("Segoe UI", 6.6F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font fonteValor = new Font("Segoe UI Semibold", 6.7F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font fonteMes = new Font("Segoe UI", 6.8F, FontStyle.Regular, GraphicsUnit.Point))
                using (Pen grade = new Pen(Color.FromArgb(235, 239, 244), 1F))
                using (SolidBrush brushEixo = new SolidBrush(VisualPadrao.TextoSecundario))
                using (SolidBrush brushValor = new SolidBrush(VisualPadrao.TextoPrincipal)) {
                    for (int i = 0; i <= 5; i++) {
                        int yy = top + alturaGrafico - (int)Math.Round(alturaGrafico * i / 5D);
                        g.DrawLine(grade, left, yy, left + larguraGrafico, yy);
                        if (i == 0)
                            continue;
                        string textoEixo = "R$ " + (i * 5000).ToString("#,##0", CultureInfo.GetCultureInfo("pt-BR"));
                        SizeF sz = g.MeasureString(textoEixo, fonteEixo);
                        g.DrawString(textoEixo, fonteEixo, brushEixo, Math.Max(0, left - sz.Width - 7), yy - sz.Height / 2F);
                    }

                    int quantidade = Math.Max(1, Math.Min(6, QuantidadeMeses));
                    int inicioDados = 6 - quantidade;
                    float passo = larguraGrafico / (float)quantidade;
                    float larguraBarra = Math.Min(34F, passo * 0.52F);
                    for (int i = 0; i < quantidade; i++) {
                        int indice = inicioDados + i;
                        float altura = (float)(valores[indice] / maximo * alturaGrafico);
                        float bx = left + passo * i + (passo - larguraBarra) / 2F;
                        float by = top + alturaGrafico - altura;
                        Color corBarra = indice == 5 ? Color.FromArgb(54, 99, 214) : Color.FromArgb(158, 194, 244);
                        using (SolidBrush barra = new SolidBrush(corBarra))
                            g.FillRectangle(barra, bx, by, larguraBarra, Math.Max(1, altura));

                        string rotuloValor = "R$ " + valores[indice].ToString("#,##0", CultureInfo.GetCultureInfo("pt-BR"));
                        SizeF tamanhoValor = g.MeasureString(rotuloValor, fonteValor);
                        float valorX = Math.Max(left - 4, Math.Min(bx + larguraBarra / 2F - tamanhoValor.Width / 2F, left + larguraGrafico - tamanhoValor.Width));
                        g.DrawString(rotuloValor, fonteValor, brushValor, valorX, Math.Max(1, by - tamanhoValor.Height - 2));

                        SizeF tamanhoMes = g.MeasureString(meses[indice], fonteMes);
                        g.DrawString(meses[indice], fonteMes, brushEixo, bx + larguraBarra / 2F - tamanhoMes.Width / 2F, top + alturaGrafico + 7);
                    }
                }
            }
        }

        private static GraphicsPath CriarCaminhoArredondado(Rectangle bounds, int radius) {
            GraphicsPath path = new GraphicsPath();
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return path;
            int diametro = Math.Max(1, Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height)));
            Rectangle arco = new Rectangle(bounds.X, bounds.Y, diametro, diametro);
            path.AddArc(arco, 180, 90);
            arco.X = bounds.Right - diametro;
            path.AddArc(arco, 270, 90);
            arco.Y = bounds.Bottom - diametro;
            path.AddArc(arco, 0, 90);
            arco.X = bounds.X;
            path.AddArc(arco, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
