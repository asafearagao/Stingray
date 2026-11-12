using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class DashboardForm : Form {
        private Button paginaSelecionada;
        private readonly Dictionary<Button, Form> paginasCriadas = new Dictionary<Button, Form>();

        public DashboardForm() {
            InitializeComponent();
            ConfigurarBordasDosCartoes();
            bool emDesignTime = DesignModeHelper.IsDesignMode(this);
            if (emDesignTime)
                return;

            WindowState = FormWindowState.Maximized;
            ConfigurarEventosNavegacao();
            AjustarLarguraNavegacao();
            SelecionarPagina(btnDashboard);
        }

        private void DashboardContentPanel_Paint(object sender, PaintEventArgs e) {
            VisualPadrao.DesenharBordaCartao(e.Graphics, dashboardContentPanel.ClientRectangle);
        }

        private void DashAulasTabela_CellPaint(object sender, TableLayoutCellPaintEventArgs e) {
            if (e.Row < 3) {
                using (Pen linha = new Pen(Color.FromArgb(229, 234, 241), 1F))
                    e.Graphics.DrawLine(linha, e.CellBounds.Left + 1, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            }
        }

        private void ConfigurarBordasDosCartoes() {
            Control[] cartoes = {
                dashCardAlunos, dashCardProfessores, dashCardTurmas,
                dashCardAulas, dashCardPagamentos
            };

            foreach (Control cartao in cartoes) {
                if (cartao == null)
                    continue;

                Panel painel = cartao as Panel;
                if (painel == null)
                    continue;

                painel.BorderStyle = BorderStyle.None;
                painel.BackColor = Color.White;
                if (painel == dashCardAlunos || painel == dashCardProfessores || painel == dashCardTurmas)
                    painel.Padding = new Padding(2);
                painel.Paint -= DashboardCard_Paint;
                painel.Paint += DashboardCard_Paint;
            }
        }

        private void DashboardCard_Paint(object sender, PaintEventArgs e) {
            Control cartao = sender as Control;
            if (cartao != null)
                VisualPadrao.DesenharBordaCartao(e.Graphics, cartao.ClientRectangle);
        }

        private void ConfigurarEventosNavegacao() {
            foreach (Button botao in BotoesNavegacao()) {
                RoundedButton visual = botao as RoundedButton;
                if (visual != null)
                    visual.CornerRadius = 7;
                botao.Height = 40;
                botao.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point);
                botao.Padding = new Padding(12, 0, 4, 0);
                botao.TextAlign = ContentAlignment.MiddleLeft;
                botao.FlatAppearance.BorderSize = 0;
                botao.Click += Navegacao_Click;
                botao.MouseEnter += BotaoMenu_MouseEnter;
                botao.MouseLeave += BotaoMenu_MouseLeave;
                botao.SizeChanged += delegate { AjustarLarguraNavegacao(); };
            }

            RoundedButton sairArredondado = btnSair as RoundedButton;
            if (sairArredondado != null)
                sairArredondado.CornerRadius = 7;
            btnSair.Height = 40;
            btnSair.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnSair.Padding = new Padding(12, 0, 4, 0);
            btnSair.FlatAppearance.BorderSize = 0;
            btnSair.Click += delegate { Close(); };
            btnSair.MouseEnter += delegate {
                btnSair.BackColor = VisualPadrao.AzulClaro;
                btnSair.ForeColor = VisualPadrao.Azul;
                btnSair.FlatAppearance.BorderColor = Color.FromArgb(185, 208, 242);
                btnSair.Invalidate();
            };
            btnSair.MouseLeave += delegate {
                btnSair.BackColor = Color.White;
                btnSair.ForeColor = VisualPadrao.TextoPrincipal;
                btnSair.FlatAppearance.BorderColor = Color.FromArgb(229, 231, 235);
                btnSair.Invalidate();
            };
            navPanel.SizeChanged += delegate { AjustarLarguraNavegacao(); };
            footerPanel.SizeChanged += delegate {
                btnSair.Width = Math.Max(100, footerPanel.ClientSize.Width - footerPanel.Padding.Left - footerPanel.Padding.Right);
                btnSair.Left = footerPanel.Padding.Left;
                btnSair.Top = Math.Max(0, footerPanel.ClientSize.Height - footerPanel.Padding.Bottom - btnSair.Height);
            };
        }

        private Button[] BotoesNavegacao() {
            return new Button[] {
                btnDashboard, btnProfessores, btnTurmas,
                btnAlunos, btnPagamentos, btnPresenca, btnRelatorios, btnConfiguracoes
            };
        }

        private void AjustarLarguraNavegacao() {
            if (navPanel == null)
                return;
            int largura = Math.Max(120, navPanel.ClientSize.Width - navPanel.Padding.Left - navPanel.Padding.Right - 2);
            foreach (Button botao in BotoesNavegacao())
                botao.Width = largura;
        }

        private void Navegacao_Click(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao != null)
                MostrarPagina(botao);
        }
        private void MostrarPagina(Button botao) {
            if (botao == btnDashboard) {
                foreach (Control paginaVisivel in contentHost.Controls) {
                    if (paginaVisivel == dashboardPagePanel)
                        continue;

                    PagamentosForm pagamentos = paginaVisivel as PagamentosForm;
                    if (pagamentos != null)
                        pagamentos.FecharSeletores();
                    PresencaForm presenca = paginaVisivel as PresencaForm;
                    if (presenca != null)
                        presenca.FecharSeletores();
                    paginaVisivel.Visible = false;
                }

                dashboardPagePanel.Visible = true;
                dashboardPagePanel.BringToFront();
                SelecionarPagina(btnDashboard);
                return;
            }

            Form pagina = ObterPagina(botao);
            if (pagina == null)
                return;

            foreach (Control controle in contentHost.Controls) {
                if (controle != pagina) {
                    PagamentosForm pagamentos = controle as PagamentosForm;
                    if (pagamentos != null)
                        pagamentos.FecharSeletores();
                    PresencaForm presenca = controle as PresencaForm;
                    if (presenca != null)
                        presenca.FecharSeletores();
                    controle.Visible = false;
                }
            }

            if (pagina.Parent != contentHost) {
                pagina.TopLevel = false;
                pagina.FormBorderStyle = FormBorderStyle.None;
                pagina.ControlBox = false;
                pagina.Dock = DockStyle.Fill;
                pagina.Visible = false;
                contentHost.Controls.Add(pagina);
            }

            pagina.Dock = DockStyle.Fill;
            if (!pagina.Visible)
                pagina.Show();
            pagina.Visible = true;
            pagina.BringToFront();
            SelecionarPagina(botao);
        }

        private Form ObterPagina(Button botao) {
            Form pagina;
            if (paginasCriadas.TryGetValue(botao, out pagina))
                return pagina;

            if (botao == btnPagamentos) pagina = new PagamentosForm();
            else if (botao == btnProfessores) pagina = new ProfessoresForm();
            else if (botao == btnTurmas) pagina = new TurmasForm();
            else if (botao == btnAlunos) pagina = new AlunosForm();
            else if (botao == btnPresenca) pagina = new PresencaForm();
            else if (botao == btnRelatorios) pagina = new RelatoriosForm();
            else if (botao == btnConfiguracoes) pagina = new ConfiguracoesForm();
            else return null;

            paginasCriadas.Add(botao, pagina);
            return pagina;
        }

        private void SelecionarPagina(Button botao) {
            paginaSelecionada = botao;
            foreach (Button item in BotoesNavegacao()) {
                bool selecionado = item == botao;
                item.BackColor = selecionado
                    ? VisualPadrao.AzulClaro
                    : Color.White;
                item.ForeColor = selecionado
                    ? VisualPadrao.Azul
                    : VisualPadrao.TextoPrincipal;
                RoundedButton botaoArredondado = item as RoundedButton;
                if (botaoArredondado != null)
                    botaoArredondado.ShowLeftIndicator = selecionado;
                item.FlatAppearance.BorderColor = selecionado
                    ? Color.FromArgb(185, 208, 242)
                    : Color.FromArgb(229, 231, 235);
                item.Invalidate();
            }
        }
        private void BotaoMenu_MouseEnter(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao == null || botao == paginaSelecionada)
                return;
            botao.BackColor = VisualPadrao.AzulClaro;
            botao.ForeColor = VisualPadrao.Azul;
            botao.FlatAppearance.BorderColor = Color.FromArgb(185, 208, 242);
            botao.Invalidate();
        }
        private void BotaoMenu_MouseLeave(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao == null || botao == paginaSelecionada)
                return;
            botao.BackColor = Color.White;
            botao.ForeColor = VisualPadrao.TextoPrincipal;
            botao.FlatAppearance.BorderColor = Color.FromArgb(229, 231, 235);
            botao.Invalidate();
        }
    }
}
