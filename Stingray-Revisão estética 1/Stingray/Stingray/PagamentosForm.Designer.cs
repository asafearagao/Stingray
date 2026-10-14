namespace Stingray {
    partial class PagamentosForm {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel paymentPagePanel;
        private System.Windows.Forms.TableLayoutPanel paymentLayout;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNovoPagamento;
        private System.Windows.Forms.Panel filtersPanel;
        private System.Windows.Forms.TableLayoutPanel filtersLayout;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblAluno;
        private System.Windows.Forms.Label lblTurma;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel periodoField;
        private System.Windows.Forms.Panel alunoField;
        private System.Windows.Forms.Panel turmaField;
        private System.Windows.Forms.Panel statusField;
        private System.Windows.Forms.TextBox txtPeriodo;
        private System.Windows.Forms.Button btnCalendario;
        private System.Windows.Forms.TextBox txtAluno;
        private System.Windows.Forms.ComboBox cmbTurma;
        private System.Windows.Forms.ComboBox cmbStatus;
        private Stingray.RoundedButton btnPesquisar;
        private Stingray.RoundedButton btnLimpar;
        private System.Windows.Forms.Panel tablePanel;
        private System.Windows.Forms.DataGridView gridPayments;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataPagamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAluno;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatricula;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTurma;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReferencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFormaPagamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAcoes;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.paymentPagePanel = new System.Windows.Forms.Panel();
            this.paymentLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnNovoPagamento = new System.Windows.Forms.Button();
            this.filtersPanel = new System.Windows.Forms.Panel();
            this.filtersLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblAluno = new System.Windows.Forms.Label();
            this.lblTurma = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.periodoField = new System.Windows.Forms.Panel();
            this.txtPeriodo = new System.Windows.Forms.TextBox();
            this.btnCalendario = new System.Windows.Forms.Button();
            this.alunoField = new System.Windows.Forms.Panel();
            this.txtAluno = new System.Windows.Forms.TextBox();
            this.turmaField = new System.Windows.Forms.Panel();
            this.cmbTurma = new System.Windows.Forms.ComboBox();
            this.statusField = new System.Windows.Forms.Panel();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnPesquisar = new Stingray.RoundedButton();
            this.btnLimpar = new Stingray.RoundedButton();
            this.tablePanel = new System.Windows.Forms.Panel();
            this.gridPayments = new System.Windows.Forms.DataGridView();
            this.colDataPagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAluno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatricula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTurma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReferencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFormaPagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAcoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentPagePanel.SuspendLayout();
            this.paymentLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.filtersPanel.SuspendLayout();
            this.filtersLayout.SuspendLayout();
            this.periodoField.SuspendLayout();
            this.alunoField.SuspendLayout();
            this.turmaField.SuspendLayout();
            this.statusField.SuspendLayout();
            this.tablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // paymentPagePanel
            // 
            this.paymentPagePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.paymentPagePanel.Controls.Add(this.paymentLayout);
            this.paymentPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentPagePanel.Location = new System.Drawing.Point(0, 0);
            this.paymentPagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.paymentPagePanel.Name = "paymentPagePanel";
            this.paymentPagePanel.Size = new System.Drawing.Size(1100, 680);
            this.paymentPagePanel.TabIndex = 0;
            // 
            // paymentLayout
            // 
            this.paymentLayout.ColumnCount = 1;
            this.paymentLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.paymentLayout.Controls.Add(this.headerPanel, 0, 0);
            this.paymentLayout.Controls.Add(this.filtersPanel, 0, 1);
            this.paymentLayout.Controls.Add(this.tablePanel, 0, 2);
            this.paymentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentLayout.Location = new System.Drawing.Point(0, 0);
            this.paymentLayout.Margin = new System.Windows.Forms.Padding(0);
            this.paymentLayout.Name = "paymentLayout";
            this.paymentLayout.RowCount = 3;
            this.paymentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.paymentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.paymentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.paymentLayout.Size = new System.Drawing.Size(1100, 680);
            this.paymentLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.btnNovoPagamento);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1100, 64);
            this.headerPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitle.Location = new System.Drawing.Point(13, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(154, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Pagamentos";
            // 
            // btnNovoPagamento
            // 
            this.btnNovoPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNovoPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(86)))), ((int)(((byte)(196)))));
            this.btnNovoPagamento.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNovoPagamento.FlatAppearance.BorderSize = 0;
            this.btnNovoPagamento.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(61)))), ((int)(((byte)(140)))));
            this.btnNovoPagamento.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(73)))), ((int)(((byte)(168)))));
            this.btnNovoPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovoPagamento.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNovoPagamento.ForeColor = System.Drawing.Color.White;
            this.btnNovoPagamento.Location = new System.Drawing.Point(1786, 7);
            this.btnNovoPagamento.Name = "btnNovoPagamento";
            this.btnNovoPagamento.Size = new System.Drawing.Size(194, 36);
            this.btnNovoPagamento.TabIndex = 1;
            this.btnNovoPagamento.Text = "+   Novo pagamento";
            this.btnNovoPagamento.UseVisualStyleBackColor = false;
            // 
            // filtersPanel
            // 
            this.filtersPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.filtersPanel.Controls.Add(this.filtersLayout);
            this.filtersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filtersPanel.Location = new System.Drawing.Point(12, 66);
            this.filtersPanel.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Padding = new System.Windows.Forms.Padding(1);
            this.filtersPanel.Size = new System.Drawing.Size(1076, 58);
            this.filtersPanel.TabIndex = 1;
            // 
            // filtersLayout
            // 
            this.filtersLayout.BackColor = System.Drawing.Color.White;
            this.filtersLayout.ColumnCount = 6;
            this.filtersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.filtersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.filtersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.filtersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.filtersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.filtersLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.filtersLayout.Controls.Add(this.lblPeriodo, 0, 0);
            this.filtersLayout.Controls.Add(this.lblAluno, 1, 0);
            this.filtersLayout.Controls.Add(this.lblTurma, 2, 0);
            this.filtersLayout.Controls.Add(this.lblStatus, 3, 0);
            this.filtersLayout.Controls.Add(this.periodoField, 0, 1);
            this.filtersLayout.Controls.Add(this.alunoField, 1, 1);
            this.filtersLayout.Controls.Add(this.turmaField, 2, 1);
            this.filtersLayout.Controls.Add(this.statusField, 3, 1);
            this.filtersLayout.Controls.Add(this.btnPesquisar, 4, 1);
            this.filtersLayout.Controls.Add(this.btnLimpar, 5, 1);
            this.filtersLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filtersLayout.Location = new System.Drawing.Point(1, 1);
            this.filtersLayout.Margin = new System.Windows.Forms.Padding(0);
            this.filtersLayout.Name = "filtersLayout";
            this.filtersLayout.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.filtersLayout.RowCount = 2;
            this.filtersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.filtersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.filtersLayout.Size = new System.Drawing.Size(1074, 56);
            this.filtersLayout.TabIndex = 0;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPeriodo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblPeriodo.Location = new System.Drawing.Point(12, 5);
            this.lblPeriodo.Margin = new System.Windows.Forms.Padding(0);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(168, 18);
            this.lblPeriodo.TabIndex = 0;
            this.lblPeriodo.Text = "Período:";
            this.lblPeriodo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAluno
            // 
            this.lblAluno.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAluno.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAluno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblAluno.Location = new System.Drawing.Point(180, 5);
            this.lblAluno.Margin = new System.Windows.Forms.Padding(0);
            this.lblAluno.Name = "lblAluno";
            this.lblAluno.Size = new System.Drawing.Size(315, 18);
            this.lblAluno.TabIndex = 1;
            this.lblAluno.Text = "Aluno:";
            this.lblAluno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTurma
            // 
            this.lblTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblTurma.Location = new System.Drawing.Point(495, 5);
            this.lblTurma.Margin = new System.Windows.Forms.Padding(0);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(178, 18);
            this.lblTurma.TabIndex = 2;
            this.lblTurma.Text = "Turma:";
            this.lblTurma.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblStatus.Location = new System.Drawing.Point(673, 5);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(178, 18);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // periodoField
            // 
            this.periodoField.BackColor = System.Drawing.Color.White;
            this.periodoField.Controls.Add(this.txtPeriodo);
            this.periodoField.Controls.Add(this.btnCalendario);
            this.periodoField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.periodoField.Location = new System.Drawing.Point(12, 25);
            this.periodoField.Margin = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.periodoField.Name = "periodoField";
            this.periodoField.Size = new System.Drawing.Size(162, 24);
            this.periodoField.TabIndex = 0;
            // 
            // txtPeriodo
            // 
            this.txtPeriodo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPeriodo.BackColor = System.Drawing.Color.White;
            this.txtPeriodo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPeriodo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtPeriodo.Location = new System.Drawing.Point(6, 4);
            this.txtPeriodo.Margin = new System.Windows.Forms.Padding(0);
            this.txtPeriodo.Name = "txtPeriodo";
            this.txtPeriodo.ReadOnly = true;
            this.txtPeriodo.Size = new System.Drawing.Size(119, 18);
            this.txtPeriodo.TabIndex = 0;
            this.txtPeriodo.Text = "Selecione o período";
            // 
            // btnCalendario
            // 
            this.btnCalendario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalendario.BackColor = System.Drawing.Color.White;
            this.btnCalendario.FlatAppearance.BorderSize = 0;
            this.btnCalendario.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.btnCalendario.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.btnCalendario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendario.Font = new System.Drawing.Font("Segoe UI Symbol", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalendario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnCalendario.Location = new System.Drawing.Point(119, 2);
            this.btnCalendario.Margin = new System.Windows.Forms.Padding(0);
            this.btnCalendario.Name = "btnCalendario";
            this.btnCalendario.Size = new System.Drawing.Size(26, 24);
            this.btnCalendario.TabIndex = 1;
            this.btnCalendario.UseVisualStyleBackColor = false;
            // 
            // alunoField
            // 
            this.alunoField.BackColor = System.Drawing.Color.White;
            this.alunoField.Controls.Add(this.txtAluno);
            this.alunoField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alunoField.Location = new System.Drawing.Point(184, 25);
            this.alunoField.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.alunoField.Name = "alunoField";
            this.alunoField.Size = new System.Drawing.Size(306, 24);
            this.alunoField.TabIndex = 1;
            // 
            // txtAluno
            // 
            this.txtAluno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAluno.BackColor = System.Drawing.Color.White;
            this.txtAluno.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAluno.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAluno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtAluno.Location = new System.Drawing.Point(6, -35);
            this.txtAluno.Margin = new System.Windows.Forms.Padding(0);
            this.txtAluno.Name = "txtAluno";
            this.txtAluno.Size = new System.Drawing.Size(376, 19);
            this.txtAluno.TabIndex = 0;
            this.txtAluno.Text = "Digite o nome do aluno...";
            // 
            // turmaField
            // 
            this.turmaField.BackColor = System.Drawing.Color.White;
            this.turmaField.Controls.Add(this.cmbTurma);
            this.turmaField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.turmaField.Location = new System.Drawing.Point(499, 24);
            this.turmaField.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.turmaField.Name = "turmaField";
            this.turmaField.Size = new System.Drawing.Size(169, 26);
            this.turmaField.TabIndex = 2;
            // 
            // cmbTurma
            // 
            this.cmbTurma.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTurma.BackColor = System.Drawing.Color.White;
            this.cmbTurma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTurma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTurma.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.cmbTurma.FormattingEnabled = true;
            this.cmbTurma.Items.AddRange(new object[] {
            "Todas",
            "Hidroginástica",
            "Natação Iniciante",
            "Natação Intermediário",
            "Natação Avançado",
            "Natação Adulto",
            "Natação Master",
            "Natação Competição"});
            this.cmbTurma.Location = new System.Drawing.Point(5, 2);
            this.cmbTurma.Margin = new System.Windows.Forms.Padding(0);
            this.cmbTurma.Name = "cmbTurma";
            this.cmbTurma.Size = new System.Drawing.Size(149, 25);
            this.cmbTurma.TabIndex = 2;
            // 
            // statusField
            // 
            this.statusField.BackColor = System.Drawing.Color.White;
            this.statusField.Controls.Add(this.cmbStatus);
            this.statusField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusField.Location = new System.Drawing.Point(677, 24);
            this.statusField.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.statusField.Name = "statusField";
            this.statusField.Size = new System.Drawing.Size(169, 26);
            this.statusField.TabIndex = 3;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "Todos",
            "Pago",
            "Pendente",
            "Atrasado"});
            this.cmbStatus.Location = new System.Drawing.Point(5, 2);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(0);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(149, 25);
            this.cmbStatus.TabIndex = 3;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.BackColor = System.Drawing.Color.White;
            this.btnPesquisar.CornerRadius = 5;
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnPesquisar.FlatAppearance.BorderSize = 0;
            this.btnPesquisar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.btnPesquisar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(86)))), ((int)(((byte)(196)))));
            this.btnPesquisar.Location = new System.Drawing.Point(855, 25);
            this.btnPesquisar.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(96, 24);
            this.btnPesquisar.TabIndex = 4;
            this.btnPesquisar.Text = "⌕  Pesquisar";
            this.btnPesquisar.UseVisualStyleBackColor = false;
            // 
            // btnLimpar
            // 
            this.btnLimpar.BackColor = System.Drawing.Color.White;
            this.btnLimpar.CornerRadius = 5;
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimpar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnLimpar.FlatAppearance.BorderSize = 0;
            this.btnLimpar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnLimpar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnLimpar.Location = new System.Drawing.Point(960, 25);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(98, 24);
            this.btnLimpar.TabIndex = 5;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = false;
            // 
            // tablePanel
            // 
            this.tablePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.tablePanel.Controls.Add(this.gridPayments);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Location = new System.Drawing.Point(12, 132);
            this.tablePanel.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.Padding = new System.Windows.Forms.Padding(1);
            this.tablePanel.Size = new System.Drawing.Size(1076, 536);
            this.tablePanel.TabIndex = 2;
            // 
            // gridPayments
            // 
            this.gridPayments.AllowUserToAddRows = false;
            this.gridPayments.AllowUserToDeleteRows = false;
            this.gridPayments.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.gridPayments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridPayments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPayments.BackgroundColor = System.Drawing.Color.White;
            this.gridPayments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridPayments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridPayments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridPayments.ColumnHeadersHeight = 38;
            this.gridPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDataPagamento,
            this.colAluno,
            this.colMatricula,
            this.colTurma,
            this.colReferencia,
            this.colFormaPagamento,
            this.colValor,
            this.colStatus,
            this.colAcoes});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridPayments.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPayments.EnableHeadersVisualStyles = false;
            this.gridPayments.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridPayments.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(245)))));
            this.gridPayments.Location = new System.Drawing.Point(1, 1);
            this.gridPayments.Margin = new System.Windows.Forms.Padding(0);
            this.gridPayments.MultiSelect = false;
            this.gridPayments.Name = "gridPayments";
            this.gridPayments.ReadOnly = true;
            this.gridPayments.RowHeadersVisible = false;
            this.gridPayments.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.gridPayments.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridPayments.RowTemplate.Height = 38;
            this.gridPayments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPayments.Size = new System.Drawing.Size(1074, 534);
            this.gridPayments.TabIndex = 0;
            // 
            // colDataPagamento
            // 
            this.colDataPagamento.FillWeight = 105F;
            this.colDataPagamento.HeaderText = "Data do Pagamento";
            this.colDataPagamento.MinimumWidth = 45;
            this.colDataPagamento.Name = "colDataPagamento";
            this.colDataPagamento.ReadOnly = true;
            this.colDataPagamento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDataPagamento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAluno
            // 
            this.colAluno.FillWeight = 110F;
            this.colAluno.HeaderText = "Aluno";
            this.colAluno.MinimumWidth = 45;
            this.colAluno.Name = "colAluno";
            this.colAluno.ReadOnly = true;
            this.colAluno.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAluno.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMatricula
            // 
            this.colMatricula.FillWeight = 90F;
            this.colMatricula.HeaderText = "Matrícula";
            this.colMatricula.MinimumWidth = 45;
            this.colMatricula.Name = "colMatricula";
            this.colMatricula.ReadOnly = true;
            this.colMatricula.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMatricula.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTurma
            // 
            this.colTurma.FillWeight = 115F;
            this.colTurma.HeaderText = "Turma";
            this.colTurma.MinimumWidth = 45;
            this.colTurma.Name = "colTurma";
            this.colTurma.ReadOnly = true;
            this.colTurma.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTurma.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colReferencia
            // 
            this.colReferencia.FillWeight = 90F;
            this.colReferencia.HeaderText = "Referência";
            this.colReferencia.MinimumWidth = 45;
            this.colReferencia.Name = "colReferencia";
            this.colReferencia.ReadOnly = true;
            this.colReferencia.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colReferencia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colFormaPagamento
            // 
            this.colFormaPagamento.FillWeight = 125F;
            this.colFormaPagamento.HeaderText = "Forma de Pagamento";
            this.colFormaPagamento.MinimumWidth = 45;
            this.colFormaPagamento.Name = "colFormaPagamento";
            this.colFormaPagamento.ReadOnly = true;
            this.colFormaPagamento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFormaPagamento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colValor
            // 
            this.colValor.FillWeight = 80F;
            this.colValor.HeaderText = "Valor (R$)";
            this.colValor.MinimumWidth = 45;
            this.colValor.Name = "colValor";
            this.colValor.ReadOnly = true;
            this.colValor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colValor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colStatus
            // 
            this.colStatus.FillWeight = 70F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 45;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAcoes
            // 
            this.colAcoes.FillWeight = 82F;
            this.colAcoes.HeaderText = "Ações";
            this.colAcoes.MinimumWidth = 45;
            this.colAcoes.Name = "colAcoes";
            this.colAcoes.ReadOnly = true;
            this.colAcoes.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAcoes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PagamentosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1100, 680);
            this.Controls.Add(this.paymentPagePanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "PagamentosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pagamentos";
            this.paymentPagePanel.ResumeLayout(false);
            this.paymentLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.filtersPanel.ResumeLayout(false);
            this.filtersLayout.ResumeLayout(false);
            this.periodoField.ResumeLayout(false);
            this.periodoField.PerformLayout();
            this.alunoField.ResumeLayout(false);
            this.alunoField.PerformLayout();
            this.turmaField.ResumeLayout(false);
            this.statusField.ResumeLayout(false);
            this.tablePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPayments)).EndInit();
            this.ResumeLayout(false);

        }
    }
}