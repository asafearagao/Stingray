using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class AlunosForm : Form {
        public AlunosForm() {
            InitializeComponent();
            painelConteudo.Paint += PainelConteudo_Paint;
            painelConteudo.BackColor = VisualPadrao.FundoPagina;
            ConstruirTela();
        }

        private void PainelConteudo_Paint(object sender, PaintEventArgs e) {
            VisualPadrao.DesenharBordaCartao(e.Graphics, painelConteudo.ClientRectangle);
        }
    }
}
