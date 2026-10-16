using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Stingray
{
    /// <summary>
    /// Parte VISUAL da tela de Dashboard.
    /// É a mesma classe DashboardForm (partial), só que em outro arquivo, para não misturar
    /// com o seu DashboardForm.cs e com o Designer. Tudo é construído dentro do "dashboardContentPanel".
    /// Ordem da tela (de cima para baixo):
    ///   1) Três cards de resumo: Alunos, Professores e Turmas (conteúdo centralizado)
    ///   2) Lado a lado: "Próximas aulas" e "Pagamentos em aberto"
    /// Cada área fica dentro de uma moldura (quadrado com borda) sobre um fundo claro.
    /// Ficaram de fora, como combinado: boas-vindas, data, usuário, card Receita do mês,
    /// etiquetas de piscina, coluna Status e "Ações rápidas".
    /// Todos os nomes começam com "Dash" para não esbarrar com nomes que já existam no projeto.
    /// </summary>
    partial class DashboardForm
    {
        // ====== Paleta (mesma do restante do sistema) ======
        private static readonly Color DashCorTexto = ColorTranslator.FromHtml("#111827");
        private static readonly Color DashCorTextoSuave = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color DashCorPrimaria = ColorTranslator.FromHtml("#1D4ED8");
        private static readonly Color DashCorTitulo = ColorTranslator.FromHtml("#1E3A8A");
        private static readonly Color DashCorBorda = ColorTranslator.FromHtml("#E5E7EB");     // linhas finas internas
        private static readonly Color DashCorFundo = ColorTranslator.FromHtml("#F3F6FB");     // fundo atrás dos quadrados
        private static readonly Color DashCorMoldura = ColorTranslator.FromHtml("#B8C6DE");   // borda dos quadrados

        // =====================================================================
        //  DADOS DE EXEMPLO (os mesmos da tela de Aluno). Serão trocados pelo banco.
        //  Colunas: [0] data | [1] aluno | [2] turma | [3] professor | [4] plano | [5] status
        //  Os números dos cards são CALCULADOS a partir desta lista.
        // =====================================================================
        private static readonly string[][] DashMatriculasExemplo =
        {
            new[] { "20/05/2024", "Maria Silva",      "Hidroginástica",        "Lucas Almeida",    "Mensal",     "Ativa" },
            new[] { "18/05/2024", "João Pedro",       "Natação Iniciante",     "Ana Paula Costa",  "Mensal",     "Ativa" },
            new[] { "22/05/2024", "Ana Clara",        "Natação Intermediário", "Gabriel Silva",    "Trimestral", "Ativa" },
            new[] { "21/05/2024", "Pedro Henrique",   "Natação Avançado",      "Felipe Rocha",     "Mensal",     "Ativa" },
            new[] { "17/05/2024", "Laura Souza",      "Natação Adulto",        "Mariana Oliveira", "Mensal",     "Ativa" },
            new[] { "19/05/2024", "Gabriel Lima",     "Natação Master",        "Juliana Lima",     "Trimestral", "Pausada" },
            new[] { "16/05/2024", "Beatriz Oliveira", "Natação Competição",    "Rafael Santos",    "Mensal",     "Ativa" },
            new[] { "15/05/2024", "Miguel Ferreira",  "Natação Iniciante",     "Ana Paula Costa",  "Mensal",     "Ativa" },
            new[] { "14/05/2024", "Sofia Martins",    "Hidroginástica",        "Lucas Almeida",    "Mensal",     "Ativa" }
        };

        // Conta quantos valores DIFERENTES existem em uma coluna (ex.: quantos alunos, professores, turmas)
        private static int DashContarDistintos(int coluna)
        {
            return DashMatriculasExemplo.Select(m => m[coluna]).Distinct().Count();
        }

        // Guarda o painel principal para o ajuste de tamanho (ver DashAjustarAoEspacoVisivel)
        private TableLayoutPanel dashRaiz;

        // Chame este método no construtor do DashboardForm, logo depois de InitializeComponent();
        private void ConstruirDashboard()
        {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(20, 16, 24, 20),
                BackColor = DashCorFundo
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // cards de resumo
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // listas (usa o que sobrar)

            raiz.Controls.Add(DashCriarLinhaResumo(), 0, 0);
            raiz.Controls.Add(DashCriarLinhaListas(), 0, 1);

            dashboardContentPanel.Controls.Add(raiz);
            dashRaiz = raiz;

            // Se a janela for maior que a tela, o painel é ajustado para caber no que dá para ver.
            // Refaz o ajuste sempre que a janela abrir, mudar de tamanho ou for movida.
            Shown += (s, e) => DashAjustarAoEspacoVisivel();
            SizeChanged += (s, e) => DashAjustarAoEspacoVisivel();
            LocationChanged += (s, e) => DashAjustarAoEspacoVisivel();
            dashboardContentPanel.SizeChanged += (s, e) => DashAjustarAoEspacoVisivel();
        }

        // ---------------------------------------------------------------------
        //  AJUSTE AO ESPAÇO VISÍVEL
        //  Mede a área do painel que realmente aparece na tela (descontando o que passa do
        //  fim da tela ou da janela) e faz o painel principal do Dashboard caber nela.
        //  Assim a moldura da direita fecha ANTES do fim da tela, e nada fica "comido".
        // ---------------------------------------------------------------------
        private void DashAjustarAoEspacoVisivel()
        {
            if (dashRaiz == null || !IsHandleCreated || WindowState == FormWindowState.Minimized)
                return;

            // Começa pela área útil da tela (sem a barra de tarefas) e vai recortando
            // com o espaço de cada painel "pai", até chegar no painel do Dashboard.
            Rectangle visivel = Screen.FromControl(this).WorkingArea;
            for (Control c = dashboardContentPanel; c != null; c = c.Parent)
                visivel = Rectangle.Intersect(visivel, c.RectangleToScreen(c.ClientRectangle));

            if (visivel.Width <= 0 || visivel.Height <= 0)
                return;

            dashRaiz.Dock = DockStyle.None;
            dashRaiz.Location = dashboardContentPanel.PointToClient(visivel.Location);
            dashRaiz.Size = visivel.Size;
        }

        // ---------------------------------------------------------------------
        //  LINHA 1: CARDS DE RESUMO
        // ---------------------------------------------------------------------
        private Control DashCriarLinhaResumo()
        {
            var linha = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = DashCorFundo,
                Margin = new Padding(0)
            };
            for (int i = 0; i < 3; i++)
                linha.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            // Números calculados a partir dos dados de exemplo (alunos, professores e turmas diferentes).
            // Ícones da fonte "Segoe MDL2 Assets" (já vem no Windows 10/11).
            // \uE7BE = chapéu de formatura | \uE716 = pessoas
            linha.Controls.Add(DashCriarCartaoResumo("\uE7BE", DashContarDistintos(1).ToString(), "Alunos", 14), 0, 0);
            linha.Controls.Add(DashCriarCartaoResumo("\uE716", DashContarDistintos(3).ToString(), "Professores", 14), 1, 0);
            linha.Controls.Add(DashCriarCartaoResumo("\uE716", DashContarDistintos(2).ToString(), "Turmas", 0), 2, 0);

            return linha;
        }

        private Control DashCriarCartaoResumo(string glifo, string numero, string rotulo, int margemDireita)
        {
            var cartao = DashCriarCartao(true, margemDireita);
            cartao.Padding = new Padding(20, 24, 20, 16);
            cartao.ColumnCount = 1;
            cartao.RowCount = 2;
            cartao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Grupo "ícone + número + nome". Com Anchor = None ele fica CENTRALIZADO no card.
            var grupo = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.White,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0)
            };

            var icone = new Label
            {
                Text = glifo,
                AutoSize = true,
                Font = new Font("Segoe MDL2 Assets", 30F),
                ForeColor = DashCorTitulo,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 0, 18, 0)
            };
            grupo.Controls.Add(icone, 0, 0);
            grupo.SetRowSpan(icone, 2);

            var lblNumero = new Label
            {
                Text = numero,
                AutoSize = true,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = DashCorTexto,
                Margin = new Padding(0)
            };
            grupo.Controls.Add(lblNumero, 1, 0);

            var lblRotulo = new Label
            {
                Text = rotulo,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = DashCorTextoSuave,
                Margin = new Padding(0)
            };
            grupo.Controls.Add(lblRotulo, 1, 1);

            cartao.Controls.Add(grupo, 0, 0);

            // "Ver detalhes" centralizado embaixo
            var link = DashCriarLink("Ver detalhes");
            link.Anchor = AnchorStyles.None;
            link.Margin = new Padding(0, 12, 0, 0);
            cartao.Controls.Add(link, 0, 1);

            // TODO (próxima etapa): link.LinkClicked += ... (abrir a tela correspondente)
            return cartao;
        }

        // ---------------------------------------------------------------------
        //  LINHA 2: PRÓXIMAS AULAS | PAGAMENTOS EM ABERTO
        // ---------------------------------------------------------------------
        private Control DashCriarLinhaListas()
        {
            var linha = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = DashCorFundo,
                Margin = new Padding(0)
            };
            // 1/3 + 2/3: as bordas ficam alinhadas com os três cards de resumo de cima
            linha.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            linha.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.67F));
            linha.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            linha.Controls.Add(DashCriarCartaoProximasAulas(), 0, 0);
            linha.Controls.Add(DashCriarCartaoPagamentos(), 1, 0);
            return linha;
        }

        // ---- Card "Próximas aulas" ----
        private Control DashCriarCartaoProximasAulas()
        {
            var cartao = DashCriarCartao(false, 14);
            cartao.ColumnCount = 2;
            cartao.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));   // horário
            cartao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));   // turma + professor

            int linha = 0;

            // Título + ícone de calendário
            var calendario = new Label
            {
                Text = "\uE787",
                AutoSize = true,
                Font = new Font("Segoe MDL2 Assets", 14F),
                ForeColor = DashCorTitulo,
                Margin = new Padding(0)
            };
            var cabecalho = DashCriarCabecalho("Próximas aulas", calendario);
            cartao.Controls.Add(cabecalho, 0, linha);
            cartao.SetColumnSpan(cabecalho, 2);
            linha++;
            DashAdicionarSeparador(cartao, linha++);

            // Turma e professor vêm das 4 primeiras linhas dos dados de exemplo; o horário é de exemplo
            string[] horarios = { "08:00", "09:00", "10:00", "14:00" };

            for (int i = 0; i < horarios.Length; i++)
            {
                string[] m = DashMatriculasExemplo[i];

                var hora = new Label
                {
                    Text = horarios[i],
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = DashCorPrimaria,
                    Anchor = AnchorStyles.Left,
                    Margin = new Padding(0)
                };
                cartao.Controls.Add(hora, 0, linha);
                cartao.Controls.Add(DashCriarTextoDuasLinhas(m[2], "Prof. " + m[3]), 1, linha);
                linha++;
                DashAdicionarSeparador(cartao, linha++);
            }

            var link = DashCriarLink("Ver todos os horários");
            link.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            link.Margin = new Padding(0, 4, 0, 0);
            cartao.Controls.Add(link, 0, linha);
            cartao.SetColumnSpan(link, 2);

            return cartao;
        }

        // ---- Card "Pagamentos em aberto" ----
        private Control DashCriarCartaoPagamentos()
        {
            var cartao = DashCriarCartao(false, 0);
            cartao.ColumnCount = 1;
            cartao.RowCount = 2;
            cartao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            cartao.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            cartao.Controls.Add(DashCriarCabecalho("Pagamentos em aberto", DashCriarLink("Ver todos")), 0, 0);
            cartao.Controls.Add(DashCriarGradePagamentos(), 0, 1);
            return cartao;
        }

        private DataGridView DashCriarGradePagamentos()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                MultiSelect = false,
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = DashCorBorda,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 34,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Margin = new Padding(0)
            };
            g.RowTemplate.Height = 36;

            // Cabeçalho (texto escuro e fundo branco, como no design)
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.ForeColor = DashCorTexto;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = DashCorTexto;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Células (sem destaque azul ao clicar: é só uma lista de consulta)
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = DashCorTexto;
            g.DefaultCellStyle.SelectionBackColor = Color.White;
            g.DefaultCellStyle.SelectionForeColor = DashCorTexto;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);

            g.Columns.Add("colAluno", "Aluno");
            g.Columns.Add("colVencimento", "Vencimento");
            g.Columns.Add("colValor", "Valor");

            // Os alunos vêm das 5 primeiras linhas dos dados de exemplo; vencimento e valor são de exemplo
            string[] vencimentos = { "10/05/2024", "12/05/2024", "15/05/2024", "18/05/2024", "20/05/2024" };
            for (int i = 0; i < vencimentos.Length; i++)
                g.Rows.Add(DashMatriculasExemplo[i][1], vencimentos[i], "R$ 180,00");

            g.ClearSelection();
            return g;
        }

        // =====================================================================
        //  MÉTODOS AUXILIARES (para não repetir código)
        // =====================================================================

        // Quadrado branco com MOLDURA (borda de 2 px) sobre o fundo claro.
        // "autoTamanho" = true ajusta a altura ao conteúdo.
        private TableLayoutPanel DashCriarCartao(bool autoTamanho, int margemDireita)
        {
            var cartao = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = autoTamanho,
                BackColor = Color.White,
                Padding = new Padding(20, 16, 20, 14),
                Margin = new Padding(0, 0, margemDireita, 14)
            };
            cartao.Paint += (s, e) =>
            {
                using (var caneta = new Pen(DashCorMoldura, 2F))
                    e.Graphics.DrawRectangle(caneta, 1, 1, cartao.Width - 3, cartao.Height - 3);
            };
            cartao.Resize += (s, e) => cartao.Invalidate();
            return cartao;
        }

        // Título do cartão (à esquerda) + um controle à direita (ícone ou link)
        private Control DashCriarCabecalho(string titulo, Control direita)
        {
            var cab = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 6)
            };
            cab.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cab.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var lbl = new Label
            {
                Text = titulo,
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = DashCorTitulo,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0)
            };
            cab.Controls.Add(lbl, 0, 0);

            direita.Anchor = AnchorStyles.Right;
            cab.Controls.Add(direita, 1, 0);
            return cab;
        }

        // Duas linhas de texto uma sobre a outra (ex.: turma em cima, professor embaixo)
        private Control DashCriarTextoDuasLinhas(string principal, string secundario)
        {
            var painel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.White,
                Margin = new Padding(0)
            };
            painel.Controls.Add(new Label
            {
                Text = principal,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = DashCorTexto,
                Margin = new Padding(0)
            }, 0, 0);
            painel.Controls.Add(new Label
            {
                Text = secundario,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DashCorTextoSuave,
                Margin = new Padding(0)
            }, 0, 1);
            return painel;
        }

        // Linha fina horizontal ocupando as duas colunas do cartão
        private void DashAdicionarSeparador(TableLayoutPanel cartao, int linha)
        {
            var separador = new Panel
            {
                Height = 1,
                BackColor = DashCorBorda,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0, 6, 0, 6)
            };
            cartao.Controls.Add(separador, 0, linha);
            cartao.SetColumnSpan(separador, 2);
        }

        // Link azul pequeno ("Ver detalhes", "Ver todos"...)
        private LinkLabel DashCriarLink(string texto)
        {
            return new LinkLabel
            {
                Text = texto,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                LinkColor = DashCorPrimaria,
                ActiveLinkColor = DashCorTitulo,
                VisitedLinkColor = DashCorPrimaria,
                LinkBehavior = LinkBehavior.HoverUnderline,
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
        }
    }
}