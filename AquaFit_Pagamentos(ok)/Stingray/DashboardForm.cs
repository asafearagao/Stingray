using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    [DesignerCategory("Form")]
    public partial class DashboardForm : Form {
        private readonly Color azul = Color.FromArgb(25, 86, 196);
        private readonly Color azulClaro = Color.FromArgb(234, 242, 255);
        private readonly Color textoEscuro = Color.FromArgb(55, 65, 81);
        private Button paginaSelecionada;
        private readonly Dictionary<Button, Form> paginasCriadas = new Dictionary<Button, Form>();

        public DashboardForm() {
            InitializeComponent();
            CentralizarLogo();
            logoPanel.SizeChanged += delegate { CentralizarLogo(); };
            dashboardContentPanel.Paint += DashboardContentPanel_Paint;

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            WindowState = FormWindowState.Maximized;
            ConfigurarEventosNavegacao();
            AjustarLarguraNavegacao();
            SelecionarPagina(btnDashboard);
        }


        private void DashboardContentPanel_Paint(object sender, PaintEventArgs e) {
            Rectangle area = dashboardContentPanel.ClientRectangle;
            if (area.Width <= 1 || area.Height <= 1)
                return;

            area.Width -= 1;
            area.Height -= 1;
            using (Pen borda = new Pen(Color.FromArgb(226, 232, 240))) {
                e.Graphics.DrawRectangle(borda, area);
            }
        }

        private void CentralizarLogo() {
            if (logoPanel == null || lblBrand == null || lblTagline == null)
                return;

            int alturaGrupo = lblBrand.Height + lblTagline.Height + 1;
            int topo = Math.Max(8, (logoPanel.ClientSize.Height - alturaGrupo) / 2);
            lblBrand.Location = new Point(Math.Max(0, (logoPanel.ClientSize.Width - lblBrand.Width) / 2), topo);
            lblTagline.Location = new Point(Math.Max(0, (logoPanel.ClientSize.Width - lblTagline.Width) / 2), topo + lblBrand.Height + 1);
        }

        private void ConfigurarEventosNavegacao() {
            foreach (Button botao in BotoesNavegacao()) {
                botao.Click += Navegacao_Click;
                botao.MouseEnter += BotaoMenu_MouseEnter;
                botao.MouseLeave += BotaoMenu_MouseLeave;
                botao.SizeChanged += delegate { AjustarLarguraNavegacao(); };
            }

            btnSair.Click += delegate { Close(); };
            btnSair.MouseEnter += delegate {
                btnSair.BackColor = azulClaro;
                btnSair.ForeColor = azul;
                btnSair.FlatAppearance.BorderColor = Color.FromArgb(185, 208, 242);
                btnSair.Invalidate();
            };
            btnSair.MouseLeave += delegate {
                btnSair.BackColor = Color.White;
                btnSair.ForeColor = textoEscuro;
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
                btnDashboard, btnAlunos, btnProfessores, btnTurmas, btnHorarios,
                btnMatriculas, btnPagamentos, btnPresenca, btnRelatorios, btnConfiguracoes
            };
        }

        private void AjustarLarguraNavegacao() {
            if (navPanel == null) return;
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
                    if (pagamentos != null) pagamentos.FecharSeletores();
                    paginaVisivel.Visible = false;
                }

                dashboardPagePanel.Visible = true;
                dashboardPagePanel.BringToFront();
                SelecionarPagina(btnDashboard);
                return;
            }

            Form pagina = ObterPagina(botao);
            if (pagina == null) return;

            foreach (Control controle in contentHost.Controls) {
                if (controle != pagina) {
                    PagamentosForm pagamentos = controle as PagamentosForm;
                    if (pagamentos != null) pagamentos.FecharSeletores();
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
            else if (botao == btnAlunos) pagina = new AlunosForm();
            else if (botao == btnProfessores) pagina = new ProfessoresForm();
            else if (botao == btnTurmas) pagina = new TurmasForm();
            else if (botao == btnHorarios) pagina = new HorariosForm();
            else if (botao == btnMatriculas) pagina = new MatriculasForm();
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
                item.BackColor = selecionado ? azulClaro : Color.White;
                item.ForeColor = selecionado ? azul : textoEscuro;
                item.FlatAppearance.BorderColor = selecionado
                    ? Color.FromArgb(185, 208, 242)
                    : Color.FromArgb(229, 231, 235);
                item.Invalidate();
            }
        }
        private void BotaoMenu_MouseEnter(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao == null || botao == paginaSelecionada) return;
            botao.BackColor = azulClaro;
            botao.ForeColor = azul;
            botao.FlatAppearance.BorderColor = Color.FromArgb(185, 208, 242);
            botao.Invalidate();
        }
        private void BotaoMenu_MouseLeave(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao == null || botao == paginaSelecionada) return;
            botao.BackColor = Color.White;
            botao.ForeColor = textoEscuro;
            botao.FlatAppearance.BorderColor = Color.FromArgb(229, 231, 235);
            botao.Invalidate();
        }

        // Aqui acaba a parte da barra lateral e começa o código do dashboard    
    }
}