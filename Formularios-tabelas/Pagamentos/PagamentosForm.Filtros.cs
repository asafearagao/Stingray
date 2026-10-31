using System;
using System.Windows.Forms;

namespace Stingray {
    public partial class PagamentosForm {
        // Limpa os filtros e restaura os valores padrão da tela
        private void LimparFiltros() {
            txtAluno.Text = "Digite o nome do aluno...";
            txtAluno.ForeColor = VisualPadrao.TextoPlaceholder;
            placeholderAluno = true;
            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;
            txtPeriodo.Text = "Selecione o período";
            txtPeriodo.ForeColor = VisualPadrao.TextoPlaceholder;
            dataInicialAplicada = null;
            dataFinalAplicada = null;
            dataInicialSelecionada = null;
            dataFinalSelecionada = null;
            if (txtDataInicial != null) {
                txtDataInicial.Text = "Selecione uma data";
                txtDataInicial.ForeColor = VisualPadrao.TextoPlaceholder;
            }
            if (txtDataFinal != null) {
                txtDataFinal.Text = "Selecione uma data";
                txtDataFinal.ForeColor = VisualPadrao.TextoPlaceholder;
            }
            FecharPopupPeriodo();
        }

        private void AplicarPesquisaVisual() {
            //  Integração com o banco dados será feita depois
        }

    }
}