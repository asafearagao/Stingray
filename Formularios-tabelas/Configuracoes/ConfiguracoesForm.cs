using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class ConfiguracoesForm : Form {
        // Estado utilizado pelas verificações
        private Timer timerEstadoWhatsApp;
        private bool verificacaoEmAndamento;
        private bool carregandoLogo;
        private DateTime ultimaTentativaLogo = DateTime.MinValue;

        // Inicializa a tela de configurações
        public ConfiguracoesForm() {
            InitializeComponent();
            ConfigurarAparencia();

            if (DesignModeHelper.IsDesignMode(this)) {
                PrepararPreVisualizacao();
                return;
            }

            ConfigurarEventos();
        }

        private void PrepararPreVisualizacao() {
            lblEstadoWhatsApp.Text = "Pré-visualização";
            lblDescricaoWhatsApp.Text = "A integração será verificada durante a execução do aplicativo.";
            btnAbrirWhatsApp.Visible = true;
        }
    }
}