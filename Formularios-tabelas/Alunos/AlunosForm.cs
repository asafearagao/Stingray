
using System;
using System.Windows.Forms;
using Stingray.Formularios.Alunos;
using Stingray.Formularios.Alunos.Componentes;
using Stingray.Modelos;
using Stingray.Interface;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class AlunosForm : Form
    {
        private AlunosFiltro filtros;  // Armazena os filtros utilizados na consulta
        private AlunosLista lista;     // Armazena os registros exibidos na tabela
        private AlunosAcoes acoes;     // Armazena as operações disponíveis para cada aluno

        // Inicializa a tela de alunos e configura filtros, lista e ações
        public AlunosForm()
        {
            InitializeComponent();  // Inicializa os componentes visuais do formulário
            ConfigurarTela();       // Configura os elementos da tela
        }

        // Coordena a configuração inicial dos componentes da tela de alunos
        private void ConfigurarTela()
        {
            ConfigurarPainelDeConteudo(); // Configura a aparência do painel principal

            rotuloTitulo.Text = "Alunos"; // Define o título da tela

            // Verifica se o formulário está sendo exibido no Designer
            bool emModoDesign = DesignModeHelper.IsDesignMode(this); 

            ConfigurarFiltros(emModoDesign);  // Inicializa os filtros de pesquisa
            ConfigurarListaDeAlunos();        // Inicializa a tabela de alunos
            ConfigurarAcoes();                // Inicializa as operações disponíveis

            CarregarDadosIniciais();          // Carrega os todos os registros da tabela

            ConfigurarComportamentoInicial(emModoDesign);
        }

        // Configura o painel principal que recebe o conteúdo da tela
        private void ConfigurarPainelDeConteudo()
        {
            painelConteudo.Paint += AoDesenharBordaDoConteudo;   // Desenho personalizado da borda
            painelConteudo.BackColor = VisualPadrao.FundoPagina; // Define a cor de fundo do painel
        }

        // Define o comportamento inicial da tela conforme o modo de execução
        private void ConfigurarComportamentoInicial(bool emModoDesign)
        {
            if (emModoDesign) // Verifica se o formulário está sendo exibido no Designer
            {
                // Exibe a quantidade de registros totais
                rotuloResumo.Text = "Mostrando " + gradeDeAlunos.Rows.Count + " alunos";
                return;
            }

            botaoNovoAluno.Click += AoClicarEmNovoAluno; // Associa o evento de cadastro

            AplicarFiltros();
        }

        // Inicializa os campos e as opções de filtragem dos alunos
        private void ConfigurarFiltros(bool emModoDesign)
        {
            filtros = new AlunosFiltro(
                campoBusca,
                listaFiltroTurma,
                listaFiltroProfessor,
                listaFiltroStatus,
                botaoSetaFiltroTurma,
                botaoSetaFiltroProfessor,
                botaoSetaFiltroStatus,
                botaoPesquisar,
                botaoLimpar);

            filtros.Configurar(emModoDesign);
            filtros.Alterado += AoAlterarFiltros;  // Associa o evento de alteração dos filtros
        }

        // Inicializa a grade que exibe os alunos cadastrados
        private void ConfigurarListaDeAlunos()
        {
            lista = new AlunosLista(gradeDeAlunos); // Cria o gerenciador utilizando a tabela da tela

            AlunosGrade.Configurar(gradeDeAlunos, AoExecutarAcaoNaGrade);
        }

        // Conecta as ações disponíveis para cada aluno
        private void ConfigurarAcoes()
        {
            // Cria o gerenciador de ações associado ao formulário e à lista
            acoes = new AlunosAcoes(this, lista, AplicarFiltros); 
        }

        // Carrega os dados iniciais
        private void CarregarDadosIniciais()
        {
            lista.CarregarDadosDemonstracao();
        }

        // Aplica os valores atuais dos filtros à lista de alunos
        private void AplicarFiltros()
        {
            filtros.Aplicar(gradeDeAlunos, rotuloResumo);
        }

        // Desenha a borda do painel principal de conteúdo
        private void AoDesenharBordaDoConteudo(object remetente, PaintEventArgs argumentos)
        {
            VisualPadrao.DesenharBordaCartao(argumentos.Graphics, painelConteudo.ClientRectangle);
        }

        // Trata a alteração de filtros
        private void AoAlterarFiltros(object remetente, EventArgs argumentos)
        {
            AplicarFiltros();
        }

        // Abre o formulário para cadastrar um novo aluno
        private void AoClicarEmNovoAluno(object remetente, EventArgs argumentos)
        {
            acoes.AbrirCadastro(null);
        }

        // Encaminha a ação selecionada na grade para o gerenciador de ações
        private void AoExecutarAcaoNaGrade(AcaoAlunoNaGrade acao, Aluno aluno)
        {
            acoes.Executar(acao, aluno); // Encaminha a operação para o gerenciador de ações
        }
    }
}