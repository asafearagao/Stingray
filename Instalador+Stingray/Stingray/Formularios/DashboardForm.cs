using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.Servicos;
using Stingray.DadosTela;
using System.Reflection;

namespace Stingray {
    [DesignerCategory("Form")]

    // Monta os indicadores, as listas e a navegação do Dashboard
    public partial class DashboardForm : Form {
        private Button paginaSelecionada;
        private readonly Dictionary<Button, Form> paginasCriadas = new Dictionary<Button, Form>();
        private readonly Usuario usuarioLogado;
        private readonly DashboardServico dashboardServico = new DashboardServico();
        private readonly FrequenciaServico frequenciaServico = new FrequenciaServico();
        private Panel dashCardInativos;
        private Panel dashCardReposicoes;
        private Label dashQuantidadeInativos;
        private Label dashQuantidadeReposicoes;
        private FlowLayoutPanel dashListaInativos;
        private FlowLayoutPanel dashListaReposicoes;
        private Timer timerAtualizacaoFrequencia;
        private int linhasProximasAulasVisiveis;

        public DashboardForm() : this(null) {
        }

        // Construtor que recebe o usuário logado para exibir seu nome e controlar permissões
        public DashboardForm(Usuario usuarioLogado) {
            this.usuarioLogado = usuarioLogado;
            InitializeComponent();
            ConfigurarBordasDosCartoes();
            CriarCartoesFrequenciaDashboard();
            ConfigurarLogoAplicativo();
            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);
            if (emDesignTime)
                return;

            WindowState = FormWindowState.Maximized;
            ConfigurarEventosNavegacao();
            ConfigurarEventosProximasAulas();
            AjustarLarguraNavegacao();
            SelecionarPagina(btnDashboard);
            AtualizarIndicadoresDashboard();
            timerAtualizacaoFrequencia = new Timer { Interval = 60000 };
            timerAtualizacaoFrequencia.Tick += delegate { AtualizarCartoesFrequenciaDashboard(); };
            timerAtualizacaoFrequencia.Start();
            FormClosed += delegate
            {
                timerAtualizacaoFrequencia.Stop();
                timerAtualizacaoFrequencia.Dispose();
            };
        }

        private void DashboardContentPanel_Paint(object sender, PaintEventArgs e) {
            VisualPadrao.DesenharBordaCartao(e.Graphics, dashboardContentPanel.ClientRectangle);
        }

        private void DashAulasTabela_CellPaint(object sender, TableLayoutCellPaintEventArgs e) {
            if (e.Row < linhasProximasAulasVisiveis - 1) {
                using (Pen linha = new Pen(Color.FromArgb(229, 234, 241), 1F))
                    e.Graphics.DrawLine(linha, e.CellBounds.Left + 1, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            }
        }

        // Configura as bordas arredondadas e a cor de fundo dos cartões do Dashboard
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
                if (painel == dashCardAlunos || painel == dashCardProfessores || painel == dashCardTurmas) {
                    // Mantém o conteúdo afastado da curva do cartão. Os controles filhos
                    // do WinForms pintam em retângulos próprios e não herdam o Region
                    // do painel pai; com 10 px de respiro, eles não cobrem as quinas
                    painel.Padding = new Padding(10);
                }
                painel.Paint -= DashboardCard_Paint;
                painel.Paint += DashboardCard_Paint;

                // Recorta a superfície e os controles filhos pela mesma geometria
                // usada na borda. Sem esse recorte, um controle filho com fundo
                // opaco pode avançar visualmente sobre as extremidades arredondadas
                PrepararRecorteCartao(painel);
            }
        }

        // Mantém o conteúdo interno recortado pelo mesmo raio da borda. Isso evita

        // que fundos brancos dos controles filhos cubram visualmente os cantos
        private void PrepararRecorteCartao(Panel cartao) {
            if (cartao == null || cartao.IsDisposed)
                return;

            cartao.SizeChanged -= CartaoArredondado_SizeChanged;
            cartao.SizeChanged += CartaoArredondado_SizeChanged;
            AplicarRegiaoArredondada(cartao);
        }

        private void CartaoArredondado_SizeChanged(object sender, EventArgs e) {
            AplicarRegiaoArredondada(sender as Panel);
        }

        // Aplica a região arredondada aos limites do cartão
        private void AplicarRegiaoArredondada(Panel cartao) {
            if (cartao == null || cartao.IsDisposed || cartao.ClientSize.Width < 12 || cartao.ClientSize.Height < 12)
                return;

            const int raio = 10;
            Rectangle area = new Rectangle(0, 0, cartao.ClientSize.Width, cartao.ClientSize.Height);
            int diametro = Math.Min(raio * 2, Math.Min(area.Width, area.Height));
            using (GraphicsPath caminho = new GraphicsPath()) {
                caminho.AddArc(area.Left, area.Top, diametro, diametro, 180, 90);
                caminho.AddArc(area.Right - diametro, area.Top, diametro, diametro, 270, 90);
                caminho.AddArc(area.Right - diametro, area.Bottom - diametro, diametro, diametro, 0, 90);
                caminho.AddArc(area.Left, area.Bottom - diametro, diametro, diametro, 90, 90);
                caminho.CloseFigure();

                Region regiaoAnterior = cartao.Region;
                cartao.Region = new Region(caminho);
                if (regiaoAnterior != null)
                    regiaoAnterior.Dispose();
            }
        }

        // Desenha a borda arredondada de um cartão do Dashboard
        private void DashboardCard_Paint(object sender, PaintEventArgs e) {
            Control cartao = sender as Control;
            if (cartao == null || cartao.ClientSize.Width < 12 || cartao.ClientSize.Height < 12)
                return;

            Rectangle area = Rectangle.Inflate(cartao.ClientRectangle, -1, -1);
            const int raio = 10;
            int diametro = raio * 2;
            using (GraphicsPath caminho = new GraphicsPath()) {
                caminho.AddArc(area.Left, area.Top, diametro, diametro, 180, 90);
                caminho.AddArc(area.Right - diametro, area.Top, diametro, diametro, 270, 90);
                caminho.AddArc(area.Right - diametro, area.Bottom - diametro, diametro, diametro, 0, 90);
                caminho.AddArc(area.Left, area.Bottom - diametro, diametro, diametro, 90, 90);
                caminho.CloseFigure();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen borda = new Pen(Color.FromArgb(222, 230, 241), 1F))
                    e.Graphics.DrawPath(borda, caminho);
            }
        }

        // Configura os eventos de clique e hover dos botões de navegação do menu lateral
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

        // Retorna os botões de navegação do menu lateral
        private Button[] BotoesNavegacao() {
            return new Button[] {
                btnDashboard, btnProfessores, btnTurmas,
                btnAlunos, btnPagamentos, btnPresenca, btnRelatorios, btnConfiguracoes
            };
        }

        // Ajusta a largura dos botões de navegação para preencher o painel lateral
        private void AjustarLarguraNavegacao() {
            if (navPanel == null)
                return;
            int largura = Math.Max(120, navPanel.ClientSize.Width - navPanel.Padding.Left - navPanel.Padding.Right - 2);
            foreach (Button botao in BotoesNavegacao())
                botao.Width = largura;
        }

        // Trata o clique em um botão de navegação e exibe a página correspondente
        private void Navegacao_Click(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao != null)
                MostrarPagina(botao);
        }

        // Exibe a página correspondente ao botão clicado, ocultando as demais páginas
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
                AtualizarIndicadoresDashboard();
                return;
            }

            // Para os demais botões, obtém a página correspondente e exibe-a
            // A informação indica se a página já existia: na primeira abertura,
            // o próprio construtor carrega os dados; nas próximas, atualizamos
            // a partir do banco ao retornar pelo menu lateral
            bool paginaJaCriada = paginasCriadas.ContainsKey(botao);
            Form pagina = ObterPagina(botao);
            if (pagina == null)
                return;

            // Oculta todas as páginas, exceto a selecionada
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

            // Exibe a página selecionada
            pagina.Dock = DockStyle.Fill;
            if (!pagina.Visible)
                pagina.Show();
            pagina.Visible = true;
            pagina.BringToFront();

            // Recarrega alunos ao clicar novamente no botão lateral, mantendo
            // busca e seleções atuais de turma, professor e status
            if (paginaJaCriada && botao == btnAlunos) {
                AlunosForm alunos = pagina as AlunosForm;
                if (alunos != null)
                    alunos.AtualizarDados();
            }

            SelecionarPagina(botao);
        }

        // Atualiza os números e os dados resumidos do Dashboard
        private void AtualizarIndicadoresDashboard() {
            try {
                ResumoDashboard resumo = dashboardServico.ObterResumo();
                dashValorAlunos.Text = resumo.TotalAlunos.ToString();
                dashValorProfessores.Text = resumo.TotalProfessores.ToString();
                dashValorTurmas.Text = resumo.TotalTurmas.ToString();
            }
            catch {
                // Mantém o painel utilizável mesmo quando o banco estiver indisponível
                dashValorAlunos.Text = "—";
                dashValorProfessores.Text = "—";
                dashValorTurmas.Text = "—";
            }

            AtualizarProximasAulas();
            AtualizarCartoesFrequenciaDashboard();
        }

        // Monta os cartões com os indicadores de frequência
        private void CriarCartoesFrequenciaDashboard()
        {
            dashBottomVisual.SuspendLayout();
            dashBottomVisual.RowCount = 2;
            dashBottomVisual.ColumnStyles.Clear();
            dashBottomVisual.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            dashBottomVisual.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.667F));
            dashBottomVisual.RowStyles.Clear();
            dashBottomVisual.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            dashBottomVisual.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            TableLayoutPanel cartoesFrequencia = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            cartoesFrequencia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            cartoesFrequencia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            cartoesFrequencia.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            dashCardInativos = CriarCartaoListaFrequencia("Alunos inativos", out dashQuantidadeInativos, out dashListaInativos);
            dashCardReposicoes = CriarCartaoListaFrequencia("Reposições pendentes", out dashQuantidadeReposicoes, out dashListaReposicoes);
            // Alinha as extremidades inferiores às extremidades externas do conjunto superior
            dashCardInativos.Margin = new Padding(0, 6, 8, 8);
            dashCardReposicoes.Margin = new Padding(8, 6, 0, 8);
            cartoesFrequencia.Controls.Add(dashCardInativos, 0, 0);
            cartoesFrequencia.Controls.Add(dashCardReposicoes, 1, 0);
            dashBottomVisual.Controls.Add(cartoesFrequencia, 0, 1);
            dashBottomVisual.SetColumnSpan(cartoesFrequencia, 2);
            dashBottomVisual.ResumeLayout(true);
        }

        private Panel CriarCartaoListaFrequencia(string titulo, out Label quantidade, out FlowLayoutPanel lista)
        {
            Panel cartao = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 9, 12, 10),
                Margin = new Padding(7, 6, 9, 8)
            };
            cartao.Paint += DashboardCard_Paint;
            PrepararRecorteCartao(cartao);

            TableLayoutPanel estrutura = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            estrutura.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            estrutura.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            estrutura.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel cabecalho = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            Label nome = new Label
            {
                Text = titulo,
                AutoSize = true,
                Location = new Point(2, 7),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 58, 138)
            };
            quantidade = new Label
            {
                Text = "—",
                AutoSize = false,
                Size = new Size(42, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(Math.Max(0, cabecalho.ClientSize.Width - 42), 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(29, 78, 216),
                BackColor = Color.FromArgb(239, 246, 255)
            };
            Label quantidadeLocal = quantidade;
            cabecalho.SizeChanged += delegate
            {
                quantidadeLocal.Left = Math.Max(0, cabecalho.ClientSize.Width - quantidadeLocal.Width);
            };
            cabecalho.Controls.Add(nome);
            cabecalho.Controls.Add(quantidade);

            lista = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(6),
                Margin = new Padding(0)
            };
            FlowLayoutPanel painelLista = lista;
            painelLista.SizeChanged += delegate { AjustarLarguraItensDashboard(painelLista); };

            estrutura.Controls.Add(cabecalho, 0, 0);
            estrutura.Controls.Add(lista, 0, 1);
            cartao.Controls.Add(estrutura);
            return cartao;
        }

        // Atualiza os cartões após a consulta dos dados
        private void AtualizarCartoesFrequenciaDashboard()
        {
            try
            {
                List<AlunoInativoDashboard> inativos = frequenciaServico.ListarAlunosInativos(10);
                List<ReposicaoPendenteDashboard> reposicoes = frequenciaServico.ListarReposicoesPendentes(10);
                dashQuantidadeInativos.Text = frequenciaServico.ContarAlunosInativos().ToString();
                dashQuantidadeReposicoes.Text = frequenciaServico.ContarReposicoesPendentes().ToString();

                dashListaInativos.Controls.Clear();
                foreach (AlunoInativoDashboard aluno in inativos)
                {
                    Label item = CriarLinhaDashboard(aluno.Nome + " · " + aluno.PercentualFaltas.ToString("0.#") + "% de faltas");
                    item.Cursor = Cursors.Default;
                    item.Width = LarguraItemDashboard(dashListaInativos);
                    dashListaInativos.Controls.Add(item);
                }
                if (inativos.Count == 0)
                {
                    Label vazio = CriarLinhaDashboard("Nenhum aluno inativado automaticamente.");
                    vazio.Width = LarguraItemDashboard(dashListaInativos);
                    vazio.Cursor = Cursors.Default;
                    dashListaInativos.Controls.Add(vazio);
                }

                dashListaReposicoes.Controls.Clear();
                foreach (ReposicaoPendenteDashboard reposicao in reposicoes)
                {
                    string texto = reposicao.Nome + " · " + reposicao.ModalidadeOriginal + " (" +
                        reposicao.DataOriginal.ToString("dd/MM") + ") → " + reposicao.Destino;
                    Label item = CriarLinhaDashboard(texto);
                    item.Cursor = Cursors.Hand;
                    item.AccessibleDescription = "Clique para agendar ou alterar a reposição desta ausência";
                    item.Click += delegate { AbrirPopupAgendarReposicao(reposicao); };
                    item.Width = LarguraItemDashboard(dashListaReposicoes);
                    dashListaReposicoes.Controls.Add(item);
                }
                if (reposicoes.Count == 0)
                {
                    Label vazio = CriarLinhaDashboard("Nenhuma reposição pendente.");
                    vazio.Width = LarguraItemDashboard(dashListaReposicoes);
                    vazio.Cursor = Cursors.Default;
                    dashListaReposicoes.Controls.Add(vazio);
                }
            }
            catch
            {
                dashQuantidadeInativos.Text = "—";
                dashQuantidadeReposicoes.Text = "—";
                dashListaInativos.Controls.Clear();
                dashListaReposicoes.Controls.Clear();
                Label falhaInativos = CriarLinhaDashboard("Não foi possível carregar os dados.");
                Label falhaReposicoes = CriarLinhaDashboard("Não foi possível carregar os dados.");
                falhaInativos.Width = LarguraItemDashboard(dashListaInativos);
                falhaReposicoes.Width = LarguraItemDashboard(dashListaReposicoes);
                falhaInativos.Cursor = Cursors.Default;
                falhaReposicoes.Cursor = Cursors.Default;
                dashListaInativos.Controls.Add(falhaInativos);
                dashListaReposicoes.Controls.Add(falhaReposicoes);
            }
        }

        private static int LarguraItemDashboard(FlowLayoutPanel lista)
        {
            if (lista == null)
                return 170;

            int largura = lista.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 8;
            return Math.Max(170, largura);
        }

        private static void AjustarLarguraItensDashboard(FlowLayoutPanel lista)
        {
            if (lista == null || lista.IsDisposed)
                return;

            int largura = LarguraItemDashboard(lista);
            foreach (Control item in lista.Controls)
                item.Width = largura;
        }

        private static Label CriarLinhaDashboard(string texto)
        {
            return new Label
            {
                Text = texto,
                AutoSize = false,
                Size = new Size(400, 28),
                Margin = new Padding(0, 1, 0, 2),
                Padding = new Padding(5, 5, 5, 3),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 8.2F),
                Cursor = Cursors.Hand,
                AutoEllipsis = true
            };
        }

        // Carrega as próximas aulas que aparecem no Dashboard
        private void AtualizarProximasAulas()
        {
            // Atualiza a lista de próximas aulas no Dashboard, exibindo até 4 aulas
            Label[] horarios = { dashAulaHora1, dashAulaHora2, dashAulaHora3, dashAulaHora4 };
            Label[] modalidades = { dashAulaNome1, dashAulaNome2, dashAulaNome3, dashAulaNome4 };
            Label[] professores = { dashAulaProfessor1, dashAulaProfessor2, dashAulaProfessor3, dashAulaProfessor4 };
            TableLayoutPanel[] paineis = { dashAulaInfo1, dashAulaInfo2, dashAulaInfo3, dashAulaInfo4 };

            try
            {
                List<ProximaAulaDashboard> aulas = dashboardServico.ObterProximasAulas(4);

                // Mantém somente as linhas que possuem aula, evitando divisórias vazias
                ConfigurarLinhasProximasAulas(aulas.Count);

                for (int i = 0; i < horarios.Length; i++)
                {
                    bool existeAula = i < aulas.Count;
                    horarios[i].Text = existeAula ? aulas[i].DataHora.ToString("HH:mm") : string.Empty;
                    modalidades[i].Text = existeAula
                        ? aulas[i].Modalidade + " · " + aulas[i].DataHora.ToString("dd/MM")
                        : string.Empty;
                    professores[i].Text = existeAula
                        ? "Professor: " + aulas[i].NomeProfessor
                        : string.Empty;
                    paineis[i].Tag = existeAula ? aulas[i] : null;
                    paineis[i].Cursor = existeAula ? Cursors.Hand : Cursors.Default;
                    foreach (Control controle in paineis[i].Controls)
                    {
                        controle.Tag = existeAula ? aulas[i] : null;
                        controle.Cursor = existeAula ? Cursors.Hand : Cursors.Default;
                    }
                }

                if (aulas.Count == 0)
                {
                    modalidades[0].Text = "Nenhuma próxima aula encontrada.";
                    professores[0].Text = string.Empty;
                }
            }
            catch
            {
                ConfigurarLinhasProximasAulas(0);
                horarios[0].Text = string.Empty;
                modalidades[0].Text = "Não foi possível carregar as próximas aulas.";
                professores[0].Text = string.Empty;

                for (int i = 0; i < horarios.Length; i++)
                {
                    if (i > 0)
                    {
                        horarios[i].Text = string.Empty;
                        modalidades[i].Text = string.Empty;
                        professores[i].Text = string.Empty;
                    }
                    paineis[i].Tag = null;
                    foreach (Control controle in paineis[i].Controls)
                        controle.Tag = null;
                }
            }
        }

        private void ConfigurarLinhasProximasAulas(int quantidade)
        {
            int linhasVisiveis = Math.Max(1, Math.Min(4, quantidade));
            Label[] horarios = { dashAulaHora1, dashAulaHora2, dashAulaHora3, dashAulaHora4 };
            TableLayoutPanel[] paineis = { dashAulaInfo1, dashAulaInfo2, dashAulaInfo3, dashAulaInfo4 };

            linhasProximasAulasVisiveis = linhasVisiveis;
            dashAulasTabela.Height = linhasVisiveis * 70;
            dashAulasTabela.SuspendLayout();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    bool visivel = i < linhasVisiveis;
                    horarios[i].Visible = visivel;
                    paineis[i].Visible = visivel;

                    RowStyle estilo = dashAulasTabela.RowStyles[i];
                    if (visivel)
                    {
                        estilo.SizeType = SizeType.Percent;
                        estilo.Height = 100F / linhasVisiveis;
                    }
                    else
                    {
                        estilo.SizeType = SizeType.Absolute;
                        estilo.Height = 0F;
                    }
                }
            }
            finally
            {
                dashAulasTabela.ResumeLayout(true);
            }
        }

        private void ConfigurarLogoAplicativo()
        {
            if (!ModoDesign.EstaEmModoDesign(this))
            {
                try
                {
                    Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
                catch { }
            }

            lblBrand.Visible = false;
            lblTagline.Visible = false;
            PictureBox logo = new PictureBox
            {
                Name = "picLogoAquaFit",
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Anchor = AnchorStyles.None,
                Size = new Size(150, 92)
            };
            try
            {
                using (var fluxo = Assembly.GetExecutingAssembly().GetManifestResourceStream("Stingray.Assets.AquaFit.png"))
                {
                    if (fluxo != null)
                        using (Image imagem = Image.FromStream(fluxo))
                            logo.Image = new Bitmap(imagem);
                }
            }
            catch { }
            logoPanel.Controls.Add(logo);
            EventHandler ajustarLogo = delegate
            {
                int margemSuperior = 16;
                int espacoVertical = Math.Max(0, logoPanel.ClientSize.Height - logo.Height - margemSuperior);
                logo.Location = new Point((logoPanel.ClientSize.Width - logo.Width) / 2,
                    margemSuperior + espacoVertical / 2);
            };
            logoPanel.SizeChanged += ajustarLogo;
            ajustarLogo(this, EventArgs.Empty);
        }

        private void ConfigurarEventosProximasAulas()
        {
            TableLayoutPanel[] paineis = { dashAulaInfo1, dashAulaInfo2, dashAulaInfo3, dashAulaInfo4 };
            foreach (TableLayoutPanel painel in paineis)
            {
                painel.Cursor = Cursors.Hand;
                painel.Click += ProximaAula_Click;
                foreach (Control controle in painel.Controls)
                {
                    controle.Cursor = Cursors.Hand;
                    controle.Click += ProximaAula_Click;
                }
            }
        }

        private void ProximaAula_Click(object sender, EventArgs e)
        {
            Control controle = sender as Control;
            ProximaAulaDashboard aula = null;
            while (controle != null && aula == null)
            {
                aula = controle.Tag as ProximaAulaDashboard;
                controle = controle.Parent;
            }
            if (aula != null)
                AbrirPopupRemarcarAula(aula);
        }

        // Abre o fluxo para agendar uma reposição
        private void AbrirPopupAgendarReposicao(ReposicaoPendenteDashboard reposicao)
        {
            if (reposicao == null)
                return;

            using (Form dialogo = new Form())
            {
                dialogo.Text = "Agendar reposição";
                dialogo.ClientSize = new Size(470, 235);
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;

                Label titulo = new Label
                {
                    Text = reposicao.Nome,
                    Location = new Point(22, 18),
                    Size = new Size(420, 24),
                    Font = new Font("Segoe UI Semibold", 11F),
                    ForeColor = VisualPadrao.TextoPrincipal
                };
                Label origem = new Label
                {
                    Text = "Ausência justificada: " + reposicao.ModalidadeOriginal + " · " + reposicao.DataOriginal.ToString("dd/MM/yyyy"),
                    Location = new Point(22, 48),
                    Size = new Size(425, 30),
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = VisualPadrao.TextoSecundario
                };
                Label legendaData = new Label
                {
                    Text = "Data da reposição",
                    Location = new Point(22, 92),
                    AutoSize = true,
                    ForeColor = VisualPadrao.TextoSecundario
                };
                DateTimePicker data = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(22, 115),
                    Width = 150,
                    MinDate = DateTime.Today,
                    Value = reposicao.DataDestino != DateTime.MinValue && reposicao.DataDestino.Date >= DateTime.Today
                        ? reposicao.DataDestino.Date : DateTime.Today.AddDays(1)
                };
                Label legendaTurma = new Label
                {
                    Text = "Aula de destino",
                    Location = new Point(195, 92),
                    AutoSize = true,
                    ForeColor = VisualPadrao.TextoSecundario
                };
                ComboBox turmas = new ComboBox
                {
                    Location = new Point(195, 115),
                    Size = new Size(250, 28),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DisplayMember = "DescricaoAgenda"
                };
                VisualPadrao.ConfigurarComboPopup(turmas);
                Button cancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(255, 180),
                    Size = new Size(88, 34),
                    DialogResult = DialogResult.Cancel
                };
                RoundedButton confirmar = new RoundedButton
                {
                    Text = "Confirmar reposição",
                    Location = new Point(350, 180),
                    Size = new Size(100, 34),
                    CornerRadius = 6,
                    BackColor = VisualPadrao.Azul,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                confirmar.FlatAppearance.BorderSize = 0;

                Action carregarTurmas = delegate
                {
                    turmas.BeginUpdate();
                    turmas.Items.Clear();
                    try
                    {
                        List<Turma> disponiveis = frequenciaServico.ListarTurmasDaData(data.Value.Date);
                        foreach (Turma turma in disponiveis)
                            turmas.Items.Add(turma);
                        if (reposicao.IdTurmaDestino > 0)
                        {
                            for (int i = 0; i < turmas.Items.Count; i++)
                            {
                                Turma turma = turmas.Items[i] as Turma;
                                if (turma != null && turma.IdTurma == reposicao.IdTurmaDestino)
                                {
                                    turmas.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        if (turmas.SelectedIndex < 0 && turmas.Items.Count > 0)
                            turmas.SelectedIndex = 0;
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(dialogo,
                            "Não foi possível carregar as aulas de destino.\r\n\r\n" + erro.Message,
                            "Agendar reposição", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        turmas.EndUpdate();
                    }
                };
                data.ValueChanged += delegate { carregarTurmas(); };
                carregarTurmas();

                confirmar.Click += delegate
                {
                    try
                    {
                        Turma destino = turmas.SelectedItem as Turma;
                        if (destino == null)
                            throw new InvalidOperationException("Não há aula de destino disponível para a data escolhida.");
                        frequenciaServico.AgendarReposicao(reposicao.IdMatricula,
                            reposicao.IdOcorrenciaOriginal, destino.IdTurma, data.Value.Date);
                        dialogo.DialogResult = DialogResult.OK;
                        dialogo.Close();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(dialogo, erro.Message, "Agendar reposição",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                dialogo.Controls.AddRange(new Control[] { titulo, origem, legendaData, data,
                    legendaTurma, turmas, cancelar, confirmar });
                dialogo.AcceptButton = confirmar;
                dialogo.CancelButton = cancelar;
                EstiloPopup.Aplicar(dialogo);
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    AtualizarCartoesFrequenciaDashboard();
            }
        }

        // Abre o fluxo para remarcar uma aula existente
        private void AbrirPopupRemarcarAula(ProximaAulaDashboard aula)
        {
            using (Form dialogo = new Form())
            {
                dialogo.Text = "Remarcar aula";
                dialogo.ClientSize = new Size(390, 178);

                Label titulo = new Label
                {
                    Text = aula.Modalidade + " · " + aula.DataHora.ToString("dd/MM/yyyy HH:mm"),
                    AutoSize = false,
                    Location = new Point(20, 20),
                    Size = new Size(350, 28),
                    Font = new Font("Segoe UI Semibold", 10F),
                    ForeColor = Color.FromArgb(31, 41, 55)
                };
                Label legenda = new Label
                {
                    Text = "Nova data da aula",
                    AutoSize = true,
                    Location = new Point(20, 65),
                    ForeColor = Color.FromArgb(100, 116, 139)
                };
                DateTimePicker data = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(20, 88),
                    Width = 150,
                    MinDate = DateTime.Today.AddDays(1),
                    Value = aula.DataHora.Date.AddDays(1) > DateTime.Today ? aula.DataHora.Date.AddDays(1) : DateTime.Today.AddDays(1)
                };
                RoundedButton salvar = new RoundedButton
                {
                    Text = "Confirmar remarcação",
                    Location = new Point(202, 130),
                    Size = new Size(168, 32),
                    CornerRadius = 6,
                    BackColor = VisualPadrao.Azul,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                salvar.FlatAppearance.BorderSize = 0;
                Button cancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(112, 130),
                    Size = new Size(82, 32),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White
                };
                cancelar.Click += delegate { dialogo.Close(); };
                salvar.Click += delegate
                {
                    try
                    {
                        frequenciaServico.RemarcarAula(aula.IdTurma, aula.DataHora.Date, data.Value.Date);
                        dialogo.DialogResult = DialogResult.OK;
                        dialogo.Close();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(dialogo, erro.Message, "Remarcar aula", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                dialogo.Controls.AddRange(new Control[] { titulo, legenda, data, cancelar, salvar });
                EstiloPopup.Aplicar(dialogo);
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    AtualizarIndicadoresDashboard();
            }
        }

        // Retorna a página correspondente ao botão de navegação, criando-a se necessário
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
            else if (botao == btnConfiguracoes) pagina = new ConfiguracoesForm(usuarioLogado);
            else return null;

            paginasCriadas.Add(botao, pagina);
            return pagina;
        }

        private void SelecionarPagina(Button botao) {
            paginaSelecionada = botao;
            foreach (Button item in BotoesNavegacao()) {
                // Atualiza a aparência do botão selecionado e dos demais botões
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

        // Altera a aparência do botão de navegação quando o mouse entra ou sai da área do botão
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
