using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Formularios.Turmas.Modelos;

namespace Stingray
{
    public partial class TurmasForm
    {
        // Cria o registro de alunos a partir da quantidade informada
        private RegistroAlunosTurma CriarRegistroNumerico(string quantidadeTexto)
        {
            int quantidade;
            if (!int.TryParse(quantidadeTexto, out quantidade))
                quantidade = 0;

            quantidade = Math.Max(0, Math.Min(quantidade, 100));
            RegistroAlunosTurma registro = new RegistroAlunosTurma();
            for (int indice = 1; indice <= quantidade; indice++)
                registro.Alunos.Add("Aluno " + indice.ToString());

            return registro;
        }

        private void GarantirRegistroAlunos(DataGridViewRow linha)
        {
            if (linha.Tag is RegistroAlunosTurma)
                return;

            linha.Tag = CriarRegistroNumerico(Convert.ToString(linha.Cells["colAlunos"].Value));
        }

        // Abre a edição da lista de alunos da turma
        private void GerirAlunosPresentes(int indiceLinha)
        {
            if (indiceLinha < 0 || indiceLinha >= dgvTurmas.Rows.Count)
                return;

            DataGridViewRow linha = dgvTurmas.Rows[indiceLinha];
            GarantirRegistroAlunos(linha);
            RegistroAlunosTurma registro = (RegistroAlunosTurma)linha.Tag;
            string modalidade = Convert.ToString(linha.Cells["colModalidade"].Value) ?? "Turma";

            using (Form dialogo = CriarDialogoDePresenca())
            {
                CheckedListBox listaAlunos = CriarListaDeAlunos(registro);
                TextBox campoNovoAluno = CriarCampoNovoAluno();
                RoundedButton botaoAdicionar = CriarBotaoAdicionarAluno();
                RoundedButton botaoCancelar = CriarBotaoCancelarPresenca();
                RoundedButton botaoSalvar = CriarBotaoSalvarPresenca();

                botaoAdicionar.Click += delegate
                {
                    AdicionarAlunoNaLista(listaAlunos, campoNovoAluno);
                };

                botaoSalvar.Click += delegate
                {
                    SalvarPresencas(linha, registro, listaAlunos);
                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };

                dialogo.Controls.AddRange(new Control[]
                {
                    CriarTituloPresenca(modalidade),
                    CriarDescricaoPresenca(),
                    listaAlunos,
                    campoNovoAluno,
                    botaoAdicionar,
                    botaoCancelar,
                    botaoSalvar
                });

                dialogo.AcceptButton = botaoSalvar;
                dialogo.CancelButton = botaoCancelar;
                dialogo.ShowDialog(this);
            }
        }

        // Cria a janela utilizada para registrar a presença dos alunos
        private Form CriarDialogoDePresenca()
        {
            return new Form
            {
                Text = "Alunos presentes",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                ClientSize = new Size(520, 500)
            };
        }

        // Cria o título da janela de registro de presença
        private Label CriarTituloPresenca(string modalidade)
        {
            return new Label
            {
                Text = modalidade,
                Font = new Font("Segoe UI Semibold", 14F),
                ForeColor = Color.FromArgb(17, 24, 39),
                AutoSize = true,
                Location = new Point(22, 18)
            };
        }

        // Cria a descrição da janela de registro de presença
        private Label CriarDescricaoPresenca()
        {
            return new Label
            {
                Text = "Marque os alunos que estão presentes nesta turma.",
                ForeColor = VisualPadrao.TextoSecundario,
                AutoSize = true,
                Location = new Point(23, 49)
            };
        }

        // Cria a lista de alunos disponíveis para seleção de presença
        private CheckedListBox CriarListaDeAlunos(RegistroAlunosTurma registro)
        {
            CheckedListBox lista = new CheckedListBox
            {
                Location = new Point(22, 80),
                Size = new Size(476, 310),
                Font = new Font("Segoe UI", 9F),
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle,
                IntegralHeight = false
            };

            foreach (string aluno in registro.Alunos)
                lista.Items.Add(aluno, registro.Presentes.Contains(aluno));

            return lista;
        }

        // Cria o campo utilizado para informar um novo aluno
        private TextBox CriarCampoNovoAluno()
        {
            return new TextBox
            {
                Location = new Point(22, 405),
                Width = 350,
                Height = 26,
                Font = new Font("Segoe UI", 9F)
            };
        }

        private RoundedButton CriarBotaoAdicionarAluno()
        {
            RoundedButton botao = new RoundedButton
            {
                Text = "+ Adicionar",
                CornerRadius = 5,
                Size = new Size(112, 30),
                Location = new Point(386, 403),
                BackColor = Color.FromArgb(239, 246, 255),
                ForeColor = VisualPadrao.Azul,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            botao.FlatAppearance.BorderColor = Color.FromArgb(219, 234, 254);
            botao.FlatAppearance.BorderSize = 1;
            return botao;
        }

        // Cria o botão que cancela a edição da lista de presença
        private RoundedButton CriarBotaoCancelarPresenca()
        {
            RoundedButton botao = new RoundedButton
            {
                Text = "Cancelar",
                CornerRadius = 5,
                Size = new Size(100, 34),
                Location = new Point(286, 450),
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

        private RoundedButton CriarBotaoSalvarPresenca()
        {
            RoundedButton botao = new RoundedButton
            {
                Text = "Salvar presença",
                CornerRadius = 5,
                Size = new Size(124, 34),
                Location = new Point(394, 450),
                BackColor = VisualPadrao.Azul,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            botao.FlatAppearance.BorderSize = 0;
            return botao;
        }

        private void AdicionarAlunoNaLista(CheckedListBox lista, TextBox campoNome)
        {
            string nome = campoNome.Text.Trim();
            if (nome.Length == 0)
                return;

            foreach (object item in lista.Items)
            {
                if (string.Equals(Convert.ToString(item), nome, StringComparison.CurrentCultureIgnoreCase))
                {
                    campoNome.Clear();
                    campoNome.Focus();
                    return;
                }
            }

            lista.Items.Add(nome, true);
            campoNome.Clear();
            campoNome.Focus();
        }

        private void SalvarPresencas(DataGridViewRow linha, RegistroAlunosTurma registro, CheckedListBox lista)
        {
            registro.Alunos.Clear();
            registro.Presentes.Clear();

            for (int indice = 0; indice < lista.Items.Count; indice++)
            {
                string aluno = Convert.ToString(lista.Items[indice]);
                registro.Alunos.Add(aluno);
                if (lista.GetItemChecked(indice))
                    registro.Presentes.Add(aluno);
            }

            linha.Cells["colAlunos"].Value = registro.Presentes.Count.ToString();
            linha.Cells["colAlunos"].ToolTipText =
                registro.Presentes.Count.ToString() + " presentes de " + registro.Alunos.Count.ToString() + " alunos";
            dgvTurmas.InvalidateCell(linha.Cells["colAlunos"]);
        }
    }
}