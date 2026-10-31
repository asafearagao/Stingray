namespace Stingray
{
    partial class CadastroAlunoForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel painelPrincipal;
        private System.Windows.Forms.Label rotuloTitulo;
        private System.Windows.Forms.Label rotuloNome;
        private System.Windows.Forms.TextBox campoNome;
        private System.Windows.Forms.Label rotuloNascimento;
        private System.Windows.Forms.DateTimePicker seletorDataNascimento;
        private System.Windows.Forms.Label rotuloTelefone;
        private System.Windows.Forms.MaskedTextBox campoTelefone;
        private System.Windows.Forms.Label rotuloNumero;
        private System.Windows.Forms.TextBox campoNumero;
        private System.Windows.Forms.Label rotuloCep;
        private System.Windows.Forms.MaskedTextBox campoCep;
        private System.Windows.Forms.Label rotuloCidade;
        private System.Windows.Forms.TextBox campoCidade;
        private System.Windows.Forms.Label rotuloResponsavel;
        private System.Windows.Forms.TextBox campoResponsavel;
        private System.Windows.Forms.Label rotuloCpf;
        private System.Windows.Forms.MaskedTextBox campoCpf;
        private System.Windows.Forms.Label rotuloRg;
        private System.Windows.Forms.TextBox campoRg;
        private System.Windows.Forms.Label rotuloEmail;
        private System.Windows.Forms.TextBox campoEmail;
        private Stingray.RoundedButton botaoSalvar;
        private Stingray.RoundedButton botaoCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.painelPrincipal = new System.Windows.Forms.Panel();
            this.rotuloTitulo = new System.Windows.Forms.Label();
            this.rotuloNome = new System.Windows.Forms.Label();
            this.campoNome = new System.Windows.Forms.TextBox();
            this.rotuloNascimento = new System.Windows.Forms.Label();
            this.seletorDataNascimento = new System.Windows.Forms.DateTimePicker();
            this.rotuloTelefone = new System.Windows.Forms.Label();
            this.campoTelefone = new System.Windows.Forms.MaskedTextBox();
            this.rotuloNumero = new System.Windows.Forms.Label();
            this.campoNumero = new System.Windows.Forms.TextBox();
            this.rotuloCep = new System.Windows.Forms.Label();
            this.campoCep = new System.Windows.Forms.MaskedTextBox();
            this.rotuloCidade = new System.Windows.Forms.Label();
            this.campoCidade = new System.Windows.Forms.TextBox();
            this.rotuloResponsavel = new System.Windows.Forms.Label();
            this.campoResponsavel = new System.Windows.Forms.TextBox();
            this.rotuloCpf = new System.Windows.Forms.Label();
            this.campoCpf = new System.Windows.Forms.MaskedTextBox();
            this.rotuloRg = new System.Windows.Forms.Label();
            this.campoRg = new System.Windows.Forms.TextBox();
            this.rotuloEmail = new System.Windows.Forms.Label();
            this.campoEmail = new System.Windows.Forms.TextBox();
            this.botaoSalvar = new Stingray.RoundedButton();
            this.botaoCancelar = new Stingray.RoundedButton();
            this.painelPrincipal.SuspendLayout();
            this.SuspendLayout();
            //
            // painelPrincipal
            //
            this.painelPrincipal.BackColor = System.Drawing.Color.White;
            this.painelPrincipal.Controls.Add(this.botaoCancelar);
            this.painelPrincipal.Controls.Add(this.botaoSalvar);
            this.painelPrincipal.Controls.Add(this.campoEmail);
            this.painelPrincipal.Controls.Add(this.rotuloEmail);
            this.painelPrincipal.Controls.Add(this.campoRg);
            this.painelPrincipal.Controls.Add(this.rotuloRg);
            this.painelPrincipal.Controls.Add(this.campoCpf);
            this.painelPrincipal.Controls.Add(this.rotuloCpf);
            this.painelPrincipal.Controls.Add(this.campoResponsavel);
            this.painelPrincipal.Controls.Add(this.rotuloResponsavel);
            this.painelPrincipal.Controls.Add(this.campoCidade);
            this.painelPrincipal.Controls.Add(this.rotuloCidade);
            this.painelPrincipal.Controls.Add(this.campoCep);
            this.painelPrincipal.Controls.Add(this.rotuloCep);
            this.painelPrincipal.Controls.Add(this.campoNumero);
            this.painelPrincipal.Controls.Add(this.rotuloNumero);
            this.painelPrincipal.Controls.Add(this.campoTelefone);
            this.painelPrincipal.Controls.Add(this.rotuloTelefone);
            this.painelPrincipal.Controls.Add(this.seletorDataNascimento);
            this.painelPrincipal.Controls.Add(this.rotuloNascimento);
            this.painelPrincipal.Controls.Add(this.campoNome);
            this.painelPrincipal.Controls.Add(this.rotuloNome);
            this.painelPrincipal.Controls.Add(this.rotuloTitulo);
            this.painelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.painelPrincipal.Name = "painelPrincipal";
            this.painelPrincipal.Padding = new System.Windows.Forms.Padding(16);
            this.painelPrincipal.Size = new System.Drawing.Size(920, 500);
            this.painelPrincipal.TabIndex = 0;
            //
            // rotuloTitulo
            //
            this.rotuloTitulo.AutoSize = true;
            this.rotuloTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.rotuloTitulo.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.rotuloTitulo.Location = new System.Drawing.Point(20, 18);
            this.rotuloTitulo.Name = "rotuloTitulo";
            this.rotuloTitulo.Size = new System.Drawing.Size(81, 20);
            this.rotuloTitulo.TabIndex = 0;
            this.rotuloTitulo.Text = "Novo aluno";
            //
            // rotuloNome
            //
            this.rotuloNome.AutoSize = true;
            this.rotuloNome.Location = new System.Drawing.Point(20, 58);
            this.rotuloNome.Name = "rotuloNome";
            this.rotuloNome.Size = new System.Drawing.Size(83, 15);
            this.rotuloNome.TabIndex = 1;
            this.rotuloNome.Text = "Nome do aluno";
            //
            // campoNome
            //
            this.campoNome.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoNome.Location = new System.Drawing.Point(20, 80);
            this.campoNome.Name = "campoNome";
            this.campoNome.Size = new System.Drawing.Size(420, 25);
            this.campoNome.TabIndex = 2;
            //
            // rotuloNascimento
            //
            this.rotuloNascimento.AutoSize = true;
            this.rotuloNascimento.Location = new System.Drawing.Point(464, 58);
            this.rotuloNascimento.Name = "rotuloNascimento";
            this.rotuloNascimento.Size = new System.Drawing.Size(112, 15);
            this.rotuloNascimento.TabIndex = 3;
            this.rotuloNascimento.Text = "Data de nascimento";
            //
            // seletorDataNascimento
            //
            this.seletorDataNascimento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.seletorDataNascimento.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.seletorDataNascimento.CustomFormat = "dd/MM/yyyy";
            this.seletorDataNascimento.Location = new System.Drawing.Point(464, 80);
            this.seletorDataNascimento.Name = "seletorDataNascimento";
            this.seletorDataNascimento.Size = new System.Drawing.Size(200, 23);
            this.seletorDataNascimento.TabIndex = 4;
            //
            // rotuloTelefone
            //
            this.rotuloTelefone.AutoSize = true;
            this.rotuloTelefone.Location = new System.Drawing.Point(680, 58);
            this.rotuloTelefone.Name = "rotuloTelefone";
            this.rotuloTelefone.Size = new System.Drawing.Size(51, 15);
            this.rotuloTelefone.TabIndex = 5;
            this.rotuloTelefone.Text = "Telefone";
            //
            // campoTelefone
            //
            this.campoTelefone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoTelefone.Location = new System.Drawing.Point(680, 80);
            this.campoTelefone.Mask = "(00) 00000-0000";
            this.campoTelefone.Name = "campoTelefone";
            this.campoTelefone.Size = new System.Drawing.Size(200, 25);
            this.campoTelefone.TabIndex = 6;
            //
            // rotuloNumero
            //
            this.rotuloNumero.AutoSize = true;
            this.rotuloNumero.Location = new System.Drawing.Point(20, 130);
            this.rotuloNumero.Name = "rotuloNumero";
            this.rotuloNumero.Size = new System.Drawing.Size(48, 15);
            this.rotuloNumero.TabIndex = 7;
            this.rotuloNumero.Text = "Número";
            //
            // campoNumero
            //
            this.campoNumero.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoNumero.Location = new System.Drawing.Point(20, 152);
            this.campoNumero.Name = "campoNumero";
            this.campoNumero.Size = new System.Drawing.Size(200, 25);
            this.campoNumero.TabIndex = 8;
            //
            // rotuloCep
            //
            this.rotuloCep.AutoSize = true;
            this.rotuloCep.Location = new System.Drawing.Point(248, 130);
            this.rotuloCep.Name = "rotuloCep";
            this.rotuloCep.Size = new System.Drawing.Size(28, 15);
            this.rotuloCep.TabIndex = 9;
            this.rotuloCep.Text = "CEP";
            //
            // campoCep
            //
            this.campoCep.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoCep.Location = new System.Drawing.Point(248, 152);
            this.campoCep.Mask = "00000-000";
            this.campoCep.Name = "campoCep";
            this.campoCep.Size = new System.Drawing.Size(190, 25);
            this.campoCep.TabIndex = 10;
            //
            // rotuloCidade
            //
            this.rotuloCidade.AutoSize = true;
            this.rotuloCidade.Location = new System.Drawing.Point(464, 130);
            this.rotuloCidade.Name = "rotuloCidade";
            this.rotuloCidade.Size = new System.Drawing.Size(43, 15);
            this.rotuloCidade.TabIndex = 11;
            this.rotuloCidade.Text = "Cidade";
            //
            // campoCidade
            //
            this.campoCidade.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoCidade.Location = new System.Drawing.Point(464, 152);
            this.campoCidade.Name = "campoCidade";
            this.campoCidade.Size = new System.Drawing.Size(416, 25);
            this.campoCidade.TabIndex = 12;
            //
            // rotuloResponsavel
            //
            this.rotuloResponsavel.AutoSize = true;
            this.rotuloResponsavel.Location = new System.Drawing.Point(20, 202);
            this.rotuloResponsavel.Name = "rotuloResponsavel";
            this.rotuloResponsavel.Size = new System.Drawing.Size(119, 15);
            this.rotuloResponsavel.TabIndex = 13;
            this.rotuloResponsavel.Text = "Nome do responsável";
            //
            // campoResponsavel
            //
            this.campoResponsavel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoResponsavel.Location = new System.Drawing.Point(20, 224);
            this.campoResponsavel.Name = "campoResponsavel";
            this.campoResponsavel.Size = new System.Drawing.Size(420, 25);
            this.campoResponsavel.TabIndex = 14;
            //
            // rotuloCpf
            //
            this.rotuloCpf.AutoSize = true;
            this.rotuloCpf.Location = new System.Drawing.Point(464, 202);
            this.rotuloCpf.Name = "rotuloCpf";
            this.rotuloCpf.Size = new System.Drawing.Size(28, 15);
            this.rotuloCpf.TabIndex = 15;
            this.rotuloCpf.Text = "CPF";
            //
            // campoCpf
            //
            this.campoCpf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoCpf.Location = new System.Drawing.Point(464, 224);
            this.campoCpf.Mask = "000\\.000\\.000-00";
            this.campoCpf.Name = "campoCpf";
            this.campoCpf.Size = new System.Drawing.Size(200, 25);
            this.campoCpf.TabIndex = 16;
            //
            // rotuloRg
            //
            this.rotuloRg.AutoSize = true;
            this.rotuloRg.Location = new System.Drawing.Point(680, 202);
            this.rotuloRg.Name = "rotuloRg";
            this.rotuloRg.Size = new System.Drawing.Size(21, 15);
            this.rotuloRg.TabIndex = 17;
            this.rotuloRg.Text = "RG";
            //
            // campoRg
            //
            this.campoRg.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoRg.Location = new System.Drawing.Point(680, 224);
            this.campoRg.Name = "campoRg";
            this.campoRg.Size = new System.Drawing.Size(200, 25);
            this.campoRg.TabIndex = 18;
            //
            // rotuloEmail
            //
            this.rotuloEmail.AutoSize = true;
            this.rotuloEmail.Location = new System.Drawing.Point(20, 274);
            this.rotuloEmail.Name = "rotuloEmail";
            this.rotuloEmail.Size = new System.Drawing.Size(39, 15);
            this.rotuloEmail.TabIndex = 19;
            this.rotuloEmail.Text = "E-mail";
            //
            // campoEmail
            //
            this.campoEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.campoEmail.Location = new System.Drawing.Point(20, 296);
            this.campoEmail.Name = "campoEmail";
            this.campoEmail.Size = new System.Drawing.Size(420, 25);
            this.campoEmail.TabIndex = 20;
            //
            // botaoSalvar
            //
            this.botaoSalvar.BackColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.botaoSalvar.CornerRadius = 5;
            this.botaoSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.botaoSalvar.FlatAppearance.BorderSize = 0;
            this.botaoSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botaoSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.botaoSalvar.ForeColor = System.Drawing.Color.White;
            this.botaoSalvar.Location = new System.Drawing.Point(20, 390);
            this.botaoSalvar.Name = "botaoSalvar";
            this.botaoSalvar.Size = new System.Drawing.Size(170, 36);
            this.botaoSalvar.TabIndex = 21;
            this.botaoSalvar.Text = "Salvar aluno";
            this.botaoSalvar.UseVisualStyleBackColor = false;
            //
            // botaoCancelar
            //
            this.botaoCancelar.BackColor = System.Drawing.Color.White;
            this.botaoCancelar.CornerRadius = 5;
            this.botaoCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.botaoCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.botaoCancelar.FlatAppearance.BorderSize = 1;
            this.botaoCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botaoCancelar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.botaoCancelar.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.botaoCancelar.Location = new System.Drawing.Point(202, 390);
            this.botaoCancelar.Name = "botaoCancelar";
            this.botaoCancelar.Size = new System.Drawing.Size(110, 36);
            this.botaoCancelar.TabIndex = 22;
            this.botaoCancelar.Text = "Cancelar";
            this.botaoCancelar.UseVisualStyleBackColor = false;
            //
            // CadastroAlunoForm
            //
            this.AcceptButton = this.botaoSalvar;
            this.CancelButton = this.botaoCancelar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(247, 249, 252);
            this.ClientSize = new System.Drawing.Size(920, 500);
            this.Controls.Add(this.painelPrincipal);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CadastroAlunoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Novo aluno";
            this.painelPrincipal.ResumeLayout(false);
            this.painelPrincipal.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
