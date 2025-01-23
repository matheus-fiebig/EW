using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.Domain.Commentaries.Entities
{
    public class CommentaryEntity : Entity
    {
        public Guid UserId { get; set; }

        public Guid TaskId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual TaskEntity Task { get; set; }

        protected CommentaryEntity()
        {
            
        }

        public static ValidationObject<CommentaryEntity> CreateNew(string commentary, Guid userId, Guid taskId)
        {
            if(string.IsNullOrEmpty(commentary))
            {
                return Issue.CreateNew(ErrorConstants.CommentaryNullCode, ErrorConstants.CommentaryNullDesc);
            }

            if (userId == default)
            {
                return Issue.CreateNew(ErrorConstants.UserNotFoundCode, ErrorConstants.UserNotFoundDesc);
            }

            if (taskId == default)
            {
                return Issue.CreateNew(ErrorConstants.TaskNotFoundCode, ErrorConstants.TaskNotFoundDesc);
            }

            return new CommentaryEntity { UserId = userId, TaskId = taskId, Description = commentary };
        }
    }
}
