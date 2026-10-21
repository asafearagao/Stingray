namespace Stingray {
    partial class AlunosForm {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel painelConteudo;
        private System.Windows.Forms.Button btnNovoAluno;
        private System.Windows.Forms.TableLayoutPanel layoutAlunos;
        private System.Windows.Forms.Panel pnlFiltrosAlunos;
        private System.Windows.Forms.TableLayoutPanel layoutFiltrosAlunos;
        private System.Windows.Forms.Label lblFiltroNome;
        private System.Windows.Forms.Label lblFiltroTurma;
        private System.Windows.Forms.Label lblFiltroProfessor;
        private System.Windows.Forms.Label lblFiltroStatus;
        private System.Windows.Forms.Panel pnlBuscaCampo;
        private System.Windows.Forms.Panel pnlBuscaSuperficie;
        private System.Windows.Forms.Panel pnlTurmaCampo;
        private System.Windows.Forms.Panel pnlTurmaSuperficie;
        private System.Windows.Forms.Panel pnlProfessorCampo;
        private System.Windows.Forms.Panel pnlProfessorSuperficie;
        private System.Windows.Forms.Panel pnlStatusCampo;
        private System.Windows.Forms.Panel pnlStatusSuperficie;
        private System.Windows.Forms.TableLayoutPanel layoutFiltroTurma;
        private System.Windows.Forms.TableLayoutPanel layoutFiltroProfessor;
        private System.Windows.Forms.TableLayoutPanel layoutFiltroStatus;
        private System.Windows.Forms.Button btnSetaFiltroTurma;
        private System.Windows.Forms.Button btnSetaFiltroProfessor;
        private System.Windows.Forms.Button btnSetaFiltroStatus;
        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.ComboBox cboFiltroTurma;
        private System.Windows.Forms.ComboBox cboFiltroProfessor;
        private System.Windows.Forms.ComboBox cboFiltroStatus;
        private Stingray.RoundedButton btnPesquisar;
        private Stingray.RoundedButton btnLimpar;
        private System.Windows.Forms.Panel pnlMolduraTabelaAlunos;
        private System.Windows.Forms.DataGridView dgvAlunos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAluno;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTurma;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfessor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlano;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAcoes;
        private System.Windows.Forms.Panel pnlRodapeAlunos;
        private System.Windows.Forms.Label lblResumo;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.painelConteudo = new System.Windows.Forms.Panel();
            this.layoutAlunos = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFiltrosAlunos = new System.Windows.Forms.Panel();
            this.layoutFiltrosAlunos = new System.Windows.Forms.TableLayoutPanel();
            this.lblFiltroNome = new System.Windows.Forms.Label();
            this.lblFiltroTurma = new System.Windows.Forms.Label();
            this.lblFiltroProfessor = new System.Windows.Forms.Label();
            this.lblFiltroStatus = new System.Windows.Forms.Label();
            this.pnlBuscaCampo = new System.Windows.Forms.Panel();
            this.pnlBuscaSuperficie = new System.Windows.Forms.Panel();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.pnlTurmaCampo = new System.Windows.Forms.Panel();
            this.pnlTurmaSuperficie = new System.Windows.Forms.Panel();
            this.layoutFiltroTurma = new System.Windows.Forms.TableLayoutPanel();
            this.cboFiltroTurma = new System.Windows.Forms.ComboBox();
            this.btnSetaFiltroTurma = new System.Windows.Forms.Button();
            this.pnlProfessorCampo = new System.Windows.Forms.Panel();
            this.pnlProfessorSuperficie = new System.Windows.Forms.Panel();
            this.layoutFiltroProfessor = new System.Windows.Forms.TableLayoutPanel();
            this.cboFiltroProfessor = new System.Windows.Forms.ComboBox();
            this.btnSetaFiltroProfessor = new System.Windows.Forms.Button();
            this.pnlStatusCampo = new System.Windows.Forms.Panel();
            this.pnlStatusSuperficie = new System.Windows.Forms.Panel();
            this.layoutFiltroStatus = new System.Windows.Forms.TableLayoutPanel();
            this.cboFiltroStatus = new System.Windows.Forms.ComboBox();
            this.btnSetaFiltroStatus = new System.Windows.Forms.Button();
            this.btnPesquisar = new Stingray.RoundedButton();
            this.btnLimpar = new Stingray.RoundedButton();
            this.pnlMolduraTabelaAlunos = new System.Windows.Forms.Panel();
            this.dgvAlunos = new System.Windows.Forms.DataGridView();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAluno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTurma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfessor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlano = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAcoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRodapeAlunos = new System.Windows.Forms.Panel();
            this.lblResumo = new System.Windows.Forms.Label();
            this.btnNovoAluno = new System.Windows.Forms.Button();
            this.painelConteudo.SuspendLayout();
            this.layoutAlunos.SuspendLayout();
            this.pnlFiltrosAlunos.SuspendLayout();
            this.layoutFiltrosAlunos.SuspendLayout();
            this.pnlBuscaCampo.SuspendLayout();
            this.pnlBuscaSuperficie.SuspendLayout();
            this.pnlTurmaCampo.SuspendLayout();
            this.pnlTurmaSuperficie.SuspendLayout();
            this.layoutFiltroTurma.SuspendLayout();
            this.pnlProfessorCampo.SuspendLayout();
            this.pnlProfessorSuperficie.SuspendLayout();
            this.layoutFiltroProfessor.SuspendLayout();
            this.pnlStatusCampo.SuspendLayout();
            this.pnlStatusSuperficie.SuspendLayout();
            this.layoutFiltroStatus.SuspendLayout();
            this.pnlMolduraTabelaAlunos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlunos)).BeginInit();
            this.pnlRodapeAlunos.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitulo.Location = new System.Drawing.Point(13, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(74, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Alunos";
            // 
            // painelConteudo
            // 
            this.painelConteudo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.painelConteudo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.painelConteudo.Controls.Add(this.layoutAlunos);
            this.painelConteudo.Location = new System.Drawing.Point(12, 64);
            this.painelConteudo.Name = "painelConteudo";
            this.painelConteudo.Size = new System.Drawing.Size(1026, 584);
            this.painelConteudo.TabIndex = 1;
            // 
            // layoutAlunos
            // 
            this.layoutAlunos.ColumnCount = 1;
            this.layoutAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutAlunos.Controls.Add(this.pnlFiltrosAlunos, 0, 0);
            this.layoutAlunos.Controls.Add(this.pnlMolduraTabelaAlunos, 0, 1);
            this.layoutAlunos.Controls.Add(this.pnlRodapeAlunos, 0, 2);
            this.layoutAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutAlunos.Location = new System.Drawing.Point(0, 0);
            this.layoutAlunos.Margin = new System.Windows.Forms.Padding(0);
            this.layoutAlunos.Name = "layoutAlunos";
            this.layoutAlunos.RowCount = 3;
            this.layoutAlunos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.layoutAlunos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutAlunos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.layoutAlunos.Size = new System.Drawing.Size(1026, 584);
            this.layoutAlunos.TabIndex = 0;
            // 
            // pnlFiltrosAlunos
            // 
            this.pnlFiltrosAlunos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlFiltrosAlunos.Controls.Add(this.layoutFiltrosAlunos);
            this.pnlFiltrosAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltrosAlunos.Location = new System.Drawing.Point(12, 2);
            this.pnlFiltrosAlunos.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.pnlFiltrosAlunos.Name = "pnlFiltrosAlunos";
            this.pnlFiltrosAlunos.Padding = new System.Windows.Forms.Padding(1);
            this.pnlFiltrosAlunos.Size = new System.Drawing.Size(1002, 58);
            this.pnlFiltrosAlunos.TabIndex = 0;
            // 
            // layoutFiltrosAlunos
            // 
            this.layoutFiltrosAlunos.BackColor = System.Drawing.Color.White;
            this.layoutFiltrosAlunos.ColumnCount = 6;
            this.layoutFiltrosAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.layoutFiltrosAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.layoutFiltrosAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.layoutFiltrosAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.layoutFiltrosAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.layoutFiltrosAlunos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutFiltrosAlunos.Controls.Add(this.lblFiltroNome, 0, 0);
            this.layoutFiltrosAlunos.Controls.Add(this.lblFiltroTurma, 1, 0);
            this.layoutFiltrosAlunos.Controls.Add(this.lblFiltroProfessor, 2, 0);
            this.layoutFiltrosAlunos.Controls.Add(this.lblFiltroStatus, 3, 0);
            this.layoutFiltrosAlunos.Controls.Add(this.pnlBuscaCampo, 0, 1);
            this.layoutFiltrosAlunos.Controls.Add(this.pnlTurmaCampo, 1, 1);
            this.layoutFiltrosAlunos.Controls.Add(this.pnlProfessorCampo, 2, 1);
            this.layoutFiltrosAlunos.Controls.Add(this.pnlStatusCampo, 3, 1);
            this.layoutFiltrosAlunos.Controls.Add(this.btnPesquisar, 4, 1);
            this.layoutFiltrosAlunos.Controls.Add(this.btnLimpar, 5, 1);
            this.layoutFiltrosAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFiltrosAlunos.Location = new System.Drawing.Point(1, 1);
            this.layoutFiltrosAlunos.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFiltrosAlunos.Name = "layoutFiltrosAlunos";
            this.layoutFiltrosAlunos.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.layoutFiltrosAlunos.RowCount = 2;
            this.layoutFiltrosAlunos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.layoutFiltrosAlunos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutFiltrosAlunos.Size = new System.Drawing.Size(1000, 56);
            this.layoutFiltrosAlunos.TabIndex = 0;
            // 
            // lblFiltroNome
            // 
            this.lblFiltroNome.AutoSize = true;
            this.lblFiltroNome.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltroNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblFiltroNome.Location = new System.Drawing.Point(12, 5);
            this.lblFiltroNome.Margin = new System.Windows.Forms.Padding(0);
            this.lblFiltroNome.Name = "lblFiltroNome";
            this.lblFiltroNome.Size = new System.Drawing.Size(37, 13);
            this.lblFiltroNome.TabIndex = 0;
            this.lblFiltroNome.Text = "Nome";
            this.lblFiltroNome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFiltroTurma
            // 
            this.lblFiltroTurma.AutoSize = true;
            this.lblFiltroTurma.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltroTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblFiltroTurma.Location = new System.Drawing.Point(273, 5);
            this.lblFiltroTurma.Margin = new System.Windows.Forms.Padding(0);
            this.lblFiltroTurma.Name = "lblFiltroTurma";
            this.lblFiltroTurma.Size = new System.Drawing.Size(38, 13);
            this.lblFiltroTurma.TabIndex = 1;
            this.lblFiltroTurma.Text = "Turma";
            this.lblFiltroTurma.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFiltroProfessor
            // 
            this.lblFiltroProfessor.AutoSize = true;
            this.lblFiltroProfessor.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltroProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblFiltroProfessor.Location = new System.Drawing.Point(441, 5);
            this.lblFiltroProfessor.Margin = new System.Windows.Forms.Padding(0);
            this.lblFiltroProfessor.Name = "lblFiltroProfessor";
            this.lblFiltroProfessor.Size = new System.Drawing.Size(55, 13);
            this.lblFiltroProfessor.TabIndex = 2;
            this.lblFiltroProfessor.Text = "Professor";
            this.lblFiltroProfessor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFiltroStatus
            // 
            this.lblFiltroStatus.AutoSize = true;
            this.lblFiltroStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltroStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblFiltroStatus.Location = new System.Drawing.Point(609, 5);
            this.lblFiltroStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblFiltroStatus.Name = "lblFiltroStatus";
            this.lblFiltroStatus.Size = new System.Drawing.Size(39, 13);
            this.lblFiltroStatus.TabIndex = 3;
            this.lblFiltroStatus.Text = "Status";
            this.lblFiltroStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBuscaCampo
            // 
            this.pnlBuscaCampo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlBuscaCampo.Controls.Add(this.pnlBuscaSuperficie);
            this.pnlBuscaCampo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBuscaCampo.Location = new System.Drawing.Point(12, 24);
            this.pnlBuscaCampo.Margin = new System.Windows.Forms.Padding(0, 1, 5, 1);
            this.pnlBuscaCampo.Name = "pnlBuscaCampo";
            this.pnlBuscaCampo.Padding = new System.Windows.Forms.Padding(1);
            this.pnlBuscaCampo.Size = new System.Drawing.Size(256, 28);
            this.pnlBuscaCampo.TabIndex = 4;
            // 
            // pnlBuscaSuperficie
            // 
            this.pnlBuscaSuperficie.BackColor = System.Drawing.Color.White;
            this.pnlBuscaSuperficie.Controls.Add(this.txtBusca);
            this.pnlBuscaSuperficie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBuscaSuperficie.Location = new System.Drawing.Point(1, 1);
            this.pnlBuscaSuperficie.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBuscaSuperficie.Name = "pnlBuscaSuperficie";
            this.pnlBuscaSuperficie.Padding = new System.Windows.Forms.Padding(8, 1, 6, 1);
            this.pnlBuscaSuperficie.Size = new System.Drawing.Size(254, 26);
            this.pnlBuscaSuperficie.TabIndex = 0;
            // 
            // txtBusca
            // 
            this.txtBusca.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBusca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBusca.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBusca.Location = new System.Drawing.Point(8, 1);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(0);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(240, 16);
            this.txtBusca.TabIndex = 0;
            // 
            // pnlTurmaCampo
            // 
            this.pnlTurmaCampo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlTurmaCampo.Controls.Add(this.pnlTurmaSuperficie);
            this.pnlTurmaCampo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTurmaCampo.Location = new System.Drawing.Point(273, 24);
            this.pnlTurmaCampo.Margin = new System.Windows.Forms.Padding(0, 1, 5, 1);
            this.pnlTurmaCampo.Name = "pnlTurmaCampo";
            this.pnlTurmaCampo.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTurmaCampo.Size = new System.Drawing.Size(163, 28);
            this.pnlTurmaCampo.TabIndex = 5;
            // 
            // pnlTurmaSuperficie
            // 
            this.pnlTurmaSuperficie.BackColor = System.Drawing.Color.White;
            this.pnlTurmaSuperficie.Controls.Add(this.layoutFiltroTurma);
            this.pnlTurmaSuperficie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTurmaSuperficie.Location = new System.Drawing.Point(1, 1);
            this.pnlTurmaSuperficie.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTurmaSuperficie.Name = "pnlTurmaSuperficie";
            this.pnlTurmaSuperficie.Size = new System.Drawing.Size(161, 26);
            this.pnlTurmaSuperficie.TabIndex = 0;
            // 
            // layoutFiltroTurma
            // 
            this.layoutFiltroTurma.ColumnCount = 2;
            this.layoutFiltroTurma.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiltroTurma.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layoutFiltroTurma.Controls.Add(this.cboFiltroTurma, 0, 0);
            this.layoutFiltroTurma.Controls.Add(this.btnSetaFiltroTurma, 1, 0);
            this.layoutFiltroTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFiltroTurma.Location = new System.Drawing.Point(0, 0);
            this.layoutFiltroTurma.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFiltroTurma.Name = "layoutFiltroTurma";
            this.layoutFiltroTurma.RowCount = 1;
            this.layoutFiltroTurma.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiltroTurma.Size = new System.Drawing.Size(161, 26);
            this.layoutFiltroTurma.TabIndex = 0;
            // 
            // cboFiltroTurma
            // 
            this.cboFiltroTurma.BackColor = System.Drawing.Color.White;
            this.cboFiltroTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboFiltroTurma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroTurma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiltroTurma.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFiltroTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.cboFiltroTurma.Items.AddRange(new object[] {
            "Todas",
            "Hidroginástica",
            "Natação Iniciante",
            "Natação Intermediário",
            "Natação Avançado",
            "Natação Adulto",
            "Natação Master",
            "Natação Competição"});
            this.cboFiltroTurma.Location = new System.Drawing.Point(0, 0);
            this.cboFiltroTurma.Margin = new System.Windows.Forms.Padding(0);
            this.cboFiltroTurma.Name = "cboFiltroTurma";
            this.cboFiltroTurma.Size = new System.Drawing.Size(135, 21);
            this.cboFiltroTurma.TabIndex = 0;
            // 
            // btnSetaFiltroTurma
            // 
            this.btnSetaFiltroTurma.BackColor = System.Drawing.Color.White;
            this.btnSetaFiltroTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetaFiltroTurma.FlatAppearance.BorderSize = 0;
            this.btnSetaFiltroTurma.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnSetaFiltroTurma.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnSetaFiltroTurma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetaFiltroTurma.Location = new System.Drawing.Point(135, 0);
            this.btnSetaFiltroTurma.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetaFiltroTurma.Name = "btnSetaFiltroTurma";
            this.btnSetaFiltroTurma.Size = new System.Drawing.Size(26, 26);
            this.btnSetaFiltroTurma.TabIndex = 1;
            this.btnSetaFiltroTurma.TabStop = false;
            this.btnSetaFiltroTurma.UseVisualStyleBackColor = false;
            // 
            // pnlProfessorCampo
            // 
            this.pnlProfessorCampo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlProfessorCampo.Controls.Add(this.pnlProfessorSuperficie);
            this.pnlProfessorCampo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProfessorCampo.Location = new System.Drawing.Point(441, 24);
            this.pnlProfessorCampo.Margin = new System.Windows.Forms.Padding(0, 1, 5, 1);
            this.pnlProfessorCampo.Name = "pnlProfessorCampo";
            this.pnlProfessorCampo.Padding = new System.Windows.Forms.Padding(1);
            this.pnlProfessorCampo.Size = new System.Drawing.Size(163, 28);
            this.pnlProfessorCampo.TabIndex = 6;
            // 
            // pnlProfessorSuperficie
            // 
            this.pnlProfessorSuperficie.BackColor = System.Drawing.Color.White;
            this.pnlProfessorSuperficie.Controls.Add(this.layoutFiltroProfessor);
            this.pnlProfessorSuperficie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProfessorSuperficie.Location = new System.Drawing.Point(1, 1);
            this.pnlProfessorSuperficie.Margin = new System.Windows.Forms.Padding(0);
            this.pnlProfessorSuperficie.Name = "pnlProfessorSuperficie";
            this.pnlProfessorSuperficie.Size = new System.Drawing.Size(161, 26);
            this.pnlProfessorSuperficie.TabIndex = 0;
            // 
            // layoutFiltroProfessor
            // 
            this.layoutFiltroProfessor.ColumnCount = 2;
            this.layoutFiltroProfessor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiltroProfessor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layoutFiltroProfessor.Controls.Add(this.cboFiltroProfessor, 0, 0);
            this.layoutFiltroProfessor.Controls.Add(this.btnSetaFiltroProfessor, 1, 0);
            this.layoutFiltroProfessor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFiltroProfessor.Location = new System.Drawing.Point(0, 0);
            this.layoutFiltroProfessor.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFiltroProfessor.Name = "layoutFiltroProfessor";
            this.layoutFiltroProfessor.RowCount = 1;
            this.layoutFiltroProfessor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiltroProfessor.Size = new System.Drawing.Size(161, 26);
            this.layoutFiltroProfessor.TabIndex = 0;
            // 
            // cboFiltroProfessor
            // 
            this.cboFiltroProfessor.BackColor = System.Drawing.Color.White;
            this.cboFiltroProfessor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboFiltroProfessor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroProfessor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiltroProfessor.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFiltroProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.cboFiltroProfessor.Items.AddRange(new object[] {
            "Todos",
            "Lucas Almeida",
            "Ana Paula Costa",
            "Gabriel Silva",
            "Mariana Oliveira",
            "Juliana Lima",
            "Rafael Santos"});
            this.cboFiltroProfessor.Location = new System.Drawing.Point(0, 0);
            this.cboFiltroProfessor.Margin = new System.Windows.Forms.Padding(0);
            this.cboFiltroProfessor.Name = "cboFiltroProfessor";
            this.cboFiltroProfessor.Size = new System.Drawing.Size(135, 21);
            this.cboFiltroProfessor.TabIndex = 0;
            // 
            // btnSetaFiltroProfessor
            // 
            this.btnSetaFiltroProfessor.BackColor = System.Drawing.Color.White;
            this.btnSetaFiltroProfessor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetaFiltroProfessor.FlatAppearance.BorderSize = 0;
            this.btnSetaFiltroProfessor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnSetaFiltroProfessor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnSetaFiltroProfessor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetaFiltroProfessor.Location = new System.Drawing.Point(135, 0);
            this.btnSetaFiltroProfessor.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetaFiltroProfessor.Name = "btnSetaFiltroProfessor";
            this.btnSetaFiltroProfessor.Size = new System.Drawing.Size(26, 26);
            this.btnSetaFiltroProfessor.TabIndex = 1;
            this.btnSetaFiltroProfessor.TabStop = false;
            this.btnSetaFiltroProfessor.UseVisualStyleBackColor = false;
            // 
            // pnlStatusCampo
            // 
            this.pnlStatusCampo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlStatusCampo.Controls.Add(this.pnlStatusSuperficie);
            this.pnlStatusCampo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatusCampo.Location = new System.Drawing.Point(609, 24);
            this.pnlStatusCampo.Margin = new System.Windows.Forms.Padding(0, 1, 5, 1);
            this.pnlStatusCampo.Name = "pnlStatusCampo";
            this.pnlStatusCampo.Padding = new System.Windows.Forms.Padding(1);
            this.pnlStatusCampo.Size = new System.Drawing.Size(163, 28);
            this.pnlStatusCampo.TabIndex = 7;
            // 
            // pnlStatusSuperficie
            // 
            this.pnlStatusSuperficie.BackColor = System.Drawing.Color.White;
            this.pnlStatusSuperficie.Controls.Add(this.layoutFiltroStatus);
            this.pnlStatusSuperficie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatusSuperficie.Location = new System.Drawing.Point(1, 1);
            this.pnlStatusSuperficie.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStatusSuperficie.Name = "pnlStatusSuperficie";
            this.pnlStatusSuperficie.Size = new System.Drawing.Size(161, 26);
            this.pnlStatusSuperficie.TabIndex = 0;
            // 
            // layoutFiltroStatus
            // 
            this.layoutFiltroStatus.ColumnCount = 2;
            this.layoutFiltroStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiltroStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layoutFiltroStatus.Controls.Add(this.cboFiltroStatus, 0, 0);
            this.layoutFiltroStatus.Controls.Add(this.btnSetaFiltroStatus, 1, 0);
            this.layoutFiltroStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFiltroStatus.Location = new System.Drawing.Point(0, 0);
            this.layoutFiltroStatus.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFiltroStatus.Name = "layoutFiltroStatus";
            this.layoutFiltroStatus.RowCount = 1;
            this.layoutFiltroStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiltroStatus.Size = new System.Drawing.Size(161, 26);
            this.layoutFiltroStatus.TabIndex = 0;
            // 
            // cboFiltroStatus
            // 
            this.cboFiltroStatus.BackColor = System.Drawing.Color.White;
            this.cboFiltroStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboFiltroStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiltroStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFiltroStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.cboFiltroStatus.Items.AddRange(new object[] {
            "Todos",
            "Ativa",
            "Pausada",
            "Cancelada"});
            this.cboFiltroStatus.Location = new System.Drawing.Point(0, 0);
            this.cboFiltroStatus.Margin = new System.Windows.Forms.Padding(0);
            this.cboFiltroStatus.Name = "cboFiltroStatus";
            this.cboFiltroStatus.Size = new System.Drawing.Size(135, 21);
            this.cboFiltroStatus.TabIndex = 0;
            // 
            // btnSetaFiltroStatus
            // 
            this.btnSetaFiltroStatus.BackColor = System.Drawing.Color.White;
            this.btnSetaFiltroStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetaFiltroStatus.FlatAppearance.BorderSize = 0;
            this.btnSetaFiltroStatus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnSetaFiltroStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnSetaFiltroStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetaFiltroStatus.Location = new System.Drawing.Point(135, 0);
            this.btnSetaFiltroStatus.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetaFiltroStatus.Name = "btnSetaFiltroStatus";
            this.btnSetaFiltroStatus.Size = new System.Drawing.Size(26, 26);
            this.btnSetaFiltroStatus.TabIndex = 1;
            this.btnSetaFiltroStatus.TabStop = false;
            this.btnSetaFiltroStatus.UseVisualStyleBackColor = false;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(86)))), ((int)(((byte)(196)))));
            this.btnPesquisar.CornerRadius = 5;
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPesquisar.FlatAppearance.BorderSize = 0;
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.ForeColor = System.Drawing.Color.White;
            this.btnPesquisar.Location = new System.Drawing.Point(781, 24);
            this.btnPesquisar.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(99, 28);
            this.btnPesquisar.TabIndex = 8;
            this.btnPesquisar.Text = "Pesquisar";
            this.btnPesquisar.UseVisualStyleBackColor = false;
            // 
            // btnLimpar
            // 
            this.btnLimpar.BackColor = System.Drawing.Color.White;
            this.btnLimpar.CornerRadius = 5;
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimpar.FlatAppearance.BorderSize = 0;
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnLimpar.Location = new System.Drawing.Point(889, 24);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(99, 28);
            this.btnLimpar.TabIndex = 9;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = false;
            // 
            // pnlMolduraTabelaAlunos
            // 
            this.pnlMolduraTabelaAlunos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlMolduraTabelaAlunos.Controls.Add(this.dgvAlunos);
            this.pnlMolduraTabelaAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMolduraTabelaAlunos.Location = new System.Drawing.Point(12, 68);
            this.pnlMolduraTabelaAlunos.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlMolduraTabelaAlunos.Name = "pnlMolduraTabelaAlunos";
            this.pnlMolduraTabelaAlunos.Padding = new System.Windows.Forms.Padding(1);
            this.pnlMolduraTabelaAlunos.Size = new System.Drawing.Size(1002, 476);
            this.pnlMolduraTabelaAlunos.TabIndex = 1;
            // 
            // dgvAlunos
            // 
            this.dgvAlunos.AllowUserToAddRows = false;
            this.dgvAlunos.AllowUserToDeleteRows = false;
            this.dgvAlunos.AllowUserToResizeRows = false;
            this.dgvAlunos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlunos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlunos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAlunos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAlunos.ColumnHeadersHeight = 38;
            this.dgvAlunos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAlunos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colData,
            this.colAluno,
            this.colTurma,
            this.colProfessor,
            this.colPlano,
            this.colStatus,
            this.colAcoes});
            this.dgvAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlunos.EnableHeadersVisualStyles = false;
            this.dgvAlunos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(245)))));
            this.dgvAlunos.Location = new System.Drawing.Point(1, 1);
            this.dgvAlunos.Margin = new System.Windows.Forms.Padding(0);
            this.dgvAlunos.MultiSelect = false;
            this.dgvAlunos.Name = "dgvAlunos";
            this.dgvAlunos.ReadOnly = true;
            this.dgvAlunos.RowHeadersVisible = false;
            this.dgvAlunos.RowTemplate.Height = 38;
            this.dgvAlunos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvAlunos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlunos.Size = new System.Drawing.Size(1000, 474);
            this.dgvAlunos.TabIndex = 0;
            // 
            // colData
            // 
            this.colData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colData.FillWeight = 90F;
            this.colData.HeaderText = "Data da Matrícula";
            this.colData.Name = "colData";
            this.colData.ReadOnly = true;
            this.colData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAluno
            // 
            this.colAluno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAluno.FillWeight = 110F;
            this.colAluno.HeaderText = "Aluno";
            this.colAluno.Name = "colAluno";
            this.colAluno.ReadOnly = true;
            this.colAluno.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTurma
            // 
            this.colTurma.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTurma.HeaderText = "Turma";
            this.colTurma.Name = "colTurma";
            this.colTurma.ReadOnly = true;
            this.colTurma.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colProfessor
            // 
            this.colProfessor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProfessor.HeaderText = "Professor";
            this.colProfessor.Name = "colProfessor";
            this.colProfessor.ReadOnly = true;
            this.colProfessor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPlano
            // 
            this.colPlano.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPlano.FillWeight = 80F;
            this.colPlano.HeaderText = "Plano";
            this.colPlano.Name = "colPlano";
            this.colPlano.ReadOnly = true;
            this.colPlano.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStatus.FillWeight = 80F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAcoes
            // 
            this.colAcoes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAcoes.HeaderText = "Ações";
            this.colAcoes.MinimumWidth = 100;
            this.colAcoes.Name = "colAcoes";
            this.colAcoes.ReadOnly = true;
            this.colAcoes.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAcoes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pnlRodapeAlunos
            // 
            this.pnlRodapeAlunos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlRodapeAlunos.Controls.Add(this.lblResumo);
            this.pnlRodapeAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRodapeAlunos.Location = new System.Drawing.Point(12, 556);
            this.pnlRodapeAlunos.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.pnlRodapeAlunos.Name = "pnlRodapeAlunos";
            this.pnlRodapeAlunos.Size = new System.Drawing.Size(1002, 28);
            this.pnlRodapeAlunos.TabIndex = 2;
            // 
            // lblResumo
            // 
            this.lblResumo.AutoSize = true;
            this.lblResumo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblResumo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblResumo.Location = new System.Drawing.Point(0, 0);
            this.lblResumo.Margin = new System.Windows.Forms.Padding(0);
            this.lblResumo.Name = "lblResumo";
            this.lblResumo.Size = new System.Drawing.Size(111, 13);
            this.lblResumo.TabIndex = 0;
            this.lblResumo.Text = "Mostrando 0 alunos";
            // 
            // btnNovoAluno
            // 
            this.btnNovoAluno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNovoAluno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(86)))), ((int)(((byte)(196)))));
            this.btnNovoAluno.FlatAppearance.BorderSize = 0;
            this.btnNovoAluno.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(61)))), ((int)(((byte)(140)))));
            this.btnNovoAluno.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(73)))), ((int)(((byte)(168)))));
            this.btnNovoAluno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovoAluno.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNovoAluno.ForeColor = System.Drawing.Color.White;
            this.btnNovoAluno.Location = new System.Drawing.Point(830, 14);
            this.btnNovoAluno.Name = "btnNovoAluno";
            this.btnNovoAluno.Size = new System.Drawing.Size(194, 36);
            this.btnNovoAluno.TabIndex = 2;
            this.btnNovoAluno.Text = "+   Novo Aluno";
            this.btnNovoAluno.UseVisualStyleBackColor = false;
            // 
            // AlunosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.btnNovoAluno);
            this.Controls.Add(this.painelConteudo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AlunosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alunos";
            this.painelConteudo.ResumeLayout(false);
            this.layoutAlunos.ResumeLayout(false);
            this.pnlFiltrosAlunos.ResumeLayout(false);
            this.layoutFiltrosAlunos.ResumeLayout(false);
            this.layoutFiltrosAlunos.PerformLayout();
            this.pnlBuscaCampo.ResumeLayout(false);
            this.pnlBuscaSuperficie.ResumeLayout(false);
            this.pnlBuscaSuperficie.PerformLayout();
            this.pnlTurmaCampo.ResumeLayout(false);
            this.pnlTurmaSuperficie.ResumeLayout(false);
            this.layoutFiltroTurma.ResumeLayout(false);
            this.pnlProfessorCampo.ResumeLayout(false);
            this.pnlProfessorSuperficie.ResumeLayout(false);
            this.layoutFiltroProfessor.ResumeLayout(false);
            this.pnlStatusCampo.ResumeLayout(false);
            this.pnlStatusSuperficie.ResumeLayout(false);
            this.layoutFiltroStatus.ResumeLayout(false);
            this.pnlMolduraTabelaAlunos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlunos)).EndInit();
            this.pnlRodapeAlunos.ResumeLayout(false);
            this.pnlRodapeAlunos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
