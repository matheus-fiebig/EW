using EclipseWorks.Domain.Reports.Interfaces;
using EclipseWorks.Domain.Reports.Models;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.Infra.Data.Repositories
{
    public class ReportRepository : IQueryReportRepository
    {
        private readonly MainContext context;

        public ReportRepository(MainContext context)
        {
            this.context = context;
        }

        public async Task<GenericReportModel> GenerateReport(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            var filter = (TaskEntity task) => task.DoneDate >= from && task.DoneDate <= to;

            var query = context.Set<UserEntity>();

            var days = to.Subtract(from).Days;

            var usersPerformance = await query
                .GroupBy(x => x.Id, (a, b) => new GenericReportUserModel()
                {
                    TotalDoneTasks = b.SelectMany(x => x.Tasks).Where(x => x.Progress == Domain._Shared.Enums.EProgress.Done && filter(x)).Count(),
                    TotalPendingTasks = b.SelectMany(x => x.Tasks).Where(x => x.Progress != Domain._Shared.Enums.EProgress.Done && filter(x)).Count(),
                    OwnerId = a,
                    OwnerName = b.FirstOrDefault()!.Name,
                    TotalPeriod = days
                })
                .ToListAsync(cancellationToken);

            return new GenericReportModel
            {
                FinishDate = to,
                StartDate = from,
                UsersPerformances =  usersPerformance
            };
        }
    }
}
