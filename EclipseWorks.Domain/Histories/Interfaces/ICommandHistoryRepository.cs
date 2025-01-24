using EclipseWorks.Domain._Shared.Interfaces.Repositories;
using EclipseWorks.Domain.Histories.Entities;

namespace EclipseWorks.Domain.Histories.Interfaces
{
    public interface ICommandHistoryRepository : ICommandRepository<HistoryEntity>
    {
    }
}
