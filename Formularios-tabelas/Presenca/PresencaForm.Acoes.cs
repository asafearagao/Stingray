using System;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray
{
    public partial class PresencaForm
    {
        // Salva os registros da chamada e atualiza o resumo de presença
        private void BtnSalvarChamada_Click(object sender, EventArgs e) {
            gdvPresenca.EndEdit();
            if (gdvPresenca.Rows.Count == 0) {
                MessageBox.Show(this, "Não há alunos carregados para salvar. Selecione uma turma que tenha alunos.",
                    "Salvar chamada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int semSituacao = 0;
            foreach (DataGridViewRow linha in gdvPresenca.Rows) {
                if (linha.IsNewRow)
                    continue;
                if (string.IsNullOrWhiteSpace(Convert.ToString(linha.Cells["colStatus"].Value)))
                    semSituacao++;
            }

            if (semSituacao > 0) {
                MessageBox.Show(this, "Defina a situação de todos os alunos antes de salvar a chamada.",
                    "Chamada incompleta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(this, "Chamada validada e mantida nesta sessão. A gravação permanente depende da integração com o banco de dados.",
                "Salvar chamada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLimparChamada_Click(object sender, EventArgs e) {
            if (gdvPresenca.Rows.Count == 0) {
                MessageBox.Show(this, "Não há alunos carregados para limpar nesta chamada.",
                    "Limpar chamada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult resposta = MessageBox.Show(this,
                "Deseja limpar as situações e observações de todos os alunos desta chamada?",
                "Limpar chamada", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resposta != DialogResult.Yes)
                return;

            foreach (DataGridViewRow linha in gdvPresenca.Rows) {
                if (linha.IsNewRow)
                    continue;
                linha.Cells["colStatus"].Value = null;
                linha.Cells["colReferencia"].Value = string.Empty;
            }
            AtualizarResumoPresenca();
        }

        private void AtualizarResumoPresenca() {
            if (gdvPresenca == null || gdvPresenca.IsDisposed)
                return;

            int presentes = 0;
            int ausentes = 0;
            int justificados = 0;
            int total = 0;

            foreach (DataGridViewRow linha in gdvPresenca.Rows) {
                if (linha.IsNewRow)
                    continue;
                total++;
                string situacao = Convert.ToString(linha.Cells["colStatus"].Value) ?? string.Empty;
                if (string.Equals(situacao, "Presente", StringComparison.OrdinalIgnoreCase))
                    presentes++;
                else if (string.Equals(situacao, "Ausente", StringComparison.OrdinalIgnoreCase))
                    ausentes++;
                else if (string.Equals(situacao, "Justificado", StringComparison.OrdinalIgnoreCase))
                    justificados++;
            }

            lblNumPresentes.Text = presentes.ToString(CultureInfo.InvariantCulture);
            lblNumAusentes.Text = ausentes.ToString(CultureInfo.InvariantCulture);
            lblNumJustificados.Text = justificados.ToString(CultureInfo.InvariantCulture);
            lblNumTotalAlunos.Text = total.ToString(CultureInfo.InvariantCulture);
        }
    }
}