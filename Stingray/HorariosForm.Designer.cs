namespace Stingray {
    partial class HorariosForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHorarios = new System.Windows.Forms.Panel();
            this.tblHorarios = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCabecalhoH = new System.Windows.Forms.Panel();
            this.lblTituloH = new System.Windows.Forms.Label();
            this.pnlFiltrosH = new System.Windows.Forms.Panel();
            this.tblFiltrosH = new System.Windows.Forms.TableLayoutPanel();
            this.lblModalidade = new System.Windows.Forms.Label();
            this.lblDiaSemana = new System.Windows.Forms.Label();
            this.pnlModalidade = new System.Windows.Forms.Panel();
            this.txtModalidade = new System.Windows.Forms.TextBox();
            this.pnlDiaSemana = new System.Windows.Forms.Panel();
            this.btnAdicionar = new Stingray.RoundedButton();
            this.pnlTabelaH = new System.Windows.Forms.Panel();
            this.dgvHorarios = new System.Windows.Forms.DataGridView();
            this.colDiaSemana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModalidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHorario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfessor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlunos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbDiaSemana = new System.Windows.Forms.ComboBox();
            this.pnlHorarios.SuspendLayout();
            this.tblHorarios.SuspendLayout();
            this.pnlCabecalhoH.SuspendLayout();
            this.pnlFiltrosH.SuspendLayout();
            this.tblFiltrosH.SuspendLayout();
            this.pnlModalidade.SuspendLayout();
            this.pnlDiaSemana.SuspendLayout();
            this.pnlTabelaH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHorarios)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHorarios
            // 
            this.pnlHorarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlHorarios.Controls.Add(this.tblHorarios);
            this.pnlHorarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHorarios.Location = new System.Drawing.Point(0, 0);
            this.pnlHorarios.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHorarios.Name = "pnlHorarios";
            this.pnlHorarios.Size = new System.Drawing.Size(1050, 660);
            this.pnlHorarios.TabIndex = 2;
            // 
            // tblHorarios
            // 
            this.tblHorarios.ColumnCount = 1;
            this.tblHorarios.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHorarios.Controls.Add(this.pnlCabecalhoH, 0, 0);
            this.tblHorarios.Controls.Add(this.pnlFiltrosH, 0, 1);
            this.tblHorarios.Controls.Add(this.pnlTabelaH, 0, 2);
            this.tblHorarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblHorarios.Location = new System.Drawing.Point(0, 0);
            this.tblHorarios.Margin = new System.Windows.Forms.Padding(0);
            this.tblHorarios.Name = "tblHorarios";
            this.tblHorarios.RowCount = 3;
            this.tblHorarios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tblHorarios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tblHorarios.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHorarios.Size = new System.Drawing.Size(1050, 660);
            this.tblHorarios.TabIndex = 0;
            // 
            // pnlCabecalhoH
            // 
            this.pnlCabecalhoH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlCabecalhoH.Controls.Add(this.lblTituloH);
            this.pnlCabecalhoH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCabecalhoH.Location = new System.Drawing.Point(0, 0);
            this.pnlCabecalhoH.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCabecalhoH.Name = "pnlCabecalhoH";
            this.pnlCabecalhoH.Size = new System.Drawing.Size(1050, 64);
            this.pnlCabecalhoH.TabIndex = 0;
            // 
            // lblTituloH
            // 
            this.lblTituloH.AutoSize = true;
            this.lblTituloH.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTituloH.Location = new System.Drawing.Point(13, 17);
            this.lblTituloH.Name = "lblTituloH";
            this.lblTituloH.Size = new System.Drawing.Size(90, 25);
            this.lblTituloH.TabIndex = 0;
            this.lblTituloH.Text = "Horários";
            // 
            // pnlFiltrosH
            // 
            this.pnlFiltrosH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlFiltrosH.Controls.Add(this.tblFiltrosH);
            this.pnlFiltrosH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltrosH.Location = new System.Drawing.Point(12, 66);
            this.pnlFiltrosH.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.pnlFiltrosH.Name = "pnlFiltrosH";
            this.pnlFiltrosH.Padding = new System.Windows.Forms.Padding(1);
            this.pnlFiltrosH.Size = new System.Drawing.Size(1026, 58);
            this.pnlFiltrosH.TabIndex = 1;
            // 
            // tblFiltrosH
            // 
            this.tblFiltrosH.BackColor = System.Drawing.Color.White;
            this.tblFiltrosH.ColumnCount = 5;
            this.tblFiltrosH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblFiltrosH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tblFiltrosH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tblFiltrosH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tblFiltrosH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblFiltrosH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblFiltrosH.Controls.Add(this.lblModalidade, 0, 0);
            this.tblFiltrosH.Controls.Add(this.lblDiaSemana, 1, 0);
            this.tblFiltrosH.Controls.Add(this.pnlModalidade, 0, 1);
            this.tblFiltrosH.Controls.Add(this.pnlDiaSemana, 1, 1);
            this.tblFiltrosH.Controls.Add(this.btnAdicionar, 4, 1);
            this.tblFiltrosH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFiltrosH.Location = new System.Drawing.Point(1, 1);
            this.tblFiltrosH.Margin = new System.Windows.Forms.Padding(0);
            this.tblFiltrosH.Name = "tblFiltrosH";
            this.tblFiltrosH.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.tblFiltrosH.RowCount = 2;
            this.tblFiltrosH.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tblFiltrosH.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblFiltrosH.Size = new System.Drawing.Size(1024, 56);
            this.tblFiltrosH.TabIndex = 0;
            // 
            // lblModalidade
            // 
            this.lblModalidade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModalidade.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblModalidade.Location = new System.Drawing.Point(12, 5);
            this.lblModalidade.Margin = new System.Windows.Forms.Padding(0);
            this.lblModalidade.Name = "lblModalidade";
            this.lblModalidade.Size = new System.Drawing.Size(333, 18);
            this.lblModalidade.TabIndex = 0;
            this.lblModalidade.Text = "Modalidades";
            this.lblModalidade.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiaSemana
            // 
            this.lblDiaSemana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiaSemana.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiaSemana.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblDiaSemana.Location = new System.Drawing.Point(345, 5);
            this.lblDiaSemana.Margin = new System.Windows.Forms.Padding(0);
            this.lblDiaSemana.Name = "lblDiaSemana";
            this.lblDiaSemana.Size = new System.Drawing.Size(177, 18);
            this.lblDiaSemana.TabIndex = 1;
            this.lblDiaSemana.Text = "Dia da Semana";
            this.lblDiaSemana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlModalidade
            // 
            this.pnlModalidade.BackColor = System.Drawing.Color.White;
            this.pnlModalidade.Controls.Add(this.txtModalidade);
            this.pnlModalidade.Location = new System.Drawing.Point(12, 25);
            this.pnlModalidade.Margin = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.pnlModalidade.Name = "pnlModalidade";
            this.pnlModalidade.Size = new System.Drawing.Size(327, 24);
            this.pnlModalidade.TabIndex = 0;
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
            this.pnlDiaSemana.Controls.Add(this.cmbDiaSemana);
            this.pnlDiaSemana.Location = new System.Drawing.Point(349, 25);
            this.pnlDiaSemana.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.pnlDiaSemana.Name = "pnlDiaSemana";
            this.pnlDiaSemana.Size = new System.Drawing.Size(168, 24);
            this.pnlDiaSemana.TabIndex = 1;
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
            this.btnAdicionar.Location = new System.Drawing.Point(902, 25);
            this.btnAdicionar.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(105, 24);
            this.btnAdicionar.TabIndex = 4;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            // 
            // pnlTabelaH
            // 
            this.pnlTabelaH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTabelaH.Controls.Add(this.dgvHorarios);
            this.pnlTabelaH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabelaH.Location = new System.Drawing.Point(12, 132);
            this.pnlTabelaH.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlTabelaH.Name = "pnlTabelaH";
            this.pnlTabelaH.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTabelaH.Size = new System.Drawing.Size(1026, 516);
            this.pnlTabelaH.TabIndex = 2;
            // 
            // dgvHorarios
            // 
            this.dgvHorarios.AllowUserToAddRows = false;
            this.dgvHorarios.AllowUserToDeleteRows = false;
            this.dgvHorarios.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvHorarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvHorarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHorarios.BackgroundColor = System.Drawing.Color.White;
            this.dgvHorarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHorarios.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvHorarios.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHorarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHorarios.ColumnHeadersHeight = 38;
            this.dgvHorarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHorarios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDiaSemana,
            this.colModalidade,
            this.colHorario,
            this.colProfessor,
            this.colAlunos});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHorarios.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvHorarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHorarios.EnableHeadersVisualStyles = false;
            this.dgvHorarios.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvHorarios.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(245)))));
            this.dgvHorarios.Location = new System.Drawing.Point(1, 1);
            this.dgvHorarios.Margin = new System.Windows.Forms.Padding(0);
            this.dgvHorarios.MultiSelect = false;
            this.dgvHorarios.Name = "dgvHorarios";
            this.dgvHorarios.ReadOnly = true;
            this.dgvHorarios.RowHeadersVisible = false;
            this.dgvHorarios.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvHorarios.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvHorarios.RowTemplate.Height = 38;
            this.dgvHorarios.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvHorarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHorarios.Size = new System.Drawing.Size(1024, 514);
            this.dgvHorarios.TabIndex = 0;
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
            this.cmbDiaSemana.Size = new System.Drawing.Size(149, 21);
            this.cmbDiaSemana.TabIndex = 3;
            // 
            // HorariosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.pnlHorarios);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HorariosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Horários";
            this.pnlHorarios.ResumeLayout(false);
            this.tblHorarios.ResumeLayout(false);
            this.pnlCabecalhoH.ResumeLayout(false);
            this.pnlCabecalhoH.PerformLayout();
            this.pnlFiltrosH.ResumeLayout(false);
            this.tblFiltrosH.ResumeLayout(false);
            this.pnlModalidade.ResumeLayout(false);
            this.pnlModalidade.PerformLayout();
            this.pnlDiaSemana.ResumeLayout(false);
            this.pnlTabelaH.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHorarios)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlHorarios;
        private System.Windows.Forms.TableLayoutPanel tblHorarios;
        private System.Windows.Forms.Panel pnlCabecalhoH;
        private System.Windows.Forms.Label lblTituloH;
        private System.Windows.Forms.Panel pnlFiltrosH;
        private System.Windows.Forms.TableLayoutPanel tblFiltrosH;
        private System.Windows.Forms.Label lblModalidade;
        private System.Windows.Forms.Label lblDiaSemana;
        private System.Windows.Forms.Panel pnlModalidade;
        private System.Windows.Forms.TextBox txtModalidade;
        private System.Windows.Forms.Panel pnlDiaSemana;
        private RoundedButton btnAdicionar;
        private System.Windows.Forms.Panel pnlTabelaH;
        private System.Windows.Forms.DataGridView dgvHorarios;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaSemana;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModalidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHorario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfessor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlunos;
        private System.Windows.Forms.ComboBox cmbDiaSemana;
    }
}