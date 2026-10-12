namespace Stingray
{
    partial class PresencaForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel painelConteudo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.painelConteudo = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblTitulo.Location = new System.Drawing.Point(14, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(100, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Presença";
            //
            // painelConteudo
            //
            this.painelConteudo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.painelConteudo.BackColor = System.Drawing.Color.White;
            this.painelConteudo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.painelConteudo.Location = new System.Drawing.Point(14, 64);
            this.painelConteudo.Name = "painelConteudo";
            this.painelConteudo.Size = new System.Drawing.Size(1018, 582);
            this.painelConteudo.TabIndex = 1;
            //
            // PresencaForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(250, 251, 252);
            this.ClientSize = new System.Drawing.Size(1050, 660);
            this.Controls.Add(this.painelConteudo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PresencaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Presença";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
