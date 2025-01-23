using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.Application.Users.Models
{
    public class UsersQueryResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Role { get; init; }

        public static IEnumerable<UsersQueryResponse> ToModel(IEnumerable<UserEntity> entities)
        {
            return entities.Select(e => new UsersQueryResponse()
            {
                Name = e.Name,
                Role = e.Role,
                Id = e.Id,
            });
        }
    }
}
