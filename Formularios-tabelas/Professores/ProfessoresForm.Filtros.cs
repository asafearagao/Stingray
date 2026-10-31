using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    public partial class ProfessoresForm : Form {
        // Configura o placeholder
        private void ConfigurarPlaceholder(TextBox campo, string placeholder) {
            placeholders[campo] = placeholder;
            campo.Text = placeholder;
            campo.ForeColor = VisualPadrao.TextoSecundario;

            campo.Enter += delegate {
                if (CampoContemPlaceholder(campo)) {
                    campo.Clear();
                    campo.ForeColor = VisualPadrao.TextoPrincipal;
                }
            };
            campo.Leave += delegate {
                if (string.IsNullOrWhiteSpace(campo.Text))
                    RestaurarPlaceholder(campo);
            };
        }

        // Verifica se o campo ainda contém o texto de placeholder
        private bool CampoContemPlaceholder(TextBox campo) {
            string placeholder;
            return placeholders.TryGetValue(campo, out placeholder) &&
                   campo.ForeColor == VisualPadrao.TextoSecundario &&
                   string.Equals(campo.Text, placeholder, StringComparison.Ordinal);
        }

        // Obtém o valor utilizado como filtro
        private string ObterValorFiltro(TextBox campo) {
            return CampoContemPlaceholder(campo)
                ? string.Empty
                : campo.Text.Trim();
        }

        private void RestaurarPlaceholder(TextBox campo) {
            string placeholder;
            if (campo != null && placeholders.TryGetValue(campo, out placeholder)) {
                campo.Text = placeholder;
                campo.ForeColor = VisualPadrao.TextoSecundario;
            }
        }

        // Registra os eventos que aplicam os filtros da tabela
        private void ConfigurarFiltrosTabela() {
            txtNome.TextChanged += delegate { AplicarFiltrosTabela(); };
            txtCref.TextChanged += delegate { AplicarFiltrosTabela(); };
            txtTelefone.TextChanged += delegate { AplicarFiltrosTabela(); };
        }

        private void AplicarFiltrosTabela() {
            if (dgvProfessores == null || dgvProfessores.IsDisposed)
                return;

            string[] filtros = {
                ObterValorFiltro(txtNome),
                ObterValorFiltro(txtCref),
                ObterValorFiltro(txtTelefone)
            };

            int total = dgvProfessores.Rows.Count;
            int exibidos = 0;
            foreach (DataGridViewRow linha in dgvProfessores.Rows) {
                bool visivel = true;
                for (int coluna = 0; coluna < filtros.Length; coluna++) {
                    if (filtros[coluna].Length == 0)
                        continue;
                    string valor = Convert.ToString(linha.Cells[coluna].Value) ?? string.Empty;

                    if (valor.IndexOf(filtros[coluna], StringComparison.CurrentCultureIgnoreCase) < 0) {
                        visivel = false;
                        break;
                    }
                }

                if (!visivel && dgvProfessores.CurrentCell != null && dgvProfessores.CurrentCell.RowIndex == linha.Index) {
                    dgvProfessores.CurrentCell = null;
                }
                linha.Visible = visivel;
                if (visivel)
                    exibidos++;
            }
        }
    }
}