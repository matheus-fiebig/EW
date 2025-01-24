using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("EclipseWorks.UnitTest")]
namespace EclipseWorks.Domain.Projects.Entities
{
    public class ProjectEntity : Entity
    {
        public string Name { get; init; }

        public string Description { get; init; }

        public virtual List<TaskEntity> Tasks { get; init; }

        public virtual List<UserEntity> Users { get; init; }

        protected ProjectEntity()
        {

        }

        internal ProjectEntity(Guid id)
        {
            Id = id;
        }

        public ValidationObject<Unit> VerifyDeletionEligibility()
        {
            bool canBeDeleted = Tasks.All(static t => t.Progress == _Shared.Enums.EProgress.Done);
            return canBeDeleted ? Unit.Value : Issue.CreateNew(ErrorConstants.ProjectDeletionCode, ErrorConstants.ProjectDeletionDesc);
        }

        public ValidationObject<Unit> VerifyAddTaskEligibility()
        {
            bool canAddTask = Tasks.Count < 20;
            return canAddTask ? Unit.Value : Issue.CreateNew(ErrorConstants.TaskLimitExceededCode, ErrorConstants.TaskLimitExceededDesc);
        }

        public static ValidationObject<ProjectEntity> TryCreateNew(
            string name,
            string desc,
            IEnumerable<UserEntity> users
         )
        {
            if (string.IsNullOrEmpty(name))
            {
                return Issue.CreateNew(ErrorConstants.TitleNullCode, ErrorConstants.TitleNullDesc);
            }

            if (users is null || !users.Any())
            {
                return Issue.CreateNew(ErrorConstants.ProjectMinUserCode, ErrorConstants.ProjectMinUserDesc);
            }

            return new ProjectEntity()
            {
                Name = name,
                Description = desc,
                Users = users!.ToList(),
                CreatedAt = DateTime.Now,
            };
        }
    }
}
