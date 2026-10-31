using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray
{
    [DesignerCategory("Form")]
    public partial class RelatoriosForm : Form
    {
        // Inicializa a tela de relatórios
        public RelatoriosForm()
        {
            InitializeComponent();
            ConfigurarAparencia();
            CarregarDadosDemonstracao();

            if (DesignModeHelper.IsDesignMode(this))
                return;

            ConfigurarPopupPeriodoRelatorio(painelConteudo);
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            txtPeriodoRelatorio.Click += PeriodoRelatorio_Click;
            btnCalendarioPeriodoRelatorio.Click += PeriodoRelatorio_Click;
            cmbPeriodoGrafico.SelectedIndexChanged += CmbPeriodoGrafico_SelectedIndexChanged;
        }

        private void CmbPeriodoGrafico_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (graficoReceita == null)
                return;

            graficoReceita.QuantidadeMeses = cmbPeriodoGrafico.SelectedIndex == 1 ? 3 : 6;
            graficoReceita.Invalidate();
        }
    }
}