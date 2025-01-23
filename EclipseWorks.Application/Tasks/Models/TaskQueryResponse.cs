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

        public string Title { get; init; }

        public string Description { get; init; }

        public DateTime DueDate { get; init; }

        public EPriority Priority { get; init; }

        public EProgress Progress { get; init; }

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
                Title = e.Title,
                Description = e.Description,
                DueDate = e.DueDate,
                Priority = e.Priority,
                Progress = e.Progress,
                Commentaries = CommentaryQueryResponse.ToModel(e.Commentaries)
            };
        }
    }
}
