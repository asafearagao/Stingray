
using System;
using System.Windows.Forms;
using Stingray.Modelos;
using Stingray.Formularios.Alunos.DadosDemonstracao;
using Stingray.Formularios.Alunos.Modelos;

namespace Stingray.Formularios.Alunos
{
    internal sealed class AlunosLista
    {
        private readonly DataGridView grade;                    // Armazena a tabela que exibi os alunos

        public AlunosLista(DataGridView grade)                  // Construtor do gerenciador da lista de alunos
        {
            if (grade == null)                                  // Verifica se a tabela foi informada
                throw new ArgumentNullException(nameof(grade)); // Impede a inicialização sem uma tabela válida

            this.grade = grade;                                 // Armazena a referência da tabela
        }

        public int QuantidadeDeLinhas                           // Retorna a quantidade de registros da tabela
        {
            get { return grade.Rows.Count; }
        }

        // Carrega os dados demonstração
        public void CarregarDadosDemonstracao()
        {
            // Percorre os alunos de demonstração/teste
            foreach (AlunoListaItem item in AlunoDadosDemonstracao.CriarAlunos()) 
                Adicionar(item);     // Adiciona cada aluno à tabela

            grade.ClearSelection();  // Remove qualquer seleção após o carregamento
        }

        public void Adicionar(AlunoListaItem item)              // Adiciona um novo aluno à tabela
        {
            if (item == null)                                   // Impede um registro nulo
                throw new ArgumentNullException(nameof(item)); 

            int indice = grade.Rows.Add(                        // Adiciona uma linha com as informações
                item.Aluno.DataCadastro.ToString("dd/MM/yyyy"),
                item.Aluno.Nome,
                item.Turma,
                item.Professor,
                item.Plano,
                item.Status);

            // Armazena o objeto do aluno na Tag da linha
            grade.Rows[indice].Tag = item.Aluno;
        }

        // Atualiza os dados do aluno na linha correspondente da lista
        public void Atualizar(Aluno aluno)
        {
            DataGridViewRow linha = Encontrar(aluno); // Localiza a linha do aluno

            if (linha == null)                        // Verifica se o aluno não foi encontrado
                return;

            linha.Tag = aluno;                                                           // Atualiza a referência do objeto
            linha.Cells["colunaAluno"].Value = aluno.Nome;                               // Atualiza o nome do aluno
            linha.Cells["colunaData"].Value = aluno.DataCadastro.ToString("dd/MM/yyyy"); // Atualiza a data de cadastro
        }



        public DataGridViewRow Encontrar(Aluno aluno)      // Localiza a linha de um aluno informado
        {
            foreach (DataGridViewRow linha in grade.Rows)  // Percorre todas as linhas da tabela
            {
                if (ReferenceEquals(linha.Tag, aluno))     // Verifica se tem a mesma tag
                    return linha;                          // Retorna a linha encontrada
            }

            return null; // Retorna nulo caso o aluno não esteja na tabela
        }

        // Obtém o nome do aluno associado à linha informada
        public string ObterNome(DataGridViewRow linha)
        {
            if (linha == null) // Verifica se a linha foi informada
                return string.Empty;

            // Retorna o nome ou um texto alternativo
            return Convert.ToString(linha.Cells["colunaAluno"].Value) ?? "este aluno"; 
        }

        // Remove da lista o aluno associado à linha informada
        public void Excluir(DataGridViewRow linha)
        {
            if (linha != null)             // Verifica se existe a linha
                grade.Rows.Remove(linha);  // Remove a linha da tabela
        }
    }
}