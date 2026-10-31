using System;
using System.Windows.Forms;

namespace Stingray
{
    public partial class PresencaForm
    {
        private void ConfigurarTabelaPresenca(DataGridView tabela) {
            ConfigurarTabela(tabela);

            DataGridViewComboBoxColumn situacao = tabela.Columns["colStatus"] as DataGridViewComboBoxColumn;
            DataGridViewButtonColumn acao = tabela.Columns["colAcoes"] as DataGridViewButtonColumn;

            if (situacao != null) {
                situacao.MinimumWidth = 110;
                situacao.FillWeight = 19F;
                situacao.FlatStyle = FlatStyle.Flat;
                situacao.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                if (situacao.Items.Count == 0)
                    situacao.Items.AddRange(new object[] { "Presente", "Ausente", "Justificado" });
                situacao.SortMode = DataGridViewColumnSortMode.NotSortable;
                situacao.ReadOnly = false;
            }

            if (acao != null) {
                acao.Text = "Marcar presente";
                acao.UseColumnTextForButtonValue = true;
                acao.FlatStyle = FlatStyle.Flat;
                acao.MinimumWidth = 125;
                acao.FillWeight = 20F;
                acao.SortMode = DataGridViewColumnSortMode.NotSortable;
                acao.ReadOnly = true;
            }

            tabela.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabela.Columns["colAluno"].FillWeight = 30F;
            tabela.Columns["colStatus"].FillWeight = 19F;
            tabela.Columns["colReferencia"].FillWeight = 31F;
            tabela.Columns["colAcoes"].FillWeight = 20F;
            tabela.Columns["colAluno"].MinimumWidth = 150;
            tabela.Columns["colStatus"].MinimumWidth = 110;
            tabela.Columns["colReferencia"].MinimumWidth = 170;
            tabela.Columns["colAcoes"].MinimumWidth = 125;
            foreach (DataGridViewColumn coluna in tabela.Columns)
                coluna.Resizable = DataGridViewTriState.False;

            tabela.ReadOnly = false;
            tabela.EditMode = DataGridViewEditMode.EditOnEnter;
            tabela.Columns["colAluno"].ReadOnly = true;
            tabela.Columns["colStatus"].ReadOnly = false;
            tabela.Columns["colReferencia"].ReadOnly = false;
            tabela.Columns["colAcoes"].ReadOnly = true;
            tabela.Columns["colAluno"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabela.Columns["colAluno"].DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            tabela.Columns["colReferencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabela.Columns["colReferencia"].DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            tabela.DataError += delegate(object sender, DataGridViewDataErrorEventArgs e) { e.ThrowException = false; };
            tabela.CellValueChanged += delegate { AtualizarResumoPresenca(); };
            tabela.CurrentCellDirtyStateChanged += delegate {
                if (tabela.IsCurrentCellDirty)
                    tabela.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            tabela.CellContentClick += delegate(object sender, DataGridViewCellEventArgs e) {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;
                if (tabela.Columns[e.ColumnIndex].Name == "colAcoes")
                    tabela.Rows[e.RowIndex].Cells["colStatus"].Value = "Presente";
            };
            AtualizarResumoPresenca();
        }

        private static void ConfigurarTabela(DataGridView tabela) {
            VisualPadrao.ConfigurarGrade(tabela);
        }
    }
}