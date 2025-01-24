using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Histories.Events;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("EclipseWorks.UnitTest")]
namespace EclipseWorks.Domain.Commentaries.Entities
{
    public class CommentaryEntity : Entity
    {
        public Guid UserId { get; init; }

        public Guid TaskId { get; init; }

        public string Description { get; init; }

        public virtual UserEntity User { get; init; }

        public virtual TaskEntity Task { get; init; }

        protected CommentaryEntity()
        {
            
        }

        internal CommentaryEntity(Guid id)
        {
            Id = id;
        }

        public static ValidationObject<CommentaryEntity> TryCreateNew(string commentary, Guid userId, Guid taskId)
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

            CommentaryEntity commentaryObj = new (){ UserId = userId, TaskId = taskId, Description = commentary, CreatedAt = DateTime.Now };
            commentaryObj.AddEvent(new AddHistoryDomainEvent("Commentary", userId, commentaryObj, _Shared.Enums.EModificationType.Created));
            return commentaryObj;
        }
    }
}
