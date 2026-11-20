
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Stingray.Repositorios;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Monta os relatórios e controla a seleção do período consultado
    public partial class RelatoriosForm : Form
    {
        private Label lblReceitaValor;
        private Label lblPresencaValor;
        private Label lblAlunosValor;
        private Label lblReceitaTotal;
        private Label lblReceitaRecebida;
        private Label lblReceitaPendente;
        private RevenueChartPanel graficoReceita;
        private AttendanceChartPanel graficoPresencaRelatorios;
        private ComboBox cmbPeriodoGrafico;
        private Label txtPeriodoRelatorio;
        private Control campoPeriodoRelatorio;
        private Panel popupPeriodoRelatorio;
        private DateTimePicker dataInicialRelatorio;
        private DateTimePicker dataFinalRelatorio;

        private readonly RelatorioRepositorio relatorioRepositorio =
            new RelatorioRepositorio();

        private readonly CultureInfo culturaBrasileira =
            CultureInfo.GetCultureInfo("pt-BR");

        private DateTime inicioPeriodoRelatorio =
            new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        private DateTime fimPeriodoRelatorio = DateTime.Today;

        // Dados armazenados para permitir a exportação do relatório
        private decimal receitaRecebidaAtual;
        private decimal receitaPendenteAtual;
        private int alunosAtivosAtual;
        private ResumoFrequenciaRelatorio frequenciaAtual;

        private string[] mesesReceitaAtual = new string[6];
        private decimal[] valoresReceitaAtual = new decimal[6];

        private bool dadosRelatoriosCarregados;

        // Construtor do relatório
        public RelatoriosForm()
        {
            InitializeComponent();

            campoPeriodoRelatorio = panelPeriodoRelatorio;
            panelPeriodoRelatorio.BorderStyle = BorderStyle.None;
            panelPeriodoRelatorio.Paint += PeriodoRelatorio_Paint;

            txtPeriodoRelatorio.Text =
                inicioPeriodoRelatorio.ToString("dd/MM/yyyy") +
                " - " +
                fimPeriodoRelatorio.ToString("dd/MM/yyyy");

            txtPeriodoRelatorio.ForeColor =
                VisualPadrao.TextoPrincipal;

            bool emDesignTime = ModoDesign.EstaEmModoDesign(this);

            if (emDesignTime)
                return;

            ConfigurarPopupPeriodoRelatorio(painelConteudo);

            txtPeriodoRelatorio.Click += delegate
            {
                AlternarPopupPeriodoRelatorio(campoPeriodoRelatorio);
            };

            btnCalendarioPeriodoRelatorio.Click += delegate
            {
                AlternarPopupPeriodoRelatorio(campoPeriodoRelatorio);
            };

            // Trata a alteração do período do gráfico
            cmbPeriodoGrafico.SelectedIndexChanged += delegate
            {
                if (graficoReceita == null)
                    return;

                try
                {
                    AtualizarGraficoReceita();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        this,
                        "Não foi possível atualizar o gráfico de receitas.\n\n" +
                        ex.Message,
                        "Erro ao atualizar gráfico",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            // Associa a exportação ao botão existente no Designer
            btnExportarRelatorio.Click += delegate
            {
                ExportarRelatorioCsv();
            };

            // Carrega os dados quando o formulário for exibido
            Shown += delegate
            {
                AtualizarRelatorios();
            };
        }

        // Consulta o banco e atualiza os indicadores e gráficos
        private void AtualizarRelatorios()
        {
            try
            {
                dadosRelatoriosCarregados = false;

                DateTime inicio = inicioPeriodoRelatorio.Date;
                DateTime fim = fimPeriodoRelatorio.Date;

                decimal recebida =
                    relatorioRepositorio.ObterReceitaRecebida(inicio, fim);

                decimal pendente =
                    relatorioRepositorio.ObterReceitaPendente();

                int alunosAtivos =
                    relatorioRepositorio.ObterQuantidadeAlunosAtivos();

                ResumoFrequenciaRelatorio frequencia =
                    relatorioRepositorio.ObterResumoFrequencia(inicio, fim);

                if (frequencia == null)
                    throw new InvalidOperationException(
                        "Não foi possível obter os dados de frequência.");

                // Armazena os dados consultados para exportação
                receitaRecebidaAtual = recebida;
                receitaPendenteAtual = pendente;
                alunosAtivosAtual = alunosAtivos;
                frequenciaAtual = frequencia;

                // Indicadores financeiros
                lblReceitaValor.Text = recebida.ToString(
                    "C2", culturaBrasileira);

                lblReceitaTotal.Text = (recebida + pendente).ToString(
                    "C2", culturaBrasileira);

                lblReceitaRecebida.Text = recebida.ToString(
                    "C2", culturaBrasileira);

                lblReceitaPendente.Text = pendente.ToString(
                    "C2", culturaBrasileira);

                // Quantidade de alunos ativos
                lblAlunosValor.Text = alunosAtivos.ToString(
                    "N0", culturaBrasileira);

                // Percentual de presença
                // Justificados não entram no cálculo do percentual.
                lblPresencaValor.Text =
                    frequencia.PercentualPresenca.ToString(
                        "N1", culturaBrasileira) + "%";

                // Atualiza o gráfico de frequência
                graficoPresencaRelatorios.ConfigurarDados(
                    frequencia.Presentes,
                    frequencia.Ausentes,
                    frequencia.Justificados);

                // Atualiza o gráfico financeiro
                AtualizarGraficoReceita();

                // Libera a exportação somente após carregar todos os dados
                dadosRelatoriosCarregados = true;
            }
            catch (Exception ex)
            {
                dadosRelatoriosCarregados = false;

                MessageBox.Show(
                    this,
                    "Não foi possível carregar os dados dos relatórios.\n\n" +
                    ex.Message,
                    "Erro ao carregar relatórios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Atualiza o gráfico de evolução da receita
        private void AtualizarGraficoReceita()
        {
            if (graficoReceita == null || cmbPeriodoGrafico == null)
                return;

            int quantidadeMeses =
                cmbPeriodoGrafico.SelectedIndex == 1 ? 3 : 6;

            DateTime referencia = fimPeriodoRelatorio;

            var dados = relatorioRepositorio.ObterEvolucaoReceita(
                referencia, quantidadeMeses);

            if (dados == null || dados.Count != quantidadeMeses)
            {
                throw new InvalidOperationException(
                    "Não foi possível obter os dados da evolução da receita.");
            }

            // O painel trabalha com seis posições.
            // Quando o usuário escolhe três meses, os dados ficam nas
            // três últimas posições do gráfico.
            string[] meses = new string[6];
            double[] valores = new double[6];

            int inicio = 6 - quantidadeMeses;

            for (int i = 0; i < quantidadeMeses; i++)
            {
                int indiceGrafico = inicio + i;

                meses[indiceGrafico] = dados[i].NomeMes;
                valores[indiceGrafico] = (double)dados[i].Valor;
            }

            // Armazena os dados para a exportação
            mesesReceitaAtual = (string[])meses.Clone();

            for (int i = 0; i < 6; i++)
            {
                valoresReceitaAtual[i] = (decimal)valores[i];
            }

            graficoReceita.QuantidadeMeses = quantidadeMeses;
            graficoReceita.ConfigurarDados(meses, valores);
        }

        // Exporta os dados consultados para um arquivo CSV compatível com Excel
        private void ExportarRelatorioCsv()
        {
            if (!dadosRelatoriosCarregados || frequenciaAtual == null)
            {
                MessageBox.Show(
                    this,
                    "Aguarde o carregamento dos dados antes de exportar.",
                    "Relatório indisponível",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Title = "Exportar relatório";
                dialogo.Filter = "Arquivo CSV (*.csv)|*.csv";
                dialogo.DefaultExt = "csv";
                dialogo.AddExtension = true;

                dialogo.FileName =
                    "Relatorio_Stingray_" +
                    DateTime.Now.ToString("yyyyMMdd_HHmm");

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    StringBuilder csv = new StringBuilder();

                    AdicionarLinhaCsv(csv, "RELATÓRIO STINGRAY");

                    AdicionarLinhaCsv(
                        csv,
                        "Período consultado",
                        inicioPeriodoRelatorio.ToString("dd/MM/yyyy") +
                        " até " +
                        fimPeriodoRelatorio.ToString("dd/MM/yyyy"));

                    AdicionarLinhaCsv(csv);

                    AdicionarLinhaCsv(csv, "INDICADORES FINANCEIROS");

                    AdicionarLinhaCsv(
                        csv,
                        "Receita recebida",
                        receitaRecebidaAtual.ToString(
                            "C2", culturaBrasileira));

                    AdicionarLinhaCsv(
                        csv,
                        "Receita pendente",
                        receitaPendenteAtual.ToString(
                            "C2", culturaBrasileira));

                    AdicionarLinhaCsv(
                        csv,
                        "Receita total",
                        (receitaRecebidaAtual + receitaPendenteAtual)
                            .ToString("C2", culturaBrasileira));

                    AdicionarLinhaCsv(csv);

                    AdicionarLinhaCsv(csv, "INDICADORES DE FREQUÊNCIA");

                    AdicionarLinhaCsv(
                        csv,
                        "Percentual de presença",
                        frequenciaAtual.PercentualPresenca.ToString(
                            "N1", culturaBrasileira) + "%");

                    AdicionarLinhaCsv(
                        csv,
                        "Presentes",
                        frequenciaAtual.Presentes.ToString(
                            "N0", culturaBrasileira));

                    AdicionarLinhaCsv(
                        csv,
                        "Ausentes",
                        frequenciaAtual.Ausentes.ToString(
                            "N0", culturaBrasileira));

                    AdicionarLinhaCsv(
                        csv,
                        "Justificados",
                        frequenciaAtual.Justificados.ToString(
                            "N0", culturaBrasileira));

                    AdicionarLinhaCsv(
                        csv,
                        "Alunos ativos",
                        alunosAtivosAtual.ToString(
                            "N0", culturaBrasileira));

                    AdicionarLinhaCsv(csv);

                    AdicionarLinhaCsv(csv, "EVOLUÇÃO DA RECEITA");

                    AdicionarLinhaCsv(csv, "Mês", "Receita");

                    int quantidadeMeses =
                        cmbPeriodoGrafico.SelectedIndex == 1 ? 3 : 6;

                    int inicio = 6 - quantidadeMeses;

                    for (int i = inicio; i < 6; i++)
                    {
                        AdicionarLinhaCsv(
                            csv,
                            mesesReceitaAtual[i] ?? "",
                            valoresReceitaAtual[i].ToString(
                                "C2", culturaBrasileira));
                    }

                    AdicionarLinhaCsv(csv);

                    AdicionarLinhaCsv(
                        csv,
                        "Observação",
                        "Registros justificados não entram no cálculo percentual de presença.");

                    // BOM UTF-8 para preservar acentos no Excel
                    File.WriteAllText(
                        dialogo.FileName,
                        csv.ToString(),
                        new UTF8Encoding(true));

                    MessageBox.Show(
                        this,
                        "Relatório exportado com sucesso!",
                        "Exportação concluída",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        this,
                        "Não foi possível exportar o relatório.\n\n" +
                        ex.Message,
                        "Erro na exportação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // Adiciona uma linha ao CSV, separando os campos por ponto e vírgula
        private void AdicionarLinhaCsv(
            StringBuilder csv,
            params string[] valores)
        {
            if (valores == null || valores.Length == 0)
            {
                csv.AppendLine();
                return;
            }

            string[] campos = new string[valores.Length];

            for (int i = 0; i < valores.Length; i++)
            {
                string valor = valores[i] ?? "";

                // Escapa aspas para manter o CSV válido
                campos[i] = "\"" + valor.Replace("\"", "\"\"") + "\"";
            }

            csv.AppendLine(string.Join(";", campos));
        }

        // Desenha o contorno arredondado do campo de período
        private void PeriodoRelatorio_Paint(object sender, PaintEventArgs e)
        {
            Panel painel = sender as Panel;

            if (painel == null || painel.Width <= 1 || painel.Height <= 1)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = new Rectangle(
                0, 0, painel.Width - 1, painel.Height - 1);

            using (GraphicsPath caminho = CriarCaminhoArredondado(area, 5))
            using (SolidBrush fundo = new SolidBrush(Color.White))
            using (Pen borda = new Pen(VisualPadrao.BordaCampo, 1F))
            {
                e.Graphics.FillPath(fundo, caminho);
                e.Graphics.DrawPath(borda, caminho);
            }
        }

        // Prepara o seletor de datas usado nos relatórios
        private void ConfigurarPopupPeriodoRelatorio(Control container)
        {
            BorderPanel popup = new BorderPanel();

            popupPeriodoRelatorio = popup;
            popup.Name = "popupPeriodoRelatorio";
            popup.Size = new Size(304, 162);
            popup.BackColor = Color.White;
            popup.BorderColor = VisualPadrao.BordaCartao;
            popup.CornerRadius = 9;
            popup.Padding = new Padding(1);
            popup.Visible = false;
            popup.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            Label titulo = new Label();
            titulo.Text = "Selecionar período";
            titulo.Font = new Font(
                "Segoe UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            titulo.ForeColor = VisualPadrao.TextoPrincipal;
            titulo.Location = new Point(14, 9);
            titulo.Size = new Size(260, 21);
            popup.Controls.Add(titulo);

            Label subtitulo = new Label();
            subtitulo.Text = "Escolha as datas para consultar os relatórios.";
            subtitulo.Font = new Font(
                "Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            subtitulo.ForeColor = VisualPadrao.TextoSecundario;
            subtitulo.Location = new Point(14, 30);
            subtitulo.Size = new Size(274, 17);
            popup.Controls.Add(subtitulo);

            Panel divisor = new Panel();
            divisor.BackColor = Color.FromArgb(241, 245, 249);
            divisor.Location = new Point(13, 52);
            divisor.Size = new Size(276, 1);
            popup.Controls.Add(divisor);

            Label inicial = new Label();
            inicial.Text = "Data inicial";
            inicial.Font = new Font(
                "Segoe UI Semibold", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            inicial.ForeColor = VisualPadrao.TextoSecundario;
            inicial.Location = new Point(14, 61);
            inicial.Size = new Size(128, 15);
            popup.Controls.Add(inicial);

            Label final = new Label();
            final.Text = "Data final";
            final.Font = new Font(
                "Segoe UI Semibold", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            final.ForeColor = VisualPadrao.TextoSecundario;
            final.Location = new Point(158, 61);
            final.Size = new Size(128, 15);
            popup.Controls.Add(final);

            dataInicialRelatorio = new DateTimePicker();
            dataInicialRelatorio.Name = "dataInicialRelatorio";
            dataInicialRelatorio.Format = DateTimePickerFormat.Short;
            dataInicialRelatorio.Font = new Font(
                "Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            dataInicialRelatorio.Location = new Point(14, 78);
            dataInicialRelatorio.Size = new Size(128, 25);
            dataInicialRelatorio.CalendarForeColor = VisualPadrao.TextoPrincipal;
            dataInicialRelatorio.CalendarTitleBackColor = VisualPadrao.Azul;
            dataInicialRelatorio.CalendarTitleForeColor = Color.White;
            dataInicialRelatorio.Value = inicioPeriodoRelatorio;
            popup.Controls.Add(dataInicialRelatorio);

            dataFinalRelatorio = new DateTimePicker();
            dataFinalRelatorio.Name = "dataFinalRelatorio";
            dataFinalRelatorio.Format = DateTimePickerFormat.Short;
            dataFinalRelatorio.Font = new Font(
                "Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            dataFinalRelatorio.Location = new Point(158, 78);
            dataFinalRelatorio.Size = new Size(128, 25);
            dataFinalRelatorio.CalendarForeColor = VisualPadrao.TextoPrincipal;
            dataFinalRelatorio.CalendarTitleBackColor = VisualPadrao.Azul;
            dataFinalRelatorio.CalendarTitleForeColor = Color.White;
            dataFinalRelatorio.Value = fimPeriodoRelatorio;
            popup.Controls.Add(dataFinalRelatorio);

            Button cancelar = new Button();
            cancelar.Name = "btnCancelarPeriodoRelatorio";
            cancelar.Text = "Cancelar";
            cancelar.Font = new Font(
                "Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            cancelar.FlatStyle = FlatStyle.Flat;
            cancelar.FlatAppearance.BorderColor = VisualPadrao.BordaCartao;
            cancelar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(248, 250, 252);
            cancelar.BackColor = Color.White;
            cancelar.ForeColor = VisualPadrao.TextoPrincipal;
            cancelar.UseVisualStyleBackColor = false;
            cancelar.Size = new Size(82, 28);
            cancelar.Location = new Point(112, 119);
            cancelar.Cursor = Cursors.Hand;

            cancelar.Click += delegate
            {
                popupPeriodoRelatorio.Visible = false;
            };

            popup.Controls.Add(cancelar);

            RoundedButton aplicar = new RoundedButton();
            aplicar.Name = "btnAplicarPeriodoRelatorio";
            aplicar.Text = "Aplicar período";
            aplicar.CornerRadius = 5;
            aplicar.Font = new Font(
                "Segoe UI Semibold", 8F, FontStyle.Regular, GraphicsUnit.Point);
            aplicar.BackColor = VisualPadrao.Azul;
            aplicar.ForeColor = Color.White;
            aplicar.Size = new Size(116, 28);
            aplicar.Location = new Point(200, 119);
            aplicar.Cursor = Cursors.Hand;

            aplicar.Click += delegate
            {
                if (dataFinalRelatorio.Value.Date <
                    dataInicialRelatorio.Value.Date)
                {
                    MessageBox.Show(
                        this,
                        "A data final não pode ser anterior à data inicial.",
                        "Período inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    popupPeriodoRelatorio.Visible = true;
                    popupPeriodoRelatorio.BringToFront();
                    return;
                }

                inicioPeriodoRelatorio = dataInicialRelatorio.Value.Date;
                fimPeriodoRelatorio = dataFinalRelatorio.Value.Date;

                txtPeriodoRelatorio.Text =
                    inicioPeriodoRelatorio.ToString("dd/MM/yyyy") +
                    " - " +
                    fimPeriodoRelatorio.ToString("dd/MM/yyyy");

                txtPeriodoRelatorio.ForeColor =
                    VisualPadrao.TextoPrincipal;

                popupPeriodoRelatorio.Visible = false;

                AtualizarRelatorios();
            };

            popup.Controls.Add(aplicar);

            container.Controls.Add(popupPeriodoRelatorio);
            popupPeriodoRelatorio.BringToFront();
        }

        // Alterna a visibilidade do popup de período
        private void AlternarPopupPeriodoRelatorio(Control campoPeriodo)
        {
            if (popupPeriodoRelatorio == null || campoPeriodo == null)
                return;

            if (popupPeriodoRelatorio.Visible)
            {
                popupPeriodoRelatorio.Visible = false;
                return;
            }

            // Atualiza os seletores com o período atualmente aplicado
            dataInicialRelatorio.Value = inicioPeriodoRelatorio;
            dataFinalRelatorio.Value = fimPeriodoRelatorio;

            Rectangle campoNaTela =
                campoPeriodo.RectangleToScreen(campoPeriodo.ClientRectangle);

            Point abaixo = popupPeriodoRelatorio.Parent.PointToClient(
                new Point(campoNaTela.Left, campoNaTela.Bottom + 5));

            int margem = 8;

            int x = Math.Max(margem, Math.Min(
                abaixo.X,
                popupPeriodoRelatorio.Parent.ClientSize.Width -
                popupPeriodoRelatorio.Width - margem));

            int y = abaixo.Y;

            if (y + popupPeriodoRelatorio.Height >
                popupPeriodoRelatorio.Parent.ClientSize.Height - margem)
            {
                Point acima = popupPeriodoRelatorio.Parent.PointToClient(
                    new Point(
                        campoNaTela.Left,
                        campoNaTela.Top - popupPeriodoRelatorio.Height - 5));

                y = acima.Y;
            }

            y = Math.Max(margem, Math.Min(
                y,
                popupPeriodoRelatorio.Parent.ClientSize.Height -
                popupPeriodoRelatorio.Height - margem));

            popupPeriodoRelatorio.Location = new Point(x, y);
            popupPeriodoRelatorio.Visible = true;
            popupPeriodoRelatorio.BringToFront();
        }

        // Desenha o ícone de calendário no botão
        private void CalendarioPeriodo_Paint(object sender, PaintEventArgs e)
        {
            Button botao = sender as Button;

            if (botao == null)
                return;

            bool hover = botao.ClientRectangle.Contains(
                botao.PointToClient(Cursor.Position));

            Color cor = hover
                ? Color.FromArgb(29, 78, 216)
                : Color.FromArgb(55, 96, 160);

            Graphics g = e.Graphics;

            SmoothingMode suavizacaoAnterior = g.SmoothingMode;
            PixelOffsetMode pixelAnterior = g.PixelOffsetMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            try
            {
                float tamanho = Math.Min(
                    18F,
                    Math.Min(
                        botao.ClientSize.Width - 4F,
                        botao.ClientSize.Height - 4F));

                if (tamanho < 12F)
                    return;

                float escala = tamanho / 18F;
                float x = (botao.ClientSize.Width - tamanho) / 2F;
                float y = (botao.ClientSize.Height - tamanho) / 2F;

                using (Pen caneta = new Pen(
                    cor, Math.Max(1F, 1.25F * escala)))
                using (Pen grade = new Pen(
                    Color.FromArgb(115, cor),
                    Math.Max(0.85F, 0.95F * escala)))
                using (Brush pontos = new SolidBrush(cor))
                using (Brush cabecalho = new SolidBrush(
                    Color.FromArgb(235, 242, 255)))
                {
                    caneta.StartCap = LineCap.Round;
                    caneta.EndCap = LineCap.Round;
                    grade.StartCap = LineCap.Round;
                    grade.EndCap = LineCap.Round;

                    RectangleF corpo = new RectangleF(
                        x + 2.7F * escala,
                        y + 4.3F * escala,
                        12.6F * escala,
                        12.2F * escala);

                    using (GraphicsPath caminho =
                        CriarCaminhoArredondado(
                            Rectangle.Round(corpo),
                            Math.Max(1, (int)(2F * escala))))
                    {
                        g.DrawPath(caneta, caminho);
                    }

                    g.FillRectangle(
                        cabecalho,
                        x + 3.6F * escala,
                        y + 5.2F * escala,
                        10.8F * escala,
                        2.5F * escala);

                    g.DrawLine(
                        grade,
                        x + 3.5F * escala,
                        y + 8.4F * escala,
                        x + 14.5F * escala,
                        y + 8.4F * escala);

                    g.DrawLine(
                        caneta,
                        x + 6.1F * escala,
                        y + 2.2F * escala,
                        x + 6.1F * escala,
                        y + 5.5F * escala);

                    g.DrawLine(
                        caneta,
                        x + 11.9F * escala,
                        y + 2.2F * escala,
                        x + 11.9F * escala,
                        y + 5.5F * escala);

                    float[,] dias = new float[,]
                    {
                        { 5.4F, 10.2F },
                        { 9.0F, 10.2F },
                        { 12.0F, 10.2F },
                        { 5.4F, 13.1F },
                        { 9.0F, 13.1F },
                        { 12.0F, 13.1F }
                    };

                    float diametro = 1.35F * escala;

                    for (int i = 0; i < dias.GetLength(0); i++)
                    {
                        g.FillEllipse(
                            pontos,
                            x + dias[i, 0] * escala,
                            y + dias[i, 1] * escala,
                            diametro,
                            diametro);
                    }
                }
            }
            finally
            {
                g.SmoothingMode = suavizacaoAnterior;
                g.PixelOffsetMode = pixelAnterior;
            }
        }

        // Botão de exportar relatório com ícone de download
        private sealed class DownloadReportButton : RoundedButton
        {
            public string Caption
            {
                get;
                set;
            }

            public DownloadReportButton()
            {
                Caption = "Exportar relatório";
                Text = string.Empty;
                TextAlign = ContentAlignment.MiddleLeft;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen seta = new Pen(ForeColor, 1.55F))
                {
                    seta.StartCap = LineCap.Round;
                    seta.EndCap = LineCap.Round;
                    seta.LineJoin = LineJoin.Round;

                    g.DrawLine(seta, 17F, 7F, 17F, 14F);
                    g.DrawLine(seta, 13.5F, 10.8F, 17F, 14.2F);
                    g.DrawLine(seta, 17F, 14.2F, 20.5F, 10.8F);
                    g.DrawLine(seta, 12.8F, 20F, 21.2F, 20F);
                }

                Rectangle areaTexto = new Rectangle(
                    30, 2, Math.Max(0, Width - 34), Math.Max(0, Height - 4));

                TextRenderer.DrawText(
                    g,
                    Caption,
                    Font,
                    areaTexto,
                    ForeColor,
                    TextFormatFlags.Left |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine |
                    TextFormatFlags.EndEllipsis |
                    TextFormatFlags.NoPrefix);
            }
        }

        // Painel com borda arredondada
        private sealed class BorderPanel : Panel
        {
            public Color BorderColor
            {
                get;
                set;
            }

            public int CornerRadius
            {
                get;
                set;
            }

            public BorderPanel()
            {
                BorderColor = Color.FromArgb(229, 234, 241);
                CornerRadius = 7;
                DoubleBuffered = true;
                ResizeRedraw = true;
                Padding = new Padding(1);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(
                    0,
                    0,
                    Math.Max(0, Width - 1),
                    Math.Max(0, Height - 1));

                using (GraphicsPath path =
                    CriarCaminhoArredondado(rect, CornerRadius))
                using (Pen pen = new Pen(BorderColor, 1F))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        // Exibe um ícone dentro de um selo visual
        private sealed class IconBadge : Panel
        {
            public Color AccentColor
            {
                get;
                set;
            }

            public Color BadgeColor
            {
                get;
                set;
            }

            public string IconType
            {
                get;
                set;
            }

            public IconBadge()
            {
                DoubleBuffered = true;
                AccentColor = Color.FromArgb(22, 163, 74);
                BadgeColor = Color.FromArgb(226, 248, 235);
                IconType = "money";
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                float d = Math.Min(Width, Height) - 2;

                RectangleF circle = new RectangleF(
                    (Width - d) / 2,
                    (Height - d) / 2,
                    d,
                    d);

                using (SolidBrush brush = new SolidBrush(BadgeColor))
                {
                    g.FillEllipse(brush, circle);
                }

                using (Pen pen = new Pen(AccentColor, 1.7F))
                {
                    if (IconType == "money")
                    {
                        RectangleF coin = new RectangleF(
                            Width / 2F - 8,
                            Height / 2F - 8,
                            16,
                            16);

                        g.DrawEllipse(pen, coin);

                        using (Font font = new Font(
                            "Segoe UI Semibold",
                            9F,
                            FontStyle.Regular,
                            GraphicsUnit.Point))
                        using (SolidBrush brush = new SolidBrush(AccentColor))
                        {
                            g.DrawString(
                                "$",
                                font,
                                brush,
                                new PointF(
                                    Width / 2F - 4.7F,
                                    Height / 2F - 7.5F));
                        }
                    }
                    else if (IconType == "people")
                    {
                        pen.Width = 1.8F;
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        pen.LineJoin = LineJoin.Round;

                        float cx = Width / 2F + 0.5F;
                        float cy = Height / 2F + 0.5F;

                        g.DrawEllipse(
                            pen,
                            cx - 4F,
                            cy - 8.5F,
                            8F,
                            8F);

                        using (GraphicsPath ombros = new GraphicsPath())
                        {
                            ombros.StartFigure();

                            ombros.AddBezier(
                                new PointF(cx - 9.5F, cy + 7.5F),
                                new PointF(cx - 9F, cy + 2.5F),
                                new PointF(cx - 5.5F, cy + 1F),
                                new PointF(cx, cy + 1F));

                            ombros.AddBezier(
                                new PointF(cx, cy + 1F),
                                new PointF(cx + 5.5F, cy + 1F),
                                new PointF(cx + 9F, cy + 2.5F),
                                new PointF(cx + 9.5F, cy + 7.5F));

                            g.DrawPath(pen, ombros);
                        }
                    }
                    else
                    {
                        PointF[] cap = new PointF[]
                        {
                            new PointF(Width / 2F, Height / 2F - 10),
                            new PointF(Width / 2F + 10, Height / 2F - 4),
                            new PointF(Width / 2F, Height / 2F + 2),
                            new PointF(Width / 2F - 10, Height / 2F - 4)
                        };

                        g.DrawPolygon(pen, cap);

                        g.DrawLine(
                            pen,
                            Width / 2F + 7,
                            Height / 2F - 1,
                            Width / 2F + 7,
                            Height / 2F + 6);

                        g.DrawArc(
                            pen,
                            Width / 2F - 8,
                            Height / 2F + 2,
                            16,
                            13,
                            185,
                            170);
                    }
                }
            }
        }

        // Desenha o gráfico de frequência do relatório
        private sealed class AttendanceChartPanel : Panel
        {
            private readonly Color corPresentes =
                Color.FromArgb(22, 163, 74);

            private readonly Color corAusentes =
                Color.FromArgb(239, 68, 68);

            private readonly Color corJustificados =
                Color.FromArgb(245, 158, 11);

            private int presentes;
            private int ausentes;
            private int justificados;

            public AttendanceChartPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            // Recebe os dados reais do banco
            public void ConfigurarDados(
                int quantidadePresentes,
                int quantidadeAusentes,
                int quantidadeJustificados)
            {
                presentes = Math.Max(0, quantidadePresentes);
                ausentes = Math.Max(0, quantidadeAusentes);
                justificados = Math.Max(0, quantidadeJustificados);

                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint =
                    System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Justificados ficam fora do total considerado.
                int total = presentes + ausentes;

                int area = Math.Max(
                    54,
                    Math.Min(136, Math.Min(Height - 35, Width / 3)));

                int x = 15;
                int y = Math.Max(8, (Height - area) / 2 - 1);

                Rectangle circulo = new Rectangle(x, y, area, area);

                if (total == 0)
                {
                    using (Pen contorno = new Pen(
                        Color.FromArgb(226, 232, 240),
                        Math.Max(8F, area * 0.16F)))
                    {
                        g.DrawEllipse(contorno, circulo);
                    }
                }
                else
                {
                    float anguloPresentes =
                        presentes / (float)total * 360F;

                    float anguloAusentes =
                        ausentes / (float)total * 360F;

                    using (SolidBrush brush =
                        new SolidBrush(corPresentes))
                    {
                        g.FillPie(
                            brush,
                            circulo,
                            -90F,
                            anguloPresentes);
                    }

                    using (SolidBrush brush =
                        new SolidBrush(corAusentes))
                    {
                        g.FillPie(
                            brush,
                            circulo,
                            -90F + anguloPresentes,
                            anguloAusentes);
                    }
                }

                int interior = (int)(area * 0.61F);

                Rectangle centro = new Rectangle(
                    x + (area - interior) / 2,
                    y + (area - interior) / 2,
                    interior,
                    interior);

                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brush, centro);
                }

                decimal percentual = total == 0
                    ? 0
                    : (decimal)presentes / total * 100;

                string textoCentro = total == 0
                    ? "—"
                    : percentual.ToString(
                        "N0", CultureInfo.GetCultureInfo("pt-BR")) + "%";

                using (Font fonte = new Font(
                    "Segoe UI Semibold",
                    Math.Max(10F, area / 8.3F),
                    FontStyle.Regular,
                    GraphicsUnit.Point))
                {
                    SizeF tam = g.MeasureString(textoCentro, fonte);

                    using (SolidBrush brushVazio =
                        new SolidBrush(VisualPadrao.TextoSecundario))
                    {
                        g.DrawString(
                            textoCentro,
                            fonte,
                            brushVazio,
                            centro.Left + (centro.Width - tam.Width) / 2,
                            centro.Top + (centro.Height - tam.Height) / 2 - 1);
                    }
                }

                int legendaX = x + area + 16;
                int larguraLegenda = Math.Max(100, Width - legendaX - 8);

                int alturaItem = Math.Max(
                    32,
                    Math.Min(43, (Height - 20) / 3));

                int yLegenda = Math.Max(
                    10,
                    (Height - alturaItem * 3 - 14) / 2);

                DesenharItemLegenda(
                    g, legendaX, yLegenda, larguraLegenda,
                    corPresentes, "Presentes", presentes);

                DesenharItemLegenda(
                    g, legendaX, yLegenda + alturaItem, larguraLegenda,
                    corAusentes, "Ausentes", ausentes);

                DesenharItemLegenda(
                    g, legendaX, yLegenda + alturaItem * 2, larguraLegenda,
                    corJustificados, "Justificadas", justificados);

                using (Font fonteTotal = new Font(
                    "Segoe UI", 7.2F, FontStyle.Regular, GraphicsUnit.Point))
                using (SolidBrush brush = new SolidBrush(
                    VisualPadrao.TextoSecundario))
                {
                    string mensagem = total == 0
                        ? (justificados > 0
                            ? "Somente registros justificados"
                            : "Sem registros de presença")
                        : "Total considerado: " +
                            total.ToString("N0",
                                CultureInfo.GetCultureInfo("pt-BR"));

                    g.DrawString(
                        mensagem,
                        fonteTotal,
                        brush,
                        legendaX,
                        Math.Min(
                            Height - 15,
                            yLegenda + alturaItem * 3 + 1));
                }
            }

            private void DesenharItemLegenda(
                Graphics g,
                int x,
                int y,
                int largura,
                Color cor,
                string titulo,
                int quantidade)
            {
                using (SolidBrush ponto = new SolidBrush(cor))
                {
                    g.FillEllipse(ponto, x, y + 5, 6, 6);
                }

                using (Font fonteTitulo = new Font(
                    "Segoe UI Semibold",
                    7.7F,
                    FontStyle.Regular,
                    GraphicsUnit.Point))
                using (SolidBrush brushTitulo = new SolidBrush(
                    Color.FromArgb(55, 65, 81)))
                {
                    g.DrawString(
                        titulo,
                        fonteTitulo,
                        brushTitulo,
                        x + 12,
                        y);
                }

                using (Font fonteDetalhe = new Font(
                    "Segoe UI",
                    7F,
                    FontStyle.Regular,
                    GraphicsUnit.Point))
                using (SolidBrush brushDetalhe = new SolidBrush(
                    VisualPadrao.TextoSecundario))
                {
                    g.DrawString(
                        quantidade.ToString(
                            "N0", CultureInfo.GetCultureInfo("pt-BR")) +
                        " registro(s)",
                        fonteDetalhe,
                        brushDetalhe,
                        x + 12,
                        y + 15);
                }
            }
        }

        // Desenha o gráfico de receitas do relatório
        private sealed class RevenueChartPanel : Panel
        {
            private string[] meses = new string[6];
            private double[] valores = new double[6];

            public int QuantidadeMeses
            {
                get;
                set;
            }

            public RevenueChartPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
                QuantidadeMeses = 6;
            }

            // Recebe os valores consultados no banco
            public void ConfigurarDados(
                string[] novosMeses,
                double[] novosValores)
            {
                if (novosMeses == null || novosValores == null ||
                    novosMeses.Length != 6 || novosValores.Length != 6)
                {
                    throw new ArgumentException(
                        "O gráfico precisa receber seis posições de dados.");
                }

                meses = (string[])novosMeses.Clone();
                valores = (double[])novosValores.Clone();

                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint =
                    System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                if (valores.Length == 0 ||
                    Array.TrueForAll(valores, valor => valor <= 0))
                {
                    using (Font fonteVazia = new Font(
                        "Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point))
                    using (SolidBrush pincelVazio = new SolidBrush(
                        VisualPadrao.TextoSecundario))
                    {
                        const string mensagem =
                            "Sem dados financeiros para o período selecionado.";

                        SizeF tamanho = g.MeasureString(
                            mensagem, fonteVazia);

                        g.DrawString(
                            mensagem,
                            fonteVazia,
                            pincelVazio,
                            Math.Max(8F, (Width - tamanho.Width) / 2F),
                            Math.Max(8F, (Height - tamanho.Height) / 2F));
                    }

                    return;
                }

                int left = 68;
                int right = 12;
                int top = 27;
                int bottom = 32;

                int larguraGrafico = Math.Max(
                    40, Width - left - right);

                int alturaGrafico = Math.Max(
                    35, Height - top - bottom);

                double maiorValor = 0;

                for (int i = 0; i < valores.Length; i++)
                {
                    if (valores[i] > maiorValor)
                        maiorValor = valores[i];
                }

                // Ajusta a escala de acordo com os valores recebidos.
                double maximo = Math.Max(
                    5000D,
                    Math.Ceiling(maiorValor / 5000D) * 5000D);

                using (Font fonteEixo = new Font(
                    "Segoe UI", 6.6F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font fonteValor = new Font(
                    "Segoe UI Semibold", 6.7F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font fonteMes = new Font(
                    "Segoe UI", 6.8F, FontStyle.Regular, GraphicsUnit.Point))
                using (Pen grade = new Pen(
                    Color.FromArgb(235, 239, 244), 1F))
                using (SolidBrush brushEixo = new SolidBrush(
                    VisualPadrao.TextoSecundario))
                using (SolidBrush brushValor = new SolidBrush(
                    VisualPadrao.TextoPrincipal))
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        int yy = top + alturaGrafico -
                            (int)Math.Round(alturaGrafico * i / 5D);

                        g.DrawLine(
                            grade,
                            left,
                            yy,
                            left + larguraGrafico,
                            yy);

                        if (i == 0)
                            continue;

                        string textoEixo = "R$ " +
                            (maximo * i / 5D).ToString(
                                "#,##0",
                                CultureInfo.GetCultureInfo("pt-BR"));

                        SizeF sz = g.MeasureString(
                            textoEixo, fonteEixo);

                        g.DrawString(
                            textoEixo,
                            fonteEixo,
                            brushEixo,
                            Math.Max(0, left - sz.Width - 7),
                            yy - sz.Height / 2F);
                    }

                    int quantidade = Math.Max(
                        1, Math.Min(6, QuantidadeMeses));

                    int inicioDados = 6 - quantidade;

                    float passo = larguraGrafico / (float)quantidade;

                    float larguraBarra = Math.Min(
                        34F, passo * 0.52F);

                    for (int i = 0; i < quantidade; i++)
                    {
                        int indice = inicioDados + i;

                        float altura = (float)(
                            valores[indice] / maximo * alturaGrafico);

                        float bx = left + passo * i +
                            (passo - larguraBarra) / 2F;

                        float by = top + alturaGrafico - altura;

                        Color corBarra = indice == 5
                            ? Color.FromArgb(54, 99, 214)
                            : Color.FromArgb(158, 194, 244);

                        using (SolidBrush barra = new SolidBrush(corBarra))
                        {
                            g.FillRectangle(
                                barra,
                                bx,
                                by,
                                larguraBarra,
                                Math.Max(1, altura));
                        }

                        string rotuloValor = "R$ " +
                            valores[indice].ToString(
                                "#,##0",
                                CultureInfo.GetCultureInfo("pt-BR"));

                        SizeF tamanhoValor = g.MeasureString(
                            rotuloValor, fonteValor);

                        float valorX = Math.Max(
                            left - 4,
                            Math.Min(
                                bx + larguraBarra / 2F -
                                tamanhoValor.Width / 2F,
                                left + larguraGrafico -
                                tamanhoValor.Width));

                        g.DrawString(
                            rotuloValor,
                            fonteValor,
                            brushValor,
                            valorX,
                            Math.Max(1, by - tamanhoValor.Height - 2));

                        SizeF tamanhoMes = g.MeasureString(
                            meses[indice] ?? "",
                            fonteMes);

                        g.DrawString(
                            meses[indice] ?? "",
                            fonteMes,
                            brushEixo,
                            bx + larguraBarra / 2F -
                            tamanhoMes.Width / 2F,
                            top + alturaGrafico + 7);
                    }
                }
            }
        }

        // Cria caminhos com cantos arredondados
        private static GraphicsPath CriarCaminhoArredondado(
            Rectangle bounds,
            int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return path;

            int diametro = Math.Max(
                1,
                Math.Min(
                    radius * 2,
                    Math.Min(bounds.Width, bounds.Height)));

            Rectangle arco = new Rectangle(
                bounds.X, bounds.Y, diametro, diametro);

            path.AddArc(arco, 180, 90);

            arco.X = bounds.Right - diametro;
            path.AddArc(arco, 270, 90);

            arco.Y = bounds.Bottom - diametro;
            path.AddArc(arco, 0, 90);

            arco.X = bounds.X;
            path.AddArc(arco, 90, 90);

            path.CloseFigure();

            return path;
        }
    }
}
