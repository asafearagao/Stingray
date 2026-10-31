using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray {
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class PagamentosForm : System.Windows.Forms.Form {
        // Controles auxiliares do estado dos filtros e do calendário
        private Panel popupPeriodo;
        private Panel campoDataInicial;
        private Panel campoDataFinal;
        private TextBox txtDataInicial;
        private TextBox txtDataFinal;
        private Button btnCalendarioInicial;
        private Button btnCalendarioFinal;
        private Button btnSetaTurma;
        private Button btnSetaStatus;
        private Panel turmaContentPanel;
        private Panel statusContentPanel;
        private Panel calendarioFlutuante;
        private Label lblMesCalendario;
        private Label lblHojeCalendario;
        private DateTime mesCalendarioVisualizado = DateTime.Today;
        private readonly List<Button> botoesDiasCalendario = new List<Button>();
        private readonly Font fonteDiaCalendario = new Font("Segoe UI", 7.8F, FontStyle.Regular);
        private readonly Font fonteDiaCalendarioHoje = new Font("Segoe UI Semibold", 7.8F, FontStyle.Regular);
        private Panel campoDataAtivo;
        private DateTime? dataInicialSelecionada;
        private DateTime? dataFinalSelecionada;
        private IMessageFilter filtroCliqueExterno;
        private bool placeholderAluno = true;
        private int linhaAcaoSobMousePg = -1;
        private int botaoAcaoSobMousePg = -1;
        private DateTime? dataInicialAplicada;
        private DateTime? dataFinalAplicada;

        private enum TipoIconeCalendario {
            Periodo,
            DataInicial,
            DataFinal
        }

        private sealed class CalendarDayButton : Button {
            protected override bool ShowFocusCues { get { return false; } }
        }

        // Inicializa a tela de pagamentos e prepara filtros, calendário e tabela
        public PagamentosForm() {
            InitializeComponent();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);

            ConfigurarAparenciaFiltros();
            ConfigurarPosicaoBotaoNovoPagamento();

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            if (emDesignTime) {
                txtPeriodo.Text = "Selecione o período";
                txtPeriodo.ForeColor = VisualPadrao.TextoPlaceholder;
                txtAluno.Text = "Digite o nome do aluno...";
                txtAluno.ForeColor = VisualPadrao.TextoPlaceholder;
                PreencherPagamentosDeExemplo();
                return;
            }

            ConfigurarEventosPagamento();
            ConfigurarPopupPeriodo();
            PreencherPagamentosDeExemplo();
        }

        // Registra os eventos dos filtros, botões e tabela de pagamentos
        private void ConfigurarEventosPagamento() {
            btnCalendario.Click += delegate { AlternarPopupPeriodo(); };
            btnPesquisar.Click += delegate { AplicarPesquisaVisual(); };
            btnLimpar.Click += delegate { LimparFiltros(); };
            btnLimpar.MouseEnter += delegate {
                btnLimpar.ForeColor = Color.FromArgb(185, 28, 28);
                btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
                btnLimpar.BackColor = Color.FromArgb(254, 242, 242);
            };
            btnLimpar.MouseLeave += delegate {
                btnLimpar.ForeColor = VisualPadrao.TextoPrincipal;
                btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                btnLimpar.BackColor = Color.White;
            };
            btnLimpar.MouseDown += delegate {
                btnLimpar.ForeColor = Color.FromArgb(153, 27, 27);
                btnLimpar.BackColor = Color.FromArgb(254, 226, 226);
            };
            btnLimpar.MouseUp += delegate {
                if (btnLimpar.ClientRectangle.Contains(btnLimpar.PointToClient(Cursor.Position))) {
                    btnLimpar.ForeColor = Color.FromArgb(185, 28, 28);
                    btnLimpar.BackColor = Color.FromArgb(254, 242, 242);
                }
            };

            txtAluno.Enter += delegate {
                if (placeholderAluno) {
                    txtAluno.Clear();
                    txtAluno.ForeColor = VisualPadrao.TextoPrincipal;
                    placeholderAluno = false;
                }
            };
            txtAluno.Leave += delegate {
                if (string.IsNullOrWhiteSpace(txtAluno.Text)) {
                    txtAluno.Text = "Digite o nome do aluno...";
                    txtAluno.ForeColor = VisualPadrao.TextoPlaceholder;
                    placeholderAluno = true;
                }
            };
            gridPayments.CellPainting += GridPayments_CellPainting;
            gridPayments.CellContentClick += GridPayments_CellContentClick;
            gridPayments.CellMouseMove += GridPayments_CellMouseMove;
            gridPayments.CellMouseLeave += GridPayments_CellMouseLeave;
            colTurma.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // Configura posicao botão novo pagamento
        private void ConfigurarPosicaoBotaoNovoPagamento() {
            if (headerPanel == null || btnNovoPagamento == null)
                return;

            btnNovoPagamento.Visible = true;
            if (!headerPanel.Controls.Contains(btnNovoPagamento))
                headerPanel.Controls.Add(btnNovoPagamento);

            btnNovoPagamento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovoPagamento.Size = new Size(194, 36);
            btnNovoPagamento.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnNovoPagamento.BringToFront();
            PosicionarBotaoNovoPagamento();
            headerPanel.Resize += delegate { PosicionarBotaoNovoPagamento(); };
        }

        // Posiciona o botão novo pagamento
        private void PosicionarBotaoNovoPagamento() {
            if (headerPanel == null || btnNovoPagamento == null)
                return;

            int x = Math.Max(8, headerPanel.ClientSize.Width - btnNovoPagamento.Width - 10);
            int y = Math.Max(4, (headerPanel.ClientSize.Height - btnNovoPagamento.Height) / 2);
            btnNovoPagamento.Location = new Point(x, y);
            btnNovoPagamento.Visible = true;
        }

    }
}