using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Histories.Events;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.Domain.Tasks.Entities
{
    public class TaskEntity : Entity
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public Guid ProjectId { get; private set; }

        public Guid OwnerId { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime? DoneDate { get; private set; }

        public EPriority Priority { get; private set; }

        public EProgress Progress { get; private set; }

        public virtual List<CommentaryEntity> Commentaries { get; private set; }

        public virtual ProjectEntity Project { get; private set; }

        public virtual UserEntity Owner { get; private set; }

        protected TaskEntity()
        {
            
        }

        public ValidationObject<TaskEntity> TryUpdate(string title, string desc, DateTime? dueDate, EProgress? progress, Guid ownerId, Guid userId)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Issue.CreateNew(ErrorConstants.TitleNullCode, ErrorConstants.TitleNullDesc);
            }

            if (dueDate is null)
            {
                return Issue.CreateNew(ErrorConstants.InvalidDueDateCode, ErrorConstants.InvalidDueDateDesc);
            }

            if (dueDate < DateTime.Now)
            {
                return Issue.CreateNew(ErrorConstants.DueDateNotAllowedCode, ErrorConstants.DueDateNotAllowedDesc);
            }

            if(progress is null)
            {
                return Issue.CreateNew(ErrorConstants.ProgressNullCode, ErrorConstants.ProgressNullDesc);
            }

            if(progress.Value == EProgress.Done)
            {
                DoneDate = DateTime.Now;
            }

            Title = title;
            Description = desc;
            DueDate = DueDate;
            Progress = progress.Value;
            OwnerId = ownerId;

            AddEvent(new AddHistoryDomainEvent("Task", userId, this, EModificationType.Updated));
            
            return this;
        }

        public ValidationObject<TaskEntity> AddCommentary(string commentary, Guid userId)
        {
            var validationObject = CommentaryEntity.TryCreateNew(commentary, userId, Id);
            
            if(validationObject.HasIssue)
            {
                return validationObject.Issue;
            }

            Commentaries.Add(validationObject.Entity);
            return this;
        }

        public static ValidationObject<TaskEntity> TryCreateNew(string title, string desc,  DateTime? dueDate, EPriority? priority, ProjectEntity project, Guid ownerId, Guid userId)
        {
            if(string.IsNullOrEmpty(title))
            {
                return Issue.CreateNew(ErrorConstants.TitleNullCode, ErrorConstants.TitleNullDesc);
            }

            if (dueDate is null)
            {
                return Issue.CreateNew(ErrorConstants.InvalidDueDateCode, ErrorConstants.InvalidDueDateDesc);
            }

            if (dueDate < DateTime.Now)
            {
                return Issue.CreateNew(ErrorConstants.DueDateNotAllowedCode, ErrorConstants.DueDateNotAllowedDesc);
            }

            if (priority is null)
            {
                return Issue.CreateNew(ErrorConstants.PriorityNullCode, ErrorConstants.PriorityNullDesc);
            }

            if (project is null)
            {
                return Issue.CreateNew(ErrorConstants.ProjectNotFoundCode, ErrorConstants.ProjectNotFoundDesc);
            }

            TaskEntity task = new TaskEntity()
            {
                Title = title,
                Description = desc,
                DueDate = dueDate.Value,
                Priority = priority.Value,
                Progress = EProgress.Todo,
                ProjectId = project.Id,
                CreatedAt = DateTime.Now,
                OwnerId = ownerId,
            };

            task.AddEvent(new AddHistoryDomainEvent("Task", userId, task, EModificationType.Created));

            return task;
        }
    }
}
