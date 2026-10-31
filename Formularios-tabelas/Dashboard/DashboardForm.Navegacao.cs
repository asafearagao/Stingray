using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray
{
    public partial class DashboardForm
    {
        private Button paginaSelecionada;
        private readonly Dictionary<Button, Form> paginasCriadas = new Dictionary<Button, Form>();

        private void ConfigurarEventosNavegacao()
        {
            foreach (Button botao in BotoesNavegacao())
            {
                RoundedButton botaoArredondado = botao as RoundedButton;
                if (botaoArredondado != null)
                    botaoArredondado.CornerRadius = 7;

                botao.Height = 40;
                botao.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point);
                botao.Padding = new Padding(18, 0, 4, 0);
                botao.TextAlign = ContentAlignment.MiddleLeft;
                botao.FlatAppearance.BorderSize = 0;
                botao.Click += Navegacao_Click;
                botao.MouseEnter += BotaoMenu_MouseEnter;
                botao.MouseLeave += BotaoMenu_MouseLeave;
                botao.SizeChanged += BotaoNavegacao_SizeChanged;
            }

            RoundedButton botaoSairArredondado = btnSair as RoundedButton;
            if (botaoSairArredondado != null)
                botaoSairArredondado.CornerRadius = 7;

            btnSair.Height = 40;
            btnSair.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnSair.Padding = new Padding(18, 0, 4, 0);
            btnSair.TextAlign = ContentAlignment.MiddleLeft;
            btnSair.FlatAppearance.BorderSize = 0;
            btnSair.Click += BtnSair_Click;
            btnSair.MouseEnter += BtnSair_MouseEnter;
            btnSair.MouseLeave += BtnSair_MouseLeave;

            navPanel.SizeChanged += NavPanel_SizeChanged;
            footerPanel.SizeChanged += FooterPanel_SizeChanged;
        }

        // Retorna os botões disponíveis na área lateral
        private Button[] BotoesNavegacao()
        {
            return new Button[]
            {
                btnDashboard,
                btnProfessores,
                btnTurmas,
                btnAlunos,
                btnPagamentos,
                btnPresenca,
                btnRelatorios,
                btnConfiguracoes
            };
        }

        private void AjustarLarguraNavegacao()
        {
            if (navPanel == null)
                return;

            int largura = Math.Max(120, navPanel.ClientSize.Width - navPanel.Padding.Left - navPanel.Padding.Right - 2);
            foreach (Button botao in BotoesNavegacao())
                botao.Width = largura;
        }

        // Exibe a página selecionada no painel principal
        private void MostrarPagina(Button botao)
        {
            if (botao == btnDashboard)
            {
                OcultarPaginasExceto(dashboardPagePanel);
                dashboardPagePanel.Visible = true;
                dashboardPagePanel.BringToFront();
                SelecionarPagina(btnDashboard);
                return;
            }

            Form pagina = ObterPagina(botao);
            if (pagina == null)
                return;

            OcultarPaginasExceto(pagina);
            PrepararPagina(pagina);
            SelecionarPagina(botao);
        }

        // Oculta todas as páginas, exceto a que permanecerá ativa
        private void OcultarPaginasExceto(Control paginaAtiva)
        {
            foreach (Control controle in contentHost.Controls)
            {
                if (controle == paginaAtiva)
                    continue;

                FecharSeletoresDaPagina(controle);
                controle.Visible = false;
            }
        }

        private void FecharSeletoresDaPagina(Control controle)
        {
            PagamentosForm paginaPagamentos = controle as PagamentosForm;
            if (paginaPagamentos != null)
                paginaPagamentos.FecharSeletores();

            PresencaForm paginaPresenca = controle as PresencaForm;
            if (paginaPresenca != null)
                paginaPresenca.FecharSeletores();
        }

        // Prepara a página para ser exibida dentro do painel principal
        private void PrepararPagina(Form pagina)
        {
            if (pagina.Parent != contentHost)
            {
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
        }

        private Form ObterPagina(Button botao)
        {
            Form pagina;
            if (paginasCriadas.TryGetValue(botao, out pagina))
                return pagina;

            if (botao == btnPagamentos)
                pagina = new PagamentosForm();
            else if (botao == btnProfessores)
                pagina = new ProfessoresForm();
            else if (botao == btnTurmas)
                pagina = new TurmasForm();
            else if (botao == btnAlunos)
                pagina = new AlunosForm();
            else if (botao == btnPresenca)
                pagina = new PresencaForm();
            else if (botao == btnRelatorios)
                pagina = new RelatoriosForm();
            else if (botao == btnConfiguracoes)
                pagina = new ConfiguracoesForm();
            else
                return null;

            paginasCriadas.Add(botao, pagina);
            return pagina;
        }

        // Atualiza a aparência dos botões e identifica a página selecionada
        private void SelecionarPagina(Button botao)
        {
            paginaSelecionada = botao;

            foreach (Button item in BotoesNavegacao())
            {
                bool selecionado = item == botao;
                item.BackColor = selecionado ? VisualPadrao.AzulClaro : Color.White;
                item.ForeColor = selecionado ? VisualPadrao.Azul : VisualPadrao.TextoPrincipal;

                RoundedButton botaoArredondado = item as RoundedButton;
                if (botaoArredondado != null)
                    botaoArredondado.ShowLeftIndicator = selecionado;

                item.FlatAppearance.BorderColor = selecionado
                    ? Color.FromArgb(185, 208, 242)
                    : Color.FromArgb(229, 231, 235);
                item.Invalidate();
            }
        }

        // Abre a página correspondente ao botão lateral selecionado
        private void Navegacao_Click(object sender, EventArgs e)
        {
            Button botao = sender as Button;
            if (botao != null)
                MostrarPagina(botao);
        }

        private void BotaoMenu_MouseEnter(object sender, EventArgs e)
        {
            Button botao = sender as Button;
            if (botao == null || botao == paginaSelecionada)
                return;

            AplicarAparenciaHover(botao);
        }

        private void BotaoMenu_MouseLeave(object sender, EventArgs e)
        {
            Button botao = sender as Button;
            if (botao == null || botao == paginaSelecionada)
                return;

            AplicarAparenciaNormal(botao);
        }

        private void AplicarAparenciaHover(Button botao)
        {
            botao.BackColor = VisualPadrao.AzulClaro;
            botao.ForeColor = VisualPadrao.Azul;
            botao.FlatAppearance.BorderColor = Color.FromArgb(185, 208, 242);
            botao.Invalidate();
        }

        private void AplicarAparenciaNormal(Button botao)
        {
            botao.BackColor = Color.White;
            botao.ForeColor = VisualPadrao.TextoPrincipal;
            botao.FlatAppearance.BorderColor = Color.FromArgb(229, 231, 235);
            botao.Invalidate();
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSair_MouseEnter(object sender, EventArgs e)
        {
            AplicarAparenciaHover(btnSair);
        }

        private void BtnSair_MouseLeave(object sender, EventArgs e)
        {
            AplicarAparenciaNormal(btnSair);
        }

        // Ajusta o layout do botão quando suas dimensões são alteradas
        private void BotaoNavegacao_SizeChanged(object sender, EventArgs e)
        {
            AjustarLarguraNavegacao();
        }

        // Atualiza a largura dos botões quando o painel de navegação é redimensionado
        private void NavPanel_SizeChanged(object sender, EventArgs e)
        {
            AjustarLarguraNavegacao();
        }

        // Ajusta os controles do rodapé quando o painel é redimensionado
        private void FooterPanel_SizeChanged(object sender, EventArgs e)
        {
            btnSair.Width = Math.Max(100, footerPanel.ClientSize.Width - footerPanel.Padding.Left - footerPanel.Padding.Right);
            btnSair.Left = footerPanel.Padding.Left;
            btnSair.Top = Math.Max(0, footerPanel.ClientSize.Height - footerPanel.Padding.Bottom - btnSair.Height);
        }
    }
}