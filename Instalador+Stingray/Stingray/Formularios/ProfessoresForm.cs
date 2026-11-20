using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.Servicos;

namespace Stingray {
    [DesignerCategory("Form")]

    // Controla a consulta, filtragem e manutenção dos professores
    public partial class ProfessoresForm : Form {
private RoundedButton btnLimparFiltros;
        private readonly Dictionary<TextBox, string> placeholders = new Dictionary<TextBox, string>();
        private int linhaAcaoSobMouseP = -1;
        private int botaoAcaoSobMouseP = -1;
        private int linhaBotaoPressionadoP = -1;
        private int botaoPressionadoP = -1;
        private readonly ProfessorServico professorServico = new ProfessorServico();

        public ProfessoresForm() {
            InitializeComponent();
            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);
            ConfigurarAparencia();
            if (emDesignTime)
                return;

            CarregarProfessores();
        }

        // Atualiza a grade com os professores cadastrados
        private void CarregarProfessores() {
            try {
                List<Professor> professores = professorServico.ListarTodos();
                dgvProfessores.Rows.Clear();

                foreach (Professor professor in professores) {
                    string telefone = professor.Telefone == null
                        ? string.Empty
                        : ExtrairDigitos(professor.Telefone.NumeroTelefone);
                    string telefoneExibicao = telefone.Length == 0
                        ? "----"
                        : FormatarTelefone(telefone);
                    string nascimento = professor.Pessoa.DataNascimento.ToString("dd/MM/yyyy");
                    string resumoTurmas = ResumoTurmas(professor.Turmas);

                    int indice = dgvProfessores.Rows.Add(
                        professor.Pessoa.Nome,
                        professor.Cref,
                        telefoneExibicao,
                        nascimento,
                        resumoTurmas,
                        string.Empty);

                    dgvProfessores.Rows[indice].Tag = professor;
                }

                AplicarFiltrosTabela();
            }
            catch (Exception erro) {
                MessageBox.Show(this,
                    "Não foi possível carregar os professores do banco de dados.\n\n" + erro.Message,
                    "Consulta de professores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarAparencia() {
            BackColor = VisualPadrao.FundoPagina;
            pnlProfessores.BackColor = VisualPadrao.FundoPagina;
            pnlCabecalho.BackColor = VisualPadrao.FundoPagina;
            pnlCabecalho.Padding = new Padding(0);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(13, 17);
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitulo.Text = "Professores";

            tblFiltros.BackColor = Color.White;
            tblFiltros.Padding = new Padding(12, 5, 12, 5);
            tblFiltros.Margin = new Padding(0);

            btnLimparFiltros.Name = "btnLimparFiltros";
            btnLimparFiltros.Text = "Limpar";
            btnLimparFiltros.CornerRadius = 5;
            btnLimparFiltros.BackColor = Color.White;
            btnLimparFiltros.ForeColor = VisualPadrao.TextoPrincipal;
            btnLimparFiltros.FlatAppearance.BorderSize = 0;
            btnLimparFiltros.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            btnLimparFiltros.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnLimparFiltros.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            btnLimparFiltros.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnLimparFiltros.Dock = DockStyle.Fill;
            btnLimparFiltros.Margin = new Padding(8, 0, 0, 0);
            btnLimparFiltros.Cursor = Cursors.Hand;
            btnLimparFiltros.TextAlign = ContentAlignment.MiddleCenter;
            btnLimparFiltros.Click += delegate {
                RestaurarPlaceholder(txtNome);
                RestaurarPlaceholder(txtCref);
                RestaurarPlaceholder(txtTelefone);
                AplicarFiltrosTabela();
            };
            btnLimparFiltros.MouseEnter += delegate {
                btnLimparFiltros.ForeColor = Color.FromArgb(185, 28, 28);
                btnLimparFiltros.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
                btnLimparFiltros.BackColor = Color.FromArgb(254, 242, 242);
            };
            btnLimparFiltros.MouseLeave += delegate {
                btnLimparFiltros.ForeColor = VisualPadrao.TextoPrincipal;
                btnLimparFiltros.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                btnLimparFiltros.BackColor = Color.White;
            };
            btnLimparFiltros.MouseDown += delegate {
                btnLimparFiltros.ForeColor = Color.FromArgb(153, 27, 27);
                btnLimparFiltros.BackColor = Color.FromArgb(254, 226, 226);
            };
            btnLimparFiltros.MouseUp += delegate {
                if (btnLimparFiltros.ClientRectangle.Contains(btnLimparFiltros.PointToClient(Cursor.Position))) {
                    btnLimparFiltros.ForeColor = Color.FromArgb(185, 28, 28);
                    btnLimparFiltros.BackColor = Color.FromArgb(254, 242, 242);
                }
            };
            pnlNome.Dock = DockStyle.Fill;
            pnlCref.Dock = DockStyle.Fill;
            pnlTelefone.Dock = DockStyle.Fill;

            PrepararCampo(pnlNome, txtNome);
            PrepararCampo(pnlCref, txtCref);
            PrepararCampo(pnlTelefone, txtTelefone);

            txtNome.ReadOnly = false;
            txtCref.ReadOnly = false;
            txtTelefone.ReadOnly = false;

            foreach (Label label in new Label[] { lblNome, lblCref, lblTelefone }) {
                label.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
                label.ForeColor = VisualPadrao.TextoPrincipal;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Margin = new Padding(0, 0, 0, 0);
            }
            lblCref.Margin = new Padding(0);
            lblTelefone.Margin = new Padding(0);

            ConfigurarPlaceholder(txtNome, "Digite o nome do professor...");
            ConfigurarPlaceholder(txtCref, "Digite o CREF...");
            ConfigurarPlaceholder(txtTelefone, "Digite o telefone...");
            ConfigurarBotaoAdicionar();
            ConfigurarFiltrosTabela();
            ConfigurarColunaAcoes();
            ConfigurarTabela(dgvProfessores);
            dgvProfessores.CellPainting += DgvProfessores_CellPainting;
            dgvProfessores.CellFormatting += DgvProfessores_CellFormatting;
            dgvProfessores.CellMouseClick += DgvProfessores_CellMouseClick;
            dgvProfessores.CellMouseMove += DgvProfessores_CellMouseMove;
            dgvProfessores.CellMouseLeave += DgvProfessores_CellMouseLeave;
            dgvProfessores.CellMouseDown += DgvProfessores_CellMouseDown;
            dgvProfessores.CellMouseUp += DgvProfessores_CellMouseUp;
            colNome.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNome.FillWeight = 155F;
            colCref.FillWeight = 125F;
            colTelefone.FillWeight = 115F;
            colDataNascimento.FillWeight = 130F;
            colTurmas.FillWeight = 55F;
            colTurmas.ToolTipText = "Clique na lupa para ver todas as turmas do professor";
            colCref.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTelefone.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDataNascimento.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTurmas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private static string ResumoTurmas(IEnumerable<string> turmas) {
            int quantidade = turmas == null ? 0 : new List<string>(turmas).Count;
            return quantidade == 1
                ? "1 turma"
                : quantidade.ToString() + " turmas";
        }

        private void ConfigurarBotaoAdicionar() {
            if (btnAdicionar == null)
                return;

            btnAdicionar.Text = "+  Novo professor";
            btnAdicionar.CornerRadius = 5;
            btnAdicionar.BackColor = VisualPadrao.Azul;
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.FlatAppearance.BorderColor = VisualPadrao.Azul;
            btnAdicionar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            btnAdicionar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            btnAdicionar.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnAdicionar.Dock = DockStyle.Fill;
            btnAdicionar.Margin = new Padding(8, 0, 0, 0);
            btnAdicionar.Cursor = Cursors.Hand;
            btnAdicionar.UseVisualStyleBackColor = false;
            btnAdicionar.TextAlign = ContentAlignment.MiddleCenter;
            btnAdicionar.Click -= BtnAdicionar_Click;
            btnAdicionar.Click += BtnAdicionar_Click;
            btnAdicionar.BringToFront();
        }

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

        private bool CampoContemPlaceholder(TextBox campo) {
            string placeholder;
            return placeholders.TryGetValue(campo, out placeholder) &&
                   campo.ForeColor == VisualPadrao.TextoSecundario &&
                   string.Equals(campo.Text, placeholder, StringComparison.Ordinal);
        }

        private string ObterValorFiltro(TextBox campo) {
            return CampoContemPlaceholder(campo)
                ? string.Empty
                : campo.Text.Trim();
        }

        private void RestaurarPlaceholder(TextBox campo) {
            string placeholder;
            if (campo != null && placeholders.TryGetValue(campo, out placeholder)) {
                // TextChanged é disparado ao restaurar o placeholder. A cor precisa
                // ser atualizada primeiro para o filtro reconhecer o campo como vazio
                campo.ForeColor = VisualPadrao.TextoSecundario;
                if (!string.Equals(campo.Text, placeholder, StringComparison.Ordinal))
                    campo.Text = placeholder;
            }
        }

        private void ConfigurarFiltrosTabela() {
            txtNome.TextChanged += delegate { AplicarFiltrosTabela(); };
            txtCref.TextChanged += delegate { AplicarFiltrosTabela(); };
            txtTelefone.TextChanged += delegate { AplicarFiltrosTabela(); };
        }

        private void BtnAdicionar_Click(object sender, EventArgs e) {
            AbrirDialogoProfessor(-1);
        }

        private void DgvProfessores_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (string.Equals(coluna, "colTurmas", StringComparison.Ordinal)) {
                MostrarTurmasProfessor(e.RowIndex);
                return;
            }
            if (!string.Equals(coluna, "colAcoes", StringComparison.Ordinal))
                return;

            int acao = IndiceBotaoAcaoSobMouseP(e.ColumnIndex, e.RowIndex);
            if (acao == 0)
                AbrirDialogoProfessor(e.RowIndex);
            else if (acao == 1)
                ExcluirProfessor(e.RowIndex);
        }

        // Apresenta as turmas vinculadas ao professor selecionado
        private void MostrarTurmasProfessor(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvProfessores.Rows.Count)
                return;

            Professor professor = dgvProfessores.Rows[indiceLinha].Tag as Professor;
            if (professor == null)
                return;

            try {
                Professor atualizado = professorServico.BuscarPorId(professor.IdProfessor);
                if (atualizado != null) {
                    professor = atualizado;
                    dgvProfessores.Rows[indiceLinha].Tag = atualizado;
                    if (dgvProfessores.Columns.Contains("colTurmas"))
                        dgvProfessores.Rows[indiceLinha].Cells["colTurmas"].Value = ResumoTurmas(atualizado.Turmas);
                }
            }
            catch (Exception erro) {
                MessageBox.Show(this,
                    "Não foi possível atualizar as turmas vinculadas ao professor.\n\n" + erro.Message,
                    "Consulta de turmas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (ProfessorTurmasForm dialogo = new ProfessorTurmasForm(
                professor.Pessoa == null ? string.Empty : professor.Pessoa.Nome,
                professor.Turmas)) {
                dialogo.ShowDialog(this);
            }
        }

        private void AbrirDialogoProfessor(int indiceLinha) {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvProfessores.Rows.Count;
            Professor professor = editando
                ? dgvProfessores.Rows[indiceLinha].Tag as Professor
                : null;

            if (professor != null) {
                try {
                    Professor atualizado = professorServico.BuscarPorId(professor.IdProfessor);
                    if (atualizado != null) {
                        professor = atualizado;
                        dgvProfessores.Rows[indiceLinha].Tag = atualizado;
                        if (dgvProfessores.Columns.Contains("colTurmas"))
                            dgvProfessores.Rows[indiceLinha].Cells["colTurmas"].Value = ResumoTurmas(atualizado.Turmas);
                    }
                }
                catch (Exception erro) {
                    MessageBox.Show(this,
                        "Não foi possível carregar os dados atualizados do professor.\n\n" + erro.Message,
                        "Consulta de professor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            using (ProfessorCadastroForm dialogo = new ProfessorCadastroForm(professor)) {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    CarregarProfessores();
            }
        }

        private static string ExtrairDigitos(string texto) {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            System.Text.StringBuilder resultado = new System.Text.StringBuilder();
            foreach (char caractere in texto) {
                if (char.IsDigit(caractere))
                    resultado.Append(caractere);
            }

            return resultado.ToString();
        }

        private static string FormatarTelefoneParcial(string digitos) {
            if (digitos.Length <= 2)
                return digitos;

            string ddd = digitos.Substring(0, 2);
            string numero = digitos.Substring(2);
            string prefixo = "(" + ddd + ") ";
            int tamanhoPrefixo = numero.StartsWith("9", StringComparison.Ordinal) ? 5 : 4;

            if (numero.Length <= tamanhoPrefixo)
                return prefixo + numero;

            return prefixo + numero.Substring(0, tamanhoPrefixo) + "-" +
                numero.Substring(tamanhoPrefixo);
        }

        private static string FormatarTelefone(string digitos) {
            return FormatarTelefoneParcial(digitos);
        }

        private void DgvProfessores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex != colTelefone.Index)
                return;

            string telefone = Convert.ToString(e.Value);
            string digitos = ExtrairDigitos(telefone);

            if (digitos.Length == 0) {
                e.Value = "----";
                e.FormattingApplied = true;
            }
        }

        // Valida e solicita a exclusão do professor selecionado
        private void ExcluirProfessor(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvProfessores.Rows.Count)
                return;

            Professor professor = dgvProfessores.Rows[indiceLinha].Tag as Professor;
            if (professor == null)
                return;

            DialogResult confirmacao = MessageBox.Show(
                this,
                "Deseja realmente excluir " + professor.Pessoa.Nome + " do banco de dados?",
                "Excluir professor",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao != DialogResult.Yes)
                return;

            try {
                professorServico.Excluir(professor.IdProfessor);
                CarregarProfessores();
            }
            catch (Exception erro) {
                MessageBox.Show(this,
                    "Não foi possível excluir o professor.\n\n" + erro.Message,
                    "Falha ao excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColunaAcoes() {
            if (dgvProfessores.Columns.Contains("colAcoes"))
                return;

            DataGridViewTextBoxColumn acoes = new DataGridViewTextBoxColumn();
            acoes.Name = "colAcoes";
            acoes.HeaderText = "Ações";
            acoes.ReadOnly = true;
            acoes.Width = 82;
            acoes.MinimumWidth = 82;
            acoes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            acoes.Resizable = DataGridViewTriState.False;
            acoes.SortMode = DataGridViewColumnSortMode.NotSortable;
            acoes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            acoes.DefaultCellStyle.Padding = new Padding(0);
            acoes.DefaultCellStyle.SelectionBackColor = Color.White;
            acoes.DefaultCellStyle.SelectionForeColor = VisualPadrao.TextoPrincipal;
            dgvProfessores.Columns.Add(acoes);
        }

        private void DgvProfessores_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colTurmas")
                return;

            e.PaintBackground(e.CellBounds, true);
            if (coluna == "colTurmas") {
                Rectangle lupa = ObterRetanguloLupaP(e.CellBounds);
                bool hover = e.RowIndex == linhaAcaoSobMouseP && botaoAcaoSobMouseP == 2;
                bool pressionado = e.RowIndex == linhaBotaoPressionadoP && botaoPressionadoP == 2;
                Color fundo;
                Color borda;
                Color icone;
                VisualPadrao.ObterCoresAcao(false, false, hover, pressionado, out fundo, out borda, out icone);
                using (Brush pincel = new SolidBrush(fundo))
                using (Pen caneta = new Pen(borda)) {
                    e.Graphics.FillRectangle(pincel, lupa);
                    e.Graphics.DrawRectangle(caneta, lupa);
                }
                using (Font fonteLupa = new Font("Segoe MDL2 Assets", 10F))
                    TextRenderer.DrawText(e.Graphics, "\uE721", fonteLupa, lupa, icone,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                e.Handled = true;
                return;
            }

            Rectangle[] botoes = ObterRetangulosBotoesAcaoP(e.CellBounds);
            for (int i = 0; i < botoes.Length; i++) {
                bool hover = e.RowIndex == linhaAcaoSobMouseP && botaoAcaoSobMouseP == i;
                bool pressionado = e.RowIndex == linhaBotaoPressionadoP && botaoPressionadoP == i;
                Color fundo;
                Color borda;
                Color icone;
                VisualPadrao.ObterCoresAcao(i == 1, false, hover, pressionado, out fundo, out borda, out icone);
                using (Brush fill = new SolidBrush(fundo))
                using (Pen pen = new Pen(borda)) { e.Graphics.FillRectangle(fill, botoes[i]); e.Graphics.DrawRectangle(pen, botoes[i]); }
                using (Font actionFont = new Font("Segoe MDL2 Assets", 10F))
                    TextRenderer.DrawText(e.Graphics, i == 0 ? "\uE104" : "\uE107", actionFont, botoes[i], icone,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            e.Handled = true;
        }

        private Rectangle[] ObterRetangulosBotoesAcaoP(Rectangle celula) {
            const int tamanho = 26, espaco = 7;
            int largura = tamanho * 2 + espaco;
            int x = celula.Left + Math.Max(2, (celula.Width - largura) / 2);
            int y = celula.Top + Math.Max(2, (celula.Height - tamanho) / 2);
            return new Rectangle[] { new Rectangle(x, y, tamanho, tamanho), new Rectangle(x + tamanho + espaco, y, tamanho, tamanho) };
        }

        private Rectangle ObterRetanguloLupaP(Rectangle celula) {
            const int tamanho = 26;
            return new Rectangle(celula.Left + (celula.Width - tamanho) / 2,
                celula.Top + (celula.Height - tamanho) / 2, tamanho, tamanho);
        }

        private int IndiceBotaoAcaoSobMouseP(int coluna, int linha) {
            if (coluna < 0 || linha < 0)
                return -1;
            Rectangle celula = dgvProfessores.GetCellDisplayRectangle(coluna, linha, false);
            Point mouse = dgvProfessores.PointToClient(Cursor.Position);
            Point local = new Point(mouse.X - celula.Left, mouse.Y - celula.Top);
            if (dgvProfessores.Columns[coluna].Name == "colTurmas")
                return ObterRetanguloLupaP(new Rectangle(0, 0, celula.Width, celula.Height)).Contains(local) ? 2 : -1;
            Rectangle[] botoes = ObterRetangulosBotoesAcaoP(new Rectangle(0, 0, celula.Width, celula.Height));
            for (int i = 0; i < botoes.Length; i++)
                if (botoes[i].Contains(local))
                    return i;

            return -1;
        }

        private void DgvProfessores_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) {
                LimparHoverAcoesP();
                return;
            }
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colTurmas") {
                LimparHoverAcoesP();
                return;
            }
            int botao = IndiceBotaoAcaoSobMouseP(e.ColumnIndex, e.RowIndex);
            if (linhaAcaoSobMouseP == e.RowIndex && botaoAcaoSobMouseP == botao)
                return;
            LimparHoverAcoesP();
            linhaAcaoSobMouseP = e.RowIndex; botaoAcaoSobMouseP = botao;
            dgvProfessores.Cursor = botao >= 0
                ? Cursors.Hand
                : Cursors.Default;
            dgvProfessores.InvalidateCell(e.ColumnIndex, e.RowIndex);
        }

        private void DgvProfessores_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == linhaAcaoSobMouseP && e.ColumnIndex >= 0 &&
                (dgvProfessores.Columns[e.ColumnIndex].Name == "colAcoes" || dgvProfessores.Columns[e.ColumnIndex].Name == "colTurmas")) LimparHoverAcoesP();
        }

        private void DgvProfessores_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvProfessores.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colTurmas")
                return;
            int botao = IndiceBotaoAcaoSobMouseP(e.ColumnIndex, e.RowIndex);
            if (botao < 0)
                return;
            linhaBotaoPressionadoP = e.RowIndex; botaoPressionadoP = botao;
            dgvProfessores.InvalidateCell(e.ColumnIndex, e.RowIndex);
            dgvProfessores.Cursor = Cursors.Hand;
        }

        private void DgvProfessores_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e) {
            int linha = linhaBotaoPressionadoP;
            int botao = botaoPressionadoP;
            linhaBotaoPressionadoP = -1; botaoPressionadoP = -1;
            if (linha >= 0 && linha < dgvProfessores.Rows.Count) {
                if (botao == 2 && dgvProfessores.Columns.Contains("colTurmas"))
                    dgvProfessores.InvalidateCell(dgvProfessores.Columns["colTurmas"].Index, linha);
                else if (dgvProfessores.Columns.Contains("colAcoes"))
                    dgvProfessores.InvalidateCell(dgvProfessores.Columns["colAcoes"].Index, linha);
            }
        }

        private void LimparHoverAcoesP() {
            int linha = linhaAcaoSobMouseP;
            int botao = botaoAcaoSobMouseP;
            linhaAcaoSobMouseP = -1;
            botaoAcaoSobMouseP = -1;
            dgvProfessores.Cursor = Cursors.Default;
            if (linha >= 0 && linha < dgvProfessores.Rows.Count) {
                string nomeColuna = botao == 2 ? "colTurmas" : "colAcoes";
                if (dgvProfessores.Columns.Contains(nomeColuna))
                    dgvProfessores.InvalidateCell(dgvProfessores.Columns[nomeColuna].Index, linha);
            }
        }

        // Atualiza a grade com os critérios de pesquisa
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

        private void PrepararCampo(Panel painel, TextBox campo) {
            painel.BackColor = VisualPadrao.BordaCampo;
            painel.Padding = new Padding(1);

            Panel superficie = null;
            foreach (Control controle in painel.Controls) {
                Panel candidata = controle as Panel;
                if (candidata != null && candidata.Name == painel.Name + "Superficie") {
                    superficie = candidata;
                    break;
                }
            }

            if (superficie == null) {
                superficie = new Panel { Name = painel.Name + "Superficie", Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0) };
                painel.Controls.Add(superficie);
            }

            if (campo.Parent != superficie)
                superficie.Controls.Add(campo);

            campo.Enter += delegate { painel.BackColor = VisualPadrao.Azul; };
            campo.Leave += delegate { painel.BackColor = VisualPadrao.BordaCampo; };
            campo.BackColor = Color.White;
            campo.BorderStyle = BorderStyle.None;
            campo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            campo.ForeColor = VisualPadrao.TextoSecundario;
            campo.Dock = DockStyle.None;
            campo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            campo.Height = 18;
            campo.Margin = new Padding(0);
            superficie.SizeChanged += delegate { PosicionarCampo(superficie, campo); };
            PosicionarCampo(superficie, campo);
        }

        private void PosicionarCampo(Panel superficie, TextBox campo) {
            if (superficie == null || campo == null)
                return;

            campo.Location = new Point(6, Math.Max(1, (superficie.ClientSize.Height - campo.Height) / 2));
            campo.Width = Math.Max(30, superficie.ClientSize.Width - campo.Left - 6);
        }

        // Prepara as colunas e os estilos da grade
        private static void ConfigurarTabela(DataGridView tabela) {
            VisualPadrao.ConfigurarGrade(tabela);
        }
    }
}
