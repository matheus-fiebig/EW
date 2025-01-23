using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Tasks.Commands
{
    public sealed record UpdateTaskCommand(Guid TaskId, UpdateTaskRequest Body) : IRequest<Response>;
}
