using System.Windows.Forms;
using System.Drawing;

namespace Stingray
{
    public partial class ProfessoresForm : Form
    {
        public ProfessoresForm()
        {
            InitializeComponent();
            painelConteudo.Paint += PainelConteudo_Paint;
        }

        private void PainelConteudo_Paint(object sender, PaintEventArgs e)
        {
            Rectangle area = painelConteudo.ClientRectangle;
            if (area.Width <= 1 || area.Height <= 1)
                return;

            area.Width -= 1;
            area.Height -= 1;
            using (Pen borda = new Pen(Color.FromArgb(226, 232, 240)))
            {
                e.Graphics.DrawRectangle(borda, area);
            }
        }

    }
}
