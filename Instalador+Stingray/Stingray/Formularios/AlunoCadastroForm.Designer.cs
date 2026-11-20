namespace Stingray
{
    partial class AlunoCadastroForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlCartao;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label lblNascimento;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.Label lblCep;
        private System.Windows.Forms.Label lblComplemento;
        private System.Windows.Forms.Label lblResponsavel;
        private System.Windows.Forms.Label lblCpf;
        private System.Windows.Forms.Label lblRg;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblDiaVencimento;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.TextBox txtDataNascimento;
        private System.Windows.Forms.TextBox txtTelefone;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.Panel pnlCep;
        private System.Windows.Forms.TextBox txtCep;
        private System.Windows.Forms.TextBox txtComplemento;
        private System.Windows.Forms.TextBox txtResponsavel;
        private System.Windows.Forms.TextBox txtCpf;
        private System.Windows.Forms.TextBox txtRg;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.NumericUpDown nudDiaVencimento;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCartao = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblNascimento = new System.Windows.Forms.Label();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblNumero = new System.Windows.Forms.Label();
            this.lblCep = new System.Windows.Forms.Label();
            this.lblComplemento = new System.Windows.Forms.Label();
            this.lblResponsavel = new System.Windows.Forms.Label();
            this.lblCpf = new System.Windows.Forms.Label();
            this.lblRg = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblDiaVencimento = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtDataNascimento = new System.Windows.Forms.TextBox();
            this.txtTelefone = new System.Windows.Forms.TextBox();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.pnlCep = new System.Windows.Forms.Panel();
            this.txtCep = new System.Windows.Forms.TextBox();
            this.txtComplemento = new System.Windows.Forms.TextBox();
            this.txtResponsavel = new System.Windows.Forms.TextBox();
            this.txtCpf = new System.Windows.Forms.TextBox();
            this.txtRg = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.nudDiaVencimento = new System.Windows.Forms.NumericUpDown();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pnlCartao.SuspendLayout();
            this.pnlCep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiaVencimento)).BeginInit();
            this.SuspendLayout();
            //
            // pnlCartao
            //
            this.pnlCartao.BackColor = System.Drawing.Color.White;
            this.pnlCartao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCartao.Controls.Add(this.lblTitulo);
            this.pnlCartao.Controls.Add(this.lblNome);
            this.pnlCartao.Controls.Add(this.lblNascimento);
            this.pnlCartao.Controls.Add(this.lblTelefone);
            this.pnlCartao.Controls.Add(this.lblNumero);
            this.pnlCartao.Controls.Add(this.lblCep);
            this.pnlCartao.Controls.Add(this.lblComplemento);
            this.pnlCartao.Controls.Add(this.lblResponsavel);
            this.pnlCartao.Controls.Add(this.lblCpf);
            this.pnlCartao.Controls.Add(this.lblRg);
            this.pnlCartao.Controls.Add(this.lblEmail);
            this.pnlCartao.Controls.Add(this.lblDiaVencimento);
            this.pnlCartao.Controls.Add(this.txtNome);
            this.pnlCartao.Controls.Add(this.txtDataNascimento);
            this.pnlCartao.Controls.Add(this.txtTelefone);
            this.pnlCartao.Controls.Add(this.txtNumero);
            this.pnlCartao.Controls.Add(this.pnlCep);
            this.pnlCartao.Controls.Add(this.txtComplemento);
            this.pnlCartao.Controls.Add(this.txtResponsavel);
            this.pnlCartao.Controls.Add(this.txtCpf);
            this.pnlCartao.Controls.Add(this.txtRg);
            this.pnlCartao.Controls.Add(this.txtEmail);
            this.pnlCartao.Controls.Add(this.nudDiaVencimento);
            this.pnlCartao.Location = new System.Drawing.Point(16, 16);
            this.pnlCartao.Name = "pnlCartao";
            this.pnlCartao.Size = new System.Drawing.Size(888, 330);
            this.pnlCartao.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblTitulo.Location = new System.Drawing.Point(18, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(93, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Novo Aluno";
            //
            // lblNome
            //
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(18, 42);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(90, 15);
            this.lblNome.TabIndex = 1;
            this.lblNome.Text = "Nome do aluno";
            //
            // lblNascimento
            //
            this.lblNascimento.AutoSize = true;
            this.lblNascimento.Location = new System.Drawing.Point(448, 42);
            this.lblNascimento.Name = "lblNascimento";
            this.lblNascimento.Size = new System.Drawing.Size(112, 15);
            this.lblNascimento.TabIndex = 2;
            this.lblNascimento.Text = "Data de nascimento";
            //
            // lblTelefone
            //
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(658, 42);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(52, 15);
            this.lblTelefone.TabIndex = 3;
            this.lblTelefone.Text = "Telefone";
            //
            // lblNumero
            //
            this.lblNumero.AutoSize = true;
            this.lblNumero.Location = new System.Drawing.Point(18, 95);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(51, 15);
            this.lblNumero.TabIndex = 4;
            this.lblNumero.Text = "Número";
            //
            // lblCep
            //
            this.lblCep.AutoSize = true;
            this.lblCep.Location = new System.Drawing.Point(238, 95);
            this.lblCep.Name = "lblCep";
            this.lblCep.Size = new System.Drawing.Size(28, 15);
            this.lblCep.TabIndex = 5;
            this.lblCep.Text = "CEP";
            //
            // lblComplemento
            //
            this.lblComplemento.AutoSize = true;
            this.lblComplemento.Location = new System.Drawing.Point(448, 95);
            this.lblComplemento.Name = "lblComplemento";
            this.lblComplemento.Size = new System.Drawing.Size(141, 15);
            this.lblComplemento.TabIndex = 6;
            this.lblComplemento.Text = "Complemento (opcional)";
            //
            // lblResponsavel
            //
            this.lblResponsavel.AutoSize = true;
            this.lblResponsavel.Location = new System.Drawing.Point(18, 148);
            this.lblResponsavel.Name = "lblResponsavel";
            this.lblResponsavel.Size = new System.Drawing.Size(122, 15);
            this.lblResponsavel.TabIndex = 7;
            this.lblResponsavel.Text = "Nome do responsável";
            //
            // lblCpf
            //
            this.lblCpf.AutoSize = true;
            this.lblCpf.Location = new System.Drawing.Point(448, 148);
            this.lblCpf.Name = "lblCpf";
            this.lblCpf.Size = new System.Drawing.Size(28, 15);
            this.lblCpf.TabIndex = 8;
            this.lblCpf.Text = "CPF";
            //
            // lblRg
            //
            this.lblRg.AutoSize = true;
            this.lblRg.Location = new System.Drawing.Point(658, 148);
            this.lblRg.Name = "lblRg";
            this.lblRg.Size = new System.Drawing.Size(22, 15);
            this.lblRg.TabIndex = 9;
            this.lblRg.Text = "RG";
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(18, 201);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(41, 15);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "E-mail";
            //
            // lblDiaVencimento
            //
            this.lblDiaVencimento.AutoSize = true;
            this.lblDiaVencimento.Location = new System.Drawing.Point(448, 201);
            this.lblDiaVencimento.Name = "lblDiaVencimento";
            this.lblDiaVencimento.Size = new System.Drawing.Size(107, 15);
            this.lblDiaVencimento.TabIndex = 11;
            this.lblDiaVencimento.Text = "Dia do vencimento";
            //
            // txtNome
            //
            this.txtNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNome.Location = new System.Drawing.Point(18, 62);
            this.txtNome.MaxLength = 50;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(410, 24);
            this.txtNome.TabIndex = 12;
            //
            // txtDataNascimento
            //
            this.txtDataNascimento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataNascimento.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDataNascimento.Location = new System.Drawing.Point(448, 62);
            this.txtDataNascimento.MaxLength = 10;
            this.txtDataNascimento.Name = "txtDataNascimento";
            this.txtDataNascimento.Size = new System.Drawing.Size(190, 24);
            this.txtDataNascimento.TabIndex = 1;
            //
            // txtTelefone
            //
            this.txtTelefone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelefone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTelefone.Location = new System.Drawing.Point(658, 62);
            this.txtTelefone.MaxLength = 15;
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Size = new System.Drawing.Size(200, 24);
            this.txtTelefone.TabIndex = 13;
            //
            // txtNumero
            //
            this.txtNumero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNumero.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNumero.Location = new System.Drawing.Point(18, 115);
            this.txtNumero.MaxLength = 5;
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(200, 24);
            this.txtNumero.TabIndex = 14;
            //
            // pnlCep
            //
            this.pnlCep.BackColor = System.Drawing.Color.White;
            this.pnlCep.Controls.Add(this.txtCep);
            this.pnlCep.Location = new System.Drawing.Point(238, 115);
            this.pnlCep.Name = "pnlCep";
            this.pnlCep.Padding = new System.Windows.Forms.Padding(0);
            this.pnlCep.Size = new System.Drawing.Size(190, 24);
            this.pnlCep.TabIndex = 4;
            //
            // txtCep
            //
            this.txtCep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCep.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCep.Location = new System.Drawing.Point(1, 1);
            this.txtCep.MaxLength = 9;
            this.txtCep.Name = "txtCep";
            this.txtCep.Size = new System.Drawing.Size(188, 17);
            this.txtCep.TabIndex = 0;
            //
            // txtComplemento
            //
            this.txtComplemento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComplemento.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtComplemento.Location = new System.Drawing.Point(448, 115);
            this.txtComplemento.MaxLength = 50;
            this.txtComplemento.Name = "txtComplemento";
            this.txtComplemento.Size = new System.Drawing.Size(410, 24);
            this.txtComplemento.TabIndex = 15;
            //
            // txtResponsavel
            //
            this.txtResponsavel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResponsavel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtResponsavel.Location = new System.Drawing.Point(18, 168);
            this.txtResponsavel.MaxLength = 50;
            this.txtResponsavel.Name = "txtResponsavel";
            this.txtResponsavel.Size = new System.Drawing.Size(410, 24);
            this.txtResponsavel.TabIndex = 16;
            //
            // txtCpf
            //
            this.txtCpf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCpf.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCpf.Location = new System.Drawing.Point(448, 168);
            this.txtCpf.MaxLength = 14;
            this.txtCpf.Name = "txtCpf";
            this.txtCpf.Size = new System.Drawing.Size(190, 24);
            this.txtCpf.TabIndex = 17;
            //
            // txtRg
            //
            this.txtRg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRg.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtRg.Location = new System.Drawing.Point(658, 168);
            this.txtRg.MaxLength = 12;
            this.txtRg.Name = "txtRg";
            this.txtRg.Size = new System.Drawing.Size(200, 24);
            this.txtRg.TabIndex = 18;
            //
            // txtEmail
            //
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtEmail.Location = new System.Drawing.Point(18, 221);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(410, 24);
            this.txtEmail.TabIndex = 19;
            //
            // nudDiaVencimento
            //
            this.nudDiaVencimento.Location = new System.Drawing.Point(448, 221);
            this.nudDiaVencimento.Maximum = new decimal(new int[] {
            28,
            0,
            0,
            0});
            this.nudDiaVencimento.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDiaVencimento.Name = "nudDiaVencimento";
            this.nudDiaVencimento.Size = new System.Drawing.Size(190, 23);
            this.nudDiaVencimento.TabIndex = 10;
            this.nudDiaVencimento.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // btnSalvar
            //
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(86)))), ((int)(((byte)(196)))));
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(16, 368);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(145, 36);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar aluno";
            this.btnSalvar.UseVisualStyleBackColor = false;
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnCancelar.Location = new System.Drawing.Point(230, 368);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 36);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            //
            // AlunoCadastroForm
            //
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(920, 430);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.pnlCartao);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlunoCadastroForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Novo Aluno";
            this.pnlCartao.ResumeLayout(false);
            this.pnlCartao.PerformLayout();
            this.pnlCep.ResumeLayout(false);
            this.pnlCep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiaVencimento)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
