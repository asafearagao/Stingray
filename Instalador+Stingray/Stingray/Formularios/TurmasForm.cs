using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Servicos;
using Stingray.Entidades;
using Stingray.DadosTela;

namespace Stingray {
    [DesignerCategory("Form")]

    // Controla a consulta, filtragem e manutenção das turmas
    public partial class TurmasForm : Form {
        private RoundedButton btnLimparFiltros;
        private int linhaAcaoSobMouseTurmas = -1;
        private int botaoAcaoSobMouseTurmas = -1;
        private int linhaBotaoPressionadoTurmas = -1;
        private int botaoPressionadoTurmas = -1;
        private readonly Dictionary<TextBox, string> placeholdersTxt = new Dictionary<TextBox, string>();
        private readonly TurmaServico turmaServico = new TurmaServico();
        private readonly ProfessorServico professorServico = new ProfessorServico();

        public TurmasForm() {
            InitializeComponent();
            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);
            ConfigurarAparencia();
            if (emDesignTime)
                return;

            CarregarTurmas();
        }

        // Carrega as turmas cadastradas
        private void CarregarTurmas() {
            try {
                List<Turma> turmas = turmaServico.ListarTodos();
                dgvTurmas.Rows.Clear();

                foreach (Turma turma in turmas) {
                    RegistroAlunosTurma registro = new RegistroAlunosTurma {
                        IdTurma = turma.IdTurma,
                        IdProfessor = turma.IdProfessor,
                        Capacidade = turma.Capacidade
                    };

                    int indice = dgvTurmas.Rows.Add(
                        turma.DiaSemana,
                        turma.Modalidade,
                        turma.HorarioInicio.ToString(@"hh\:mm") + " - " + turma.HorarioFim.ToString(@"hh\:mm"),
                        turma.NomeProfessor,
                        turma.ValorMensal.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")),
                        turma.QuantidadeParticipantes.ToString(),
                        string.Empty);
                    dgvTurmas.Rows[indice].Tag = registro;
                    dgvTurmas.Rows[indice].Cells["colAlunos"].ToolTipText =
                        turma.QuantidadeParticipantes + " participantes de " + turma.Capacidade + " vagas";
                }

                AplicarFiltrosTabela();
            }
            catch (Exception erro) {
                MessageBox.Show(this,
                    "Não foi possível carregar as turmas do banco de dados.\n\n" + erro.Message,
                    "Consulta de turmas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarAparencia() {
            BackColor = VisualPadrao.FundoPagina;
            pnlTurmas.BackColor = VisualPadrao.FundoPagina;

            pnlCabecalhoTurmas.BackColor = VisualPadrao.FundoPagina;
            pnlCabecalhoTurmas.Padding = new Padding(0);

            lblTituloTurmas.AutoSize = true;
            lblTituloTurmas.Location = new Point(13, 17);
            lblTituloTurmas.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTituloTurmas.ForeColor = Color.FromArgb(17, 24, 39);
            lblTituloTurmas.Text = "Turmas";

            tblFiltrosTurmas.BackColor = Color.White;
            tblFiltrosTurmas.Padding = new Padding(12, 5, 12, 5);
            tblFiltrosTurmas.Margin = new Padding(0);

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
                RestaurarPlaceholder(txtModalidade);
                cmbDiaSemana.SelectedIndex = 0;
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

            pnlModalidade.Dock = DockStyle.Fill;
            pnlDiaSemana.Dock = DockStyle.Fill;

            PrepararCampoTexto(pnlModalidade, txtModalidade);
            PrepararCampoCombo(pnlDiaSemana, cmbDiaSemana);

            txtModalidade.ReadOnly = false;

            foreach (Label label in new Label[] { lblModalidade, lblDiaSemana }) {
                label.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
                label.ForeColor = VisualPadrao.TextoPrincipal;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Margin = new Padding(0, 0, 0, 0);
            }
            // Alinhar os dois rótulos com o início visual dos respectivos campos
            lblModalidade.Margin = new Padding(0);
            lblDiaSemana.Margin = new Padding(0);

            PreencherDiasDaSemana(cmbDiaSemana);
            cmbDiaSemana.SelectedIndex = 0;
            cmbDiaSemana.ForeColor = VisualPadrao.TextoSecundario;
            ConfigurarPlaceholderTexto(txtModalidade, "Digite a modalidade...");

            ConfigurarBotaoAdicionar();

            txtModalidade.TextChanged += delegate { AplicarFiltrosTabela(); };
            cmbDiaSemana.SelectedIndexChanged += delegate {
                cmbDiaSemana.ForeColor = cmbDiaSemana.SelectedIndex <= 0 ? VisualPadrao.TextoSecundario : VisualPadrao.TextoPrincipal;
                AplicarFiltrosTabela();
            };

            ConfigurarColunaAcoes();
            ConfigurarTabelaVisual(dgvTurmas);
            dgvTurmas.CellPainting += DgvTurmas_CellPainting;
            dgvTurmas.CellMouseClick += DgvTurmas_CellMouseClick;
            dgvTurmas.CellMouseMove += DgvTurmas_CellMouseMove;
            dgvTurmas.CellMouseLeave += DgvTurmas_CellMouseLeave;
            dgvTurmas.CellMouseDown += DgvTurmas_CellMouseDown;
            dgvTurmas.CellMouseUp += DgvTurmas_CellMouseUp;

            colDiaSemana.FillWeight = 110F;
            colModalidade.FillWeight = 160F;
            colHorario.FillWeight = 110F;
            colProfessor.FillWeight = 145F;
            colMensalidade.FillWeight = 95F;
            colAlunos.FillWeight = 75F;
            colAlunos.ToolTipText = "Clique na lupa para gerenciar os alunos participantes";

            colDiaSemana.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colModalidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHorario.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colProfessor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colAlunos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void ConfigurarBotaoAdicionar() {
            if (btnAdicionar == null)
                return;

            btnAdicionar.Text = "+  Nova turma";
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

        // Prepara as colunas e os estilos da lista de turmas
        private static void ConfigurarTabelaVisual(DataGridView tabela) {
            VisualPadrao.ConfigurarGrade(tabela);
        }

        private void ConfigurarColunaAcoes() {
            if (dgvTurmas.Columns.Contains("colAcoes"))
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
            dgvTurmas.Columns.Add(acoes);
        }

        private void DgvTurmas_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvTurmas.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colAlunos")
                return;

            e.PaintBackground(e.CellBounds, true);
            if (coluna == "colAlunos") {
                Rectangle lupa = ObterRetanguloLupaTurmas(e.CellBounds);
                bool hoverLupa = e.RowIndex == linhaAcaoSobMouseTurmas && botaoAcaoSobMouseTurmas == 2;
                bool pressionada = e.RowIndex == linhaBotaoPressionadoTurmas && botaoPressionadoTurmas == 2;
                Color fundoLupa;
                Color bordaLupa;
                Color iconeLupa;
                VisualPadrao.ObterCoresAcao(false, false, hoverLupa, pressionada, out fundoLupa, out bordaLupa, out iconeLupa);
                using (Brush fundo = new SolidBrush(fundoLupa))
                using (Pen borda = new Pen(bordaLupa)) {
                    e.Graphics.FillRectangle(fundo, lupa);
                    e.Graphics.DrawRectangle(borda, lupa);
                }
                using (Font fonteLupa = new Font("Segoe MDL2 Assets", 10F))
                    TextRenderer.DrawText(e.Graphics, "\uE721", fonteLupa, lupa,
                        iconeLupa, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                e.Handled = true;
                return;
            }

            Rectangle[] botoes = ObterRetangulosBotoesAcaoTurmas(e.CellBounds);
            for (int i = 0; i < botoes.Length; i++) {
                bool sobMouse = e.RowIndex == linhaAcaoSobMouseTurmas && i == botaoAcaoSobMouseTurmas;
                bool pressionado = e.RowIndex == linhaBotaoPressionadoTurmas && i == botaoPressionadoTurmas;
                Color fundo;
                Color borda;
                Color icone;
                VisualPadrao.ObterCoresAcao(i == 1, false, sobMouse, pressionado, out fundo, out borda, out icone);
                using (Brush pincel = new SolidBrush(fundo))
                using (Pen caneta = new Pen(borda)) {
                    e.Graphics.FillRectangle(pincel, botoes[i]);
                    e.Graphics.DrawRectangle(caneta, botoes[i]);
                }
                using (Font fonte = new Font("Segoe MDL2 Assets", 10F))
                    TextRenderer.DrawText(e.Graphics, i == 0
                        ? "\uE104"
                        : "\uE107", fonte, botoes[i], icone,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            e.Handled = true;
        }

        private Rectangle[] ObterRetangulosBotoesAcaoTurmas(Rectangle celula) {
            const int tamanho = 26;
            const int espaco = 7;
            int largura = tamanho * 2 + espaco;
            int x = celula.Left + Math.Max(2, (celula.Width - largura) / 2);
            int y = celula.Top + Math.Max(2, (celula.Height - tamanho) / 2);
            return new Rectangle[] { new Rectangle(x, y, tamanho, tamanho), new Rectangle(x + tamanho + espaco, y, tamanho, tamanho) };
        }

        private Rectangle ObterRetanguloLupaTurmas(Rectangle celula) {
            const int tamanho = 26;
            return new Rectangle(celula.Left + (celula.Width - tamanho) / 2, celula.Top + (celula.Height - tamanho) / 2, tamanho, tamanho);
        }

        private int IndiceBotaoAcaoSobMouseTurmas(int coluna, int linha) {
            if (coluna < 0 || linha < 0)
                return -1;
            Rectangle celula = dgvTurmas.GetCellDisplayRectangle(coluna, linha, false);
            Point mouse = dgvTurmas.PointToClient(Cursor.Position);
            Point local = new Point(mouse.X - celula.Left, mouse.Y - celula.Top);
            if (dgvTurmas.Columns[coluna].Name == "colAlunos")
                return ObterRetanguloLupaTurmas(new Rectangle(0, 0, celula.Width, celula.Height)).Contains(local) ? 2 : -1;
            Rectangle[] botoes = ObterRetangulosBotoesAcaoTurmas(new Rectangle(0, 0, celula.Width, celula.Height));
            for (int i = 0; i < botoes.Length; i++)
                if (botoes[i].Contains(local))
                    return i;

            return -1;
        }

        private void DgvTurmas_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) {
                LimparHoverAcoesTurmas();
                return;
            }
            string coluna = dgvTurmas.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colAlunos") {
                LimparHoverAcoesTurmas();
                return;
            }
            int botao = IndiceBotaoAcaoSobMouseTurmas(e.ColumnIndex, e.RowIndex);
            if (linhaAcaoSobMouseTurmas == e.RowIndex && botaoAcaoSobMouseTurmas == botao)
                return;
            LimparHoverAcoesTurmas();
            linhaAcaoSobMouseTurmas = e.RowIndex;
            botaoAcaoSobMouseTurmas = botao;
            dgvTurmas.Cursor = botao >= 0
                ? Cursors.Hand
                : Cursors.Default;
            dgvTurmas.InvalidateCell(e.ColumnIndex, e.RowIndex);
        }

        private void DgvTurmas_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == linhaAcaoSobMouseTurmas && e.ColumnIndex >= 0 &&
                (dgvTurmas.Columns[e.ColumnIndex].Name == "colAcoes" || dgvTurmas.Columns[e.ColumnIndex].Name == "colAlunos"))
                LimparHoverAcoesTurmas();
        }

        private void DgvTurmas_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvTurmas.Columns[e.ColumnIndex].Name;
            if (coluna != "colAcoes" && coluna != "colAlunos")
                return;
            int botao = IndiceBotaoAcaoSobMouseTurmas(e.ColumnIndex, e.RowIndex);
            if (botao < 0)
                return;
            linhaBotaoPressionadoTurmas = e.RowIndex; botaoPressionadoTurmas = botao;
            dgvTurmas.InvalidateCell(e.ColumnIndex, e.RowIndex);
            dgvTurmas.Cursor = Cursors.Hand;
        }

        private void DgvTurmas_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e) {
            int linha = linhaBotaoPressionadoTurmas; int botao = botaoPressionadoTurmas;
            linhaBotaoPressionadoTurmas = -1; botaoPressionadoTurmas = -1;
            if (linha >= 0 && linha < dgvTurmas.Rows.Count) {
                string coluna = botao == 2 ? "colAlunos" : "colAcoes";
                if (dgvTurmas.Columns.Contains(coluna))
                    dgvTurmas.InvalidateCell(dgvTurmas.Columns[coluna].Index, linha);
            }
        }

        private void LimparHoverAcoesTurmas() {
            int linha = linhaAcaoSobMouseTurmas; int botao = botaoAcaoSobMouseTurmas;
            linhaAcaoSobMouseTurmas = -1;
            botaoAcaoSobMouseTurmas = -1;
            dgvTurmas.Cursor = Cursors.Default;
            if (linha >= 0 && linha < dgvTurmas.Rows.Count) {
                string coluna = botao == 2
                    ? "colAlunos"
                    : "colAcoes";
                if (dgvTurmas.Columns.Contains(coluna))
                    dgvTurmas.InvalidateCell(dgvTurmas.Columns[coluna].Index, linha);
            }
        }

        private void PrepararCampoTexto(Panel painel, TextBox campo) {
            painel.BackColor = VisualPadrao.BordaCampo;
            painel.Padding = new Padding(1);
            painel.Margin = new Padding(0, 1, 6, 1);

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

        private void PrepararCampoCombo(Panel painel, ComboBox combo) {
            painel.BackColor = VisualPadrao.BordaCampo;
            painel.Padding = new Padding(1);
            painel.Margin = new Padding(0, 1, 6, 1);

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
            if (combo.Parent != superficie)
                superficie.Controls.Add(combo);

            combo.Enter += delegate { painel.BackColor = VisualPadrao.Azul; };
            combo.Leave += delegate { painel.BackColor = VisualPadrao.BordaCampo; };
            combo.BackColor = Color.White;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            combo.ForeColor = VisualPadrao.TextoPrincipal;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Margin = new Padding(0);
            combo.Dock = DockStyle.Fill;
            combo.BringToFront();

            // A seta personalizada ocupa toda a altura e encosta na borda direita do campo
            Button seta = btnSetaDiaSemana;
            seta.Name = "btnSetaDiaSemana";
            seta.TabStop = false;
            seta.Text = string.Empty;
            seta.Cursor = Cursors.Hand;
            seta.FlatStyle = FlatStyle.Flat;
            seta.BackColor = Color.White;
            seta.UseVisualStyleBackColor = false;
            seta.Padding = new Padding(0);
            seta.Margin = new Padding(0);
            seta.FlatAppearance.BorderSize = 0;
            seta.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            seta.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            seta.Paint += delegate(object sender, PaintEventArgs e) {
                Rectangle r = seta.ClientRectangle;
                if (r.Width < 8 || r.Height < 8)
                    return;
                bool hover = r.Contains(seta.PointToClient(Cursor.Position));
                Color cor = hover ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen separador = new Pen(Color.FromArgb(226, 232, 240))) e.Graphics.DrawLine(separador, 0, 1, 0, r.Height - 2);
                float cx = r.Width / 2F, cy = r.Height / 2F;
                using (Pen pen = new Pen(cor, 1.6F)) {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    e.Graphics.DrawLines(pen, new PointF[] { new PointF(cx - 3.5F, cy - 1.5F), new PointF(cx, cy + 2F), new PointF(cx + 3.5F, cy - 1.5F) });
                }
            };
            seta.MouseEnter += delegate { seta.Invalidate(); };
            seta.MouseLeave += delegate { seta.Invalidate(); };
            seta.Click += delegate { combo.Focus(); combo.DroppedDown = true; };
            if (seta.Parent != superficie)
                superficie.Controls.Add(seta);
            seta.BringToFront();
            Action posicionarSeta = delegate {
                int larguraSeta = Math.Min(28, Math.Max(22, superficie.ClientSize.Width / 6));
                seta.SetBounds(Math.Max(0, superficie.ClientSize.Width - larguraSeta), 0, larguraSeta, superficie.ClientSize.Height);
                combo.SetBounds(0, 0, superficie.ClientSize.Width, superficie.ClientSize.Height);
                seta.BringToFront();
            };
            superficie.SizeChanged += delegate { posicionarSeta(); };
            posicionarSeta();
        }

        private void PosicionarCampo(Panel superficie, Control campo) {
            if (superficie == null || campo == null)
                return;
            campo.Location = new Point(6, Math.Max(1, (superficie.ClientSize.Height - campo.Height) / 2));
            campo.Width = Math.Max(30, superficie.ClientSize.Width - campo.Left - 6);
        }

        private void PreencherDiasDaSemana(ComboBox combo) {
            combo.Items.Clear();
            combo.Items.AddRange(new string[] {
                "Selecione...", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado", "Domingo"
            });
        }

        private void ConfigurarPlaceholderTexto(TextBox campo, string placeholder) {
            placeholdersTxt[campo] = placeholder;
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
            return placeholdersTxt.TryGetValue(campo, out string placeholder) &&
                   campo.ForeColor == VisualPadrao.TextoSecundario &&
                   string.Equals(campo.Text, placeholder, StringComparison.Ordinal);
        }

        private void RestaurarPlaceholder(TextBox campo) {
            if (campo != null && placeholdersTxt.TryGetValue(campo, out string placeholder)) {
                campo.Text = placeholder;
                campo.ForeColor = VisualPadrao.TextoSecundario;
            }
        }

        // Agrupa os dados necessários para exibir os alunos da turma
        private sealed class RegistroAlunosTurma {
            public int IdTurma { get; set; }
            public int IdProfessor { get; set; }
            public int Capacidade { get; set; }
        }

        // Abre a manutenção dos alunos vinculados à turma
        private void GerirAlunosParticipantes(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvTurmas.Rows.Count)
                return;

            DataGridViewRow linha = dgvTurmas.Rows[indiceLinha];
            RegistroAlunosTurma registro = linha.Tag as RegistroAlunosTurma;
            if (registro == null || registro.IdTurma <= 0)
                return;

            string modalidade = Convert.ToString(linha.Cells["colModalidade"].Value) ?? "Turma";
            List<ParticipanteTurma> participantes;
            try {
                participantes = turmaServico.ListarParticipantes(registro.IdTurma);
            }
            catch (Exception erro) {
                MessageBox.Show(this,
                    "Não foi possível consultar os alunos com matrícula ativa.\n\n" + erro.Message,
                    "Consulta de participantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (TurmaParticipantesForm dialogo = new TurmaParticipantesForm(
                registro.IdTurma, modalidade, registro.Capacidade, participantes)) {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    CarregarTurmas();
            }
        }

        // Aplica os critérios atuais à lista de turmas
        private void AplicarFiltrosTabela() {
            if (dgvTurmas == null || dgvTurmas.IsDisposed)
                return;

            string filtroModalidade = CampoContemPlaceholder(txtModalidade) ? string.Empty : txtModalidade.Text.Trim();
            string filtroDia = cmbDiaSemana.SelectedIndex <= 0 ? string.Empty : (cmbDiaSemana.SelectedItem?.ToString() ?? string.Empty);

            foreach (DataGridViewRow linha in dgvTurmas.Rows) {
                bool visivel = true;

                if (filtroModalidade.Length > 0) {
                    string valModalidade = Convert.ToString(linha.Cells[1].Value) ?? string.Empty;
                    if (valModalidade.IndexOf(filtroModalidade, StringComparison.CurrentCultureIgnoreCase) < 0)
                        visivel = false;
                }

                if (visivel && filtroDia.Length > 0) {
                    string valDia = Convert.ToString(linha.Cells[0].Value) ?? string.Empty;
                    if (!string.Equals(valDia, filtroDia, StringComparison.OrdinalIgnoreCase)) {
                        visivel = false;
                    }
                }

                if (!visivel && dgvTurmas.CurrentCell != null && dgvTurmas.CurrentCell.RowIndex == linha.Index)
                    dgvTurmas.CurrentCell = null;

                linha.Visible = visivel;
            }
        }

        private void DgvTurmas_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            string coluna = dgvTurmas.Columns[e.ColumnIndex].Name;
            if (coluna == "colAlunos") {
                GerirAlunosParticipantes(e.RowIndex);
                return;
            }
            if (coluna != "colAcoes")
                return;

            int acao = IndiceBotaoAcaoSobMouseTurmas(e.ColumnIndex, e.RowIndex);
            if (acao == 0)
                AbrirDialogoHorario(e.RowIndex);
            else if (acao == 1)
                ExcluirHorario(e.RowIndex);
        }

        private void BtnAdicionar_Click(object sender, EventArgs e) {
            AbrirDialogoHorario(-1);
        }

        private void ExcluirHorario(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvTurmas.Rows.Count)
                return;

            DataGridViewRow linha = dgvTurmas.Rows[indiceLinha];
            RegistroAlunosTurma registro = linha.Tag as RegistroAlunosTurma;
            if (registro == null || registro.IdTurma <= 0)
                return;

            string modalidade = Convert.ToString(linha.Cells[1].Value) ?? "esta turma";
            DialogResult confirmacao = MessageBox.Show(
                this,
                "Deseja realmente excluir a turma " + modalidade + " do banco de dados?",
                "Excluir turma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao != DialogResult.Yes)
                return;

            try {
                turmaServico.Excluir(registro.IdTurma);
                CarregarTurmas();
            }
            catch (Exception erro) {
                MessageBox.Show(this,
                    "Não foi possível excluir a turma. Ela pode possuir matrículas ou aulas vinculadas.\n\n" + erro.Message,
                    "Falha ao excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Converte o valor informado para o formato monetário esperado
        private static decimal ConverterValorMensal(object valor)
        {
            string texto = Convert.ToString(valor);
            decimal resultado;
            if (decimal.TryParse(texto, System.Globalization.NumberStyles.Currency,
                System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), out resultado))
                return resultado;
            throw new InvalidOperationException("O valor mensal da turma não pôde ser recuperado da tabela.");
        }

        // Abre o cadastro para criar ou editar uma turma
        private void AbrirDialogoHorario(int indiceLinha) {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvTurmas.Rows.Count;
            Turma turmaAtual = null;

            if (editando) {
                DataGridViewRow linha = dgvTurmas.Rows[indiceLinha];
                RegistroAlunosTurma registro = linha.Tag as RegistroAlunosTurma;
                if (registro == null)
                    return;

                string horario = Convert.ToString(linha.Cells["colHorario"].Value) ?? string.Empty;
                string[] partes = horario.Split('-');
                TimeSpan inicio;
                TimeSpan fim;
                if (partes.Length != 2 ||
                    !TimeSpan.TryParse(partes[0].Trim(), out inicio) ||
                    !TimeSpan.TryParse(partes[1].Trim(), out fim)) {
                    MessageBox.Show(this, "O horário desta turma não está em um formato válido.",
                        "Dados da turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                turmaAtual = new Turma {
                    IdTurma = registro.IdTurma,
                    IdProfessor = registro.IdProfessor,
                    Capacidade = registro.Capacidade,
                    ValorMensal = ConverterValorMensal(linha.Cells["colMensalidade"].Value),
                    DiaSemana = Convert.ToString(linha.Cells["colDiaSemana"].Value),
                    Modalidade = Convert.ToString(linha.Cells["colModalidade"].Value),
                    HorarioInicio = inicio,
                    HorarioFim = fim
                };
            }

            using (TurmaCadastroForm dialogo = new TurmaCadastroForm(turmaAtual)) {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    CarregarTurmas();
            }
        }


    }
}
