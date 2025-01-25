using EclipseWorks.Domain._Shared.Interfaces.Repositories;
using EclipseWorks.Domain.Projects.Entities;

namespace EclipseWorks.Domain.Projects.Interfaces
{
    public interface ICommandProjectRepository : ICommandRepository<ProjectEntity>
    {
    }
}
