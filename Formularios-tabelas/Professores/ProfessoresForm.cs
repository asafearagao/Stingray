using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    public partial class ProfessoresForm : Form {
        private RoundedButton btnLimparFiltros;
        private readonly Dictionary<TextBox, string> placeholders = new Dictionary<TextBox, string>();
        private int linhaAcaoSobMouse = -1;
        private int botaoAcaoSobMouse = -1;
        private int linhaBotaoPressionado = -1;
        private int botaoPressionado = -1;

        // Inicializa a tela de professores
        public ProfessoresForm() {
            InitializeComponent();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            ConfigurarAparencia();
            if (emDesignTime)
                return;
        }
    }
}