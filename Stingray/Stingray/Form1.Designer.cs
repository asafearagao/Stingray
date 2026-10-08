namespace Stingray
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlConteudo = new System.Windows.Forms.Panel();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblAquafit = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.pnlSair = new System.Windows.Forms.Panel();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnConfiguracoes = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnRelatorios = new System.Windows.Forms.Button();
            this.btnMatriculas = new System.Windows.Forms.Button();
            this.btnPresenca = new System.Windows.Forms.Button();
            this.btnPagamentos = new System.Windows.Forms.Button();
            this.btnAlunos = new System.Windows.Forms.Button();
            this.btnTurmas = new System.Windows.Forms.Button();
            this.btnHorarios = new System.Windows.Forms.Button();
            this.btnProfessores = new System.Windows.Forms.Button();
            this.tblBotoesMenu = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMenu.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlSair.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tblBotoesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConteudo
            // 
            this.pnlConteudo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.pnlConteudo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConteudo.Location = new System.Drawing.Point(0, 0);
            this.pnlConteudo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlConteudo.Name = "pnlConteudo";
            this.pnlConteudo.Size = new System.Drawing.Size(1262, 673);
            this.pnlConteudo.TabIndex = 11;
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.White;
            this.pnlMenu.Controls.Add(this.tblBotoesMenu);
            this.pnlMenu.Controls.Add(this.pnlLogo);
            this.pnlMenu.Controls.Add(this.pnlSair);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(313, 673);
            this.pnlMenu.TabIndex = 12;
            // 
            // pnlLogo
            // 
            this.pnlLogo.Controls.Add(this.lblDescricao);
            this.pnlLogo.Controls.Add(this.lblAquafit);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(313, 160);
            this.pnlLogo.TabIndex = 0;
            // 
            // lblAquafit
            // 
            this.lblAquafit.AutoSize = true;
            this.lblAquafit.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAquafit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(100)))), ((int)(((byte)(215)))));
            this.lblAquafit.Location = new System.Drawing.Point(54, 45);
            this.lblAquafit.Name = "lblAquafit";
            this.lblAquafit.Size = new System.Drawing.Size(191, 62);
            this.lblAquafit.TabIndex = 0;
            this.lblAquafit.Text = "Aquafit";
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricao.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDescricao.Location = new System.Drawing.Point(78, 121);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(150, 23);
            this.lblDescricao.TabIndex = 1;
            this.lblDescricao.Text = "Escola de Natação";
            // 
            // pnlSair
            // 
            this.pnlSair.Controls.Add(this.flowLayoutPanel1);
            this.pnlSair.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSair.Location = new System.Drawing.Point(0, 552);
            this.pnlSair.Name = "pnlSair";
            this.pnlSair.Size = new System.Drawing.Size(313, 121);
            this.pnlSair.TabIndex = 1;
            // 
            // btnSair
            // 
            this.btnSair.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnSair.Location = new System.Drawing.Point(10, 60);
            this.btnSair.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(280, 35);
            this.btnSair.TabIndex = 3;
            this.btnSair.Text = "Sair";
            this.btnSair.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSair.UseVisualStyleBackColor = false;
            // 
            // btnConfiguracoes
            // 
            this.btnConfiguracoes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracoes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfiguracoes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnConfiguracoes.Location = new System.Drawing.Point(10, 15);
            this.btnConfiguracoes.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.Size = new System.Drawing.Size(280, 35);
            this.btnConfiguracoes.TabIndex = 4;
            this.btnConfiguracoes.Text = "Configurações";
            this.btnConfiguracoes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracoes.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnConfiguracoes);
            this.flowLayoutPanel1.Controls.Add(this.btnSair);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 1);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(313, 120);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnDashboard
            // 
            this.btnDashboard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnDashboard.Location = new System.Drawing.Point(5, 48);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(5);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(303, 33);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            // 
            // btnRelatorios
            // 
            this.btnRelatorios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRelatorios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnRelatorios.Location = new System.Drawing.Point(5, 5);
            this.btnRelatorios.Margin = new System.Windows.Forms.Padding(5);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.Size = new System.Drawing.Size(303, 33);
            this.btnRelatorios.TabIndex = 8;
            this.btnRelatorios.Text = "Relatórios";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelatorios.UseVisualStyleBackColor = false;
            // 
            // btnMatriculas
            // 
            this.btnMatriculas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMatriculas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnMatriculas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMatriculas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMatriculas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnMatriculas.Location = new System.Drawing.Point(5, 220);
            this.btnMatriculas.Margin = new System.Windows.Forms.Padding(5);
            this.btnMatriculas.Name = "btnMatriculas";
            this.btnMatriculas.Size = new System.Drawing.Size(303, 33);
            this.btnMatriculas.TabIndex = 5;
            this.btnMatriculas.Text = "Matrículas";
            this.btnMatriculas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMatriculas.UseVisualStyleBackColor = false;
            // 
            // btnPresenca
            // 
            this.btnPresenca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPresenca.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnPresenca.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPresenca.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPresenca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnPresenca.Location = new System.Drawing.Point(5, 177);
            this.btnPresenca.Margin = new System.Windows.Forms.Padding(5);
            this.btnPresenca.Name = "btnPresenca";
            this.btnPresenca.Size = new System.Drawing.Size(303, 33);
            this.btnPresenca.TabIndex = 7;
            this.btnPresenca.Text = "Presença";
            this.btnPresenca.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPresenca.UseVisualStyleBackColor = false;
            // 
            // btnPagamentos
            // 
            this.btnPagamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPagamentos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnPagamentos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamentos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagamentos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnPagamentos.Location = new System.Drawing.Point(5, 263);
            this.btnPagamentos.Margin = new System.Windows.Forms.Padding(5);
            this.btnPagamentos.Name = "btnPagamentos";
            this.btnPagamentos.Size = new System.Drawing.Size(303, 33);
            this.btnPagamentos.TabIndex = 6;
            this.btnPagamentos.Text = "Pagamentos";
            this.btnPagamentos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPagamentos.UseVisualStyleBackColor = false;
            // 
            // btnAlunos
            // 
            this.btnAlunos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlunos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnAlunos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlunos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlunos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnAlunos.Location = new System.Drawing.Point(5, 134);
            this.btnAlunos.Margin = new System.Windows.Forms.Padding(5);
            this.btnAlunos.Name = "btnAlunos";
            this.btnAlunos.Size = new System.Drawing.Size(303, 33);
            this.btnAlunos.TabIndex = 1;
            this.btnAlunos.Text = "Alunos";
            this.btnAlunos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAlunos.UseVisualStyleBackColor = false;
            // 
            // btnTurmas
            // 
            this.btnTurmas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTurmas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnTurmas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurmas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurmas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnTurmas.Location = new System.Drawing.Point(5, 306);
            this.btnTurmas.Margin = new System.Windows.Forms.Padding(5);
            this.btnTurmas.Name = "btnTurmas";
            this.btnTurmas.Size = new System.Drawing.Size(303, 33);
            this.btnTurmas.TabIndex = 3;
            this.btnTurmas.Text = "Turmas";
            this.btnTurmas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTurmas.UseVisualStyleBackColor = false;
            // 
            // btnHorarios
            // 
            this.btnHorarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHorarios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnHorarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHorarios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHorarios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnHorarios.Location = new System.Drawing.Point(5, 91);
            this.btnHorarios.Margin = new System.Windows.Forms.Padding(5);
            this.btnHorarios.Name = "btnHorarios";
            this.btnHorarios.Size = new System.Drawing.Size(303, 33);
            this.btnHorarios.TabIndex = 4;
            this.btnHorarios.Text = "Horários";
            this.btnHorarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHorarios.UseVisualStyleBackColor = false;
            // 
            // btnProfessores
            // 
            this.btnProfessores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProfessores.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(232)))));
            this.btnProfessores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfessores.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfessores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(49)))), ((int)(((byte)(83)))));
            this.btnProfessores.Location = new System.Drawing.Point(5, 349);
            this.btnProfessores.Margin = new System.Windows.Forms.Padding(5);
            this.btnProfessores.Name = "btnProfessores";
            this.btnProfessores.Size = new System.Drawing.Size(303, 38);
            this.btnProfessores.TabIndex = 2;
            this.btnProfessores.Text = "Professores";
            this.btnProfessores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProfessores.UseVisualStyleBackColor = false;
            // 
            // tblBotoesMenu
            // 
            this.tblBotoesMenu.ColumnCount = 1;
            this.tblBotoesMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBotoesMenu.Controls.Add(this.btnProfessores, 0, 8);
            this.tblBotoesMenu.Controls.Add(this.btnHorarios, 0, 2);
            this.tblBotoesMenu.Controls.Add(this.btnTurmas, 0, 7);
            this.tblBotoesMenu.Controls.Add(this.btnAlunos, 0, 3);
            this.tblBotoesMenu.Controls.Add(this.btnPagamentos, 0, 6);
            this.tblBotoesMenu.Controls.Add(this.btnPresenca, 0, 4);
            this.tblBotoesMenu.Controls.Add(this.btnMatriculas, 0, 5);
            this.tblBotoesMenu.Controls.Add(this.btnRelatorios, 0, 0);
            this.tblBotoesMenu.Controls.Add(this.btnDashboard, 0, 1);
            this.tblBotoesMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBotoesMenu.Location = new System.Drawing.Point(0, 160);
            this.tblBotoesMenu.Name = "tblBotoesMenu";
            this.tblBotoesMenu.RowCount = 9;
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblBotoesMenu.Size = new System.Drawing.Size(313, 392);
            this.tblBotoesMenu.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.pnlConteudo);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlMenu.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            this.pnlSair.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tblBotoesMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlConteudo;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Label lblAquafit;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Panel pnlSair;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnConfiguracoes;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.TableLayoutPanel tblBotoesMenu;
        private System.Windows.Forms.Button btnProfessores;
        private System.Windows.Forms.Button btnHorarios;
        private System.Windows.Forms.Button btnTurmas;
        private System.Windows.Forms.Button btnAlunos;
        private System.Windows.Forms.Button btnPagamentos;
        private System.Windows.Forms.Button btnPresenca;
        private System.Windows.Forms.Button btnMatriculas;
        private System.Windows.Forms.Button btnRelatorios;
        private System.Windows.Forms.Button btnDashboard;
    }
}

