using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class ProfessoresForm : Form {
        private Panel pnlCadastro;
        private TextBox txtCadastroNome;
        private TextBox txtCadastroCref;
        private TextBox txtCadastroTelefone;
        private TextBox txtCadastroNascimento;
        private RoundedButton btnLimparFiltros;
        private readonly Dictionary<TextBox, string> placeholders = new Dictionary<TextBox, string>();
        private int linhaAcaoSobMouseP = -1;
        private int botaoAcaoSobMouseP = -1;
        private int linhaBotaoPressionadoP = -1;
        private int botaoPressionadoP = -1;

        public ProfessoresForm() {
            InitializeComponent();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            ConfigurarAparencia();
            if (emDesignTime)
                return;
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
            PreencherProfessoresDeExemplo();
        }

        private Panel CriarAreaCadastro() {
            Panel cartao = new Panel();
            cartao.Name = "pnlCadastroProfessor";
            cartao.Dock = DockStyle.Fill;
            cartao.Margin = new Padding(16, 6, 16, 14);
            cartao.Padding = new Padding(1);
            cartao.BackColor = VisualPadrao.BordaCartao;

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Name = "tlpCadastroProfessor";
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.White;
            layout.Margin = new Padding(0);
            layout.Padding = new Padding(16, 8, 16, 8);
            layout.ColumnCount = 5;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            layout.RowCount = 4;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

            Label titulo = new Label();
            titulo.Name = "lblCadastroProfessor";
            titulo.Text = "Novo professor";
            titulo.Dock = DockStyle.Fill;
            titulo.Margin = new Padding(0);
            titulo.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = Color.FromArgb(17, 24, 39);
            titulo.TextAlign = ContentAlignment.MiddleLeft;
            layout.Controls.Add(titulo, 0, 0);
            layout.SetColumnSpan(titulo, 5);

            Label descricao = new Label();
            descricao.Name = "lblDescricaoCadastroProfessor";
            descricao.Text = "Preencha os dados abaixo para incluir um novo docente.";
            descricao.Dock = DockStyle.Fill;
            descricao.Margin = new Padding(0);
            descricao.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            descricao.ForeColor = VisualPadrao.TextoSecundario;
            descricao.TextAlign = ContentAlignment.MiddleLeft;
            layout.Controls.Add(descricao, 0, 1);
            layout.SetColumnSpan(descricao, 5);

            string[] nomes = { "Nome", "CREF", "Telefone", "Data de nascimento", "" };
            for (int coluna = 0; coluna < nomes.Length; coluna++) {
                Label label = new Label();
                label.Text = nomes[coluna];
                label.Dock = DockStyle.Fill;
                label.Margin = new Padding(coluna == 0 ? 0 : 6, 0, 0, 0);
                label.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
                label.ForeColor = VisualPadrao.TextoPrincipal;
                label.TextAlign = ContentAlignment.MiddleLeft;
                layout.Controls.Add(label, coluna, 2);
            }

            txtCadastroNome = CriarTextBoxCadastro("txtCadastroNome");
            txtCadastroCref = CriarTextBoxCadastro("txtCadastroCref");
            txtCadastroTelefone = CriarTextBoxCadastro("txtCadastroTelefone");
            txtCadastroNascimento = CriarTextBoxCadastro("txtCadastroNascimento");

            layout.Controls.Add(CriarCampoCadastro(txtCadastroNome), 0, 3);
            layout.Controls.Add(CriarCampoCadastro(txtCadastroCref), 1, 3);
            layout.Controls.Add(CriarCampoCadastro(txtCadastroTelefone), 2, 3);
            layout.Controls.Add(CriarCampoCadastro(txtCadastroNascimento), 3, 3);

            btnAdicionar.Text = "+  Adicionar professor";
            btnAdicionar.CornerRadius = 5;
            btnAdicionar.BackColor = VisualPadrao.Azul;
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.FlatAppearance.BorderColor = VisualPadrao.Azul;
            btnAdicionar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            btnAdicionar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            btnAdicionar.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnAdicionar.Dock = DockStyle.Fill;
            btnAdicionar.Margin = new Padding(8, 1, 0, 1);
            btnAdicionar.Cursor = Cursors.Hand;
            btnAdicionar.UseVisualStyleBackColor = false;
            btnAdicionar.TextAlign = ContentAlignment.MiddleCenter;
            btnAdicionar.Click -= BtnAdicionar_Click;
            btnAdicionar.Click += BtnAdicionar_Click;
            layout.Controls.Add(btnAdicionar, 4, 3);

            cartao.Controls.Add(layout);
            return cartao;
        }

        private TextBox CriarTextBoxCadastro(string nome) {
            return new TextBox {
                Name = nome,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = VisualPadrao.TextoPrincipal,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                TabStop = true
            };
        }

        private Panel CriarCampoCadastro(TextBox campo) {
            Panel painel = new Panel();
            painel.Name = "pnl" + campo.Name;
            painel.BackColor = VisualPadrao.BordaCampo;
            painel.Padding = new Padding(1);
            painel.Dock = DockStyle.Fill;
            painel.Margin = new Padding(8, 1, 0, 1);

            Panel superficie = new Panel();
            superficie.Dock = DockStyle.Fill;
            superficie.BackColor = Color.White;
            superficie.Margin = new Padding(0);
            painel.Controls.Add(superficie);
            superficie.Controls.Add(campo);

            campo.Dock = DockStyle.None;
            campo.Height = 20;
            campo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            campo.Enter += delegate { painel.BackColor = VisualPadrao.Azul; };
            campo.Leave += delegate { painel.BackColor = VisualPadrao.BordaCampo; };
            superficie.SizeChanged += delegate { PosicionarCampo(superficie, campo); };
            PosicionarCampo(superficie, campo);
            return painel;
        }

        private void PreencherProfessoresDeExemplo() {
            // Registros fictícios de demonstração. Cada professor guarda uma lista de turmas na linha
            AdicionarProfessorDeExemplo("Mariana Costa", "CREF 000001-G/SP", "(11) 90000-0001", "14/03/1987", "Natação Iniciante", "Natação Adulto");
            AdicionarProfessorDeExemplo("Rafael Mendes", "CREF 000002-G/SP", "(11) 90000-0002", "22/07/1990", "Natação Avançado", "Natação Competição");
            AdicionarProfessorDeExemplo("Camila Ferreira", "CREF 000003-G/SP", "(11) 90000-0003", "09/11/1984", "Hidroginástica");
            AdicionarProfessorDeExemplo("Lucas Almeida", "CREF 000004-G/SP", "(11) 90000-0004", "30/01/1992", "Natação Infantil", "Natação (5 a 7 anos)");
            AdicionarProfessorDeExemplo("Patrícia Souza", "CREF 000005-G/SP", "(11) 90000-0005", "05/06/1986", "Natação Intermediário", "Natação Iniciante");
            AdicionarProfessorDeExemplo("Bruno Oliveira", "CREF 000006-G/SP", "(11) 90000-0006", "18/09/1989", "Natação Adulto");
            AplicarFiltrosTabela();
        }

        private void AdicionarProfessorDeExemplo(string nome, string cref, string telefone, string nascimento, params string[] turmas) {
            int indice = dgvProfessores.Rows.Add(nome, cref, telefone, nascimento, ResumoTurmas(turmas), "");
            dgvProfessores.Rows[indice].Tag = new List<string>(turmas);
        }

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
                campo.Text = placeholder;
                campo.ForeColor = VisualPadrao.TextoSecundario;
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

        private void MostrarTurmasProfessor(int indiceLinha) {
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

        private void AbrirDialogoProfessor(int indiceLinha) {
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

        private void ExcluirProfessor(int indiceLinha) {
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

        private TextBox CriarCampoDialogo(Form dialogo, string legenda, int y, int largura) {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(24, y), ForeColor = VisualPadrao.TextoPrincipal };
            TextBox campo = new TextBox { Name = "txt" + legenda.Replace(" ", ""), Location = new Point(24, y + 19), Width = largura, Height = 25, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F) };
            dialogo.Controls.Add(label);
            dialogo.Controls.Add(campo);
            return campo;
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

        private static void ConfigurarTabela(DataGridView tabela) {
            VisualPadrao.ConfigurarGrade(tabela);
        }
    }
}
