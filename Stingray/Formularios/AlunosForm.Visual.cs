using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Linq;
using Stingray.DadosTela;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.Servicos;

namespace Stingray {
    // Monta a tela de Alunos e a ficha individual do aluno
    partial class AlunosForm {
        // Estado dos botões de ação da grade
        private int linhaAcaoSobMouseA = -1;
        private int botaoAcaoSobMouseA = -1;
        private int linhaBotaoPressionadoA = -1;
        private int botaoPressionadoA = -1;
        private bool suspendendoFiltrosAlunos;

        // Grade e rodapé

        // Monta os filtros, a grade e os comandos da tela
        private void ConstruirTela() {
            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);
            lblTitulo.Text = "Alunos";

            ConfigurarCamposDeFiltro();
            ConfigurarBotaoLimpar();
            ConfigurarGradeExistente();
            AtualizarLayoutFiltrosAlunos();

            txtBusca.Text = string.Empty;
            txtBusca.ForeColor = VisualPadrao.TextoPrincipal;
            DefinirPlaceholder(txtBusca, "Digite o nome do aluno...");

            if (!emDesignTime) {
                btnNovoAluno.Click += AbrirDialogoNovoAluno;
                btnLimpar.Click += delegate { LimparFiltrosAlunos(); };

                txtBusca.TextChanged += delegate { if (!suspendendoFiltrosAlunos) AplicarFiltrosAlunos(); };
                cboFiltroTurma.SelectedIndexChanged += delegate { if (!suspendendoFiltrosAlunos) AplicarFiltrosAlunos(); };
                cboFiltroProfessor.SelectedIndexChanged += delegate { if (!suspendendoFiltrosAlunos) AplicarFiltrosAlunos(); };
                cboFiltroStatus.SelectedIndexChanged += delegate { if (!suspendendoFiltrosAlunos) AplicarFiltrosAlunos(); };
            }

            PopularOpcoesFiltrosAlunos();

            if (!emDesignTime)
                CarregarAlunos();
            else
                lblResumo.Text = "Mostrando " + dgvAlunos.Rows.Count.ToString() + " alunos";
        }

        private void AtualizarLayoutFiltrosAlunos() {
            layoutFiltrosAlunos.SuspendLayout();
            layoutFiltrosAlunos.Controls.Clear();
            layoutFiltrosAlunos.ColumnStyles.Clear();
            layoutFiltrosAlunos.RowStyles.Clear();

            layoutFiltrosAlunos.ColumnCount = 5;
            layoutFiltrosAlunos.RowCount = 2;
            layoutFiltrosAlunos.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            layoutFiltrosAlunos.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            layoutFiltrosAlunos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layoutFiltrosAlunos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.33F));
            layoutFiltrosAlunos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.33F));
            layoutFiltrosAlunos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.34F));
            layoutFiltrosAlunos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));

            string[] filtros = { "Nome", "Turma", "Professor", "Status" };
            for (int i = 0; i < filtros.Length; i++) {
                Label titulo = ObterTituloFiltroAluno(filtros[i]);
                Control campo = ObterCampoFiltroAluno(filtros[i]);

                titulo.Dock = DockStyle.Fill;
                campo.Dock = DockStyle.Fill;
                layoutFiltrosAlunos.Controls.Add(titulo, i, 0);
                layoutFiltrosAlunos.Controls.Add(campo, i, 1);
            }

            btnLimpar.Dock = DockStyle.Fill;
            layoutFiltrosAlunos.Controls.Add(btnLimpar, 4, 1);
            layoutFiltrosAlunos.ResumeLayout(true);
        }

        private Label ObterTituloFiltroAluno(string chave) {
            if (chave == "Nome") return lblFiltroNome;
            if (chave == "Turma") return lblFiltroTurma;
            if (chave == "Professor") return lblFiltroProfessor;
            return lblFiltroStatus;
        }

        private Control ObterCampoFiltroAluno(string chave) {
            if (chave == "Nome") return pnlBuscaCampo;
            if (chave == "Turma") return pnlTurmaCampo;
            if (chave == "Professor") return pnlProfessorCampo;
            return pnlStatusCampo;
        }

        // Carrega as opções de turma e professor a partir dos cadastros
        private void PopularOpcoesFiltrosAlunos() {
            suspendendoFiltrosAlunos = true;
            try {
                cboFiltroTurma.Items.Clear();
                cboFiltroProfessor.Items.Clear();
                cboFiltroStatus.Items.Clear();
                cboFiltroTurma.Items.Add(new OpcaoFiltroAluno(0, "Todas"));
                cboFiltroProfessor.Items.Add(new OpcaoFiltroAluno(0, "Todos"));
                cboFiltroStatus.Items.Add("Todos");
                cboFiltroStatus.Items.Add("Ativo");
                cboFiltroStatus.Items.Add("Inativo");

                try {
                    // As opções vêm dos cadastros completos, não dos alunos visíveis na grade
                    foreach (Turma turma in new TurmaServico().ListarTodos()) {
                        // O valor interno continua sendo o ID real da turma; a apresentação
                        // fica restrita ao nome/modalidade e ao dia da semana
                        string nomeTurma = string.IsNullOrWhiteSpace(turma.Modalidade)
                            ? "Turma sem nome"
                            : turma.Modalidade.Trim();
                        string diaSemana = string.IsNullOrWhiteSpace(turma.DiaSemana)
                            ? string.Empty
                            : turma.DiaSemana.Trim();
                        string descricao = string.IsNullOrWhiteSpace(diaSemana)
                            ? nomeTurma
                            : nomeTurma + " · " + diaSemana;
                        cboFiltroTurma.Items.Add(new OpcaoFiltroAluno(turma.IdTurma, descricao));
                    }

                    foreach (Professor professor in new ProfessorServico().ListarTodos()) {
                        // Exibe somente o nome; CREF e outros dados permanecem fora do seletor
                        string nomeProfessor = string.IsNullOrWhiteSpace(professor.NomeExibicao)
                            ? "Nome não cadastrado"
                            : professor.NomeExibicao.Trim();
                        cboFiltroProfessor.Items.Add(new OpcaoFiltroAluno(professor.IdProfessor, nomeProfessor));
                    }
                }
                catch (Exception erro) {
                    MessageBox.Show(this,
                        "Não foi possível carregar as opções de turma e professor diretamente do banco.\r\n\r\n" + erro.Message,
                        "Falha ao carregar filtros", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cboFiltroTurma.SelectedIndex = 0;
                cboFiltroProfessor.SelectedIndex = 0;
                cboFiltroStatus.SelectedIndex = 0;
            }
            finally {
                suspendendoFiltrosAlunos = false;
            }
        }

        // Configura os campos usados para filtrar os alunos
        private void ConfigurarCamposDeFiltro() {
            foreach (ComboBox combo in new ComboBox[] { cboFiltroTurma, cboFiltroProfessor, cboFiltroStatus }) {
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.FlatStyle = FlatStyle.Flat;
                combo.DrawMode = DrawMode.OwnerDrawFixed;
                combo.ItemHeight = 18;
                combo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
                combo.ForeColor = VisualPadrao.TextoPrincipal;
                combo.BackColor = Color.White;
                combo.Margin = new Padding(0);
                combo.DrawItem += ComboFiltroAlunos_DrawItem;
            }

            ConfigurarSetaCombo(btnSetaFiltroTurma, cboFiltroTurma);
            ConfigurarSetaCombo(btnSetaFiltroProfessor, cboFiltroProfessor);
            ConfigurarSetaCombo(btnSetaFiltroStatus, cboFiltroStatus);

            txtBusca.BorderStyle = BorderStyle.None;
            txtBusca.AutoSize = false;
            txtBusca.Dock = DockStyle.Fill;
            txtBusca.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtBusca.BackColor = Color.White;
            txtBusca.Margin = new Padding(0);
        }

        private void ConfigurarSetaCombo(Button seta, ComboBox combo) {
            if (seta == null || combo == null)
                return;

            TableLayoutPanel layout = seta.Parent as TableLayoutPanel;
            Panel superficie = layout == null ? null : layout.Parent as Panel;
            if (layout == null || superficie == null)
                return;

            // O ComboBox ocupa a superfície inteira. O botão fica sobre a área
            // da seta nativa, para que só o chevron do Stingray fique visível
            layout.Controls.Remove(combo);
            layout.Controls.Remove(seta);
            layout.Controls.Clear();
            layout.ColumnStyles.Clear();
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(combo, 0, 0);
            combo.Dock = DockStyle.Fill;

            superficie.Controls.Add(seta);
            seta.Dock = DockStyle.None;
            seta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            seta.Width = 26;
            seta.Height = superficie.ClientSize.Height;
            seta.Location = new Point(Math.Max(0, superficie.ClientSize.Width - seta.Width), 0);
            seta.AccessibleName = "Abrir opções";
            seta.AccessibleDescription = "Abre a lista de opções";
            seta.Visible = true;
            seta.Enabled = true;
            seta.TabStop = false;
            seta.Tag = combo;
            seta.Text = string.Empty;
            seta.Cursor = Cursors.Hand;
            seta.FlatStyle = FlatStyle.Flat;
            seta.FlatAppearance.BorderSize = 0;
            seta.BackColor = Color.White;
            seta.UseVisualStyleBackColor = false;
            seta.Paint -= DesenharSetaFiltroAluno;
            seta.Paint += DesenharSetaFiltroAluno;
            seta.MouseEnter += delegate { seta.BackColor = Color.FromArgb(239, 246, 255); seta.Invalidate(); };
            seta.MouseLeave += delegate { seta.BackColor = Color.White; seta.Invalidate(); };
            seta.Click -= AbrirComboFiltroAluno;
            seta.Click += AbrirComboFiltroAluno;

            superficie.SizeChanged += delegate {
                seta.SetBounds(Math.Max(0, superficie.ClientSize.Width - 26), 0, 26,
                    superficie.ClientSize.Height);
            };
            seta.BringToFront();
        }

        private void DesenharSetaFiltroAluno(object sender, PaintEventArgs e) {
            Button seta = sender as Button;
            if (seta == null || seta.ClientSize.Width < 8 || seta.ClientSize.Height < 8)
                return;

            bool sobre = seta.ClientRectangle.Contains(seta.PointToClient(Cursor.Position));
            Color cor = sobre ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                e.Graphics.DrawLine(separador, 0, 1, 0, seta.ClientSize.Height - 2);
            float x = seta.ClientSize.Width / 2F;
            float y = seta.ClientSize.Height / 2F;
            using (Pen caneta = new Pen(cor, 1.6F)) {
                caneta.StartCap = LineCap.Round;
                caneta.EndCap = LineCap.Round;
                e.Graphics.DrawLines(caneta, new PointF[] {
                    new PointF(x - 3.5F, y - 1.5F),
                    new PointF(x, y + 2F),
                    new PointF(x + 3.5F, y - 1.5F)
                });
            }
        }

        private void AbrirComboFiltroAluno(object sender, EventArgs e) {
            Button seta = sender as Button;
            ComboBox combo = seta == null ? null : seta.Tag as ComboBox;
            if (combo == null || combo.IsDisposed)
                return;

            combo.Focus();
            combo.DroppedDown = true;
        }

        private void ComboFiltroAlunos_DrawItem(object sender, DrawItemEventArgs e) {
            ComboBox combo = sender as ComboBox;
            if (combo == null || e.Bounds.Width <= 0 || e.Bounds.Height <= 0) return;
            bool selecionado = combo.DroppedDown && e.Index >= 0 &&
                (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color fundo = selecionado ? Color.FromArgb(239, 246, 255) : Color.White;
            using (Brush pincel = new SolidBrush(fundo)) e.Graphics.FillRectangle(pincel, e.Bounds);
            string valor = e.Index >= 0 && e.Index < combo.Items.Count
                ? Convert.ToString(combo.Items[e.Index])
                : combo.Text;
            TextRenderer.DrawText(e.Graphics, valor, combo.Font,
                Rectangle.Inflate(e.Bounds, -6, 0), VisualPadrao.TextoPrincipal,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        private void ConfigurarBotaoLimpar() {
            btnLimpar.Text = "Limpar";
            btnLimpar.Size = new Size(92, 34);
            btnLimpar.CornerRadius = 5;
            btnLimpar.BackColor = Color.White;
            btnLimpar.ForeColor = VisualPadrao.TextoPrincipal;
            btnLimpar.FlatStyle = FlatStyle.Flat;
            btnLimpar.FlatAppearance.BorderSize = 0;
            btnLimpar.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnLimpar.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            btnLimpar.Cursor = Cursors.Hand;
            btnLimpar.UseVisualStyleBackColor = false;
            btnLimpar.MouseEnter += delegate {
                btnLimpar.ForeColor = Color.FromArgb(185, 28, 28);
                btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
                btnLimpar.BackColor = Color.FromArgb(254, 242, 242);
            };
            btnLimpar.MouseLeave += delegate {
                btnLimpar.ForeColor = VisualPadrao.TextoPrincipal;
                btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                btnLimpar.BackColor = Color.White;
            };
            btnLimpar.MouseDown += delegate {
                btnLimpar.ForeColor = Color.FromArgb(153, 27, 27);
                btnLimpar.BackColor = Color.FromArgb(254, 226, 226);
            };
            btnLimpar.MouseUp += delegate {
                if (btnLimpar.ClientRectangle.Contains(btnLimpar.PointToClient(Cursor.Position))) {
                    btnLimpar.ForeColor = Color.FromArgb(185, 28, 28);
                    btnLimpar.BackColor = Color.FromArgb(254, 242, 242);
                }
            };
        }

        private void ConfigurarGradeExistente() {
            VisualPadrao.ConfigurarGrade(dgvAlunos);
            dgvAlunos.ColumnHeadersHeight = 38;
            dgvAlunos.RowTemplate.Height = 38;
            dgvAlunos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAlunos.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgvAlunos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAlunos.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            // Prioriza a coluna Turmas sem reduzir a largura reservada às Ações
            if (dgvAlunos.Columns.Contains("colData")) {
                dgvAlunos.Columns["colData"].FillWeight = 12F;
                dgvAlunos.Columns["colData"].MinimumWidth = 85;
            }
            if (dgvAlunos.Columns.Contains("colAluno")) {
                dgvAlunos.Columns["colAluno"].FillWeight = 22F;
                dgvAlunos.Columns["colAluno"].MinimumWidth = 110;
            }
            if (dgvAlunos.Columns.Contains("colTurma")) {
                dgvAlunos.Columns["colTurma"].FillWeight = 34F;
                dgvAlunos.Columns["colTurma"].MinimumWidth = 180;
            }
            if (dgvAlunos.Columns.Contains("colPlano")) {
                dgvAlunos.Columns["colPlano"].FillWeight = 13F;
                dgvAlunos.Columns["colPlano"].MinimumWidth = 95;
            }
            if (dgvAlunos.Columns.Contains("colStatus")) {
                dgvAlunos.Columns["colStatus"].FillWeight = 12F;
                dgvAlunos.Columns["colStatus"].MinimumWidth = 85;
            }
            dgvAlunos.CellFormatting += Grade_CellFormatting;
            dgvAlunos.CellPainting += Grade_CellPainting;
            dgvAlunos.CellMouseMove += Grade_CellMouseMove;
            dgvAlunos.CellMouseLeave += Grade_CellMouseLeave;
            dgvAlunos.CellMouseClick += Grade_CellMouseClick;
            dgvAlunos.CellMouseDown += Grade_CellMouseDown;
            dgvAlunos.CellMouseUp += Grade_CellMouseUp;
            colAcoes.Width = 100;
            colAcoes.MinimumWidth = 100;
            colAcoes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colAcoes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colAcoes.DefaultCellStyle.Padding = new Padding(0);
        }

        private void AbrirDialogoNovoAluno(object sender, EventArgs e) {
            AbrirDialogoAluno(null);
        }

        private void AbrirDialogoAluno(FichaAluno alunoExistente) {
            using (AlunoCadastroForm dialogo = new AlunoCadastroForm(alunoExistente)) {
                if (dialogo.ShowDialog(this) == DialogResult.OK) {
                    PopularOpcoesFiltrosAlunos();
                    CarregarAlunos();
                }
            }
        }

        // Formulário de cadastro de aluno

        // Consulta o CEP sem bloquear a janela

        // Recarrega a grade com os registros do banco
        private void CarregarAlunos() {
            try {
                OpcaoFiltroAluno opcaoTurma = cboFiltroTurma.SelectedItem as OpcaoFiltroAluno;
                OpcaoFiltroAluno opcaoProfessor = cboFiltroProfessor.SelectedItem as OpcaoFiltroAluno;
                int idTurma = opcaoTurma == null ? 0 : opcaoTurma.Id;
                int idProfessor = opcaoProfessor == null ? 0 : opcaoProfessor.Id;
                string situacao = cboFiltroStatus.SelectedIndex > 0
                    ? Convert.ToString(cboFiltroStatus.SelectedItem) : string.Empty;
                string busca = txtBusca == null ? string.Empty : txtBusca.Text.Trim();

                // Consulta apenas os alunos que atendem aos filtros no próprio MySQL
                List<Aluno> alunos = new AlunoServico().ListarFiltrados(
                    idTurma, idProfessor, situacao, busca);

                dgvAlunos.Rows.Clear();

                foreach (Aluno item in alunos) {
                    FichaAluno ficha = new FichaAluno {
                        IdAluno = item.IdAluno,
                        IdEndereco = item.Pessoa.IdEndereco,
                        IdMatricula = item.IdMatricula,
                        Nome = item.Pessoa.Nome,
                        DataNascimento = item.Pessoa.DataNascimento,
                        Telefone = item.Telefone == null ? string.Empty : item.Telefone.NumeroTelefone,
                        Numero = item.Endereco.Numero,
                        Cep = item.Endereco.Cep,
                        Complemento = item.Endereco.Complemento,
                        Responsavel = ExtrairNomeResponsavel(item.Observacao),
                        Cpf = item.Pessoa.Cpf,
                        Rg = item.Pessoa.Rg,
                        Email = item.Pessoa.Email,
                        DataCadastro = item.DataMatricula,
                        DiaVencimento = item.DiaVencimento,
                        Cidade = string.Empty,
                        QuantidadeTurmas = item.QuantidadeTurmas,
                        SituacaoFiltro = item.Situacao ?? string.Empty
                    };

                    string turma = item.QuantidadeTurmas <= 0 || string.IsNullOrWhiteSpace(item.PrimeiraTurma)
                        ? (item.IdMatricula > 0 ? "Sem turma vinculada" : "Sem matrícula")
                        : item.PrimeiraTurma + (item.QuantidadeTurmas > 1 ? "  +" + (item.QuantidadeTurmas - 1) : string.Empty);
                    string professor = string.IsNullOrWhiteSpace(item.Professores)
                        ? "Sem professor vinculado" : item.Professores;
                    string plano = item.QuantidadeTurmas <= 0
                        ? (item.IdMatricula > 0 ? "Sem plano vinculado" : "Sem matrícula")
                        : item.ValorPlano.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
                    string status = item.Situacao == "Ativo" ? "Ativa"
                        : item.Situacao == "Inativo" ? "Inativa"
                        : item.Situacao == "Cancelado" ? "Cancelada"
                        : "Sem matrícula";

                    AdicionarLinhaAluno(ficha, turma, professor, plano, status);
                    if (dgvAlunos.Rows.Count > 0 && item.QuantidadeTurmas > 1)
                        dgvAlunos.Rows[dgvAlunos.Rows.Count - 1].Cells["colTurma"].ToolTipText =
                            "Clique para consultar todas as " + item.QuantidadeTurmas + " turmas e o valor total do plano";
                }

                if (lblResumo != null)
                    lblResumo.Text = "Mostrando " + dgvAlunos.Rows.Count + " alunos";
            }
            catch (Exception erro) {
                dgvAlunos.Rows.Clear();
                MessageBox.Show(this,
                    "Não foi possível carregar os alunos do banco de dados.\r\n\r\n" + erro.Message,
                    "Falha ao consultar alunos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Obtém o nome do responsável registrado no cadastro do aluno
        private static string ExtrairNomeResponsavel(string observacao) {
            const string prefixo = "Responsável:";
            if (string.IsNullOrWhiteSpace(observacao))
                return string.Empty;

            return observacao.StartsWith(prefixo, StringComparison.CurrentCultureIgnoreCase)
                ? observacao.Substring(prefixo.Length).Trim()
                : string.Empty;
        }

        private Rectangle[] ObterRetangulosBotoesAcaoA(Rectangle celula) {
            const int tamanho = 24, espaco = 4;
            int largura = tamanho * 3 + espaco * 2;
            int x = celula.Left + Math.Max(2, (celula.Width - largura) / 2);
            int y = celula.Top + Math.Max(2, (celula.Height - tamanho) / 2);
            return new Rectangle[] { new Rectangle(x, y, tamanho, tamanho), new Rectangle(x + tamanho + espaco, y, tamanho, tamanho), new Rectangle(x + (tamanho + espaco) * 2, y, tamanho, tamanho) };
        }

        private int IndiceBotaoAcaoSobMouseA(int coluna, int linha) {
            if (coluna < 0 || linha < 0) return -1;
            Rectangle celula = dgvAlunos.GetCellDisplayRectangle(coluna, linha, false);
            Point mouse = dgvAlunos.PointToClient(Cursor.Position);
            Point local = new Point(mouse.X - celula.Left, mouse.Y - celula.Top);
            Rectangle[] botoes = ObterRetangulosBotoesAcaoA(new Rectangle(0, 0, celula.Width, celula.Height));
            for (int i = 0; i < botoes.Length; i++) if (botoes[i].Contains(local)) return i;
            return -1;
        }

        private void Grade_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string nomeColuna = dgvAlunos.Columns[e.ColumnIndex].Name;
            if (nomeColuna == "colStatus") {
                using (Brush fundoCelula = new SolidBrush(Color.White)) e.Graphics.FillRectangle(fundoCelula, e.CellBounds);
                using (Pen linha = new Pen(Color.FromArgb(235, 239, 245)))
                    e.Graphics.DrawLine(linha, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                string status = Convert.ToString(e.FormattedValue) ?? string.Empty;
                Color fundoSelo = Color.FromArgb(232, 247, 237);
                Color textoSelo = Color.FromArgb(38, 149, 74);

                if (status == "Inativa" || status == "Pausada") {
                    fundoSelo = Color.FromArgb(255, 244, 217);
                    textoSelo = Color.FromArgb(166, 103, 0);
                }
                else if (status == "Cancelada") {
                    fundoSelo = Color.FromArgb(254, 228, 226);
                    textoSelo = Color.FromArgb(217, 45, 32);
                }
                else if (status == "Sem matrícula") {
                    fundoSelo = Color.FromArgb(241, 245, 249);
                    textoSelo = Color.FromArgb(100, 116, 139);
                }

                using (Font fonteStatus = new Font("Segoe UI", 7.5F)) {
                    int larguraTexto = TextRenderer.MeasureText(status, fonteStatus).Width;
                    int largura = Math.Min(e.CellBounds.Width - 12, Math.Max(44, larguraTexto + 16));
                    Rectangle selo = new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - largura) / 2,
                        e.CellBounds.Top + (e.CellBounds.Height - 22) / 2, largura, 22);
                    using (Brush brush = new SolidBrush(fundoSelo))
                        e.Graphics.FillRectangle(brush, selo);
                    TextRenderer.DrawText(e.Graphics, status, fonteStatus, selo, textoSelo,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                        TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
                }
                e.Handled = true;
                return;
            }
            if (nomeColuna != "colAcoes") return;
            e.PaintBackground(e.CellBounds, true);
            Rectangle[] botoes = ObterRetangulosBotoesAcaoA(e.CellBounds);
            string[] icones = { "\uE721", "\uE70F", "\uE74D" };
            for (int i = 0; i < botoes.Length; i++) {
                bool hover = e.RowIndex == linhaAcaoSobMouseA && botaoAcaoSobMouseA == i;
                bool pressionado = e.RowIndex == linhaBotaoPressionadoA && botaoPressionadoA == i;
                Color fundo;
                Color borda;
                Color icone;
                VisualPadrao.ObterCoresAcao(i == 2, i == 0, hover, pressionado, out fundo, out borda, out icone);
                using (Brush pincel = new SolidBrush(fundo))
                using (Pen caneta = new Pen(borda)) { e.Graphics.FillRectangle(pincel, botoes[i]); e.Graphics.DrawRectangle(caneta, botoes[i]); }
                using (Font fonte = new Font("Segoe MDL2 Assets", 10F))
                    TextRenderer.DrawText(e.Graphics, icones[i], fonte, botoes[i], icone, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            e.Handled = true;
        }

        private void Grade_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvAlunos.Columns[e.ColumnIndex].Name == "colTurma") {
                FichaAluno aluno = dgvAlunos.Rows[e.RowIndex].Tag as FichaAluno;
                dgvAlunos.Cursor = aluno != null && aluno.QuantidadeTurmas > 1 ? Cursors.Hand : Cursors.Default;
                LimparEstadoHoverAcoesSemAlterarCursorA();
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvAlunos.Columns[e.ColumnIndex].Name != "colAcoes") { LimparHoverAcoesA(); return; }
            int botao = IndiceBotaoAcaoSobMouseA(e.ColumnIndex, e.RowIndex);
            if (linhaAcaoSobMouseA == e.RowIndex && botaoAcaoSobMouseA == botao) return;
            LimparHoverAcoesA();
            linhaAcaoSobMouseA = e.RowIndex; botaoAcaoSobMouseA = botao;
            dgvAlunos.Cursor = botao >= 0 ? Cursors.Hand : Cursors.Default;
            dgvAlunos.InvalidateCell(e.ColumnIndex, e.RowIndex);
        }

        private void LimparEstadoHoverAcoesSemAlterarCursorA() {
            int linha = linhaAcaoSobMouseA;
            linhaAcaoSobMouseA = -1;
            botaoAcaoSobMouseA = -1;
            if (linha >= 0 && linha < dgvAlunos.Rows.Count && dgvAlunos.Columns.Contains("colAcoes"))
                dgvAlunos.InvalidateCell(dgvAlunos.Columns["colAcoes"].Index, linha);
        }

        private void Grade_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == linhaAcaoSobMouseA && e.ColumnIndex >= 0 && dgvAlunos.Columns[e.ColumnIndex].Name == "colAcoes") LimparHoverAcoesA();
        }

        private void LimparHoverAcoesA() {
            int linha = linhaAcaoSobMouseA; linhaAcaoSobMouseA = -1; botaoAcaoSobMouseA = -1;
            dgvAlunos.Cursor = Cursors.Default;
            if (linha >= 0 && linha < dgvAlunos.Rows.Count && dgvAlunos.Columns.Contains("colAcoes")) dgvAlunos.InvalidateCell(dgvAlunos.Columns["colAcoes"].Index, linha);
        }

        private void Grade_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvAlunos.Columns[e.ColumnIndex].Name != "colAcoes") return;
            int acao = IndiceBotaoAcaoSobMouseA(e.ColumnIndex, e.RowIndex);
            if (acao < 0) return;
            linhaBotaoPressionadoA = e.RowIndex; botaoPressionadoA = acao;
            dgvAlunos.InvalidateCell(e.ColumnIndex, e.RowIndex);
            dgvAlunos.Cursor = Cursors.Hand;
        }

        private void Grade_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e) {
            int linha = linhaBotaoPressionadoA;
            linhaBotaoPressionadoA = -1; botaoPressionadoA = -1;
            if (linha >= 0 && linha < dgvAlunos.Rows.Count && dgvAlunos.Columns.Contains("colAcoes"))
                dgvAlunos.InvalidateCell(dgvAlunos.Columns["colAcoes"].Index, linha);
        }

        private void Grade_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvAlunos.Columns[e.ColumnIndex].Name == "colTurma" && e.Button == MouseButtons.Left) {
                FichaAluno alunoComTurmas = dgvAlunos.Rows[e.RowIndex].Tag as FichaAluno;
                if (alunoComTurmas != null && alunoComTurmas.QuantidadeTurmas > 1)
                    AbrirTurmasAluno(alunoComTurmas);
                return;
            }

            if (dgvAlunos.Columns[e.ColumnIndex].Name != "colAcoes") return;
            int acao = IndiceBotaoAcaoSobMouseA(e.ColumnIndex, e.RowIndex);
            if (acao == 0) {
                FichaAluno aluno = dgvAlunos.Rows[e.RowIndex].Tag as FichaAluno;
                if (aluno != null) {
                    using (FichaAlunoForm ficha = new FichaAlunoForm(aluno))
                        ficha.ShowDialog(this);
                }
            }
            else if (acao == 1) EditarAluno(e.RowIndex);
            else if (acao == 2) ExcluirAluno(e.RowIndex);
        }

        // Abre o cadastro para alterar o aluno selecionado
        private void EditarAluno(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvAlunos.Rows.Count) return;
            FichaAluno aluno = dgvAlunos.Rows[indiceLinha].Tag as FichaAluno;
            if (aluno == null) return;
            AbrirDialogoAluno(aluno);
        }

        // Solicita a exclusão do aluno e atualiza a grade
        private void ExcluirAluno(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvAlunos.Rows.Count) return;
            string nome = Convert.ToString(dgvAlunos.Rows[indiceLinha].Cells["colAluno"].Value) ?? "este aluno";
            if (MessageBox.Show(this, "Deseja realmente excluir definitivamente " + nome + "?\r\n\r\nTodos os dados, matrículas, mensalidades e registros de frequência desse aluno serão removidos. Esta ação não pode ser desfeita.", "Excluir aluno", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                try {
                    FichaAluno aluno = dgvAlunos.Rows[indiceLinha].Tag as FichaAluno;
                    if (aluno == null || aluno.IdAluno <= 0)
                        return;

                    new AlunoServico().Excluir(aluno.IdAluno);
                    PopularOpcoesFiltrosAlunos();
                    CarregarAlunos();
                }
                catch (Exception erro) {
                    MessageBox.Show(this, "Não foi possível excluir o aluno do banco de dados.\r\n\r\n" + erro.Message,
                        "Falha ao excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // O status é desenhado como uma etiqueta colorida
        private void Grade_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string coluna = dgvAlunos.Columns[e.ColumnIndex].Name;
            if (coluna == "colTurma") {
                FichaAluno ficha = dgvAlunos.Rows[e.RowIndex].Tag as FichaAluno;
                bool variasTurmas = ficha != null && ficha.QuantidadeTurmas > 1;
                e.CellStyle.ForeColor = variasTurmas ? VisualPadrao.Azul : VisualPadrao.TextoPrincipal;
                e.CellStyle.Font = new Font(variasTurmas ? "Segoe UI Semibold" : "Segoe UI",
                    8.25F, FontStyle.Regular, GraphicsUnit.Point);
                return;
            }

            if (coluna != "colStatus")
                return;

            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            e.CellStyle.SelectionBackColor = Color.White;
            e.CellStyle.SelectionForeColor = Color.FromArgb(55, 65, 81);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            e.CellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]

        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private static void DefinirPlaceholder(TextBox txt, string texto) {
            if (txt.IsHandleCreated)
                SendMessage(txt.Handle, EM_SETCUEBANNER, (IntPtr)1, texto);
            else
                txt.HandleCreated += (s, e) => SendMessage(txt.Handle, EM_SETCUEBANNER, (IntPtr)1, texto);
        }

        // Restaura os filtros para a consulta sem restrições
        private void LimparFiltrosAlunos() {
            suspendendoFiltrosAlunos = true;
            try {
                txtBusca.Clear();
                if (cboFiltroTurma.Items.Count > 0) cboFiltroTurma.SelectedIndex = 0;
                if (cboFiltroProfessor.Items.Count > 0) cboFiltroProfessor.SelectedIndex = 0;
                if (cboFiltroStatus.Items.Count > 0) cboFiltroStatus.SelectedIndex = 0;
            }
            finally {
                suspendendoFiltrosAlunos = false;
            }
            AplicarFiltrosAlunos();
        }

        // Atualiza a consulta com os filtros atualmente selecionados
        private void AplicarFiltrosAlunos() {
            if (suspendendoFiltrosAlunos || dgvAlunos == null || dgvAlunos.IsDisposed)
                return;

            CarregarAlunos();
        }

        // Apresenta as turmas vinculadas ao aluno selecionado
        private void AbrirTurmasAluno(FichaAluno aluno) {
            if (aluno == null || aluno.IdMatricula <= 0)
                return;

            try {
                List<TurmaAlunoLinha> turmas = new AlunoServico().ListarTurmasMatricula(aluno.IdMatricula);
                if (turmas.Count == 0) {
                    MessageBox.Show(this, "Este aluno não possui turmas vinculadas à matrícula atual.",
                        "Turmas do aluno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (Form dialogo = new Form()) {
                    dialogo.Text = "Turmas de " + aluno.Nome;
                    dialogo.ClientSize = new Size(650, 330);
                    dialogo.StartPosition = FormStartPosition.CenterParent;
                    dialogo.MinimizeBox = false;
                    dialogo.MaximizeBox = false;

                    Label titulo = new Label {
                        Text = "Turmas vinculadas à matrícula",
                        Font = new Font("Segoe UI Semibold", 12F),
                        ForeColor = VisualPadrao.TextoPrincipal,
                        AutoSize = true,
                        Location = new Point(20, 18)
                    };
                    Label total = new Label {
                        Text = "Valor mensal total: " + turmas.Sum(item => item.ValorMensal).ToString("C2", CultureInfo.GetCultureInfo("pt-BR")),
                        Font = new Font("Segoe UI Semibold", 9F),
                        ForeColor = VisualPadrao.Azul,
                        AutoSize = true,
                        Location = new Point(20, 48)
                    };
                    DataGridView grade = new DataGridView {
                        Location = new Point(20, 82),
                        Size = new Size(610, 190),
                        Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        AllowUserToDeleteRows = false,
                        AllowUserToResizeRows = false,
                        RowHeadersVisible = false,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        MultiSelect = false
                    };
                    VisualPadrao.ConfigurarGrade(grade);
                    grade.Columns.Add("modalidade", "Turma");
                    grade.Columns.Add("dia", "Dia");
                    grade.Columns.Add("horario", "Horário");
                    grade.Columns.Add("professor", "Professor");
                    grade.Columns.Add("valor", "Valor mensal");
                    foreach (TurmaAlunoLinha turma in turmas) {
                        grade.Rows.Add(turma.Modalidade, turma.DiaSemana,
                            turma.HorarioInicio.ToString(@"hh\:mm") + "–" + turma.HorarioFim.ToString(@"hh\:mm"),
                            turma.NomeProfessor,
                            turma.ValorMensal.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")));
                    }
                    Button fechar = new Button {
                        Text = "Fechar",
                        Size = new Size(100, 32),
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                        Location = new Point(530, 284),
                        DialogResult = DialogResult.OK
                    };
                    dialogo.Controls.AddRange(new Control[] { titulo, total, grade, fechar });
                    dialogo.AcceptButton = fechar;
                    EstiloPopup.Aplicar(dialogo);
                    dialogo.ShowDialog(this);
                }
            }
            catch (Exception erro) {
                MessageBox.Show(this, "Não foi possível consultar as turmas do aluno.\r\n\r\n" + erro.Message,
                    "Turmas do aluno", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Adiciona uma linha na grade e guarda a ficha completa do aluno dentro dela (Tag)
        private void AdicionarLinhaAluno(FichaAluno aluno, string turma, string professor, string plano, string status) {
            string dataMatricula = aluno.DataCadastro == DateTime.MinValue
                ? "—"
                : aluno.DataCadastro.ToString("dd/MM/yyyy");
            int indice = dgvAlunos.Rows.Add(dataMatricula, aluno.Nome,
                                                turma, professor, plano, status);
            dgvAlunos.Rows[indice].Tag = aluno;
        }

    }

    // Dados exibidos na ficha do aluno
    public class FichaAluno {
        public int IdAluno { get; set; }
        public int IdEndereco { get; set; }
        public int IdMatricula { get; set; }
        public byte DiaVencimento { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Responsavel { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public int QuantidadeTurmas { get; set; }
        public string SituacaoFiltro { get; set; }
    }

    // Monta a tela de alunos e coordena seus filtros e ações visuais
    internal sealed class OpcaoFiltroAluno {
        public int Id { get; private set; }
        private readonly string texto;

        public OpcaoFiltroAluno(int id, string texto) {
            Id = id;
            this.texto = texto ?? string.Empty;
        }

        public override string ToString() {
            return texto;
        }
    }

}
