using EclipseWorks.Application._Shared.Models;
using MediatR;

namespace EclipseWorks.Application.Reports.Queries
{
    public sealed record GetGenericReportQuery(Guid UserId, DateTime? StartingDate, DateTime? EndingDate) : IRequest<Response>;
}
