using System;
using System.Collections.Generic; // Listas
using Stingray.Entidades;
using Stingray.Repositorios;

namespace Stingray.Servicos
{
    // Regras de cadastro e da situação dos alunos
    public class AlunoServico
    {
        private readonly AlunoRepositorio repositorio;

        public AlunoServico() // Construtor
        {
            repositorio = new AlunoRepositorio();
        }

        // Retorna os alunos cadastrados no banco
        public List<Aluno> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        // Consulta os alunos aplicando turma, professor, situação e busca no MySQL
        public List<Aluno> ListarFiltrados(int idTurma, int idProfessor, string situacao, string busca)
        {
            return repositorio.ListarFiltrados(idTurma, idProfessor, situacao, busca);
        }

        public List<Stingray.DadosTela.TurmaAlunoLinha> ListarTurmasMatricula(int idMatricula)
        {
            return repositorio.ListarTurmasMatricula(idMatricula);
        }

        // Adiciona um aluno novo ou atualiza a ficha de um existente
        public void Salvar(Aluno aluno)
        {
            Validar(aluno);

            if (aluno.IdAluno > 0)
                repositorio.Atualizar(aluno);

            else //O ID negativo indica que o aluno é novo
                aluno.IdAluno = repositorio.Inserir(aluno);
        }

        // Exclui definitivamente o aluno e seus registros dependentes
        public void Excluir(int idAluno)
        {
            if (idAluno <= 0)
                throw new ArgumentException("Aluno inválido.", nameof(idAluno));

            repositorio.Excluir(idAluno);
        }

        private static void Validar(Aluno aluno)
        {
            if (aluno == null)
                throw new ArgumentNullException(nameof(aluno));

            ValidacaoDadosCadastro.ValidarPessoa(aluno.Pessoa);
            ValidacaoDadosCadastro.ValidarEndereco(aluno.Endereco);
            ValidacaoDadosCadastro.ValidarTelefone(aluno.Telefone);

            if (!string.IsNullOrWhiteSpace(aluno.Observacao) && aluno.Observacao.Trim().Length > 100)
                throw new ArgumentException("A observação deve ter até 100 caracteres.");

            if (aluno.DiaVencimento < 1 || aluno.DiaVencimento > 28)
                throw new ArgumentException("O dia de vencimento deve estar entre 1 e 28.");
        }
    }
}
