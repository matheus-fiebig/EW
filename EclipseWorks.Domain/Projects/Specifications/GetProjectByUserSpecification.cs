using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain.Projects.Entities;
using System.Linq.Expressions;

namespace EclipseWorks.Domain.Projects.Specification
{
    public class GetProjectByUserSpecification : ISpecification<ProjectEntity>
    {
        private Guid Id { get; set; }

        private GetProjectByUserSpecification()
        {
        }

        public Expression<Func<ProjectEntity, bool>> Predicate => e => e.Id == Id;

        Expression<Func<ProjectEntity, bool>> ISpecification<ProjectEntity>.Predicate => p => p.Users.Any(u => u.Id == Id);

        public static GetProjectByUserSpecification Create(Guid userID) 
        {
            return new GetProjectByUserSpecification() { Id = userID };
        }
    }
}
