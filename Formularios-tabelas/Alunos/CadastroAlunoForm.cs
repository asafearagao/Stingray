
using System;
using System.Windows.Forms;
using Stingray.Modelos;
using Stingray.Interface;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class CadastroAlunoForm : Form
    {
        private readonly Aluno alunoOriginal;              // Armazena o aluno recebido para edição
        public Aluno AlunoPreenchido { get; private set; } // Disponibiliza os dados preenchidos após a confirmação

        public CadastroAlunoForm() : this(null) { } // Construtor padrão vazio

        public CadastroAlunoForm(Aluno aluno) // Inicializa o formulário para cadastrar ou editar um aluno
        {
            InitializeComponent();            // Inicializa os componentes visuais do formulário

            alunoOriginal = aluno;            // Armazena o aluno recebido

            ConfigurarFormulario();           // Atualiza a linha do formulário
            PreencherCampos();                // Preenche com os novos dados
        }

        // Configura o formulário
        private void ConfigurarFormulario()
        {
            bool criandoAluno = alunoOriginal == null; // Verifica se adicionando ou editando um aluno

            Text = criandoAluno                        // Define o título da janela
                ? "Novo aluno"
                : "Editar aluno"; 

            rotuloTitulo.Text = criandoAluno           // Define o título do formulário
                ? "Novo aluno"
                : "Editar aluno"; 

            botaoSalvar.Text = criandoAluno            // Define o texto do botão de salvamento
                ? "Salvar aluno"
                : "Salvar alterações"; 

            seletorDataNascimento.MaxDate = DateTime.Today; // Impede a seleção de datas futuras

            ConfigurarPlaceholders(); // Define os textos de placeholders

            seletorDataNascimento.ValueChanged += AoAlterarDataDeNascimento; // Atualiza o responsável se for de menor
            botaoSalvar.Click += AoClicarEmSalvar;     // Associa o evento de salvamento ao botão
            botaoCancelar.Click += AoClicarEmCancelar; // Associa o evento de cancelamento ao botão
            Shown += AoExibirFormulario;               // Executa uma ação quando o formulário for exibido

            AtualizarCampoResponsavel();               // Ajusta a disponibilidade do campo de responsável
        }

        private void ConfigurarPlaceholders() // Define os textos de orientação dos campos
        {
            PlaceholderTexto.Aplicar(campoNome, "Nome completo do aluno");
            PlaceholderTexto.Aplicar(campoNumero, "Número");
            PlaceholderTexto.Aplicar(campoCidade, "Cidade");
            PlaceholderTexto.Aplicar(campoResponsavel, "Nome do responsável");
            PlaceholderTexto.Aplicar(campoRg, "RG");
            PlaceholderTexto.Aplicar(campoEmail, "exemplo@email.com");
        }


        // Preenche os campos
        private void PreencherCampos()
        {
            if (alunoOriginal == null)    // Verifica se existe um aluno para edição
                return;

            PreencherDadosPessoais();     // Preenche os dados pessoais
            PreencherEndereco();          // Preenche as informações de endereço
            PreencherDocumentos();        // Preenche os documentos e os dados de contato

            AtualizarCampoResponsavel();  // Atualiza a disponibilidade do campo de responsável
        }

        // Preenche os dados pessoais
        private void PreencherDadosPessoais()
        {
            campoNome.Text = alunoOriginal.Nome ?? string.Empty; // Preenche o nome ou utiliza um texto vazio

            // Define uma data de nascimento dentro dos limites permitidos
            seletorDataNascimento.Value = AjustarDataDeNascimento(alunoOriginal.DataNascimento);

            campoTelefone.Text = alunoOriginal.Telefone ?? string.Empty;        // Preenche o telefone
            campoResponsavel.Text = alunoOriginal.Responsavel ?? string.Empty;  // Preenche o nome do responsável
        }

        // Preenche endereço
        private void PreencherEndereco()
        {
            campoNumero.Text = alunoOriginal.Numero ?? string.Empty;  // Preenche o número
            campoCep.Text = alunoOriginal.Cep ?? string.Empty;        // Preenche o CEP
            campoCidade.Text = alunoOriginal.Cidade ?? string.Empty;  // Preenche a cidade
        }

        // Preenche documentos
        private void PreencherDocumentos()
        {
            campoCpf.Text = alunoOriginal.Cpf ?? string.Empty;      // Preenche o CPF
            campoRg.Text = alunoOriginal.Rg ?? string.Empty;        // Preenche o RG
            campoEmail.Text = alunoOriginal.Email ?? string.Empty;  // Preenche o e-mail
        }

        // Trata a alteração de data de nascimento
        private void AoAlterarDataDeNascimento(object remetente, EventArgs argumentos)
        {
            AtualizarCampoResponsavel(); // Atualiza o campo de responsável conforme a idade
        }

        // Atualiza o campo de responsável conforme a idade do aluno
        private void AtualizarCampoResponsavel()
        {
            bool menorDeIdade = EhMenorDeIdade();     // Verifica se o aluno possui menos de 18 anos

            campoResponsavel.Enabled = menorDeIdade;  // Habilita o campo somente para menores de idade

            if (!menorDeIdade)                        // Verifica se o aluno é maior de idade
                campoResponsavel.Clear();             // Limpa o campo de responsável
        }

        private bool EhMenorDeIdade() // Verifica se o aluno possui é menor de idade
        {
            return CalcularIdade(seletorDataNascimento.Value) < 18;
        }

        private static int CalcularIdade(DateTime dataNascimento)  // Calcula a idade do aluno
        {
            DateTime hoje = DateTime.Today;                        // Obtém a data atual

            int idade = hoje.Year - dataNascimento.Year;           // Calcula a diferença entre os anos

            if (dataNascimento.Date > hoje.AddYears(-idade))       // Verifica ainda não fez aniversário este ano
                idade--;                                           // Desconta um ano da idade calculada

            return idade;
        }

        // Ajusta data de nascimento
        private DateTime AjustarDataDeNascimento(DateTime dataNascimento)
        {
            // Verifica se a data está fora do intervalo permitido
            if (dataNascimento < seletorDataNascimento.MinDate || dataNascimento > seletorDataNascimento.MaxDate)
                return DateTime.Today; // Utiliza a data atual como alternativa

            return dataNascimento;
        }

        // Valida e salva os dados informados no formulário
        private void AoClicarEmSalvar(object remetente, EventArgs argumentos)
        {
            if (!ValidarNome())              // Verifica se o nome obrigatório foi informado
                return;

            SalvarDadosDoAluno();            // Prepara e armazena os dados preenchidos

            DialogResult = DialogResult.OK;  // Informa que o formulário foi concluído com sucesso
            Close();                         // Fecha o formulário
        }

        // Valida nome
        private bool ValidarNome()
        {
            if (!string.IsNullOrWhiteSpace(campoNome.Text)) // Verifica se o campo contém algum conteúdo válido
                return true;

            MessageBox.Show(
                this,
                "Informe o nome completo do aluno.",
                "Novo aluno",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning); // Exibe um aviso de erro

            campoNome.Focus();           // Posiciona o cursor no campo de nome

            return false;
        }

        // Salva os dados do aluno
        private void SalvarDadosDoAluno()
        {
            PrepararAlunoParaEdicao();  // Define a instância requisitos

            SalvarDadosPessoais();      // Salva as informações pessoais
            SalvarEndereco();           // Salva as informações de endereço
            SalvarDocumentos();         // Salva os documentos e os dados de contato
        }

        // Prepara o aluno para edição
        private void PrepararAlunoParaEdicao()
        {
            // Reutiliza o aluno existente ou cria um novo com a data de cadastro atual
            AlunoPreenchido = alunoOriginal ?? new Aluno { DataCadastro = DateTime.Today }; 
        }

        // Salva os dados pessoais
        private void SalvarDadosPessoais()
        {
            AlunoPreenchido.Nome = campoNome.Text.Trim();                       // Salva o nome sem espaços nas extremidades
            AlunoPreenchido.DataNascimento = seletorDataNascimento.Value.Date;  // Salva a data de nascimento sem o horário
            AlunoPreenchido.Telefone = campoTelefone.Text.Trim();               // Salva o telefone sem espaços nas extremidades

            AlunoPreenchido.Responsavel = EhMenorDeIdade() // Verifica se o campo de responsável deve ser salvo
                ? campoResponsavel.Text.Trim()
                : string.Empty;
        }

        // Salva endereço
        private void SalvarEndereco()
        {
            AlunoPreenchido.Numero = campoNumero.Text.Trim();  // Salva o número
            AlunoPreenchido.Cep = campoCep.Text.Trim();        // Salva o CEP
            AlunoPreenchido.Cidade = campoCidade.Text.Trim();  // Salva a cidade
        }

        // Salva documentos
        private void SalvarDocumentos()
        {
            AlunoPreenchido.Cpf = campoCpf.Text.Trim();      // Salva o CPF
            AlunoPreenchido.Rg = campoRg.Text.Trim();        // Salva o RG
            AlunoPreenchido.Email = campoEmail.Text.Trim();  // Salva o e-mail
        }


        // Cancela a operação atual e fecha o formulário
        private void AoClicarEmCancelar(object remetente, EventArgs argumentos)
        {
            DialogResult = DialogResult.Cancel;  // Informa que a operação foi cancelada
            Close();                             // Fecha o formulário
        }

        // Ajusta os dados e o estado visual quando o formulário é exibido
        private void AoExibirFormulario(object remetente, EventArgs argumentos)
        {
            campoNome.Focus(); // Posiciona o cursor inicialmente no campo de nome
        }
    }
}