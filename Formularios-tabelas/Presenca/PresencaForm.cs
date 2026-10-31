using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class PresencaForm : Form {
        private Panel superficieDataAula;
        private Panel superficieTurma;
        private Button btnSetaTurma;
        private Panel calendarioFlutuante;
        private Label lblMesCalendario;
        private Label lblHojeCalendario;
        private DateTime mesCalendarioVisualizado = DateTime.Today;
        private DateTime dataSelecionada = DateTime.Today;
        private readonly List<Button> botoesDiasCalendario = new List<Button>();
        private IMessageFilter filtroCliqueCalendario;
        private RoundedButton btnSalvarChamada;
        private RoundedButton btnLimparChamada;

        private sealed class FiltroCliqueCalendario : IMessageFilter {
            private readonly PresencaForm formulario;

            // Inicializa o detector de cliques externos ao calendário
            public FiltroCliqueCalendario(PresencaForm form) {
                formulario = form;
            }

            public bool PreFilterMessage(ref Message m) {
                if (formulario.calendarioFlutuante == null || !formulario.calendarioFlutuante.Visible)
                    return false;

                if (m.Msg != 0x0201 && m.Msg != 0x0204 && m.Msg != 0x0207)
                    return false;

                Point ponto = Control.MousePosition;
                Rectangle areaCalendario = formulario.calendarioFlutuante.RectangleToScreen(
                    formulario.calendarioFlutuante.ClientRectangle);
                Rectangle areaData = formulario.superficieDataAula.RectangleToScreen(
                    formulario.superficieDataAula.ClientRectangle);
                if (!areaCalendario.Contains(ponto) && !areaData.Contains(ponto))
                    formulario.FecharCalendario();

                return false;
            }
        }

        // Inicializa a tela de presença
        public PresencaForm() {
            InitializeComponent();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);

            ConfigurarAparencia();
            ConfigurarBotoesChamada();
            txtDataAula.Text = dataSelecionada.ToString("dd/MM/yyyy");
            txtDataAula.ForeColor = VisualPadrao.TextoPrincipal;

            if (emDesignTime) {
                if (gdvPresenca.Rows.Count == 0) {
                    gdvPresenca.Rows.Add("Maria Silva", "Presente", "", "Marcar presente");
                    gdvPresenca.Rows.Add("João Pedro", "Ausente", "Reposição pendente", "Marcar presente");
                    gdvPresenca.Rows.Add("Ana Clara", "Justificado", "Atestado", "Marcar presente");
                }
                AtualizarResumoPresenca();
                return;
            }

            ConfigurarCalendario();
        }
    }
}