namespace Stingray
{
    partial class TurmaParticipantesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblModalidade;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblPesquisa;
        private System.Windows.Forms.TextBox txtPesquisa;
        private System.Windows.Forms.CheckedListBox lstParticipantes;
        private System.Windows.Forms.Label lblEstadoVazio;
        private System.Windows.Forms.Label lblResumo;
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
            this.lblModalidade = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblPesquisa = new System.Windows.Forms.Label();
            this.txtPesquisa = new System.Windows.Forms.TextBox();
            this.lstParticipantes = new System.Windows.Forms.CheckedListBox();
            this.lblEstadoVazio = new System.Windows.Forms.Label();
            this.lblResumo = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // Cabeçalho
            //
            this.lblModalidade.AutoSize = true;
            this.lblModalidade.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblModalidade.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblModalidade.Location = new System.Drawing.Point(24, 22);
            this.lblModalidade.Name = "lblModalidade";
            this.lblModalidade.Size = new System.Drawing.Size(80, 25);
            this.lblModalidade.Text = "Turma";
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblDescricao.Location = new System.Drawing.Point(25, 53);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(246, 15);
            this.lblDescricao.Text = "Marque os alunos que participam desta turma.";
            //
            // Pesquisa
            //
            this.lblPesquisa.AutoSize = true;
            this.lblPesquisa.Location = new System.Drawing.Point(24, 88);
            this.lblPesquisa.Name = "lblPesquisa";
            this.lblPesquisa.Size = new System.Drawing.Size(79, 15);
            this.lblPesquisa.Text = "Pesquisar aluno";
            this.txtPesquisa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPesquisa.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPesquisa.Location = new System.Drawing.Point(24, 110);
            this.txtPesquisa.Name = "txtPesquisa";
            this.txtPesquisa.Size = new System.Drawing.Size(512, 24);
            this.txtPesquisa.TabIndex = 0;
            //
            // Lista
            //
            this.lstParticipantes.CheckOnClick = true;
            this.lstParticipantes.DisplayMember = "Nome";
            this.lstParticipantes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstParticipantes.FormattingEnabled = true;
            this.lstParticipantes.IntegralHeight = false;
            this.lstParticipantes.ItemHeight = 26;
            this.lstParticipantes.Location = new System.Drawing.Point(24, 153);
            this.lstParticipantes.Name = "lstParticipantes";
            this.lstParticipantes.Size = new System.Drawing.Size(512, 270);
            this.lstParticipantes.TabIndex = 1;
            //
            // Estado vazio
            //
            this.lblEstadoVazio.BackColor = System.Drawing.Color.White;
            this.lblEstadoVazio.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblEstadoVazio.Location = new System.Drawing.Point(36, 260);
            this.lblEstadoVazio.Name = "lblEstadoVazio";
            this.lblEstadoVazio.Size = new System.Drawing.Size(488, 45);
            this.lblEstadoVazio.TabIndex = 2;
            this.lblEstadoVazio.Text = "Nenhum aluno com matrícula ativa foi encontrado.";
            this.lblEstadoVazio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEstadoVazio.Visible = false;
            //
            // Resumo
            //
            this.lblResumo.AutoSize = true;
            this.lblResumo.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblResumo.Location = new System.Drawing.Point(24, 435);
            this.lblResumo.Name = "lblResumo";
            this.lblResumo.Size = new System.Drawing.Size(220, 15);
            this.lblResumo.Text = "0 alunos com matrícula ativa selecionados";
            //
            // Botões
            //
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Location = new System.Drawing.Point(314, 458);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 36);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnSalvar.BackColor = VisualPadrao.Azul;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(420, 458);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(116, 36);
            this.btnSalvar.TabIndex = 3;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            //
            // Form
            //
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(560, 510);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblResumo);
            this.Controls.Add(this.lblEstadoVazio);
            this.Controls.Add(this.lstParticipantes);
            this.Controls.Add(this.txtPesquisa);
            this.Controls.Add(this.lblPesquisa);
            this.Controls.Add(this.lblDescricao);
            this.Controls.Add(this.lblModalidade);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TurmaParticipantesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alunos participantes";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
