namespace Stingray {
    partial class ProfessoresForm {
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
            this.pnlProfessores = new System.Windows.Forms.Panel();
            this.tblProfessores = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCabecalho = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.tblFiltros = new System.Windows.Forms.TableLayoutPanel();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblCref = new System.Windows.Forms.Label();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblDataNascimento = new System.Windows.Forms.Label();
            this.pnlNome = new System.Windows.Forms.Panel();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.pnlCref = new System.Windows.Forms.Panel();
            this.txtCref = new System.Windows.Forms.TextBox();
            this.pnlTelefone = new System.Windows.Forms.Panel();
            this.txtTelefone = new System.Windows.Forms.TextBox();
            this.pnlDataNascimento = new System.Windows.Forms.Panel();
            this.txtDataNascimento = new System.Windows.Forms.TextBox();
            this.btnAdicionar = new Stingray.RoundedButton();
            this.pnlTabela = new System.Windows.Forms.Panel();
            this.dgvProfessores = new System.Windows.Forms.DataGridView();
            this.colNome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCref = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTelefone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataNascimento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTurmas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlProfessores.SuspendLayout();
            this.tblProfessores.SuspendLayout();
            this.pnlCabecalho.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.tblFiltros.SuspendLayout();
            this.pnlNome.SuspendLayout();
            this.pnlCref.SuspendLayout();
            this.pnlTelefone.SuspendLayout();
            this.pnlDataNascimento.SuspendLayout();
            this.pnlTabela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfessores)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlProfessores
            // 
            this.pnlProfessores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlProfessores.Controls.Add(this.tblProfessores);
            this.pnlProfessores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProfessores.Location = new System.Drawing.Point(0, 0);
            this.pnlProfessores.Margin = new System.Windows.Forms.Padding(0);
            this.pnlProfessores.Name = "pnlProfessores";
            this.pnlProfessores.Size = new System.Drawing.Size(1050, 660);
            this.pnlProfessores.TabIndex = 1;
            // 
            // tblProfessores
            // 
            this.tblProfessores.ColumnCount = 1;
            this.tblProfessores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProfessores.Controls.Add(this.pnlCabecalho, 0, 0);
            this.tblProfessores.Controls.Add(this.pnlFiltros, 0, 1);
            this.tblProfessores.Controls.Add(this.pnlTabela, 0, 2);
            this.tblProfessores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProfessores.Location = new System.Drawing.Point(0, 0);
            this.tblProfessores.Margin = new System.Windows.Forms.Padding(0);
            this.tblProfessores.Name = "tblProfessores";
            this.tblProfessores.RowCount = 3;
            this.tblProfessores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tblProfessores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tblProfessores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProfessores.Size = new System.Drawing.Size(1050, 660);
            this.tblProfessores.TabIndex = 0;
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
            this.lblTitulo.Size = new System.Drawing.Size(115, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Professores";
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlFiltros.Controls.Add(this.tblFiltros);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltros.Location = new System.Drawing.Point(12, 66);
            this.pnlFiltros.Margin = new System.Windows.Forms.Padding(12, 2, 12, 8);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Padding = new System.Windows.Forms.Padding(1);
            this.pnlFiltros.Size = new System.Drawing.Size(1026, 58);
            this.pnlFiltros.TabIndex = 1;
            // 
            // tblFiltros
            // 
            this.tblFiltros.BackColor = System.Drawing.Color.White;
            this.tblFiltros.ColumnCount = 5;
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblFiltros.Controls.Add(this.lblNome, 0, 0);
            this.tblFiltros.Controls.Add(this.lblCref, 1, 0);
            this.tblFiltros.Controls.Add(this.lblTelefone, 2, 0);
            this.tblFiltros.Controls.Add(this.lblDataNascimento, 3, 0);
            this.tblFiltros.Controls.Add(this.pnlNome, 0, 1);
            this.tblFiltros.Controls.Add(this.pnlCref, 1, 1);
            this.tblFiltros.Controls.Add(this.pnlTelefone, 2, 1);
            this.tblFiltros.Controls.Add(this.pnlDataNascimento, 3, 1);
            this.tblFiltros.Controls.Add(this.btnAdicionar, 4, 1);
            this.tblFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFiltros.Location = new System.Drawing.Point(1, 1);
            this.tblFiltros.Margin = new System.Windows.Forms.Padding(0);
            this.tblFiltros.Name = "tblFiltros";
            this.tblFiltros.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.tblFiltros.RowCount = 2;
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblFiltros.Size = new System.Drawing.Size(1024, 56);
            this.tblFiltros.TabIndex = 0;
            // 
            // lblNome
            // 
            this.lblNome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblNome.Location = new System.Drawing.Point(12, 5);
            this.lblNome.Margin = new System.Windows.Forms.Padding(0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(333, 18);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Nome:";
            this.lblNome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCref
            // 
            this.lblCref.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCref.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCref.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblCref.Location = new System.Drawing.Point(345, 5);
            this.lblCref.Margin = new System.Windows.Forms.Padding(0);
            this.lblCref.Name = "lblCref";
            this.lblCref.Size = new System.Drawing.Size(177, 18);
            this.lblCref.TabIndex = 1;
            this.lblCref.Text = "CREF:";
            this.lblCref.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTelefone
            // 
            this.lblTelefone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTelefone.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblTelefone.Location = new System.Drawing.Point(522, 5);
            this.lblTelefone.Margin = new System.Windows.Forms.Padding(0);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(188, 18);
            this.lblTelefone.TabIndex = 2;
            this.lblTelefone.Text = "Telefone:";
            this.lblTelefone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDataNascimento
            // 
            this.lblDataNascimento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataNascimento.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataNascimento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblDataNascimento.Location = new System.Drawing.Point(710, 5);
            this.lblDataNascimento.Margin = new System.Windows.Forms.Padding(0);
            this.lblDataNascimento.Name = "lblDataNascimento";
            this.lblDataNascimento.Size = new System.Drawing.Size(188, 18);
            this.lblDataNascimento.TabIndex = 3;
            this.lblDataNascimento.Text = "Data de Nascimento:";
            this.lblDataNascimento.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlNome
            // 
            this.pnlNome.BackColor = System.Drawing.Color.White;
            this.pnlNome.Controls.Add(this.txtNome);
            this.pnlNome.Location = new System.Drawing.Point(12, 25);
            this.pnlNome.Margin = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.pnlNome.Name = "pnlNome";
            this.pnlNome.Size = new System.Drawing.Size(327, 24);
            this.pnlNome.TabIndex = 0;
            // 
            // txtNome
            // 
            this.txtNome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.txtNome.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtNome.Location = new System.Drawing.Point(0, 0);
            this.txtNome.Margin = new System.Windows.Forms.Padding(0);
            this.txtNome.Name = "txtNome";
            this.txtNome.ReadOnly = true;
            this.txtNome.Size = new System.Drawing.Size(327, 15);
            this.txtNome.TabIndex = 0;
            // 
            // pnlCref
            // 
            this.pnlCref.BackColor = System.Drawing.Color.White;
            this.pnlCref.Controls.Add(this.txtCref);
            this.pnlCref.Location = new System.Drawing.Point(349, 25);
            this.pnlCref.Margin = new System.Windows.Forms.Padding(4, 2, 5, 2);
            this.pnlCref.Name = "pnlCref";
            this.pnlCref.Size = new System.Drawing.Size(168, 24);
            this.pnlCref.TabIndex = 1;
            // 
            // txtCref
            // 
            this.txtCref.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.txtCref.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCref.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCref.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCref.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtCref.Location = new System.Drawing.Point(0, 0);
            this.txtCref.Margin = new System.Windows.Forms.Padding(0);
            this.txtCref.Name = "txtCref";
            this.txtCref.Size = new System.Drawing.Size(168, 16);
            this.txtCref.TabIndex = 0;
            // 
            // pnlTelefone
            // 
            this.pnlTelefone.BackColor = System.Drawing.Color.White;
            this.pnlTelefone.Controls.Add(this.txtTelefone);
            this.pnlTelefone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTelefone.Location = new System.Drawing.Point(526, 24);
            this.pnlTelefone.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.pnlTelefone.Name = "pnlTelefone";
            this.pnlTelefone.Size = new System.Drawing.Size(179, 26);
            this.pnlTelefone.TabIndex = 2;
            // 
            // txtTelefone
            // 
            this.txtTelefone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.txtTelefone.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTelefone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTelefone.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtTelefone.Location = new System.Drawing.Point(0, 0);
            this.txtTelefone.Margin = new System.Windows.Forms.Padding(0);
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Size = new System.Drawing.Size(179, 16);
            this.txtTelefone.TabIndex = 1;
            // 
            // pnlDataNascimento
            // 
            this.pnlDataNascimento.BackColor = System.Drawing.Color.White;
            this.pnlDataNascimento.Controls.Add(this.txtDataNascimento);
            this.pnlDataNascimento.Location = new System.Drawing.Point(714, 24);
            this.pnlDataNascimento.Margin = new System.Windows.Forms.Padding(4, 1, 5, 1);
            this.pnlDataNascimento.Name = "pnlDataNascimento";
            this.pnlDataNascimento.Size = new System.Drawing.Size(179, 26);
            this.pnlDataNascimento.TabIndex = 3;
            // 
            // txtDataNascimento
            // 
            this.txtDataNascimento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.txtDataNascimento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataNascimento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataNascimento.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataNascimento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtDataNascimento.Location = new System.Drawing.Point(0, 0);
            this.txtDataNascimento.Margin = new System.Windows.Forms.Padding(0);
            this.txtDataNascimento.Name = "txtDataNascimento";
            this.txtDataNascimento.Size = new System.Drawing.Size(179, 16);
            this.txtDataNascimento.TabIndex = 1;
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
            // pnlTabela
            // 
            this.pnlTabela.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlTabela.Controls.Add(this.dgvProfessores);
            this.pnlTabela.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabela.Location = new System.Drawing.Point(12, 132);
            this.pnlTabela.Margin = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlTabela.Name = "pnlTabela";
            this.pnlTabela.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTabela.Size = new System.Drawing.Size(1026, 516);
            this.pnlTabela.TabIndex = 2;
            // 
            // dgvProfessores
            // 
            this.dgvProfessores.AllowUserToAddRows = false;
            this.dgvProfessores.AllowUserToDeleteRows = false;
            this.dgvProfessores.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvProfessores.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProfessores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProfessores.BackgroundColor = System.Drawing.Color.White;
            this.dgvProfessores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProfessores.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProfessores.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProfessores.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProfessores.ColumnHeadersHeight = 38;
            this.dgvProfessores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProfessores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNome,
            this.colCref,
            this.colTelefone,
            this.colDataNascimento,
            this.colTurmas});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProfessores.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProfessores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProfessores.EnableHeadersVisualStyles = false;
            this.dgvProfessores.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvProfessores.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(245)))));
            this.dgvProfessores.Location = new System.Drawing.Point(1, 1);
            this.dgvProfessores.Margin = new System.Windows.Forms.Padding(0);
            this.dgvProfessores.MultiSelect = false;
            this.dgvProfessores.Name = "dgvProfessores";
            this.dgvProfessores.ReadOnly = true;
            this.dgvProfessores.RowHeadersVisible = false;
            this.dgvProfessores.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvProfessores.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvProfessores.RowTemplate.Height = 38;
            this.dgvProfessores.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvProfessores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfessores.Size = new System.Drawing.Size(1024, 514);
            this.dgvProfessores.TabIndex = 0;
            // 
            // colNome
            // 
            this.colNome.FillWeight = 105F;
            this.colNome.HeaderText = "Nome";
            this.colNome.MinimumWidth = 45;
            this.colNome.Name = "colNome";
            this.colNome.ReadOnly = true;
            this.colNome.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNome.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCref
            // 
            this.colCref.FillWeight = 110F;
            this.colCref.HeaderText = "CREF";
            this.colCref.MinimumWidth = 45;
            this.colCref.Name = "colCref";
            this.colCref.ReadOnly = true;
            this.colCref.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colCref.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTelefone
            // 
            this.colTelefone.FillWeight = 90F;
            this.colTelefone.HeaderText = "Telefone";
            this.colTelefone.MinimumWidth = 45;
            this.colTelefone.Name = "colTelefone";
            this.colTelefone.ReadOnly = true;
            this.colTelefone.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTelefone.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDataNascimento
            // 
            this.colDataNascimento.FillWeight = 115F;
            this.colDataNascimento.HeaderText = "Data de Nascimento";
            this.colDataNascimento.MinimumWidth = 45;
            this.colDataNascimento.Name = "colDataNascimento";
            this.colDataNascimento.ReadOnly = true;
            this.colDataNascimento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDataNascimento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTurmas
            // 
            this.colTurmas.FillWeight = 90F;
            this.colTurmas.HeaderText = "Turmas";
            this.colTurmas.MinimumWidth = 45;
            this.colTurmas.Name = "colTurmas";
            this.colTurmas.ReadOnly = true;
            this.colTurmas.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colTurmas.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProfessoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.pnlProfessores);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProfessoresForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Professores";
            this.pnlProfessores.ResumeLayout(false);
            this.tblProfessores.ResumeLayout(false);
            this.pnlCabecalho.ResumeLayout(false);
            this.pnlCabecalho.PerformLayout();
            this.pnlFiltros.ResumeLayout(false);
            this.tblFiltros.ResumeLayout(false);
            this.pnlNome.ResumeLayout(false);
            this.pnlNome.PerformLayout();
            this.pnlCref.ResumeLayout(false);
            this.pnlCref.PerformLayout();
            this.pnlTelefone.ResumeLayout(false);
            this.pnlTelefone.PerformLayout();
            this.pnlDataNascimento.ResumeLayout(false);
            this.pnlDataNascimento.PerformLayout();
            this.pnlTabela.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfessores)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlProfessores;
        private System.Windows.Forms.TableLayoutPanel tblProfessores;
        private System.Windows.Forms.Panel pnlTabela;
        private System.Windows.Forms.DataGridView dgvProfessores;
        private System.Windows.Forms.Panel pnlCabecalho;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.TableLayoutPanel tblFiltros;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label lblCref;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.Label lblDataNascimento;
        private System.Windows.Forms.Panel pnlNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Panel pnlCref;
        private System.Windows.Forms.TextBox txtCref;
        private System.Windows.Forms.Panel pnlTelefone;
        private System.Windows.Forms.TextBox txtTelefone;
        private System.Windows.Forms.Panel pnlDataNascimento;
        private System.Windows.Forms.TextBox txtDataNascimento;
        private RoundedButton btnAdicionar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNome;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCref;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTelefone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataNascimento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTurmas;
    }
}