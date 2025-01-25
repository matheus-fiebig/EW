using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Projects.Models;
using EclipseWorks.Application.Projects.Queries;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Projects.Specification;

namespace EclipseWorks.Application.Projects.Handlers
{
    public class GetAllProjectsByUserQueryHandler : BaseQueryHandler<GetAllProjectsByUserQuery, Response>
    {
        private readonly IQueryProjectRepository queryProjectRepository;

        public GetAllProjectsByUserQueryHandler(IQueryProjectRepository queryProjectRepository)
        {
            this.queryProjectRepository = queryProjectRepository;
        }

        protected override async Task<Response> TryHandle(GetAllProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == default)
            {
                return Response.FromError(Issue.CreateNew(ErrorConstants.InvalidRequestCode, ErrorConstants.InvalidRequestDesc));
            }

            var projects = await queryProjectRepository.GetPagedAsync(-1, -1, GetProjectByUserSpecification.Create(request.UserId), cancellationToken);

            if (!projects.Any())
            {
                return Response.Empty();
            }

            return Response.FromData(ProjectQueryResponse.ToModel(projects));
        }
    }
}
