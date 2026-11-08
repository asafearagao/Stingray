using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stingray {
    // Monta a tela de Alunos e a ficha individual do aluno
    partial class AlunosForm {
        // Campos do formulário de novo aluno
        private TextBox txtNomeAluno;
        private DateTimePicker dtpNascimento;
        private MaskedTextBox mtbTelefone;
        private TextBox txtNumero;
        private MaskedTextBox mtbCep;
        private TextBox txtCidade;
        private TextBox txtResponsavel;
        private MaskedTextBox mtbCpf;
        private TextBox txtRg;
        private TextBox txtEmail;
        private Button btnCadastrar;
        private int linhaEdicaoAluno = -1;
        private int linhaAcaoSobMouseA = -1;
        private int botaoAcaoSobMouseA = -1;
        private int linhaBotaoPressionadoA = -1;
        private int botaoPressionadoA = -1;

        // Filtros

        // Grade e rodapé

        private void ConstruirTela() {
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            lblTitulo.Text = "Alunos";

            ConfigurarCamposDeFiltro();
            ConfigurarBotoesDeFiltro();
            ConfigurarGradeExistente();

            if (emDesignTime) {
                txtBusca.Text = "Digite o nome do aluno...";
                txtBusca.ForeColor = VisualPadrao.TextoSecundario;
            }
            else {
                txtBusca.Text = string.Empty;
                txtBusca.ForeColor = VisualPadrao.TextoPrincipal;
                DefinirPlaceholder(txtBusca, "Digite o nome do aluno...");

                btnNovoAluno.Click += AbrirDialogoNovoAluno;
                btnPesquisar.Click += delegate { AplicarFiltrosAlunos(); };
                btnLimpar.Click += delegate { LimparFiltrosAlunos(); };
                txtBusca.TextChanged += delegate { AplicarFiltrosAlunos(); };
                cboFiltroTurma.SelectedIndexChanged += delegate { AplicarFiltrosAlunos(); };
                cboFiltroProfessor.SelectedIndexChanged += delegate { AplicarFiltrosAlunos(); };
                cboFiltroStatus.SelectedIndexChanged += delegate { AplicarFiltrosAlunos(); };
            }

            CarregarDadosDeExemplo();
            if (!emDesignTime)
                AplicarFiltrosAlunos();
            else
                lblResumo.Text = "Mostrando " + dgvAlunos.Rows.Count.ToString() + " alunos";
        }

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

            ConfigurarSetaFiltro(btnSetaFiltroTurma, cboFiltroTurma);
            ConfigurarSetaFiltro(btnSetaFiltroProfessor, cboFiltroProfessor);
            ConfigurarSetaFiltro(btnSetaFiltroStatus, cboFiltroStatus);

            txtBusca.BorderStyle = BorderStyle.None;
            txtBusca.AutoSize = false;
            txtBusca.Dock = DockStyle.Fill;
            txtBusca.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtBusca.BackColor = Color.White;
            txtBusca.Margin = new Padding(0);
        }

        private void ConfigurarSetaFiltro(Button seta, ComboBox combo) {
            seta.AccessibleName = "Abrir opções";
            seta.AccessibleDescription = "Abre a lista de opções";
            seta.Cursor = Cursors.Hand;
            seta.Paint += delegate(object sender, PaintEventArgs e) {
                Rectangle r = seta.ClientRectangle;
                if (r.Width < 8 || r.Height < 8) return;
                bool hover = r.Contains(seta.PointToClient(Cursor.Position));
                Color cor = hover ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                    e.Graphics.DrawLine(separador, 0, 1, 0, r.Height - 2);
                float cx = r.Width / 2F;
                float cy = r.Height / 2F;
                using (Pen pen = new Pen(cor, 1.6F)) {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    e.Graphics.DrawLines(pen, new PointF[] {
                        new PointF(cx - 3.5F, cy - 1.5F),
                        new PointF(cx, cy + 2F),
                        new PointF(cx + 3.5F, cy - 1.5F)
                    });
                }
            };
            seta.MouseEnter += delegate { seta.BackColor = Color.FromArgb(239, 246, 255); seta.Invalidate(); };
            seta.MouseLeave += delegate { seta.BackColor = Color.White; seta.Invalidate(); };
            seta.Click += delegate {
                if (combo.IsDisposed) return;
                combo.Focus();
                combo.DroppedDown = true;
            };
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

        private void ConfigurarBotoesDeFiltro() {
            btnPesquisar.CornerRadius = 5;
            btnPesquisar.BackColor = VisualPadrao.Azul;
            btnPesquisar.ForeColor = Color.White;
            btnPesquisar.FlatStyle = FlatStyle.Flat;
            btnPesquisar.FlatAppearance.BorderSize = 0;
            btnPesquisar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            btnPesquisar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            btnPesquisar.Cursor = Cursors.Hand;
            btnPesquisar.UseVisualStyleBackColor = false;

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
            linhaEdicaoAluno = -1;
            AbrirDialogoAluno(null);
        }

        private void AbrirDialogoAluno(FichaAluno alunoExistente) {
            bool editando = alunoExistente != null;
            using (Form dialogo = new Form()) {
                dialogo.Text = editando ? "Editar aluno" : "Novo Aluno";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.FromArgb(247, 249, 252);
                dialogo.Font = new Font("Segoe UI", 9F);
                dialogo.ClientSize = new Size(920, 500);
                dialogo.MinimumSize = new Size(920, 500);

                Panel areaRolagem = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(16), BackColor = dialogo.BackColor };
                TableLayoutPanel cadastro = (TableLayoutPanel)CriarCardCadastroAluno(editando);
                cadastro.Dock = DockStyle.Top;
                cadastro.Width = 860;
                cadastro.Margin = new Padding(0);
                areaRolagem.Controls.Add(cadastro);
                dialogo.Controls.Add(areaRolagem);

                if (editando) {
                    txtNomeAluno.Text = alunoExistente.Nome ?? string.Empty;
                    dtpNascimento.Value = alunoExistente.DataNascimento < dtpNascimento.MinDate || alunoExistente.DataNascimento > dtpNascimento.MaxDate ? DateTime.Today : alunoExistente.DataNascimento;
                    mtbTelefone.Text = alunoExistente.Telefone ?? string.Empty;
                    txtNumero.Text = alunoExistente.Numero ?? string.Empty;
                    mtbCep.Text = alunoExistente.Cep ?? string.Empty;
                    txtCidade.Text = alunoExistente.Cidade ?? string.Empty;
                    txtResponsavel.Text = string.IsNullOrWhiteSpace(alunoExistente.Responsavel) ? string.Empty : alunoExistente.Responsavel;
                    mtbCpf.Text = alunoExistente.Cpf ?? string.Empty;
                    txtRg.Text = alunoExistente.Rg ?? string.Empty;
                    txtEmail.Text = alunoExistente.Email ?? string.Empty;
                    AtualizarCampoResponsavelPorIdade();
                }
                dialogo.Shown += delegate { txtNomeAluno.Focus(); };
                dialogo.ShowDialog(this);
            }
        }

        // Formulário de cadastro de aluno
        private Control CriarCardCadastroAluno(bool editando = false) {
            // 4 colunas de tamanho igual; campos maiores (nome, endereço...) ocupam 2 colunas
            var card = CriarCard(4);
            for (int i = 0; i < 4; i++)
                card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            var lblCard = new Label
            {
                Text = editando ? "Editar Aluno" : "Novo Aluno",
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = VisualPadrao.TextoPrincipal,
                Margin = new Padding(0, 0, 0, 10)
            };
            card.Controls.Add(lblCard, 0, 0);
            card.SetColumnSpan(lblCard, 4);

            // Cria os campos do formulário
            txtNomeAluno = new TextBox();
            DefinirPlaceholder(txtNomeAluno, "Nome completo do aluno");

            dtpNascimento = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MaxDate = DateTime.Today
            };

            mtbTelefone = CriarMascara(@"(00) 00000-0000");

            txtNumero = new TextBox();
            DefinirPlaceholder(txtNumero, "Número");

            mtbCep = CriarMascara(@"00000-000");

            txtCidade = new TextBox();
            DefinirPlaceholder(txtCidade, "Cidade");

            txtResponsavel = new TextBox();
            DefinirPlaceholder(txtResponsavel, "Nome do responsável");
            dtpNascimento.ValueChanged += delegate { AtualizarCampoResponsavelPorIdade(); };
            AtualizarCampoResponsavelPorIdade();

            // O "\." é necessário: sem a barra, o ponto viraria vírgula no Windows em português
            mtbCpf = CriarMascara(@"000\.000\.000-00");

            txtRg = new TextBox();
            DefinirPlaceholder(txtRg, "RG");

            txtEmail = new TextBox();
            DefinirPlaceholder(txtEmail, "exemplo@email.com");

            // Posiciona os campos do formulário
            // Linhas 1-2: Nome do aluno (2 colunas) | Data de nascimento | Telefone
            AdicionarCampo(card, "Nome do aluno", txtNomeAluno, 0, 1, 2);
            AdicionarCampo(card, "Data de nascimento", dtpNascimento, 2, 1);
            AdicionarCampo(card, "Telefone", mtbTelefone, 3, 1);

            // Linha 3-4: Número | CEP | Cidade
            AdicionarCampo(card, "Número", txtNumero, 0, 3);
            AdicionarCampo(card, "CEP", mtbCep, 1, 3);
            AdicionarCampo(card, "Cidade", txtCidade, 2, 3, 2);

            // Linhas 5-6: Nome do responsável (2 colunas) | CPF | RG
            AdicionarCampo(card, "Nome do responsável", txtResponsavel, 0, 5, 2);
            AdicionarCampo(card, "CPF", mtbCpf, 2, 5);
            AdicionarCampo(card, "RG", txtRg, 3, 5);

            // Linhas 7-8: E-mail (2 colunas)
            AdicionarCampo(card, "E-mail", txtEmail, 0, 7, 2);

            // Linha 9: botão
            btnCadastrar = CriarBotao(editando ? "Salvar alterações" : "Salvar aluno", editando ? 170 : 145, VisualPadrao.Azul, Color.White, VisualPadrao.Azul);
            btnCadastrar.Anchor = AnchorStyles.Left;
            btnCadastrar.Margin = new Padding(0, 4, 0, 4);
            btnCadastrar.Click += CadastrarNovoAluno;
            card.Controls.Add(btnCadastrar, 0, 9);

            var btnCancelar = CriarBotao("Cancelar", 110, Color.White, VisualPadrao.TextoPrincipal, VisualPadrao.LinhaGrade);
            btnCancelar.Margin = new Padding(0, 4, 0, 4);
            btnCancelar.Click += delegate { Form janela = btnCancelar.FindForm(); if (janela != null) janela.Close(); };
            card.Controls.Add(btnCancelar, 1, 9);
            return card;
        }

        private static int CalcularIdade(DateTime nascimento) {
            DateTime hoje = DateTime.Today;
            int idade = hoje.Year - nascimento.Year;
            if (nascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }

        private void AtualizarCampoResponsavelPorIdade() {
            if (txtResponsavel == null || dtpNascimento == null) return;
            bool maiorDeIdade = CalcularIdade(dtpNascimento.Value) >= 18;
            if (maiorDeIdade) {
                txtResponsavel.Clear();
                txtResponsavel.Enabled = false;
                txtResponsavel.BackColor = SystemColors.Control;
            }
            else {
                txtResponsavel.Enabled = true;
                txtResponsavel.BackColor = Color.White;
            }
        }

        private void CadastrarNovoAluno(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtNomeAluno.Text)) {
                MessageBox.Show(btnCadastrar.FindForm(), "Informe o nome completo do aluno.", "Novo Aluno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomeAluno.Focus();
                return;
            }

            bool maiorDeIdade = CalcularIdade(dtpNascimento.Value) >= 18;
            FichaAluno aluno = linhaEdicaoAluno >= 0 && linhaEdicaoAluno < dgvAlunos.Rows.Count
                ? (dgvAlunos.Rows[linhaEdicaoAluno].Tag as FichaAluno ?? new FichaAluno())
                : new FichaAluno { DataCadastro = DateTime.Today };
            aluno.Nome = txtNomeAluno.Text.Trim();
            aluno.DataNascimento = dtpNascimento.Value.Date;
            aluno.Telefone = mtbTelefone.Text.Trim();
            aluno.Numero = txtNumero.Text.Trim();
            aluno.Cep = mtbCep.Text.Trim();
            aluno.Cidade = txtCidade.Text.Trim();
            aluno.Responsavel = maiorDeIdade ? string.Empty : txtResponsavel.Text.Trim();
            aluno.Cpf = mtbCpf.Text.Trim();
            aluno.Rg = txtRg.Text.Trim();
            aluno.Email = txtEmail.Text.Trim();

            if (linhaEdicaoAluno >= 0 && linhaEdicaoAluno < dgvAlunos.Rows.Count) {
                DataGridViewRow linha = dgvAlunos.Rows[linhaEdicaoAluno];
                linha.Tag = aluno;
                linha.Cells["colAluno"].Value = aluno.Nome;
                linha.Cells["colData"].Value = aluno.DataCadastro.ToString("dd/MM/yyyy");
            }
            else {
                AdicionarLinhaExemplo(aluno, "—", "—", "Mensal", "Ativa");
            }

            AplicarFiltrosAlunos();
            Form janela = btnCadastrar.FindForm();
            if (janela != null) { janela.DialogResult = DialogResult.OK; janela.Close(); }
        }

        // Filtros da lista de alunos
        private Control CriarCardFiltros() {
            var card = CriarCard(6);
            card.Padding = new Padding(12, 5, 12, 5);
            card.Margin = new Padding(12, 2, 12, 8);
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            txtBusca = new TextBox { Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = VisualPadrao.TextoPrincipal, BackColor = Color.White, BorderStyle = BorderStyle.None };
            DefinirPlaceholder(txtBusca, "Digite o nome do aluno...");
            cboFiltroTurma = CriarCombo(null, "Todas", "Hidroginástica", "Natação Iniciante", "Natação Intermediário", "Natação Avançado", "Natação Adulto", "Natação Master", "Natação Competição");
            cboFiltroProfessor = CriarCombo(null, "Todos", "Lucas Almeida", "Ana Paula Costa", "Gabriel Silva", "Felipe Rocha", "Mariana Oliveira", "Juliana Lima", "Rafael Santos");
            cboFiltroStatus = CriarCombo(null, "Todos", "Ativa", "Pausada", "Cancelada");

            AdicionarCampo(card, "Nome", CriarCampoFiltro(txtBusca), 0, 0);
            AdicionarCampo(card, "Turma", CriarCampoFiltro(cboFiltroTurma), 1, 0);
            AdicionarCampo(card, "Professor", CriarCampoFiltro(cboFiltroProfessor), 2, 0);
            AdicionarCampo(card, "Status", CriarCampoFiltro(cboFiltroStatus), 3, 0);

            RoundedButton pesquisar = new RoundedButton { Text = "Pesquisar", CornerRadius = 5, Size = new Size(100, 34), BackColor = Color.White, ForeColor = VisualPadrao.Azul, Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat, UseVisualStyleBackColor = false };
            pesquisar.FlatAppearance.BorderSize = 0;
            pesquisar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            pesquisar.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            pesquisar.FlatAppearance.MouseDownBackColor = Color.FromArgb(234, 242, 255);
            pesquisar.Margin = new Padding(0, 0, 8, 12);
            btnPesquisar = pesquisar;

            RoundedButton limpar = new RoundedButton { Text = "Limpar", CornerRadius = 5, Size = new Size(90, 34), BackColor = Color.White, ForeColor = Color.FromArgb(55, 65, 81), Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat, UseVisualStyleBackColor = false };
            limpar.FlatAppearance.BorderSize = 0;
            limpar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            limpar.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            limpar.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            limpar.Margin = new Padding(0, 0, 0, 12);
            limpar.MouseEnter += delegate { limpar.ForeColor = Color.FromArgb(185, 28, 28); limpar.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113); limpar.BackColor = Color.FromArgb(254, 242, 242); };
            limpar.MouseLeave += delegate { limpar.ForeColor = VisualPadrao.TextoPrincipal; limpar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225); limpar.BackColor = Color.White; };
            limpar.MouseDown += delegate { limpar.ForeColor = Color.FromArgb(153, 27, 27); limpar.BackColor = Color.FromArgb(254, 226, 226); };
            limpar.MouseUp += delegate { if (limpar.ClientRectangle.Contains(limpar.PointToClient(Cursor.Position))) { limpar.ForeColor = Color.FromArgb(185, 28, 28); limpar.BackColor = Color.FromArgb(254, 242, 242); } };
            btnLimpar = limpar;
            card.Controls.Add(btnPesquisar, 4, 1);
            card.Controls.Add(btnLimpar, 5, 1);
            return card;
        }

        private Panel CriarCampoFiltro(Control campo) {
            Panel moldura = new Panel { Dock = DockStyle.Fill, Height = 28, Padding = new Padding(1), Margin = new Padding(0), BackColor = Color.FromArgb(203, 213, 225) };
            Panel superficie = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0), Padding = new Padding(0), BackColor = Color.White };
            moldura.Controls.Add(superficie);
            campo.Margin = new Padding(0);
            campo.BackColor = Color.White;
            campo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            if (campo is TextBox) {
                TextBox texto = (TextBox)campo;
                texto.BorderStyle = BorderStyle.None;
                texto.AutoSize = false;
                texto.Height = 20;
                texto.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                texto.TextAlign = HorizontalAlignment.Left;

                Action posicionarTexto = delegate {
                    int altura = Math.Min(20, superficie.ClientSize.Height);
                    texto.SetBounds(8, Math.Max(0, (superficie.ClientSize.Height - altura) / 2),
                        Math.Max(0, superficie.ClientSize.Width - 16), altura);
                };
                superficie.SizeChanged += delegate { posicionarTexto(); };
                posicionarTexto();
            }
            else if (campo is ComboBox) {
                ComboBox combo = (ComboBox)campo;
                combo.FlatStyle = FlatStyle.Flat;
                combo.Dock = DockStyle.None;
                combo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                combo.DrawMode = DrawMode.OwnerDrawFixed;
                combo.ItemHeight = 18;
                combo.ForeColor = VisualPadrao.TextoPrincipal;
                combo.DrawItem += delegate(object sender, DrawItemEventArgs e) {
                    ComboBox cb = sender as ComboBox;
                    if (cb == null || e.Bounds.Width <= 0 || e.Bounds.Height <= 0) return;
                    bool selecionado = cb.DroppedDown && e.Index >= 0 &&
                        (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color fundo = selecionado ? Color.FromArgb(239, 246, 255) : Color.White;
                    Color corTexto = VisualPadrao.TextoPrincipal;
                    string valor = e.Index >= 0 && e.Index < cb.Items.Count
                        ? Convert.ToString(cb.Items[e.Index])
                        : cb.Text;
                    TextRenderer.DrawText(e.Graphics, valor, cb.Font,
                        Rectangle.Inflate(e.Bounds, -6, 0), corTexto,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                };

                string nomeSeta = ReferenceEquals(campo, cboFiltroTurma) ? "Turma" :
                    ReferenceEquals(campo, cboFiltroProfessor) ? "Professor" : "Status";
                Button seta = new Button {
                    Name = "btnSetaFiltroAluno" + nomeSeta,
                    AccessibleName = "Abrir opções",
                    AccessibleDescription = "Abre a lista de opções",
                    TabStop = false,
                    Text = string.Empty,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    UseVisualStyleBackColor = false,
                    Padding = new Padding(0),
                    Margin = new Padding(0)
                };
                seta.FlatAppearance.BorderSize = 0;
                seta.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
                seta.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
                seta.Paint += delegate(object sender, PaintEventArgs e) {
                    Rectangle r = seta.ClientRectangle;
                    if (r.Width < 8 || r.Height < 8) return;
                    bool hover = r.Contains(seta.PointToClient(Cursor.Position));
                    Color cor = hover ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                        e.Graphics.DrawLine(separador, 0, 1, 0, r.Height - 2);
                    float cx = r.Width / 2F;
                    float cy = r.Height / 2F;
                    using (Pen pen = new Pen(cor, 1.6F)) {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        e.Graphics.DrawLines(pen, new PointF[] {
                            new PointF(cx - 3.5F, cy - 1.5F),
                            new PointF(cx, cy + 2F),
                            new PointF(cx + 3.5F, cy - 1.5F)
                        });
                    }
                };
                seta.MouseEnter += delegate { seta.BackColor = Color.FromArgb(239, 246, 255); seta.Invalidate(); };
                seta.MouseLeave += delegate { seta.BackColor = Color.White; seta.Invalidate(); };
                seta.Click += delegate {
                    if (combo.IsDisposed) return;
                    combo.Focus();
                    combo.DroppedDown = true;
                };

                Action posicionarCombo = delegate {
                    combo.SetBounds(0, 0, superficie.ClientSize.Width, superficie.ClientSize.Height);
                    int larguraSeta = Math.Min(26, superficie.ClientSize.Width);
                    seta.SetBounds(Math.Max(0, superficie.ClientSize.Width - larguraSeta), 0,
                        larguraSeta, superficie.ClientSize.Height);
                    seta.BringToFront();
                };
                superficie.Controls.Add(campo);
                superficie.Controls.Add(seta);
                superficie.SizeChanged += delegate { posicionarCombo(); };
                posicionarCombo();
            }
            else {
                campo.Dock = DockStyle.Fill;
            }

            if (!(campo is ComboBox)) superficie.Controls.Add(campo);
            campo.Enter += delegate { moldura.BackColor = VisualPadrao.Azul; };
            campo.Leave += delegate { moldura.BackColor = Color.FromArgb(203, 213, 225); };
            return moldura;
        }

        // Tabela de alunos
        private DataGridView CriarGrade() {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = VisualPadrao.LinhaGrade,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 38,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Margin = new Padding(0)
            };
            g.RowTemplate.Height = 38;

            // Cabeçalho com a mesma paleta da grade de Pagamentos
            g.ColumnHeadersDefaultCellStyle.BackColor = VisualPadrao.FundoCabecalhoGrade;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = VisualPadrao.FundoCabecalhoGrade;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(55, 65, 81);
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            g.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Células alinhadas e coloridas como em Pagamentos
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            g.DefaultCellStyle.SelectionBackColor = Color.White;
            g.DefaultCellStyle.SelectionForeColor = Color.FromArgb(55, 65, 81);
            g.DefaultCellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            g.DefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
            g.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            g.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.White;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(55, 65, 81);
            g.ScrollBars = ScrollBars.Vertical;

            // Colunas principais da tabela de alunos
            g.Columns.Add("colData", "Data da Matrícula");
            g.Columns.Add("colAluno", "Aluno");
            g.Columns.Add("colTurma", "Turma");
            g.Columns.Add("colProfessor", "Professor");
            g.Columns.Add("colPlano", "Plano");
            g.Columns.Add("colStatus", "Status");

            // Mantém visualizar, editar e excluir na coluna Ações
            DataGridViewTextBoxColumn acoes = new DataGridViewTextBoxColumn
            {
                Name = "colAcoes", HeaderText = "Ações", ReadOnly = true, Width = 100, MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Resizable = DataGridViewTriState.False,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            acoes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            acoes.DefaultCellStyle.Padding = new Padding(0);
            acoes.DefaultCellStyle.SelectionBackColor = Color.White;
            acoes.DefaultCellStyle.SelectionForeColor = VisualPadrao.TextoPrincipal;
            g.Columns.Add(acoes);
            g.CellFormatting += Grade_CellFormatting;
            g.CellPainting += Grade_CellPainting;
            g.CellMouseMove += Grade_CellMouseMove;
            g.CellMouseLeave += Grade_CellMouseLeave;
            g.CellMouseClick += Grade_CellMouseClick;
            g.CellMouseDown += Grade_CellMouseDown;
            g.CellMouseUp += Grade_CellMouseUp;
            VisualPadrao.ConfigurarGrade(g);
            return g;
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
                int largura = 38;
                if (status == "Pausada") {
                    fundoSelo = Color.FromArgb(255, 244, 217);
                    textoSelo = Color.FromArgb(196, 122, 0);
                    largura = 54;
                } else if (status == "Cancelada") {
                    fundoSelo = Color.FromArgb(254, 228, 226);
                    textoSelo = Color.FromArgb(217, 45, 32);
                    largura = 58;
                }
                Rectangle selo = new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - largura) / 2,
                    e.CellBounds.Top + (e.CellBounds.Height - 22) / 2, largura, 22);
                using (Brush brush = new SolidBrush(fundoSelo)) e.Graphics.FillRectangle(brush, selo);
                using (Font fonteStatus = new Font("Segoe UI", 7.5F))
                    TextRenderer.DrawText(e.Graphics, status, fonteStatus, selo, textoSelo,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvAlunos.Columns[e.ColumnIndex].Name != "colAcoes") { LimparHoverAcoesA(); return; }
            int botao = IndiceBotaoAcaoSobMouseA(e.ColumnIndex, e.RowIndex);
            if (linhaAcaoSobMouseA == e.RowIndex && botaoAcaoSobMouseA == botao) return;
            LimparHoverAcoesA();
            linhaAcaoSobMouseA = e.RowIndex; botaoAcaoSobMouseA = botao;
            dgvAlunos.Cursor = botao >= 0 ? Cursors.Hand : Cursors.Default;
            dgvAlunos.InvalidateCell(e.ColumnIndex, e.RowIndex);
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvAlunos.Columns[e.ColumnIndex].Name != "colAcoes") return;
            int acao = IndiceBotaoAcaoSobMouseA(e.ColumnIndex, e.RowIndex);
            if (acao == 0) {
                FichaAluno aluno = dgvAlunos.Rows[e.RowIndex].Tag as FichaAluno;
                if (aluno != null) {
                    using (FichaAlunoJanela ficha = new FichaAlunoJanela(aluno))
                        ficha.ShowDialog(this);
                }
            }
            else if (acao == 1) EditarAluno(e.RowIndex);
            else if (acao == 2) ExcluirAluno(e.RowIndex);
        }

        private void EditarAluno(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvAlunos.Rows.Count) return;
            FichaAluno aluno = dgvAlunos.Rows[indiceLinha].Tag as FichaAluno;
            if (aluno == null) return;
            linhaEdicaoAluno = indiceLinha;
            AbrirDialogoAluno(aluno);
        }

        private void ExcluirAluno(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvAlunos.Rows.Count) return;
            string nome = Convert.ToString(dgvAlunos.Rows[indiceLinha].Cells["colAluno"].Value) ?? "este aluno";
            if (MessageBox.Show(this, "Deseja realmente excluir " + nome + "?", "Excluir aluno", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                dgvAlunos.Rows.RemoveAt(indiceLinha);
                AplicarFiltrosAlunos();
            }
        }

        // O status é desenhado como uma etiqueta colorida
        private void Grade_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvAlunos.Columns[e.ColumnIndex].Name != "colStatus")
                return;
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            e.CellStyle.SelectionBackColor = Color.White;
            e.CellStyle.SelectionForeColor = Color.FromArgb(55, 65, 81);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            e.CellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        }

        // Rodapé da lista de alunos
        private Control CriarRodape() {
            var rodape = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                BackColor = Color.White,
                Padding = new Padding(0, 8, 0, 0),
                Margin = new Padding(0)
            };
            rodape.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            lblResumo = new Label
            {
                Text = "Mostrando 1 a 9 de 42 alunos",
                AutoSize = true,
                ForeColor = VisualPadrao.TextoSecundario,
                Anchor = AnchorStyles.Left
            };
            rodape.Controls.Add(lblResumo, 0, 0);

            return rodape;
        }

        // Métodos auxiliares de layout

        // Cria um cartão com borda para cadastro e filtros
        private TableLayoutPanel CriarCard(int colunas) {
            var card = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = colunas,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(16, 10, 16, 4),
                Margin = new Padding(12, 0, 12, 12)
            };
            card.Paint += delegate(object sender, PaintEventArgs e) {
                VisualPadrao.DesenharBordaCartao(e.Graphics, card.ClientRectangle);
            };
            return card;
        }

        // Coloca "label em cima + controle embaixo" na posição pedida
        // "colunasOcupadas" permite um campo ocupar mais de uma coluna (ex.: Nome do aluno)
        private void AdicionarCampo(TableLayoutPanel card, string texto, Control controle,
                                    int coluna, int linhaDoLabel, int colunasOcupadas = 1) {
            var lbl = new Label
            {
                Text = texto,
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(55, 65, 81),
                Margin = new Padding(0, 0, 0, 4)
            };
            controle.Font = new Font("Segoe UI", 10F);
            controle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            controle.Margin = new Padding(0, 0, 16, 10);

            card.Controls.Add(lbl, coluna, linhaDoLabel);
            card.Controls.Add(controle, coluna, linhaDoLabel + 1);

            if (colunasOcupadas > 1) {
                card.SetColumnSpan(lbl, colunasOcupadas);
                card.SetColumnSpan(controle, colunasOcupadas);
            }
        }

        // ComboBox somente de seleção. "textoInicial" (se vier) vira o 1º item, tipo um placeholder
        private ComboBox CriarCombo(string textoInicial, params string[] itens) {
            var cbo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            if (textoInicial != null) cbo.Items.Add(textoInicial);
            cbo.Items.AddRange(itens);
            cbo.SelectedIndex = 0;
            return cbo;
        }

        // Aplica uma máscara aos campos numéricos
        private MaskedTextBox CriarMascara(string mascara) {
            return new MaskedTextBox
            {
                Mask = mascara,
                PromptChar = '_'
            };
        }

        private Button CriarBotao(string texto, int largura, Color fundo, Color corTexto, Color borda) {
            var b = new Button
            {
                Text = texto,
                Size = new Size(largura, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = fundo,
                ForeColor = corTexto,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderColor = borda;
            b.FlatAppearance.BorderSize = 1;
            return b;
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

        private void LimparFiltrosAlunos() {
            txtBusca.Clear();
            if (cboFiltroTurma.Items.Count > 0) cboFiltroTurma.SelectedIndex = 0;
            if (cboFiltroProfessor.Items.Count > 0) cboFiltroProfessor.SelectedIndex = 0;
            if (cboFiltroStatus.Items.Count > 0) cboFiltroStatus.SelectedIndex = 0;
            AplicarFiltrosAlunos();
        }

        private void AplicarFiltrosAlunos() {
            if (dgvAlunos == null || dgvAlunos.IsDisposed) return;
            string busca = txtBusca == null ? string.Empty : txtBusca.Text.Trim();
            string turma = cboFiltroTurma == null || cboFiltroTurma.SelectedIndex <= 0 ? string.Empty : Convert.ToString(cboFiltroTurma.SelectedItem);
            string professor = cboFiltroProfessor == null || cboFiltroProfessor.SelectedIndex <= 0 ? string.Empty : Convert.ToString(cboFiltroProfessor.SelectedItem);
            string status = cboFiltroStatus == null || cboFiltroStatus.SelectedIndex <= 0 ? string.Empty : Convert.ToString(cboFiltroStatus.SelectedItem);
            int exibidos = 0;
            foreach (DataGridViewRow linha in dgvAlunos.Rows) {
                FichaAluno aluno = linha.Tag as FichaAluno;
                string nome = Convert.ToString(linha.Cells["colAluno"].Value) ?? string.Empty;
                bool visivel = busca.Length == 0 || nome.IndexOf(busca, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    (aluno != null && aluno.Cpf.IndexOf(busca, StringComparison.CurrentCultureIgnoreCase) >= 0);
                if (visivel && turma.Length > 0) visivel = string.Equals(Convert.ToString(linha.Cells["colTurma"].Value), turma, StringComparison.CurrentCultureIgnoreCase);
                if (visivel && professor.Length > 0) visivel = string.Equals(Convert.ToString(linha.Cells["colProfessor"].Value), professor, StringComparison.CurrentCultureIgnoreCase);
                if (visivel && status.Length > 0) visivel = string.Equals(Convert.ToString(linha.Cells["colStatus"].Value), status, StringComparison.CurrentCultureIgnoreCase);
                if (!visivel && dgvAlunos.CurrentCell != null && dgvAlunos.CurrentCell.RowIndex == linha.Index) dgvAlunos.CurrentCell = null;
                linha.Visible = visivel;
                if (visivel) exibidos++;
            }
            if (lblResumo != null) lblResumo.Text = "Mostrando " + exibidos.ToString() + " de " + dgvAlunos.Rows.Count.ToString() + " alunos";
        }

        // Dados de exemplo para visualizar a tela antes da integração com o banco
        private void CarregarDadosDeExemplo() {
            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Maria Silva", new DateTime(1985, 3, 12), "(19) 99876-5432",
                    "120", "13025-000", "Campinas", "",
                    "123.456.789-09", "12.345.678-9", "maria.silva@email.com", new DateTime(2024, 5, 20)),
                "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("João Pedro", new DateTime(2016, 7, 4), "(19) 98765-4321",
                    "450", "13073-001", "Campinas", "Carla Pereira",
                    "234.567.890-10", "23.456.789-0", "carla.pereira@email.com", new DateTime(2024, 5, 18)),
                "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Ana Clara", new DateTime(2012, 11, 25), "(19) 97654-3210",
                    "85", "13015-001", "Campinas", "Roberto Santos",
                    "345.678.901-21", "34.567.890-1", "roberto.santos@email.com", new DateTime(2024, 5, 22)),
                "Natação Intermediário", "Gabriel Silva", "Trimestral", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Pedro Henrique", new DateTime(2009, 1, 30), "(19) 96543-2109",
                    "301", "13076-000", "Campinas", "Fernanda Alves",
                    "456.789.012-32", "45.678.901-2", "fernanda.alves@email.com", new DateTime(2024, 5, 21)),
                "Natação Avançado", "Felipe Rocha", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Laura Souza", new DateTime(1992, 9, 8), "(19) 95432-1098",
                    "77", "13015-080", "Campinas", "",
                    "567.890.123-43", "56.789.012-3", "laura.souza@email.com", new DateTime(2024, 5, 17)),
                "Natação Adulto", "Mariana Oliveira", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Gabriel Lima", new DateTime(1978, 6, 15), "(19) 94321-0987",
                    "1500", "13035-000", "Campinas", "",
                    "678.901.234-54", "67.890.123-4", "gabriel.lima@email.com", new DateTime(2024, 5, 19)),
                "Natação Master", "Juliana Lima", "Trimestral", "Pausada");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Beatriz Oliveira", new DateTime(2005, 12, 2), "(19) 93210-9876",
                    "640", "13070-010", "Campinas", "Marcos Oliveira",
                    "789.012.345-65", "78.901.234-5", "marcos.oliveira@email.com", new DateTime(2024, 5, 16)),
                "Natação Competição", "Rafael Santos", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Miguel Ferreira", new DateTime(2017, 4, 19), "(19) 92109-8765",
                    "233", "13070-210", "Campinas", "Patrícia Ferreira",
                    "890.123.456-76", "89.012.345-6", "patricia.ferreira@email.com", new DateTime(2024, 5, 15)),
                "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Sofia Martins", new DateTime(1999, 8, 27), "(19) 91098-7654",
                    "18", "13092-120", "Campinas", "",
                    "901.234.567-87", "90.123.456-7", "sofia.martins@email.com", new DateTime(2024, 5, 14)),
                "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");

            dgvAlunos.ClearSelection();
        }

        // Adiciona uma linha na grade e guarda a ficha completa do aluno dentro dela (Tag)
        private void AdicionarLinhaExemplo(FichaAluno aluno, string turma, string professor, string plano, string status) {
            int indice = dgvAlunos.Rows.Add(aluno.DataCadastro.ToString("dd/MM/yyyy"), aluno.Nome,
                                                turma, professor, plano, status);
            dgvAlunos.Rows[indice].Tag = aluno;
        }

        private FichaAluno CriarAlunoExemplo(string nome, DateTime nascimento, string telefone, string numero,
                                             string cep, string cidade, string responsavel, string cpf,
                                             string rg, string email, DateTime cadastro) {
            return new FichaAluno
            {
                Nome = nome,
                DataNascimento = nascimento,
                Telefone = telefone,
                Numero = numero,
                Cep = cep,
                Cidade = cidade,
                Responsavel = responsavel,
                Cpf = cpf,
                Rg = rg,
                Email = email,
                DataCadastro = cadastro
            };
        }
    }

    // Janela com os dados completos do aluno

    // Dados exibidos na ficha do aluno
    public class FichaAluno {
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
    }

    // Janela de consulta da ficha do aluno
    public class FichaAlunoJanela : Form {

        private static readonly CultureInfo PtBr = new CultureInfo("pt-BR");

        private readonly FichaAluno aluno;
        private Button btnFechar;

        public FichaAlunoJanela(FichaAluno aluno) {
            this.aluno = aluno;

            Text = "Ficha do aluno";
            ClientSize = new Size(760, 660);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);

            ConstruirFicha();

            // Enter e Esc fecham a janela
            AcceptButton = btnFechar;
            CancelButton = btnFechar;
        }

        private void ConstruirFicha() {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                Padding = new Padding(36, 28, 36, 20),
                BackColor = Color.White,
                AutoScroll = true
            };
            for (int i = 0; i < 4; i++)
                raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            int linha = 0;

            var titulo = new Label
            {
                Text = "FICHA DE MATRÍCULA",
                AutoSize = false,
                Dock = DockStyle.Fill,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = VisualPadrao.TextoPrincipal,
                Margin = new Padding(0, 0, 0, 18)
            };
            raiz.Controls.Add(titulo, 0, linha);
            raiz.SetColumnSpan(titulo, 4);
            linha++;

            AdicionarCampo(raiz, "Nome do aluno", aluno.Nome, 0, linha++, 4);

            AdicionarCampo(raiz, "Data de nascimento", aluno.DataNascimento.ToString("dd/MM/yyyy"), 0, linha, 2);
            AdicionarCampo(raiz, "Telefone", aluno.Telefone, 2, linha++, 2);

            AdicionarCampo(raiz, "Número", aluno.Numero, 0, linha, 1);
            AdicionarCampo(raiz, "CEP", aluno.Cep, 1, linha, 1);
            AdicionarCampo(raiz, "Cidade", aluno.Cidade, 2, linha++, 2);

            AdicionarCampo(raiz, "Nome do responsável", string.IsNullOrWhiteSpace(aluno.Responsavel) ? "—" : aluno.Responsavel, 0, linha++, 4);

            AdicionarCampo(raiz, "CPF", aluno.Cpf, 0, linha, 1);
            AdicionarCampo(raiz, "RG", aluno.Rg, 1, linha, 1);
            AdicionarCampo(raiz, "E-mail", aluno.Email, 2, linha++, 2);

            var lblData = new Label
            {
                Text = "Campinas, " + aluno.DataCadastro.ToString("d 'de' MMMM 'de' yyyy", PtBr) + ".",
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = VisualPadrao.TextoSecundario,
                Margin = new Padding(0, 18, 0, 18)
            };
            raiz.Controls.Add(lblData, 0, linha);
            raiz.SetColumnSpan(lblData, 4);
            linha++;

            btnFechar = new Button
            {
                Text = "Fechar",
                Size = new Size(110, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = VisualPadrao.Azul,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right
            };
            btnFechar.FlatAppearance.BorderColor = VisualPadrao.Azul;
            raiz.Controls.Add(btnFechar, 0, linha);
            raiz.SetColumnSpan(btnFechar, 4);

            Controls.Add(raiz);
        }

        // Um campo da ficha: rótulo pequeno + valor + linha embaixo, como no papel
        private void AdicionarCampo(TableLayoutPanel tabela, string rotulo, string valor,
                                    int coluna, int linha, int colunasOcupadas) {
            var campo = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 18, 14),
                BackColor = Color.White
            };
            campo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            campo.Controls.Add(new Label
            {
                Text = rotulo.ToUpper(PtBr),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = VisualPadrao.TextoSecundario,
                Margin = new Padding(0, 0, 0, 2)
            }, 0, 0);

            campo.Controls.Add(new Label
            {
                Text = string.IsNullOrWhiteSpace(valor) ? "—" : valor,
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                ForeColor = VisualPadrao.TextoPrincipal,
                Margin = new Padding(0, 0, 0, 4)
            }, 0, 1);

            campo.Controls.Add(new Panel
            {
                Height = 1,
                BackColor = VisualPadrao.BordaCampo,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0)
            }, 0, 2);

            tabela.Controls.Add(campo, coluna, linha);
            if (colunasOcupadas > 1)
                tabela.SetColumnSpan(campo, colunasOcupadas);
        }
    }
}
