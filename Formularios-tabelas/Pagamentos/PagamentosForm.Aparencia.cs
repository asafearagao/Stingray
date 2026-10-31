using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray {
    public partial class PagamentosForm {
        // Configura a aparência dos campos, seletores e botões de filtro
        private void ConfigurarAparenciaFiltros() {
            periodoField.BackColor = Color.White;
            alunoField.BackColor = Color.White;
            PrepararCampoSeletor(statusField, cmbStatus, out statusContentPanel);
            txtPeriodo.BackColor = Color.White;
            txtAluno.BackColor = Color.White;
            txtAluno.BorderStyle = BorderStyle.None;
            txtAluno.Dock = DockStyle.None;
            txtAluno.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPeriodo.Dock = DockStyle.None;
            txtPeriodo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnCalendario.Dock = DockStyle.None;
            btnCalendario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCalendario.AccessibleName = "Selecionar período completo";
            btnCalendario.AccessibleDescription = "Abre o seletor de data inicial e data final";
            ConfigurarBotaoIconeCalendario(btnCalendario, TipoIconeCalendario.Periodo);
            cmbStatus.Dock = DockStyle.None;
            cmbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Mantém o valor selecionado com fundo branco
            cmbStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cmbStatus.ItemHeight = 18;
            cmbStatus.DrawItem += ComboFiltro_DrawItem;
            cmbStatus.DropDownClosed += delegate { cmbStatus.Invalidate(); statusField.Invalidate(); };

            CriarBotaoSetaCombo(statusContentPanel, cmbStatus, false);

            periodoField.Paint += CampoFiltro_Paint;
            alunoField.Paint += CampoFiltro_Paint;
            cmbStatus.AutoSize = false;
            cmbStatus.FlatStyle = FlatStyle.Flat;

            periodoField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            alunoField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            statusField.SizeChanged += delegate { PosicionarControlesFiltro(); };
            if (statusContentPanel != null) statusContentPanel.SizeChanged += delegate { PosicionarControlesFiltro(); };
            PosicionarControlesFiltro();
        }

        private void PrepararCampoSeletor(Panel campo, ComboBox combo, out Panel conteudo) {
            campo.Paint -= CampoFiltro_Paint;
            campo.Padding = new Padding(1);
            campo.BackColor = Color.FromArgb(203, 213, 225);

            conteudo = null;
            foreach (Control controle in campo.Controls) {
                Panel candidata = controle as Panel;
                if (candidata != null && candidata.Name == campo.Name + "Content") {
                    conteudo = candidata;
                    break;
                }
            }
            if (conteudo == null) {
                conteudo = new Panel();
                conteudo.Name = campo.Name + "Content";
                conteudo.Dock = DockStyle.Fill;
                conteudo.Margin = new Padding(0);
                conteudo.Padding = new Padding(0);
                conteudo.BackColor = Color.White;
                campo.Controls.Add(conteudo);
            }
            if (combo.Parent != conteudo)
                conteudo.Controls.Add(combo);
            combo.BackColor = Color.White;
        }

        private void CriarBotaoSetaCombo(Panel campo, ComboBox combo, bool turma) {
            Button seta = turma ? btnSetaTurma : btnSetaStatus;
            if (seta == null)
                seta = new Button();
            seta.Name = turma ? "btnSetaTurma" : "btnSetaStatus";
            seta.AccessibleName = turma ? "Abrir opções de turma" : "Abrir opções de status";
            seta.AccessibleDescription = "Abre a lista de opções";
            seta.Text = string.Empty;
            seta.TabStop = false;
            seta.Cursor = Cursors.Hand;
            seta.FlatStyle = FlatStyle.Flat;
            seta.FlatAppearance.BorderSize = 0;
            seta.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            seta.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            seta.BackColor = Color.White;
            seta.UseVisualStyleBackColor = false;
            seta.Padding = new Padding(0);
            seta.Margin = new Padding(0);
            seta.Paint += ComboSeta_Paint;
            seta.MouseEnter += delegate { seta.BackColor = Color.FromArgb(239, 246, 255); seta.Invalidate(); };
            seta.MouseLeave += delegate { seta.BackColor = Color.White; seta.Invalidate(); };
            seta.Click += delegate {
                if (combo == null || combo.IsDisposed) return;
                combo.Focus();
                combo.DroppedDown = true;
            };

            if (seta.Parent != campo)
                campo.Controls.Add(seta);
            seta.BringToFront();
            if (turma)
                btnSetaTurma = seta;
            else
                btnSetaStatus = seta;
        }

        private void ComboSeta_Paint(object sender, PaintEventArgs e) {
            Button seta = sender as Button;
            if (seta == null)
                return;

            Rectangle r = seta.ClientRectangle;
            if (r.Width < 8 || r.Height < 8)
                return;

            bool hover = r.Contains(seta.PointToClient(Cursor.Position));
            Color corSeta = hover ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                e.Graphics.DrawLine(separador, 0, 1, 0, r.Height - 2);

            float cx = r.Width / 2F;
            float cy = r.Height / 2F;
            using (Pen pen = new Pen(corSeta, 1.6F)) {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                e.Graphics.DrawLines(pen, new PointF[] {
                    new PointF(cx - 3.5F, cy - 1.5F),
                    new PointF(cx, cy + 2F),
                    new PointF(cx + 3.5F, cy - 1.5F)
                });
            }
        }

        private void ComboFiltro_DrawItem(object sender, DrawItemEventArgs e) {
            ComboBox combo = sender as ComboBox;
            if (combo == null || e.Bounds.Width <= 0 || e.Bounds.Height <= 0)
                return;

            bool itemSelecionadoNaLista = combo.DroppedDown && e.Index >= 0 &&
                (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color fundo = itemSelecionadoNaLista ? VisualPadrao.Azul : Color.White;
            Color texto = itemSelecionadoNaLista ? Color.White : VisualPadrao.TextoPrincipal;

            using (Brush brushFundo = new SolidBrush(fundo))
                e.Graphics.FillRectangle(brushFundo, e.Bounds);

            string valor = e.Index >= 0 && e.Index < combo.Items.Count
                ? Convert.ToString(combo.Items[e.Index])
                : combo.Text;
            Rectangle areaTexto = Rectangle.Inflate(e.Bounds, -6, 0);
            TextRenderer.DrawText(e.Graphics, valor, combo.Font, areaTexto, texto,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        // Desenha a borda do campo de filtro
        private void CampoFiltro_Paint(object sender, PaintEventArgs e) {
            Panel panel = sender as Panel;
            if (panel == null || panel.ClientSize.Width < 2 || panel.ClientSize.Height < 2)
                return;

            // Ajusta a posição da caneta/lápis
            using (Pen pen = new Pen(Color.FromArgb(203, 213, 225), 1F)) {
                e.Graphics.DrawRectangle(pen, 0, 0,
                    Math.Max(0, panel.ClientSize.Width - 1),
                    Math.Max(0, panel.ClientSize.Height - 1));
            }
        }

        private void ConfigurarBotaoIconeCalendario(Button botao, TipoIconeCalendario tipo) {
            if (botao == null)
                return;
            botao.Tag = tipo;
            botao.Text = string.Empty;
            botao.Padding = new Padding(0);
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            botao.BackColor = Color.White;
            botao.UseVisualStyleBackColor = false;
            botao.Cursor = Cursors.Hand;
            botao.Paint += CalendarioButton_Paint;
            botao.MouseEnter += delegate { botao.Invalidate(); };
            botao.MouseLeave += delegate { botao.Invalidate(); };
        }

        private void CalendarioButton_Paint(object sender, PaintEventArgs e) {
            Button botao = sender as Button;
            if (botao == null)
                return;

            bool hover = botao.ClientRectangle.Contains(botao.PointToClient(Cursor.Position));
            Color cor = hover ? Color.FromArgb(29, 78, 216) : Color.FromArgb(55, 96, 160);
            TipoIconeCalendario tipo = botao.Tag is TipoIconeCalendario
                ? (TipoIconeCalendario) botao.Tag
                : TipoIconeCalendario.DataInicial;
            DesenharIconeCalendario(e.Graphics, botao.ClientRectangle, cor, tipo);
        }

        private static void DesenharIconeCalendario(Graphics graphics, Rectangle bounds, Color color, TipoIconeCalendario tipo) {
            float tamanho = Math.Min(18F, Math.Min(bounds.Width - 4F, bounds.Height - 4F));
            if (tamanho < 12F)
                return;

            float escala = tamanho / 18F;
            float x = bounds.Left + (bounds.Width - tamanho) / 2F;
            float y = bounds.Top + (bounds.Height - tamanho) / 2F;

            var oldSmoothing = graphics.SmoothingMode;
            var oldPixelOffset = graphics.PixelOffsetMode;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            try {
                using (Pen pen = new Pen(color, Math.Max(1F, 1.25F * escala)))
                using (Pen gridPen = new Pen(Color.FromArgb(115, color), Math.Max(0.85F, 0.95F * escala)))
                using (Brush dots = new SolidBrush(color))
                using (Brush headerFill = new SolidBrush(Color.FromArgb(235, 242, 255))) {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    gridPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    gridPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                    RectangleF body = new RectangleF(x + 2.7F * escala, y + 4.3F * escala,
                        12.6F * escala, 12.2F * escala);
                    using (var path = CriarRetanguloArredondado( Rectangle.Round(body), Math.Max(1.5F, 2F * escala))) {
                        graphics.DrawPath(pen, path);
                    }

                    graphics.FillRectangle(headerFill, x + 3.6F * escala, y + 5.2F * escala, 10.8F * escala, 2.5F * escala);
                    graphics.DrawLine(gridPen, x + 3.5F * escala, y + 8.4F * escala, x + 14.5F * escala, y + 8.4F * escala);

                    graphics.DrawLine(pen, x + 6.1F * escala, y + 2.2F * escala,
                        x + 6.1F * escala, y + 5.5F * escala);
                    graphics.DrawLine(pen, x + 11.9F * escala, y + 2.2F * escala,
                        x + 11.9F * escala, y + 5.5F * escala);

                    float d = 1.35F * escala;
                    float[,] dias = new float[,] {
                        { 5.4F, 10.2F }, { 9.0F, 10.2F },
                        { 12.0F, 10.2F }, { 5.4F, 13.1F },
                        { 9.0F, 13.1F }, { 12.0F, 13.1F }
                    };
                    for (int i = 0; i < dias.GetLength(0); i++)
                        graphics.FillEllipse(dots, x + dias[i, 0] * escala, y + dias[i, 1] * escala, d, d);
                }

            } finally {
                graphics.SmoothingMode = oldSmoothing;
                graphics.PixelOffsetMode = oldPixelOffset;
            }
        }

        // Cria o caminho gráfico com cantos arredondados
        private static System.Drawing.Drawing2D.GraphicsPath CriarRetanguloArredondado(Rectangle rect, float raio) {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            float diametro = raio * 2F;
            path.AddArc(rect.X, rect.Y, diametro, diametro, 180F, 90F);
            path.AddArc(rect.Right - diametro, rect.Y, diametro, diametro, 270F, 90F);
            path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0F, 90F);
            path.AddArc(rect.X, rect.Bottom - diametro, diametro, diametro, 90F, 90F);
            path.CloseFigure();
            return path;
        }

        private void PosicionarControlesFiltro() {
            if (periodoField != null && periodoField.ClientSize.Width > 30 && periodoField.ClientSize.Height > 4) {
                int h = periodoField.ClientSize.Height;
                int buttonWidth = 25;
                int buttonHeight = Math.Max(18, Math.Min(24, h - 4));
                btnCalendario.SetBounds(
                    periodoField.ClientSize.Width - buttonWidth - 2,
                    Math.Max(1, (h - buttonHeight) / 2), buttonWidth, buttonHeight);

                int textHeight = txtPeriodo.Height;
                txtPeriodo.SetBounds(6, Math.Max(0, (h - textHeight) / 2),
                    Math.Max(20, periodoField.ClientSize.Width - buttonWidth - 12), textHeight);
            }

            if (alunoField != null && txtAluno != null && alunoField.ClientSize.Width > 20 && alunoField.ClientSize.Height > 4) {
                int hAluno = alunoField.ClientSize.Height;
                int alturaTextoAluno = txtAluno.Height;
                txtAluno.SetBounds(6, Math.Max(0, (hAluno - alturaTextoAluno) / 2),
                    Math.Max(20, alunoField.ClientSize.Width - 12), alturaTextoAluno);
            }

            PosicionarComboNoCampo(statusContentPanel, cmbStatus, btnSetaStatus);
        }
        
        private static void PosicionarComboNoCampo(Panel campo, ComboBox combo, Button seta) {
            if (campo == null || combo == null || campo.ClientSize.Width < 30 || campo.ClientSize.Height < 12)
                return;

            const int larguraSeta = 24;
            int alturaDisponivel = Math.Max(18, campo.ClientSize.Height - 2);
            int altura = Math.Min(22, alturaDisponivel);
            int topo = Math.Max(0, (campo.ClientSize.Height - altura) / 2);
            combo.SetBounds(0, topo, campo.ClientSize.Width, altura);

            if (seta != null) {
                int alturaSeta = campo.ClientSize.Height;
                seta.SetBounds(Math.Max(0, campo.ClientSize.Width - larguraSeta),
                    0, larguraSeta, alturaSeta);
                seta.BringToFront();
            }
        }
    }
}