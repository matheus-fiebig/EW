using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using EclipseWorks.Infra.Data.Context;

namespace EclipseWorks.Infra.Data.Repositories
{
    public class TaskRepository : BaseRepository<TaskEntity>, IQueryTaskRepository, ICommandTaskRepository
    {
        public TaskRepository(MainContext context) : base(context)
        {
        }

        protected override IQueryable<TaskEntity> GetWithIncludes()
        {
            return _dbSet.AsQueryable();
        }
    }
}
