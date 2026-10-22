namespace Stingray {
    partial class TurmasForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTurmas = new System.Windows.Forms.Panel();
            this.tblTurmas = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCabecalhoTurmas = new System.Windows.Forms.Panel();
            this.lblTituloTurmas = new System.Windows.Forms.Label();
            this.pnlFiltrosTurmas = new System.Windows.Forms.Panel();
            this.tblFiltrosTurmas = new System.Windows.Forms.TableLayoutPanel();
            this.lblModalidade = new System.Windows.Forms.Label();
            this.lblDiaSemana = new System.Windows.Forms.Label();
            this.pnlModalidade = new System.Windows.Forms.Panel();
            this.pnlModalidadeSuperficie = new System.Windows.Forms.Panel();
            this.txtModalidade = new System.Windows.Forms.TextBox();
            this.pnlDiaSemana = new System.Windows.Forms.Panel();
            this.pnlDiaSemanaSuperficie = new System.Windows.Forms.Panel();
            this.cmbDiaSemana = new System.Windows.Forms.ComboBox();
            this.btnSetaDiaSemana = new System.Windows.Forms.Button();
            this.btnLimparFiltros = new Stingray.RoundedButton();
            this.btnAdicionar = new Stingray.RoundedButton();
            this.pnlTabelaTurmas = new System.Windows.Forms.Panel();
            this.dgvTurmas = new System.Windows.Forms.DataGridView();
            this.colDiaSemana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModalidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHorario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfessor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlunos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAcoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTurmas.SuspendLayout();
            this.tblTurmas.SuspendLayout();
            this.pnlCabecalhoTurmas.SuspendLayout();
            this.pnlFiltrosTurmas.SuspendLayout();
            this.tblFiltrosTurmas.SuspendLayout();
            this.pnlModalidade.SuspendLayout();
            this.pnlModalidadeSuperficie.SuspendLayout();
            this.pnlDiaSemana.SuspendLayout();
            this.pnlDiaSemanaSuperficie.SuspendLayout();
            this.pnlTabelaTurmas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurmas)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTurmas
            // 
            this.pnlTurmas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlTurmas.Controls.Add(this.tblTurmas);
            this.pnlTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTurmas.Location = new System.Drawing.Point(0, 0);
            this.pnlTurmas.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTurmas.Name = "pnlTurmas";
            this.pnlTurmas.Size = new System.Drawing.Size(1050, 660);
            this.pnlTurmas.TabIndex = 2;
            // 
            // tblTurmas
            // 
            this.tblTurmas.ColumnCount = 1;
            this.tblTurmas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTurmas.Controls.Add(this.pnlCabecalhoTurmas, 0, 0);
            this.tblTurmas.Controls.Add(this.pnlFiltrosTurmas, 0, 1);
            this.tblTurmas.Controls.Add(this.pnlTabelaTurmas, 0, 2);
            this.tblTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTurmas.Location = new System.Drawing.Point(0, 0);
            this.tblTurmas.Margin = new System.Windows.Forms.Padding(0);
            this.tblTurmas.Name = "tblTurmas";
            this.tblTurmas.RowCount = 3;
            this.tblTurmas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tblTurmas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tblTurmas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTurmas.Size = new System.Drawing.Size(1050, 660);
            this.tblTurmas.TabIndex = 0;
            // 
            // pnlCabecalhoTurmas
            // 
            this.pnlCabecalhoTurmas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlCabecalhoTurmas.Controls.Add(this.lblTituloTurmas);
            this.pnlCabecalhoTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCabecalhoTurmas.Location = new System.Drawing.Point(0, 0);
            this.pnlCabecalhoTurmas.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCabecalhoTurmas.Name = "pnlCabecalhoTurmas";
            this.pnlCabecalhoTurmas.Size = new System.Drawing.Size(1050, 64);
            this.pnlCabecalhoTurmas.TabIndex = 0;
            // 
            // lblTituloTurmas
            // 
            this.lblTituloTurmas.AutoSize = true;
            this.lblTituloTurmas.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloTurmas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTituloTurmas.Location = new System.Drawing.Point(13, 17);
            this.lblTituloTurmas.Name = "lblTituloTurmas";
            this.lblTituloTurmas.Size = new System.Drawing.Size(77, 25);
            this.lblTituloTurmas.TabIndex = 0;
            this.lblTituloTurmas.Text = "Turmas";
            // 
            // pnlFiltrosTurmas
            // 
            this.pnlFiltrosTurmas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlFiltrosTurmas.Controls.Add(this.tblFiltrosTurmas);
            this.pnlFiltrosTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltrosTurmas.Location = new System.Drawing.Point(12, 66);
            this.pnlFiltrosTurmas.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.pnlFiltrosTurmas.Name = "pnlFiltrosTurmas";
            this.pnlFiltrosTurmas.Padding = new System.Windows.Forms.Padding(1);
            this.pnlFiltrosTurmas.Size = new System.Drawing.Size(1026, 58);
            this.pnlFiltrosTurmas.TabIndex = 1;
            // 
            // tblFiltrosTurmas
            // 
            this.tblFiltrosTurmas.BackColor = System.Drawing.Color.White;
            this.tblFiltrosTurmas.ColumnCount = 5;
            this.tblFiltrosTurmas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tblFiltrosTurmas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblFiltrosTurmas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.tblFiltrosTurmas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tblFiltrosTurmas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tblFiltrosTurmas.Controls.Add(this.lblModalidade, 0, 0);
            this.tblFiltrosTurmas.Controls.Add(this.lblDiaSemana, 1, 0);
            this.tblFiltrosTurmas.Controls.Add(this.pnlModalidade, 0, 1);
            this.tblFiltrosTurmas.Controls.Add(this.pnlDiaSemana, 1, 1);
            this.tblFiltrosTurmas.Controls.Add(this.btnLimparFiltros, 3, 1);
            this.tblFiltrosTurmas.Controls.Add(this.btnAdicionar, 4, 1);
            this.tblFiltrosTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFiltrosTurmas.Location = new System.Drawing.Point(1, 1);
            this.tblFiltrosTurmas.Margin = new System.Windows.Forms.Padding(0);
            this.tblFiltrosTurmas.Name = "tblFiltrosTurmas";
            this.tblFiltrosTurmas.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.tblFiltrosTurmas.RowCount = 2;
            this.tblFiltrosTurmas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tblFiltrosTurmas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblFiltrosTurmas.Size = new System.Drawing.Size(1024, 56);
            this.tblFiltrosTurmas.TabIndex = 0;
            // 
            // lblModalidade
            // 
            this.lblModalidade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModalidade.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblModalidade.Location = new System.Drawing.Point(12, 5);
            this.lblModalidade.Margin = new System.Windows.Forms.Padding(0);
            this.lblModalidade.Name = "lblModalidade";
            this.lblModalidade.Size = new System.Drawing.Size(350, 18);
            this.lblModalidade.TabIndex = 0;
            this.lblModalidade.Text = "Modalidades";
            this.lblModalidade.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiaSemana
            // 
            this.lblDiaSemana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiaSemana.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiaSemana.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblDiaSemana.Location = new System.Drawing.Point(362, 5);
            this.lblDiaSemana.Margin = new System.Windows.Forms.Padding(0);
            this.lblDiaSemana.Name = "lblDiaSemana";
            this.lblDiaSemana.Size = new System.Drawing.Size(200, 18);
            this.lblDiaSemana.TabIndex = 1;
            this.lblDiaSemana.Text = "Dia da Semana";
            this.lblDiaSemana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlModalidade
            // 
            this.pnlModalidade.BackColor = System.Drawing.Color.White;
            this.pnlModalidade.Controls.Add(this.pnlModalidadeSuperficie);
            this.pnlModalidade.Location = new System.Drawing.Point(12, 25);
            this.pnlModalidade.Margin = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.pnlModalidade.Name = "pnlModalidade";
            this.pnlModalidade.Size = new System.Drawing.Size(327, 24);
            this.pnlModalidade.TabIndex = 0;
            // 
            // pnlModalidadeSuperficie
            // 
            this.pnlModalidadeSuperficie.BackColor = System.Drawing.Color.White;
            this.pnlModalidadeSuperficie.Controls.Add(this.txtModalidade);
            this.pnlModalidadeSuperficie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModalidadeSuperficie.Location = new System.Drawing.Point(0, 0);
            this.pnlModalidadeSuperficie.Margin = new System.Windows.Forms.Padding(0);
            this.pnlModalidadeSuperficie.Name = "pnlModalidadeSuperficie";
            this.pnlModalidadeSuperficie.Size = new System.Drawing.Size(327, 24);
            this.pnlModalidadeSuperficie.TabIndex = 0;
            // 
            // txtModalidade
            // 
            this.txtModalidade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.txtModalidade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtModalidade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtModalidade.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModalidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtModalidade.Location = new System.Drawing.Point(0, 0);
            this.txtModalidade.Margin = new System.Windows.Forms.Padding(0);
            this.txtModalidade.Name = "txtModalidade";
            this.txtModalidade.ReadOnly = true;
            this.txtModalidade.Size = new System.Drawing.Size(327, 15);
            this.txtModalidade.TabIndex = 0;
            // 
            // pnlDiaSemana
            // 
            this.pnlDiaSemana.BackColor = System.Drawing.Color.White;
            this.pnlDiaSemana.Controls.Add(this.pnlDiaSemanaSuperficie);
            this.pnlDiaSemana.Location = new System.Drawing.Point(366, 25);
            this.pnlDiaSemana.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.pnlDiaSemana.Name = "pnlDiaSemana";
            this.pnlDiaSemana.Size = new System.Drawing.Size(168, 24);
            this.pnlDiaSemana.TabIndex = 1;
            // 
            // pnlDiaSemanaSuperficie
            // 
            this.pnlDiaSemanaSuperficie.BackColor = System.Drawing.Color.White;
            this.pnlDiaSemanaSuperficie.Controls.Add(this.cmbDiaSemana);
            this.pnlDiaSemanaSuperficie.Controls.Add(this.btnSetaDiaSemana);
            this.pnlDiaSemanaSuperficie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDiaSemanaSuperficie.Location = new System.Drawing.Point(0, 0);
            this.pnlDiaSemanaSuperficie.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDiaSemanaSuperficie.Name = "pnlDiaSemanaSuperficie";
            this.pnlDiaSemanaSuperficie.Size = new System.Drawing.Size(168, 24);
            this.pnlDiaSemanaSuperficie.TabIndex = 0;
            // 
            // cmbDiaSemana
            // 
            this.cmbDiaSemana.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDiaSemana.BackColor = System.Drawing.Color.White;
            this.cmbDiaSemana.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiaSemana.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDiaSemana.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDiaSemana.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.cmbDiaSemana.FormattingEnabled = true;
            this.cmbDiaSemana.Items.AddRange(new object[] {
            "Todas",
            "Hidroginástica",
            "Natação Iniciante",
            "Natação Intermediário",
            "Natação Avançado",
            "Natação Adulto",
            "Natação Master",
            "Natação Competição"});
            this.cmbDiaSemana.Location = new System.Drawing.Point(10, 2);
            this.cmbDiaSemana.Margin = new System.Windows.Forms.Padding(0);
            this.cmbDiaSemana.Name = "cmbDiaSemana";
            this.cmbDiaSemana.Size = new System.Drawing.Size(117, 21);
            this.cmbDiaSemana.TabIndex = 3;
            // 
            // btnSetaDiaSemana
            // 
            this.btnSetaDiaSemana.AccessibleDescription = "Abre a lista de dias da semana";
            this.btnSetaDiaSemana.AccessibleName = "Abrir opções de dia da semana";
            this.btnSetaDiaSemana.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetaDiaSemana.BackColor = System.Drawing.Color.White;
            this.btnSetaDiaSemana.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetaDiaSemana.FlatAppearance.BorderSize = 0;
            this.btnSetaDiaSemana.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.btnSetaDiaSemana.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnSetaDiaSemana.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetaDiaSemana.Location = new System.Drawing.Point(143, 0);
            this.btnSetaDiaSemana.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetaDiaSemana.Name = "btnSetaDiaSemana";
            this.btnSetaDiaSemana.Size = new System.Drawing.Size(26, 26);
            this.btnSetaDiaSemana.TabIndex = 1;
            this.btnSetaDiaSemana.UseVisualStyleBackColor = false;
            // 
            // btnLimparFiltros
            // 
            this.btnLimparFiltros.BackColor = System.Drawing.Color.White;
            this.btnLimparFiltros.CornerRadius = 5;
            this.btnLimparFiltros.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimparFiltros.FlatAppearance.BorderSize = 0;
            this.btnLimparFiltros.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnLimparFiltros.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnLimparFiltros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimparFiltros.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnLimparFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.btnLimparFiltros.Location = new System.Drawing.Point(760, 23);
            this.btnLimparFiltros.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnLimparFiltros.Name = "btnLimparFiltros";
            this.btnLimparFiltros.Size = new System.Drawing.Size(100, 28);
            this.btnLimparFiltros.TabIndex = 3;
            this.btnLimparFiltros.Text = "Limpar";
            this.btnLimparFiltros.UseVisualStyleBackColor = false;
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.Color.White;
            this.btnAdicionar.CornerRadius = 0;
            this.btnAdicionar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdicionar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdicionar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnAdicionar.FlatAppearance.BorderSize = 0;
            this.btnAdicionar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.btnAdicionar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(86)))), ((int)(((byte)(196)))));
            this.btnAdicionar.Location = new System.Drawing.Point(876, 25);
            this.btnAdicionar.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(131, 24);
            this.btnAdicionar.TabIndex = 4;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            // 
            // pnlTabelaTurmas
            // 
            this.pnlTabelaTurmas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTabelaTurmas.Controls.Add(this.dgvTurmas);
            this.pnlTabelaTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabelaTurmas.Location = new System.Drawing.Point(12, 132);
            this.pnlTabelaTurmas.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlTabelaTurmas.Name = "pnlTabelaTurmas";
            this.pnlTabelaTurmas.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTabelaTurmas.Size = new System.Drawing.Size(1026, 516);
            this.pnlTabelaTurmas.TabIndex = 2;
            // 
            // dgvTurmas
            // 
            this.dgvTurmas.AllowUserToAddRows = false;
            this.dgvTurmas.AllowUserToDeleteRows = false;
            this.dgvTurmas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvTurmas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTurmas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurmas.BackgroundColor = System.Drawing.Color.White;
            this.dgvTurmas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTurmas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTurmas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTurmas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTurmas.ColumnHeadersHeight = 38;
            this.dgvTurmas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTurmas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDiaSemana,
            this.colModalidade,
            this.colHorario,
            this.colProfessor,
            this.colAlunos,
            this.colAcoes});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTurmas.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTurmas.EnableHeadersVisualStyles = false;
            this.dgvTurmas.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTurmas.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(245)))));
            this.dgvTurmas.Location = new System.Drawing.Point(1, 1);
            this.dgvTurmas.Margin = new System.Windows.Forms.Padding(0);
            this.dgvTurmas.MultiSelect = false;
            this.dgvTurmas.Name = "dgvTurmas";
            this.dgvTurmas.ReadOnly = true;
            this.dgvTurmas.RowHeadersVisible = false;
            this.dgvTurmas.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            this.dgvTurmas.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTurmas.RowTemplate.Height = 38;
            this.dgvTurmas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvTurmas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurmas.Size = new System.Drawing.Size(1024, 514);
            this.dgvTurmas.TabIndex = 0;
            // 
            // colDiaSemana
            // 
            this.colDiaSemana.FillWeight = 105F;
            this.colDiaSemana.HeaderText = "Dia da Semana";
            this.colDiaSemana.MinimumWidth = 45;
            this.colDiaSemana.Name = "colDiaSemana";
            this.colDiaSemana.ReadOnly = true;
            this.colDiaSemana.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDiaSemana.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colModalidade
            // 
            this.colModalidade.FillWeight = 110F;
            this.colModalidade.HeaderText = "Modalidade";
            this.colModalidade.MinimumWidth = 45;
            this.colModalidade.Name = "colModalidade";
            this.colModalidade.ReadOnly = true;
            this.colModalidade.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colModalidade.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colHorario
            // 
            this.colHorario.FillWeight = 90F;
            this.colHorario.HeaderText = "Horário";
            this.colHorario.MinimumWidth = 45;
            this.colHorario.Name = "colHorario";
            this.colHorario.ReadOnly = true;
            this.colHorario.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colHorario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colProfessor
            // 
            this.colProfessor.FillWeight = 115F;
            this.colProfessor.HeaderText = "Professor";
            this.colProfessor.MinimumWidth = 45;
            this.colProfessor.Name = "colProfessor";
            this.colProfessor.ReadOnly = true;
            this.colProfessor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colProfessor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAlunos
            // 
            this.colAlunos.FillWeight = 90F;
            this.colAlunos.HeaderText = "Alunos";
            this.colAlunos.MinimumWidth = 45;
            this.colAlunos.Name = "colAlunos";
            this.colAlunos.ReadOnly = true;
            this.colAlunos.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAlunos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAcoes
            // 
            this.colAcoes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.colAcoes.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAcoes.HeaderText = "Ações";
            this.colAcoes.MinimumWidth = 82;
            this.colAcoes.Name = "colAcoes";
            this.colAcoes.ReadOnly = true;
            this.colAcoes.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAcoes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAcoes.Width = 82;
            // 
            // TurmasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.pnlTurmas);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TurmasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Turmas";
            this.pnlTurmas.ResumeLayout(false);
            this.tblTurmas.ResumeLayout(false);
            this.pnlCabecalhoTurmas.ResumeLayout(false);
            this.pnlCabecalhoTurmas.PerformLayout();
            this.pnlFiltrosTurmas.ResumeLayout(false);
            this.tblFiltrosTurmas.ResumeLayout(false);
            this.pnlModalidade.ResumeLayout(false);
            this.pnlModalidadeSuperficie.ResumeLayout(false);
            this.pnlModalidadeSuperficie.PerformLayout();
            this.pnlDiaSemana.ResumeLayout(false);
            this.pnlDiaSemanaSuperficie.ResumeLayout(false);
            this.pnlTabelaTurmas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurmas)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTurmas;
        private System.Windows.Forms.TableLayoutPanel tblTurmas;
        private System.Windows.Forms.Panel pnlCabecalhoTurmas;
        private System.Windows.Forms.Label lblTituloTurmas;
        private System.Windows.Forms.Panel pnlFiltrosTurmas;
        private System.Windows.Forms.TableLayoutPanel tblFiltrosTurmas;
        private System.Windows.Forms.Label lblModalidade;
        private System.Windows.Forms.Label lblDiaSemana;
        private System.Windows.Forms.Panel pnlModalidade;
        private System.Windows.Forms.Panel pnlModalidadeSuperficie;
        private System.Windows.Forms.TextBox txtModalidade;
        private System.Windows.Forms.Panel pnlDiaSemana;
        private System.Windows.Forms.Panel pnlDiaSemanaSuperficie;
        private System.Windows.Forms.Button btnSetaDiaSemana;
        private RoundedButton btnAdicionar;
        private System.Windows.Forms.Panel pnlTabelaTurmas;
        private System.Windows.Forms.DataGridView dgvTurmas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaSemana;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModalidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHorario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfessor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlunos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAcoes;
        private System.Windows.Forms.ComboBox cmbDiaSemana;
    }
}
