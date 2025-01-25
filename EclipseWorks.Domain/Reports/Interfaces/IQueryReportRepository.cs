using EclipseWorks.Domain.Reports.Models;

namespace EclipseWorks.Domain.Reports.Interfaces
{
    public interface IQueryReportRepository 
    {
        Task<GenericReportModel> GenerateReport(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    }
}
