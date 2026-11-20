using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stingray.DadosTela;
using Stingray.Entidades;
using Stingray.Servicos;

namespace Stingray
{

    // Coordena a seleção das aulas e a gravação das chamadas
    public partial class PresencaForm
    {
        private readonly FrequenciaServico frequenciaServico = new FrequenciaServico();
        private int idOcorrenciaAtual;
        private bool carregandoTurmas;

        // Prepara os dados necessários para abrir a chamada
        private void ConfigurarAcessoFrequencia()
        {
            gdvPresenca.Dock = DockStyle.Fill;
            gdvPresenca.Margin = new Padding(0);
            if (!pnlTabela.Controls.Contains(gdvPresenca))
                pnlTabela.Controls.Add(gdvPresenca);

            cmbTurma.SelectedIndexChanged += delegate
            {
                if (!carregandoTurmas)
                    CarregarChamadaSelecionada();
            };
        }

        // Abre a chamada a partir de uma aula selecionada no Dashboard
        public void AbrirAulaDashboard(int idTurma, DateTime data)
        {
            SelecionarData(data);
            foreach (Turma turma in cmbTurma.Items)
            {
                if (turma.IdTurma == idTurma)
                {
                    cmbTurma.SelectedItem = turma;
                    break;
                }
            }
        }

        // Lista as turmas que possuem aula na data escolhida
        private void CarregarTurmasDaData()
        {
            if (ModoDesign.EstaEmModoDesign(this) || IsDisposed)
                return;

            try
            {
                carregandoTurmas = true;
                List<Turma> turmas = frequenciaServico.ListarTurmasDaData(dataSelecionada);
                cmbTurma.Items.Clear();
                foreach (Turma turma in turmas)
                    cmbTurma.Items.Add(turma);

                if (turmas.Count == 0)
                {
                    gdvPresenca.Rows.Clear();
                    idOcorrenciaAtual = 0;
                    AtualizarResumoPresenca();
                    return;
                }

                cmbTurma.SelectedIndex = 0;
            }
            catch (Exception erro)
            {
                cmbTurma.Items.Clear();
                gdvPresenca.Rows.Clear();
                idOcorrenciaAtual = 0;
                AtualizarResumoPresenca();
                MessageBox.Show(this,
                    "Não foi possível carregar as aulas desta data.\r\n\r\n" + erro.Message,
                    "Consulta de frequência", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                carregandoTurmas = false;
            }

            CarregarChamadaSelecionada();
        }

        // Recupera os participantes e as presenças já registradas
        private void CarregarChamadaSelecionada()
        {
            Turma turma = cmbTurma.SelectedItem as Turma;
            gdvPresenca.Rows.Clear();
            idOcorrenciaAtual = 0;
            if (turma == null)
            {
                AtualizarResumoPresenca();
                return;
            }

            try
            {
                idOcorrenciaAtual = frequenciaServico.ObterOuCriarOcorrencia(turma.IdTurma, dataSelecionada);
                List<ParticipantePresenca> participantes = frequenciaServico.ListarParticipantes(turma.IdTurma, dataSelecionada);
                foreach (ParticipantePresenca participante in participantes)
                {
                    int indice = gdvPresenca.Rows.Add(participante.Nome, string.Empty);
                    gdvPresenca.Rows[indice].Tag = participante;
                    AtualizarVisualBotoesSituacao(gdvPresenca.Rows[indice]);
                }
                AtualizarResumoPresenca();
            }
            catch (Exception erro)
            {
                MessageBox.Show(this,
                    "Não foi possível consultar os participantes da aula.\r\n\r\n" + erro.Message,
                    "Consulta de frequência", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Grava as situações de presença da aula selecionada
        private void SalvarChamadaNoBanco()
        {
            Turma turma = cmbTurma.SelectedItem as Turma;
            if (turma == null)
            {
                MessageBox.Show(this, "Selecione uma aula antes de salvar a chamada.",
                    "Salvar chamada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gdvPresenca.EndEdit();
            List<ParticipantePresenca> participantes = new List<ParticipantePresenca>();
            int totalAlunos = 0;
            foreach (DataGridViewRow linha in gdvPresenca.Rows)
            {
                if (linha.IsNewRow)
                    continue;

                totalAlunos++;

                ParticipantePresenca participante = linha.Tag as ParticipantePresenca;
                if (participante == null)
                {
                    MessageBox.Show(this, "Há um aluno sem vínculo de matrícula. Atualize a chamada e tente novamente.",
                        "Chamada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(participante.Situacao))
                    continue;

                participantes.Add(participante);
            }

            try
            {
                if (participantes.Count == 0)
                {
                    MessageBox.Show(this, "Informe ao menos uma presença ou justificativa antes de salvar.",
                        "Chamada vazia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool permiteParcial = frequenciaServico.PermiteJustificativasAntecipadas(turma.IdTurma, dataSelecionada);
                if (!permiteParcial && participantes.Count != totalAlunos)
                {
                    MessageBox.Show(this, "Defina a situação de todos os alunos antes de salvar a chamada.",
                        "Chamada incompleta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                frequenciaServico.SalvarChamada(turma.IdTurma, dataSelecionada, participantes);
                MessageBox.Show(this, "A chamada foi salva no banco de dados.",
                    "Salvar chamada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarChamadaSelecionada();
            }
            catch (Exception erro)
            {
                MessageBox.Show(this, "Não foi possível salvar a chamada.\r\n\r\n" + erro.Message,
                    "Falha ao salvar frequência", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Abre o formulário para agendar uma reposição
        private void AbrirAgendamentoReposicao(ParticipantePresenca participante)
        {
            if (participante == null || idOcorrenciaAtual <= 0)
                return;

            using (Form dialogo = new Form())
            {
                dialogo.Text = "Agendar reposição";
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.ClientSize = new Size(430, 184);
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.Font = new Font("Segoe UI", 9F);
                dialogo.BackColor = Color.White;

                Label titulo = new Label
                {
                    Text = "Escolha uma aula para a reposição de " + participante.Nome,
                    AutoSize = false,
                    Location = new Point(20, 18),
                    Size = new Size(390, 38),
                    Font = new Font("Segoe UI Semibold", 10F),
                    ForeColor = VisualPadrao.TextoPrincipal
                };
                Label lblData = new Label { Text = "Data da reposição", Location = new Point(20, 70), AutoSize = true, ForeColor = VisualPadrao.TextoSecundario };
                DateTimePicker data = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(20, 94),
                    Width = 150,
                    MinDate = DateTime.Today,
                    Value = dataSelecionada.Date >= DateTime.Today ? dataSelecionada.Date.AddDays(7) : DateTime.Today.AddDays(1)
                };
                Label lblTurma = new Label { Text = "Aula de destino", Location = new Point(190, 70), AutoSize = true, ForeColor = VisualPadrao.TextoSecundario };
                ComboBox turmaDestino = new ComboBox
                {
                    Location = new Point(190, 94),
                    Width = 220,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Segoe UI", 9F)
                };
                VisualPadrao.ConfigurarComboPopup(turmaDestino);
                RoundedButton salvar = new RoundedButton
                {
                    Text = "Confirmar reposição",
                    Location = new Point(242, 138),
                    Size = new Size(168, 32),
                    CornerRadius = 6,
                    BackColor = VisualPadrao.Azul,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                salvar.FlatAppearance.BorderSize = 0;
                Button cancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(150, 138),
                    Size = new Size(82, 32),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White
                };

                Action carregarTurmas = delegate
                {
                    turmaDestino.Items.Clear();
                    foreach (Turma item in frequenciaServico.ListarTurmasDaData(data.Value.Date))
                        turmaDestino.Items.Add(item);
                    if (turmaDestino.Items.Count > 0)
                        turmaDestino.SelectedIndex = 0;
                    salvar.Enabled = turmaDestino.Items.Count > 0;
                };
                data.ValueChanged += delegate { carregarTurmas(); };
                salvar.Click += delegate
                {
                    Turma destino = turmaDestino.SelectedItem as Turma;
                    if (destino == null)
                        return;
                    try
                    {
                        frequenciaServico.AgendarReposicao(participante.IdMatricula, idOcorrenciaAtual,
                            destino.IdTurma, data.Value.Date);
                        dialogo.DialogResult = DialogResult.OK;
                        dialogo.Close();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(dialogo, erro.Message, "Agendar reposição", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                cancelar.Click += delegate { dialogo.Close(); };
                dialogo.Controls.AddRange(new Control[] { titulo, lblData, data, lblTurma, turmaDestino, cancelar, salvar });
                EstiloPopup.Aplicar(dialogo);
                carregarTurmas();
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    CarregarChamadaSelecionada();
            }
        }

        private void TratarAcaoLinha(int indiceLinha)
        {
            if (indiceLinha < 0 || indiceLinha >= gdvPresenca.Rows.Count)
                return;

            DataGridViewRow linha = gdvPresenca.Rows[indiceLinha];
            ParticipantePresenca participante = linha.Tag as ParticipantePresenca;
            string situacao = participante == null ? string.Empty : participante.Situacao ?? string.Empty;

            if (participante != null && participante.EhReposicao)
                return;

            if (string.Equals(situacao, "Justificado", StringComparison.OrdinalIgnoreCase))
            {
                if (participante != null && participante.SituacaoReposicao == "Concluida")
                {
                    MessageBox.Show(this, "A reposição desta justificativa já foi concluída. O registro original permanece justificado.",
                        "Reposição concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (participante == null || participante.IdReposicao <= 0)
                {
                    MessageBox.Show(this, "Salve a chamada com a situação Justificado antes de agendar a reposição.",
                        "Agendar reposição", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                AbrirAgendamentoReposicao(participante);
                return;
            }

        }
    }
}
