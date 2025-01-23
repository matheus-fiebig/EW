using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Infra.Data.Context;

namespace EclipseWorks.Infra.Data.Repositories
{
    public class ProjectRepository : BaseRepository<ProjectEntity>, IQueryProjectRepository, ICommandProjectRepository
    {
        public ProjectRepository(MainContext context) : base(context)
        {
        }

        protected override IQueryable<ProjectEntity> GetWithIncludes()
        {
            return _dbSet;
        }
    }
}
