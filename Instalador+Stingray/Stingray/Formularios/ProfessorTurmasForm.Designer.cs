namespace Stingray
{
    partial class ProfessorTurmasForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblNomeProfessor;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.ListBox lstTurmas;
        private System.Windows.Forms.Button btnFechar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblNomeProfessor = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lstTurmas = new System.Windows.Forms.ListBox();
            this.btnFechar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblNomeProfessor
            //
            this.lblNomeProfessor.AutoSize = true;
            this.lblNomeProfessor.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblNomeProfessor.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblNomeProfessor.Location = new System.Drawing.Point(22, 18);
            this.lblNomeProfessor.Name = "lblNomeProfessor";
            this.lblNomeProfessor.Size = new System.Drawing.Size(150, 25);
            this.lblNomeProfessor.TabIndex = 0;
            this.lblNomeProfessor.Text = "Nome do professor";
            //
            // lblDescricao
            //
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDescricao.Location = new System.Drawing.Point(23, 49);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(276, 15);
            this.lblDescricao.TabIndex = 1;
            this.lblDescricao.Text = "Turmas vinculadas a este professor no banco de dados";
            //
            // lstTurmas
            //
            this.lstTurmas.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstTurmas.FormattingEnabled = true;
            this.lstTurmas.IntegralHeight = false;
            this.lstTurmas.ItemHeight = 17;
            this.lstTurmas.Location = new System.Drawing.Point(22, 82);
            this.lstTurmas.Name = "lstTurmas";
            this.lstTurmas.Size = new System.Drawing.Size(396, 208);
            this.lstTurmas.TabIndex = 2;
            //
            // btnFechar
            //
            this.btnFechar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Location = new System.Drawing.Point(318, 304);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(100, 34);
            this.btnFechar.TabIndex = 3;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            //
            // ProfessorTurmasForm
            //
            this.AcceptButton = this.btnFechar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnFechar;
            this.ClientSize = new System.Drawing.Size(440, 350);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.lstTurmas);
            this.Controls.Add(this.lblDescricao);
            this.Controls.Add(this.lblNomeProfessor);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfessorTurmasForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Turmas do professor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
