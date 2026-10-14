namespace Stingray {
    partial class PresencaForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlPresenca = new System.Windows.Forms.Panel();
            this.tlpPresenca = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCabecalho = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.tlpFiltros = new System.Windows.Forms.TableLayoutPanel();
            this.lblDataAula = new System.Windows.Forms.Label();
            this.lblTurma = new System.Windows.Forms.Label();
            this.pnlDataAula = new System.Windows.Forms.Panel();
            this.txtDataAula = new System.Windows.Forms.TextBox();
            this.btnCalendario = new System.Windows.Forms.Button();
            this.pnlTurma = new System.Windows.Forms.Panel();
            this.cmbTurma = new System.Windows.Forms.ComboBox();
            this.pnlTabela = new System.Windows.Forms.Panel();
            this.gdvPresenca = new System.Windows.Forms.DataGridView();
            this.colAluno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReferencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAcoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTotal = new System.Windows.Forms.Panel();
            this.tlpTotal = new System.Windows.Forms.TableLayoutPanel();
            this.lblPresentes = new System.Windows.Forms.Label();
            this.lblNumPresentes = new System.Windows.Forms.Label();
            this.lblAusentes = new System.Windows.Forms.Label();
            this.lblNumAusentes = new System.Windows.Forms.Label();
            this.lblJustificados = new System.Windows.Forms.Label();
            this.lblNumJustificados = new System.Windows.Forms.Label();
            this.lblTotalAlunos = new System.Windows.Forms.Label();
            this.lblNumTotalAlunos = new System.Windows.Forms.Label();
            this.pnlPresenca.SuspendLayout();
            this.tlpPresenca.SuspendLayout();
            this.pnlCabecalho.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.tlpFiltros.SuspendLayout();
            this.pnlDataAula.SuspendLayout();
            this.pnlTurma.SuspendLayout();
            this.pnlTabela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvPresenca)).BeginInit();
            this.pnlTotal.SuspendLayout();
            this.tlpTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPresenca
            // 
            this.pnlPresenca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlPresenca.Controls.Add(this.tlpPresenca);
            this.pnlPresenca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPresenca.Location = new System.Drawing.Point(0, 0);
            this.pnlPresenca.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPresenca.Name = "pnlPresenca";
            this.pnlPresenca.Size = new System.Drawing.Size(1050, 660);
            this.pnlPresenca.TabIndex = 1;
            // 
            // tlpPresenca
            // 
            this.tlpPresenca.ColumnCount = 1;
            this.tlpPresenca.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPresenca.Controls.Add(this.pnlCabecalho, 0, 0);
            this.tlpPresenca.Controls.Add(this.pnlFiltros, 0, 1);
            this.tlpPresenca.Controls.Add(this.pnlTabela, 0, 2);
            this.tlpPresenca.Controls.Add(this.pnlTotal, 0, 3);
            this.tlpPresenca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPresenca.Location = new System.Drawing.Point(0, 0);
            this.tlpPresenca.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPresenca.Name = "tlpPresenca";
            this.tlpPresenca.RowCount = 4;
            this.tlpPresenca.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tlpPresenca.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tlpPresenca.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPresenca.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tlpPresenca.Size = new System.Drawing.Size(1050, 660);
            this.tlpPresenca.TabIndex = 0;
            // 
            // pnlCabecalho
            // 
            this.pnlCabecalho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlCabecalho.Controls.Add(this.lblTitulo);
            this.pnlCabecalho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCabecalho.Location = new System.Drawing.Point(0, 0);
            this.pnlCabecalho.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCabecalho.Name = "pnlCabecalho";
            this.pnlCabecalho.Size = new System.Drawing.Size(1050, 64);
            this.pnlCabecalho.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitulo.Location = new System.Drawing.Point(13, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(116, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Presença";
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlFiltros.Controls.Add(this.tlpFiltros);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltros.Location = new System.Drawing.Point(12, 66);
            this.pnlFiltros.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Padding = new System.Windows.Forms.Padding(1);
            this.pnlFiltros.Size = new System.Drawing.Size(1026, 58);
            this.pnlFiltros.TabIndex = 1;
            // 
            // tlpFiltros
            // 
            this.tlpFiltros.BackColor = System.Drawing.Color.White;
            this.tlpFiltros.ColumnCount = 2;
            this.tlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpFiltros.Controls.Add(this.lblDataAula, 0, 0);
            this.tlpFiltros.Controls.Add(this.lblTurma, 1, 0);
            this.tlpFiltros.Controls.Add(this.pnlDataAula, 0, 1);
            this.tlpFiltros.Controls.Add(this.pnlTurma, 1, 1);
            this.tlpFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFiltros.Location = new System.Drawing.Point(1, 1);
            this.tlpFiltros.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFiltros.Name = "tlpFiltros";
            this.tlpFiltros.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.tlpFiltros.RowCount = 2;
            this.tlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFiltros.Size = new System.Drawing.Size(1024, 56);
            this.tlpFiltros.TabIndex = 0;
            // 
            // lblDataAula
            // 
            this.lblDataAula.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataAula.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataAula.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblDataAula.Location = new System.Drawing.Point(12, 5);
            this.lblDataAula.Margin = new System.Windows.Forms.Padding(0);
            this.lblDataAula.Name = "lblDataAula";
            this.lblDataAula.Size = new System.Drawing.Size(500, 18);
            this.lblDataAula.TabIndex = 0;
            this.lblDataAula.Text = "Data da Aula:";
            this.lblDataAula.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTurma
            // 
            this.lblTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblTurma.Location = new System.Drawing.Point(512, 5);
            this.lblTurma.Margin = new System.Windows.Forms.Padding(0);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(500, 18);
            this.lblTurma.TabIndex = 1;
            this.lblTurma.Text = "Turma:";
            this.lblTurma.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDataAula
            // 
            this.pnlDataAula.BackColor = System.Drawing.Color.White;
            this.pnlDataAula.Controls.Add(this.txtDataAula);
            this.pnlDataAula.Controls.Add(this.btnCalendario);
            this.pnlDataAula.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataAula.Location = new System.Drawing.Point(12, 25);
            this.pnlDataAula.Margin = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.pnlDataAula.Name = "pnlDataAula";
            this.pnlDataAula.Size = new System.Drawing.Size(494, 24);
            this.pnlDataAula.TabIndex = 0;
            // 
            // txtDataAula
            // 
            this.txtDataAula.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataAula.BackColor = System.Drawing.Color.White;
            this.txtDataAula.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataAula.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataAula.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtDataAula.Location = new System.Drawing.Point(6, 4);
            this.txtDataAula.Margin = new System.Windows.Forms.Padding(0);
            this.txtDataAula.Name = "txtDataAula";
            this.txtDataAula.ReadOnly = true;
            this.txtDataAula.Size = new System.Drawing.Size(451, 18);
            this.txtDataAula.TabIndex = 0;
            this.txtDataAula.Text = "Selecione a data";
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
            this.btnCalendario.Location = new System.Drawing.Point(451, 2);
            this.btnCalendario.Margin = new System.Windows.Forms.Padding(0);
            this.btnCalendario.Name = "btnCalendario";
            this.btnCalendario.Size = new System.Drawing.Size(26, 24);
            this.btnCalendario.TabIndex = 1;
            this.btnCalendario.UseVisualStyleBackColor = false;
            // 
            // pnlTurma
            // 
            this.pnlTurma.BackColor = System.Drawing.Color.White;
            this.pnlTurma.Controls.Add(this.cmbTurma);
            this.pnlTurma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTurma.Location = new System.Drawing.Point(516, 25);
            this.pnlTurma.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.pnlTurma.Name = "pnlTurma";
            this.pnlTurma.Size = new System.Drawing.Size(491, 24);
            this.pnlTurma.TabIndex = 1;
            // 
            // cmbTurma
            //
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
            this.cmbTurma.Location = new System.Drawing.Point(6, 2);
            this.cmbTurma.Margin = new System.Windows.Forms.Padding(0);
            this.cmbTurma.Name = "cmbTurma";
            this.cmbTurma.Size = new System.Drawing.Size(480, 25);
            this.cmbTurma.TabIndex = 0;
            //
            // pnlTabela
            // 
            this.pnlTabela.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTabela.Controls.Add(this.gdvPresenca);
            this.pnlTabela.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabela.Location = new System.Drawing.Point(12, 132);
            this.pnlTabela.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlTabela.Name = "pnlTabela";
            this.pnlTabela.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTabela.Size = new System.Drawing.Size(1026, 448);
            this.pnlTabela.TabIndex = 2;
            // 
            // gdvPresenca
            // 
            this.gdvPresenca.AllowUserToAddRows = false;
            this.gdvPresenca.AllowUserToDeleteRows = false;
            this.gdvPresenca.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            this.gdvPresenca.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.gdvPresenca.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gdvPresenca.BackgroundColor = System.Drawing.Color.White;
            this.gdvPresenca.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gdvPresenca.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gdvPresenca.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gdvPresenca.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gdvPresenca.ColumnHeadersHeight = 38;
            this.gdvPresenca.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gdvPresenca.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAluno,
            this.colStatus,
            this.colReferencia,
            this.colAcoes});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gdvPresenca.DefaultCellStyle = dataGridViewCellStyle7;
            this.gdvPresenca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdvPresenca.EnableHeadersVisualStyles = false;
            this.gdvPresenca.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gdvPresenca.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(245)))));
            this.gdvPresenca.Location = new System.Drawing.Point(1, 1);
            this.gdvPresenca.Margin = new System.Windows.Forms.Padding(0);
            this.gdvPresenca.MultiSelect = false;
            this.gdvPresenca.Name = "gdvPresenca";
            this.gdvPresenca.ReadOnly = true;
            this.gdvPresenca.RowHeadersVisible = false;
            this.gdvPresenca.RowHeadersWidth = 51;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            this.gdvPresenca.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.gdvPresenca.RowTemplate.Height = 38;
            this.gdvPresenca.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gdvPresenca.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gdvPresenca.Size = new System.Drawing.Size(1024, 446);
            this.gdvPresenca.TabIndex = 0;
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
            // colStatus
            // 
            this.colStatus.FillWeight = 70F;
            this.colStatus.HeaderText = "Situação";
            this.colStatus.MinimumWidth = 45;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colReferencia
            // 
            this.colReferencia.FillWeight = 90F;
            this.colReferencia.HeaderText = "Observação";
            this.colReferencia.MinimumWidth = 45;
            this.colReferencia.Name = "colReferencia";
            this.colReferencia.ReadOnly = true;
            this.colReferencia.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colReferencia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // pnlTotal
            // 
            this.pnlTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTotal.Controls.Add(this.tlpTotal);
            this.pnlTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTotal.Location = new System.Drawing.Point(12, 594);
            this.pnlTotal.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.pnlTotal.Name = "pnlTotal";
            this.pnlTotal.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTotal.Size = new System.Drawing.Size(1026, 58);
            this.pnlTotal.TabIndex = 3;
            // 
            // tlpTotal
            // 
            this.tlpTotal.BackColor = System.Drawing.Color.White;
            this.tlpTotal.ColumnCount = 8;
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpTotal.Controls.Add(this.lblPresentes, 0, 0);
            this.tlpTotal.Controls.Add(this.lblNumPresentes, 1, 0);
            this.tlpTotal.Controls.Add(this.lblAusentes, 2, 0);
            this.tlpTotal.Controls.Add(this.lblNumAusentes, 3, 0);
            this.tlpTotal.Controls.Add(this.lblJustificados, 4, 0);
            this.tlpTotal.Controls.Add(this.lblNumJustificados, 5, 0);
            this.tlpTotal.Controls.Add(this.lblTotalAlunos, 6, 0);
            this.tlpTotal.Controls.Add(this.lblNumTotalAlunos, 7, 0);
            this.tlpTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTotal.Location = new System.Drawing.Point(1, 1);
            this.tlpTotal.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTotal.Name = "tlpTotal";
            this.tlpTotal.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.tlpTotal.RowCount = 1;
            this.tlpTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpTotal.Size = new System.Drawing.Size(1024, 56);
            this.tlpTotal.TabIndex = 0;
            // 
            // lblPresentes
            // 
            this.lblPresentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPresentes.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPresentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblPresentes.Location = new System.Drawing.Point(12, 5);
            this.lblPresentes.Margin = new System.Windows.Forms.Padding(0);
            this.lblPresentes.Name = "lblPresentes";
            this.lblPresentes.Size = new System.Drawing.Size(125, 46);
            this.lblPresentes.TabIndex = 0;
            this.lblPresentes.Text = "Presentes:";
            this.lblPresentes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumPresentes
            // 
            this.lblNumPresentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumPresentes.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumPresentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblNumPresentes.Location = new System.Drawing.Point(137, 5);
            this.lblNumPresentes.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumPresentes.Name = "lblNumPresentes";
            this.lblNumPresentes.Size = new System.Drawing.Size(125, 46);
            this.lblNumPresentes.TabIndex = 1;
            this.lblNumPresentes.Text = "0";
            this.lblNumPresentes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAusentes
            // 
            this.lblAusentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAusentes.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAusentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblAusentes.Location = new System.Drawing.Point(262, 5);
            this.lblAusentes.Margin = new System.Windows.Forms.Padding(0);
            this.lblAusentes.Name = "lblAusentes";
            this.lblAusentes.Size = new System.Drawing.Size(125, 46);
            this.lblAusentes.TabIndex = 2;
            this.lblAusentes.Text = "Ausentes:";
            this.lblAusentes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumAusentes
            // 
            this.lblNumAusentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumAusentes.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumAusentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblNumAusentes.Location = new System.Drawing.Point(387, 5);
            this.lblNumAusentes.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumAusentes.Name = "lblNumAusentes";
            this.lblNumAusentes.Size = new System.Drawing.Size(125, 46);
            this.lblNumAusentes.TabIndex = 3;
            this.lblNumAusentes.Text = "0";
            this.lblNumAusentes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblJustificados
            // 
            this.lblJustificados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblJustificados.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJustificados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblJustificados.Location = new System.Drawing.Point(512, 5);
            this.lblJustificados.Margin = new System.Windows.Forms.Padding(0);
            this.lblJustificados.Name = "lblJustificados";
            this.lblJustificados.Size = new System.Drawing.Size(125, 46);
            this.lblJustificados.TabIndex = 4;
            this.lblJustificados.Text = "Justificados:";
            this.lblJustificados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumJustificados
            // 
            this.lblNumJustificados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumJustificados.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumJustificados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblNumJustificados.Location = new System.Drawing.Point(637, 5);
            this.lblNumJustificados.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumJustificados.Name = "lblNumJustificados";
            this.lblNumJustificados.Size = new System.Drawing.Size(125, 46);
            this.lblNumJustificados.TabIndex = 5;
            this.lblNumJustificados.Text = "0";
            this.lblNumJustificados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalAlunos
            // 
            this.lblTotalAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalAlunos.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAlunos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblTotalAlunos.Location = new System.Drawing.Point(762, 5);
            this.lblTotalAlunos.Margin = new System.Windows.Forms.Padding(0);
            this.lblTotalAlunos.Name = "lblTotalAlunos";
            this.lblTotalAlunos.Size = new System.Drawing.Size(125, 46);
            this.lblTotalAlunos.TabIndex = 6;
            this.lblTotalAlunos.Text = "Total de Alunos:";
            this.lblTotalAlunos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumTotalAlunos
            // 
            this.lblNumTotalAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumTotalAlunos.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumTotalAlunos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblNumTotalAlunos.Location = new System.Drawing.Point(887, 5);
            this.lblNumTotalAlunos.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumTotalAlunos.Name = "lblNumTotalAlunos";
            this.lblNumTotalAlunos.Size = new System.Drawing.Size(125, 46);
            this.lblNumTotalAlunos.TabIndex = 7;
            this.lblNumTotalAlunos.Text = "0";
            this.lblNumTotalAlunos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PresencaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.pnlPresenca);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PresencaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Presença";
            this.pnlPresenca.ResumeLayout(false);
            this.tlpPresenca.ResumeLayout(false);
            this.pnlCabecalho.ResumeLayout(false);
            this.pnlCabecalho.PerformLayout();
            this.pnlFiltros.ResumeLayout(false);
            this.tlpFiltros.ResumeLayout(false);
            this.pnlDataAula.ResumeLayout(false);
            this.pnlDataAula.PerformLayout();
            this.pnlTurma.ResumeLayout(false);
            this.pnlTurma.PerformLayout();
            this.pnlTabela.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdvPresenca)).EndInit();
            this.pnlTotal.ResumeLayout(false);
            this.tlpTotal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlPresenca;
        private System.Windows.Forms.TableLayoutPanel tlpPresenca;
        private System.Windows.Forms.Panel pnlCabecalho;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.TableLayoutPanel tlpFiltros;
        private System.Windows.Forms.Label lblDataAula;
        private System.Windows.Forms.Label lblTurma;
        private System.Windows.Forms.Panel pnlDataAula;
        private System.Windows.Forms.TextBox txtDataAula;
        private System.Windows.Forms.Button btnCalendario;
        private System.Windows.Forms.Panel pnlTurma;
        private System.Windows.Forms.ComboBox cmbTurma;
        private System.Windows.Forms.Panel pnlTabela;
        private System.Windows.Forms.DataGridView gdvPresenca;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAluno;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReferencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAcoes;
        private System.Windows.Forms.Panel pnlTotal;
        private System.Windows.Forms.TableLayoutPanel tlpTotal;
        private System.Windows.Forms.Label lblPresentes;
        private System.Windows.Forms.Label lblNumPresentes;
        private System.Windows.Forms.Label lblNumTotalAlunos;
        private System.Windows.Forms.Label lblTotalAlunos;
        private System.Windows.Forms.Label lblNumJustificados;
        private System.Windows.Forms.Label lblJustificados;
        private System.Windows.Forms.Label lblNumAusentes;
        private System.Windows.Forms.Label lblAusentes;
    }
}