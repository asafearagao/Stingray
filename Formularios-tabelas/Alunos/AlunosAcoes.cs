
using System;
using System.Windows.Forms;
using Stingray;
using Stingray.Modelos;
using Stingray.Formularios.Alunos.Componentes;
using Stingray.Formularios.Alunos.DadosDemonstracao;
using Stingray.Formularios.Alunos.Modelos;

namespace Stingray.Formularios.Alunos
{
    internal sealed class AlunosAcoes
    {
        private readonly IWin32Window janelaPrincipal;  // Armazena a janela utilizada como referência
        private readonly AlunosLista lista;             // Armazena os alunos exibidos na tabela
        private readonly Action aplicarFiltros;         // Armazena a ação por atualizar os filtros

        // Inicializa o gerenciador de ações dos alunos
        public AlunosAcoes(IWin32Window janelaPrincipal, AlunosLista lista, Action aplicarFiltros)
        {
            if (janelaPrincipal == null)  // Verifica se a janela principal foi informada
                throw new ArgumentNullException(nameof(janelaPrincipal));

            if (lista == null)            // Verifica se o gerenciador da lista foi informado
                throw new ArgumentNullException(nameof(lista));

            if (aplicarFiltros == null)   // Verifica se a ação de atualização dos filtros foi informada
                throw new ArgumentNullException(nameof(aplicarFiltros));

            this.janelaPrincipal = janelaPrincipal;  // Armazena a referência da janela principal
            this.lista = lista;                      // Armazena a referência do gerenciador da lista
            this.aplicarFiltros = aplicarFiltros;    // Armazena a ação de atualização dos filtros
        }


        // Executa a ação selecionada para o aluno informado
        public void Executar(AcaoAlunoNaGrade acao, Aluno aluno)
        {
            switch (acao)                      // Verifica qual ação foi solicitada na tabela
            {
                case AcaoAlunoNaGrade.AbrirFicha:
                    AbrirFicha(aluno);         // Abre a ficha com os dados do aluno
                    break;

                case AcaoAlunoNaGrade.Editar:
                    AbrirCadastro(aluno);      // Abre o formulário de edição do aluno
                    break;

                case AcaoAlunoNaGrade.Excluir:
                    ConfirmarExclusao(aluno);  // Solicita a confirmação antes de excluir o aluno
                    break;
            }
        }

        public void AbrirCadastro(Aluno alunoExistente) // Abre o formulário para cadastrar ou editar um aluno
        {
            // Cria o formulário utilizando o aluno existente, caso seja uma edição
            using (CadastroAlunoForm formulario = new CadastroAlunoForm(alunoExistente))
            {
                // Verifica se o formulário foi criado corretamente
                if (formulario.ShowDialog(janelaPrincipal) != DialogResult.OK) 
                    return;

                Aluno alunoPreenchido = formulario.AlunoPreenchido; // Obtém os dados preenchidos no formulário

                if (alunoPreenchido == null) // Verifica se o formulário retornou um aluno válido
                    return;

                SalvarNaLista(alunoExistente, alunoPreenchido);
                aplicarFiltros();
            }
        }

        // Salva na lista
        private void SalvarNaLista(Aluno alunoExistente, Aluno alunoPreenchido)
        {
            if (alunoExistente == null) // Verifica se está sendo realizado um novo cadastro
            {
                lista.Adicionar(new AlunoListaItem(
                    alunoPreenchido,
                    "—",
                    "—",
                    "Mensal",
                    "Ativa")); // Adiciona valores iniciais de turma, professor, plano e status

                return;
            }

            lista.Atualizar(alunoPreenchido); // Atualiza os dados da tabela
        }

        // Abre a ficha do aluno selecionado para visualização
        private void AbrirFicha(Aluno aluno)
        {
            using (FichaAlunoForm formulario = new FichaAlunoForm(aluno)) // Cria o formulário com os dados do aluno
                formulario.ShowDialog(janelaPrincipal); // Exibe a ficha vinculada à janela principal
        }

        // Solicita a confirmação antes de excluir o aluno selecionado
        private void ConfirmarExclusao(Aluno aluno)
        {
            DataGridViewRow linha = lista.Encontrar(aluno); // Localiza a linha correspondente ao aluno

            if (linha == null) // Verifica se o aluno não foi encontrado na tabela
                return;

            string nome = lista.ObterNome(linha); // Obtém o nome do aluno para utilizar na mensagem de confirmação

            DialogResult resposta = MessageBox.Show(
                janelaPrincipal,
                "Deseja realmente excluir " + nome + "?",
                "Excluir aluno",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning); // Exibe a confirmação de exclusão

            if (resposta != DialogResult.Yes) // Verifica se o usuário não confirmou a exclusão
                return;

            lista.Excluir(linha);  // Remove o aluno da tabela
            aplicarFiltros();      // Atualiza a tabela após a exclusão
        }
    }
}