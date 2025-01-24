using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("EclipseWorks.UnitTest")]
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

        internal UserEntity(Guid id)
        {
            Id = id;
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
