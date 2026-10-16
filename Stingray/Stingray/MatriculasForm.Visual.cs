using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stingray
{
    /// <summary>
    /// Parte VISUAL da tela de Matrículas.
    /// É a mesma classe MatriculasForm (partial), só que em outro arquivo, para não misturar
    /// com o seu MatriculasForm.cs e com o Designer. Tudo é construído dentro do "painelConteudo".
    /// Ordem da tela (de cima para baixo):
    ///   1) Card "Nova matrícula"
    ///   2) Card de filtros
    ///   3) Grade com a lista
    ///   4) Rodapé com resumo + paginação
    /// </summary>
    partial class MatriculasForm
    {
        // ====== Paleta (mesma do menu e do título que vocês já têm) ======
        private static readonly Color CorTexto = ColorTranslator.FromHtml("#111827");
        private static readonly Color CorTextoSuave = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color CorPrimaria = ColorTranslator.FromHtml("#1D4ED8");
        private static readonly Color CorPrimariaClara = ColorTranslator.FromHtml("#E8EFFD");
        private static readonly Color CorBorda = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color CorCabecalhoGrade = ColorTranslator.FromHtml("#F3F4F6");

        // ====== Controles que a lógica vai usar depois ======
        // Card "Nova matrícula"
        private ComboBox cboAluno;
        private ComboBox cboTurma;
        private DateTimePicker dtpInicio;
        private ComboBox cboPlano;
        private TextBox txtValor;
        private ComboBox cboPagamento;
        private Button btnConfirmar;

        // Filtros
        private TextBox txtBusca;
        private ComboBox cboFiltroTurma;
        private ComboBox cboFiltroProfessor;
        private ComboBox cboFiltroStatus;
        private Button btnPesquisar;
        private Button btnLimpar;

        // Grade e rodapé
        private DataGridView dgvMatriculas;
        private Label lblResumo;

        private void ConstruirTela()
        {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(20, 16, 20, 12),
                BackColor = Color.White
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // nova matrícula
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // filtros
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // grade (usa o que sobrar)
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // rodapé

            raiz.Controls.Add(CriarCardNovaMatricula(), 0, 0);
            raiz.Controls.Add(CriarCardFiltros(), 0, 1);

            dgvMatriculas = CriarGrade();
            raiz.Controls.Add(dgvMatriculas, 0, 2);
            raiz.Controls.Add(CriarRodape(), 0, 3);

            painelConteudo.Controls.Add(raiz);

            CarregarDadosDeExemplo();
        }

        // ---------------------------------------------------------------------
        //  CARD: NOVA MATRÍCULA
        // ---------------------------------------------------------------------
        private Control CriarCardNovaMatricula()
        {
            var card = CriarCard(3);
            for (int i = 0; i < 3; i++)
                card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            var lblCard = new Label
            {
                Text = "Nova matrícula",
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = CorTexto,
                Margin = new Padding(0, 0, 0, 10)
            };
            card.Controls.Add(lblCard, 0, 0);
            card.SetColumnSpan(lblCard, 3);

            cboAluno = CriarCombo("Selecionar aluno...");
            cboTurma = CriarCombo("Selecionar turma...");
            dtpInicio = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };
            cboPlano = CriarCombo(null, "Mensal", "Trimestral");
            txtValor = new TextBox();
            DefinirPlaceholder(txtValor, "R$ 0,00");
            cboPagamento = CriarCombo(null, "Pix", "Cartão de crédito", "Cartão de débito", "Dinheiro", "Boleto");

            // Linhas 1-2: Aluno | Turma | Data de início
            AdicionarCampo(card, "Aluno", cboAluno, 0, 1);
            AdicionarCampo(card, "Turma", cboTurma, 1, 1);
            AdicionarCampo(card, "Data de início", dtpInicio, 2, 1);

            // Linhas 3-4: Plano | Valor | Forma de pagamento
            AdicionarCampo(card, "Plano", cboPlano, 0, 3);
            AdicionarCampo(card, "Valor", txtValor, 1, 3);
            AdicionarCampo(card, "Forma de pagamento", cboPagamento, 2, 3);

            // Linha 5: botão
            btnConfirmar = CriarBotao("Confirmar matrícula", 160, CorPrimaria, Color.White, CorPrimaria);
            btnConfirmar.Anchor = AnchorStyles.Left;
            btnConfirmar.Margin = new Padding(0, 0, 0, 4);
            card.Controls.Add(btnConfirmar, 0, 5);

            // TODO (próxima etapa): btnConfirmar.Click += ...
            return card;
        }

        // ---------------------------------------------------------------------
        //  CARD: FILTROS
        // ---------------------------------------------------------------------
        private Control CriarCardFiltros()
        {
            var card = CriarCard(6);
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            card.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            txtBusca = new TextBox();
            DefinirPlaceholder(txtBusca, "Digite o nome ou CPF do aluno...");
            cboFiltroTurma = CriarCombo(null, "Todas", "Hidroginástica", "Natação Iniciante", "Natação Intermediário",
                                        "Natação Avançado", "Natação Adulto", "Natação Master", "Natação Competição");
            cboFiltroProfessor = CriarCombo(null, "Todos", "Lucas Almeida", "Ana Paula Costa", "Gabriel Silva",
                                            "Felipe Rocha", "Mariana Oliveira", "Juliana Lima", "Rafael Santos");
            cboFiltroStatus = CriarCombo(null, "Todos", "Ativa", "Pausada", "Cancelada");

            AdicionarCampo(card, "Pesquisar (Aluno)", txtBusca, 0, 0);
            AdicionarCampo(card, "Turma", cboFiltroTurma, 1, 0);
            AdicionarCampo(card, "Professor", cboFiltroProfessor, 2, 0);
            AdicionarCampo(card, "Status", cboFiltroStatus, 3, 0);

            btnPesquisar = CriarBotao("Pesquisar", 100, CorPrimariaClara, CorPrimaria, CorPrimaria);
            btnPesquisar.Margin = new Padding(0, 0, 8, 12);
            btnLimpar = CriarBotao("Limpar", 90, Color.White, CorTexto, CorBorda);
            btnLimpar.Margin = new Padding(0, 0, 0, 12);
            card.Controls.Add(btnPesquisar, 4, 1);
            card.Controls.Add(btnLimpar, 5, 1);

            // TODO (próxima etapa): eventos de Pesquisar e Limpar
            return card;
        }

        // ---------------------------------------------------------------------
        //  GRADE (DataGridView)
        // ---------------------------------------------------------------------
        private DataGridView CriarGrade()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = CorBorda,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 38,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Margin = new Padding(0)
            };
            g.RowTemplate.Height = 40;

            // Cabeçalho
            g.ColumnHeadersDefaultCellStyle.BackColor = CorCabecalhoGrade;
            g.ColumnHeadersDefaultCellStyle.ForeColor = CorTexto;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = CorCabecalhoGrade;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = CorTexto;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            // Células
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = CorTexto;
            g.DefaultCellStyle.SelectionBackColor = CorPrimariaClara;
            g.DefaultCellStyle.SelectionForeColor = CorTexto;
            g.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            // Colunas de texto (modalidade, início das aulas e término previsto ficaram de fora, como no design)
            g.Columns.Add("colData", "Data da Matrícula");
            g.Columns.Add("colAluno", "Aluno");
            g.Columns.Add("colTurma", "Turma");
            g.Columns.Add("colProfessor", "Professor");
            g.Columns.Add("colPlano", "Plano");
            g.Columns.Add("colStatus", "Status");

            // Colunas de ações (ícones da fonte "Segoe MDL2 Assets", que já vem no Windows 10/11)
            g.Columns.Add(CriarColunaIcone("colEditar", "Ações", "\uE70F",
                                           CorPrimaria, CorPrimariaClara));
            g.Columns.Add(CriarColunaIcone("colExcluir", "", "\uE74D",
                                           ColorTranslator.FromHtml("#D92D20"),
                                           ColorTranslator.FromHtml("#FEECEB")));

            g.CellFormatting += Grade_CellFormatting;
            return g;
        }

        private DataGridViewButtonColumn CriarColunaIcone(string nome, string cabecalho, string glifo,
                                                          Color corIcone, Color corFundo)
        {
            var col = new DataGridViewButtonColumn
            {
                Name = nome,
                HeaderText = cabecalho,
                Text = glifo,
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 50,
                Resizable = DataGridViewTriState.False
            };
            col.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe MDL2 Assets", 10F),
                ForeColor = corIcone,
                BackColor = corFundo,
                SelectionForeColor = corIcone,
                SelectionBackColor = corFundo,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(4, 6, 4, 6)
            };
            return col;
        }

        // Pinta a célula de Status conforme o valor (Ativa = verde, Pausada = laranja, etc.)
        private void Grade_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMatriculas.Columns[e.ColumnIndex].Name != "colStatus")
                return;

            Color fundo, texto;
            switch (Convert.ToString(e.Value))
            {
                case "Ativa":
                    fundo = ColorTranslator.FromHtml("#E6F6EC");
                    texto = ColorTranslator.FromHtml("#1E8E4E");
                    break;
                case "Pausada":
                    fundo = ColorTranslator.FromHtml("#FFF3D6");
                    texto = ColorTranslator.FromHtml("#B7791F");
                    break;
                default: // Cancelada
                    fundo = ColorTranslator.FromHtml("#FEECEB");
                    texto = ColorTranslator.FromHtml("#D92D20");
                    break;
            }

            e.CellStyle.BackColor = fundo;
            e.CellStyle.ForeColor = texto;
            e.CellStyle.SelectionBackColor = fundo;
            e.CellStyle.SelectionForeColor = texto;
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        }

        // ---------------------------------------------------------------------
        //  RODAPÉ (resumo + paginação)
        // ---------------------------------------------------------------------
        private Control CriarRodape()
        {
            var rodape = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                BackColor = Color.White,
                Padding = new Padding(0, 8, 0, 0),
                Margin = new Padding(0)
            };
            rodape.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            rodape.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            lblResumo = new Label
            {
                Text = "Mostrando 1 a 9 de 42 matrículas",
                AutoSize = true,
                ForeColor = CorTextoSuave,
                Anchor = AnchorStyles.Left
            };
            rodape.Controls.Add(lblResumo, 0, 0);

            var paginas = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };
            foreach (string t in new[] { "«", "‹", "1", "2", "3", "...", "5", "›", "»" })
            {
                bool ativo = t == "1";
                var b = CriarBotao(t, 32, ativo ? CorPrimaria : Color.White,
                                   ativo ? Color.White : CorTexto, ativo ? CorPrimaria : CorBorda);
                b.Height = 30;
                b.Margin = new Padding(2, 0, 2, 0);
                paginas.Controls.Add(b);
            }
            rodape.Controls.Add(paginas, 1, 0);

            return rodape;
        }

        // =====================================================================
        //  MÉTODOS AUXILIARES (para não repetir código)
        // =====================================================================

        // Card com borda, usado em "Nova matrícula" e "Filtros"
        private TableLayoutPanel CriarCard(int colunas)
        {
            return new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = colunas,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16, 12, 16, 4),
                Margin = new Padding(0, 0, 0, 12)
            };
        }

        // Coloca "label em cima + controle embaixo" na posição pedida
        private void AdicionarCampo(TableLayoutPanel card, string texto, Control controle, int coluna, int linhaDoLabel)
        {
            var lbl = new Label
            {
                Text = texto,
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = CorTexto,
                Margin = new Padding(0, 0, 0, 4)
            };
            controle.Font = new Font("Segoe UI", 10F);
            controle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            controle.Margin = new Padding(0, 0, 16, 10);

            card.Controls.Add(lbl, coluna, linhaDoLabel);
            card.Controls.Add(controle, coluna, linhaDoLabel + 1);
        }

        // ComboBox somente de seleção. "textoInicial" (se vier) vira o 1º item, tipo um placeholder.
        private ComboBox CriarCombo(string textoInicial, params string[] itens)
        {
            var cbo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            if (textoInicial != null) cbo.Items.Add(textoInicial);
            cbo.Items.AddRange(itens);
            cbo.SelectedIndex = 0;
            return cbo;
        }

        private Button CriarBotao(string texto, int largura, Color fundo, Color corTexto, Color borda)
        {
            var b = new Button
            {
                Text = texto,
                Size = new Size(largura, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = fundo,
                ForeColor = corTexto,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderColor = borda;
            b.FlatAppearance.BorderSize = 1;
            return b;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private static void DefinirPlaceholder(TextBox txt, string texto)
        {
            if (txt.IsHandleCreated)
                SendMessage(txt.Handle, EM_SETCUEBANNER, (IntPtr)1, texto);
            else
                txt.HandleCreated += (s, e) => SendMessage(txt.Handle, EM_SETCUEBANNER, (IntPtr)1, texto);
        }

        // =====================================================================
        //  DADOS FALSOS, só para ver a tela "viva". Serão trocados pelo banco.
        // =====================================================================
        private void CarregarDadosDeExemplo()
        {
            cboAluno.Items.AddRange(new object[] { "Maria Silva", "João Pedro", "Ana Clara", "Pedro Henrique", "Laura Souza" });
            cboTurma.Items.AddRange(new object[] { "Hidroginástica", "Natação Iniciante", "Natação Intermediário", "Natação Avançado" });

            dgvMatriculas.Rows.Add("20/05/2024", "Maria Silva", "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");
            dgvMatriculas.Rows.Add("18/05/2024", "João Pedro", "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");
            dgvMatriculas.Rows.Add("22/05/2024", "Ana Clara", "Natação Intermediário", "Gabriel Silva", "Trimestral", "Ativa");
            dgvMatriculas.Rows.Add("21/05/2024", "Pedro Henrique", "Natação Avançado", "Felipe Rocha", "Mensal", "Ativa");
            dgvMatriculas.Rows.Add("17/05/2024", "Laura Souza", "Natação Adulto", "Mariana Oliveira", "Mensal", "Ativa");
            dgvMatriculas.Rows.Add("19/05/2024", "Gabriel Lima", "Natação Master", "Juliana Lima", "Trimestral", "Pausada");
            dgvMatriculas.Rows.Add("16/05/2024", "Beatriz Oliveira", "Natação Competição", "Rafael Santos", "Mensal", "Ativa");
            dgvMatriculas.Rows.Add("15/05/2024", "Miguel Ferreira", "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");
            dgvMatriculas.Rows.Add("14/05/2024", "Sofia Martins", "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");

            dgvMatriculas.ClearSelection();
        }
    }
}
