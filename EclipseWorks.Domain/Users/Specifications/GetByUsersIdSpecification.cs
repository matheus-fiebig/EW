using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain.Users.Entities;
using System.Linq.Expressions;

namespace EclipseWorks.Domain.Users.Specifications
{
    public class GetByUsersIdSpecification : ISpecification<UserEntity>
    {
        private List<Guid> Users { get; init; }

        private GetByUsersIdSpecification()
        {

        }

        public Expression<Func<UserEntity, bool>> Predicate => u => Users != null && Users.Any(_u => _u == u.Id);

        public static GetByUsersIdSpecification Create(List<Guid> users)
        {
            return new GetByUsersIdSpecification() { Users = users };
        }
    }
}
