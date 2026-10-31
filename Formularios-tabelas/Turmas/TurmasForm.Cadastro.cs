using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Formularios.Turmas.Modelos;

namespace Stingray
{
    public partial class TurmasForm
    {
        // Abre o formulário para adicionar uma nova turma
        private void AoClicarEmNovaTurma(object remetente, EventArgs argumentos)
        {
            AbrirCadastroTurma(-1);
        }

        private void ExcluirTurma(int indiceLinha)
        {
            if (indiceLinha < 0 || indiceLinha >= dgvTurmas.Rows.Count)
                return;

            string modalidade = Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells["colModalidade"].Value) ?? "esta turma";
            DialogResult confirmacao = MessageBox.Show(
                this,
                "Deseja realmente excluir a turma " + modalidade + "?",
                "Excluir turma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao != DialogResult.Yes)
                return;

            dgvTurmas.Rows.RemoveAt(indiceLinha);
            AplicarFiltrosTabela();
        }

        // Abre o formulário para cadastrar ou editar a turma selecionada
        private void AbrirCadastroTurma(int indiceLinha)
        {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvTurmas.Rows.Count;
            DataGridViewRow linhaAtual = editando ? dgvTurmas.Rows[indiceLinha] : null;
            string diaAtual = editando ? ObterValorDaCelula(linhaAtual, "colDiaSemana") : string.Empty;
            string modalidadeAtual = editando ? ObterValorDaCelula(linhaAtual, "colModalidade") : string.Empty;
            string horarioAtual = editando ? ObterValorDaCelula(linhaAtual, "colHorario") : string.Empty;
            string professorAtual = editando ? ObterValorDaCelula(linhaAtual, "colProfessor") : string.Empty;
            RegistroAlunosTurma registroAtual = editando ? linhaAtual.Tag as RegistroAlunosTurma : null;
            string quantidadeAlunosAtual = ObterQuantidadeAtualDeAlunos(linhaAtual, registroAtual, editando);

            using (Form dialogo = CriarDialogoCadastro(editando))
            {
                Label titulo = CriarTituloCadastro(editando);
                Label descricao = CriarDescricaoCadastro(editando);
                ComboBox dia = CriarComboDialogo(
                    dialogo,
                    "Dia da Semana",
                    82,
                    180,
                    new string[]
                    {
                        "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira",
                        "Sexta-feira", "Sábado", "Domingo"
                    });
                TextBox horario = CriarCampoDialogo(dialogo, "Horário (ex: 14:00 - 15:00)", 82, 180, 240);
                ComboBox modalidade = CriarComboDialogo(
                    dialogo,
                    "Modalidade",
                    132,
                    420,
                    new string[]
                    {
                        "Hidroginástica", "Natação A", "Natação (8 a 14 anos)", "Natação (5 a 7 anos)"
                    });
                TextBox professor = CriarCampoDialogo(dialogo, "Professor", 182, 420);
                TextBox alunos = CriarCampoDialogo(dialogo, "Total de Alunos", 232, 420);

                PreencherCamposDoCadastro(
                    editando,
                    dia,
                    modalidade,
                    horario,
                    professor,
                    alunos,
                    diaAtual,
                    modalidadeAtual,
                    horarioAtual,
                    professorAtual,
                    quantidadeAlunosAtual);

                RoundedButton cancelar = CriarBotaoCancelarCadastro(editando);
                RoundedButton salvar = CriarBotaoSalvarCadastro(editando);
                salvar.Click += delegate
                {
                    if (!ValidarCadastroTurma(dialogo, dia, modalidade))
                        return;

                    SalvarCadastroTurma(
                        editando,
                        linhaAtual,
                        registroAtual,
                        dia,
                        modalidade,
                        horario,
                        professor,
                        alunos);

                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };

                dialogo.Controls.AddRange(new Control[] { titulo, descricao, cancelar, salvar });
                dialogo.AcceptButton = salvar;
                dialogo.CancelButton = cancelar;
                dialogo.Shown += delegate { dia.Focus(); };
                dialogo.ShowDialog(this);
            }
        }

        // Cria a janela utilizada para cadastrar ou editar uma turma
        private Form CriarDialogoCadastro(bool editando)
        {
            return new Form
            {
                Text = editando ? "Editar turma" : "Nova turma",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                BackColor = Color.FromArgb(247, 249, 252),
                ClientSize = new Size(480, 420),
                Font = new Font("Segoe UI", 9F)
            };
        }

        // Cria o título da janela de cadastro da turma
        private Label CriarTituloCadastro(bool editando)
        {
            return new Label
            {
                Text = editando ? "Editar turma" : "Nova turma",
                Font = new Font("Segoe UI Semibold", 14F),
                ForeColor = Color.FromArgb(17, 24, 39),
                AutoSize = true,
                Location = new Point(22, 18)
            };
        }

        // Cria a descrição da janela de cadastro da turma
        private Label CriarDescricaoCadastro(bool editando)
        {
            return new Label
            {
                Text = editando ? "Atualize os dados desta turma." : "Preencha os dados da nova turma.",
                ForeColor = VisualPadrao.TextoSecundario,
                AutoSize = true,
                Location = new Point(23, 48)
            };
        }

        // Preenche os campos do formulário com os dados da turma
        private void PreencherCamposDoCadastro(
            bool editando,
            ComboBox dia,
            ComboBox modalidade,
            TextBox horario,
            TextBox professor,
            TextBox alunos,
            string diaAtual,
            string modalidadeAtual,
            string horarioAtual,
            string professorAtual,
            string quantidadeAlunosAtual)
        {
            if (editando)
            {
                dia.SelectedItem = diaAtual;
                modalidade.SelectedItem = modalidadeAtual;
            }

            horario.Text = horarioAtual;
            professor.Text = professorAtual;
            alunos.Text = quantidadeAlunosAtual;
        }

        private RoundedButton CriarBotaoCancelarCadastro(bool editando)
        {
            RoundedButton botao = new RoundedButton
            {
                Text = "Cancelar",
                CornerRadius = 5,
                Size = new Size(100, 36),
                Location = new Point(editando ? 218 : 250, 310),
                BackColor = Color.White,
                ForeColor = VisualPadrao.TextoPrincipal,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            botao.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            botao.FlatAppearance.BorderSize = 1;
            return botao;
        }

        private RoundedButton CriarBotaoSalvarCadastro(bool editando)
        {
            RoundedButton botao = new RoundedButton
            {
                Text = editando ? "Salvar alterações" : "Adicionar",
                CornerRadius = 5,
                Size = new Size(editando ? 142 : 112, 36),
                Location = new Point(editando ? 328 : 358, 310),
                BackColor = VisualPadrao.Azul,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            return botao;
        }

        private bool ValidarCadastroTurma(Form dialogo, ComboBox dia, ComboBox modalidade)
        {
            if (dia.SelectedIndex == -1)
            {
                MessageBox.Show(dialogo, "Selecione o dia da semana.", "Cadastro de turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dia.Focus();
                return false;
            }

            if (modalidade.SelectedIndex == -1)
            {
                MessageBox.Show(dialogo, "Selecione a modalidade.", "Cadastro de turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                modalidade.Focus();
                return false;
            }

            return true;
        }

        private void SalvarCadastroTurma(
            bool editando,
            DataGridViewRow linhaAtual,
            RegistroAlunosTurma registroAtual,
            ComboBox dia,
            ComboBox modalidade,
            TextBox horario,
            TextBox professor,
            TextBox alunos)
        {
            string diaEscolhido = dia.SelectedItem.ToString();
            string modalidadeEscolhida = modalidade.SelectedItem.ToString();

            if (editando)
            {
                linhaAtual.Cells["colDiaSemana"].Value = diaEscolhido;
                linhaAtual.Cells["colModalidade"].Value = modalidadeEscolhida;
                linhaAtual.Cells["colHorario"].Value = horario.Text.Trim();
                linhaAtual.Cells["colProfessor"].Value = professor.Text.Trim();
                AtualizarQuantidadeDeAlunos(linhaAtual, registroAtual, alunos.Text.Trim());
            }
            else
            {
                RegistroAlunosTurma novoRegistro = CriarRegistroNumerico(alunos.Text.Trim());
                int novaLinha = dgvTurmas.Rows.Add(
                    diaEscolhido,
                    modalidadeEscolhida,
                    horario.Text.Trim(),
                    professor.Text.Trim(),
                    "0");
                dgvTurmas.Rows[novaLinha].Tag = novoRegistro;
                dgvTurmas.Rows[novaLinha].Cells["colAlunos"].ToolTipText =
                    "0 presentes de " + novoRegistro.Alunos.Count.ToString() + " alunos";
            }

            AplicarFiltrosTabela();
        }

        private void AtualizarQuantidadeDeAlunos(DataGridViewRow linha, RegistroAlunosTurma registroAtual, string quantidadeTexto)
        {
            int quantidadeDesejada;
            if (!int.TryParse(quantidadeTexto, out quantidadeDesejada) || quantidadeDesejada < 0 || quantidadeDesejada > 100)
                return;

            RegistroAlunosTurma registro = registroAtual ?? CriarRegistroNumerico(Convert.ToString(linha.Cells["colAlunos"].Value));
            while (registro.Alunos.Count < quantidadeDesejada)
            {
                int proximo = registro.Alunos.Count + 1;
                string nomeGenerico = "Aluno " + proximo.ToString();
                while (registro.Alunos.Contains(nomeGenerico))
                {
                    proximo++;
                    nomeGenerico = "Aluno " + proximo.ToString();
                }
                registro.Alunos.Add(nomeGenerico);
            }

            while (registro.Alunos.Count > quantidadeDesejada)
            {
                string removido = registro.Alunos[registro.Alunos.Count - 1];
                registro.Alunos.RemoveAt(registro.Alunos.Count - 1);
                registro.Presentes.Remove(removido);
            }

            linha.Tag = registro;
            linha.Cells["colAlunos"].Value = registro.Presentes.Count.ToString();
            linha.Cells["colAlunos"].ToolTipText =
                registro.Presentes.Count.ToString() + " presentes de " + registro.Alunos.Count.ToString() + " alunos";
        }

        private string ObterValorDaCelula(DataGridViewRow linha, string nomeColuna)
        {
            return Convert.ToString(linha.Cells[nomeColuna].Value) ?? string.Empty;
        }

        private string ObterQuantidadeAtualDeAlunos(DataGridViewRow linha, RegistroAlunosTurma registro, bool editando)
        {
            if (!editando)
                return string.Empty;

            return registro != null
                ? registro.Alunos.Count.ToString()
                : ObterValorDaCelula(linha, "colAlunos");
        }

        private TextBox CriarCampoDialogo(Form dialogo, string legenda, int y, int largura, int x = 24)
        {
            Label rotulo = new Label
            {
                Text = legenda,
                AutoSize = true,
                Location = new Point(x, y),
                ForeColor = VisualPadrao.TextoPrincipal
            };
            TextBox campo = new TextBox
            {
                Name = "txt" + legenda.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(":", "").Replace("/", ""),
                Location = new Point(x, y + 19),
                Width = largura,
                Height = 25,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F)
            };

            dialogo.Controls.Add(rotulo);
            dialogo.Controls.Add(campo);
            return campo;
        }

        private ComboBox CriarComboDialogo(Form dialogo, string legenda, int y, int largura, string[] itens, int x = 24)
        {
            Label rotulo = new Label
            {
                Text = legenda,
                AutoSize = true,
                Location = new Point(x, y),
                ForeColor = VisualPadrao.TextoPrincipal
            };
            ComboBox combo = new ComboBox
            {
                Name = "cmb" + legenda.Replace(" ", ""),
                Location = new Point(x, y + 19),
                Width = largura,
                Height = 25,
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            combo.Items.AddRange(itens);

            dialogo.Controls.Add(rotulo);
            dialogo.Controls.Add(combo);
            return combo;
        }
    }
}