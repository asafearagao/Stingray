using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    [System.ComponentModel.DesignerCategory("Form")]

    // Monta os relatórios e controla a seleção do período consultado
    public partial class RelatoriosForm : Form {
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

        // Construtor do relatório
        public RelatoriosForm() {
            InitializeComponent();
            campoPeriodoRelatorio = panelPeriodoRelatorio;
            panelPeriodoRelatorio.BorderStyle = BorderStyle.None;
            panelPeriodoRelatorio.Paint += PeriodoRelatorio_Paint;

            ConfigurarAparenciaRelatorios();

            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);
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

        // Organiza os elementos visuais do relatório sem interferir nas consultas
        private void ConfigurarAparenciaRelatorios() {
            BackColor = VisualPadrao.FundoPagina;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            lblTitulo.Location = new Point(14, 11);
            lblTitulo.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitulo.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitulo.Text = "Relatórios";

            painelConteudo.BackColor = VisualPadrao.FundoPagina;
            painelConteudo.Location = new Point(12, 51);
            painelConteudo.Padding = new Padding(0);
            layoutRelatorios.BackColor = VisualPadrao.FundoPagina;
            layoutRelatorios.Padding = new Padding(3, 0, 3, 3);
            layoutRelatorios.RowStyles[0].Height = 40F;
            layoutRelatorios.RowStyles[1].Height = 126F;
            layoutRelatorios.RowStyles[2].SizeType = SizeType.Percent;
            layoutRelatorios.RowStyles[2].Height = 59F;
            layoutRelatorios.RowStyles[3].SizeType = SizeType.Percent;
            layoutRelatorios.RowStyles[3].Height = 41F;

            barraSuperiorRelatorios.BackColor = VisualPadrao.FundoPagina;
            lblVisaoGeralRelatorios.Visible = false;
            lblVisaoGeralRelatorios.Location = new Point(5, 3);
            lblVisaoGeralRelatorios.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblVisaoGeralRelatorios.ForeColor = VisualPadrao.TextoPrincipal;
            lblVisaoGeralRelatorios.Text = "Visão geral";

            acoesRelatorios.Dock = DockStyle.Right;
            acoesRelatorios.Width = 408;
            acoesRelatorios.Height = 54;
            acoesRelatorios.Padding = new Padding(0, 0, 0, 0);
            acoesRelatorios.FlowDirection = FlowDirection.LeftToRight;
            acoesRelatorios.WrapContents = false;

            lblPeriodoRelatorios.Text = "PERÍODO";
            lblPeriodoRelatorios.Width = 55;
            lblPeriodoRelatorios.Height = 34;
            lblPeriodoRelatorios.Margin = new Padding(0, 0, 5, 0);
            lblPeriodoRelatorios.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblPeriodoRelatorios.ForeColor = VisualPadrao.TextoSecundario;

            panelPeriodoRelatorio.Size = new Size(194, 34);
            panelPeriodoRelatorio.Margin = new Padding(0, 0, 9, 0);
            panelPeriodoRelatorio.BackColor = Color.White;
            panelPeriodoRelatorio.BorderStyle = BorderStyle.None;
            txtPeriodoRelatorio.Location = new Point(10, 2);
            txtPeriodoRelatorio.Size = new Size(148, 29);
            txtPeriodoRelatorio.Font = new Font("Segoe UI", 8.2F, FontStyle.Regular, GraphicsUnit.Point);
            btnCalendarioPeriodoRelatorio.Location = new Point(160, 2);
            btnCalendarioPeriodoRelatorio.Size = new Size(30, 29);
            btnCalendarioPeriodoRelatorio.BackColor = Color.White;

            btnExportarRelatorio.Size = new Size(138, 34);
            btnExportarRelatorio.Margin = new Padding(0);
            btnExportarRelatorio.CornerRadius = 7;
            btnExportarRelatorio.Font = new Font("Segoe UI Semibold", 8.3F, FontStyle.Regular, GraphicsUnit.Point);
            btnExportarRelatorio.BackColor = Color.White;
            btnExportarRelatorio.ForeColor = VisualPadrao.Azul;
            btnExportarRelatorio.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            btnExportarRelatorio.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;

            linhaIndicadoresRelatorios.BackColor = VisualPadrao.FundoPagina;
            linhaIndicadoresRelatorios.Margin = new Padding(0, 2, 0, 7);
            lblTituloReceitaRelatorios.Text = "RECEITA NO PERÍODO";
            lblTituloPresencaRelatorios.Text = "PRESENÇA MÉDIA";
            lblTituloAlunosRelatorios.Text = "ALUNOS CADASTRADOS";
            AjustarCartaoIndicador(cartaoReceitaRelatorios, badgeReceitaRelatorios,
                lblTituloReceitaRelatorios, lblReceitaValor,
                "Consolidado do período", Color.FromArgb(22, 163, 74));
            AjustarCartaoIndicador(cartaoPresencaRelatorios, badgePresencaRelatorios,
                lblTituloPresencaRelatorios, lblPresencaValor,
                "Frequência das aulas", Color.FromArgb(79, 70, 229));
            AjustarCartaoIndicador(cartaoAlunosRelatorios, badgeAlunosRelatorios,
                lblTituloAlunosRelatorios, lblAlunosValor,
                "Base de alunos cadastrados", Color.FromArgb(147, 51, 234));

            cartaoReceitaRelatorios.Margin = new Padding(0, 0, 7, 0);
            cartaoPresencaRelatorios.Margin = new Padding(3, 0, 3, 0);
            cartaoAlunosRelatorios.Margin = new Padding(7, 0, 0, 0);

            layoutGraficosRelatorios.BackColor = VisualPadrao.FundoPagina;
            layoutGraficosRelatorios.ColumnStyles[0].Width = 45F;
            layoutGraficosRelatorios.ColumnStyles[1].Width = 55F;
            layoutGraficosRelatorios.Margin = new Padding(0, 1, 0, 7);
            ConfigurarCartaoGrafico(cartaoGraficoPresencaRelatorios);
            ConfigurarCartaoGrafico(cartaoGraficoReceitaRelatorios);
            AjustarCabecalhoGrafico(lblTituloGraficoPresencaRelatorios);
            AjustarCabecalhoGrafico(lblTituloGraficoReceitaRelatorios);
            lblTituloGraficoPresencaRelatorios.Text = "Frequência das aulas";
            lblTituloGraficoReceitaRelatorios.Text = "Evolução da receita";
            estruturaGraficoPresencaRelatorios.RowStyles[0].Height = 42F;
            estruturaGraficoReceitaRelatorios.RowStyles[0].Height = 42F;
            graficoPresencaRelatorios.BackColor = Color.White;
            graficoReceita.BackColor = Color.White;

            painelPeriodoGraficoRelatorios.BorderStyle = BorderStyle.None;
            painelPeriodoGraficoRelatorios.BackColor = Color.White;
            painelPeriodoGraficoRelatorios.Size = new Size(146, 29);
            painelPeriodoGraficoRelatorios.Padding = new Padding(4, 2, 4, 2);
            painelPeriodoGraficoRelatorios.Location = new Point(
                Math.Max(10, cartaoGraficoReceitaRelatorios.ClientSize.Width - 158), 7);
            painelPeriodoGraficoRelatorios.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            painelPeriodoGraficoRelatorios.Paint += SeletorGrafico_Paint;
            cmbPeriodoGrafico.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            cmbPeriodoGrafico.ForeColor = VisualPadrao.TextoPrincipal;
            cmbPeriodoGrafico.BackColor = Color.White;
            cmbPeriodoGrafico.FlatStyle = FlatStyle.Flat;
            cmbPeriodoGrafico.SelectedIndex = 0;
            cartaoGraficoReceitaRelatorios.Resize += delegate {
                painelPeriodoGraficoRelatorios.Location = new Point(
                    Math.Max(10, cartaoGraficoReceitaRelatorios.ClientSize.Width - 158), 7);
            };

            cartaoResumoFinanceiroRelatorios.BackColor = Color.White;
            cartaoResumoFinanceiroRelatorios.BorderColor = VisualPadrao.BordaCartao;
            cartaoResumoFinanceiroRelatorios.CornerRadius = 12;
            lblTituloReceitaTotalRelatorios.Text = "Receita total no período";
            lblTituloResumoFinanceiroRelatorios.Location = new Point(17, 11);
            lblTituloResumoFinanceiroRelatorios.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTituloResumoFinanceiroRelatorios.ForeColor = VisualPadrao.TextoPrincipal;
            colunasResumoFinanceiroRelatorios.Margin = new Padding(0, 37, 0, 0);
            colunasResumoFinanceiroRelatorios.RowStyles[0].Height = 25F;
            colunasResumoFinanceiroRelatorios.RowStyles[1].Height = 40F;

            ConfigurarValorResumo(lblTituloReceitaTotalRelatorios, lblReceitaTotal, Color.FromArgb(22, 163, 74));
            ConfigurarValorResumo(lblTituloReceitaRecebidaRelatorios, lblReceitaRecebida, Color.FromArgb(22, 163, 74));
            ConfigurarValorResumo(lblTituloReceitaPendenteRelatorios, lblReceitaPendente, Color.FromArgb(217, 119, 6));
        }

        // Padroniza um cartão de indicador e acrescenta uma legenda de apoio
        private void AjustarCartaoIndicador(BorderPanel cartao, IconBadge icone,
            Label titulo, Label valor, string descricao, Color corDestaque) {
            cartao.BackColor = Color.White;
            cartao.BorderColor = VisualPadrao.BordaCartao;
            cartao.CornerRadius = 12;
            cartao.AccentColor = corDestaque;
            icone.Location = new Point(17, 39);
            icone.Size = new Size(42, 42);
            titulo.Location = new Point(72, 29);
            titulo.AutoSize = false;
            titulo.Size = new Size(Math.Max(130, cartao.ClientSize.Width - 90), 18);
            titulo.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = VisualPadrao.TextoSecundario;
            valor.Location = new Point(71, 48);
            valor.Font = new Font("Segoe UI Semibold", 19F, FontStyle.Regular, GraphicsUnit.Point);
            valor.ForeColor = corDestaque;

            Label legenda = new Label();
            legenda.Name = "lblDescricao" + cartao.Name;
            legenda.AutoSize = false;
            legenda.Location = new Point(73, 81);
            legenda.Size = new Size(Math.Max(130, cartao.ClientSize.Width - 92), 18);
            legenda.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            legenda.ForeColor = Color.FromArgb(148, 163, 184);
            legenda.Text = descricao;
            cartao.Controls.Add(legenda);
            legenda.BringToFront();
        }

        // Aplica o mesmo acabamento visual aos cartões de gráfico
        private void ConfigurarCartaoGrafico(BorderPanel cartao) {
            cartao.BackColor = Color.White;
            cartao.BorderColor = VisualPadrao.BordaCartao;
            cartao.CornerRadius = 12;
            cartao.Padding = new Padding(1);
        }

        // Ajusta a tipografia e o alinhamento dos títulos dos gráficos
        private void AjustarCabecalhoGrafico(Label titulo) {
            titulo.Margin = new Padding(15, 0, 0, 0);
            titulo.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = VisualPadrao.TextoPrincipal;
        }

        // Destaca os valores apresentados no resumo financeiro
        private void ConfigurarValorResumo(Label titulo, Label valor, Color cor) {
            titulo.Font = new Font("Segoe UI", 8.2F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = VisualPadrao.TextoSecundario;
            valor.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Regular, GraphicsUnit.Point);
            valor.ForeColor = cor;
        }

        // Desenha uma borda discreta no seletor de intervalo do gráfico
        private void SeletorGrafico_Paint(object sender, PaintEventArgs e) {
            Panel painel = sender as Panel;
            if (painel == null || painel.Width <= 1 || painel.Height <= 1)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, painel.Width - 1, painel.Height - 1);
            using (GraphicsPath caminho = CriarCaminhoArredondado(area, 6))
            using (Pen borda = new Pen(VisualPadrao.BordaCampo, 1F))
                e.Graphics.DrawPath(borda, caminho);
        }

        // Desenha o contorno arredondado do campo de período do relatório
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

        // Prepara o seletor de datas usado nos relatórios
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

        // Alterna a visibilidade do popup de período do relatório
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

        // Desenha o ícone de calendário no botão do campo de período do relatório
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

        // Botão de exportar relatório com ícone de download
        private sealed class DownloadReportButton : RoundedButton {
            public string Caption {
                get;
                set;
            }

            // Construtor do botão de exportar relatório
            public DownloadReportButton() {
                Caption = "Exportar relatório";
                Text = string.Empty;
                TextAlign = ContentAlignment.MiddleLeft;
            }

            // Desenha o ícone de download e o texto do botão

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

        // Painel com borda arredondada
        private sealed class BorderPanel : Panel {
            public Color BorderColor {
                get;
                set;
            }
            public int CornerRadius {
                get;
                set;
            }
            public Color AccentColor {
                get;
                set;
            }

            public BorderPanel() {
                BorderColor = Color.FromArgb(229, 234, 241);
                CornerRadius = 7;
                AccentColor = Color.Empty;
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

                if (AccentColor != Color.Empty && Width > 32) {
                    using (Pen destaque = new Pen(AccentColor, 3F)) {
                        destaque.StartCap = LineCap.Round;
                        destaque.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(destaque, 15, 2, Width - 16, 2);
                    }
                }
            }
        }

        // Exibe um ícone dentro de um selo visual
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

        // Desenha o gráfico de frequência do relatório
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
                float[] valores = new float[] { 0F, 0F, 0F };
                Color[] cores = new Color[] { corPresentes, corAusentes, corJustificados };

                if (valores[0] + valores[1] + valores[2] == 0F) {
                    using (Pen contorno = new Pen(Color.FromArgb(226, 232, 240), Math.Max(8F, area * 0.16F)))
                        g.DrawEllipse(contorno, circulo);
                }
                else {
                    for (int i = 0; i < valores.Length; i++) {
                        float angulo = valores[i] / 100F * 360F;
                        using (SolidBrush brush = new SolidBrush(cores[i]))
                            g.FillPie(brush, circulo, inicio, angulo);
                        inicio += angulo;
                    }
                }

                int interior = (int)(area * 0.61F);
                Rectangle centro = new Rectangle(x + (area - interior) / 2, y + (area - interior) / 2, interior, interior);
                using (SolidBrush brush = new SolidBrush(Color.White)) g.FillEllipse(brush, centro);
                using (Font fonte = new Font("Segoe UI Semibold", Math.Max(10F, area / 8.3F), FontStyle.Regular, GraphicsUnit.Point)) {
                    string textoCentro = "—";
                    SizeF tam = g.MeasureString(textoCentro, fonte);
                    using (SolidBrush brushVazio = new SolidBrush(VisualPadrao.TextoSecundario))
                        g.DrawString(textoCentro, fonte, brushVazio, centro.Left + (centro.Width - tam.Width) / 2, centro.Top + (centro.Height - tam.Height) / 2 - 1);
                }

                int legendaX = x + area + 43;
                int larguraLegenda = Math.Max(100, Width - legendaX - 8);
                int alturaItem = Math.Max(32, Math.Min(43, (Height - 20) / 3));
                int yLegenda = Math.Max(10, (Height - alturaItem * 3 - 14) / 2);
                DesenharItemLegenda(g, legendaX, yLegenda, larguraLegenda, corPresentes, "Presentes", "Sem dados");
                DesenharItemLegenda(g, legendaX, yLegenda + alturaItem, larguraLegenda, corAusentes, "Ausentes", "Sem dados");
                DesenharItemLegenda(g, legendaX, yLegenda + alturaItem * 2, larguraLegenda, corJustificados, "Justificadas", "Sem dados");
                using (Font fonteTotal = new Font("Segoe UI", 7.2F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush brush = new SolidBrush(VisualPadrao.TextoSecundario))
                    g.DrawString("Sem registros de presença", fonteTotal, brush, legendaX, Math.Min(Height - 15, yLegenda + alturaItem * 3 + 1));
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

        // Desenha o gráfico de receitas do relatório
        private sealed class RevenueChartPanel : Panel {
            private readonly string[] meses = new string[0];
            private readonly double[] valores = new double[0];
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

                if (valores.Length == 0 || Array.TrueForAll(valores, valor => valor <= 0)) {
                    using (Font fonteVazia = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point))
                    using (SolidBrush pincelVazio = new SolidBrush(VisualPadrao.TextoSecundario)) {
                        const string mensagem = "Sem dados financeiros para o período selecionado.";
                        SizeF tamanho = e.Graphics.MeasureString(mensagem, fonteVazia);
                        e.Graphics.DrawString(mensagem, fonteVazia, pincelVazio,
                            Math.Max(8F, (Width - tamanho.Width) / 2F), Math.Max(8F, (Height - tamanho.Height) / 2F));
                    }
                    return;
                }

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
