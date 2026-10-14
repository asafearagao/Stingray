using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Stingray {
    public partial class ProfessoresForm : Form {
        private readonly Color fundoPagina = Color.FromArgb(250, 251, 252);
        private readonly Color bordaCartao = Color.FromArgb(231, 236, 243);
        private readonly Color bordaCampo = Color.FromArgb(203, 213, 225);
        private readonly Color azul = Color.FromArgb(25, 86, 196);
        private readonly Color texto = Color.FromArgb(55, 65, 81);
        private readonly Color textoSecundario = Color.FromArgb(100, 116, 139);

        private Panel pnlCadastro;
        private TextBox txtCadastroNome;
        private TextBox txtCadastroCref;
        private TextBox txtCadastroTelefone;
        private TextBox txtCadastroNascimento;
        private RoundedButton btnLimparFiltros;
        private readonly Dictionary<TextBox, string> placeholders = new Dictionary<TextBox, string>();

        public ProfessoresForm() {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ConfigurarAparencia();
        }

        private void ConfigurarAparencia() {
            BackColor = fundoPagina;
            pnlProfessores.BackColor = fundoPagina;
            pnlCabecalho.BackColor = fundoPagina;
            pnlCabecalho.Padding = new Padding(0);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(13, 17);
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitulo.Text = "Professores";

            ConfigurarEstruturaDaTela();

            tblFiltros.BackColor = Color.White;
            tblFiltros.Padding = new Padding(12, 5, 12, 5);
            tblFiltros.Margin = new Padding(0);
            tblFiltros.Controls.Remove(lblDataNascimento);
            tblFiltros.Controls.Remove(pnlDataNascimento);
            tblFiltros.ColumnCount = 5;
            tblFiltros.ColumnStyles.Clear();
            tblFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tblFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tblFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21F));
            tblFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tblFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            tblFiltros.RowCount = 2;
            tblFiltros.RowStyles.Clear();
            tblFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tblFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

            tblFiltros.SetColumn(lblNome, 0);
            tblFiltros.SetColumn(lblCref, 1);
            tblFiltros.SetColumn(lblTelefone, 2);
            tblFiltros.SetRow(lblNome, 0);
            tblFiltros.SetRow(lblCref, 0);
            tblFiltros.SetRow(lblTelefone, 0);
            tblFiltros.SetColumn(pnlNome, 0);
            tblFiltros.SetColumn(pnlCref, 1);
            tblFiltros.SetColumn(pnlTelefone, 2);
            tblFiltros.SetRow(pnlNome, 1);
            tblFiltros.SetRow(pnlCref, 1);
            tblFiltros.SetRow(pnlTelefone, 1);

            btnLimparFiltros = new RoundedButton();
            btnLimparFiltros.Name = "btnLimparFiltros";
            btnLimparFiltros.Text = "Limpar filtros";
            btnLimparFiltros.CornerRadius = 6;
            btnLimparFiltros.BackColor = Color.White;
            btnLimparFiltros.ForeColor = textoSecundario;
            btnLimparFiltros.FlatAppearance.BorderSize = 0;
            btnLimparFiltros.FlatAppearance.BorderColor = bordaCampo;
            btnLimparFiltros.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnLimparFiltros.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            btnLimparFiltros.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
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
            };
            btnLimparFiltros.MouseLeave += delegate {
                btnLimparFiltros.ForeColor = textoSecundario;
                btnLimparFiltros.FlatAppearance.BorderColor = bordaCampo;
            };
            tblFiltros.Controls.Add(btnLimparFiltros, 3, 1);
            if (!tblFiltros.Controls.Contains(btnAdicionar))
                tblFiltros.Controls.Add(btnAdicionar, 4, 1);
            else
                tblFiltros.SetCellPosition(btnAdicionar, new TableLayoutPanelCellPosition(4, 1));

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
                label.ForeColor = texto;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Margin = new Padding(0, 0, 0, 0);
            }
            lblCref.Margin = new Padding(8, 0, 0, 0);
            lblTelefone.Margin = new Padding(8, 0, 0, 0);

            ConfigurarPlaceholder(txtNome, "Digite o nome do professor...");
            ConfigurarPlaceholder(txtCref, "Digite o CREF...");
            ConfigurarPlaceholder(txtTelefone, "Digite o telefone...");
            ConfigurarBotaoAdicionar();
            ConfigurarFiltrosTabela();
            ConfigurarColunaAcoes();
            ConfigurarTabela(dgvProfessores);
            dgvProfessores.CellPainting += DgvProfessores_CellPainting;
            dgvProfessores.CellMouseClick += DgvProfessores_CellMouseClick;
            colNome.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colNome.FillWeight = 155F;
            colCref.FillWeight = 125F;
            colTelefone.FillWeight = 115F;
            colDataNascimento.FillWeight = 130F;
            colTurmas.FillWeight = 155F;
            colCref.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTelefone.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDataNascimento.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTurmas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            PreencherProfessoresDeExemplo();
        }

        private void ConfigurarEstruturaDaTela() {
            pnlFiltros.Controls.Remove(tblFiltros);
            pnlTabela.Controls.Remove(dgvProfessores);

            tblProfessores.SuspendLayout();
            tblProfessores.Controls.Remove(pnlFiltros);
            tblProfessores.Controls.Remove(pnlTabela);
            tblProfessores.RowCount = 2;
            tblProfessores.RowStyles.Clear();
            tblProfessores.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tblProfessores.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlLista = new Panel();
            pnlLista.Name = "pnlListaProfessores";
            pnlLista.Dock = DockStyle.Fill;
            pnlLista.Margin = new Padding(16, 0, 16, 16);
            pnlLista.Padding = new Padding(0);
            pnlLista.BackColor = fundoPagina;

            TableLayoutPanel tblLista = new TableLayoutPanel();
            tblLista.Name = "tblListaProfessores";
            tblLista.Dock = DockStyle.Fill;
            tblLista.Margin = new Padding(0);
            tblLista.Padding = new Padding(0);
            tblLista.BackColor = fundoPagina;
            tblLista.ColumnCount = 1;
            tblLista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblLista.RowCount = 3;
            tblLista.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tblLista.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tblLista.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlFiltrosCartao = new Panel();
            pnlFiltrosCartao.Name = "pnlFiltrosProfessoresCartao";
            pnlFiltrosCartao.Dock = DockStyle.Fill;
            pnlFiltrosCartao.Margin = new Padding(0);
            pnlFiltrosCartao.Padding = new Padding(1);
            pnlFiltrosCartao.BackColor = bordaCartao;

            Panel pnlTabelaCartao = new Panel();
            pnlTabelaCartao.Name = "pnlTabelaProfessoresCartao";
            pnlTabelaCartao.Dock = DockStyle.Fill;
            pnlTabelaCartao.Margin = new Padding(0);
            pnlTabelaCartao.Padding = new Padding(1);
            pnlTabelaCartao.BackColor = bordaCartao;

            tblFiltros.Dock = DockStyle.Fill;
            tblFiltros.Margin = new Padding(0);
            dgvProfessores.Dock = DockStyle.Fill;
            dgvProfessores.Margin = new Padding(0);

            pnlFiltrosCartao.Controls.Add(tblFiltros);
            pnlTabelaCartao.Controls.Add(dgvProfessores);
            tblLista.Controls.Add(pnlFiltrosCartao, 0, 0);
            tblLista.Controls.Add(pnlTabelaCartao, 0, 2);
            pnlLista.Controls.Add(tblLista);
            tblProfessores.Controls.Add(pnlLista, 0, 1);
            tblProfessores.ResumeLayout(true);
        }

        private Panel CriarAreaCadastro() {
            Panel cartao = new Panel();
            cartao.Name = "pnlCadastroProfessor";
            cartao.Dock = DockStyle.Fill;
            cartao.Margin = new Padding(16, 6, 16, 14);
            cartao.Padding = new Padding(1);
            cartao.BackColor = bordaCartao;

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
            descricao.ForeColor = textoSecundario;
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
                label.ForeColor = texto;
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
            btnAdicionar.BackColor = azul;
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.FlatAppearance.BorderColor = azul;
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
                ForeColor = texto,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                TabStop = true
            };
        }

        private Panel CriarCampoCadastro(TextBox campo) {
            Panel painel = new Panel();
            painel.Name = "pnl" + campo.Name;
            painel.BackColor = Color.FromArgb(215, 223, 235);
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
            campo.Enter += delegate { painel.BackColor = azul; };
            campo.Leave += delegate { painel.BackColor = Color.FromArgb(215, 223, 235); };
            superficie.SizeChanged += delegate { PosicionarCampo(superficie, campo); };
            PosicionarCampo(superficie, campo);
            return painel;
        }

        private void PreencherProfessoresDeExemplo() {
            // Registros fictícios de demonstração
            dgvProfessores.Rows.Add("Mariana Costa", "CREF 000001-G/SP", "(11) 90000-0001", "14/03/1987", "Natação Iniciante", "");
            dgvProfessores.Rows.Add("Rafael Mendes", "CREF 000002-G/SP", "(11) 90000-0002", "22/07/1990", "Natação Avançado", "");
            dgvProfessores.Rows.Add("Camila Ferreira", "CREF 000003-G/SP", "(11) 90000-0003", "09/11/1984", "Hidroginástica", "");
            dgvProfessores.Rows.Add("Lucas Almeida", "CREF 000004-G/SP", "(11) 90000-0004", "30/01/1992", "Natação Infantil", "");
            dgvProfessores.Rows.Add("Patrícia Souza", "CREF 000005-G/SP", "(11) 90000-0005", "05/06/1986", "Natação Intermediário", "");
            dgvProfessores.Rows.Add("Bruno Oliveira", "CREF 000006-G/SP", "(11) 90000-0006", "18/09/1989", "Natação Adulto", "");
            AplicarFiltrosTabela();
        }

        private void ConfigurarBotaoAdicionar() {
            if (btnAdicionar == null)
                return;

            btnAdicionar.Text = "+  Novo professor";
            btnAdicionar.CornerRadius = 5;
            btnAdicionar.BackColor = azul;
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.FlatAppearance.BorderColor = azul;
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
            campo.ForeColor = textoSecundario;

            campo.Enter += delegate {
                if (CampoContemPlaceholder(campo)) {
                    campo.Clear();
                    campo.ForeColor = texto;
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
                   campo.ForeColor == textoSecundario &&
                   string.Equals(campo.Text, placeholder, StringComparison.Ordinal);
        }

        private string ObterValorFiltro(TextBox campo) {
            return CampoContemPlaceholder(campo) ? string.Empty : campo.Text.Trim();
        }

        private void RestaurarPlaceholder(TextBox campo) {
            string placeholder;
            if (campo != null && placeholders.TryGetValue(campo, out placeholder)) {
                campo.Text = placeholder;
                campo.ForeColor = textoSecundario;
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                !string.Equals(dgvProfessores.Columns[e.ColumnIndex].Name, "colAcoes", StringComparison.Ordinal))
                return;

            if (e.X < dgvProfessores.Columns[e.ColumnIndex].Width / 2)
                AbrirDialogoProfessor(e.RowIndex);
            else
                ExcluirProfessor(e.RowIndex);
        }

        private void AbrirDialogoProfessor(int indiceLinha) {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvProfessores.Rows.Count;
            string nomeAtual = editando ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[0].Value) ?? string.Empty : string.Empty;
            string crefAtual = editando ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[1].Value) ?? string.Empty : string.Empty;
            string telefoneAtual = editando ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[2].Value) ?? string.Empty : string.Empty;
            string nascimentoAtual = editando ? Convert.ToString(dgvProfessores.Rows[indiceLinha].Cells[3].Value) ?? string.Empty : string.Empty;

            using (Form dialogo = new Form()) {
                dialogo.Text = editando ? "Editar professor" : "Novo professor";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.FromArgb(247, 249, 252);
                dialogo.ClientSize = new Size(480, 365);
                dialogo.Font = new Font("Segoe UI", 9F);

                Label titulo = new Label { Text = editando ? "Editar professor" : "Adicionar professor", Font = new Font("Segoe UI Semibold", 14F), ForeColor = Color.FromArgb(17, 24, 39), AutoSize = true, Location = new Point(22, 18) };
                Label descricao = new Label { Text = editando ? "Atualize os dados deste docente." : "Preencha os dados do novo docente.", ForeColor = textoSecundario, AutoSize = true, Location = new Point(23, 48) };
                TextBox nome = CriarCampoDialogo(dialogo, "Nome", 82, 420);
                TextBox cref = CriarCampoDialogo(dialogo, "CREF", 132, 420);
                TextBox telefone = CriarCampoDialogo(dialogo, "Telefone", 182, 200);
                TextBox nascimento = CriarCampoDialogo(dialogo, "Data de nascimento (dd/MM/aaaa)", 232, 200);
                nome.Text = nomeAtual;
                cref.Text = crefAtual;
                telefone.Text = telefoneAtual;
                nascimento.Text = nascimentoAtual;

                RoundedButton cancelar = new RoundedButton { Text = "Cancelar", CornerRadius = 5, Size = new Size(100, 36), Location = new Point(editando ? 218 : 250, 310), BackColor = Color.White, ForeColor = texto, Cursor = Cursors.Hand };
                cancelar.FlatStyle = FlatStyle.Flat;
                cancelar.FlatAppearance.BorderColor = bordaCampo;
                cancelar.FlatAppearance.BorderSize = 1;
                cancelar.DialogResult = DialogResult.Cancel;
                RoundedButton salvar = new RoundedButton { Text = editando ? "Salvar alterações" : "Adicionar", CornerRadius = 5, Size = new Size(editando ? 142 : 112, 36), Location = new Point(editando ? 328 : 358, 310), BackColor = azul, ForeColor = Color.White, Cursor = Cursors.Hand };
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

                    if (editando) {
                        DataGridViewRow linha = dgvProfessores.Rows[indiceLinha];
                        linha.Cells[0].Value = nome.Text.Trim();
                        linha.Cells[1].Value = cref.Text.Trim();
                        linha.Cells[2].Value = telefone.Text.Trim();
                        linha.Cells[3].Value = dataFormatada;
                    } else {
                        dgvProfessores.Rows.Add(nome.Text.Trim(), cref.Text.Trim(), telefone.Text.Trim(), dataFormatada, "—", "");
                    }

                    AplicarFiltrosTabela();
                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };
                dialogo.Controls.AddRange(new Control[] { titulo, descricao, cancelar, salvar });
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
            acoes.DefaultCellStyle.SelectionForeColor = texto;
            dgvProfessores.Columns.Add(acoes);
        }

        private void DgvProfessores_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                !string.Equals(dgvProfessores.Columns[e.ColumnIndex].Name, "colAcoes", StringComparison.Ordinal))
                return;

            e.PaintBackground(e.CellBounds, true);

            const int actionSize = 26;
            const int actionGap = 7;
            int groupWidth = actionSize * 2 + actionGap;
            int startX = e.CellBounds.Left + Math.Max(2, (e.CellBounds.Width - groupWidth) / 2);
            int startY = e.CellBounds.Top + Math.Max(2, (e.CellBounds.Height - actionSize) / 2);
            Rectangle edit = new Rectangle(startX, startY, actionSize, actionSize);
            Rectangle delete = new Rectangle(edit.Right + actionGap, startY, actionSize, actionSize);

            using (Brush editFill = new SolidBrush(Color.FromArgb(247, 250, 255)))
            using (Pen editPen = new Pen(Color.FromArgb(219, 234, 254)))
            using (Brush deleteFill = new SolidBrush(Color.FromArgb(255, 248, 248)))
            using (Pen deletePen = new Pen(Color.FromArgb(254, 226, 226))) {
                e.Graphics.FillRectangle(editFill, edit);
                e.Graphics.DrawRectangle(editPen, edit);
                e.Graphics.FillRectangle(deleteFill, delete);
                e.Graphics.DrawRectangle(deletePen, delete);

                using (Font actionFont = new Font("Segoe MDL2 Assets", 10F)) {
                    TextRenderer.DrawText(e.Graphics, "\uE104", actionFont, edit,
                        Color.FromArgb(25, 86, 196), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, "\uE107", actionFont, delete,
                        Color.FromArgb(220, 38, 38), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                }
            }

            e.Handled = true;
        }

        private TextBox CriarCampoDialogo(Form dialogo, string legenda, int y, int largura) {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(24, y), ForeColor = texto };
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
                if (visivel) exibidos++;
            }
        }

        private void PrepararCampo(Panel painel, TextBox campo) {
            painel.BackColor = Color.FromArgb(215, 223, 235);
            painel.Padding = new Padding(1);

            Panel superficie = new Panel();
            superficie.Dock = DockStyle.Fill;
            superficie.BackColor = Color.White;
            superficie.Margin = new Padding(0);
            painel.Controls.Add(superficie);
            superficie.Controls.Add(campo);

            campo.Enter += delegate { painel.BackColor = azul; };
            campo.Leave += delegate { painel.BackColor = Color.FromArgb(215, 223, 235); };
            campo.BackColor = Color.White;
            campo.BorderStyle = BorderStyle.None;
            campo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            campo.ForeColor = textoSecundario;
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
            Color branco = Color.White;
            Color fundoCabecalho = Color.FromArgb(247, 249, 252);
            Color textoTabela = Color.FromArgb(55, 65, 81);
            Color linhas = Color.FromArgb(235, 239, 245);

            tabela.BackgroundColor = branco;
            tabela.BorderStyle = BorderStyle.None;
            tabela.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            tabela.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            tabela.EnableHeadersVisualStyles = false;
            tabela.ColumnHeadersHeight = 42;
            tabela.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tabela.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tabela.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.ColumnHeadersDefaultCellStyle.BackColor = fundoCabecalho;
            tabela.ColumnHeadersDefaultCellStyle.ForeColor = textoTabela;
            tabela.ColumnHeadersDefaultCellStyle.SelectionBackColor = fundoCabecalho;
            tabela.ColumnHeadersDefaultCellStyle.SelectionForeColor = textoTabela;
            tabela.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            tabela.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            tabela.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            tabela.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.DefaultCellStyle.BackColor = branco;
            tabela.DefaultCellStyle.ForeColor = textoTabela;
            tabela.DefaultCellStyle.SelectionBackColor = branco;
            tabela.DefaultCellStyle.SelectionForeColor = textoTabela;
            tabela.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            tabela.DefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
            tabela.AlternatingRowsDefaultCellStyle.BackColor = branco;
            tabela.AlternatingRowsDefaultCellStyle.ForeColor = textoTabela;
            tabela.AlternatingRowsDefaultCellStyle.SelectionBackColor = branco;
            tabela.AlternatingRowsDefaultCellStyle.SelectionForeColor = textoTabela;
            tabela.RowsDefaultCellStyle.BackColor = branco;
            tabela.RowsDefaultCellStyle.ForeColor = textoTabela;
            tabela.GridColor = linhas;
            tabela.RowTemplate.Height = 42;
            tabela.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabela.RowHeadersVisible = false;
            tabela.AllowUserToAddRows = false;
            tabela.AllowUserToDeleteRows = false;
            tabela.AllowUserToResizeRows = false;
            tabela.AllowUserToResizeColumns = false;
            tabela.AllowUserToOrderColumns = false;
            foreach (DataGridViewColumn coluna in tabela.Columns)
                coluna.Resizable = DataGridViewTriState.False;
            tabela.MultiSelect = false;
            tabela.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabela.ClearSelection();
        }
    }
}