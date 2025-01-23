using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Tasks.Commands
{
    public sealed record DeleteTaskCommand(Guid TaskId) : IRequest<Response>;
}
