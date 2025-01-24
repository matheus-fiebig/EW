using EclipseWorks.Domain.Histories.Entities;
using EclipseWorks.Domain.Histories.Interfaces;
using EclipseWorks.Infra.Data.Context;

namespace EclipseWorks.Infra.Data.Repositories
{
    public class HistoryRepository : BaseRepository<HistoryEntity>, ICommandHistoryRepository
    {
        public HistoryRepository(MainContext context) : base(context)
        {
        }

        protected override IQueryable<HistoryEntity> GetWithIncludes()
        {
            throw new NotImplementedException();
        }
    }
}
