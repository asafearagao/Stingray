using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stingray
{
    /// <summary>
    /// Parte VISUAL da tela de Aluno (antes "Matrículas").
    /// É a mesma classe MatriculasForm (partial), só que em outro arquivo, para não misturar
    /// com o seu MatriculasForm.cs e com o Designer. Tudo é construído dentro do "painelConteudo".
    /// Ordem da tela (de cima para baixo):
    ///   1) Card "Cadastrar novo aluno" (baseado na Ficha de Matrícula em papel)
    ///   2) Card de filtros
    ///   3) Grade com a lista (com a lupa que abre a ficha do aluno)
    ///   4) Rodapé com resumo + paginação
    ///
    /// No FINAL deste arquivo estão as duas classes da ficha (FichaAluno e FichaAlunoJanela),
    /// para você só precisar copiar e colar este arquivo inteiro.
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
        // Card "Cadastrar novo aluno" (campos da Ficha de Matrícula)
        private TextBox txtNomeAluno;
        private DateTimePicker dtpNascimento;
        private MaskedTextBox mtbTelefone;
        private TextBox txtEndereco;
        private MaskedTextBox mtbCep;
        private TextBox txtCidade;
        private TextBox txtResponsavel;
        private MaskedTextBox mtbCpf;
        private TextBox txtRg;
        private TextBox txtEmail;
        private Button btnCadastrar;

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
            // Título da tela (antes era "Matrículas", agora é "Aluno")
            lblTitulo.Text = "Aluno";

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(20, 16, 20, 12),
                BackColor = Color.White
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // cadastro do aluno
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // filtros
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // grade (usa o que sobrar)
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // rodapé

            raiz.Controls.Add(CriarCardCadastroAluno(), 0, 0);
            raiz.Controls.Add(CriarCardFiltros(), 0, 1);

            dgvMatriculas = CriarGrade();
            raiz.Controls.Add(dgvMatriculas, 0, 2);
            raiz.Controls.Add(CriarRodape(), 0, 3);

            painelConteudo.Controls.Add(raiz);

            CarregarDadosDeExemplo();
        }

        // ---------------------------------------------------------------------
        //  CARD: CADASTRAR NOVO ALUNO (baseado na Ficha de Matrícula)
        // ---------------------------------------------------------------------
        private Control CriarCardCadastroAluno()
        {
            // 4 colunas de tamanho igual; campos maiores (nome, endereço...) ocupam 2 colunas
            var card = CriarCard(4);
            for (int i = 0; i < 4; i++)
                card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            var lblCard = new Label
            {
                Text = "Cadastrar novo aluno",
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = CorTexto,
                Margin = new Padding(0, 0, 0, 10)
            };
            card.Controls.Add(lblCard, 0, 0);
            card.SetColumnSpan(lblCard, 4);

            // ---- Criação dos controles ----
            txtNomeAluno = new TextBox();
            DefinirPlaceholder(txtNomeAluno, "Nome completo do aluno");

            dtpNascimento = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MaxDate = DateTime.Today
            };

            mtbTelefone = CriarMascara(@"(00) 00000-0000");

            txtEndereco = new TextBox();
            DefinirPlaceholder(txtEndereco, "Rua, número e bairro");

            mtbCep = CriarMascara(@"00000-000");

            txtCidade = new TextBox();
            DefinirPlaceholder(txtCidade, "Cidade");

            txtResponsavel = new TextBox();
            DefinirPlaceholder(txtResponsavel, "Nome do responsável");

            // O "\." é necessário: sem a barra, o ponto viraria vírgula no Windows em português
            mtbCpf = CriarMascara(@"000\.000\.000-00");

            txtRg = new TextBox();
            DefinirPlaceholder(txtRg, "RG");

            txtEmail = new TextBox();
            DefinirPlaceholder(txtEmail, "exemplo@email.com");

            // ---- Posição na tela (mesma ordem da ficha em papel) ----
            // Linhas 1-2: Nome do aluno (2 colunas) | Data de nascimento | Telefone
            AdicionarCampo(card, "Nome do aluno", txtNomeAluno, 0, 1, 2);
            AdicionarCampo(card, "Data de nascimento", dtpNascimento, 2, 1);
            AdicionarCampo(card, "Telefone", mtbTelefone, 3, 1);

            // Linhas 3-4: Endereço (2 colunas) | CEP | Cidade
            AdicionarCampo(card, "Endereço", txtEndereco, 0, 3, 2);
            AdicionarCampo(card, "CEP", mtbCep, 2, 3);
            AdicionarCampo(card, "Cidade", txtCidade, 3, 3);

            // Linhas 5-6: Nome do responsável (2 colunas) | CPF | RG
            AdicionarCampo(card, "Nome do responsável", txtResponsavel, 0, 5, 2);
            AdicionarCampo(card, "CPF", mtbCpf, 2, 5);
            AdicionarCampo(card, "RG", txtRg, 3, 5);

            // Linhas 7-8: E-mail (2 colunas)
            AdicionarCampo(card, "E-mail", txtEmail, 0, 7, 2);

            // Linha 9: botão
            btnCadastrar = CriarBotao("Cadastrar aluno", 160, CorPrimaria, Color.White, CorPrimaria);
            btnCadastrar.Anchor = AnchorStyles.Left;
            btnCadastrar.Margin = new Padding(0, 4, 0, 4);
            card.Controls.Add(btnCadastrar, 0, 9);

            // TODO (próxima etapa): btnCadastrar.Click += ...
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
            // \uE721 = lupa (ver ficha) | \uE70F = lápis (editar) | \uE74D = lixeira (excluir)
            g.Columns.Add(CriarColunaIcone("colVer", "Ações", "\uE721",
                                           CorTexto, CorCabecalhoGrade));
            g.Columns.Add(CriarColunaIcone("colEditar", "", "\uE70F",
                                           CorPrimaria, CorPrimariaClara));
            g.Columns.Add(CriarColunaIcone("colExcluir", "", "\uE74D",
                                           ColorTranslator.FromHtml("#D92D20"),
                                           ColorTranslator.FromHtml("#FEECEB")));

            g.CellFormatting += Grade_CellFormatting;
            g.CellContentClick += Grade_CellContentClick;
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

        // Clique nos botões da grade. Por enquanto só a lupa faz algo: abre a ficha do aluno da linha clicada.
        private void Grade_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string coluna = dgvMatriculas.Columns[e.ColumnIndex].Name;

            if (coluna == "colVer")
            {
                var aluno = dgvMatriculas.Rows[e.RowIndex].Tag as FichaAluno;
                if (aluno == null) return;

                using (var ficha = new FichaAlunoJanela(aluno))
                {
                    ficha.ShowDialog(this);
                }
            }

            // TODO (próxima etapa): colEditar e colExcluir
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

        // Card com borda, usado em "Cadastrar novo aluno" e "Filtros"
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

        // Coloca "label em cima + controle embaixo" na posição pedida.
        // "colunasOcupadas" permite um campo ocupar mais de uma coluna (ex.: Nome do aluno).
        private void AdicionarCampo(TableLayoutPanel card, string texto, Control controle,
                                    int coluna, int linhaDoLabel, int colunasOcupadas = 1)
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

            if (colunasOcupadas > 1)
            {
                card.SetColumnSpan(lbl, colunasOcupadas);
                card.SetColumnSpan(controle, colunasOcupadas);
            }
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

        // Campo com máscara (CPF, CEP, telefone): só aceita os números e já coloca os traços/parênteses
        private MaskedTextBox CriarMascara(string mascara)
        {
            return new MaskedTextBox
            {
                Mask = mascara,
                PromptChar = '_'
            };
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
        //  Cada linha da grade guarda a sua FichaAluno em "Tag"; a lupa lê dali.
        // =====================================================================
        private void CarregarDadosDeExemplo()
        {
            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Maria Silva", new DateTime(1985, 3, 12), "(19) 99876-5432",
                    "Rua das Palmeiras, 120 - Cambuí", "13025-000", "Campinas", "",
                    "123.456.789-09", "12.345.678-9", "maria.silva@email.com", new DateTime(2024, 5, 20)),
                "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("João Pedro", new DateTime(2016, 7, 4), "(19) 98765-4321",
                    "Av. Brasil, 450 - Guanabara", "13073-001", "Campinas", "Carla Pereira",
                    "234.567.890-10", "23.456.789-0", "carla.pereira@email.com", new DateTime(2024, 5, 18)),
                "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Ana Clara", new DateTime(2012, 11, 25), "(19) 97654-3210",
                    "Rua Barão de Jaguara, 85 - Centro", "13015-001", "Campinas", "Roberto Santos",
                    "345.678.901-21", "34.567.890-1", "roberto.santos@email.com", new DateTime(2024, 5, 22)),
                "Natação Intermediário", "Gabriel Silva", "Trimestral", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Pedro Henrique", new DateTime(2009, 1, 30), "(19) 96543-2109",
                    "Rua Sete de Setembro, 301 - Taquaral", "13076-000", "Campinas", "Fernanda Alves",
                    "456.789.012-32", "45.678.901-2", "fernanda.alves@email.com", new DateTime(2024, 5, 21)),
                "Natação Avançado", "Felipe Rocha", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Laura Souza", new DateTime(1992, 9, 8), "(19) 95432-1098",
                    "Rua Dr. Quirino, 77 - Centro", "13015-080", "Campinas", "",
                    "567.890.123-43", "56.789.012-3", "laura.souza@email.com", new DateTime(2024, 5, 17)),
                "Natação Adulto", "Mariana Oliveira", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Gabriel Lima", new DateTime(1978, 6, 15), "(19) 94321-0987",
                    "Av. Norte-Sul, 1500 - Vila Industrial", "13035-000", "Campinas", "",
                    "678.901.234-54", "67.890.123-4", "gabriel.lima@email.com", new DateTime(2024, 5, 19)),
                "Natação Master", "Juliana Lima", "Trimestral", "Pausada");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Beatriz Oliveira", new DateTime(2005, 12, 2), "(19) 93210-9876",
                    "Rua Tiradentes, 640 - Bonfim", "13070-010", "Campinas", "Marcos Oliveira",
                    "789.012.345-65", "78.901.234-5", "marcos.oliveira@email.com", new DateTime(2024, 5, 16)),
                "Natação Competição", "Rafael Santos", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Miguel Ferreira", new DateTime(2017, 4, 19), "(19) 92109-8765",
                    "Rua Paraná, 233 - Jardim Chapadão", "13070-210", "Campinas", "Patrícia Ferreira",
                    "890.123.456-76", "89.012.345-6", "patricia.ferreira@email.com", new DateTime(2024, 5, 15)),
                "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");

            AdicionarLinhaExemplo(
                CriarAlunoExemplo("Sofia Martins", new DateTime(1999, 8, 27), "(19) 91098-7654",
                    "Rua Sampainho, 18 - Nova Campinas", "13092-120", "Campinas", "",
                    "901.234.567-87", "90.123.456-7", "sofia.martins@email.com", new DateTime(2024, 5, 14)),
                "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");

            dgvMatriculas.ClearSelection();
        }

        // Adiciona uma linha na grade e guarda a ficha completa do aluno dentro dela (Tag)
        private void AdicionarLinhaExemplo(FichaAluno aluno, string turma, string professor, string plano, string status)
        {
            int indice = dgvMatriculas.Rows.Add(aluno.DataCadastro.ToString("dd/MM/yyyy"), aluno.Nome,
                                                turma, professor, plano, status);
            dgvMatriculas.Rows[indice].Tag = aluno;
        }

        private FichaAluno CriarAlunoExemplo(string nome, DateTime nascimento, string telefone, string endereco,
                                             string cep, string cidade, string responsavel, string cpf,
                                             string rg, string email, DateTime cadastro)
        {
            return new FichaAluno
            {
                Nome = nome,
                DataNascimento = nascimento,
                Telefone = telefone,
                Endereco = endereco,
                Cep = cep,
                Cidade = cidade,
                Responsavel = responsavel,
                Cpf = cpf,
                Rg = rg,
                Email = email,
                DataCadastro = cadastro
            };
        }
    }

    // =========================================================================
    //  FICHA DO ALUNO (janela que abre ao clicar na lupa)
    // =========================================================================

    /// <summary>
    /// Guarda os dados de um aluno (os mesmos campos da Ficha de Matrícula em papel).
    /// Por enquanto é preenchida com dados de exemplo; depois vai receber os dados do banco.
    /// </summary>
    public class FichaAluno
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Responsavel { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    /// <summary>
    /// Janela que mostra os dados cadastrados de UM aluno, no formato da Ficha de Matrícula em papel.
    /// É montada por código (não tem Designer): não precisa abrir o "Exibir Designer" dela.
    /// </summary>
    public class FichaAlunoJanela : Form
    {
        private static readonly Color CorTexto = ColorTranslator.FromHtml("#111827");
        private static readonly Color CorTextoSuave = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color CorPrimaria = ColorTranslator.FromHtml("#1D4ED8");
        private static readonly Color CorLinha = ColorTranslator.FromHtml("#D1D5DB");

        private static readonly CultureInfo PtBr = new CultureInfo("pt-BR");

        private readonly FichaAluno aluno;
        private Button btnFechar;

        public FichaAlunoJanela(FichaAluno aluno)
        {
            this.aluno = aluno;

            Text = "Ficha do aluno";
            ClientSize = new Size(760, 660);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);

            ConstruirFicha();

            // Enter e Esc fecham a janela
            AcceptButton = btnFechar;
            CancelButton = btnFechar;
        }

        private void ConstruirFicha()
        {
            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                Padding = new Padding(36, 28, 36, 20),
                BackColor = Color.White,
                AutoScroll = true
            };
            for (int i = 0; i < 4; i++)
                raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            int linha = 0;

            // ---- Título (igual ao papel) ----
            var titulo = new Label
            {
                Text = "FICHA DE MATRÍCULA",
                AutoSize = false,
                Dock = DockStyle.Fill,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = CorTexto,
                Margin = new Padding(0, 0, 0, 18)
            };
            raiz.Controls.Add(titulo, 0, linha);
            raiz.SetColumnSpan(titulo, 4);
            linha++;

            // ---- Campos (mesma ordem da ficha) ----
            AdicionarCampo(raiz, "Nome do aluno", aluno.Nome, 0, linha++, 4);

            AdicionarCampo(raiz, "Data de nascimento", aluno.DataNascimento.ToString("dd/MM/yyyy"), 0, linha, 2);
            AdicionarCampo(raiz, "Telefone", aluno.Telefone, 2, linha++, 2);

            AdicionarCampo(raiz, "Endereço", aluno.Endereco, 0, linha, 3);
            AdicionarCampo(raiz, "CEP", aluno.Cep, 3, linha++, 1);

            AdicionarCampo(raiz, "Cidade", aluno.Cidade, 0, linha, 2);
            AdicionarCampo(raiz, "Nome do responsável", aluno.Responsavel, 2, linha++, 2);

            AdicionarCampo(raiz, "CPF", aluno.Cpf, 0, linha, 1);
            AdicionarCampo(raiz, "RG", aluno.Rg, 1, linha, 1);
            AdicionarCampo(raiz, "E-mail", aluno.Email, 2, linha++, 2);

            // ---- Local e data (como no fim do papel) ----
            var lblData = new Label
            {
                Text = "Campinas, " + aluno.DataCadastro.ToString("d 'de' MMMM 'de' yyyy", PtBr) + ".",
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = CorTextoSuave,
                Margin = new Padding(0, 18, 0, 18)
            };
            raiz.Controls.Add(lblData, 0, linha);
            raiz.SetColumnSpan(lblData, 4);
            linha++;

            // ---- Botão Fechar ----
            btnFechar = new Button
            {
                Text = "Fechar",
                Size = new Size(110, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = CorPrimaria,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right
            };
            btnFechar.FlatAppearance.BorderColor = CorPrimaria;
            raiz.Controls.Add(btnFechar, 0, linha);
            raiz.SetColumnSpan(btnFechar, 4);

            Controls.Add(raiz);
        }

        // Um campo da ficha: rótulo pequeno + valor + linha embaixo, como no papel
        private void AdicionarCampo(TableLayoutPanel tabela, string rotulo, string valor,
                                    int coluna, int linha, int colunasOcupadas)
        {
            var campo = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 18, 14),
                BackColor = Color.White
            };
            campo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            campo.Controls.Add(new Label
            {
                Text = rotulo.ToUpper(PtBr),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = CorTextoSuave,
                Margin = new Padding(0, 0, 0, 2)
            }, 0, 0);

            campo.Controls.Add(new Label
            {
                Text = string.IsNullOrWhiteSpace(valor) ? "—" : valor,
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                ForeColor = CorTexto,
                Margin = new Padding(0, 0, 0, 4)
            }, 0, 1);

            campo.Controls.Add(new Panel
            {
                Height = 1,
                BackColor = CorLinha,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0)
            }, 0, 2);

            tabela.Controls.Add(campo, coluna, linha);
            if (colunasOcupadas > 1)
                tabela.SetColumnSpan(campo, colunasOcupadas);
        }
    }
}