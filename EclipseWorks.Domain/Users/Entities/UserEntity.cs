using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;

namespace EclipseWorks.Domain.Users.Entities
{
    public class UserEntity : Entity
    {
        public string Name { get; init; }

        public string Role { get; init; }

        public virtual List<ProjectEntity> Projects { get; init; }

        public virtual List<TaskEntity> Tasks { get; init; }

        public virtual List<CommentaryEntity> Commentaries{ get; init; }

        protected UserEntity()
        {

        }

        public static UserEntity Create(string name, string role)
        {
            return new()
            {
                Name = name,
                Role = role,
                CreatedAt = DateTime.Now,
            };
        }
    }
}
