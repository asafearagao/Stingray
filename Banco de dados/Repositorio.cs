using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stingray.Persistencia.Interfaces
{
    public interface Repositorio<T>
    {
        Task<IList<T>> ListarTodosAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<T> ObterPorIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> AdicionarAsync(T entidade, CancellationToken cancellationToken = default(CancellationToken));
        Task AtualizarAsync(T entidade, CancellationToken cancellationToken = default(CancellationToken));
        Task ExcluirAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
