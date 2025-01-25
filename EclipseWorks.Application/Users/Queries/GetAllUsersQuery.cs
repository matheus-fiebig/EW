using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Users.Queries
{
    public sealed record GetAllUsersQuery() : IRequest<Response>;
}
