using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.DadosTela;
using Stingray.Servicos;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Controla o cadastro e a edição dos dados de um aluno
    public partial class AlunoCadastroForm : Form
    {
        private readonly FichaAluno alunoExistente;
        private readonly AlunoServico alunoServico = new AlunoServico();
        private readonly ConsultaCepServico consultaCepServico = new ConsultaCepServico();
        private ResultadoConsultaCep fichaEnderecoConsulta;
        private bool consultandoCep;

        public AlunoCadastroForm()
        {
            InitializeComponent();
            EstiloPopup.Aplicar(this);
        }

        // Construtor que recebe uma ficha de aluno existente para edição
        public AlunoCadastroForm(FichaAluno aluno) : this()
        {
            alunoExistente = aluno;
            bool editando = alunoExistente != null;

            // Configura o título e os textos dos botões de acordo com o contexto (novo ou edição)
            Text = editando ? "Editar aluno" : "Novo aluno";
            lblTitulo.Text = editando ? "Editar Aluno" : "Novo Aluno";
            btnSalvar.Text = editando ? "Salvar alterações" : "Salvar aluno";

            ConfigurarCampos();
            if (editando)
                PreencherCampos(alunoExistente);

            txtDataNascimento.TextChanged += delegate { AtualizarCampoResponsavelPorIdade(); };
            txtCep.Leave += TxtCep_Leave;
            btnSalvar.Click += BtnSalvar_Click;
            btnCancelar.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            Shown += delegate { txtNome.Focus(); };

            AtualizarCampoResponsavelPorIdade();
        }

        // Configura os campos de entrada com limites de caracteres
        private void ConfigurarCampos()
        {
            ConfigurarFormatacaoProgressiva(txtTelefone, FormatarTelefone, 11);
            ConfigurarFormatacaoProgressiva(txtCep, FormatarCep, 8);
            ConfigurarFormatacaoProgressiva(txtCpf, FormatarCpf, 11);
            ConfigurarFormatacaoProgressiva(txtDataNascimento, FormatarData, 8);
            ConfigurarFormatacaoRg(txtRg);
            txtRg.MaxLength = 12;
            txtComplemento.MaxLength = 50;
        }

        // Preenche os campos do formulário com os dados do aluno existente
        private void PreencherCampos(FichaAluno aluno)
        {
            txtNome.Text = aluno.Nome ?? string.Empty;
            txtDataNascimento.Text = aluno.DataNascimento == DateTime.MinValue
                ? string.Empty : aluno.DataNascimento.ToString("ddMMyyyy");
            txtTelefone.Text = ExtrairDigitos(aluno.Telefone);
            txtNumero.Text = aluno.Numero ?? string.Empty;
            txtCep.Text = ExtrairDigitos(aluno.Cep);
            txtComplemento.Text = aluno.Complemento ?? string.Empty;
            txtResponsavel.Text = aluno.Responsavel ?? string.Empty;
            txtCpf.Text = ExtrairDigitos(aluno.Cpf);
            txtRg.Text = aluno.Rg ?? string.Empty;
            txtEmail.Text = aluno.Email ?? string.Empty;
            nudDiaVencimento.Value = Math.Max(nudDiaVencimento.Minimum,
                Math.Min(nudDiaVencimento.Maximum, aluno.DiaVencimento));
        }

        // Exibe o campo de responsável conforme a idade informada
        private void AtualizarCampoResponsavelPorIdade()
        {
            // Habilita ou desabilita o campo de responsável com base na idade do aluno
            DateTime nascimento;
            if (!TentarObterDataNascimento(out nascimento))
            {
                txtResponsavel.Enabled = true;
                txtResponsavel.BackColor = System.Drawing.Color.White;
                return;
            }

            bool maiorDeIdade = CalcularIdade(nascimento) >= 18;
            if (maiorDeIdade)
            {
                txtResponsavel.Clear();
                txtResponsavel.Enabled = false;
                txtResponsavel.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                txtResponsavel.Enabled = true;
                txtResponsavel.BackColor = System.Drawing.Color.White;
            }
        }

        private async void TxtCep_Leave(object sender, EventArgs e)
        {
            string cep = ExtrairDigitos(txtCep.Text);
            if (cep.Length == 8 && !consultandoCep)
                await ConsultarCepAsync(cep);
        }

        // Busca o endereço correspondente ao CEP informado
        private async System.Threading.Tasks.Task ConsultarCepAsync(string cep)
        {
            if (cep.Length != 8)
                return;

            consultandoCep = true;
            UseWaitCursor = true;
            try
            {
                ResultadoConsultaCep endereco = await consultaCepServico.ConsultarPorCepAsync(
                    cep, System.Threading.CancellationToken.None);
                if (!IsDisposed && endereco != null)
                {
                    txtCep.Text = FormatarCep(ExtrairDigitos(endereco.Cep ?? cep));
                    fichaEnderecoConsulta = endereco;
                }
            }
            catch (Exception erro)
            {
                if (!IsDisposed)
                    MessageBox.Show(this, "Não foi possível consultar o CEP. O cadastro poderá continuar sem o preenchimento automático do endereço.\r\n\r\n" + erro.Message,
                        "Consulta de CEP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (!IsDisposed)
                    UseWaitCursor = false;
                consultandoCep = false;
            }
        }

        // Evento de clique do botão de salvar, valida os campos e salva os dados do aluno
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                Avisar("Informe o nome completo do aluno.", "Novo aluno", txtNome);
                return;
            }

            string cpf = ExtrairDigitos(txtCpf.Text);
            string telefone = ExtrairDigitos(txtTelefone.Text);
            string cep = ExtrairDigitos(txtCep.Text);
            DateTime nascimento;
            if (!TentarObterDataNascimento(out nascimento))
            {
                Avisar("Informe uma data de nascimento válida no formato dd/MM/aaaa.", "Data inválida", txtDataNascimento);
                return;
            }

            bool maiorDeIdade = CalcularIdade(nascimento) >= 18;

            if (cpf.Length != 11)
            {
                Avisar("Informe os 11 números do CPF.", "CPF inválido", txtCpf);
                return;
            }
            if (!CpfValidador.EhValido(cpf))
            {
                Avisar("O CPF informado não é válido. Confira os dígitos verificadores.", "CPF inválido", txtCpf);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRg.Text) || txtRg.Text.Trim().Length > 14)
            {
                Avisar("Informe o RG (até 14 caracteres).", "RG inválido", txtRg);
                return;
            }
            if (cep.Length != 8)
            {
                Avisar("Informe os 8 números do CEP.", "CEP inválido", txtCep);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNumero.Text) || txtNumero.Text.Trim().Length > 5)
            {
                Avisar("Informe o número do endereço (até 5 caracteres).", "Endereço", txtNumero);
                return;
            }
            if (telefone.Length > 0 && telefone.Length != 10 && telefone.Length != 11)
            {
                Avisar("Informe um telefone completo com DDD ou deixe o campo vazio.", "Telefone inválido", txtTelefone);
                return;
            }

            // Se o aluno for menor de idade, o campo de responsável é obrigatório
            FichaAluno ficha = alunoExistente ?? new FichaAluno { DataCadastro = DateTime.Today };
            ficha.Nome = txtNome.Text.Trim();
            ficha.DataNascimento = nascimento.Date;
            ficha.Telefone = txtTelefone.Text.Trim();
            ficha.Numero = txtNumero.Text.Trim();
            ficha.Cep = cep.Substring(0, 5) + "-" + cep.Substring(5);
            ficha.Complemento = txtComplemento.Text.Trim();

            if (fichaEnderecoConsulta != null)
            {
                ficha.Logradouro = fichaEnderecoConsulta.Logradouro;
                ficha.Bairro = fichaEnderecoConsulta.Bairro;
                ficha.Cidade = fichaEnderecoConsulta.Cidade;
            }

            // Se o aluno for menor de idade, o campo de responsável é obrigatório
            ficha.Responsavel = maiorDeIdade ? string.Empty : txtResponsavel.Text.Trim();
            ficha.Cpf = txtCpf.Text.Trim();
            ficha.Rg = txtRg.Text.Trim();
            ficha.Email = txtEmail.Text.Trim();
            ficha.DiaVencimento = Convert.ToByte(nudDiaVencimento.Value);

            // Se o campo de responsável for obrigatório e estiver vazio, exibe um aviso
            string observacao = string.IsNullOrWhiteSpace(ficha.Responsavel)
                ? null
                : "Responsável: " + ficha.Responsavel.Trim();

            // Cria um objeto Aluno com os dados preenchidos no formulário
            Aluno aluno = new Aluno
            {
                IdAluno = ficha.IdAluno,
                Observacao = observacao,
                DataMatricula = ficha.DataCadastro == DateTime.MinValue ? DateTime.Today : ficha.DataCadastro,
                DiaVencimento = ficha.DiaVencimento,
                Situacao = "Ativo",
                Pessoa = new Pessoa
                {
                    Nome = ficha.Nome,
                    DataNascimento = ficha.DataNascimento,
                    IdEndereco = ficha.IdEndereco,
                    Cpf = cpf,
                    Rg = ficha.Rg,
                    Email = ficha.Email
                },
                Endereco = new Endereco
                {
                    Numero = ficha.Numero,
                    Cep = ficha.Cep,
                    Complemento = ficha.Complemento
                },
                Telefone = new Telefone { NumeroTelefone = telefone }
            };

            // Tenta salvar o aluno no banco de dados e trata possíveis erros
            try
            {
                alunoServico.Salvar(aluno);
                ficha.IdAluno = aluno.IdAluno;
                ficha.DataCadastro = ficha.DataCadastro == DateTime.MinValue ? DateTime.Today : ficha.DataCadastro;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(this,
                    "Não foi possível salvar o aluno no banco de dados.\r\n\r\n" + erro.Message,
                    "Falha no cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Exibe uma mensagem de aviso para o usuário e foca no campo especificado com erro
        private void Avisar(string mensagem, string titulo, Control campo)
        {
            MessageBox.Show(this, mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            campo.Focus();
        }

        // Calcula a idade com base na data de nascimento fornecida
        private static int CalcularIdade(DateTime nascimento)
        {
            DateTime hoje = DateTime.Today;
            int idade = hoje.Year - nascimento.Year;
            if (nascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }

        // Aplica a máscara enquanto o usuário digita
        private static void ConfigurarFormatacaoProgressiva(TextBox campo, Func<string, string> formatador, int limite)
        {
            bool atualizando = false;
            campo.TextChanged += delegate
            {
                if (atualizando) return;
                int posicao = Math.Min(campo.SelectionStart, campo.Text.Length);
                int digitosAntes = ExtrairDigitos(campo.Text.Substring(0, posicao)).Length;
                string digitos = ExtrairDigitos(campo.Text);
                if (digitos.Length > limite) digitos = digitos.Substring(0, limite);
                string textoFormatado = formatador(digitos);
                if (campo.Text == textoFormatado) return;
                atualizando = true;
                try
                {
                    campo.Text = textoFormatado;
                    campo.SelectionStart = PosicaoAposDigitos(textoFormatado, digitosAntes);
                    campo.SelectionLength = 0;
                }
                finally { atualizando = false; }
            };
        }

        // Aplica a pontuação padrão do RG durante a digitação
        private static void ConfigurarFormatacaoRg(TextBox campo)
        {
            bool atualizando = false;
            campo.TextChanged += delegate
            {
                if (atualizando) return;
                int posicao = Math.Min(campo.SelectionStart, campo.Text.Length);
                int caracteresAntes = ExtrairRg(campo.Text.Substring(0, posicao)).Length;
                string caracteres = ExtrairRg(campo.Text);
                if (caracteres.Length > 9) caracteres = caracteres.Substring(0, 9);
                string formatado = FormatarRg(caracteres);
                if (campo.Text == formatado) return;
                atualizando = true;
                try
                {
                    campo.Text = formatado;
                    campo.SelectionStart = PosicaoAposCaracteresRg(formatado, caracteresAntes);
                    campo.SelectionLength = 0;
                }
                finally { atualizando = false; }
            };
        }

        private static string ExtrairRg(string texto)
        {
            return Regex.Replace(texto ?? string.Empty, "[^0-9Xx]", string.Empty).ToUpperInvariant();
        }

        // Organiza os dígitos do RG no formato exibido pelo cadastro
        private static string FormatarRg(string caracteres)
        {
            if (caracteres.Length <= 2) return caracteres;
            if (caracteres.Length <= 5) return caracteres.Substring(0, 2) + "." + caracteres.Substring(2);
            if (caracteres.Length <= 8) return caracteres.Substring(0, 2) + "." + caracteres.Substring(2, 3) + "." + caracteres.Substring(5);
            return caracteres.Substring(0, 2) + "." + caracteres.Substring(2, 3) + "." + caracteres.Substring(5, 3) + "-" + caracteres.Substring(8);
        }

        private static int PosicaoAposCaracteresRg(string texto, int quantidade)
        {
            if (quantidade <= 0) return 0;
            int encontrados = 0;
            for (int i = 0; i < texto.Length; i++)
                if (char.IsLetterOrDigit(texto[i]) && ++encontrados == quantidade) return i + 1;
            return texto.Length;
        }

        // Formata a data de nascimento no mesmo padrão usado no cadastro de professores
        private static string FormatarData(string digitos)
        {
            if (digitos.Length <= 2) return digitos;
            if (digitos.Length <= 4) return digitos.Substring(0, 2) + "/" + digitos.Substring(2);
            return digitos.Substring(0, 2) + "/" + digitos.Substring(2, 2) + "/" + digitos.Substring(4);
        }

        private bool TentarObterDataNascimento(out DateTime nascimento)
        {
            nascimento = DateTime.MinValue;
            string digitos = ExtrairDigitos(txtDataNascimento.Text);
            if (digitos.Length != 8 ||
                !DateTime.TryParseExact(digitos, "ddMMyyyy", new CultureInfo("pt-BR"),
                    DateTimeStyles.None, out nascimento))
                return false;

            return nascimento.Date <= DateTime.Today;
        }

        // Formata o número de telefone com DDD e traço
        private static string FormatarTelefone(string digitos)
        {
            if (digitos.Length <= 2) return digitos;
            string ddd = digitos.Substring(0, 2);
            string numero = digitos.Substring(2);
            string prefixo = "(" + ddd + ") ";
            int prefixoNumero = numero.StartsWith("9", StringComparison.Ordinal) ? 5 : 4;
            return numero.Length <= prefixoNumero
                ? prefixo + numero
                : prefixo + numero.Substring(0, prefixoNumero) + "-" + numero.Substring(prefixoNumero);
        }

        // Formata o CEP com traço
        private static string FormatarCep(string digitos)
        {
            return digitos.Length <= 5 ? digitos : digitos.Substring(0, 5) + "-" + digitos.Substring(5);
        }

        // Formata o CPF com pontos e traço
        private static string FormatarCpf(string digitos)
        {
            if (digitos.Length <= 3) return digitos;
            if (digitos.Length <= 6) return digitos.Substring(0, 3) + "." + digitos.Substring(3);
            if (digitos.Length <= 9) return digitos.Substring(0, 3) + "." + digitos.Substring(3, 3) + "." + digitos.Substring(6);
            return digitos.Substring(0, 3) + "." + digitos.Substring(3, 3) + "." + digitos.Substring(6, 3) + "-" + digitos.Substring(9);
        }

        // Extrai apenas os dígitos de uma string, removendo quaisquer caracteres não numéricos
        private static string ExtrairDigitos(string texto)
        {
            return Regex.Replace(texto ?? string.Empty, @"\D", string.Empty);
        }

        private static int PosicaoAposDigitos(string texto, int quantidade)
        {
            if (quantidade <= 0) return 0;
            int encontrados = 0;
            for (int i = 0; i < texto.Length; i++)
                if (char.IsDigit(texto[i]) && ++encontrados == quantidade) return i + 1;
            return texto.Length;
        }
    }
}
