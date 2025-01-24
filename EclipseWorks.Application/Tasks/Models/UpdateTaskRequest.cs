using EclipseWorks.Domain._Shared.Enums;

namespace EclipseWorks.Application.Tasks.Commands
{
    public sealed record UpdateTaskRequest(string Title, string Description, DateTime DueDate, EProgress Progress, Guid OwnerId, Guid UserId);
}
