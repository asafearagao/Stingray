using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray
{
    [DesignerCategory("Form")]
    public partial class TurmasForm : Form
    {
        private RoundedButton btnLimparFiltros;

        // Inicializa a tela de turmas, tabela e dados de demonstração
        public TurmasForm()
        {
            InitializeComponent();
            ConfigurarAparencia();
        }
    }
}