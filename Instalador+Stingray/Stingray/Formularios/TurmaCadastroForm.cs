using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.Servicos;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Controla o cadastro e a edição dos horários e valores de uma turma
    public partial class TurmaCadastroForm : Form
    {
        private readonly Turma turmaAtual;
        private readonly TurmaServico turmaServico = new TurmaServico();
        private readonly ProfessorServico professorServico = new ProfessorServico();
        private readonly List<Professor> professores = new List<Professor>();
        private Professor professorSelecionado;
        private bool atualizandoProfessor;

        public TurmaCadastroForm()
        {
            InitializeComponent();
            EstiloPopup.Aplicar(this);
        }

        public TurmaCadastroForm(Turma turma) : this() // Construtor
        {
            turmaAtual = turma;

            // Define o título e a descrição do formulário, dependendo se é uma edição ou criação
            bool editando = turmaAtual != null && turmaAtual.IdTurma > 0;
            Text = editando ? "Editar turma" : "Nova turma";
            lblTitulo.Text = editando ? "Editar turma" : "Nova turma";
            lblDescricao.Text = editando
                ? "Atualize os dados desta turma."
                : "Preencha os dados da nova turma.";
            btnSalvar.Text = editando ? "Salvar alterações" : "Adicionar";

            ConfigurarCampos();
            PreencherCampos();
            CarregarProfessores();

            txtProfessor.TextChanged += TxtProfessor_TextChanged;
            txtProfessor.KeyDown += TxtProfessor_KeyDown;
            lstSugestoes.Click += delegate { SelecionarProfessor(); };
            lstSugestoes.KeyDown += LstSugestoes_KeyDown;
            btnSalvar.Click += BtnSalvar_Click;
            btnCancelar.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            Shown += delegate { cmbDiaSemana.Focus(); };
        }

        // Prepara os campos e os eventos do cadastro da turma
        private void ConfigurarCampos()
        {
            cmbDiaSemana.FlatStyle = FlatStyle.Standard;
            cmbDiaSemana.BackColor = System.Drawing.Color.White;
            cmbDiaSemana.ForeColor = VisualPadrao.TextoPrincipal;
            ConfigurarFormatacaoHora(txtHorarioInicio);
            ConfigurarFormatacaoHora(txtHorarioFim);
        }

        // Carrega os dados da turma selecionada para edição
        private void PreencherCampos()
        {
            // Se a turma atual for nula, define os campos como vazios
            if (turmaAtual == null)
            {
                cmbDiaSemana.SelectedIndex = 0;
                return;
            }

            int indiceDia = cmbDiaSemana.Items.IndexOf(turmaAtual.DiaSemana);
            // Se o dia da semana não for encontrado, define o índice como -1
            cmbDiaSemana.SelectedIndex = indiceDia >= 0 ? indiceDia : -1;
            // Se a modalidade for nula, define o campo como vazio
            txtModalidade.Text = turmaAtual.Modalidade ?? string.Empty;

            txtHorarioInicio.Text = turmaAtual.HorarioInicio.ToString(@"hh\:mm");
            txtHorarioFim.Text = turmaAtual.HorarioFim.ToString(@"hh\:mm");
            txtCapacidade.Text = turmaAtual.Capacidade.ToString();
            txtValorMensal.Text = turmaAtual.ValorMensal.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));
        }

        // Busca os professores disponíveis para a turma
        private void CarregarProfessores()
        {
            try
            {
                // Limpa a lista de professores e adiciona os professores do banco de dados
                professores.Clear();
                professores.AddRange(professorServico.ListarTodos());

                // Se a turma atual tiver um professor selecionado, seleciona o professor na lista
                if (turmaAtual != null && turmaAtual.IdProfessor > 0)
                {
                    professorSelecionado = professores.FirstOrDefault(
                        item => item.IdProfessor == turmaAtual.IdProfessor);
                    if (professorSelecionado != null)
                    {
                        atualizandoProfessor = true;
                        txtProfessor.Text = professorSelecionado.NomeExibicao;
                        atualizandoProfessor = false;
                    }
                }
            }
            catch (Exception erro)
            {
                erroConsultaProfessores = "Não foi possível consultar o banco: " + erro.Message;
            }
        }

        private string erroConsultaProfessores;

        // Exibe as sugestões de professores conforme o usuário digita no campo de texto
        private void TxtProfessor_TextChanged(object sender, EventArgs e)
        {
            if (atualizandoProfessor)
                return;

            professorSelecionado = null;
            string termo = txtProfessor.Text.Trim();
            lstSugestoes.BeginUpdate();
            try
            {
                // Limpa as sugestões e adiciona os professores que correspondem ao digitado
                lstSugestoes.Items.Clear();
                if (termo.Length > 0 && string.IsNullOrEmpty(erroConsultaProfessores))
                {
                    foreach (Professor professor in professores)
                    {
                        if (professor.NomeExibicao.IndexOf(termo,
                            StringComparison.CurrentCultureIgnoreCase) >= 0)
                            lstSugestoes.Items.Add(professor);
                    }
                }
            }
            finally
            {
                lstSugestoes.EndUpdate();
            }

            // Se o termo estiver vazio, oculta as sugestões
            if (termo.Length == 0)
            {
                OcultarSugestoes();
                return;
            }

            // Se houver erro na consulta dos professores, exibe a mensagem de erro
            if (!string.IsNullOrEmpty(erroConsultaProfessores))
            {
                MostrarEstadoSugestoes(erroConsultaProfessores);
            }
            else if (lstSugestoes.Items.Count == 0)
            {
                MostrarEstadoSugestoes(professores.Count == 0
                    ? "Nenhum professor cadastrado no banco de dados. Cadastre um professor antes de criar a turma."
                    : "Nenhum professor encontrado para essa pesquisa.");
            }
            else
            {
                // Se houver sugestões, exibe a lista de sugestões
                lblEstadoSugestoes.Visible = false;
                lstSugestoes.Visible = true;
                pnlSugestoes.Visible = true;
                pnlSugestoes.BringToFront();
            }
        }

        // Seleciona o professor nas sugestões quando o usuário clica ou da Enter na lista
        private void TxtProfessor_KeyDown(object sender, KeyEventArgs e)
        {
            // Se o usuário pressionar a seta para baixo
            // e houver sugestões, seleciona a primeira sugestão da lista
            if (e.KeyCode == Keys.Down && lstSugestoes.Items.Count > 0)
            {
                lstSugestoes.SelectedIndex = 0;
                lstSugestoes.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            // Se o usuário pressionar Enter e houver sugestões, seleciona o professor da lista
            else if (e.KeyCode == Keys.Enter && lstSugestoes.Items.Count > 0)
            {
                if (lstSugestoes.SelectedIndex < 0)
                    lstSugestoes.SelectedIndex = 0;
                SelecionarProfessor();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            // Se o usuário pressionar Escape, oculta as sugestões
            else if (e.KeyCode == Keys.Escape)
            {
                OcultarSugestoes();
                e.Handled = true;
            }
        }

        // Seleciona o professor nas sugestões quando o usuário clica ou da Enter na lista
        private void LstSugestoes_KeyDown(object sender, KeyEventArgs e)
        {
            // Se o usuário pressionar Enter, seleciona o professor da lista
            if (e.KeyCode == Keys.Enter)
            {
                SelecionarProfessor();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            // Se o usuário pressionar Escape, oculta as sugestões
            else if (e.KeyCode == Keys.Escape)
            {
                OcultarSugestoes();
                txtProfessor.Focus();
                e.Handled = true;
            }
        }

        // Seleciona o professor da lista de sugestões e preenche o campo de texto
        private void SelecionarProfessor()
        {
            // Se não houver sugestão selecionada, não faz nada
            Professor selecionado = lstSugestoes.SelectedItem as Professor;
            if (selecionado == null)
                return;

            // Preenche o campo de texto com o nome do professor selecionado
            professorSelecionado = selecionado;
            atualizandoProfessor = true;
            txtProfessor.Text = selecionado.NomeExibicao;
            txtProfessor.SelectionStart = txtProfessor.Text.Length;
            txtProfessor.SelectionLength = 0;
            atualizandoProfessor = false;

            // Oculta as sugestões e foca o campo de texto
            OcultarSugestoes();
            txtProfessor.Focus();
        }

        private void MostrarEstadoSugestoes(string mensagem)
        {
            lblEstadoSugestoes.Text = mensagem;
            lblEstadoSugestoes.Visible = true;
            lstSugestoes.Visible = false;
            pnlSugestoes.Visible = true;
            pnlSugestoes.BringToFront();
        }

        private void OcultarSugestoes()
        {
            pnlSugestoes.Visible = false;
            lstSugestoes.Visible = false;
            lblEstadoSugestoes.Visible = false;
            lstSugestoes.Items.Clear();
        }

        // Valida os dados e grava a turma
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (cmbDiaSemana.SelectedIndex < 0)
            {
                Avisar("Selecione o dia da semana.", "Cadastro de turma", cmbDiaSemana);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtModalidade.Text) || txtModalidade.Text.Trim().Length > 30)
            {
                Avisar("Informe uma modalidade com até 30 caracteres.", "Modalidade inválida", txtModalidade);
                return;
            }

            // Valida os horários de início e fim da turma
            TimeSpan inicio;
            TimeSpan fim;
            if (!TentarLerHorario(txtHorarioInicio.Text, out inicio))
            {
                Avisar("Informe um horário de início válido, no formato HH:mm.", "Horário inválido", txtHorarioInicio);
                return;
            }
            if (!TentarLerHorario(txtHorarioFim.Text, out fim) || fim <= inicio)
            {
                Avisar("Informe um horário de fim válido e posterior ao início.", "Horário inválido", txtHorarioFim);
                return;
            }

            // Valida a capacidade da turma
            int capacidade;
            if (!int.TryParse(txtCapacidade.Text, out capacidade) || capacidade < 1 || capacidade > 100)
            {
                Avisar("Informe a capacidade da turma entre 1 e 100 alunos.", "Quantidade inválida", txtCapacidade);
                return;
            }

            decimal valorMensal;
            if (!decimal.TryParse(txtValorMensal.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"), out valorMensal) || valorMensal < 0)
            {
                Avisar("Informe um valor mensal válido e não negativo.", "Valor mensal inválido", txtValorMensal);
                return;
            }

            // Valida se o professor selecionado é o mesmo que está no campo de texto
            if (professorSelecionado == null || !string.Equals(
                professorSelecionado.NomeExibicao, txtProfessor.Text.Trim(),
                StringComparison.CurrentCultureIgnoreCase))
            {
                Avisar("Selecione um professor cadastrado na lista de sugestões.", "Professor obrigatório", txtProfessor);
                return;
            }

            Turma turma = new Turma
            {
                IdTurma = turmaAtual == null ? 0 : turmaAtual.IdTurma,
                DiaSemana = Convert.ToString(cmbDiaSemana.SelectedItem),
                Modalidade = txtModalidade.Text.Trim(),
                HorarioInicio = inicio,
                HorarioFim = fim,
                Capacidade = capacidade,
                ValorMensal = valorMensal,
                IdProfessor = professorSelecionado.IdProfessor
            };

            try
            {
                turmaServico.Salvar(turma);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(this,
                    "Não foi possível salvar a turma no banco de dados.\n\n" + erro.Message,
                    "Falha no cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Exibe uma mensagem de aviso e foca no campo informado
        private void Avisar(string mensagem, string titulo, Control campo)
        {
            MessageBox.Show(this, mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            campo.Focus();
        }

        // Campo de horário deve só aceitar apenas números e dois pontos, no formato HH:m
        private static void ConfigurarFormatacaoHora(TextBox campo)
        {
            bool atualizando = false;
            campo.MaxLength = 5;
            campo.TextChanged += delegate
            {
                if (atualizando) return;
                int posicao = Math.Min(campo.SelectionStart, campo.Text.Length);
                int digitosAntes = ExtrairDigitos(campo.Text.Substring(0, posicao)).Length;
                string digitos = ExtrairDigitos(campo.Text);
                if (digitos.Length > 4) digitos = digitos.Substring(0, 4);
                // Formata o texto como HH:mm, adicionando o ":" após os dois primeiros dígitos
                string formatado = digitos.Length <= 2
                    ? digitos
                    : digitos.Substring(0, 2) + ":" + digitos.Substring(2);
                if (campo.Text == formatado) return;
                atualizando = true;
                try
                {
                    campo.Text = formatado;
                    int novaPosicao = 0;
                    int encontrados = 0;
                    while (novaPosicao < formatado.Length && encontrados < digitosAntes)
                    {
                        if (char.IsDigit(formatado[novaPosicao])) encontrados++;
                        novaPosicao++;
                    }
                    if (digitosAntes >= 2 && formatado.Length > 2 && novaPosicao == 2)
                        novaPosicao++;
                    campo.SelectionStart = Math.Min(novaPosicao, campo.Text.Length);
                    campo.SelectionLength = 0;
                }
                finally { atualizando = false; }
            };
        }

        // Extrai apenas os dígitos de uma string, removendo quaisquer outros caracteres
        private static string ExtrairDigitos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            return new string(texto.Where(char.IsDigit).ToArray());
        }

        // Lê um horário no formato HH:mm e retorna um TimeSpan
        private static bool TentarLerHorario(string texto, out TimeSpan horario)
        {
            bool valido = TimeSpan.TryParseExact((texto ?? string.Empty).Trim(),
                @"hh\:mm", CultureInfo.InvariantCulture, out horario);
            return valido && horario.TotalHours < 24;
        }
    }
}
