using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class TurmasForm : Form {
        private RoundedButton btnLimparFiltros;
        private int linhaAcaoSobMouseTurmas = -1;
        private int botaoAcaoSobMouseTurmas = -1;
        private int linhaBotaoPressionadoTurmas = -1;
        private int botaoPressionadoTurmas = -1;
        private readonly Dictionary<TextBox, string> placeholdersTxt = new Dictionary<TextBox, string>();

        public TurmasForm() {
            InitializeComponent();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            ConfigurarAparencia();
            if (emDesignTime)
                return;
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
            colProfessor.FillWeight = 160F;
            colAlunos.FillWeight = 75F;
            colAlunos.ToolTipText = "Clique na lupa para gerenciar os alunos presentes";

            colDiaSemana.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colModalidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHorario.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colProfessor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colAlunos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            PreencherTurmasDeExemplo();
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

        private sealed class RegistroAlunosTurma {
            public List<string> Alunos = new List<string>();
            public HashSet<string> Presentes = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
        }

        private void AdicionarTurmaDeExemplo(string dia, string modalidade, string horario, string professor, string[] alunos, int presentes) {
            int indice = dgvTurmas.Rows.Add(dia, modalidade, horario, professor, presentes.ToString());
            RegistroAlunosTurma registro = new RegistroAlunosTurma();
            registro.Alunos.AddRange(alunos);
            for (int i = 0; i < Math.Min(presentes, alunos.Length); i++) registro.Presentes.Add(alunos[i]);
            dgvTurmas.Rows[indice].Tag = registro;
        }

        private void PreencherTurmasDeExemplo() {
            AdicionarTurmaDeExemplo("Sexta-feira", "Natação A", "08:00 - 09:00", "Mariana Costa",
                new string[] { "Ana Clara", "Bruno Lima", "Caio Mendes", "Daniela Souza", "Eduardo Costa", "Fernanda Alves", "Gabriel Rocha", "Helena Martins", "Igor Santos", "Julia Ferreira", "Lucas Oliveira", "Marina Ribeiro" }, 9);
            AdicionarTurmaDeExemplo("Sábado", "Hidroginástica", "09:30 - 10:30", "Camila Ferreira",
                new string[] { "Adriana Lima", "Beatriz Souza", "Cecília Ramos", "Denise Costa", "Elaine Martins", "Fabiana Rocha", "Gisele Alves", "Helena Melo", "Isabel Nunes", "Joana Prado", "Karen Dias", "Larissa Reis", "Márcia Silva", "Neusa Costa", "Olga Ribeiro", "Paula Santos", "Quitéria Luz", "Renata Lima", "Sônia Mendes", "Tereza Alves" }, 15);
            AdicionarTurmaDeExemplo("Domingo", "Natação 8 a 14 anos", "18:00 - 19:00", "Rafael Mendes",
                new string[] { "Arthur Lima", "Bia Nunes", "Carlos Prado", "Davi Santos", "Enzo Rocha", "Felipe Melo", "Giovana Reis", "Heitor Dias" }, 6);
            AdicionarTurmaDeExemplo("Segunda-feira", "Natação 5 a 7 anos", "14:00 - 15:00", "Lucas Almeida",
                new string[] { "Alice Costa", "Bernardo Luz", "Clara Alves", "Daniel Silva", "Ester Prado", "Felipe Nunes", "Giulia Rocha", "Hugo Reis", "Isabela Dias", "João Melo", "Kauã Santos", "Laura Lima", "Miguel Costa", "Nina Martins", "Otávio Souza" }, 12);
            AplicarFiltrosTabela();
        }

        private RegistroAlunosTurma CriarRegistroNumerico(string quantidadeTexto) {
            int quantidade;
            if (!int.TryParse(quantidadeTexto, out quantidade))
                quantidade = 0;
            quantidade = Math.Max(0, Math.Min(quantidade, 100));
            RegistroAlunosTurma registro = new RegistroAlunosTurma();
            for (int i = 1; i <= quantidade; i++)
                registro.Alunos.Add("Aluno " + i.ToString());

            return registro;
        }

        private void GarantirRegistroAlunos(DataGridViewRow linha) {
            if (linha.Tag is RegistroAlunosTurma)
                return;
            linha.Tag = CriarRegistroNumerico(Convert.ToString(linha.Cells["colAlunos"].Value));
        }

        private void GerirAlunosPresentes(int indiceLinha) {
            if (indiceLinha < 0 || indiceLinha >= dgvTurmas.Rows.Count)
                return;
            DataGridViewRow linha = dgvTurmas.Rows[indiceLinha];
            GarantirRegistroAlunos(linha);
            RegistroAlunosTurma registro = (RegistroAlunosTurma)linha.Tag;
            string modalidade = Convert.ToString(linha.Cells["colModalidade"].Value) ?? "Turma";

            using (Form dialogo = new Form()) {
                dialogo.Text = "Alunos presentes";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.White;
                dialogo.Font = new Font("Segoe UI", 9F);
                dialogo.ClientSize = new Size(520, 500);

                Label titulo = new Label { Text = modalidade, Font = new Font("Segoe UI Semibold", 14F), ForeColor = Color.FromArgb(17, 24, 39), AutoSize = true, Location = new Point(22, 18) };
                Label descricao = new Label { Text = "Marque os alunos que estão presentes nesta turma.", ForeColor = VisualPadrao.TextoSecundario, AutoSize = true, Location = new Point(23, 49) };
                CheckedListBox lista = new CheckedListBox { Location = new Point(22, 80), Size = new Size(476, 310), Font = new Font("Segoe UI", 9F), CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle, IntegralHeight = false };
                foreach (string aluno in registro.Alunos) lista.Items.Add(aluno, registro.Presentes.Contains(aluno));
                TextBox novoAluno = new TextBox { Location = new Point(22, 405), Width = 350, Height = 26, Font = new Font("Segoe UI", 9F) };
                RoundedButton adicionar = new RoundedButton { Text = "+ Adicionar", CornerRadius = 5, Size = new Size(112, 30), Location = new Point(386, 403), BackColor = Color.FromArgb(239, 246, 255), ForeColor = VisualPadrao.Azul, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                adicionar.FlatAppearance.BorderColor = Color.FromArgb(219, 234, 254);
                adicionar.FlatAppearance.BorderSize = 1;
                adicionar.Click += delegate {
                    string nome = novoAluno.Text.Trim();
                    if (nome.Length == 0)
                        return;
                    bool existe = false;
                    foreach (object item in lista.Items)
                        if (string.Equals(Convert.ToString(item), nome, StringComparison.CurrentCultureIgnoreCase)) {
                            existe = true;
                            break;
                        }
                    if (!existe)
                        lista.Items.Add(nome, true);
                    novoAluno.Clear();
                    novoAluno.Focus();
                };

                RoundedButton cancelar = new RoundedButton { Text = "Cancelar", CornerRadius = 5, Size = new Size(100, 34), Location = new Point(286, 450), BackColor = Color.White, ForeColor = VisualPadrao.TextoPrincipal, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                cancelar.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                cancelar.FlatAppearance.BorderSize = 1;
                cancelar.DialogResult = DialogResult.Cancel;
                RoundedButton salvar = new RoundedButton { Text = "Salvar presença", CornerRadius = 5, Size = new Size(124, 34), Location = new Point(394, 450), BackColor = VisualPadrao.Azul, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                salvar.FlatAppearance.BorderSize = 0;
                salvar.Click += delegate {
                    registro.Alunos.Clear();
                    registro.Presentes.Clear();
                    for (int i = 0; i < lista.Items.Count; i++) {
                        string aluno = Convert.ToString(lista.Items[i]);
                        registro.Alunos.Add(aluno);
                        if (lista.GetItemChecked(i))
                            registro.Presentes.Add(aluno);
                    }
                    linha.Cells["colAlunos"].Value = registro.Presentes.Count.ToString();
                    linha.Cells["colAlunos"].ToolTipText = registro.Presentes.Count.ToString() + " presentes de " + registro.Alunos.Count.ToString() + " alunos";
                    dgvTurmas.InvalidateCell(linha.Cells["colAlunos"]);
                    dialogo.DialogResult = DialogResult.OK;
                    dialogo.Close();
                };
                dialogo.Controls.AddRange(new Control[] { titulo, descricao, lista, novoAluno, adicionar, cancelar, salvar });
                dialogo.AcceptButton = salvar;
                dialogo.CancelButton = cancelar;
                dialogo.ShowDialog(this);
            }
        }

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
                GerirAlunosPresentes(e.RowIndex);
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

            string modalidade = Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells[1].Value) ?? "esta turma";
            DialogResult confirmacao = MessageBox.Show(this, "Deseja realmente excluir a turma " + modalidade + "?", "Excluir turma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes) {
                dgvTurmas.Rows.RemoveAt(indiceLinha);
                AplicarFiltrosTabela();
            }
        }

        private void AbrirDialogoHorario(int indiceLinha) {
            bool editando = indiceLinha >= 0 && indiceLinha < dgvTurmas.Rows.Count;

            string diaAtual = editando
                ? Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells[0].Value)
                ?? string.Empty
                : string.Empty;
            string modAtual = editando
                ? Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells[1].Value)
                ?? string.Empty
                : string.Empty;
            string horAtual = editando
                ? Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells[2].Value)
                ?? string.Empty
                : string.Empty;
            string profAtual = editando
                ? Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells[3].Value)
                ?? string.Empty
                : string.Empty;
            RegistroAlunosTurma registroAtual = editando
                ? (dgvTurmas.Rows[indiceLinha].Tag as RegistroAlunosTurma)
                : null;
            string aluAtual = editando
                ? (registroAtual != null
                    ? registroAtual.Alunos.Count.ToString()
                    : Convert.ToString(dgvTurmas.Rows[indiceLinha].Cells[4].Value) ?? string.Empty)
                : string.Empty;

            using (Form dialogo = new Form()) {
                dialogo.Text = editando ? "Editar turma" : "Nova turma";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.BackColor = Color.FromArgb(247, 249, 252);
                dialogo.ClientSize = new Size(480, 420);
                dialogo.Font = new Font("Segoe UI", 9F);

                Label titulo = new Label {
                    Text = editando
                        ? "Editar turma"
                        : "Nova turma", Font = new Font("Segoe UI Semibold", 14F), ForeColor = Color.FromArgb(17, 24, 39), AutoSize = true, Location = new Point(22, 18) };
                Label descricao = new Label { Text = editando ? "Atualize os dados desta turma." : "Preencha os dados da nova turma.", ForeColor = VisualPadrao.TextoSecundario, AutoSize = true, Location = new Point(23, 48) };

                string[] opcoesDias = new string[] { "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado", "Domingo" };
                ComboBox cmbDia = CriarComboDialogo(dialogo, "Dia da Semana", 82, 180, opcoesDias);

                TextBox horario = CriarCampoDialogo(dialogo, "Horário (ex: 14:00 - 15:00)", 82, 180, 240);

                string[] opcoesModalidade = new string[] { "Hidroginástica", "Natação A", "Natação (8 a 14 anos)", "Natação (5 a 7 anos)" };
                ComboBox cmbMod = CriarComboDialogo(dialogo, "Modalidade", 132, 420, opcoesModalidade);

                TextBox professor = CriarCampoDialogo(dialogo, "Professor", 182, 420);
                TextBox alunos = CriarCampoDialogo(dialogo, "Total de Alunos", 232, 420);

                if (editando) {
                    cmbDia.SelectedItem = diaAtual;
                    cmbMod.SelectedItem = modAtual;
                }
                horario.Text = horAtual;
                professor.Text = profAtual;
                alunos.Text = aluAtual;

                RoundedButton cancelar = new RoundedButton { Text = "Cancelar", CornerRadius = 5, Size = new Size(100, 36), Location = new Point(editando ? 218 : 250, 310), BackColor = Color.White, ForeColor = VisualPadrao.TextoPrincipal, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                cancelar.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                cancelar.FlatAppearance.BorderSize = 1;
                cancelar.DialogResult = DialogResult.Cancel;

                RoundedButton salvar = new RoundedButton {
                    Text = editando
                    ? "Salvar alterações"
                    : "Adicionar", CornerRadius = 5, Size = new Size(editando ? 142 : 112, 36), Location = new Point(editando ? 328 : 358, 310), BackColor = VisualPadrao.Azul, ForeColor = Color.White, Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
                salvar.FlatAppearance.BorderSize = 0;
                salvar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
                salvar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);

                salvar.Click += delegate {
                    if (cmbDia.SelectedIndex == -1) {
                        MessageBox.Show(dialogo, "Selecione o dia da semana.", "Cadastro de turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDia.Focus();
                        return;
                    }
                    if (cmbMod.SelectedIndex == -1) {
                        MessageBox.Show(dialogo, "Selecione a modalidade.", "Cadastro de turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbMod.Focus();
                        return;
                    }

                    string diaEscolhido = cmbDia.SelectedItem.ToString();
                    string modEscolhida = cmbMod.SelectedItem.ToString();

                    if (editando) {
                        DataGridViewRow linha = dgvTurmas.Rows[indiceLinha];
                        linha.Cells[0].Value = diaEscolhido;
                        linha.Cells[1].Value = modEscolhida;
                        linha.Cells[2].Value = horario.Text.Trim();
                        linha.Cells[3].Value = professor.Text.Trim();
                        int quantidadeDesejada;
                        if (int.TryParse(alunos.Text.Trim(), out quantidadeDesejada) && quantidadeDesejada >= 0 && quantidadeDesejada <= 100) {
                            RegistroAlunosTurma registro = registroAtual ?? CriarRegistroNumerico(Convert.ToString(linha.Cells[4].Value));
                            while (registro.Alunos.Count < quantidadeDesejada) {
                                int proximo = registro.Alunos.Count + 1;
                                string nomeGenerico = "Aluno " + proximo.ToString();
                                while (registro.Alunos.Contains(nomeGenerico)) { proximo++; nomeGenerico = "Aluno " + proximo.ToString(); }
                                registro.Alunos.Add(nomeGenerico);
                            }
                            while (registro.Alunos.Count > quantidadeDesejada) {
                                string removido = registro.Alunos[registro.Alunos.Count - 1];
                                registro.Alunos.RemoveAt(registro.Alunos.Count - 1);
                                registro.Presentes.Remove(removido);
                            }
                            linha.Tag = registro;
                            linha.Cells[4].Value = registro.Presentes.Count.ToString();
                            linha.Cells[4].ToolTipText = registro.Presentes.Count.ToString() + " presentes de " + registro.Alunos.Count.ToString() + " alunos";
                        }

                    } else {
                        RegistroAlunosTurma novoRegistro = CriarRegistroNumerico(alunos.Text.Trim());
                        int novaLinha = dgvTurmas.Rows.Add(diaEscolhido, modEscolhida, horario.Text.Trim(), professor.Text.Trim(), "0");
                        dgvTurmas.Rows[novaLinha].Tag = novoRegistro;
                        dgvTurmas.Rows[novaLinha].Cells["colAlunos"].ToolTipText = "0 presentes de " + novoRegistro.Alunos.Count.ToString() + " alunos";
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

        private TextBox CriarCampoDialogo(Form dialogo, string legenda, int y, int largura, int x = 24) {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(x, y), ForeColor = VisualPadrao.TextoPrincipal };
            TextBox campo = new TextBox { Name = "txt" + legenda.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(":", "").Replace("/", ""), Location = new Point(x, y + 19), Width = largura, Height = 25, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9F) };
            dialogo.Controls.Add(label);
            dialogo.Controls.Add(campo);
            return campo;
        }

        private ComboBox CriarComboDialogo(Form dialogo, string legenda, int y, int largura, string[] itens, int x = 24) {
            Label label = new Label { Text = legenda, AutoSize = true, Location = new Point(x, y), ForeColor = VisualPadrao.TextoPrincipal };
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
