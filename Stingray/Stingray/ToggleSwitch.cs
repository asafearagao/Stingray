using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray {
    /// <summary>Interruptor liga/desliga azul e bola branca quando ativo
    public sealed class ToggleSwitch : CheckBox {
        public ToggleSwitch() {
            AutoSize = false;
            Size = new Size(46, 24);
            Cursor = Cursors.Hand;
            Text = string.Empty;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent == null ? BackColor : Parent.BackColor);
            Rectangle trilho = new Rectangle(1, 2, Width - 3, Height - 5);
            int diametro = trilho.Height - 6;
            using (GraphicsPath path = new GraphicsPath()) {
                int d = trilho.Height;
                path.AddArc(trilho.X, trilho.Y, d, d, 90, 180);
                path.AddArc(trilho.Right - d, trilho.Y, d, d, 270, 180);
                path.CloseFigure();
                using (SolidBrush fundo = new SolidBrush(Checked ? Color.FromArgb(37, 99, 235) : Color.FromArgb(203, 213, 225)))
                    e.Graphics.FillPath(fundo, path);
            }
            int x = Checked ? Width - diametro - 5 : 5;
            Rectangle bola = new Rectangle(x, (Height - diametro) / 2, diametro, diametro);
            using (SolidBrush branco = new SolidBrush(Color.White))
                e.Graphics.FillEllipse(branco, bola);
        }
    }
}