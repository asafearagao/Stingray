namespace Stingray.Formularios
{
    partial class ContaAcessoForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel painelPrincipal;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblSenhaAtual;
        private System.Windows.Forms.TextBox txtSenhaAtual;
        private System.Windows.Forms.Label lblNomeUsuario;
        private System.Windows.Forms.TextBox txtNomeUsuario;
        private System.Windows.Forms.Label lblNovaSenha;
        private System.Windows.Forms.TextBox txtNovaSenha;
        private System.Windows.Forms.Label lblConfirmarSenha;
        private System.Windows.Forms.TextBox txtConfirmarSenha;
        private System.Windows.Forms.Label lblOrientacao;
        private Stingray.RoundedButton btnSalvar;
        private Stingray.RoundedButton btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.painelPrincipal = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblSenhaAtual = new System.Windows.Forms.Label();
            this.txtSenhaAtual = new System.Windows.Forms.TextBox();
            this.lblNomeUsuario = new System.Windows.Forms.Label();
            this.txtNomeUsuario = new System.Windows.Forms.TextBox();
            this.lblNovaSenha = new System.Windows.Forms.Label();
            this.txtNovaSenha = new System.Windows.Forms.TextBox();
            this.lblConfirmarSenha = new System.Windows.Forms.Label();
            this.txtConfirmarSenha = new System.Windows.Forms.TextBox();
            this.lblOrientacao = new System.Windows.Forms.Label();
            this.btnSalvar = new Stingray.RoundedButton();
            this.btnCancelar = new Stingray.RoundedButton();
            this.painelPrincipal.SuspendLayout();
            this.SuspendLayout();
            //
            // painelPrincipal
            //
            this.painelPrincipal.BackColor = System.Drawing.Color.White;
            this.painelPrincipal.Controls.Add(this.lblTitulo);
            this.painelPrincipal.Controls.Add(this.lblDescricao);
            this.painelPrincipal.Controls.Add(this.lblSenhaAtual);
            this.painelPrincipal.Controls.Add(this.txtSenhaAtual);
            this.painelPrincipal.Controls.Add(this.lblNomeUsuario);
            this.painelPrincipal.Controls.Add(this.txtNomeUsuario);
            this.painelPrincipal.Controls.Add(this.lblNovaSenha);
            this.painelPrincipal.Controls.Add(this.txtNovaSenha);
            this.painelPrincipal.Controls.Add(this.lblConfirmarSenha);
            this.painelPrincipal.Controls.Add(this.txtConfirmarSenha);
            this.painelPrincipal.Controls.Add(this.lblOrientacao);
            this.painelPrincipal.Controls.Add(this.btnSalvar);
            this.painelPrincipal.Controls.Add(this.btnCancelar);
            this.painelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.painelPrincipal.Name = "painelPrincipal";
            this.painelPrincipal.Padding = new System.Windows.Forms.Padding(32);
            this.painelPrincipal.Size = new System.Drawing.Size(540, 590);
            this.painelPrincipal.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblTitulo.Location = new System.Drawing.Point(35, 27);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(235, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Configurar acesso";
            //
            // lblDescricao
            //
            this.lblDescricao.AutoSize = false;
            this.lblDescricao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblDescricao.Location = new System.Drawing.Point(38, 67);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(460, 38);
            this.lblDescricao.TabIndex = 1;
            this.lblDescricao.Text = "Crie as credenciais que serão usadas para entrar no Stingray.";
            //
            // lblSenhaAtual
            //
            this.lblSenhaAtual.AutoSize = true;
            this.lblSenhaAtual.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSenhaAtual.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblSenhaAtual.Location = new System.Drawing.Point(38, 119);
            this.lblSenhaAtual.Name = "lblSenhaAtual";
            this.lblSenhaAtual.Size = new System.Drawing.Size(68, 15);
            this.lblSenhaAtual.TabIndex = 2;
            this.lblSenhaAtual.Text = "Senha atual";
            //
            // txtSenhaAtual
            //
            this.txtSenhaAtual.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSenhaAtual.Location = new System.Drawing.Point(40, 142);
            this.txtSenhaAtual.Name = "txtSenhaAtual";
            this.txtSenhaAtual.PasswordChar = '●';
            this.txtSenhaAtual.Size = new System.Drawing.Size(458, 25);
            this.txtSenhaAtual.TabIndex = 0;
            //
            // lblNomeUsuario
            //
            this.lblNomeUsuario.AutoSize = true;
            this.lblNomeUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNomeUsuario.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblNomeUsuario.Location = new System.Drawing.Point(38, 204);
            this.lblNomeUsuario.Name = "lblNomeUsuario";
            this.lblNomeUsuario.Size = new System.Drawing.Size(108, 15);
            this.lblNomeUsuario.TabIndex = 3;
            this.lblNomeUsuario.Text = "Novo nome de usuário";
            //
            // txtNomeUsuario
            //
            this.txtNomeUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNomeUsuario.Location = new System.Drawing.Point(40, 227);
            this.txtNomeUsuario.MaxLength = 50;
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Size = new System.Drawing.Size(458, 25);
            this.txtNomeUsuario.TabIndex = 1;
            //
            // lblNovaSenha
            //
            this.lblNovaSenha.AutoSize = true;
            this.lblNovaSenha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNovaSenha.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblNovaSenha.Location = new System.Drawing.Point(38, 289);
            this.lblNovaSenha.Name = "lblNovaSenha";
            this.lblNovaSenha.Size = new System.Drawing.Size(67, 15);
            this.lblNovaSenha.TabIndex = 4;
            this.lblNovaSenha.Text = "Nova senha";
            //
            // txtNovaSenha
            //
            this.txtNovaSenha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNovaSenha.Location = new System.Drawing.Point(40, 312);
            this.txtNovaSenha.Name = "txtNovaSenha";
            this.txtNovaSenha.PasswordChar = '●';
            this.txtNovaSenha.Size = new System.Drawing.Size(458, 25);
            this.txtNovaSenha.TabIndex = 2;
            //
            // lblConfirmarSenha
            //
            this.lblConfirmarSenha.AutoSize = true;
            this.lblConfirmarSenha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblConfirmarSenha.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblConfirmarSenha.Location = new System.Drawing.Point(38, 374);
            this.lblConfirmarSenha.Name = "lblConfirmarSenha";
            this.lblConfirmarSenha.Size = new System.Drawing.Size(91, 15);
            this.lblConfirmarSenha.TabIndex = 5;
            this.lblConfirmarSenha.Text = "Confirme a senha";
            //
            // txtConfirmarSenha
            //
            this.txtConfirmarSenha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtConfirmarSenha.Location = new System.Drawing.Point(40, 397);
            this.txtConfirmarSenha.Name = "txtConfirmarSenha";
            this.txtConfirmarSenha.PasswordChar = '●';
            this.txtConfirmarSenha.Size = new System.Drawing.Size(458, 25);
            this.txtConfirmarSenha.TabIndex = 3;
            //
            // lblOrientacao
            //
            this.lblOrientacao.AutoSize = true;
            this.lblOrientacao.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblOrientacao.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblOrientacao.Location = new System.Drawing.Point(38, 439);
            this.lblOrientacao.Name = "lblOrientacao";
            this.lblOrientacao.Size = new System.Drawing.Size(240, 15);
            this.lblOrientacao.TabIndex = 6;
            this.lblOrientacao.Text = "A senha não pode ficar vazia.";
            //
            // btnSalvar
            //
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnSalvar.CornerRadius = 7;
            this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalvar.FlatAppearance.BorderSize = 0;
            this.btnSalvar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(18, 61, 140);
            this.btnSalvar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(22, 73, 168);
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(40, 493);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(285, 42);
            this.btnSalvar.TabIndex = 4;
            this.btnSalvar.Text = "Salvar acesso";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.CornerRadius = 7;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnCancelar.Location = new System.Drawing.Point(338, 493);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(160, 42);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //
            // ContaAcessoForm
            //
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(540, 570);
            this.Controls.Add(this.painelPrincipal);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContaAcessoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acesso ao Stingray";
            this.painelPrincipal.ResumeLayout(false);
            this.painelPrincipal.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
