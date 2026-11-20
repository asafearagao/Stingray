namespace Stingray.Formularios
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Panel pnlCabecalho;
        private System.Windows.Forms.Panel pnlCorpo;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.TextBox txtUsuario;
        private Stingray.RoundedButton btnVisualizar;
        private Stingray.RoundedButton btnLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCard = new System.Windows.Forms.Panel();
            this.pnlCabecalho = new System.Windows.Forms.Panel();
            this.pnlCorpo = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblSenha = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.btnVisualizar = new Stingray.RoundedButton();
            this.btnLogin = new Stingray.RoundedButton();
            this.pnlCard.SuspendLayout();
            this.pnlCabecalho.SuspendLayout();
            this.pnlCorpo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            //
            // pnlCard
            //
            this.pnlCard.BackColor = System.Drawing.Color.White;
            this.pnlCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.pnlCorpo);
            this.pnlCard.Controls.Add(this.pnlCabecalho);
            this.pnlCard.Location = new System.Drawing.Point(22, 20);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(426, 560);
            this.pnlCard.TabIndex = 0;
            //
            // pnlCabecalho
            //
            this.pnlCabecalho.BackColor = System.Drawing.Color.White;
            this.pnlCabecalho.Controls.Add(this.lblDescricao);
            this.pnlCabecalho.Controls.Add(this.lblTitulo);
            this.pnlCabecalho.Controls.Add(this.picLogo);
            this.pnlCabecalho.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCabecalho.Location = new System.Drawing.Point(0, 0);
            this.pnlCabecalho.Name = "pnlCabecalho";
            this.pnlCabecalho.Size = new System.Drawing.Size(424, 222);
            this.pnlCabecalho.TabIndex = 0;
            //
            // picLogo
            //
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.Location = new System.Drawing.Point(91, 13);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(242, 136);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblTitulo.Location = new System.Drawing.Point(20, 151);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(382, 31);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Acesso ao sistema";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblDescricao
            //
            this.lblDescricao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDescricao.Location = new System.Drawing.Point(20, 184);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(382, 22);
            this.lblDescricao.TabIndex = 2;
            this.lblDescricao.Text = "Entre com suas credenciais para continuar";
            this.lblDescricao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlCorpo
            //
            this.pnlCorpo.BackColor = System.Drawing.Color.White;
            this.pnlCorpo.Controls.Add(this.btnVisualizar);
            this.pnlCorpo.Controls.Add(this.btnLogin);
            this.pnlCorpo.Controls.Add(this.txtSenha);
            this.pnlCorpo.Controls.Add(this.txtUsuario);
            this.pnlCorpo.Controls.Add(this.lblSenha);
            this.pnlCorpo.Controls.Add(this.lblUsuario);
            this.pnlCorpo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCorpo.Location = new System.Drawing.Point(0, 222);
            this.pnlCorpo.Name = "pnlCorpo";
            this.pnlCorpo.Size = new System.Drawing.Size(424, 336);
            this.pnlCorpo.TabIndex = 1;
            //
            // lblUsuario
            //
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblUsuario.Location = new System.Drawing.Point(42, 27);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(50, 15);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuário";
            //
            // txtUsuario
            //
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUsuario.Location = new System.Drawing.Point(45, 50);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(334, 25);
            this.txtUsuario.TabIndex = 1;
            //
            // lblSenha
            //
            this.lblSenha.AutoSize = true;
            this.lblSenha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSenha.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblSenha.Location = new System.Drawing.Point(42, 103);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(39, 15);
            this.lblSenha.TabIndex = 2;
            this.lblSenha.Text = "Senha";
            //
            // txtSenha
            //
            this.txtSenha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSenha.Location = new System.Drawing.Point(45, 126);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '•';
            this.txtSenha.Size = new System.Drawing.Size(255, 25);
            this.txtSenha.TabIndex = 2;
            //
            // btnVisualizar
            //
            this.btnVisualizar.BackColor = System.Drawing.Color.White;
            this.btnVisualizar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnVisualizar.CornerRadius = 7;
            this.btnVisualizar.FlatAppearance.BorderSize = 0;
            this.btnVisualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVisualizar.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnVisualizar.ForeColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnVisualizar.Location = new System.Drawing.Point(307, 122);
            this.btnVisualizar.Name = "btnVisualizar";
            this.btnVisualizar.Size = new System.Drawing.Size(72, 34);
            this.btnVisualizar.TabIndex = 3;
            this.btnVisualizar.Text = "Mostrar";
            this.btnVisualizar.UseVisualStyleBackColor = false;
            this.btnVisualizar.Click += new System.EventHandler(this.btnVisualizar_Click);
            //
            // btnLogin
            //
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnLogin.CornerRadius = 8;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(45, 201);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(334, 46);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Entrar";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            //
            // LoginForm
            //
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(243, 246, 251);
            this.ClientSize = new System.Drawing.Size(470, 600);
            this.Controls.Add(this.pnlCard);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AquaFit - Acesso";
            this.pnlCard.ResumeLayout(false);
            this.pnlCabecalho.ResumeLayout(false);
            this.pnlCorpo.ResumeLayout(false);
            this.pnlCorpo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
