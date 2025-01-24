namespace EclipseWorks.Domain.Reports.Models
{
    public class GenericReportModel
    {
        public List<GenericReportUserModel> UsersPerformances { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime FinishDate { get; init; }
    }

    public class GenericReportUserModel
    {
        public Guid OwnerId { get; init; }
        public string OwnerName { get; init; }
        public decimal TotalDoneTasks { get; init; }
        public int TotalPendingTasks { get; init; }
        public int TotalPeriod { private get; init; }
        public decimal AverageDoneTasks => TotalDoneTasks / TotalPeriod;
    }
}
