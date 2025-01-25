using EclipseWorks.Domain._Shared.Entities;
using System.Linq.Expressions;

namespace EclipseWorks.Domain._Shared.Interfaces.Specification
{
    public interface ISpecification<TEntity> where TEntity : Entity
    {
        Expression<Func<TEntity, bool>> Predicate { get; }
    }
}
