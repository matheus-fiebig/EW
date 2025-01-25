using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Histories.Events;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Users.Entities;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleToAttribute("EclipseWorks.UnitTest")]
namespace EclipseWorks.Domain.Tasks.Entities
{
    public class TaskEntity : Entity
    {
        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public Guid ProjectId { get; internal set; }

        public Guid? OwnerId { get; internal set; }

        public DateTime DueDate { get; internal set; }

        public DateTime? DoneDate { get; internal set; }

        public EPriority Priority { get; internal set; }

        public EProgress Progress { get; internal set; }

        [JsonIgnore]
        public virtual List<CommentaryEntity> Commentaries { get; internal set; } = new();

        [JsonIgnore]
        public virtual ProjectEntity Project { get; internal set; }

        [JsonIgnore]
        public virtual UserEntity Owner { get; internal set; }

        protected TaskEntity()
        {
            
        }

        internal TaskEntity(Guid id)
        {
            Id = id;
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

            if(!Enum.IsDefined(typeof(EProgress), progress.Value))
            {
                return Issue.CreateNew(ErrorConstants.ProgressNotFoundCode, ErrorConstants.ProgressNotFoundDesc);
            }

            if(progress.Value == EProgress.Done)
            {
                DoneDate = DateTime.Now;
            }

            Title = title;
            Description = desc;
            DueDate = DueDate;
            Progress = progress.Value;
            OwnerId = ownerId == Guid.Empty ? null : ownerId;

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

        public static ValidationObject<TaskEntity> TryCreateNew(string title, string desc,  DateTime? dueDate, EPriority? priority, ProjectEntity project, Guid ownerId, Guid loggedUserId)
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

            if (!Enum.IsDefined(typeof(EPriority), priority.Value))
            {
                return Issue.CreateNew(ErrorConstants.PriorityNotFoundCode, ErrorConstants.PriorityNotFoundDesc);
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
                OwnerId = ownerId == Guid.Empty ? null : ownerId
            };

            task.AddEvent(new AddHistoryDomainEvent("Task", loggedUserId, task, EModificationType.Created));

            return task;
        }
    }
}
