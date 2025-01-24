using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Reports.Queries;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Reports.Interfaces;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;

namespace EclipseWorks.Application.Reports.Handlers
{
    public class GetGenericReportQueryHandler : BaseQueryHandler<GetGenericReportQuery, Response>
    {
        private readonly IQueryReportRepository queryReportRepository;
        private readonly IQueryUserRepository queryUserRepository;

        public GetGenericReportQueryHandler(IQueryReportRepository queryReportRepository, IQueryUserRepository queryUserRepository) 
        {
            this.queryReportRepository = queryReportRepository;
            this.queryUserRepository = queryUserRepository;
        }

        protected override async Task<Response> TryHandle(GetGenericReportQuery request, CancellationToken cancellationToken)
        {
            var user = await queryUserRepository.GetAsync(GetByIdSpecification<UserEntity>.Create(request.UserId), cancellationToken);
            if(user.Role.ToLower().Trim() != "gerente")
            {
                return Issue.CreateNew(ErrorConstants.NotAuthorizedCode, ErrorConstants.NotAuthorizedDesc);
            }

            if (request.EndingDate < request.StartingDate)
            {
                return Issue.CreateNew(ErrorConstants.NullEndDateCode, ErrorConstants.NullEndDateDesc);
            }

            var endingDate = request.EndingDate;
            var startingDate = request.StartingDate; 
            
            if (endingDate is null)
            {
                endingDate = DateTime.Now;
            }

            if (startingDate is null)
            {
                startingDate = DateTime.Now.AddDays(-30);
            }

            var report = await queryReportRepository.GenerateReport(startingDate.Value, endingDate.Value, cancellationToken);
            return Response.FromData(report);
        }
    }
}

