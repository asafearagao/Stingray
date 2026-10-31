using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    public partial class ProfessoresForm : Form {
        // Configura a aparência da tabela de professores
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
            dgvProfessores.CellPainting += AoDesenharCelulaDaTabela;
            dgvProfessores.CellMouseClick += AoClicarNaCelulaDaTabela;
            dgvProfessores.CellMouseMove += AoMoverMouseNaCelulaDaTabela;
            dgvProfessores.CellMouseLeave += AoSairDaCelulaDaTabela;
            dgvProfessores.CellMouseDown += AoPressionarMouseNaCelulaDaTabela;
            dgvProfessores.CellMouseUp += AoSoltarMouseNaCelulaDaTabela;
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

        // Configura a coluna que contém as ações disponíveis para cada professor
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