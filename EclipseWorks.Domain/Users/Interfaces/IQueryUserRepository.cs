using EclipseWorks.Domain._Shared.Interfaces.Repositories;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.Domain.Users.Interfaces
{
    public interface IQueryUserRepository : IQueryRepository<UserEntity>
    {
    }
}
