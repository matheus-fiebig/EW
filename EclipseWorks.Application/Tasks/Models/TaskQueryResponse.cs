using EclipseWorks.Application.Commentaries.Models;
using EclipseWorks.Application.Projects.Models;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;

namespace EclipseWorks.Application.Tasks.Models
{
    public class TaskQueryResponse
    {
        public Guid Id { get; init; }

        public Guid ProjectId { get; init; }

        public string ProjectName { get; init; }

        public string Title { get; init; }

        public string Description { get; init; }

        public DateTime DueDate { get; init; }

        public EPriority PriorityId { get; init; }

        public string PriorityDesc => PriorityId.ToString();

        public EProgress ProgressId { get; init; }
        
        public string ProgressDesc => ProgressId.ToString();

        public string OwnerName { get; init; }

        public IEnumerable<CommentaryQueryResponse> Commentaries { get; init; }

        public static IEnumerable<TaskQueryResponse> ToModel(IEnumerable<TaskEntity> entities)
        {
            return entities.Select(e => ToModel(e));
        }

        public static TaskQueryResponse ToModel(TaskEntity e)
        {
            return new TaskQueryResponse()
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ProjectName = e.Project?.Name ?? string.Empty,
                Title = e.Title,
                Description = e.Description,
                DueDate = e.DueDate,
                PriorityId = e.Priority,
                ProgressId = e.Progress,
                OwnerName = e.Owner?.Name ?? string.Empty,
                Commentaries = CommentaryQueryResponse.ToModel(e.Commentaries)
            };
        }
    }
}
