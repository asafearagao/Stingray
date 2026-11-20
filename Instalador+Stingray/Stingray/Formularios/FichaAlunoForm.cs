using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Stingray.DadosTela;
using Stingray.Servicos;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Apresenta os dados cadastrais e o histórico de frequência do aluno
    public partial class FichaAlunoForm : Form
    {
        private static readonly CultureInfo CulturaPtBr = new CultureInfo("pt-BR");
        private readonly FichaAluno aluno;
        private readonly ConsultaCepServico consultaCepServico = new ConsultaCepServico();

        public FichaAlunoForm()
        {
            InitializeComponent();
            EstiloPopup.Aplicar(this);
        }

        // Construtor que recebe um aluno existente para exibir seus dados
        public FichaAlunoForm(FichaAluno aluno) : this()
        {
            if (aluno == null)
                throw new ArgumentNullException(nameof(aluno));

            this.aluno = aluno;
            PreencherDados();
            CarregarResumoFrequencia();
            Shown += async delegate { await CarregarEnderecoViaCepAsync(); };
            btnFechar.Click += delegate { Close(); };
            AcceptButton = btnFechar;
            CancelButton = btnFechar;
        }

        // Preenche os campos do formulário com os dados do aluno
        private void PreencherDados()
        {
            lblNomeValor.Text = ValorOuTraco(aluno.Nome);
            lblNascimentoValor.Text = aluno.DataNascimento.ToString("dd/MM/yyyy");
            lblTelefoneValor.Text = ValorOuTraco(aluno.Telefone);
            lblNumeroValor.Text = ValorOuTraco(aluno.Numero);
            lblCepValor.Text = ValorOuTraco(aluno.Cep);
            lblComplementoValor.Text = ValorOuTraco(aluno.Complemento);
            lblRuaValor.Text = ValorOuTraco(aluno.Logradouro);
            lblBairroValor.Text = ValorOuTraco(aluno.Bairro);
            lblCidadeValor.Text = ValorOuTraco(aluno.Cidade);
            lblResponsavelValor.Text = ValorOuTraco(aluno.Responsavel);
            lblCpfValor.Text = ValorOuTraco(aluno.Cpf);
            lblRgValor.Text = ValorOuTraco(aluno.Rg);
            lblEmailValor.Text = ValorOuTraco(aluno.Email);
            lblDataMatricula.Text = aluno.DataCadastro == DateTime.MinValue
                ? "Matrícula ainda não registrada."
                : "Campinas, " + aluno.DataCadastro.ToString("d 'de' MMMM 'de' yyyy", CulturaPtBr) + ".";
        }

        // Busca e organiza o histórico de frequência do aluno
        private void CarregarResumoFrequencia()
        {
            Panel cartao = new Panel
            {
                Name = "cartaoResumoFrequencia",
                Location = new Point(36, 544),
                Size = new Size(670, 66),
                BackColor = Color.White
            };
            cartao.Paint += delegate (object sender, PaintEventArgs e) {
                VisualPadrao.DesenharBordaCartao(e.Graphics, cartao.ClientRectangle);
            };

            Label titulo = new Label
            {
                Text = "Frequência recente",
                AutoSize = true,
                Location = new Point(12, 7),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            Label numeros = new Label
            {
                AutoSize = false,
                Location = new Point(12, 37),
                Size = new Size(640, 20),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(75, 85, 99)
            };
            Panel barra = new Panel
            {
                Location = new Point(12, 27),
                Size = new Size(644, 7),
                BackColor = Color.FromArgb(229, 231, 235)
            };

            cartao.Controls.Add(titulo);
            cartao.Controls.Add(barra);
            cartao.Controls.Add(numeros);
            Controls.Add(cartao);
            cartao.BringToFront();

            try
            {
                ResumoFrequenciaAluno resumo = new FrequenciaServico().ObterResumoAluno(aluno.IdAluno);
                if (resumo.TotalAulas == 0)
                {
                    titulo.Text = "Frequência recente · Ainda não há aulas registradas";
                    barra.Visible = false;
                    numeros.Text = "";
                    return;
                }

                int larguraPresencas = (int)Math.Round(barra.Width * (double)resumo.Presentes / resumo.TotalAulas);
                int larguraAusencias = (int)Math.Round(barra.Width * (double)resumo.Ausentes / resumo.TotalAulas);
                int larguraJustificadas = Math.Max(0, barra.Width - larguraPresencas - larguraAusencias);
                AdicionarFaixa(barra, Color.FromArgb(22, 163, 74), 0, larguraPresencas);
                AdicionarFaixa(barra, Color.FromArgb(220, 38, 38), larguraPresencas, larguraAusencias);
                AdicionarFaixa(barra, Color.FromArgb(234, 179, 8), larguraPresencas + larguraAusencias, larguraJustificadas);
                numeros.Text = "Presentes: " + resumo.Presentes + "     Ausentes: " + resumo.Ausentes +
                    "     Justificados: " + resumo.Justificados + "     Total: " + resumo.TotalAulas;
            }
            catch (Exception erro)
            {
                titulo.Text = "Frequência recente · Não foi possível carregar os dados";
                barra.Visible = false;
                numeros.Text = erro.Message;
                numeros.ForeColor = Color.FromArgb(185, 28, 28);
            }
        }

        private static void AdicionarFaixa(Panel barra, Color cor, int esquerda, int largura)
        {
            if (largura <= 0)
                return;
            barra.Controls.Add(new Panel
            {
                BackColor = cor,
                Location = new Point(esquerda, 0),
                Size = new Size(largura, barra.Height),
                Margin = new Padding(0)
            });
        }

        // Consulta o endereço do aluno pelo CEP usando o serviço ViaCEP
        private async Task CarregarEnderecoViaCepAsync()
        {
            if (string.IsNullOrWhiteSpace(aluno.Cep))
                return;

            try
            {
                ResultadoConsultaCep endereco = await consultaCepServico.ConsultarPorCepAsync(aluno.Cep);
                if (IsDisposed || endereco == null)
                    return;

                lblRuaValor.Text = ValorOuTraco(endereco.Logradouro);
                lblBairroValor.Text = ValorOuTraco(endereco.Bairro);
                lblCidadeValor.Text = ValorOuTraco(endereco.Cidade);
            }
            catch
            {
                // A ficha permanece disponível mesmo quando o serviço externo está indisponível
            }
        }

        // Retorna um valor ou um traço se o valor for nulo ou vazio
        private static string ValorOuTraco(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? "—" : valor;
        }
    }
}
