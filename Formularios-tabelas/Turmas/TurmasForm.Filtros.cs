using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Stingray
{
    public partial class TurmasForm
    {
        private readonly Dictionary<TextBox, string> placeholdersTxt = new Dictionary<TextBox, string>();

        // Lista com os dias da semana
        private void PreencherDiasDaSemana(ComboBox combo)
        {
            combo.Items.Clear();
            combo.Items.AddRange(new string[]
            {
                "Selecione...", "Segunda-feira", "Terça-feira", "Quarta-feira",
                "Quinta-feira", "Sexta-feira", "Sábado", "Domingo"
            });
        }

        private void ConfigurarPlaceholderTexto(TextBox campo, string placeholder)
        {
            placeholdersTxt[campo] = placeholder;
            campo.Text = placeholder;
            campo.ForeColor = VisualPadrao.TextoSecundario;

            campo.Enter += AoEntrarNoCampoDeModalidade;
            campo.Leave += AoSairDoCampoDeModalidade;
        }

        private void AoEntrarNoCampoDeModalidade(object remetente, EventArgs argumentos)
        {
            TextBox campo = (TextBox)remetente;
            if (!CampoContemPlaceholder(campo))
                return;

            campo.Clear();
            campo.ForeColor = VisualPadrao.TextoPrincipal;
        }

        // Restaura o placeholder quando o campo de modalidade deseleciona
        private void AoSairDoCampoDeModalidade(object remetente, EventArgs argumentos)
        {
            TextBox campo = (TextBox)remetente;
            if (string.IsNullOrWhiteSpace(campo.Text))
                RestaurarPlaceholder(campo);
        }

        // Verifica se o campo ainda contém o texto de placeholder
        private bool CampoContemPlaceholder(TextBox campo)
        {
            string placeholder;
            return placeholdersTxt.TryGetValue(campo, out placeholder) &&
                   campo.ForeColor == VisualPadrao.TextoSecundario &&
                   string.Equals(campo.Text, placeholder, StringComparison.Ordinal);
        }

        private void RestaurarPlaceholder(TextBox campo)
        {
            string placeholder;
            if (campo == null || !placeholdersTxt.TryGetValue(campo, out placeholder))
                return;

            campo.Text = placeholder;
            campo.ForeColor = VisualPadrao.TextoSecundario;
        }

        // Aplica os filtros quando o texto da modalidade é alterado
        private void AoAlterarTextoDoFiltroDeModalidade(object remetente, EventArgs argumentos)
        {
            AplicarFiltrosTabela();
        }

        // Aplica os filtros quando o dia da semana é alterado
        private void AoAlterarDiaDoFiltro(object remetente, EventArgs argumentos)
        {
            cmbDiaSemana.ForeColor = cmbDiaSemana.SelectedIndex <= 0
                ? VisualPadrao.TextoSecundario
                : VisualPadrao.TextoPrincipal;
            AplicarFiltrosTabela();
        }

        private void AoClicarEmLimparFiltros(object remetente, EventArgs argumentos)
        {
            RestaurarPlaceholder(txtModalidade);
            cmbDiaSemana.SelectedIndex = 0;
            AplicarFiltrosTabela();
        }

        private void AplicarFiltrosTabela()
        {
            if (dgvTurmas == null || dgvTurmas.IsDisposed)
                return;

            string filtroModalidade = CampoContemPlaceholder(txtModalidade)
                ? string.Empty
                : txtModalidade.Text.Trim();
            string filtroDia = cmbDiaSemana.SelectedIndex <= 0
                ? string.Empty
                : (cmbDiaSemana.SelectedItem == null ? string.Empty : cmbDiaSemana.SelectedItem.ToString());

            foreach (DataGridViewRow linha in dgvTurmas.Rows)
            {
                bool visivel = true;

                if (filtroModalidade.Length > 0)
                {
                    string modalidade = Convert.ToString(linha.Cells["colModalidade"].Value) ?? string.Empty;
                    if (modalidade.IndexOf(filtroModalidade, StringComparison.CurrentCultureIgnoreCase) < 0)
                        visivel = false;
                }

                if (visivel && filtroDia.Length > 0)
                {
                    string dia = Convert.ToString(linha.Cells["colDiaSemana"].Value) ?? string.Empty;
                    if (!string.Equals(dia, filtroDia, StringComparison.OrdinalIgnoreCase))
                        visivel = false;
                }

                if (!visivel && dgvTurmas.CurrentCell != null && dgvTurmas.CurrentCell.RowIndex == linha.Index)
                    dgvTurmas.CurrentCell = null;

                linha.Visible = visivel;
            }
        }
    }
}