using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using System.Linq.Expressions;

namespace EclipseWorks.Domain._Shared.Specifications
{
    public class GetByIdSpecification<TEntity> : ISpecification<TEntity> where TEntity : Entity
    {
        private Guid Id { get; set; }

        private GetByIdSpecification()
        {
        }

        public Expression<Func<TEntity, bool>> Predicate => e => e.Id == Id;

        public static ISpecification<TEntity> Create(Guid id)
        {
            return new GetByIdSpecification<TEntity>() { Id = id };
        }
    }
}
