using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray
{
    [DesignerCategory("Form")]
    public partial class DashboardForm : Form
    {
        // Inicializa o dashboard e configura a navegação entre as telas
        public DashboardForm()
        {
            InitializeComponent();
            CarregarDadosDemonstracao();
            ConfigurarBordasDosCartoes();

            if (DesignModeHelper.IsDesignMode(this))
                return;

            WindowState = FormWindowState.Maximized;
            ConfigurarEventosNavegacao();
            AjustarLarguraNavegacao();
            SelecionarPagina(btnDashboard);
        }
    }
}