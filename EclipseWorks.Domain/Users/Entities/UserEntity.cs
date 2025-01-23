using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain.Projects.Entities;

namespace EclipseWorks.Domain.Users.Entities
{
    public class UserEntity : Entity
    {
        public string Name { get; init; }

        public string Role { get; init; }

        public virtual List<ProjectEntity> Projects { get; init; }

        protected UserEntity()
        {

        }

        public static UserEntity Create(string name, string role)
        {
            return new()
            {
                Name = name,
                Role = role
            };
        }

        public static UserEntity FromExistingUser(Guid id)
        {
            return new UserEntity()
            {
                Id = id
            };
        }
    }
}
