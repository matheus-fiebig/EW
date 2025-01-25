using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Interfaces.Specification;

namespace EclipseWorks.Domain._Shared.Interfaces.Repositories
{
    public interface IQueryRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> spec = null, CancellationToken cancellationToken = default);
        Task<TEntity?> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageSize = -1, int pageNumber = -1, ISpecification<TEntity> spec = null, CancellationToken cancellationToken = default);
    }
}
