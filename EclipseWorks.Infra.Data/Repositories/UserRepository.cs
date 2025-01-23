using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using EclipseWorks.Infra.Data.Context;

namespace EclipseWorks.Infra.Data.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IQueryUserRepository
    {
        public UserRepository(MainContext context) : base(context)
        {
        }

        protected override IQueryable<UserEntity> GetWithIncludes()
        {
            return _dbSet;
        }
    }
}
