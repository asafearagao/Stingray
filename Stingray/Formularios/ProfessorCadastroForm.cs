using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Stingray.Entidades;
using Stingray.Servicos;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Controla o cadastro e a edição dos dados de um professor
    public partial class ProfessorCadastroForm : Form
    {
        private readonly Professor professorAtual;
        private readonly ProfessorServico professorServico = new ProfessorServico();

        public ProfessorCadastroForm()
        {
            InitializeComponent();
            EstiloPopup.Aplicar(this);
        }

        // Construtor que recebe um professor existente para edição
        public ProfessorCadastroForm(Professor professor) : this()
        {
            professorAtual = professor;

            bool editando = professorAtual != null;

            // Define os textos do formulário com base na ação (novo ou editar)
            Text = editando ? "Editar professor" : "Novo professor";
            lblTitulo.Text = editando ? "Editar professor" : "Adicionar professor";
            btnSalvar.Text = editando ? "Salvar alterações" : "Adicionar";

            ConfigurarCampos();
            if (editando)
                PreencherCampos(professorAtual);

            btnSalvar.Click += BtnSalvar_Click;
            btnCancelar.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            Shown += delegate { txtNome.Focus(); };
        }

        private void ConfigurarCampos()
        {
            ConfigurarFormatacaoProgressiva(txtCpf, FormatarCpf, 11);
            ConfigurarFormatacaoProgressiva(txtCep, FormatarCep, 8);
            ConfigurarFormatacaoProgressiva(txtDataNascimento, FormatarData, 8);
            ConfigurarFormatacaoProgressiva(txtTelefone, FormatarTelefone, 11);
            ConfigurarFormatacaoRg(txtRg);
            txtRg.MaxLength = 12;
        }

        // Preenche os campos do formulário com os dados do professor existentes
        private void PreencherCampos(Professor professor)
        {
            txtNome.Text = professor.Pessoa.Nome ?? string.Empty;
            txtCpf.Text = ExtrairDigitos(professor.Pessoa.Cpf);
            txtRg.Text = professor.Pessoa.Rg ?? string.Empty;
            txtCref.Text = professor.Cref ?? string.Empty;
            txtTelefone.Text = professor.Telefone == null
                ? string.Empty
                : ExtrairDigitos(professor.Telefone.NumeroTelefone);
            txtDataNascimento.Text = professor.Pessoa.DataNascimento.ToString("ddMMyyyy");
            txtCep.Text = professor.Endereco == null
                ? string.Empty
                : ExtrairDigitos(professor.Endereco.Cep);
            txtNumero.Text = professor.Endereco == null ? string.Empty : professor.Endereco.Numero;
            txtComplemento.Text = professor.Endereco == null
                ? string.Empty
                : professor.Endereco.Complemento ?? string.Empty;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            string cpf = ExtrairDigitos(txtCpf.Text);
            string cep = ExtrairDigitos(txtCep.Text);
            string telefone = ExtrairDigitos(txtTelefone.Text);
            string data = ExtrairDigitos(txtDataNascimento.Text);

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                Avisar("Informe o nome do professor.", "Cadastro de professor", txtNome);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCref.Text) || txtCref.Text.Trim().Length > 255)
            {
                Avisar("Informe o CREF (até 255 caracteres).", "Cadastro de professor", txtCref);
                return;
            }
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
            if (data.Length != 8)
            {
                Avisar("Informe a data de nascimento completa.", "Data inválida", txtDataNascimento);
                return;
            }

            DateTime nascimento;
            if (!DateTime.TryParseExact(data, "ddMMyyyy", new CultureInfo("pt-BR"),
                DateTimeStyles.None, out nascimento))
            {
                Avisar("Informe uma data válida no formato dd/MM/aaaa.", "Data inválida", txtDataNascimento);
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

            // Cria um objeto Professor com os dados do formulário
            Professor professor = new Professor
            {
                IdProfessor = professorAtual == null ? 0 : professorAtual.IdProfessor,
                Cref = txtCref.Text.Trim(),
                Pessoa = new Pessoa
                {
                    Nome = txtNome.Text.Trim(),
                    DataNascimento = nascimento,
                    Cpf = cpf,
                    Rg = txtRg.Text.Trim(),
                    Email = professorAtual == null ? null : professorAtual.Pessoa.Email
                },
                Endereco = new Endereco
                {
                    Numero = txtNumero.Text.Trim(),
                    Cep = cep.Substring(0, 5) + "-" + cep.Substring(5),
                    Complemento = txtComplemento.Text.Trim()
                },
                Telefone = new Telefone { NumeroTelefone = telefone }
            };

            try
            {
                professorServico.Salvar(professor);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(this,
                    "Não foi possível salvar o professor no banco de dados.\n\n" + erro.Message,
                    "Falha no cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Avisar(string mensagem, string titulo, Control campo)
        {
            MessageBox.Show(this, mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            campo.Focus();
        }

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
                string formatado = formatador(digitos);
                if (campo.Text == formatado) return;
                atualizando = true;
                try
                {
                    campo.Text = formatado;
                    campo.SelectionStart = PosicaoAposDigitos(formatado, digitosAntes);
                    campo.SelectionLength = 0;
                }
                finally { atualizando = false; }
            };
        }

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
            return System.Text.RegularExpressions.Regex.Replace(texto ?? string.Empty, "[^0-9Xx]", string.Empty).ToUpperInvariant();
        }

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

        private static string FormatarCpf(string digitos)
        {
            if (digitos.Length <= 3) return digitos;
            if (digitos.Length <= 6) return digitos.Substring(0, 3) + "." + digitos.Substring(3);
            if (digitos.Length <= 9) return digitos.Substring(0, 3) + "." + digitos.Substring(3, 3) + "." + digitos.Substring(6);
            return digitos.Substring(0, 3) + "." + digitos.Substring(3, 3) + "." + digitos.Substring(6, 3) + "-" + digitos.Substring(9);
        }

        private static string FormatarCep(string digitos)
        {
            return digitos.Length <= 5 ? digitos : digitos.Substring(0, 5) + "-" + digitos.Substring(5);
        }

        private static string FormatarData(string digitos)
        {
            if (digitos.Length <= 2) return digitos;
            if (digitos.Length <= 4) return digitos.Substring(0, 2) + "/" + digitos.Substring(2);
            return digitos.Substring(0, 2) + "/" + digitos.Substring(2, 2) + "/" + digitos.Substring(4);
        }

        private static string FormatarTelefone(string digitos)
        {
            if (digitos.Length <= 2) return digitos;
            string ddd = digitos.Substring(0, 2);
            string numero = digitos.Substring(2);
            string prefixo = "(" + ddd + ") ";
            int tamanhoPrefixo = numero.StartsWith("9", StringComparison.Ordinal) ? 5 : 4;
            return numero.Length <= tamanhoPrefixo
                ? prefixo + numero
                : prefixo + numero.Substring(0, tamanhoPrefixo) + "-" + numero.Substring(tamanhoPrefixo);
        }

        private static string ExtrairDigitos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            StringBuilder resultado = new StringBuilder();
            foreach (char caractere in texto)
                if (char.IsDigit(caractere)) resultado.Append(caractere);
            return resultado.ToString();
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
