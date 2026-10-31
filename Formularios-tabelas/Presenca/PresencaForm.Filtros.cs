using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray
{
    public partial class PresencaForm
    {
        // Prepara o campo de filtro
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
            }

            foreach (Control controle in controles) {
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

    }
}