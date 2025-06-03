using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Common;

namespace Elomoas.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;

    Task<int> Save(CancellationToken cancellationToken);

    Task Rollback();
}
