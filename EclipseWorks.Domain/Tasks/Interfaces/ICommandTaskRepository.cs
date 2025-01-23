using EclipseWorks.Domain._Shared.Interfaces.Repositories;
using EclipseWorks.Domain.Tasks.Entities;

namespace EclipseWorks.Domain.Tasks.Interfaces
{
    public interface ICommandTaskRepository : ICommandRepository<TaskEntity>
    {
    }
}
