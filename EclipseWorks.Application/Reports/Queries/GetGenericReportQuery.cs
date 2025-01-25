using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Reports.Queries
{
    public sealed record GetGenericReportQuery(Guid LoggedUserId, DateTime? StartingDate, DateTime? EndingDate) : IRequest<Response>;
}
