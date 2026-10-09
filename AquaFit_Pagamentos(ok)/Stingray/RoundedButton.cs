using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray {
    [DefaultEvent("Click")]
    [ToolboxItem(true)]
    public class RoundedButton : Button {
        private int cornerRadius = 7;
        private bool pointerInside;
        private bool pointerPressed;
        private const float BorderThickness = 1f;

        public RoundedButton() {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            FlatStyle = FlatStyle.Flat;
            // A borda é desenhada neste controle. A borda nativa fica desligada
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.White;
        }

        [Category("Appearance")]
        [DefaultValue(7)]
        public int CornerRadius {
            get { return cornerRadius; }
            set {
                int adjusted = Math.Max(0, value);
                if (cornerRadius == adjusted) return;
                cornerRadius = adjusted;
                Invalidate();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            // WinForms não oferece transparência para controles elementos dentro de outros (filhos). Pintar o fundo
            // do objeto pai evita que os cantos arredondados revelem pixels pretos
            Color background = Parent != null ? Parent.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(background))
                e.Graphics.FillRectangle(brush, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (ClientSize.Width < 2 || ClientSize.Height < 2) return;

            Graphics graphics = e.Graphics;
            SmoothingMode oldSmoothing = graphics.SmoothingMode;
            PixelOffsetMode oldOffset = graphics.PixelOffsetMode;
            CompositingQuality oldCompositing = graphics.CompositingQuality;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            try {
                // Limpa antes de desenhar a forma
                Color background = Parent != null ? Parent.BackColor : SystemColors.Control;
                using (SolidBrush backgroundBrush = new SolidBrush(background))
                    graphics.FillRectangle(backgroundBrush, ClientRectangle);

                RectangleF bounds = new RectangleF(
                    0.5f,
                    0.5f,
                    Math.Max(1f, ClientSize.Width - 1f),
                    Math.Max(1f, ClientSize.Height - 1f));

                using (GraphicsPath path = CreateRoundedPath(bounds, cornerRadius))
                using (SolidBrush fill = new SolidBrush(GetDisplayedBackColor())) {
                    graphics.FillPath(fill, path);

                    Color borderColor = Enabled ? FlatAppearance.BorderColor : SystemColors.ControlDark;
                    if (borderColor == Color.Empty)
                        borderColor = Color.FromArgb(229, 231, 235);

                    using (Pen border = new Pen(borderColor, BorderThickness)) {
                        border.Alignment = PenAlignment.Center;
                        border.LineJoin = LineJoin.Round;
                        graphics.DrawPath(border, path);
                    }
                }

                DrawButtonText(graphics);
            }
            finally {
                graphics.SmoothingMode = oldSmoothing;
                graphics.PixelOffsetMode = oldOffset;
                graphics.CompositingQuality = oldCompositing;
            }
        }

        private Color GetDisplayedBackColor() {
            if (!Enabled) return SystemColors.Control;
            if (pointerPressed && FlatAppearance.MouseDownBackColor != Color.Empty)
                return FlatAppearance.MouseDownBackColor;
            if (pointerInside && FlatAppearance.MouseOverBackColor != Color.Empty)
                return FlatAppearance.MouseOverBackColor;
            return BackColor;
        }

        private void DrawButtonText(Graphics graphics) {
            if (string.IsNullOrEmpty(Text)) return;

            Rectangle area = ClientRectangle;
            area.X += Padding.Left + 2;
            area.Y += Padding.Top;
            area.Width -= Padding.Horizontal + 4;
            area.Height -= Padding.Vertical;
            if (area.Width <= 0 || area.Height <= 0) return;

            TextFormatFlags flags = TextFormatFlags.SingleLine |
                                    TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.NoPrefix;

            switch (TextAlign) {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Right;
                    break;
                default:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(graphics, Text, Font, area, textColor, flags);
        }

        protected override void OnMouseEnter(EventArgs e) {
            pointerInside = true;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            pointerInside = false;
            pointerPressed = false;
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) pointerPressed = true;
            base.OnMouseDown(e);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            pointerPressed = false;
            base.OnMouseUp(e);
            Invalidate();
        }

        private static GraphicsPath CreateRoundedPath(RectangleF rect, int radius) {
            GraphicsPath path = new GraphicsPath();
            float diameter = Math.Min(Math.Max(0, radius) * 2f, Math.Min(rect.Width, rect.Height));
            if (diameter < 1f) {
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}