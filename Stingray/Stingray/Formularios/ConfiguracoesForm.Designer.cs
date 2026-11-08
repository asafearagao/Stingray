namespace Stingray {
    partial class ConfiguracoesForm {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel painelConteudo;
        private System.Windows.Forms.Label lblTituloWhatsApp;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.painelConteudo = new System.Windows.Forms.Panel();
            this.cartaoWhatsApp = new System.Windows.Forms.Panel();
            this.lblTituloWhatsApp = new System.Windows.Forms.Label();
            this.iconeWhatsApp = new System.Windows.Forms.PictureBox();
            this.chkWhatsAppAtivado = new Stingray.ToggleSwitch();
            this.lblEstadoWhatsApp = new System.Windows.Forms.Label();
            this.lblDescricaoWhatsApp = new System.Windows.Forms.Label();
            this.btnAbrirWhatsApp = new Stingray.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)(this.iconeWhatsApp)).BeginInit();
            this.painelConteudo.SuspendLayout();
            this.cartaoWhatsApp.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblTitulo.Location = new System.Drawing.Point(13, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(143, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Configurações";
            //
            // painelConteudo
            //
            this.painelConteudo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.painelConteudo.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.painelConteudo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.painelConteudo.Controls.Add(this.cartaoWhatsApp);
            this.painelConteudo.Location = new System.Drawing.Point(12, 64);
            this.painelConteudo.Name = "painelConteudo";
            this.painelConteudo.Size = new System.Drawing.Size(1026, 584);
            this.painelConteudo.TabIndex = 1;
            this.painelConteudo.Paint += new System.Windows.Forms.PaintEventHandler(this.PainelConteudo_Paint);
            //
            // cartaoWhatsApp
            //
            this.cartaoWhatsApp.BackColor = System.Drawing.Color.White;
            this.cartaoWhatsApp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cartaoWhatsApp.Controls.Add(this.btnAbrirWhatsApp);
            this.cartaoWhatsApp.Controls.Add(this.lblDescricaoWhatsApp);
            this.cartaoWhatsApp.Controls.Add(this.lblEstadoWhatsApp);
            this.cartaoWhatsApp.Controls.Add(this.chkWhatsAppAtivado);
            this.cartaoWhatsApp.Controls.Add(this.iconeWhatsApp);
            this.cartaoWhatsApp.Controls.Add(this.lblTituloWhatsApp);
            this.cartaoWhatsApp.Location = new System.Drawing.Point(24, 28);
            this.cartaoWhatsApp.Name = "cartaoWhatsApp";
            this.cartaoWhatsApp.Size = new System.Drawing.Size(440, 166);
            this.cartaoWhatsApp.TabIndex = 0;
            this.cartaoWhatsApp.Paint += new System.Windows.Forms.PaintEventHandler(this.CartaoWhatsApp_Paint);
            //
            // lblTituloWhatsApp
            //
            this.lblTituloWhatsApp.AutoSize = true;
            this.lblTituloWhatsApp.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloWhatsApp.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.lblTituloWhatsApp.Location = new System.Drawing.Point(62, 14);
            this.lblTituloWhatsApp.Name = "lblTituloWhatsApp";
            this.lblTituloWhatsApp.Size = new System.Drawing.Size(111, 20);
            this.lblTituloWhatsApp.TabIndex = 0;
            this.lblTituloWhatsApp.Text = "WhatsApp Web";
            //
            // iconeWhatsApp
            //
            this.iconeWhatsApp.BackColor = System.Drawing.Color.Transparent;
            this.iconeWhatsApp.Location = new System.Drawing.Point(16, 12);
            this.iconeWhatsApp.Name = "iconeWhatsApp";
            this.iconeWhatsApp.Size = new System.Drawing.Size(36, 36);
            this.iconeWhatsApp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconeWhatsApp.TabIndex = 1;
            this.iconeWhatsApp.TabStop = false;
            //
            // chkWhatsAppAtivado
            //
            this.chkWhatsAppAtivado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkWhatsAppAtivado.Checked = false;
            this.chkWhatsAppAtivado.Location = new System.Drawing.Point(360, 16);
            this.chkWhatsAppAtivado.Name = "chkWhatsAppAtivado";
            this.chkWhatsAppAtivado.Size = new System.Drawing.Size(62, 30);
            this.chkWhatsAppAtivado.TabIndex = 2;
            //
            // lblEstadoWhatsApp
            //
            this.lblEstadoWhatsApp.AutoSize = true;
            this.lblEstadoWhatsApp.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoWhatsApp.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblEstadoWhatsApp.Location = new System.Drawing.Point(16, 64);
            this.lblEstadoWhatsApp.Name = "lblEstadoWhatsApp";
            this.lblEstadoWhatsApp.Size = new System.Drawing.Size(51, 15);
            this.lblEstadoWhatsApp.TabIndex = 3;
            this.lblEstadoWhatsApp.Text = "Inativo";
            //
            // lblDescricaoWhatsApp
            //
            this.lblDescricaoWhatsApp.AutoSize = true;
            this.lblDescricaoWhatsApp.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescricaoWhatsApp.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblDescricaoWhatsApp.Location = new System.Drawing.Point(16, 88);
            this.lblDescricaoWhatsApp.Name = "lblDescricaoWhatsApp";
            this.lblDescricaoWhatsApp.Size = new System.Drawing.Size(236, 15);
            this.lblDescricaoWhatsApp.TabIndex = 4;
            this.lblDescricaoWhatsApp.Text = "Ative para verificar a conexão e o login.";
            //
            // btnAbrirWhatsApp
            //
            this.btnAbrirWhatsApp.BackColor = System.Drawing.Color.FromArgb(25, 86, 196);
            this.btnAbrirWhatsApp.CornerRadius = 7;
            this.btnAbrirWhatsApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbrirWhatsApp.FlatAppearance.BorderSize = 0;
            this.btnAbrirWhatsApp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(18, 61, 140);
            this.btnAbrirWhatsApp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(22, 73, 168);
            this.btnAbrirWhatsApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirWhatsApp.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirWhatsApp.ForeColor = System.Drawing.Color.White;
            this.btnAbrirWhatsApp.Location = new System.Drawing.Point(16, 116);
            this.btnAbrirWhatsApp.Name = "btnAbrirWhatsApp";
            this.btnAbrirWhatsApp.Size = new System.Drawing.Size(165, 36);
            this.btnAbrirWhatsApp.TabIndex = 5;
            this.btnAbrirWhatsApp.Text = "Abrir WhatsApp Web";
            this.btnAbrirWhatsApp.UseVisualStyleBackColor = false;
            this.btnAbrirWhatsApp.Visible = true;
            //
            // ConfiguracoesForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.painelConteudo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ConfiguracoesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurações";
            ((System.ComponentModel.ISupportInitialize)(this.iconeWhatsApp)).EndInit();
            this.painelConteudo.ResumeLayout(false);
            this.cartaoWhatsApp.ResumeLayout(false);
            this.cartaoWhatsApp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
