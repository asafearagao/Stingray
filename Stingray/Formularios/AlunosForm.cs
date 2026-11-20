using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]

    // Coordena a atualização dos dados exibidos na área de alunos
    public partial class AlunosForm : Form {
        public AlunosForm() {
            InitializeComponent();
            painelConteudo.Paint += PainelConteudo_Paint;
            painelConteudo.BackColor = VisualPadrao.FundoPagina;
            ConstruirTela();
        }

        // Atualiza os dados persistidos sempre que a página volta a ser selecionada

        // no menu lateral. Os filtros atuais são preservados e reaplicados no MySQL
        public void AtualizarDados() {
            if (IsDisposed || Disposing || dgvAlunos == null || dgvAlunos.IsDisposed)
                return;

            CarregarAlunos();
        }

        private void PainelConteudo_Paint(object sender, PaintEventArgs e) {
            VisualPadrao.DesenharBordaCartao(e.Graphics, painelConteudo.ClientRectangle);
        }
    }
}
