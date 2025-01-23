using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Interfaces.Specification;

namespace EclipseWorks.Domain._Shared.Interfaces.Repositories
{
    public interface ICommandRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken);
    }
}
