using EclipseWorks.Domain._Shared.Interfaces.Repositories;
using EclipseWorks.Domain.Tasks.Entities;

namespace EclipseWorks.Domain.Tasks.Interfaces
{
    public interface IQueryTaskRepository : IQueryRepository<TaskEntity>
    {
    }
}
