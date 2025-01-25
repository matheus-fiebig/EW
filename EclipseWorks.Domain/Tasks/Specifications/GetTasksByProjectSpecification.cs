using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain.Tasks.Entities;
using System.Linq.Expressions;

namespace EclipseWorks.Domain.Tasks.Specifications
{
    public class GetTasksByProjectSpecification : ISpecification<TaskEntity>
    {
        private Guid _id;

        private GetTasksByProjectSpecification()
        {
            
        }

        public Expression<Func<TaskEntity, bool>> Predicate => t => t.Project.Id == _id;
    
        public static GetTasksByProjectSpecification Create(Guid projectId)
        {
            return new()
            {
                _id = projectId,
            };
        }
    }
}
