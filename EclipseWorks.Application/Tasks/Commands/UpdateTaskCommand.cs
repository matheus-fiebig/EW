using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Domain._Shared.Enums;
using MediatR;

namespace EclipseWorks.Application.Tasks.Commands
{
    public sealed record UpdateTaskCommand(Guid TaskId, UpdateTaskRequest Body) : IRequest<Response>;
}
