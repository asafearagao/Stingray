using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Stingray
{
    public partial class HorariosForm : Form
    {
        private readonly Color fundoPagina = Color.FromArgb(250, 251, 252);
        private readonly Color bordaCartao = Color.FromArgb(231, 236, 243);
        private readonly Color bordaCampo = Color.FromArgb(203, 213, 225);
        private readonly Color azul = Color.FromArgb(25, 86, 196);
        private readonly Color texto = Color.FromArgb(55, 65, 81);
        private readonly Color textoSecundario = Color.FromArgb(100, 116, 139);

        private RoundedButton btnLimparFiltros;
        private readonly Dictionary<TextBox, string> placeholdersTxt = new Dictionary<TextBox, string>();

        public HorariosForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ConfigurarAparencia();
        }

        private void ConfigurarAparencia()
        {
            BackColor = fundoPagina;
            pnlHorarios.BackColor = fundoPagina;

            pnlCabecalhoH.BackColor = fundoPagina;
            pnlCabecalhoH.Padding = new Padding(0);

            lblTituloH.AutoSize = true;
            lblTituloH.Location = new Point(13, 17);
            lblTituloH.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTituloH.ForeColor = Color.FromArgb(17, 24, 39);
            lblTituloH.Text = "Horários";

            ConfigurarEstruturaDaTela();

            tblFiltrosH.BackColor = Color.White;
            tblFiltrosH.Padding = new Padding(12, 5, 12, 5);
            tblFiltrosH.Margin = new Padding(0);

            tblFiltrosH.ColumnCount = 5;
            tblFiltrosH.ColumnStyles.Clear();
            tblFiltrosH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tblFiltrosH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblFiltrosH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
            tblFiltrosH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tblFiltrosH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));

            tblFiltrosH.RowCount = 2;
            tblFiltrosH.RowStyles.Clear();
            tblFiltrosH.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tblFiltrosH.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

            tblFiltrosH.SetColumn(lblModalidade, 0);
            tblFiltrosH.SetColumn(lblDiaSemana, 1);
            tblFiltrosH.SetRow(lblModalidade, 0);
            tblFiltrosH.SetRow(lblDiaSemana, 0);

            tblFiltrosH.SetColumn(pnlModalidade, 0);
            tblFiltrosH.SetColumn(pnlDiaSemana, 1);
            tblFiltrosH.SetRow(pnlModalidade, 1);
            tblFiltrosH.SetRow(pnlDiaSemana, 1);

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
                RestaurarPlaceholder(txtModalidade);
                cmbDiaSemana.SelectedIndex = -1;
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
            tblFiltrosH.Controls.Add(btnLimparFiltros, 3, 1);

            if (!tblFiltrosH.Controls.Contains(btnAdicionar))
                tblFiltrosH.Controls.Add(btnAdicionar, 4, 1);
            else
                tblFiltrosH.SetCellPosition(btnAdicionar, new TableLayoutPanelCellPosition(4, 1));

            pnlModalidade.Dock = DockStyle.Fill;
            pnlDiaSemana.Dock = DockStyle.Fill;

            PrepararCampoTexto(pnlModalidade, txtModalidade);
            PrepararCampoCombo(pnlDiaSemana, cmbDiaSemana);

            txtModalidade.ReadOnly = false;

            foreach (Label label in new Label[] { lblModalidade, lblDiaSemana })
            {
                label.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
                label.ForeColor = texto;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Margin = new Padding(0, 0, 0, 0);
            }
            lblDiaSemana.Margin = new Padding(8, 0, 0, 0);

            PreencherDiasDaSemana(cmbDiaSemana);
            ConfigurarPlaceholderTexto(txtModalidade, "Digite a modalidade...");

            ConfigurarBotaoAdicionar();

            txtModalidade.TextChanged += delegate { AplicarFiltrosTabela(); };
            cmbDiaSemana.SelectedIndexChanged += delegate { AplicarFiltrosTabela(); };

            ConfigurarColunaAcoes();
            ConfigurarTabelaVisual(dgvHorarios);
            dgvHorarios.CellPainting += DgvHorarios_CellPainting;
            dgvHorarios.CellMouseClick += DgvHorarios_CellMouseClick;

            colDiaSemana.FillWeight = 110F;
            colModalidade.FillWeight = 160F;
            colHorario.FillWeight = 110F;
            colProfessor.FillWeight = 160F;
            colAlunos.FillWeight = 100F;

            colDiaSemana.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colModalidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colHorario.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colProfessor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colAlunos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            PreencherHorariosDeExemplo();
        }

        private void ConfigurarEstruturaDaTela()
        {
            pnlFiltrosH.Controls.Remove(tblFiltrosH);
            pnlTabelaH.Controls.Remove(dgvHorarios);

            tblHorarios.SuspendLayout();
            tblHorarios.Controls.Remove(pnlFiltrosH);
            tblHorarios.Controls.Remove(pnlTabelaH);
            tblHorarios.RowCount = 2;
            tblHorarios.RowStyles.Clear();
            tblHorarios.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tblHorarios.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlLista = new Panel
            {
                Name = "pnlListaHorarios",
                Dock = DockStyle.Fill,
                Margin = new Padding(16, 0, 16, 16),
                Padding = new Padding(0),
                BackColor = fundoPagina
            };

            TableLayoutPanel tblLista = new TableLayoutPanel
            {
                Name = "tblListaHorarios",
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = fundoPagina,
                ColumnCount = 1
            };
            tblLista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblLista.RowCount = 3;
            tblLista.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tblLista.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tblLista.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlFiltrosCartao = new Panel { Name = "pnlFiltrosHorariosCartao", Dock = DockStyle.Fill, Margin = new Padding(0), Padding = new Padding(1), BackColor = bordaCartao };
            Panel pnlTabelaCartao = new Panel { Name = "pnlTabelaHorariosCartao", Dock = DockStyle.Fill, Margin = new Padding(0), Padding = new Padding(1), BackColor = bordaCartao };

            tblFiltrosH.Dock = DockStyle.Fill;
            tblFiltrosH.Margin = new Padding(0);
            dgvHorarios.Dock = DockStyle.Fill;
            dgvHorarios.Margin = new Padding(0);

            pnlFiltrosCartao.Controls.Add(tblFiltrosH);
            pnlTabelaCartao.Controls.Add(dgvHorarios);
            tblLista.Controls.Add(pnlFiltrosCartao, 0, 0);
            tblLista.Controls.Add(pnlTabelaCartao, 0, 2);
            pnlLista.Controls.Add(tblLista);
            tblHorarios.Controls.Add(pnlLista, 0, 1);
            tblHorarios.ResumeLayout(true);
        }

        private void ConfigurarBotaoAdicionar()
        {
            if (btnAdicionar == null) return;

            btnAdicionar.Text = "+  Novo horário";
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

        private static void ConfigurarTabelaVisual(DataGridView tabela)
        {
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
            tabela.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.ColumnHeadersDefaultCellStyle.BackColor = fundoCabecalho;
            tabela.ColumnHeadersDefaultCellStyle.ForeColor = textoTabela;
            tabela.ColumnHeadersDefaultCellStyle.SelectionBackColor = fundoCabecalho;
            tabela.ColumnHeadersDefaultCellStyle.SelectionForeColor = textoTabela;
            tabela.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
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

        private void ConfigurarColunaAcoes()
        {
            if (dgvHorarios.Columns.Contains("colAcoes")) return;

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
            dgvHorarios.Columns.Add(acoes);
        }

        private void DgvHorarios_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !string.Equals(dgvHorarios.Columns[e.ColumnIndex].Name, "colAcoes", StringComparison.Ordinal))
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
            using (Pen deletePen = new Pen(Color.FromArgb(254, 226, 226)))
            {
                e.Graphics.FillRectangle(editFill, edit);
                e.Graphics.DrawRectangle(editPen, edit);
                e.Graphics.FillRectangle(deleteFill, delete);
                e.Graphics.DrawRectangle(deletePen, delete);

                using (Font actionFont = new Font("Segoe MDL2 Assets", 10F))
                {
                    TextRenderer.DrawText(e.Graphics, "\uE104", actionFont, edit, Color.FromArgb(25, 86, 196), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, "\uE107", actionFont, delete, Color.FromArgb(220, 38, 38), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                }
            }
            e.Handled = true;
        }

        private void PrepararCampoTexto(Panel painel, TextBox campo)
        {
            painel.BackColor = Color.FromArgb(215, 223, 235);
            painel.Padding = new Padding(1);
            painel.Margin = new Padding(8, 1, 0, 1);

            Panel superficie = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0) };
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

        private void PrepararCampoCombo(Panel painel, ComboBox combo)
        {
            painel.BackColor = Color.FromArgb(215, 223, 235);
            painel.Padding = new Padding(1);
            painel.Margin = new Padding(8, 1, 0, 1);

            Panel superficie = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0) };
            painel.Controls.Add(superficie);
            superficie.Controls.Add(combo);

            combo.Enter += delegate { painel.BackColor = azul; };
            combo.Leave += delegate { painel.BackColor = Color.FromArgb(215, 223, 235); };
            combo.BackColor = Color.White;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            combo.ForeColor = texto;
            combo.Dock = DockStyle.None;
            combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Margin = new Padding(0);
            superficie.SizeChanged += delegate { PosicionarCampo(superficie, combo); };
            PosicionarCampo(superficie, combo);
        }

        private void PosicionarCampo(Panel superficie, Control campo)
        {
            if (superficie == null || campo == null) return;
            campo.Location = new Point(6, Math.Max(1, (superficie.ClientSize.Height - campo.Height) / 2));
            campo.Width = Math.Max(30, superficie.ClientSize.Width - campo.Left - 6);
        }

        private void PreencherDiasDaSemana(ComboBox combo)
        {
            combo.Items.Clear();
            combo.Items.AddRange(new string[] { "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado", "Domingo" });
        }

        private void ConfigurarPlaceholderTexto(TextBox campo, string placeholder)
        {
            placeholdersTxt[campo] = placeholder;
            campo.Text = placeholder;
            campo.ForeColor = textoSecundario;

            campo.Enter += delegate {
                if (CampoContemPlaceholder(campo))
                {
                    campo.Clear();
                    campo.ForeColor = texto;
                }
            };
            campo.Leave += delegate {
                if (string.IsNullOrWhiteSpace(campo.Text))
                    RestaurarPlaceholder(campo);
            };
        }

        private bool CampoContemPlaceholder(TextBox campo)
        {
            return placeholdersTxt.TryGetValue(campo, out string placeholder) &&
                   campo.ForeColor == textoSecundario &&
                   string.Equals(campo.Text, placeholder, StringComparison.Ordinal);
        }

        private void RestaurarPlaceholder(TextBox campo)
        {
            if (campo != null && placeholdersTxt.TryGetValue(campo, out string placeholder))
            {
                campo.Text = placeholder;
                campo.ForeColor = textoSecundario;
            }
        }

        private void PreencherHorariosDeExemplo()
        {
            dgvHorarios.Rows.Add("Sexta-feira", "natação A", "08:00 - 09:00", "Mariana Costa", "12");
            dgvHorarios.Rows.Add("Sábado", "hidroginástica", "09:30 - 10:30", "Camila Ferreira", "20");
            dgvHorarios.Rows.Add("Domingo", "natação 8 a 14 anos", "18:00 - 19:00", "Rafael Mendes", "8");
            dgvHorarios.Rows.Add("Segunda-feira", "natação 5 a 7 anos", "14:00 - 15:00", "Lucas Almeida", "15");
            AplicarFiltrosTabela();
        }

        private void AplicarFiltrosTabela()
        {
            if (dgvHorarios == null || dgvHorarios.IsDisposed) return;

            string filtroModalidade = CampoContemPlaceholder(txtModalidade) ? string.Empty : txtModalidade.Text.Trim();
            string filtroDia = cmbDiaSemana.SelectedItem?.ToString() ?? string.Empty;

            foreach (DataGridViewRow linha in dgvHorarios.Rows)
            {
                bool visivel = true;

                if (filtroModalidade.Length > 0)
                {
                    string valModalidade = Convert.ToString(linha.Cells[1].Value) ?? string.Empty;
                    if (valModalidade.IndexOf(filtroModalidade, StringComparison.CurrentCultureIgnoreCase) < 0)
                        visivel = false;
                }

                if (visivel && filtroDia.Length > 0)
                {
                    string valDia = Convert.ToString(linha.Cells[0].Value) ?? string.Empty;
                    if (!string.Equals(valDia, filtroDia, StringComparison.OrdinalIgnoreCase))
                    {
                        visivel = false;
                    }
                }

                if (!visivel && dgvHorarios.CurrentCell != null && dgvHorarios.CurrentCell.RowIndex == linha.Index)
                    dgvHorarios.CurrentCell = null;

                linha.Visible = visivel;
            }
        }

        private void DgvHorarios_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !string.Equals(dgvHorarios.Columns[e.ColumnIndex].Name, "colAcoes", StringComparison.Ordinal))
                return;

            if (e.X < dgvHorarios.Columns[e.ColumnIndex].Width / 2)
                AbrirDialogoHorario(e.RowIndex);
            else
                ExcluirHorario(e.RowIndex);
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            AbrirDialogoHorario(-1);
        }

        private void ExcluirHorario(int indiceLinha)
        {
            if (indiceLinha < 0 || indiceLinha >= dgvHorarios.Rows.Count) return;

            string modalidade = Convert.ToString(dgvHorarios.Rows[indiceLinha].Cells[1].Value) ?? "este horário";
            DialogResult confirmacao = MessageBox.Show(this, "Deseja realmente excluir a aula de " + modalidade + "?", "Excluir horário", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                dgvHorarios.Rows.RemoveAt(indiceLinha);
                AplicarFiltrosTabela();
            }
        }

        private void AbrirDialogoHorario(int indiceLinha)
        {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvHorarios.Rows.Count;

            string diaAtual = editando ? Convert.ToString(dgvHorarios.Rows[indiceLinha].Cells[0].Value) ?? string.Empty : string.Empty;
            string modAtual = editando ? Convert.ToString(dgvHorarios.Rows[indiceLinha].Cells[1].Value) ?? string.Empty : string.Empty;
            string horAtual = editando ? Convert.ToString(dgvHorarios.Rows[indiceLinha].Cells[2].Value) ?? string.Empty : string.Empty;
            string profAtual = editando ? Convert.ToString(dgvHorarios.Rows[indiceLinha].Cells[3].Value) ?? string.Empty : string.Empty;
            string aluAtual = editando ? Convert.ToString(dgvHorarios.Rows[indiceLinha].Cells[4].Value) ?? string.Empty : string.Empty;

            using (Form dialogo = new Form())
            {
                dialogo.Text = editando ? "Editar horário" : "Novo horário";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.FromArgb(247, 249, 252);
                dialogo.ClientSize = new Size(480, 420);
                dialogo.Font = new Font("Segoe UI", 9F);

                Label titulo = new Label { Text = editando ? "Editar horário" : "Adicionar horário", Font = new Font("Segoe UI Semibold", 14F), ForeColor = Color.FromArgb(17, 24, 39), AutoSize = true, Location = new Point(22, 18) };
                Label descricao = new Label { Text = editando ? "Atualize os dados desta aula." : "Preencha os dados da nova aula.", ForeColor = textoSecundario, AutoSize = true, Location = new Point(23, 48) };

                string[] opcoesDias = new string[] { "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado", "Domingo" };
                ComboBox cmbDia = CriarComboDialogo(dialogo, "Dia da Semana", 82, 180, opcoesDias);

                TextBox horario = CriarCampoDialogo(dialogo, "Horário (ex: 14:00 - 15:00)", 82, 180, 240);

                string[] opcoesModalidade = new string[] { "Hidroginástica", "Natação A", "Natação (8 a 14 anos)", "Natação (5 a 7 anos)" };
                ComboBox cmbMod = CriarComboDialogo(dialogo, "Modalidade", 132, 420, opcoesModalidade);

                TextBox professor = CriarCampoDialogo(dialogo, "Professor", 182, 420);
                TextBox alunos = CriarCampoDialogo(dialogo, "Total de Alunos", 232, 420);

                if (editando)
                {
                    cmbDia.SelectedItem = diaAtual;
                    cmbMod.SelectedItem = modAtual;
                }
                horario.Text = horAtual;
                professor.Text = profAtual;
                alunos.Text = aluAtual;

                RoundedButton cancelar = new RoundedButton { Text = "Cancelar", CornerRadius = 5, Size = new Size(100, 36), Location = new Point(editando ? 218 : 250, 310), BackColor = Color.White, ForeColor = texto, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                cancelar.FlatAppearance.BorderColor = bordaCampo;
                cancelar.FlatAppearance.BorderSize = 1;
                cancelar.DialogResult = DialogResult.Cancel;

                RoundedButton salvar = new RoundedButton { Text = editando ? "Salvar alterações" : "Adicionar", CornerRadius = 5, Size = new Size(editando ? 142 : 112, 36), Location = new Point(editando ? 328 : 358, 310), BackColor = azul, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                salvar.FlatAppearance.BorderSize = 0;
                salvar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
                salvar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);

                salvar.Click += delegate {
                    if (cmbDia.SelectedIndex == -1)
                    {
                        MessageBox.Show(dialogo, "Selecione o dia da semana.", "Cadastro de horário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDia.Focus();
                        return;
                    }
                    if (cmbMod.SelectedIndex == -1)
                    {
                        MessageBox.Show(dialogo, "Selecione a modalidade.", "Cadastro de horário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbMod.Focus();
                        return;
                    }

                    string diaEscolhido = cmbDia.SelectedItem.ToString();
                    string modEscolhida = cmbMod.SelectedItem.ToString();

                    if (editando)
                    {
                        DataGridViewRow linha = dgvHorarios.Rows[indiceLinha];
                        linha.Cells[0].Value = diaEscolhido;
                        linha.Cells[1].Value = modEscolhida;
                        linha.Cells[2].Value = horario.Text.Trim();
                        linha.Cells[3].Value = professor.Text.Trim();
                        linha.Cells[4].Value = alunos.Text.Trim();
                    }
                    else
                    {
                        dgvHorarios.Rows.Add(diaEscolhido, modEscolhida, horario.Text.Trim(), professor.Text.Trim(), alunos.Text.Trim());
                    }

                    AplicarFiltrosTabela();
                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };

                dialogo.Controls.AddRange(new Control[] { titulo, descricao, cancelar, salvar });
                dialogo.AcceptButton = salvar;
                dialogo.CancelButton = cancelar;
                dialogo.Shown += delegate { cmbDia.Focus(); };
                dialogo.ShowDialog(this);
            }
        }

        private TextBox CriarCampoDialogo(Form dialogo, string legenda, int y, int largura, int x = 24)
        {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(x, y), ForeColor = texto };
            TextBox campo = new TextBox { Name = "txt" + legenda.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(":", "").Replace("/", ""), Location = new Point(x, y + 19), Width = largura, Height = 25, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F) };
            dialogo.Controls.Add(label);
            dialogo.Controls.Add(campo);
            return campo;
        }

        private ComboBox CriarComboDialogo(Form dialogo, string legenda, int y, int largura, string[] itens, int x = 24)
        {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(x, y), ForeColor = texto };
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
            dialogo.Controls.Add(label);
            dialogo.Controls.Add(combo);
            return combo;
        }
    }
}