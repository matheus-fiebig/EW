using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Projects.Queries
{
    public sealed record GetAllProjectsByUserQuery(Guid? UserId): IRequest<Response>;
}
