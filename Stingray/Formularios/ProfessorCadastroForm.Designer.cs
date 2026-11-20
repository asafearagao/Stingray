namespace Stingray
{
    partial class ProfessorCadastroForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label lblCpf;
        private System.Windows.Forms.Label lblRg;
        private System.Windows.Forms.Label lblCref;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.Label lblNascimento;
        private System.Windows.Forms.Label lblCep;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.Label lblComplemento;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.TextBox txtCpf;
        private System.Windows.Forms.TextBox txtRg;
        private System.Windows.Forms.TextBox txtCref;
        private System.Windows.Forms.TextBox txtTelefone;
        private System.Windows.Forms.TextBox txtDataNascimento;
        private System.Windows.Forms.TextBox txtCep;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.TextBox txtComplemento;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblCpf = new System.Windows.Forms.Label();
            this.lblRg = new System.Windows.Forms.Label();
            this.lblCref = new System.Windows.Forms.Label();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblNascimento = new System.Windows.Forms.Label();
            this.lblCep = new System.Windows.Forms.Label();
            this.lblNumero = new System.Windows.Forms.Label();
            this.lblComplemento = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtCpf = new System.Windows.Forms.TextBox();
            this.txtRg = new System.Windows.Forms.TextBox();
            this.txtCref = new System.Windows.Forms.TextBox();
            this.txtTelefone = new System.Windows.Forms.TextBox();
            this.txtDataNascimento = new System.Windows.Forms.TextBox();
            this.txtCep = new System.Windows.Forms.TextBox();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.txtComplemento = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // Cabeçalho
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblTitulo.Location = new System.Drawing.Point(24, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(170, 25);
            this.lblTitulo.Text = "Adicionar professor";
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDescricao.Location = new System.Drawing.Point(24, 51);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(260, 15);
            this.lblDescricao.Text = "Informe os dados do docente para salvar o cadastro.";
            // Rótulos
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(24, 91);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(70, 15);
            this.lblNome.Text = "Nome completo";
            this.lblCpf.AutoSize = true;
            this.lblCpf.Location = new System.Drawing.Point(24, 164);
            this.lblCpf.Name = "lblCpf";
            this.lblCpf.Size = new System.Drawing.Size(70, 15);
            this.lblCpf.Text = "CPF";
            this.lblRg.AutoSize = true;
            this.lblRg.Location = new System.Drawing.Point(194, 164);
            this.lblRg.Name = "lblRg";
            this.lblRg.Size = new System.Drawing.Size(70, 15);
            this.lblRg.Text = "RG";
            this.lblCref.AutoSize = true;
            this.lblCref.Location = new System.Drawing.Point(364, 164);
            this.lblCref.Name = "lblCref";
            this.lblCref.Size = new System.Drawing.Size(70, 15);
            this.lblCref.Text = "CREF";
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(24, 235);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(70, 15);
            this.lblTelefone.Text = "Telefone";
            this.lblNascimento.AutoSize = true;
            this.lblNascimento.Location = new System.Drawing.Point(194, 235);
            this.lblNascimento.Name = "lblNascimento";
            this.lblNascimento.Size = new System.Drawing.Size(70, 15);
            this.lblNascimento.Text = "Data de nascimento";
            this.lblCep.AutoSize = true;
            this.lblCep.Location = new System.Drawing.Point(364, 235);
            this.lblCep.Name = "lblCep";
            this.lblCep.Size = new System.Drawing.Size(70, 15);
            this.lblCep.Text = "CEP";
            this.lblNumero.AutoSize = true;
            this.lblNumero.Location = new System.Drawing.Point(24, 306);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(70, 15);
            this.lblNumero.Text = "Número";
            this.lblComplemento.AutoSize = true;
            this.lblComplemento.Location = new System.Drawing.Point(194, 306);
            this.lblComplemento.Name = "lblComplemento";
            this.lblComplemento.Size = new System.Drawing.Size(70, 15);
            this.lblComplemento.Text = "Complemento (opcional)";
            // Campos
            this.txtNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNome.Location = new System.Drawing.Point(24, 112);
            this.txtNome.MaxLength = 50;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(500, 36);
            this.txtCpf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCpf.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCpf.Location = new System.Drawing.Point(24, 185);
            this.txtCpf.MaxLength = 14;
            this.txtCpf.Name = "txtCpf";
            this.txtCpf.Size = new System.Drawing.Size(155, 36);
            this.txtRg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRg.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtRg.Location = new System.Drawing.Point(194, 185);
            this.txtRg.MaxLength = 12;
            this.txtRg.Name = "txtRg";
            this.txtRg.Size = new System.Drawing.Size(155, 36);
            this.txtCref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCref.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCref.Location = new System.Drawing.Point(364, 185);
            this.txtCref.MaxLength = 255;
            this.txtCref.Name = "txtCref";
            this.txtCref.Size = new System.Drawing.Size(160, 36);
            this.txtTelefone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelefone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTelefone.Location = new System.Drawing.Point(24, 256);
            this.txtTelefone.MaxLength = 15;
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Size = new System.Drawing.Size(155, 36);
            this.txtDataNascimento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataNascimento.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDataNascimento.Location = new System.Drawing.Point(194, 256);
            this.txtDataNascimento.MaxLength = 10;
            this.txtDataNascimento.Name = "txtDataNascimento";
            this.txtDataNascimento.Size = new System.Drawing.Size(155, 36);
            this.txtCep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCep.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCep.Location = new System.Drawing.Point(364, 256);
            this.txtCep.MaxLength = 9;
            this.txtCep.Name = "txtCep";
            this.txtCep.Size = new System.Drawing.Size(160, 36);
            this.txtNumero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNumero.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNumero.Location = new System.Drawing.Point(24, 327);
            this.txtNumero.MaxLength = 5;
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(155, 36);
            this.txtComplemento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComplemento.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtComplemento.Location = new System.Drawing.Point(194, 327);
            this.txtComplemento.MaxLength = 50;
            this.txtComplemento.Name = "txtComplemento";
            this.txtComplemento.Size = new System.Drawing.Size(330, 36);
            // Botões
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnCancelar.Location = new System.Drawing.Point(278, 397);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 38);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnCancelar.FlatAppearance.BorderSize = 1;
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(394, 397);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(130, 38);
            this.btnSalvar.Text = "Adicionar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnSalvar.FlatAppearance.BorderSize = 1;
            // ProfessorCadastroForm
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(548, 455);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitulo, this.lblDescricao, this.lblNome, this.lblCpf, this.lblRg,
                this.lblCref, this.lblTelefone, this.lblNascimento, this.lblCep,
                this.lblNumero, this.lblComplemento, this.txtNome, this.txtCpf, this.txtRg,
                this.txtCref, this.txtTelefone, this.txtDataNascimento, this.txtCep,
                this.txtNumero, this.txtComplemento, this.btnCancelar, this.btnSalvar
            });
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfessorCadastroForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Novo professor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

    }
}
