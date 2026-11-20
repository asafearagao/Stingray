using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Stingray.DadosTela;

namespace Stingray {
    [DesignerCategory("Form")]

    // Controla a tela de chamada e a interação com as situações dos alunos
    public partial class PresencaForm : Form {
        private Panel superficieDataAula;
        private Panel superficieTurma;
        private Button btnSetaTurma;
        private Panel calendarioFlutuante;
        private Label lblMesCalendario;
        private Label lblHojeCalendario;
        private DateTime mesCalendarioVisualizado = DateTime.Today;
        private DateTime dataSelecionada = DateTime.Today;
        private readonly List<Button> botoesDiasCalendario = new List<Button>();
        private IMessageFilter filtroCliqueCalendario;
        private RoundedButton btnSalvarChamada;
        private RoundedButton btnLimparChamada;
        private ContextMenuStrip menuPresenca;
        private ToolStripMenuItem itemAgendarReposicao;
        private int linhaBotaoSituacaoHover = -1;
        private int indiceBotaoSituacaoHover = -1;

        // Fecha o calendário quando o clique ocorre fora dele
        private sealed class FiltroCliqueCalendario : IMessageFilter {
            private readonly PresencaForm formulario;

            public FiltroCliqueCalendario(PresencaForm form) {
                formulario = form;
            }

            public bool PreFilterMessage(ref Message m) {
                if (formulario.calendarioFlutuante == null || !formulario.calendarioFlutuante.Visible)
                    return false;

                if (m.Msg != 0x0201 && m.Msg != 0x0204 && m.Msg != 0x0207)
                    return false;

                Point ponto = Control.MousePosition;
                Rectangle areaCalendario = formulario.calendarioFlutuante.RectangleToScreen(
                    formulario.calendarioFlutuante.ClientRectangle);
                Rectangle areaData = formulario.superficieDataAula.RectangleToScreen(
                    formulario.superficieDataAula.ClientRectangle);
                if (!areaCalendario.Contains(ponto) && !areaData.Contains(ponto))
                    formulario.FecharCalendario();

                return false;
            }
        }

        public PresencaForm() {
            InitializeComponent();
            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);

            ConfigurarAparencia();
            ConfigurarBotoesChamada();
            txtDataAula.Text = dataSelecionada.ToString("dd/MM/yyyy");
            txtDataAula.ForeColor = VisualPadrao.TextoPrincipal;

            ConfigurarAcessoFrequencia();
            if (emDesignTime) {
                AtualizarResumoPresenca();
                return;
            }

            ConfigurarCalendario();
            CarregarTurmasDaData();
        }

        // Prepara os controles e a apresentação da chamada
        private void ConfigurarAparencia() {
            BackColor = VisualPadrao.FundoPagina;
            pnlPresenca.BackColor = VisualPadrao.FundoPagina;

            pnlFiltros.BackColor = VisualPadrao.BordaCartao;
            pnlFiltros.Padding = new Padding(1);
            pnlTabela.BackColor = VisualPadrao.BordaCartao;
            pnlTabela.Padding = new Padding(1);
            pnlTotal.BackColor = VisualPadrao.BordaCartao;
            pnlTotal.Padding = new Padding(1);

            tlpFiltros.BackColor = Color.White;
            tlpFiltros.Padding = new Padding(12, 4, 12, 4);
            tlpFiltros.ColumnCount = 4;
            tlpFiltros.ColumnStyles.Clear();
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
            tlpFiltros.RowCount = 2;
            tlpFiltros.RowStyles.Clear();
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            tlpFiltros.SetColumn(lblDataAula, 0);
            tlpFiltros.SetRow(lblDataAula, 0);
            tlpFiltros.SetColumn(lblTurma, 1);
            tlpFiltros.SetRow(lblTurma, 0);
            lblDataAula.Dock = DockStyle.Fill;
            lblTurma.Dock = DockStyle.Fill;
            lblDataAula.Margin = new Padding(0);
            lblTurma.Margin = new Padding(6, 0, 0, 0);

            superficieDataAula = PrepararCampo(pnlDataAula, txtDataAula, btnCalendario);
            superficieTurma = PrepararCampo(pnlTurma, cmbTurma);

            txtDataAula.BackColor = Color.White;
            txtDataAula.BorderStyle = BorderStyle.None;
            txtDataAula.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtDataAula.ForeColor = VisualPadrao.TextoPrincipal;
            txtDataAula.ReadOnly = true;
            txtDataAula.Cursor = Cursors.Hand;
            txtDataAula.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDataAula.Click += delegate { AlternarCalendario(); };

            btnCalendario.BackColor = Color.White;
            btnCalendario.FlatStyle = FlatStyle.Flat;
            btnCalendario.FlatAppearance.BorderSize = 0;
            btnCalendario.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            btnCalendario.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnCalendario.ForeColor = VisualPadrao.TextoPrincipal;
            btnCalendario.Cursor = Cursors.Hand;
            btnCalendario.UseVisualStyleBackColor = false;
            btnCalendario.Text = string.Empty;
            btnCalendario.AccessibleName = "Abrir calendário da data da aula";
            btnCalendario.Paint += CalendarioButton_Paint;
            btnCalendario.MouseEnter += delegate { btnCalendario.Invalidate(); };
            btnCalendario.MouseLeave += delegate { btnCalendario.Invalidate(); };
            btnCalendario.Click += delegate { AlternarCalendario(); };

            cmbTurma.BackColor = Color.White;
            cmbTurma.ForeColor = VisualPadrao.TextoPrincipal;
            cmbTurma.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            cmbTurma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTurma.FlatStyle = FlatStyle.Flat;
            cmbTurma.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTurma.ItemHeight = 18;
            cmbTurma.AutoSize = false;
            cmbTurma.DrawItem += ComboTurma_DrawItem;
            cmbTurma.DropDownClosed += delegate { cmbTurma.Invalidate(); superficieTurma.Invalidate(); };
            if (cmbTurma.Items.Count > 0)
                cmbTurma.SelectedIndex = 0;

            CriarBotaoSetaTurma();

            tlpTotal.BackColor = Color.White;
            foreach (Label label in new Label[] {
                lblPresentes, lblNumPresentes, lblAusentes, lblNumAusentes,
                lblJustificados, lblNumJustificados, lblTotalAlunos, lblNumTotalAlunos})
                {
                label.ForeColor = VisualPadrao.TextoPrincipal;
                label.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            }

            ConfigurarTabelaPresenca(gdvPresenca);
            superficieDataAula.SizeChanged += delegate { PosicionarCamposFiltro(); };
            superficieTurma.SizeChanged += delegate { PosicionarCamposFiltro(); };
            PosicionarCamposFiltro();
        }

        // Configura os comandos de presença, ausência e justificativa
        private void ConfigurarBotoesChamada() {
            btnSalvarChamada.Name = "btnSalvarChamada";
            btnSalvarChamada.Text = "Salvar chamada";
            btnSalvarChamada.CornerRadius = 5;
            btnSalvarChamada.BackColor = VisualPadrao.Azul;
            btnSalvarChamada.ForeColor = Color.White;
            btnSalvarChamada.FlatAppearance.BorderSize = 0;
            btnSalvarChamada.FlatAppearance.BorderColor = VisualPadrao.Azul;
            btnSalvarChamada.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            btnSalvarChamada.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            btnSalvarChamada.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnSalvarChamada.Size = new Size(132, 30);
            btnSalvarChamada.Dock = DockStyle.Fill;
            btnSalvarChamada.Margin = new Padding(6, 0, 0, 0);
            btnSalvarChamada.TextAlign = ContentAlignment.MiddleCenter;
            btnSalvarChamada.Cursor = Cursors.Hand;
            btnSalvarChamada.Click += BtnSalvarChamada_Click;

            btnLimparChamada.Name = "btnLimparChamada";
            btnLimparChamada.Text = "Limpar chamada";
            btnLimparChamada.CornerRadius = 5;
            btnLimparChamada.BackColor = Color.White;
            btnLimparChamada.ForeColor = VisualPadrao.TextoPrincipal;
            btnLimparChamada.FlatAppearance.BorderSize = 0;
            btnLimparChamada.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            btnLimparChamada.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnLimparChamada.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            btnLimparChamada.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnLimparChamada.Size = new Size(126, 30);
            btnLimparChamada.Dock = DockStyle.Fill;
            btnLimparChamada.Margin = new Padding(6, 0, 0, 0);
            btnLimparChamada.TextAlign = ContentAlignment.MiddleCenter;
            btnLimparChamada.Cursor = Cursors.Hand;
            btnLimparChamada.Click += BtnLimparChamada_Click;
            btnLimparChamada.MouseEnter += delegate {
                btnLimparChamada.ForeColor = Color.FromArgb(185, 28, 28);
                btnLimparChamada.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
            };
            btnLimparChamada.MouseLeave += delegate {
                btnLimparChamada.ForeColor = VisualPadrao.TextoPrincipal;
                btnLimparChamada.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            };

        }

        private void BtnSalvarChamada_Click(object sender, EventArgs e) {
            SalvarChamadaNoBanco();
        }

        private void BtnLimparChamada_Click(object sender, EventArgs e) {
            if (gdvPresenca.Rows.Count == 0) {
                MessageBox.Show(this, "Não há alunos carregados para limpar nesta chamada.",
                    "Limpar chamada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult resposta = MessageBox.Show(this,
                "Deseja limpar as situações de todos os alunos desta chamada?",
                "Limpar chamada", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resposta != DialogResult.Yes)
                return;

            foreach (DataGridViewRow linha in gdvPresenca.Rows) {
                if (linha.IsNewRow)
                    continue;
                ParticipantePresenca participante = linha.Tag as ParticipantePresenca;
                if (participante != null)
                    participante.Situacao = null;
                AtualizarVisualBotoesSituacao(linha);
            }
            AtualizarResumoPresenca();
        }

        // Recalcula os totais exibidos para a chamada
        private void AtualizarResumoPresenca() {
            if (gdvPresenca == null || gdvPresenca.IsDisposed)
                return;

            int presentes = 0;
            int ausentes = 0;
            int justificados = 0;
            int total = 0;

            foreach (DataGridViewRow linha in gdvPresenca.Rows) {
                if (linha.IsNewRow)
                    continue;
                total++;
                ParticipantePresenca participante = linha.Tag as ParticipantePresenca;
                string situacao = participante == null ? string.Empty : participante.Situacao ?? string.Empty;
                if (string.Equals(situacao, "Presente", StringComparison.OrdinalIgnoreCase))
                    presentes++;
                else if (string.Equals(situacao, "Ausente", StringComparison.OrdinalIgnoreCase))
                    ausentes++;
                else if (string.Equals(situacao, "Justificado", StringComparison.OrdinalIgnoreCase))
                    justificados++;
            }

            lblNumPresentes.Text = presentes.ToString(CultureInfo.InvariantCulture);
            lblNumAusentes.Text = ausentes.ToString(CultureInfo.InvariantCulture);
            lblNumJustificados.Text = justificados.ToString(CultureInfo.InvariantCulture);
            lblNumTotalAlunos.Text = total.ToString(CultureInfo.InvariantCulture);
        }

        // Atualiza o destaque visual da situação selecionada
        private void AtualizarVisualBotoesSituacao(DataGridViewRow linha)
        {
            if (linha == null || !gdvPresenca.Columns.Contains("colSituacoes"))
                return;

            gdvPresenca.InvalidateCell(gdvPresenca.Columns["colSituacoes"].Index, linha.Index);
        }

        private static Rectangle[] ObterAreasBotoesSituacao(Rectangle celula)
        {
            const int margemHorizontal = 12;
            const int espaco = 10;
            int altura = Math.Min(32, Math.Max(24, celula.Height - 12));
            int larguraDisponivel = celula.Width - (margemHorizontal * 2) - (espaco * 2);
            int largura = Math.Max(70, larguraDisponivel / 3);
            int larguraTotal = largura * 3 + espaco * 2;
            int inicioX = celula.Left + Math.Max(margemHorizontal, (celula.Width - larguraTotal) / 2);
            int inicioY = celula.Top + (celula.Height - altura) / 2;

            return new Rectangle[]
            {
                new Rectangle(inicioX, inicioY, largura, altura),
                new Rectangle(inicioX + largura + espaco, inicioY, largura, altura),
                new Rectangle(inicioX + (largura + espaco) * 2, inicioY, largura, altura)
            };
        }

        private static GraphicsPath CriarCaminhoBotaoArredondado(Rectangle area, int raio)
        {
            int diametro = Math.Min(raio * 2, Math.Min(area.Width, area.Height));
            GraphicsPath caminho = new GraphicsPath();
            caminho.AddArc(area.Left, area.Top, diametro, diametro, 180, 90);
            caminho.AddArc(area.Right - diametro, area.Top, diametro, diametro, 270, 90);
            caminho.AddArc(area.Right - diametro, area.Bottom - diametro, diametro, diametro, 0, 90);
            caminho.AddArc(area.Left, area.Bottom - diametro, diametro, diametro, 90, 90);
            caminho.CloseFigure();
            return caminho;
        }

        private void DesenharBotaoSituacao(Graphics graphics, Rectangle area, string texto,
            string situacaoAtual, string situacaoBotao, bool hover, Color fundoNormal,
            Color bordaNormal, Color textoNormal, Color fundoAtivo)
        {
            bool selecionado = string.Equals(situacaoAtual, situacaoBotao, StringComparison.OrdinalIgnoreCase);
            Color fundo = selecionado ? fundoAtivo : (hover ? ControlPaint.Light(fundoNormal, 0.12F) : fundoNormal);
            Color borda = selecionado ? fundoAtivo : bordaNormal;
            Color corTexto = selecionado ? Color.White : textoNormal;
            Rectangle contorno = new Rectangle(area.X, area.Y, area.Width - 1, area.Height - 1);

            using (GraphicsPath caminho = CriarCaminhoBotaoArredondado(contorno, 8))
            using (SolidBrush pincel = new SolidBrush(fundo))
            using (Pen caneta = new Pen(borda, selecionado ? 1.5F : 1F))
            {
                graphics.FillPath(pincel, caminho);
                graphics.DrawPath(caneta, caminho);
            }

            using (Font fonte = new Font("Segoe UI Semibold", 8.2F, FontStyle.Regular, GraphicsUnit.Point))
                TextRenderer.DrawText(graphics, texto, fonte, contorno, corTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void Presenca_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || gdvPresenca.Columns[e.ColumnIndex].Name != "colSituacoes")
                return;

            e.PaintBackground(e.CellBounds, true);
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);

            ParticipantePresenca participante = gdvPresenca.Rows[e.RowIndex].Tag as ParticipantePresenca;
            string atual = participante == null ? string.Empty : participante.Situacao ?? string.Empty;
            Rectangle[] areas = ObterAreasBotoesSituacao(e.CellBounds);
            GraphicsState estado = e.Graphics.Save();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            try
            {
                DesenharBotaoSituacao(e.Graphics, areas[0], "Presente", atual, "Presente",
                    linhaBotaoSituacaoHover == e.RowIndex && indiceBotaoSituacaoHover == 0,
                    Color.FromArgb(220, 252, 231), Color.FromArgb(134, 239, 172), Color.FromArgb(22, 101, 52), Color.FromArgb(22, 163, 74));
                DesenharBotaoSituacao(e.Graphics, areas[1], "Ausente", atual, "Ausente",
                    linhaBotaoSituacaoHover == e.RowIndex && indiceBotaoSituacaoHover == 1,
                    Color.FromArgb(254, 226, 226), Color.FromArgb(252, 165, 165), Color.FromArgb(153, 27, 27), Color.FromArgb(220, 38, 38));
                DesenharBotaoSituacao(e.Graphics, areas[2], "Justificado", atual, "Justificado",
                    linhaBotaoSituacaoHover == e.RowIndex && indiceBotaoSituacaoHover == 2,
                    Color.FromArgb(254, 249, 195), Color.FromArgb(253, 224, 71), Color.FromArgb(133, 77, 14), Color.FromArgb(202, 138, 4));
            }
            finally
            {
                e.Graphics.Restore(estado);
            }
            e.Handled = true;
        }

        private int ObterBotaoSituacaoSobMouse(DataGridView tabela, int linha, Point ponto)
        {
            if (linha < 0 || linha >= tabela.Rows.Count || !tabela.Columns.Contains("colSituacoes"))
                return -1;

            Rectangle celula = tabela.GetCellDisplayRectangle(tabela.Columns["colSituacoes"].Index, linha, false);
            Rectangle[] areas = ObterAreasBotoesSituacao(celula);
            for (int i = 0; i < areas.Length; i++)
            {
                if (areas[i].Contains(ponto))
                    return i;
            }
            return -1;
        }


        // Altera a situação do participante na chamada atual
        private void DefinirSituacaoLinha(DataGridViewRow linha, string situacao)
        {
            if (linha == null)
                return;

            ParticipantePresenca participante = linha.Tag as ParticipantePresenca;
            if (participante == null)
                return;

            participante.Situacao = situacao;
            AtualizarVisualBotoesSituacao(linha);
            AtualizarResumoPresenca();
        }

        private Panel PrepararCampo(Panel campo, params Control[] controles) {
            campo.BackColor = VisualPadrao.BordaCampo;
            campo.Padding = new Padding(1);

            Panel superficie = null;
            string nomeSuperficie = "superficie" + campo.Name.Substring(3);
            foreach (Control controle in campo.Controls) {
                Panel candidata = controle as Panel;
                if (candidata != null && candidata.Name == nomeSuperficie) {
                    superficie = candidata;
                    break;
                }
            }
            if (superficie == null) {
                superficie = new Panel { Name = nomeSuperficie, Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0) };
                campo.Controls.Add(superficie);

            } foreach (Control controle in controles) {
                if (controle.Parent != superficie)
                    superficie.Controls.Add(controle);
            }

            return superficie;
        }

        private void PosicionarCamposFiltro() {
            if (superficieDataAula != null && txtDataAula != null && btnCalendario != null) {
                int alturaBotao = Math.Max(18, Math.Min(24, superficieDataAula.ClientSize.Height - 2));
                btnCalendario.Size = new Size(26, alturaBotao);
                btnCalendario.Location = new Point(
                    Math.Max(2, superficieDataAula.ClientSize.Width - btnCalendario.Width - 2),
                    Math.Max(0, (superficieDataAula.ClientSize.Height - btnCalendario.Height) / 2));

                txtDataAula.Location = new Point(6, Math.Max(1, (superficieDataAula.ClientSize.Height - txtDataAula.Height) / 2));
                txtDataAula.Width = Math.Max(30, btnCalendario.Left - txtDataAula.Left - 3);
            }

            if (superficieTurma != null && cmbTurma != null) {
                int alturaCombo = Math.Max(20, Math.Min(24, superficieTurma.ClientSize.Height - 2));
                cmbTurma.SetBounds(0, Math.Max(0, (superficieTurma.ClientSize.Height - alturaCombo) / 2),
                    Math.Max(30, superficieTurma.ClientSize.Width), alturaCombo);

                if (btnSetaTurma != null) {
                    btnSetaTurma.SetBounds(Math.Max(0, superficieTurma.ClientSize.Width - 24), 0, 24, superficieTurma.ClientSize.Height);
                    btnSetaTurma.BringToFront();
                }
            }
        }

        private void CriarBotaoSetaTurma() {
            btnSetaTurma.Name = "btnSetaTurma";
            btnSetaTurma.AccessibleName = "Abrir opções de turma";
            btnSetaTurma.AccessibleDescription = "Abre a lista de turmas";
            btnSetaTurma.Text = string.Empty;
            btnSetaTurma.TabStop = false;
            btnSetaTurma.Cursor = Cursors.Hand;
            btnSetaTurma.FlatStyle = FlatStyle.Flat;
            btnSetaTurma.FlatAppearance.BorderSize = 0;
            btnSetaTurma.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            btnSetaTurma.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnSetaTurma.BackColor = Color.White;
            btnSetaTurma.UseVisualStyleBackColor = false;
            btnSetaTurma.Margin = new Padding(0);
            btnSetaTurma.Paint += ComboSetaTurma_Paint;
            btnSetaTurma.MouseEnter += delegate { btnSetaTurma.BackColor = VisualPadrao.AzulClaro; btnSetaTurma.Invalidate(); };
            btnSetaTurma.MouseLeave += delegate { btnSetaTurma.BackColor = Color.White; btnSetaTurma.Invalidate(); };
            btnSetaTurma.Click += delegate {
                if (cmbTurma == null || cmbTurma.IsDisposed)
                    return;
                cmbTurma.Focus();
                cmbTurma.DroppedDown = true;
            };
            if (btnSetaTurma.Parent != superficieTurma)
                superficieTurma.Controls.Add(btnSetaTurma);
            btnSetaTurma.BringToFront();
        }

        private void ComboSetaTurma_Paint(object sender, PaintEventArgs e) {
            Button seta = sender as Button;
            if (seta == null || seta.ClientSize.Width < 8 || seta.ClientSize.Height < 8)
                return;

            bool hover = seta.ClientRectangle.Contains(seta.PointToClient(Cursor.Position));
            Color cor = hover ? Color.FromArgb(37, 99, 235) : VisualPadrao.TextoSecundario;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen separador = new Pen(VisualPadrao.BordaCartao))
                e.Graphics.DrawLine(separador, 0, 1, 0, seta.ClientSize.Height - 2);
            using (Pen pen = new Pen(cor, 1.6F)) {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                float cx = seta.ClientSize.Width / 2F;
                float cy = seta.ClientSize.Height / 2F;
                e.Graphics.DrawLines(pen, new PointF[] {
                    new PointF(cx - 3.5F, cy - 1.5F),
                    new PointF(cx, cy + 2F),
                    new PointF(cx + 3.5F, cy - 1.5F)
                });
            }
        }

        private void ComboTurma_DrawItem(object sender, DrawItemEventArgs e) {
            ComboBox combo = sender as ComboBox;
            if (combo == null || e.Bounds.Width <= 0 || e.Bounds.Height <= 0)
                return;

            bool itemSelecionadoNaLista = combo.DroppedDown && e.Index >= 0 &&
                (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color fundo = itemSelecionadoNaLista
                ? VisualPadrao.Azul
                : Color.White;
            Color corTexto = itemSelecionadoNaLista
                ? Color.White
                : VisualPadrao.TextoPrincipal;
            using (Brush brush = new SolidBrush(fundo))
                e.Graphics.FillRectangle(brush, e.Bounds);

            string valor = e.Index >= 0 && e.Index < combo.Items.Count
                ? Convert.ToString(combo.Items[e.Index])
                : combo.Text;
            TextRenderer.DrawText(e.Graphics, valor, combo.Font, Rectangle.Inflate(e.Bounds, -6, 0), corTexto,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        private void CalendarioButton_Paint(object sender, PaintEventArgs e) {
            Button botao = sender as Button;
            if (botao == null)
                return;

            bool hover = botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position));
            Color cor = hover
                ? Color.FromArgb(29, 78, 216)
                : Color.FromArgb(55, 96, 160);
            DesenharIconeCalendario(e.Graphics, botao.ClientRectangle, cor);
        }

        private static void DesenharIconeCalendario(Graphics graphics, Rectangle bounds, Color color) {
            float tamanho = Math.Min(18F, Math.Min(bounds.Width - 4F, bounds.Height - 4F));
            if (tamanho < 12F) return;

            float escala = tamanho / 18F;
            float x = bounds.Left + (bounds.Width - tamanho) / 2F;
            float y = bounds.Top + (bounds.Height - tamanho) / 2F;
            SmoothingMode oldSmoothing = graphics.SmoothingMode;
            PixelOffsetMode oldPixelOffset = graphics.PixelOffsetMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            try {
                using (Pen pen = new Pen(color, Math.Max(1F, 1.25F * escala)))
                using (Pen gridPen = new Pen(Color.FromArgb(115, color), Math.Max(0.85F, 0.95F * escala)))
                using (Brush dots = new SolidBrush(color))
                using (Brush headerFill = new SolidBrush(Color.FromArgb(235, 242, 255))) {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    gridPen.StartCap = LineCap.Round;
                    gridPen.EndCap = LineCap.Round;

                    RectangleF body = new RectangleF(x + 2.7F * escala, y + 4.3F * escala, 12.6F * escala, 12.2F * escala);
                    using (GraphicsPath path = CriarRetanguloArredondado(Rectangle.Round(body), Math.Max(1.5F, 2F * escala)))
                        graphics.DrawPath(pen, path);

                    graphics.FillRectangle(headerFill, x + 3.6F * escala, y + 5.2F * escala, 10.8F * escala, 2.5F * escala);
                    graphics.DrawLine(gridPen, x + 3.5F * escala, y + 8.4F * escala, x + 14.5F * escala, y + 8.4F * escala);
                    graphics.DrawLine(pen, x + 6.1F * escala, y + 2.2F * escala, x + 6.1F * escala, y + 5.5F * escala);
                    graphics.DrawLine(pen, x + 11.9F * escala, y + 2.2F * escala, x + 11.9F * escala, y + 5.5F * escala);

                    float d = 1.35F * escala;
                    float[,] dias = new float[,] {
                        { 5.4F, 10.2F }, { 9.0F, 10.2F }, { 12.0F, 10.2F },
                        { 5.4F, 13.1F }, { 9.0F, 13.1F }, { 12.0F, 13.1F }
                    };
                    for (int i = 0; i < dias.GetLength(0); i++)
                        graphics.FillEllipse(dots, x + dias[i, 0] * escala, y + dias[i, 1] * escala, d, d);
                }

            } finally {
                graphics.SmoothingMode = oldSmoothing;
                graphics.PixelOffsetMode = oldPixelOffset;
            }
        }

        private static GraphicsPath CriarRetanguloArredondado(Rectangle rect, float raio) {
            GraphicsPath path = new GraphicsPath();
            float diametro = raio * 2F;
            path.AddArc(rect.X, rect.Y, diametro, diametro, 180F, 90F);
            path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270F, 90F);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0F, 90F);
            path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90F, 90F);
            path.CloseFigure();
            return path;
        }

        private void ConfigurarCalendario() {
            calendarioFlutuante = new Panel();
            calendarioFlutuante.Name = "calendarioDataAula";
            calendarioFlutuante.Size = new Size(250, 240);
            calendarioFlutuante.BackColor = VisualPadrao.BordaCartao;
            calendarioFlutuante.Padding = new Padding(1);
            calendarioFlutuante.Visible = false;
            calendarioFlutuante.TabStop = false;

            Panel conteudo = new Panel();
            conteudo.Dock = DockStyle.Fill;
            conteudo.BackColor = Color.White;
            calendarioFlutuante.Controls.Add(conteudo);

            Button anterior = CriarBotaoMes("‹");
            anterior.Location = new Point(6, 6);
            anterior.Size = new Size(28, 25);
            anterior.Click += delegate {
                mesCalendarioVisualizado = mesCalendarioVisualizado.AddMonths(-1);
                AtualizarCalendario();
            };
            conteudo.Controls.Add(anterior);

            Button proximo = CriarBotaoMes("›");
            proximo.Location = new Point(215, 6);
            proximo.Size = new Size(28, 25);
            proximo.Click += delegate {
                mesCalendarioVisualizado = mesCalendarioVisualizado.AddMonths(1);
                AtualizarCalendario();
            };
            conteudo.Controls.Add(proximo);

            lblMesCalendario = new Label();
            lblMesCalendario.Name = "lblMesCalendario";
            lblMesCalendario.Location = new Point(34, 7);
            lblMesCalendario.Size = new Size(181, 23);
            lblMesCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblMesCalendario.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblMesCalendario.ForeColor = VisualPadrao.TextoPrincipal;
            lblMesCalendario.BackColor = Color.White;
            conteudo.Controls.Add(lblMesCalendario);

            string[] diasSemana = { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
            for (int coluna = 0; coluna < diasSemana.Length; coluna++) {
                Label diaSemana = new Label();
                diaSemana.Text = diasSemana[coluna];
                diaSemana.TextAlign = ContentAlignment.MiddleCenter;
                diaSemana.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
                diaSemana.ForeColor = VisualPadrao.TextoSecundario;
                diaSemana.BackColor = Color.White;
                diaSemana.SetBounds(8 + coluna * 33, 34, 31, 17);
                conteudo.Controls.Add(diaSemana);
            }

            for (int indice = 0; indice < 42; indice++) {
                Button dia = new Button();
                dia.Name = "btnDiaAula" + indice;
                dia.Tag = DateTime.MinValue;
                dia.SetBounds(8 + (indice % 7) * 33, 52 + (indice / 7) * 23, 31, 22);
                dia.FlatStyle = FlatStyle.Flat;
                dia.FlatAppearance.BorderSize = 0;
                dia.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
                dia.BackColor = Color.White;
                dia.ForeColor = VisualPadrao.TextoPrincipal;
                dia.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
                dia.Margin = new Padding(0);
                dia.Padding = new Padding(0);
                dia.TabStop = false;
                dia.Cursor = Cursors.Hand;
                dia.Click += DiaCalendario_Click;
                dia.MouseEnter += delegate(object sender, EventArgs e) {
                    Button b = sender as Button;
                    if (b != null)
                        EstilizarDiaCalendario(b, true);
                };
                dia.MouseLeave += delegate(object sender, EventArgs e) {
                    Button b = sender as Button;
                    if (b != null)
                        EstilizarDiaCalendario(b, false);
                };
                botoesDiasCalendario.Add(dia);
                conteudo.Controls.Add(dia);
            }

            lblHojeCalendario = new Label();
            lblHojeCalendario.Name = "lblHojeCalendario";
            lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");
            lblHojeCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblHojeCalendario.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105);
            lblHojeCalendario.BackColor = Color.White;
            lblHojeCalendario.Cursor = Cursors.Hand;
            lblHojeCalendario.SetBounds(8, 195, 232, 22);
            lblHojeCalendario.MouseEnter += delegate { lblHojeCalendario.ForeColor = VisualPadrao.Azul; lblHojeCalendario.BackColor = VisualPadrao.AzulClaro; };
            lblHojeCalendario.MouseLeave += delegate { lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105); lblHojeCalendario.BackColor = Color.White; };
            lblHojeCalendario.Click += delegate { SelecionarData(DateTime.Today); };
            conteudo.Controls.Add(lblHojeCalendario);

            pnlPresenca.Controls.Add(calendarioFlutuante);
            calendarioFlutuante.BringToFront();
            AtualizarCalendario();
        }

        private Button CriarBotaoMes(string textoBotao) {
            Button botao = new Button();
            botao.Text = textoBotao;
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            botao.BackColor = Color.White;
            botao.ForeColor = VisualPadrao.TextoSecundario;
            botao.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            botao.Cursor = Cursors.Hand;
            botao.TabStop = false;
            return botao;
        }

        private void AlternarCalendario() {
            if (calendarioFlutuante == null)
                return;

            if (calendarioFlutuante.Visible) {
                FecharCalendario();
                return;
            }

            mesCalendarioVisualizado = new DateTime(dataSelecionada.Year, dataSelecionada.Month, 1);
            AtualizarCalendario();

            Point abaixoNaTela = superficieDataAula.PointToScreen(new Point(0, superficieDataAula.Height + 3));
            Point posicao = pnlPresenca.PointToClient(abaixoNaTela);
            int margem = 4;
            if (posicao.X + calendarioFlutuante.Width > pnlPresenca.ClientSize.Width - margem)
                posicao.X = pnlPresenca.ClientSize.Width - calendarioFlutuante.Width - margem;
            if (posicao.Y + calendarioFlutuante.Height > pnlPresenca.ClientSize.Height - margem) {
                Point acimaNaTela = superficieDataAula.PointToScreen(new Point(0, -calendarioFlutuante.Height - 3));
                posicao = pnlPresenca.PointToClient(acimaNaTela);
            }
            posicao.X = Math.Max(margem, posicao.X);
            posicao.Y = Math.Max(margem, posicao.Y);
            calendarioFlutuante.Location = posicao;
            calendarioFlutuante.Visible = true;
            calendarioFlutuante.BringToFront();

            if (filtroCliqueCalendario == null)
                filtroCliqueCalendario = new FiltroCliqueCalendario(this);
            Application.AddMessageFilter(filtroCliqueCalendario);
        }

        private void FecharCalendario() {
            if (calendarioFlutuante != null)
                calendarioFlutuante.Visible = false;
            if (filtroCliqueCalendario != null)
                Application.RemoveMessageFilter(filtroCliqueCalendario);
        }

        public void FecharSeletores() {
            FecharCalendario();
        }

        private void AtualizarCalendario() {
            if (lblMesCalendario != null) {
                string mes = mesCalendarioVisualizado.ToString("MMMM 'de' yyyy", new CultureInfo("pt-BR"));
                lblMesCalendario.Text = char.ToUpper(mes[0], new CultureInfo("pt-BR")) + mes.Substring(1);
            }
            if (lblHojeCalendario != null)
                lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");

            DateTime primeiroDia = new DateTime(mesCalendarioVisualizado.Year, mesCalendarioVisualizado.Month, 1);
            DateTime inicioGrade = primeiroDia.AddDays(-(int)primeiroDia.DayOfWeek);
            for (int i = 0; i < botoesDiasCalendario.Count; i++) {
                Button dia = botoesDiasCalendario[i];
                dia.Tag = inicioGrade.AddDays(i).Date;
                dia.Text = ((DateTime)dia.Tag).Day.ToString(CultureInfo.InvariantCulture);
                EstilizarDiaCalendario(dia, false);
            }
        }

        private void EstilizarDiaCalendario(Button botao, bool hover) {
            if (botao == null || !(botao.Tag is DateTime))
                return;

            DateTime data = ((DateTime)botao.Tag).Date;
            bool mesAtual = data.Month == mesCalendarioVisualizado.Month && data.Year == mesCalendarioVisualizado.Year;
            bool selecionada = data.Date == dataSelecionada.Date;
            bool hoje = data.Date == DateTime.Today;

            botao.BackColor = selecionada ? VisualPadrao.Azul : (hover ? VisualPadrao.AzulClaro : Color.White);
            botao.ForeColor = selecionada
                ? Color.White
                : (mesAtual ? VisualPadrao.TextoPrincipal : Color.FromArgb(180, 188, 199));
            botao.Font = (selecionada || hoje)
                ? new Font("Segoe UI Semibold", 7.8F, FontStyle.Regular, GraphicsUnit.Point)
                : new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            botao.FlatAppearance.BorderColor = selecionada ? VisualPadrao.Azul : Color.White;
        }

        private void DiaCalendario_Click(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao != null && botao.Tag is DateTime)
                SelecionarData((DateTime)botao.Tag);
        }

        private void SelecionarData(DateTime data) {
            dataSelecionada = data.Date;
            txtDataAula.Text = dataSelecionada.ToString("dd/MM/yyyy");
            txtDataAula.ForeColor = VisualPadrao.TextoPrincipal;
            AtualizarCalendario();
            FecharCalendario();
            CarregarTurmasDaData();
        }

        // Prepara as colunas e o comportamento da grade de chamada
        private void ConfigurarTabelaPresenca(DataGridView tabela) {
            ConfigurarTabela(tabela);

            tabela.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabela.Columns["colAluno"].FillWeight = 40F;
            tabela.Columns["colAluno"].MinimumWidth = 180;
            tabela.Columns["colSituacoes"].FillWeight = 60F;
            tabela.Columns["colSituacoes"].MinimumWidth = 360;
            tabela.Columns["colAluno"].ReadOnly = true;
            tabela.Columns["colSituacoes"].ReadOnly = true;
            tabela.ReadOnly = true;
            tabela.EditMode = DataGridViewEditMode.EditProgrammatically;
            tabela.RowTemplate.Height = 54;
            tabela.Columns["colAluno"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabela.Columns["colAluno"].DefaultCellStyle.Padding = new Padding(16, 0, 8, 0);
            tabela.Columns["colSituacoes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.Columns["colSituacoes"].DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            foreach (DataGridViewColumn coluna in tabela.Columns)
                coluna.Resizable = DataGridViewTriState.False;

            tabela.CellPainting += Presenca_CellPainting;
            tabela.MouseClick += delegate(object sender, MouseEventArgs e) {
                if (e.Button != MouseButtons.Left)
                    return;
                DataGridView.HitTestInfo hit = tabela.HitTest(e.X, e.Y);
                if (hit.RowIndex < 0 || hit.ColumnIndex < 0 || tabela.Columns[hit.ColumnIndex].Name != "colSituacoes")
                    return;
                int botao = ObterBotaoSituacaoSobMouse(tabela, hit.RowIndex, new Point(e.X, e.Y));
                string situacao = botao == 0 ? "Presente" : botao == 1 ? "Ausente" : botao == 2 ? "Justificado" : null;
                if (situacao != null)
                    DefinirSituacaoLinha(tabela.Rows[hit.RowIndex], situacao);
            };
            tabela.MouseMove += delegate(object sender, MouseEventArgs e) {
                DataGridView.HitTestInfo hit = tabela.HitTest(e.X, e.Y);
                int linhaHover = -1;
                int botaoHover = -1;
                if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0 && tabela.Columns[hit.ColumnIndex].Name == "colSituacoes") {
                    linhaHover = hit.RowIndex;
                    botaoHover = ObterBotaoSituacaoSobMouse(tabela, linhaHover, new Point(e.X, e.Y));
                }
                if (linhaHover == linhaBotaoSituacaoHover && botaoHover == indiceBotaoSituacaoHover)
                    return;
                int linhaAnterior = linhaBotaoSituacaoHover;
                linhaBotaoSituacaoHover = linhaHover;
                indiceBotaoSituacaoHover = botaoHover;
                tabela.Cursor = botaoHover >= 0 ? Cursors.Hand : Cursors.Default;
                if (linhaAnterior >= 0 && linhaAnterior < tabela.Rows.Count)
                    tabela.InvalidateCell(tabela.Columns["colSituacoes"].Index, linhaAnterior);
                if (linhaHover >= 0)
                    tabela.InvalidateCell(tabela.Columns["colSituacoes"].Index, linhaHover);
            };
            tabela.MouseLeave += delegate {
                int linhaAnterior = linhaBotaoSituacaoHover;
                linhaBotaoSituacaoHover = -1;
                indiceBotaoSituacaoHover = -1;
                tabela.Cursor = Cursors.Default;
                if (linhaAnterior >= 0 && linhaAnterior < tabela.Rows.Count)
                    tabela.InvalidateCell(tabela.Columns["colSituacoes"].Index, linhaAnterior);
            };

            menuPresenca = new ContextMenuStrip();
            itemAgendarReposicao = new ToolStripMenuItem("Agendar reposição");
            itemAgendarReposicao.Click += delegate {
                if (tabela.CurrentRow != null)
                    TratarAcaoLinha(tabela.CurrentRow.Index);
            };
            menuPresenca.Items.Add(itemAgendarReposicao);
            menuPresenca.Opening += delegate {
                DataGridViewRow linha = tabela.CurrentRow;
                ParticipantePresenca participante = linha == null ? null : linha.Tag as ParticipantePresenca;
                string status = participante == null ? string.Empty : participante.Situacao ?? string.Empty;
                itemAgendarReposicao.Enabled = participante != null && !participante.EhReposicao &&
                    participante.IdReposicao > 0 &&
                    string.Equals(status, "Justificado", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(participante.SituacaoReposicao, "Concluida", StringComparison.OrdinalIgnoreCase);
            };
            tabela.MouseDown += delegate(object sender, MouseEventArgs e) {
                if (e.Button != MouseButtons.Right)
                    return;
                DataGridView.HitTestInfo hit = tabela.HitTest(e.X, e.Y);
                if (hit.RowIndex < 0)
                    return;
                tabela.ClearSelection();
                tabela.Rows[hit.RowIndex].Selected = true;
                tabela.CurrentCell = tabela.Rows[hit.RowIndex].Cells["colAluno"];
                menuPresenca.Show(tabela, e.Location);
            };

            AtualizarResumoPresenca();
        }

        private static void ConfigurarTabela(DataGridView tabela) {
            VisualPadrao.ConfigurarGrade(tabela);
        }
    }
}
