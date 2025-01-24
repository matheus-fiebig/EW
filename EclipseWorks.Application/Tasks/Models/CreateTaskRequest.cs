using EclipseWorks.Domain._Shared.Enums;

namespace EclipseWorks.Application.Tasks.Models
{
    public sealed record CreateTaskRequest(Guid ProjectId, Guid UserId, string Title, string Description, DateTime DueDate, EPriority Priority, Guid OwnerId);
}
