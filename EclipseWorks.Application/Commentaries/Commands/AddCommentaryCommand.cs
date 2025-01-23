using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Commentaries.Models;
using MediatR;

namespace EclipseWorks.Application.Commentaries.Commands
{
    public sealed record AddCommentaryCommand(AddCommentaryRequest Body) : IRequest<Response>;
}
