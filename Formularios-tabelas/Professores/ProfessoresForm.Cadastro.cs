using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    public partial class ProfessoresForm : Form {
        // Carrega os professores utilizados como dados de demonstração
        private void PreencherProfessoresDeExemplo() {
            // Registros de demonstração/teste
            AdicionarProfessorDeExemplo("Mariana Costa", "CREF 000001-G/SP", "(11) 90000-0001", "14/03/1987", "Natação Iniciante", "Natação Adulto");
            AdicionarProfessorDeExemplo("Rafael Mendes", "CREF 000002-G/SP", "(11) 90000-0002", "22/07/1990", "Natação Avançado", "Natação Competição");
            AdicionarProfessorDeExemplo("Camila Ferreira", "CREF 000003-G/SP", "(11) 90000-0003", "09/11/1984", "Hidroginástica");
            AdicionarProfessorDeExemplo("Lucas Almeida", "CREF 000004-G/SP", "(11) 90000-0004", "30/01/1992", "Natação Infantil", "Natação (5 a 7 anos)");
            AdicionarProfessorDeExemplo("Patrícia Souza", "CREF 000005-G/SP", "(11) 90000-0005", "05/06/1986", "Natação Intermediário", "Natação Iniciante");
            AdicionarProfessorDeExemplo("Bruno Oliveira", "CREF 000006-G/SP", "(11) 90000-0006", "18/09/1989", "Natação Adulto");
            AplicarFiltrosTabela();
        }

        // Adiciona um professor à lista de demonstração
        private void AdicionarProfessorDeExemplo(string nome, string cref, string telefone, string nascimento, params string[] turmas) {
            int indice = dgvProfessores.Rows.Add(nome, cref, telefone, nascimento, ResumoTurmas(turmas), "");
            dgvProfessores.Rows[indice].Tag = new List<string>(turmas);
        }

        // Separa as informações das turmas para exibição
        private static List<string> SepararTurmas(string textoTurmas) {
            List<string> resultado = new List<string>();
            if (string.IsNullOrWhiteSpace(textoTurmas))
                return resultado;
            foreach (string item in textoTurmas.Split(new char[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)) {
                string turma = item.Trim();
                bool existe = false;
                foreach (string cadastrada in resultado) {
                    if (string.Equals(cadastrada, turma, StringComparison.CurrentCultureIgnoreCase)) {
                        existe = true;
                        break;
                    }
                }
                if (turma.Length > 0 && !existe)
                    resultado.Add(turma);
            }
            return resultado;
        }

        // Monta o resumo das turmas associadas ao professor
        private static string ResumoTurmas(IEnumerable<string> turmas) {
            int quantidade = turmas == null ? 0 : new List<string>(turmas).Count;
            return quantidade == 1
                ? "1 turma"
                : quantidade.ToString() + " turmas";
        }

        private void MostrarTurmasDoProfessor(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvProfessores.Rows.Count)
                return;

            DataGridViewRow linha = dgvProfessores.Rows[indiceLinha];
            string nome = Convert.ToString(linha.Cells[0].Value) ?? "Professor";
            List<string> turmas = linha.Tag as List<string> ?? new List<string>();

            using (Form dialogo = new Form()) {
                dialogo.Text = "Turmas do professor";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.White;
                dialogo.Font = new Font("Segoe UI", 9F);
                dialogo.ClientSize = new Size(440, 350);

                Label titulo = new Label { Text = nome, Font = new Font("Segoe UI Semibold", 14F), ForeColor = Color.FromArgb(17, 24, 39), AutoSize = true, Location = new Point(22, 18) };
                Label descricao = new Label { Text = "Turmas que este professor leciona", ForeColor = VisualPadrao.TextoSecundario, AutoSize = true, Location = new Point(23, 49) };
                ListBox lista = new ListBox { Location = new Point(22, 82), Size = new Size(396, 208), Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle, IntegralHeight = false };
                foreach (string turma in turmas) lista.Items.Add(turma);
                if (lista.Items.Count == 0)
                    lista.Items.Add("Nenhuma turma cadastrada");
                RoundedButton fechar = new RoundedButton { Text = "Fechar", CornerRadius = 5, Size = new Size(100, 34), Location = new Point(318, 304), BackColor = Color.White, ForeColor = VisualPadrao.TextoPrincipal, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                fechar.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                fechar.FlatAppearance.BorderSize = 1;
                fechar.DialogResult = DialogResult.Cancel;
                dialogo.Controls.AddRange(new Control[] { titulo, descricao, lista, fechar });
                dialogo.CancelButton = fechar;
                dialogo.ShowDialog(this);
            }
        }

        // Abre o formulário para cadastrar ou editar um professor
        private void AbrirCadastroProfessor(int indiceLinha) {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvProfessores.Rows.Count;
            string nomeAtual = editando
                ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[0].Value)
                ?? string.Empty
                : string.Empty;
            string crefAtual = editando
                ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[1].Value)
                ?? string.Empty
                : string.Empty;
            string telefoneAtual = editando
                ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[2].Value)
                ?? string.Empty
                : string.Empty;
            string nascimentoAtual = editando
                ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[3].Value)
                ?? string.Empty
                : string.Empty;
            List<string> turmasAtuais = editando
                ? (dgvProfessores.Rows[indiceLinha].Tag as List<string>
                ?? new List<string>())
                : new List<string>();

            using (Form dialogo = new Form()) {
                dialogo.Text = editando
                    ? "Editar professor"
                    : "Novo professor";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.FromArgb(247, 249, 252);
                dialogo.ClientSize = new Size(480, 430);
                dialogo.Font = new Font("Segoe UI", 9F);

                Label titulo = new Label {
                    Text = editando
                        ? "Editar professor"
                        : "Adicionar professor",Font = new Font("Segoe UI Semibold", 14F), ForeColor = Color.FromArgb(17, 24, 39), AutoSize = true, Location = new Point(22, 18) };
                Label descricao = new Label {
                    Text = editando
                    ? "Atualize os dados deste docente."
                    : "Preencha os dados do novo docente.", ForeColor = VisualPadrao.TextoSecundario, AutoSize = true, Location = new Point(23, 48) };
                TextBox nome = CriarCampoDialogo(dialogo, "Nome", 82, 420);
                TextBox cref = CriarCampoDialogo(dialogo, "CREF", 132, 420);
                MaskedTextBox telefone = new MaskedTextBox("(00) 00000-0000") { Location = new Point(22, 202), Size = new Size(200, 25), Font = new Font("Segoe UI", 9F), BorderStyle = BorderStyle.FixedSingle, TextMaskFormat = MaskFormat.IncludeLiterals };
                TextBox nascimento = CriarCampoDialogo(dialogo, "Data de nascimento (dd/MM/aaaa)", 232, 200);
                CheckedListBox turmas = new CheckedListBox { Location = new Point(22, 307), Size = new Size(420, 62), CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F), IntegralHeight = false };
                Label lblTurmas = new Label { Text = "Turmas", Location = new Point(22, 282), AutoSize = true, ForeColor = VisualPadrao.TextoSecundario };
                foreach (string turmaDisponivel in new[] { "Hidroginástica", "Natação Iniciante", "Natação Intermediário", "Natação Avançado", "Natação Adulto", "Natação Master", "Natação Competição" }) { int idx = turmas.Items.Add(turmaDisponivel); if (turmasAtuais.Contains(turmaDisponivel)) turmas.SetItemChecked(idx, true); }
                nome.Text = nomeAtual;
                cref.Text = crefAtual;
                telefone.Text = telefoneAtual;
                telefone.TextMaskFormat = MaskFormat.IncludeLiterals;
                nascimento.Text = nascimentoAtual;

                RoundedButton cancelar = new RoundedButton { Text = "Cancelar", CornerRadius = 5, Size = new Size(100, 36), Location = new Point(editando ? 218 : 250, 378), BackColor = Color.White, ForeColor = VisualPadrao.TextoPrincipal, Cursor = Cursors.Hand };
                cancelar.FlatStyle = FlatStyle.Flat;
                cancelar.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                cancelar.FlatAppearance.BorderSize = 1;
                cancelar.DialogResult = DialogResult.Cancel;
                RoundedButton salvar = new RoundedButton {
                    Text = editando
                    ? "Salvar alterações"
                    : "Adicionar", CornerRadius = 5, Size = new Size(editando ? 142 : 112, 36), Location = new Point(editando ? 328 : 358, 378), BackColor = VisualPadrao.Azul, ForeColor = Color.White, Cursor = Cursors.Hand };
                salvar.FlatStyle = FlatStyle.Flat;
                salvar.FlatAppearance.BorderSize = 0;
                salvar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
                salvar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
                salvar.Click += delegate {
                    if (string.IsNullOrWhiteSpace(nome.Text)) {
                        MessageBox.Show(dialogo, "Informe o nome do professor.", "Cadastro de professor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        nome.Focus();
                        return;
                    }

                    string dataFormatada = nascimento.Text.Trim();
                    if (dataFormatada.Length > 0) {
                        DateTime data;
                        if (!DateTime.TryParseExact(dataFormatada, new string[] { "dd/MM/yyyy", "d/M/yyyy" }, new CultureInfo("pt-BR"), DateTimeStyles.None, out data)) {
                            MessageBox.Show(dialogo, "Informe a data no formato dd/MM/aaaa.", "Data inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            nascimento.Focus();
                            return;
                        }
                        dataFormatada = data.ToString("dd/MM/yyyy");
                    }

                    List<string> turmasDefinidas = new List<string>();
                    foreach (object item in turmas.CheckedItems) turmasDefinidas.Add(Convert.ToString(item));
                    if (editando) {
                        DataGridViewRow linha = dgvProfessores.Rows[indiceLinha];
                        linha.Cells[0].Value = nome.Text.Trim();
                        linha.Cells[1].Value = cref.Text.Trim();
                        linha.Cells[2].Value = telefone.Text.Trim();
                        linha.Cells[3].Value = dataFormatada;
                        linha.Cells[4].Value = ResumoTurmas(turmasDefinidas);
                        linha.Tag = turmasDefinidas;
                    } else {
                        int novaLinha = dgvProfessores.Rows.Add(nome.Text.Trim(), cref.Text.Trim(), telefone.Text.Trim(), dataFormatada, ResumoTurmas(turmasDefinidas), "");
                        dgvProfessores.Rows[novaLinha].Tag = turmasDefinidas;
                    }

                    AplicarFiltrosTabela();
                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };
                dialogo.Controls.AddRange(new Control[] { titulo, descricao, lblTurmas, turmas, cancelar, salvar });
                dialogo.AcceptButton = salvar;
                dialogo.CancelButton = cancelar;
                dialogo.Shown += delegate { nome.Focus(); };
                dialogo.ShowDialog(this);
            }
        }

        private void ConfirmarExclusaoProfessor(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvProfessores.Rows.Count)
                return;

            string nome = Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[0].Value) ?? "este professor";
            DialogResult confirmacao = MessageBox.Show(
                this,
                "Deseja realmente excluir " + nome + "?",
                "Excluir professor",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes) {
                dgvProfessores.Rows.RemoveAt(indiceLinha);
                AplicarFiltrosTabela();
            }
        }

        private TextBox CriarCampoDialogo(Form dialogo, string legenda, int y, int largura) {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(24, y), ForeColor = VisualPadrao.TextoPrincipal };
            TextBox campo = new TextBox { Name = "txt" + legenda.Replace(" ", ""), Location = new Point(24, y + 19), Width = largura, Height = 25, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F) };
            dialogo.Controls.Add(label);
            dialogo.Controls.Add(campo);
            return campo;
        }
    }
}