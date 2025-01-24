using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Interfaces.Repositories;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.Infra.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : ICommandRepository<TEntity>, IQueryRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly MainContext _context;

        public BaseRepository(MainContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        protected abstract IQueryable<TEntity> GetWithIncludes();

        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int pageSize = -1, int pageNumber = -1, ISpecification<TEntity>? spec = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetWithIncludes();

            if (spec != null)
            {
                query = query.Where(spec.Predicate);
            }

            if (pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }


            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity>? spec = null, CancellationToken cancellationToken = default)
        {
            var query = GetWithIncludes();
            
            if (spec != null)
            {
                query = query.Where(spec.Predicate);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity?> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await GetWithIncludes().FirstOrDefaultAsync(spec.Predicate, cancellationToken);
            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var addedEntity = await _dbSet.AddAsync(entity, cancellationToken);
            _ = await _context.SaveChangesAsync(cancellationToken);
            return addedEntity.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
            _ = await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task DeleteAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await GetAsync(spec, cancellationToken);
            _ = _dbSet.Remove(entity!);
            _ = await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
