using System;
using System.Collections.Generic; // Listas
using Stingray.Entidades;
using Stingray.Repositorios;

namespace Stingray.Servicos
{

    // Aplica as regras de negócio do cadastro de professores
    public class ProfessorServico // Valida e cadastra os dados dos professores
    {
        private const int TamanhoMaximoCref = 255;
        private readonly ProfessorRepositorio repositorio;

        public ProfessorServico()
        {
            repositorio = new ProfessorRepositorio();
        }

        // Lista os nomes cadastrados para seleção outros módulos
        public List<string> ListarNomesCadastrados()
        {
            return repositorio.ListarNomes();
        }

        // Lista os todos os dados dos professores
        public List<Professor> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        // Busca os dados completos de um professor
        public Professor BuscarPorId(int idProfessor)
        {
            // Valida o ID do professor
            if (idProfessor <= 0)
                throw new ArgumentException("Professor inválido.", nameof(idProfessor));

            return repositorio.BuscarPorId(idProfessor);
        }

        public void Salvar(Professor professor) // Cadastra um professor ou atualiza um existente
        {
            ValidarCadastro(professor);

            if (professor.IdProfessor > 0)
                repositorio.Atualizar(professor);

            else // ID negativo indica que o professor é novo
                professor.IdProfessor = repositorio.Inserir(professor);
        }

        // Exclui o professor depois que as turmas são desvinculadas
        public void Excluir(int idProfessor)
        {
            // Valida o ID do professor
            if (idProfessor <= 0)
                throw new ArgumentException("Professor inválido.", nameof(idProfessor));

            repositorio.Excluir(idProfessor);
        }

        private static void ValidarCadastro(Professor professor)
        {
            // Verfica se existe o professor
            if (professor == null)
                throw new ArgumentNullException(nameof(professor));

            ValidacaoDadosCadastro.ValidarPessoa(professor.Pessoa);
            ValidacaoDadosCadastro.ValidarEndereco(professor.Endereco);
            ValidacaoDadosCadastro.ValidarTelefone(professor.Telefone);

            // Valida o CREF do professor
            if (string.IsNullOrWhiteSpace(professor.Cref) || professor.Cref.Trim().Length > TamanhoMaximoCref)
                throw new ArgumentException("Informe o CREF (até 255 caracteres).");
        }
    }
}
