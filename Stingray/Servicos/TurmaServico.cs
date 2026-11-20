using System;
using System.Collections.Generic;
using System.Linq; // Listas
using Stingray.Entidades;
using Stingray.DadosTela;
using Stingray.Repositorios;

namespace Stingray.Servicos
{

    // Aplica as regras de negócio do cadastro e dos participantes de turmas
    public class TurmaServico
    {
        private readonly TurmaRepositorio repositorio;

        public TurmaServico() // Construtor
        {
            repositorio = new TurmaRepositorio();
        }

        public List<Turma> ListarTodos() // Lista as turmas e os professores responsáveis
        {
            return repositorio.ListarTodos();
        }

        public void Salvar(Turma turma) // Cria ou atualiza uma turma
        {
            // Valida os dados da turma antes de salvar
            Validar(turma);

            if (turma.IdTurma > 0)
                repositorio.Atualizar(turma);

            // ID negativo indica que a turma é nova
            else
                turma.IdTurma = repositorio.Inserir(turma);
        }

        public void Excluir(int idTurma) // Exclui uma turma
        {
            if (idTurma <= 0) // Valida o ID da turma
                throw new ArgumentException("Turma inválida.", nameof(idTurma));

            repositorio.Excluir(idTurma);
        }

        // Lista as matrículas ativas que podem participar da turma
        public List<ParticipanteTurma> ListarParticipantes(int idTurma)
        {
            if (idTurma <= 0) // Valida o ID da turma
                throw new ArgumentException("Turma inválida.", nameof(idTurma));

            return repositorio.ListarParticipantes(idTurma);
        }

        public void SalvarParticipantes(int idTurma, List<int> idsMatricula, int capacidade)
        {
            if (idTurma <= 0)          // Valida o ID da turma
                throw new ArgumentException("Turma inválida.", nameof(idTurma));
            if (idsMatricula == null)  // Valida a lista de matrículas
                throw new ArgumentNullException(nameof(idsMatricula));
            if (capacidade <= 0)       // Valida a capacidade da turma
                throw new ArgumentOutOfRangeException(nameof(capacidade));

            // Valida se a quantidade de participantes não ultrapassa a capacidade
            if (idsMatricula.Count > capacidade)
                throw new InvalidOperationException("A quantidade de participantes ultrapassa a capacidade da turma.");
            // Valida se a lista de matrículas não tem IDs repetidos
            if (idsMatricula.Distinct().Count() != idsMatricula.Count)
                throw new ArgumentException("A lista contém matrículas repetidas.", nameof(idsMatricula));

            repositorio.AtualizarParticipantes(idTurma, idsMatricula);
        }

        private static void Validar(Turma turma)
        {
            if (turma == null) // Valida se a turma não é nula
                throw new ArgumentNullException(nameof(turma));
            if (string.IsNullOrWhiteSpace(turma.Modalidade) || turma.Modalidade.Trim().Length > 30)
                throw new ArgumentException("Informe uma modalidade com até 30 caracteres.");
            // Valida se o dia da semana é válido
            if (string.IsNullOrWhiteSpace(turma.DiaSemana) || turma.DiaSemana.Trim().Length > 20)
                throw new ArgumentException("Selecione um dia da semana.");
            // Valida se o horário de início é depois do horário de fim
            if (turma.HorarioFim <= turma.HorarioInicio)
                throw new ArgumentException("O horário de fim deve ser posterior ao horário de início.");

            if (turma.Capacidade <= 0)
                throw new ArgumentException("A capacidade deve ser maior que zero.");
            if (turma.ValorMensal < 0 || turma.ValorMensal > 99999999.99M)
                throw new ArgumentException("O valor mensal deve ser maior ou igual a zero e caber no limite financeiro da turma.");
            // Valida se um professor responsável está selecionado
            if (turma.IdProfessor <= 0)
                throw new ArgumentException("Selecione um professor cadastrado.");
        }
    }
}
