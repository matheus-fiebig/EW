using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Projects.Commands
{
    public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest<Response>;
}
