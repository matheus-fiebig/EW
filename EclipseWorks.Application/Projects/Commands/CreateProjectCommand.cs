using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Projects.Models;
using MediatR;

namespace EclipseWorks.Application.Projects.Commands
{
    public sealed record CreateProjectCommand(CreateProjectRequest Body) : IRequest<Response>;
}
