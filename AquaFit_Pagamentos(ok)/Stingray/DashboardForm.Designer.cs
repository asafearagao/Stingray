namespace Stingray
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel sideBar;
        private System.Windows.Forms.TableLayoutPanel sideLayout;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblTagline;
        private System.Windows.Forms.FlowLayoutPanel navPanel;
        private Stingray.RoundedButton btnDashboard;
        private Stingray.RoundedButton btnAlunos;
        private Stingray.RoundedButton btnProfessores;
        private Stingray.RoundedButton btnTurmas;
        private Stingray.RoundedButton btnHorarios;
        private Stingray.RoundedButton btnMatriculas;
        private Stingray.RoundedButton btnPagamentos;
        private Stingray.RoundedButton btnPresenca;
        private Stingray.RoundedButton btnRelatorios;
        private Stingray.RoundedButton btnConfiguracoes;
        private System.Windows.Forms.Panel footerPanel;
        private Stingray.RoundedButton btnSair;
        private System.Windows.Forms.Panel contentHost;
        private System.Windows.Forms.Panel dashboardPagePanel;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Panel dashboardContentPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.sideBar = new System.Windows.Forms.Panel();
            this.sideLayout = new System.Windows.Forms.TableLayoutPanel();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblTagline = new System.Windows.Forms.Label();
            this.navPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDashboard = new Stingray.RoundedButton();
            this.btnAlunos = new Stingray.RoundedButton();
            this.btnProfessores = new Stingray.RoundedButton();
            this.btnTurmas = new Stingray.RoundedButton();
            this.btnHorarios = new Stingray.RoundedButton();
            this.btnMatriculas = new Stingray.RoundedButton();
            this.btnPagamentos = new Stingray.RoundedButton();
            this.btnPresenca = new Stingray.RoundedButton();
            this.btnRelatorios = new Stingray.RoundedButton();
            this.btnConfiguracoes = new Stingray.RoundedButton();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.btnSair = new Stingray.RoundedButton();
            this.contentHost = new System.Windows.Forms.Panel();
            this.dashboardPagePanel = new System.Windows.Forms.Panel();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.dashboardContentPanel = new System.Windows.Forms.Panel();
            this.mainLayout.SuspendLayout();
            this.sideBar.SuspendLayout();
            this.sideLayout.SuspendLayout();
            this.logoPanel.SuspendLayout();
            this.navPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.contentHost.SuspendLayout();
            this.dashboardPagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.mainLayout.Controls.Add(this.sideBar, 0, 0);
            this.mainLayout.Controls.Add(this.contentHost, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.TabIndex = 0;
            // 
            // sideBar
            // 
            this.sideBar.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.sideBar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sideBar.Controls.Add(this.sideLayout);
            this.sideBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideBar.Margin = new System.Windows.Forms.Padding(0);
            this.sideBar.Name = "sideBar";
            this.sideBar.Padding = new System.Windows.Forms.Padding(1);
            this.sideBar.TabIndex = 0;
            // 
            // sideLayout
            // 
            this.sideLayout.ColumnCount = 1;
            this.sideLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.sideLayout.Controls.Add(this.logoPanel, 0, 0);
            this.sideLayout.Controls.Add(this.navPanel, 0, 1);
            this.sideLayout.Controls.Add(this.footerPanel, 0, 2);
            this.sideLayout.BackColor = System.Drawing.Color.White;
            this.sideLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideLayout.Margin = new System.Windows.Forms.Padding(0);
            this.sideLayout.Name = "sideLayout";
            this.sideLayout.RowCount = 3;
            this.sideLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.sideLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.sideLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.sideLayout.TabIndex = 0;
            // 
            // logoPanel
            // 
            this.logoPanel.BackColor = System.Drawing.Color.White;
            this.logoPanel.Controls.Add(this.lblBrand);
            this.logoPanel.Controls.Add(this.lblTagline);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPanel.Margin = new System.Windows.Forms.Padding(0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.TabIndex = 0;
            // 
            // lblBrand
            // 
            this.lblBrand.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrand.ForeColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.lblBrand.Location = new System.Drawing.Point(0, 40);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(119, 41);
            this.lblBrand.TabIndex = 0;
            this.lblBrand.Text = "AquaFit";
            // 
            // lblTagline
            // 
            this.lblTagline.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.lblTagline.AutoSize = true;
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagline.ForeColor = System.Drawing.Color.FromArgb(91, 125, 189);
            this.lblTagline.Location = new System.Drawing.Point(0, 83);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(91, 12);
            this.lblTagline.TabIndex = 1;
            this.lblTagline.Text = "ESCOLA DE NATAÇÃO";
            // 
            // navPanel
            // 
            this.navPanel.AutoScroll = true;
            this.navPanel.BackColor = System.Drawing.Color.White;
            this.navPanel.Controls.Add(this.btnDashboard);
            this.navPanel.Controls.Add(this.btnAlunos);
            this.navPanel.Controls.Add(this.btnProfessores);
            this.navPanel.Controls.Add(this.btnTurmas);
            this.navPanel.Controls.Add(this.btnHorarios);
            this.navPanel.Controls.Add(this.btnMatriculas);
            this.navPanel.Controls.Add(this.btnPagamentos);
            this.navPanel.Controls.Add(this.btnPresenca);
            this.navPanel.Controls.Add(this.btnRelatorios);
            this.navPanel.Controls.Add(this.btnConfiguracoes);
            this.navPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.navPanel.Location = new System.Drawing.Point(0, 136);
            this.navPanel.Margin = new System.Windows.Forms.Padding(0);
            this.navPanel.Name = "navPanel";
            this.navPanel.Padding = new System.Windows.Forms.Padding(9, 2, 9, 0);
            this.navPanel.WrapContents = false;
            this.navPanel.TabIndex = 1;
            // 
            // navigation buttons
            //
            this.btnDashboard.BackColor = System.Drawing.Color.White;
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.CornerRadius = 7;
            this.btnDashboard.Size = new System.Drawing.Size(236, 37);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnAlunos.BackColor = System.Drawing.Color.White;
            this.btnAlunos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlunos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnAlunos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnAlunos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnAlunos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlunos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlunos.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnAlunos.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnAlunos.Name = "btnAlunos";
            this.btnAlunos.CornerRadius = 7;
            this.btnAlunos.Size = new System.Drawing.Size(236, 37);
            this.btnAlunos.TabIndex = 1;
            this.btnAlunos.Text = "Alunos";
            this.btnAlunos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAlunos.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAlunos.UseVisualStyleBackColor = false;
            this.btnProfessores.BackColor = System.Drawing.Color.White;
            this.btnProfessores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProfessores.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnProfessores.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnProfessores.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnProfessores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfessores.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfessores.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnProfessores.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnProfessores.Name = "btnProfessores";
            this.btnProfessores.CornerRadius = 7;
            this.btnProfessores.Size = new System.Drawing.Size(236, 37);
            this.btnProfessores.TabIndex = 2;
            this.btnProfessores.Text = "Professores";
            this.btnProfessores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProfessores.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnProfessores.UseVisualStyleBackColor = false;
            this.btnTurmas.BackColor = System.Drawing.Color.White;
            this.btnTurmas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTurmas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnTurmas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnTurmas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnTurmas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurmas.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurmas.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnTurmas.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnTurmas.Name = "btnTurmas";
            this.btnTurmas.CornerRadius = 7;
            this.btnTurmas.Size = new System.Drawing.Size(236, 37);
            this.btnTurmas.TabIndex = 3;
            this.btnTurmas.Text = "Turmas";
            this.btnTurmas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTurmas.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnTurmas.UseVisualStyleBackColor = false;
            this.btnHorarios.BackColor = System.Drawing.Color.White;
            this.btnHorarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHorarios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnHorarios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnHorarios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnHorarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHorarios.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHorarios.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnHorarios.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnHorarios.Name = "btnHorarios";
            this.btnHorarios.CornerRadius = 7;
            this.btnHorarios.Size = new System.Drawing.Size(236, 37);
            this.btnHorarios.TabIndex = 4;
            this.btnHorarios.Text = "Horários";
            this.btnHorarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHorarios.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnHorarios.UseVisualStyleBackColor = false;
            this.btnMatriculas.BackColor = System.Drawing.Color.White;
            this.btnMatriculas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMatriculas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnMatriculas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnMatriculas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnMatriculas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMatriculas.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMatriculas.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnMatriculas.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnMatriculas.Name = "btnMatriculas";
            this.btnMatriculas.CornerRadius = 7;
            this.btnMatriculas.Size = new System.Drawing.Size(236, 37);
            this.btnMatriculas.TabIndex = 5;
            this.btnMatriculas.Text = "Matrículas";
            this.btnMatriculas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMatriculas.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnMatriculas.UseVisualStyleBackColor = false;
            this.btnPagamentos.BackColor = System.Drawing.Color.White;
            this.btnPagamentos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPagamentos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnPagamentos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnPagamentos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnPagamentos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamentos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagamentos.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnPagamentos.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnPagamentos.Name = "btnPagamentos";
            this.btnPagamentos.CornerRadius = 7;
            this.btnPagamentos.Size = new System.Drawing.Size(236, 37);
            this.btnPagamentos.TabIndex = 6;
            this.btnPagamentos.Text = "Pagamentos";
            this.btnPagamentos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPagamentos.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPagamentos.UseVisualStyleBackColor = false;
            this.btnPresenca.BackColor = System.Drawing.Color.White;
            this.btnPresenca.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPresenca.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnPresenca.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnPresenca.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnPresenca.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPresenca.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPresenca.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnPresenca.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnPresenca.Name = "btnPresenca";
            this.btnPresenca.CornerRadius = 7;
            this.btnPresenca.Size = new System.Drawing.Size(236, 37);
            this.btnPresenca.TabIndex = 7;
            this.btnPresenca.Text = "Presença";
            this.btnPresenca.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPresenca.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPresenca.UseVisualStyleBackColor = false;
            this.btnRelatorios.BackColor = System.Drawing.Color.White;
            this.btnRelatorios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnRelatorios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnRelatorios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelatorios.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnRelatorios.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.CornerRadius = 7;
            this.btnRelatorios.Size = new System.Drawing.Size(236, 37);
            this.btnRelatorios.TabIndex = 8;
            this.btnRelatorios.Text = "Relatórios";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelatorios.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRelatorios.UseVisualStyleBackColor = false;
            this.btnConfiguracoes.BackColor = System.Drawing.Color.White;
            this.btnConfiguracoes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfiguracoes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnConfiguracoes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnConfiguracoes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracoes.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfiguracoes.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnConfiguracoes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.CornerRadius = 7;
            this.btnConfiguracoes.Size = new System.Drawing.Size(236, 37);
            this.btnConfiguracoes.TabIndex = 9;
            this.btnConfiguracoes.Text = "Configurações";
            this.btnConfiguracoes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracoes.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnConfiguracoes.UseVisualStyleBackColor = false;
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnDashboard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(185, 208, 242);
            this.btnPagamentos.BackColor = System.Drawing.Color.White;
            this.btnPagamentos.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnPagamentos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            // 
            // footerPanel
            // 
            this.footerPanel.BackColor = System.Drawing.Color.White;
            this.footerPanel.Controls.Add(this.btnSair);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(9, 0, 9, 9);
            this.footerPanel.TabIndex = 2;
            // 
            // btnSair
            // 
            this.btnSair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSair.BackColor = System.Drawing.Color.White;
            this.btnSair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSair.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnSair.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(219, 232, 253);
            this.btnSair.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnSair.Location = new System.Drawing.Point(9, 20);
            this.btnSair.Margin = new System.Windows.Forms.Padding(0);
            this.btnSair.Name = "btnSair";
            this.btnSair.CornerRadius = 7;
            this.btnSair.Size = new System.Drawing.Size(230, 37);
            this.btnSair.TabIndex = 0;
            this.btnSair.Text = "Sair";
            this.btnSair.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSair.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnSair.UseVisualStyleBackColor = false;
            // 
            // contentHost
            // 
            this.contentHost.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.contentHost.Controls.Add(this.dashboardPagePanel);
            this.contentHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentHost.Margin = new System.Windows.Forms.Padding(0);
            this.contentHost.Name = "contentHost";
            this.contentHost.TabIndex = 1;
            //
            // dashboardPagePanel
            //
            this.dashboardPagePanel.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.dashboardPagePanel.Controls.Add(this.dashboardContentPanel);
            this.dashboardPagePanel.Controls.Add(this.lblDashboardTitle);
            this.dashboardPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardPagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.dashboardPagePanel.Name = "dashboardPagePanel";
            this.dashboardPagePanel.TabIndex = 0;
            //
            // lblDashboardTitle
            //
            this.lblDashboardTitle.AutoSize = true;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashboardTitle.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblDashboardTitle.Location = new System.Drawing.Point(14, 12);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Size = new System.Drawing.Size(125, 32);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "Dashboard";
            //
            // dashboardContentPanel
            //
            this.dashboardContentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dashboardContentPanel.BackColor = System.Drawing.Color.White;
            this.dashboardContentPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dashboardContentPanel.Location = new System.Drawing.Point(14, 64);
            this.dashboardContentPanel.Name = "dashboardContentPanel";
            this.dashboardContentPanel.Size = new System.Drawing.Size(1018, 582);
            this.dashboardContentPanel.TabIndex = 1;

            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.ClientSize = new System.Drawing.Size(1360, 760);
            this.Controls.Add(this.mainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1100, 650);
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AquaFit - Sistema de Gestão";
            this.mainLayout.ResumeLayout(false);
            this.sideBar.ResumeLayout(false);
            this.sideLayout.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            this.logoPanel.PerformLayout();
            this.navPanel.ResumeLayout(false);
            this.footerPanel.ResumeLayout(false);
            this.dashboardPagePanel.ResumeLayout(false);
            this.dashboardPagePanel.PerformLayout();
            this.contentHost.ResumeLayout(false);
            this.ResumeLayout(false);
        }

    }
}
