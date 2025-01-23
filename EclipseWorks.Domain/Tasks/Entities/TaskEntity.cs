using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Projects.Entities;

namespace EclipseWorks.Domain.Tasks.Entities
{
    public class TaskEntity : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid ProjectId { get; set; }

        public DateTime DueDate { get; set; }

        public EPriority Priority { get; set; }

        public EProgress Progress { get; set; }

        public virtual List<CommentaryEntity> Commentaries { get; set; }

        public virtual ProjectEntity Project { get; set; }

        protected TaskEntity()
        {
            
        }

        public ValidationObject<TaskEntity> TryUpdate(string title, string desc, DateTime? dueDate, EProgress? progress)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Issue.CreateNew(ErrorConstants.TitleNullCode, ErrorConstants.TitleNullDesc);
            }

            if (string.IsNullOrEmpty(desc))
            {
                return Issue.CreateNew(ErrorConstants.DescriptionNullCode, ErrorConstants.DescriptionNullDesc);
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

            Title = title;
            Description = desc;
            DueDate = DueDate;
            Progress = progress.Value;
            return this;
        }

        public ValidationObject<TaskEntity> AddCommentary(string commentary, Guid userId)
        {
            var validationObject = CommentaryEntity.CreateNew(commentary, userId, Id);
            
            if(validationObject.HasIssue)
            {
                return validationObject.Issue;
            }

            Commentaries.Add(validationObject.Entity);
            return this;
        }

        public static ValidationObject<TaskEntity> TryCreateNew(string title, string desc,  DateTime? dueDate, EPriority? priority, ProjectEntity project)
        {
            if(string.IsNullOrEmpty(title))
            {
                return Issue.CreateNew(ErrorConstants.TitleNullCode, ErrorConstants.TitleNullDesc);
            }

            if(string.IsNullOrEmpty(desc))
            {
                return Issue.CreateNew(ErrorConstants.DescriptionNullCode, ErrorConstants.DescriptionNullDesc);
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

            return new TaskEntity()
            {
                Title = title,
                Description = desc,
                DueDate = dueDate.Value,
                Priority = priority.Value,
                Progress = EProgress.Todo,
                ProjectId = project.Id,
            };
        }
    }
}
