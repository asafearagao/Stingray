
using System;
using System.Globalization;
using System.Windows.Forms;
using Stingray.Modelos;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class FichaAlunoForm : Form
    {
        // Define a "cultura" utilizada na formatação das datas
        private static readonly CultureInfo CulturaBrasileira = new CultureInfo("pt-BR");
        private readonly Aluno aluno; // Armazena os dados do aluno exibido na ficha

        public FichaAlunoForm() : this(new Aluno()) { } // Construtor padrão vazio

        public FichaAlunoForm(Aluno aluno)  // Inicializa a ficha utilizando os dados 
        {
            if (aluno == null)              // Impede a inicialização com uma referência nula
                throw new ArgumentNullException(nameof(aluno)); 

            this.aluno = aluno;             // Armazena a referência recebido para uso posterior

            InitializeComponent();          // Inicializa os componentes visuais do formulário
            PreencherInformacoesDoAluno();  // Preenche a ficha com os dados do aluno
        }

        private void PreencherInformacoesDoAluno()  // Preenchimento das informações
        {
            PreencherDadosPessoais();               // Preenche os dados pessoais
            PreencherEndereco();                    // Preenche as informações de endereço
            PreencherDocumentos();                  // Preenche os documentos e os dados de contato

            rotuloDataCadastro.Text = FormatarDataDeCadastro(aluno.DataCadastro); // Exibe a data de cadastro formatada
        }

        private void PreencherDadosPessoais()                                 // Preenche as informações pessoais do aluno
        {
            valorNome.Text = ObterValorOuTraco(aluno.Nome);                   // Exibe o nome ou um traço caso esteja vazio

            valorNascimento.Text = aluno.DataNascimento == DateTime.MinValue  // Verifica se existe uma data de nascimento
                ? "—"
                : aluno.DataNascimento.ToString("dd/MM/yyyy");                // Formata a data de nascimento

            valorTelefone.Text = ObterValorOuTraco(aluno.Telefone);           // Exibe o telefone ou um traço
            valorResponsavel.Text = ObterValorOuTraco(aluno.Responsavel);     // Exibe o responsável ou um traço
        }

        private void PreencherEndereco()                         // Preenche as informações de endereço do aluno
        {
            valorNumero.Text = ObterValorOuTraco(aluno.Numero);  // Exibe o número ou um traço
            valorCep.Text = ObterValorOuTraco(aluno.Cep);        // Exibe o CEP ou um traço
            valorCidade.Text = ObterValorOuTraco(aluno.Cidade);  // Exibe a cidade ou um traço
        }

        private void PreencherDocumentos()                       // Preenche os documentos e as informações de contato
        {
            valorCpf.Text = ObterValorOuTraco(aluno.Cpf);        // Exibe o CPF ou um traço
            valorRg.Text = ObterValorOuTraco(aluno.Rg);          // Exibe o RG ou um traço
            valorEmail.Text = ObterValorOuTraco(aluno.Email);    // Exibe o e-mail ou um traço
        }


        private static string ObterValorOuTraco(string valor) // Verifica se o conteúdo é nulo, vazio ou espaços
        {
            return string.IsNullOrWhiteSpace(valor)
                ? "—"
                : valor; 
        }

        private static string FormatarDataDeCadastro(DateTime dataCadastro) // Formata a data de cadastro para exibição na ficha
        {
            if (dataCadastro == DateTime.MinValue)                          // Verifica se existe uma data de cadastro inválida
                return string.Empty;

            // Retorna a data por extenso utilizando a "cultura brasileira"
            return "Campinas, " + dataCadastro.ToString("d 'de' MMMM 'de' yyyy", CulturaBrasileira) + ".";
        }
    }
}