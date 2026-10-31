using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stingray.Persistencia.Interfaces;
using Stingray.Modelos;

namespace Stingray.Servicos {
    public sealed class AlunoServico {
        private readonly Repositorio<Aluno> repositorio;

        // Inicializa o serviço de alunos para persistência
        public AlunoServico(Repositorio<Aluno> repositorio) {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        // Obtém a lista de alunos
        public Task<IList<Aluno>> ListarAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return repositorio.ListarTodosAsync(cancellationToken);
        }

        // Obtém um aluno pelo id
        public Task<Aluno> ObterAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "O identificador deve ser maior que zero.");

            return repositorio.ObterPorIdAsync(id, cancellationToken);
        }

        // Valida os dados e adiciona o aluno
        public Task<int> AdicionarAsync(Aluno aluno, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidarAluno(aluno);
            return repositorio.AdicionarAsync(aluno, cancellationToken);
        }

        // Valida os dados e solicita a atualização do aluno
        public Task AtualizarAsync(Aluno aluno, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidarAluno(aluno);
            if (aluno.Id <= 0)
                throw new ArgumentException("O aluno precisa ter um identificador válido para ser atualizado.", nameof(aluno));

            return repositorio.AtualizarAsync(aluno, cancellationToken);
        }

        public Task ExcluirAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "O identificador deve ser maior que zero.");

            return repositorio.ExcluirAsync(id, cancellationToken);
        }

        // Verifica se o aluno e o nome foram informados
        private static void ValidarAluno(Aluno aluno)
        {
            if (aluno == null)
                throw new ArgumentNullException(nameof(aluno));
            if (string.IsNullOrWhiteSpace(aluno.Nome))
                throw new ArgumentException("O nome do aluno é obrigatório.", nameof(aluno));
        }
    }
}