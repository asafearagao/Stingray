namespace Stingray
{
    partial class PagamentosForm
    {
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.alunoField = new System.Windows.Forms.Panel();
            this.turmaField = new System.Windows.Forms.Panel();
            this.statusField = new System.Windows.Forms.Panel();
            this.txtPeriodo = new System.Windows.Forms.TextBox();
            this.btnCalendario = new System.Windows.Forms.Button();
            this.txtAluno = new System.Windows.Forms.TextBox();
            this.cmbTurma = new System.Windows.Forms.ComboBox();
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
            this.tablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // paymentPagePanel
            // 
            this.paymentPagePanel.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.paymentPagePanel.Controls.Add(this.paymentLayout);
            this.paymentPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentPagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.paymentPagePanel.Name = "paymentPagePanel";
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
            this.paymentLayout.Margin = new System.Windows.Forms.Padding(0);
            this.paymentLayout.Name = "paymentLayout";
            this.paymentLayout.RowCount = 3;
            this.paymentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.paymentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.paymentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.paymentLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.btnNovoPagamento);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblTitle.Location = new System.Drawing.Point(13, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Pagamentos";
            // 
            // btnNovoPagamento
            // 
            this.btnNovoPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNovoPagamento.BackColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnNovoPagamento.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNovoPagamento.FlatAppearance.BorderSize = 0;
            this.btnNovoPagamento.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(18, 61, 140);
            this.btnNovoPagamento.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(22, 73, 168);
            this.btnNovoPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovoPagamento.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNovoPagamento.ForeColor = System.Drawing.Color.White;
            this.btnNovoPagamento.Location = new System.Drawing.Point(886, 7);
            this.btnNovoPagamento.Name = "btnNovoPagamento";
            this.btnNovoPagamento.Size = new System.Drawing.Size(194, 36);
            this.btnNovoPagamento.TabIndex = 1;
            this.btnNovoPagamento.Text = "+   Novo pagamento";
            this.btnNovoPagamento.UseVisualStyleBackColor = false;
            // 
            // filtersPanel
            // 
            this.filtersPanel.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.filtersPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filtersPanel.Controls.Add(this.filtersLayout);
            this.filtersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filtersPanel.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Padding = new System.Windows.Forms.Padding(1);
            this.filtersPanel.TabIndex = 1;
            // 
            // filtersLayout
            // 
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
            this.filtersLayout.BackColor = System.Drawing.Color.White;
            this.filtersLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filtersLayout.Margin = new System.Windows.Forms.Padding(0);
            this.filtersLayout.Name = "filtersLayout";
            this.filtersLayout.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.filtersLayout.RowCount = 2;
            this.filtersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.filtersLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.filtersLayout.TabIndex = 0;
            // 
            // filter labels
            this.lblPeriodo.AutoSize = false;
            this.lblPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPeriodo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodo.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblPeriodo.Margin = new System.Windows.Forms.Padding(0);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Text = "Período:";
            this.lblPeriodo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAluno.AutoSize = false;
            this.lblAluno.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAluno.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAluno.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAluno.Margin = new System.Windows.Forms.Padding(0);
            this.lblAluno.Name = "lblAluno";
            this.lblAluno.Text = "Aluno:";
            this.lblAluno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTurma.AutoSize = false;
            this.lblTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblTurma.Margin = new System.Windows.Forms.Padding(0);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Text = "Turma:";
            this.lblTurma.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.AutoSize = false;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // periodoField
            // 
            this.periodoField.BackColor = System.Drawing.Color.White;
            this.periodoField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.periodoField.Padding = new System.Windows.Forms.Padding(0);
            this.periodoField.Controls.Add(this.txtPeriodo);
            this.periodoField.Controls.Add(this.btnCalendario);
            this.periodoField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.periodoField.Margin = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.periodoField.Name = "periodoField";
            this.periodoField.TabIndex = 0;
            // 
            // txtPeriodo
            // 
            this.txtPeriodo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPeriodo.Dock = System.Windows.Forms.DockStyle.None;
            this.txtPeriodo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtPeriodo.BackColor = System.Drawing.Color.White;
            this.txtPeriodo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPeriodo.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtPeriodo.Location = new System.Drawing.Point(6, 4);
            this.txtPeriodo.Margin = new System.Windows.Forms.Padding(0);
            this.txtPeriodo.Name = "txtPeriodo";
            this.txtPeriodo.ReadOnly = true;
            this.txtPeriodo.Size = new System.Drawing.Size(119, 15);
            this.txtPeriodo.TabIndex = 0;
            this.txtPeriodo.Text = "Selecione o período";
            this.txtPeriodo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnCalendario
            // 
            this.btnCalendario.BackColor = System.Drawing.Color.White;
            this.btnCalendario.Dock = System.Windows.Forms.DockStyle.None;
            this.btnCalendario.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCalendario.FlatAppearance.BorderSize = 0;
            this.btnCalendario.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnCalendario.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(245, 249, 255);
            this.btnCalendario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendario.Font = new System.Drawing.Font("Segoe UI Symbol", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalendario.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnCalendario.Location = new System.Drawing.Point(119, 2);
            this.btnCalendario.Margin = new System.Windows.Forms.Padding(0);
            this.btnCalendario.Name = "btnCalendario";
            this.btnCalendario.Size = new System.Drawing.Size(26, 24);
            this.btnCalendario.TabIndex = 1;
            this.btnCalendario.Text = "";
            this.btnCalendario.UseVisualStyleBackColor = false;
            // 
            // alunoField - mesma borda cinza-clara dos demais campos
            // 
            this.alunoField.BackColor = System.Drawing.Color.White;
            this.alunoField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alunoField.Controls.Add(this.txtAluno);
            this.alunoField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alunoField.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.alunoField.Name = "alunoField";
            this.alunoField.TabIndex = 1;
            // 
            // txtAluno
            // 
            this.txtAluno.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtAluno.BackColor = System.Drawing.Color.White;
            this.txtAluno.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAluno.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAluno.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.txtAluno.Location = new System.Drawing.Point(6, 3);
            this.txtAluno.Margin = new System.Windows.Forms.Padding(0);
            this.txtAluno.Name = "txtAluno";
            this.txtAluno.Size = new System.Drawing.Size(270, 22);
            this.txtAluno.TabIndex = 0;
            this.txtAluno.Text = "Digite o nome do aluno...";
            // 
            // turmaField - contorno cinza-claro completo do seletor
            // 
            this.turmaField.BackColor = System.Drawing.Color.White;
            this.turmaField.Padding = new System.Windows.Forms.Padding(0);
            this.turmaField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.turmaField.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.turmaField.Name = "turmaField";
            this.turmaField.TabIndex = 2;
            this.turmaField.Controls.Add(this.cmbTurma);
            // statusField - contorno cinza-claro completo do seletor
            // 
            this.statusField.BackColor = System.Drawing.Color.White;
            this.statusField.Padding = new System.Windows.Forms.Padding(0);
            this.statusField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusField.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.statusField.Name = "statusField";
            this.statusField.TabIndex = 3;
            this.statusField.Controls.Add(this.cmbStatus);
            // cmbTurma
            // 
            this.cmbTurma.Dock = System.Windows.Forms.DockStyle.None;
            this.cmbTurma.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.cmbTurma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTurma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTurma.BackColor = System.Drawing.Color.White;
            this.cmbTurma.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.cmbTurma.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTurma.FormattingEnabled = true;
            this.cmbTurma.Items.AddRange(new object[] { "Todas", "Hidroginástica", "Natação Iniciante", "Natação Intermediário", "Natação Avançado", "Natação Adulto", "Natação Master", "Natação Competição" });
            this.cmbTurma.Location = new System.Drawing.Point(5, 2);
            this.cmbTurma.Margin = new System.Windows.Forms.Padding(0);
            this.cmbTurma.Name = "cmbTurma";
            this.cmbTurma.Size = new System.Drawing.Size(141, 21);
            this.cmbTurma.TabIndex = 2;
            this.cmbTurma.SelectedIndex = 0;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.None;
            this.cmbStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] { "Todos", "Pago", "Pendente", "Atrasado" });
            this.cmbStatus.Location = new System.Drawing.Point(5, 2);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(0);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(141, 21);
            this.cmbStatus.TabIndex = 3;
            this.cmbStatus.SelectedIndex = 0;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.BackColor = System.Drawing.Color.White;
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPesquisar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnPesquisar.FlatAppearance.BorderSize = 0;
            this.btnPesquisar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(234, 242, 255);
            this.btnPesquisar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.ForeColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnPesquisar.Location = new System.Drawing.Point(766, 23);
            this.btnPesquisar.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.CornerRadius = 5;
            this.btnPesquisar.Size = new System.Drawing.Size(88, 30);
            this.btnPesquisar.TabIndex = 4;
            this.btnPesquisar.Text = "⌕  Pesquisar";
            this.btnPesquisar.UseVisualStyleBackColor = false;
            // 
            // btnLimpar
            // 
            this.btnLimpar.BackColor = System.Drawing.Color.White;
            this.btnLimpar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimpar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnLimpar.FlatAppearance.BorderSize = 0;
            this.btnLimpar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(254, 226, 226);
            this.btnLimpar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(254, 242, 242);
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnLimpar.Location = new System.Drawing.Point(863, 23);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.CornerRadius = 5;
            this.btnLimpar.Size = new System.Drawing.Size(79, 30);
            this.btnLimpar.TabIndex = 5;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = false;
            // 
            // tablePanel
            // 
            this.tablePanel.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.tablePanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tablePanel.Controls.Add(this.gridPayments);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.Padding = new System.Windows.Forms.Padding(1);
            this.tablePanel.TabIndex = 2;
            // 
            // gridPayments
            // 
            this.gridPayments.AllowUserToAddRows = false;
            this.gridPayments.AllowUserToDeleteRows = false;
            this.gridPayments.AllowUserToResizeRows = false;
            this.gridPayments.AutoGenerateColumns = false;
            this.gridPayments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPayments.BackgroundColor = System.Drawing.Color.White;
            this.gridPayments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridPayments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridPayments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridPayments.ColumnHeadersHeight = 38;
            this.gridPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDataPagamento, this.colAluno, this.colMatricula, this.colTurma,
                this.colReferencia, this.colFormaPagamento,
                this.colValor, this.colStatus, this.colAcoes});
            this.gridPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPayments.EnableHeadersVisualStyles = false;
            this.gridPayments.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridPayments.GridColor = System.Drawing.Color.FromArgb(235, 239, 245);
            this.gridPayments.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.gridPayments.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.gridPayments.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridPayments.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridPayments.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.gridPayments.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.gridPayments.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.gridPayments.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.gridPayments.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.gridPayments.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridPayments.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridPayments.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.gridPayments.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.gridPayments.Location = new System.Drawing.Point(0, 0);
            this.gridPayments.Margin = new System.Windows.Forms.Padding(0);
            this.gridPayments.MultiSelect = false;
            this.gridPayments.Name = "gridPayments";
            this.gridPayments.ReadOnly = true;
            this.gridPayments.RowHeadersVisible = false;
            this.gridPayments.RowTemplate.Height = 38;
            this.gridPayments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPayments.Size = new System.Drawing.Size(1040, 560);
            this.gridPayments.TabIndex = 0;
            // 
            // data grid columns
            this.colDataPagamento.HeaderText = "Data do Pagamento";
            this.colDataPagamento.FillWeight = 105F;
            this.colDataPagamento.MinimumWidth = 45;
            this.colDataPagamento.Name = "colDataPagamento";
            this.colDataPagamento.ReadOnly = true;
            this.colDataPagamento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDataPagamento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAluno.HeaderText = "Aluno";
            this.colAluno.FillWeight = 110F;
            this.colAluno.MinimumWidth = 45;
            this.colAluno.Name = "colAluno";
            this.colAluno.ReadOnly = true;
            this.colAluno.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAluno.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMatricula.HeaderText = "Matrícula";
            this.colMatricula.FillWeight = 90F;
            this.colMatricula.MinimumWidth = 45;
            this.colMatricula.Name = "colMatricula";
            this.colMatricula.ReadOnly = true;
            this.colMatricula.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMatricula.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTurma.HeaderText = "Turma";
            this.colTurma.FillWeight = 115F;
            this.colTurma.MinimumWidth = 45;
            this.colTurma.Name = "colTurma";
            this.colTurma.ReadOnly = true;
            this.colTurma.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTurma.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colReferencia.HeaderText = "Referência";
            this.colReferencia.FillWeight = 90F;
            this.colReferencia.MinimumWidth = 45;
            this.colReferencia.Name = "colReferencia";
            this.colReferencia.ReadOnly = true;
            this.colReferencia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colReferencia.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFormaPagamento.HeaderText = "Forma de Pagamento";
            this.colFormaPagamento.FillWeight = 125F;
            this.colFormaPagamento.MinimumWidth = 45;
            this.colFormaPagamento.Name = "colFormaPagamento";
            this.colFormaPagamento.ReadOnly = true;
            this.colFormaPagamento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFormaPagamento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colValor.HeaderText = "Valor (R$)";
            this.colValor.FillWeight = 80F;
            this.colValor.MinimumWidth = 45;
            this.colValor.Name = "colValor";
            this.colValor.ReadOnly = true;
            this.colValor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colValor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colStatus.HeaderText = "Status";
            this.colStatus.FillWeight = 70F;
            this.colStatus.MinimumWidth = 45;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAcoes.HeaderText = "Ações";
            this.colAcoes.FillWeight = 82F;
            this.colAcoes.MinimumWidth = 45;
            this.colAcoes.Name = "colAcoes";
            this.colAcoes.ReadOnly = true;
            this.colAcoes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAcoes.Resizable = System.Windows.Forms.DataGridViewTriState.False;

            this.Controls.Add(this.paymentPagePanel);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.ClientSize = new System.Drawing.Size(1100, 680);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "PagamentosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pagamentos";
            this.tablePanel.ResumeLayout(false);
            this.filtersLayout.ResumeLayout(false);
            this.filtersLayout.PerformLayout();
            this.filtersPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.paymentLayout.ResumeLayout(false);
            this.paymentPagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPayments)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
