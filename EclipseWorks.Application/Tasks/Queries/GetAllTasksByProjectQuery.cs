using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Tasks.Queries
{
    public sealed record GetAllTasksByProjectQuery(Guid ProjectId) : IRequest<Response>;
}
