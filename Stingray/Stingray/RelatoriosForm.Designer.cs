namespace Stingray {
    partial class RelatoriosForm {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel painelConteudo;
        private System.Windows.Forms.TableLayoutPanel layoutRelatorios;
        private System.Windows.Forms.Panel barraSuperiorRelatorios;
        private System.Windows.Forms.Label lblVisaoGeralRelatorios;
        private System.Windows.Forms.FlowLayoutPanel acoesRelatorios;
        private System.Windows.Forms.Label lblPeriodoRelatorios;
        private System.Windows.Forms.Panel panelPeriodoRelatorio;
        private System.Windows.Forms.Button btnCalendarioPeriodoRelatorio;
        private Stingray.RoundedButton btnExportarRelatorio;
        private System.Windows.Forms.TableLayoutPanel linhaIndicadoresRelatorios;
        private BorderPanel cartaoReceitaRelatorios;
        private BorderPanel cartaoPresencaRelatorios;
        private BorderPanel cartaoAlunosRelatorios;
        private IconBadge badgeReceitaRelatorios;
        private IconBadge badgePresencaRelatorios;
        private IconBadge badgeAlunosRelatorios;
        private System.Windows.Forms.Label lblTituloReceitaRelatorios;
        private System.Windows.Forms.Label lblTituloPresencaRelatorios;
        private System.Windows.Forms.Label lblTituloAlunosRelatorios;
        private System.Windows.Forms.TableLayoutPanel layoutGraficosRelatorios;
        private BorderPanel cartaoGraficoPresencaRelatorios;
        private BorderPanel cartaoGraficoReceitaRelatorios;
        private System.Windows.Forms.TableLayoutPanel estruturaGraficoPresencaRelatorios;
        private System.Windows.Forms.TableLayoutPanel estruturaGraficoReceitaRelatorios;
        private System.Windows.Forms.Label lblTituloGraficoPresencaRelatorios;
        private System.Windows.Forms.Label lblTituloGraficoReceitaRelatorios;
        private System.Windows.Forms.Panel painelPeriodoGraficoRelatorios;
        private System.Windows.Forms.TableLayoutPanel estruturaResumoFinanceiroRelatorios;
        private BorderPanel cartaoResumoFinanceiroRelatorios;
        private System.Windows.Forms.Label lblTituloResumoFinanceiroRelatorios;
        private System.Windows.Forms.TableLayoutPanel colunasResumoFinanceiroRelatorios;
        private System.Windows.Forms.Label lblTituloReceitaTotalRelatorios;
        private System.Windows.Forms.Label lblTituloReceitaRecebidaRelatorios;
        private System.Windows.Forms.Label lblTituloReceitaPendenteRelatorios;
        private System.Windows.Forms.Panel divisorResumo1Relatorios;
        private System.Windows.Forms.Panel divisorResumo2Relatorios;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.painelConteudo = new System.Windows.Forms.Panel();
            this.layoutRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.barraSuperiorRelatorios = new System.Windows.Forms.Panel();
            this.lblVisaoGeralRelatorios = new System.Windows.Forms.Label();
            this.acoesRelatorios = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPeriodoRelatorios = new System.Windows.Forms.Label();
            this.panelPeriodoRelatorio = new System.Windows.Forms.Panel();
            this.txtPeriodoRelatorio = new System.Windows.Forms.Label();
            this.btnCalendarioPeriodoRelatorio = new System.Windows.Forms.Button();
            this.btnExportarRelatorio = new Stingray.RoundedButton();
            this.linhaIndicadoresRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.cartaoReceitaRelatorios = new Stingray.RelatoriosForm.BorderPanel();
            this.badgeReceitaRelatorios = new Stingray.RelatoriosForm.IconBadge();
            this.lblTituloReceitaRelatorios = new System.Windows.Forms.Label();
            this.lblReceitaValor = new System.Windows.Forms.Label();
            this.cartaoPresencaRelatorios = new Stingray.RelatoriosForm.BorderPanel();
            this.badgePresencaRelatorios = new Stingray.RelatoriosForm.IconBadge();
            this.lblTituloPresencaRelatorios = new System.Windows.Forms.Label();
            this.lblPresencaValor = new System.Windows.Forms.Label();
            this.cartaoAlunosRelatorios = new Stingray.RelatoriosForm.BorderPanel();
            this.badgeAlunosRelatorios = new Stingray.RelatoriosForm.IconBadge();
            this.lblTituloAlunosRelatorios = new System.Windows.Forms.Label();
            this.lblAlunosValor = new System.Windows.Forms.Label();
            this.layoutGraficosRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.cartaoGraficoPresencaRelatorios = new Stingray.RelatoriosForm.BorderPanel();
            this.estruturaGraficoPresencaRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.lblTituloGraficoPresencaRelatorios = new System.Windows.Forms.Label();
            this.graficoPresencaRelatorios = new Stingray.RelatoriosForm.AttendanceChartPanel();
            this.cartaoGraficoReceitaRelatorios = new Stingray.RelatoriosForm.BorderPanel();
            this.estruturaGraficoReceitaRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.lblTituloGraficoReceitaRelatorios = new System.Windows.Forms.Label();
            this.graficoReceita = new Stingray.RelatoriosForm.RevenueChartPanel();
            this.painelPeriodoGraficoRelatorios = new System.Windows.Forms.Panel();
            this.cmbPeriodoGrafico = new System.Windows.Forms.ComboBox();
            this.estruturaResumoFinanceiroRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.cartaoResumoFinanceiroRelatorios = new Stingray.RelatoriosForm.BorderPanel();
            this.lblTituloResumoFinanceiroRelatorios = new System.Windows.Forms.Label();
            this.colunasResumoFinanceiroRelatorios = new System.Windows.Forms.TableLayoutPanel();
            this.lblTituloReceitaTotalRelatorios = new System.Windows.Forms.Label();
            this.lblReceitaTotal = new System.Windows.Forms.Label();
            this.lblTituloReceitaRecebidaRelatorios = new System.Windows.Forms.Label();
            this.lblReceitaRecebida = new System.Windows.Forms.Label();
            this.lblTituloReceitaPendenteRelatorios = new System.Windows.Forms.Label();
            this.lblReceitaPendente = new System.Windows.Forms.Label();
            this.divisorResumo1Relatorios = new System.Windows.Forms.Panel();
            this.divisorResumo2Relatorios = new System.Windows.Forms.Panel();
            this.painelConteudo.SuspendLayout();
            this.layoutRelatorios.SuspendLayout();
            this.barraSuperiorRelatorios.SuspendLayout();
            this.acoesRelatorios.SuspendLayout();
            this.panelPeriodoRelatorio.SuspendLayout();
            this.linhaIndicadoresRelatorios.SuspendLayout();
            this.cartaoReceitaRelatorios.SuspendLayout();
            this.cartaoPresencaRelatorios.SuspendLayout();
            this.cartaoAlunosRelatorios.SuspendLayout();
            this.layoutGraficosRelatorios.SuspendLayout();
            this.cartaoGraficoPresencaRelatorios.SuspendLayout();
            this.estruturaGraficoPresencaRelatorios.SuspendLayout();
            this.cartaoGraficoReceitaRelatorios.SuspendLayout();
            this.estruturaGraficoReceitaRelatorios.SuspendLayout();
            this.painelPeriodoGraficoRelatorios.SuspendLayout();
            this.estruturaResumoFinanceiroRelatorios.SuspendLayout();
            this.cartaoResumoFinanceiroRelatorios.SuspendLayout();
            this.colunasResumoFinanceiroRelatorios.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitulo.Location = new System.Drawing.Point(13, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(101, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Relatórios";
            // 
            // painelConteudo
            // 
            this.painelConteudo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.painelConteudo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.painelConteudo.Controls.Add(this.layoutRelatorios);
            this.painelConteudo.Location = new System.Drawing.Point(12, 52);
            this.painelConteudo.Name = "painelConteudo";
            this.painelConteudo.Size = new System.Drawing.Size(1026, 596);
            this.painelConteudo.TabIndex = 1;
            // 
            // layoutRelatorios
            // 
            this.layoutRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.layoutRelatorios.ColumnCount = 1;
            this.layoutRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRelatorios.Controls.Add(this.barraSuperiorRelatorios, 0, 0);
            this.layoutRelatorios.Controls.Add(this.linhaIndicadoresRelatorios, 0, 1);
            this.layoutRelatorios.Controls.Add(this.layoutGraficosRelatorios, 0, 2);
            this.layoutRelatorios.Controls.Add(this.estruturaResumoFinanceiroRelatorios, 0, 3);
            this.layoutRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRelatorios.Location = new System.Drawing.Point(0, 0);
            this.layoutRelatorios.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRelatorios.Name = "layoutRelatorios";
            this.layoutRelatorios.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.layoutRelatorios.RowCount = 4;
            this.layoutRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.layoutRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.layoutRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.layoutRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.layoutRelatorios.Size = new System.Drawing.Size(1026, 596);
            this.layoutRelatorios.TabIndex = 0;
            // 
            // barraSuperiorRelatorios
            // 
            this.barraSuperiorRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.barraSuperiorRelatorios.Controls.Add(this.lblVisaoGeralRelatorios);
            this.barraSuperiorRelatorios.Controls.Add(this.acoesRelatorios);
            this.barraSuperiorRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barraSuperiorRelatorios.Location = new System.Drawing.Point(2, 0);
            this.barraSuperiorRelatorios.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.barraSuperiorRelatorios.Name = "barraSuperiorRelatorios";
            this.barraSuperiorRelatorios.Size = new System.Drawing.Size(1022, 32);
            this.barraSuperiorRelatorios.TabIndex = 0;
            // 
            // lblVisaoGeralRelatorios
            // 
            this.lblVisaoGeralRelatorios.AutoSize = true;
            this.lblVisaoGeralRelatorios.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisaoGeralRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblVisaoGeralRelatorios.Location = new System.Drawing.Point(4, 7);
            this.lblVisaoGeralRelatorios.Name = "lblVisaoGeralRelatorios";
            this.lblVisaoGeralRelatorios.Size = new System.Drawing.Size(78, 19);
            this.lblVisaoGeralRelatorios.TabIndex = 0;
            this.lblVisaoGeralRelatorios.Text = "Visão geral";
            // 
            // acoesRelatorios
            // 
            this.acoesRelatorios.Controls.Add(this.lblPeriodoRelatorios);
            this.acoesRelatorios.Controls.Add(this.panelPeriodoRelatorio);
            this.acoesRelatorios.Controls.Add(this.btnExportarRelatorio);
            this.acoesRelatorios.Dock = System.Windows.Forms.DockStyle.Right;
            this.acoesRelatorios.Location = new System.Drawing.Point(650, 0);
            this.acoesRelatorios.Margin = new System.Windows.Forms.Padding(0);
            this.acoesRelatorios.Name = "acoesRelatorios";
            this.acoesRelatorios.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.acoesRelatorios.Size = new System.Drawing.Size(372, 32);
            this.acoesRelatorios.TabIndex = 1;
            this.acoesRelatorios.WrapContents = false;
            // 
            // lblPeriodoRelatorios
            // 
            this.lblPeriodoRelatorios.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodoRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblPeriodoRelatorios.Location = new System.Drawing.Point(0, 1);
            this.lblPeriodoRelatorios.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblPeriodoRelatorios.Name = "lblPeriodoRelatorios";
            this.lblPeriodoRelatorios.Size = new System.Drawing.Size(58, 27);
            this.lblPeriodoRelatorios.TabIndex = 0;
            this.lblPeriodoRelatorios.Text = "Período:";
            this.lblPeriodoRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelPeriodoRelatorio
            // 
            this.panelPeriodoRelatorio.BackColor = System.Drawing.Color.White;
            this.panelPeriodoRelatorio.Controls.Add(this.txtPeriodoRelatorio);
            this.panelPeriodoRelatorio.Controls.Add(this.btnCalendarioPeriodoRelatorio);
            this.panelPeriodoRelatorio.Location = new System.Drawing.Point(62, 1);
            this.panelPeriodoRelatorio.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelPeriodoRelatorio.Name = "panelPeriodoRelatorio";
            this.panelPeriodoRelatorio.Size = new System.Drawing.Size(142, 28);
            this.panelPeriodoRelatorio.TabIndex = 1;
            // 
            // txtPeriodoRelatorio
            // 
            this.txtPeriodoRelatorio.AutoEllipsis = true;
            this.txtPeriodoRelatorio.BackColor = System.Drawing.Color.White;
            this.txtPeriodoRelatorio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtPeriodoRelatorio.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPeriodoRelatorio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtPeriodoRelatorio.Location = new System.Drawing.Point(8, 1);
            this.txtPeriodoRelatorio.Name = "txtPeriodoRelatorio";
            this.txtPeriodoRelatorio.Size = new System.Drawing.Size(102, 24);
            this.txtPeriodoRelatorio.TabIndex = 0;
            this.txtPeriodoRelatorio.Text = "Selecione o período";
            this.txtPeriodoRelatorio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCalendarioPeriodoRelatorio
            // 
            this.btnCalendarioPeriodoRelatorio.AccessibleName = "Selecionar período";
            this.btnCalendarioPeriodoRelatorio.BackColor = System.Drawing.Color.White;
            this.btnCalendarioPeriodoRelatorio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalendarioPeriodoRelatorio.FlatAppearance.BorderSize = 0;
            this.btnCalendarioPeriodoRelatorio.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnCalendarioPeriodoRelatorio.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnCalendarioPeriodoRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarioPeriodoRelatorio.Location = new System.Drawing.Point(113, 1);
            this.btnCalendarioPeriodoRelatorio.Name = "btnCalendarioPeriodoRelatorio";
            this.btnCalendarioPeriodoRelatorio.Size = new System.Drawing.Size(27, 24);
            this.btnCalendarioPeriodoRelatorio.TabIndex = 1;
            this.btnCalendarioPeriodoRelatorio.Text = "▦";
            this.btnCalendarioPeriodoRelatorio.UseVisualStyleBackColor = false;
            this.btnCalendarioPeriodoRelatorio.Paint += new System.Windows.Forms.PaintEventHandler(this.CalendarioPeriodo_Paint);
            // 
            // btnExportarRelatorio
            // 
            this.btnExportarRelatorio.BackColor = System.Drawing.Color.White;
            this.btnExportarRelatorio.CornerRadius = 5;
            this.btnExportarRelatorio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarRelatorio.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.btnExportarRelatorio.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnExportarRelatorio.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.btnExportarRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarRelatorio.Font = new System.Drawing.Font("Segoe UI Semibold", 8.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportarRelatorio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnExportarRelatorio.Location = new System.Drawing.Point(212, 1);
            this.btnExportarRelatorio.Margin = new System.Windows.Forms.Padding(0);
            this.btnExportarRelatorio.Name = "btnExportarRelatorio";
            this.btnExportarRelatorio.Size = new System.Drawing.Size(132, 28);
            this.btnExportarRelatorio.TabIndex = 2;
            this.btnExportarRelatorio.Text = "↓  Exportar relatório";
            this.btnExportarRelatorio.UseVisualStyleBackColor = false;
            // 
            // linhaIndicadoresRelatorios
            // 
            this.linhaIndicadoresRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.linhaIndicadoresRelatorios.ColumnCount = 3;
            this.linhaIndicadoresRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.linhaIndicadoresRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.linhaIndicadoresRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.334F));
            this.linhaIndicadoresRelatorios.Controls.Add(this.cartaoReceitaRelatorios, 0, 0);
            this.linhaIndicadoresRelatorios.Controls.Add(this.cartaoPresencaRelatorios, 1, 0);
            this.linhaIndicadoresRelatorios.Controls.Add(this.cartaoAlunosRelatorios, 2, 0);
            this.linhaIndicadoresRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linhaIndicadoresRelatorios.Location = new System.Drawing.Point(2, 36);
            this.linhaIndicadoresRelatorios.Margin = new System.Windows.Forms.Padding(0, 2, 0, 5);
            this.linhaIndicadoresRelatorios.Name = "linhaIndicadoresRelatorios";
            this.linhaIndicadoresRelatorios.RowCount = 1;
            this.linhaIndicadoresRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.linhaIndicadoresRelatorios.Size = new System.Drawing.Size(1022, 129);
            this.linhaIndicadoresRelatorios.TabIndex = 1;
            // 
            // cartaoReceitaRelatorios
            // 
            this.cartaoReceitaRelatorios.BackColor = System.Drawing.Color.White;
            this.cartaoReceitaRelatorios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.cartaoReceitaRelatorios.Controls.Add(this.badgeReceitaRelatorios);
            this.cartaoReceitaRelatorios.Controls.Add(this.lblTituloReceitaRelatorios);
            this.cartaoReceitaRelatorios.Controls.Add(this.lblReceitaValor);
            this.cartaoReceitaRelatorios.CornerRadius = 9;
            this.cartaoReceitaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartaoReceitaRelatorios.Location = new System.Drawing.Point(0, 0);
            this.cartaoReceitaRelatorios.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.cartaoReceitaRelatorios.Name = "cartaoReceitaRelatorios";
            this.cartaoReceitaRelatorios.Padding = new System.Windows.Forms.Padding(1);
            this.cartaoReceitaRelatorios.Size = new System.Drawing.Size(334, 129);
            this.cartaoReceitaRelatorios.TabIndex = 0;
            // 
            // badgeReceitaRelatorios
            // 
            this.badgeReceitaRelatorios.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.badgeReceitaRelatorios.BadgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(248)))), ((int)(((byte)(235)))));
            this.badgeReceitaRelatorios.IconType = "money";
            this.badgeReceitaRelatorios.Location = new System.Drawing.Point(14, 42);
            this.badgeReceitaRelatorios.Name = "badgeReceitaRelatorios";
            this.badgeReceitaRelatorios.Size = new System.Drawing.Size(38, 38);
            this.badgeReceitaRelatorios.TabIndex = 0;
            // 
            // lblTituloReceitaRelatorios
            // 
            this.lblTituloReceitaRelatorios.AutoSize = true;
            this.lblTituloReceitaRelatorios.Font = new System.Drawing.Font("Segoe UI", 7.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloReceitaRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblTituloReceitaRelatorios.Location = new System.Drawing.Point(64, 42);
            this.lblTituloReceitaRelatorios.Name = "lblTituloReceitaRelatorios";
            this.lblTituloReceitaRelatorios.Size = new System.Drawing.Size(92, 13);
            this.lblTituloReceitaRelatorios.TabIndex = 1;
            this.lblTituloReceitaRelatorios.Text = "RECEITA DO MÊS";
            // 
            // lblReceitaValor
            // 
            this.lblReceitaValor.AutoSize = true;
            this.lblReceitaValor.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceitaValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblReceitaValor.Location = new System.Drawing.Point(64, 62);
            this.lblReceitaValor.Name = "lblReceitaValor";
            this.lblReceitaValor.Size = new System.Drawing.Size(129, 28);
            this.lblReceitaValor.TabIndex = 2;
            this.lblReceitaValor.Text = "R$ 23.450,00";
            // 
            // cartaoPresencaRelatorios
            // 
            this.cartaoPresencaRelatorios.BackColor = System.Drawing.Color.White;
            this.cartaoPresencaRelatorios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.cartaoPresencaRelatorios.Controls.Add(this.badgePresencaRelatorios);
            this.cartaoPresencaRelatorios.Controls.Add(this.lblTituloPresencaRelatorios);
            this.cartaoPresencaRelatorios.Controls.Add(this.lblPresencaValor);
            this.cartaoPresencaRelatorios.CornerRadius = 9;
            this.cartaoPresencaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartaoPresencaRelatorios.Location = new System.Drawing.Point(343, 0);
            this.cartaoPresencaRelatorios.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cartaoPresencaRelatorios.Name = "cartaoPresencaRelatorios";
            this.cartaoPresencaRelatorios.Padding = new System.Windows.Forms.Padding(1);
            this.cartaoPresencaRelatorios.Size = new System.Drawing.Size(334, 129);
            this.cartaoPresencaRelatorios.TabIndex = 1;
            // 
            // badgePresencaRelatorios
            // 
            this.badgePresencaRelatorios.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.badgePresencaRelatorios.BadgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.badgePresencaRelatorios.IconType = "people";
            this.badgePresencaRelatorios.Location = new System.Drawing.Point(14, 42);
            this.badgePresencaRelatorios.Name = "badgePresencaRelatorios";
            this.badgePresencaRelatorios.Size = new System.Drawing.Size(38, 38);
            this.badgePresencaRelatorios.TabIndex = 0;
            // 
            // lblTituloPresencaRelatorios
            // 
            this.lblTituloPresencaRelatorios.AutoSize = true;
            this.lblTituloPresencaRelatorios.Font = new System.Drawing.Font("Segoe UI", 7.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloPresencaRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblTituloPresencaRelatorios.Location = new System.Drawing.Point(64, 42);
            this.lblTituloPresencaRelatorios.Name = "lblTituloPresencaRelatorios";
            this.lblTituloPresencaRelatorios.Size = new System.Drawing.Size(142, 13);
            this.lblTituloPresencaRelatorios.TabIndex = 1;
            this.lblTituloPresencaRelatorios.Text = "PRESENÇA MÉDIA NO MÊS";
            // 
            // lblPresencaValor
            // 
            this.lblPresencaValor.AutoSize = true;
            this.lblPresencaValor.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPresencaValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.lblPresencaValor.Location = new System.Drawing.Point(64, 62);
            this.lblPresencaValor.Name = "lblPresencaValor";
            this.lblPresencaValor.Size = new System.Drawing.Size(51, 28);
            this.lblPresencaValor.TabIndex = 2;
            this.lblPresencaValor.Text = "86%";
            // 
            // cartaoAlunosRelatorios
            // 
            this.cartaoAlunosRelatorios.BackColor = System.Drawing.Color.White;
            this.cartaoAlunosRelatorios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.cartaoAlunosRelatorios.Controls.Add(this.badgeAlunosRelatorios);
            this.cartaoAlunosRelatorios.Controls.Add(this.lblTituloAlunosRelatorios);
            this.cartaoAlunosRelatorios.Controls.Add(this.lblAlunosValor);
            this.cartaoAlunosRelatorios.CornerRadius = 9;
            this.cartaoAlunosRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartaoAlunosRelatorios.Location = new System.Drawing.Point(686, 0);
            this.cartaoAlunosRelatorios.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.cartaoAlunosRelatorios.Name = "cartaoAlunosRelatorios";
            this.cartaoAlunosRelatorios.Padding = new System.Windows.Forms.Padding(1);
            this.cartaoAlunosRelatorios.Size = new System.Drawing.Size(336, 129);
            this.cartaoAlunosRelatorios.TabIndex = 2;
            // 
            // badgeAlunosRelatorios
            // 
            this.badgeAlunosRelatorios.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(51)))), ((int)(((byte)(234)))));
            this.badgeAlunosRelatorios.BadgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.badgeAlunosRelatorios.IconType = "student";
            this.badgeAlunosRelatorios.Location = new System.Drawing.Point(14, 42);
            this.badgeAlunosRelatorios.Name = "badgeAlunosRelatorios";
            this.badgeAlunosRelatorios.Size = new System.Drawing.Size(38, 38);
            this.badgeAlunosRelatorios.TabIndex = 0;
            // 
            // lblTituloAlunosRelatorios
            // 
            this.lblTituloAlunosRelatorios.AutoSize = true;
            this.lblTituloAlunosRelatorios.Font = new System.Drawing.Font("Segoe UI", 7.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloAlunosRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblTituloAlunosRelatorios.Location = new System.Drawing.Point(64, 42);
            this.lblTituloAlunosRelatorios.Name = "lblTituloAlunosRelatorios";
            this.lblTituloAlunosRelatorios.Size = new System.Drawing.Size(89, 13);
            this.lblTituloAlunosRelatorios.TabIndex = 1;
            this.lblTituloAlunosRelatorios.Text = "ALUNOS ATIVOS";
            // 
            // lblAlunosValor
            // 
            this.lblAlunosValor.AutoSize = true;
            this.lblAlunosValor.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlunosValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(51)))), ((int)(((byte)(234)))));
            this.lblAlunosValor.Location = new System.Drawing.Point(64, 62);
            this.lblAlunosValor.Name = "lblAlunosValor";
            this.lblAlunosValor.Size = new System.Drawing.Size(42, 28);
            this.lblAlunosValor.TabIndex = 2;
            this.lblAlunosValor.Text = "128";
            // 
            // layoutGraficosRelatorios
            // 
            this.layoutGraficosRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.layoutGraficosRelatorios.ColumnCount = 2;
            this.layoutGraficosRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47F));
            this.layoutGraficosRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53F));
            this.layoutGraficosRelatorios.Controls.Add(this.cartaoGraficoPresencaRelatorios, 0, 0);
            this.layoutGraficosRelatorios.Controls.Add(this.cartaoGraficoReceitaRelatorios, 1, 0);
            this.layoutGraficosRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutGraficosRelatorios.Location = new System.Drawing.Point(2, 172);
            this.layoutGraficosRelatorios.Margin = new System.Windows.Forms.Padding(0, 2, 0, 5);
            this.layoutGraficosRelatorios.Name = "layoutGraficosRelatorios";
            this.layoutGraficosRelatorios.RowCount = 1;
            this.layoutGraficosRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutGraficosRelatorios.Size = new System.Drawing.Size(1022, 247);
            this.layoutGraficosRelatorios.TabIndex = 2;
            // 
            // cartaoGraficoPresencaRelatorios
            // 
            this.cartaoGraficoPresencaRelatorios.BackColor = System.Drawing.Color.White;
            this.cartaoGraficoPresencaRelatorios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.cartaoGraficoPresencaRelatorios.Controls.Add(this.estruturaGraficoPresencaRelatorios);
            this.cartaoGraficoPresencaRelatorios.CornerRadius = 9;
            this.cartaoGraficoPresencaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartaoGraficoPresencaRelatorios.Location = new System.Drawing.Point(0, 0);
            this.cartaoGraficoPresencaRelatorios.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.cartaoGraficoPresencaRelatorios.Name = "cartaoGraficoPresencaRelatorios";
            this.cartaoGraficoPresencaRelatorios.Padding = new System.Windows.Forms.Padding(1);
            this.cartaoGraficoPresencaRelatorios.Size = new System.Drawing.Size(474, 247);
            this.cartaoGraficoPresencaRelatorios.TabIndex = 0;
            // 
            // estruturaGraficoPresencaRelatorios
            // 
            this.estruturaGraficoPresencaRelatorios.ColumnCount = 1;
            this.estruturaGraficoPresencaRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.estruturaGraficoPresencaRelatorios.Controls.Add(this.lblTituloGraficoPresencaRelatorios, 0, 0);
            this.estruturaGraficoPresencaRelatorios.Controls.Add(this.graficoPresencaRelatorios, 0, 1);
            this.estruturaGraficoPresencaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.estruturaGraficoPresencaRelatorios.Location = new System.Drawing.Point(1, 1);
            this.estruturaGraficoPresencaRelatorios.Margin = new System.Windows.Forms.Padding(0);
            this.estruturaGraficoPresencaRelatorios.Name = "estruturaGraficoPresencaRelatorios";
            this.estruturaGraficoPresencaRelatorios.RowCount = 2;
            this.estruturaGraficoPresencaRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.estruturaGraficoPresencaRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.estruturaGraficoPresencaRelatorios.Size = new System.Drawing.Size(472, 245);
            this.estruturaGraficoPresencaRelatorios.TabIndex = 0;
            // 
            // lblTituloGraficoPresencaRelatorios
            // 
            this.lblTituloGraficoPresencaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTituloGraficoPresencaRelatorios.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloGraficoPresencaRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTituloGraficoPresencaRelatorios.Location = new System.Drawing.Point(12, 0);
            this.lblTituloGraficoPresencaRelatorios.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTituloGraficoPresencaRelatorios.Name = "lblTituloGraficoPresencaRelatorios";
            this.lblTituloGraficoPresencaRelatorios.Size = new System.Drawing.Size(460, 34);
            this.lblTituloGraficoPresencaRelatorios.TabIndex = 0;
            this.lblTituloGraficoPresencaRelatorios.Text = "Percentual de Presença no Mês";
            this.lblTituloGraficoPresencaRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // graficoPresencaRelatorios
            // 
            this.graficoPresencaRelatorios.BackColor = System.Drawing.Color.White;
            this.graficoPresencaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graficoPresencaRelatorios.Location = new System.Drawing.Point(3, 37);
            this.graficoPresencaRelatorios.Name = "graficoPresencaRelatorios";
            this.graficoPresencaRelatorios.Size = new System.Drawing.Size(466, 205);
            this.graficoPresencaRelatorios.TabIndex = 1;
            // 
            // cartaoGraficoReceitaRelatorios
            // 
            this.cartaoGraficoReceitaRelatorios.BackColor = System.Drawing.Color.White;
            this.cartaoGraficoReceitaRelatorios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.cartaoGraficoReceitaRelatorios.Controls.Add(this.estruturaGraficoReceitaRelatorios);
            this.cartaoGraficoReceitaRelatorios.Controls.Add(this.painelPeriodoGraficoRelatorios);
            this.cartaoGraficoReceitaRelatorios.CornerRadius = 9;
            this.cartaoGraficoReceitaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartaoGraficoReceitaRelatorios.Location = new System.Drawing.Point(483, 0);
            this.cartaoGraficoReceitaRelatorios.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.cartaoGraficoReceitaRelatorios.Name = "cartaoGraficoReceitaRelatorios";
            this.cartaoGraficoReceitaRelatorios.Padding = new System.Windows.Forms.Padding(1);
            this.cartaoGraficoReceitaRelatorios.Size = new System.Drawing.Size(539, 247);
            this.cartaoGraficoReceitaRelatorios.TabIndex = 1;
            // 
            // estruturaGraficoReceitaRelatorios
            // 
            this.estruturaGraficoReceitaRelatorios.ColumnCount = 1;
            this.estruturaGraficoReceitaRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.estruturaGraficoReceitaRelatorios.Controls.Add(this.lblTituloGraficoReceitaRelatorios, 0, 0);
            this.estruturaGraficoReceitaRelatorios.Controls.Add(this.graficoReceita, 0, 1);
            this.estruturaGraficoReceitaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.estruturaGraficoReceitaRelatorios.Location = new System.Drawing.Point(1, 1);
            this.estruturaGraficoReceitaRelatorios.Margin = new System.Windows.Forms.Padding(0);
            this.estruturaGraficoReceitaRelatorios.Name = "estruturaGraficoReceitaRelatorios";
            this.estruturaGraficoReceitaRelatorios.RowCount = 2;
            this.estruturaGraficoReceitaRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.estruturaGraficoReceitaRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.estruturaGraficoReceitaRelatorios.Size = new System.Drawing.Size(537, 245);
            this.estruturaGraficoReceitaRelatorios.TabIndex = 0;
            // 
            // lblTituloGraficoReceitaRelatorios
            // 
            this.lblTituloGraficoReceitaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTituloGraficoReceitaRelatorios.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloGraficoReceitaRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTituloGraficoReceitaRelatorios.Location = new System.Drawing.Point(12, 0);
            this.lblTituloGraficoReceitaRelatorios.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTituloGraficoReceitaRelatorios.Name = "lblTituloGraficoReceitaRelatorios";
            this.lblTituloGraficoReceitaRelatorios.Size = new System.Drawing.Size(525, 34);
            this.lblTituloGraficoReceitaRelatorios.TabIndex = 0;
            this.lblTituloGraficoReceitaRelatorios.Text = "Evolução da Receita (Últimos 6 meses)";
            this.lblTituloGraficoReceitaRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // graficoReceita
            // 
            this.graficoReceita.BackColor = System.Drawing.Color.White;
            this.graficoReceita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graficoReceita.Location = new System.Drawing.Point(3, 37);
            this.graficoReceita.Name = "graficoReceita";
            this.graficoReceita.QuantidadeMeses = 6;
            this.graficoReceita.Size = new System.Drawing.Size(531, 205);
            this.graficoReceita.TabIndex = 1;
            // 
            // painelPeriodoGraficoRelatorios
            // 
            this.painelPeriodoGraficoRelatorios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.painelPeriodoGraficoRelatorios.BackColor = System.Drawing.Color.White;
            this.painelPeriodoGraficoRelatorios.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.painelPeriodoGraficoRelatorios.Controls.Add(this.cmbPeriodoGrafico);
            this.painelPeriodoGraficoRelatorios.Location = new System.Drawing.Point(699, 5);
            this.painelPeriodoGraficoRelatorios.Name = "painelPeriodoGraficoRelatorios";
            this.painelPeriodoGraficoRelatorios.Size = new System.Drawing.Size(120, 23);
            this.painelPeriodoGraficoRelatorios.TabIndex = 1;
            // 
            // cmbPeriodoGrafico
            // 
            this.cmbPeriodoGrafico.BackColor = System.Drawing.Color.White;
            this.cmbPeriodoGrafico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPeriodoGrafico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriodoGrafico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPeriodoGrafico.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPeriodoGrafico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.cmbPeriodoGrafico.Items.AddRange(new object[] {
            "Últimos 6 meses",
            "Últimos 3 meses"});
            this.cmbPeriodoGrafico.Location = new System.Drawing.Point(0, 0);
            this.cmbPeriodoGrafico.Margin = new System.Windows.Forms.Padding(0);
            this.cmbPeriodoGrafico.Name = "cmbPeriodoGrafico";
            this.cmbPeriodoGrafico.Size = new System.Drawing.Size(118, 20);
            this.cmbPeriodoGrafico.TabIndex = 0;
            // 
            // estruturaResumoFinanceiroRelatorios
            // 
            this.estruturaResumoFinanceiroRelatorios.ColumnCount = 1;
            this.estruturaResumoFinanceiroRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.estruturaResumoFinanceiroRelatorios.Controls.Add(this.cartaoResumoFinanceiroRelatorios, 0, 0);
            this.estruturaResumoFinanceiroRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.estruturaResumoFinanceiroRelatorios.Location = new System.Drawing.Point(2, 426);
            this.estruturaResumoFinanceiroRelatorios.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.estruturaResumoFinanceiroRelatorios.Name = "estruturaResumoFinanceiroRelatorios";
            this.estruturaResumoFinanceiroRelatorios.RowCount = 1;
            this.estruturaResumoFinanceiroRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.estruturaResumoFinanceiroRelatorios.Size = new System.Drawing.Size(1022, 168);
            this.estruturaResumoFinanceiroRelatorios.TabIndex = 3;
            // 
            // cartaoResumoFinanceiroRelatorios
            // 
            this.cartaoResumoFinanceiroRelatorios.BackColor = System.Drawing.Color.White;
            this.cartaoResumoFinanceiroRelatorios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.cartaoResumoFinanceiroRelatorios.Controls.Add(this.lblTituloResumoFinanceiroRelatorios);
            this.cartaoResumoFinanceiroRelatorios.Controls.Add(this.colunasResumoFinanceiroRelatorios);
            this.cartaoResumoFinanceiroRelatorios.CornerRadius = 9;
            this.cartaoResumoFinanceiroRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartaoResumoFinanceiroRelatorios.Location = new System.Drawing.Point(0, 0);
            this.cartaoResumoFinanceiroRelatorios.Margin = new System.Windows.Forms.Padding(0);
            this.cartaoResumoFinanceiroRelatorios.Name = "cartaoResumoFinanceiroRelatorios";
            this.cartaoResumoFinanceiroRelatorios.Padding = new System.Windows.Forms.Padding(1);
            this.cartaoResumoFinanceiroRelatorios.Size = new System.Drawing.Size(1022, 168);
            this.cartaoResumoFinanceiroRelatorios.TabIndex = 0;
            // 
            // lblTituloResumoFinanceiroRelatorios
            // 
            this.lblTituloResumoFinanceiroRelatorios.AutoSize = true;
            this.lblTituloResumoFinanceiroRelatorios.Font = new System.Drawing.Font("Segoe UI Semibold", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloResumoFinanceiroRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTituloResumoFinanceiroRelatorios.Location = new System.Drawing.Point(12, 10);
            this.lblTituloResumoFinanceiroRelatorios.Name = "lblTituloResumoFinanceiroRelatorios";
            this.lblTituloResumoFinanceiroRelatorios.Size = new System.Drawing.Size(108, 15);
            this.lblTituloResumoFinanceiroRelatorios.TabIndex = 0;
            this.lblTituloResumoFinanceiroRelatorios.Text = "Resumo Financeiro";
            // 
            // colunasResumoFinanceiroRelatorios
            // 
            this.colunasResumoFinanceiroRelatorios.ColumnCount = 5;
            this.colunasResumoFinanceiroRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.2F));
            this.colunasResumoFinanceiroRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.colunasResumoFinanceiroRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.2F));
            this.colunasResumoFinanceiroRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.colunasResumoFinanceiroRelatorios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.6F));
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.lblTituloReceitaTotalRelatorios, 0, 0);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.lblReceitaTotal, 0, 1);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.lblTituloReceitaRecebidaRelatorios, 2, 0);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.lblReceitaRecebida, 2, 1);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.lblTituloReceitaPendenteRelatorios, 4, 0);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.lblReceitaPendente, 4, 1);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.divisorResumo1Relatorios, 1, 0);
            this.colunasResumoFinanceiroRelatorios.Controls.Add(this.divisorResumo2Relatorios, 3, 0);
            this.colunasResumoFinanceiroRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colunasResumoFinanceiroRelatorios.Location = new System.Drawing.Point(1, 1);
            this.colunasResumoFinanceiroRelatorios.Margin = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.colunasResumoFinanceiroRelatorios.Name = "colunasResumoFinanceiroRelatorios";
            this.colunasResumoFinanceiroRelatorios.RowCount = 3;
            this.colunasResumoFinanceiroRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.colunasResumoFinanceiroRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.colunasResumoFinanceiroRelatorios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.colunasResumoFinanceiroRelatorios.Size = new System.Drawing.Size(1020, 166);
            this.colunasResumoFinanceiroRelatorios.TabIndex = 1;
            // 
            // lblTituloReceitaTotalRelatorios
            // 
            this.lblTituloReceitaTotalRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTituloReceitaTotalRelatorios.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloReceitaTotalRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTituloReceitaTotalRelatorios.Location = new System.Drawing.Point(10, 0);
            this.lblTituloReceitaTotalRelatorios.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.lblTituloReceitaTotalRelatorios.Name = "lblTituloReceitaTotalRelatorios";
            this.lblTituloReceitaTotalRelatorios.Size = new System.Drawing.Size(323, 24);
            this.lblTituloReceitaTotalRelatorios.TabIndex = 0;
            this.lblTituloReceitaTotalRelatorios.Text = "Receita Total (Mês)";
            this.lblTituloReceitaTotalRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceitaTotal
            // 
            this.lblReceitaTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReceitaTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceitaTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblReceitaTotal.Location = new System.Drawing.Point(10, 24);
            this.lblReceitaTotal.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.lblReceitaTotal.Name = "lblReceitaTotal";
            this.lblReceitaTotal.Size = new System.Drawing.Size(323, 34);
            this.lblReceitaTotal.TabIndex = 1;
            this.lblReceitaTotal.Text = "R$ 23.450,00";
            this.lblReceitaTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTituloReceitaRecebidaRelatorios
            // 
            this.lblTituloReceitaRecebidaRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTituloReceitaRecebidaRelatorios.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloReceitaRecebidaRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTituloReceitaRecebidaRelatorios.Location = new System.Drawing.Point(348, 0);
            this.lblTituloReceitaRecebidaRelatorios.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.lblTituloReceitaRecebidaRelatorios.Name = "lblTituloReceitaRecebidaRelatorios";
            this.lblTituloReceitaRecebidaRelatorios.Size = new System.Drawing.Size(323, 24);
            this.lblTituloReceitaRecebidaRelatorios.TabIndex = 2;
            this.lblTituloReceitaRecebidaRelatorios.Text = "Receitas Recebidas";
            this.lblTituloReceitaRecebidaRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceitaRecebida
            // 
            this.lblReceitaRecebida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReceitaRecebida.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceitaRecebida.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblReceitaRecebida.Location = new System.Drawing.Point(348, 24);
            this.lblReceitaRecebida.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.lblReceitaRecebida.Name = "lblReceitaRecebida";
            this.lblReceitaRecebida.Size = new System.Drawing.Size(323, 34);
            this.lblReceitaRecebida.TabIndex = 3;
            this.lblReceitaRecebida.Text = "R$ 21.830,00";
            this.lblReceitaRecebida.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTituloReceitaPendenteRelatorios
            // 
            this.lblTituloReceitaPendenteRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTituloReceitaPendenteRelatorios.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloReceitaPendenteRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTituloReceitaPendenteRelatorios.Location = new System.Drawing.Point(686, 0);
            this.lblTituloReceitaPendenteRelatorios.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.lblTituloReceitaPendenteRelatorios.Name = "lblTituloReceitaPendenteRelatorios";
            this.lblTituloReceitaPendenteRelatorios.Size = new System.Drawing.Size(330, 24);
            this.lblTituloReceitaPendenteRelatorios.TabIndex = 4;
            this.lblTituloReceitaPendenteRelatorios.Text = "Receitas Pendentes";
            this.lblTituloReceitaPendenteRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceitaPendente
            // 
            this.lblReceitaPendente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReceitaPendente.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceitaPendente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblReceitaPendente.Location = new System.Drawing.Point(686, 24);
            this.lblReceitaPendente.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.lblReceitaPendente.Name = "lblReceitaPendente";
            this.lblReceitaPendente.Size = new System.Drawing.Size(330, 34);
            this.lblReceitaPendente.TabIndex = 5;
            this.lblReceitaPendente.Text = "R$ 1.620,00";
            this.lblReceitaPendente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // divisorResumo1Relatorios
            // 
            this.divisorResumo1Relatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.divisorResumo1Relatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divisorResumo1Relatorios.Location = new System.Drawing.Point(337, 5);
            this.divisorResumo1Relatorios.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.divisorResumo1Relatorios.Name = "divisorResumo1Relatorios";
            this.colunasResumoFinanceiroRelatorios.SetRowSpan(this.divisorResumo1Relatorios, 3);
            this.divisorResumo1Relatorios.Size = new System.Drawing.Size(1, 156);
            this.divisorResumo1Relatorios.TabIndex = 6;
            // 
            // divisorResumo2Relatorios
            // 
            this.divisorResumo2Relatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(234)))), ((int)(((byte)(241)))));
            this.divisorResumo2Relatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divisorResumo2Relatorios.Location = new System.Drawing.Point(675, 5);
            this.divisorResumo2Relatorios.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.divisorResumo2Relatorios.Name = "divisorResumo2Relatorios";
            this.colunasResumoFinanceiroRelatorios.SetRowSpan(this.divisorResumo2Relatorios, 3);
            this.divisorResumo2Relatorios.Size = new System.Drawing.Size(1, 156);
            this.divisorResumo2Relatorios.TabIndex = 7;
            // 
            // RelatoriosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.painelConteudo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RelatoriosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Relatórios";
            this.painelConteudo.ResumeLayout(false);
            this.layoutRelatorios.ResumeLayout(false);
            this.barraSuperiorRelatorios.ResumeLayout(false);
            this.barraSuperiorRelatorios.PerformLayout();
            this.acoesRelatorios.ResumeLayout(false);
            this.panelPeriodoRelatorio.ResumeLayout(false);
            this.linhaIndicadoresRelatorios.ResumeLayout(false);
            this.cartaoReceitaRelatorios.ResumeLayout(false);
            this.cartaoReceitaRelatorios.PerformLayout();
            this.cartaoPresencaRelatorios.ResumeLayout(false);
            this.cartaoPresencaRelatorios.PerformLayout();
            this.cartaoAlunosRelatorios.ResumeLayout(false);
            this.cartaoAlunosRelatorios.PerformLayout();
            this.layoutGraficosRelatorios.ResumeLayout(false);
            this.cartaoGraficoPresencaRelatorios.ResumeLayout(false);
            this.estruturaGraficoPresencaRelatorios.ResumeLayout(false);
            this.cartaoGraficoReceitaRelatorios.ResumeLayout(false);
            this.estruturaGraficoReceitaRelatorios.ResumeLayout(false);
            this.painelPeriodoGraficoRelatorios.ResumeLayout(false);
            this.estruturaResumoFinanceiroRelatorios.ResumeLayout(false);
            this.cartaoResumoFinanceiroRelatorios.ResumeLayout(false);
            this.cartaoResumoFinanceiroRelatorios.PerformLayout();
            this.colunasResumoFinanceiroRelatorios.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
