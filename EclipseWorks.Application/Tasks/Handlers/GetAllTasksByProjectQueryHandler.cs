using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Application.Tasks.Queries;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using EclipseWorks.Domain.Tasks.Specifications;

namespace EclipseWorks.Application.Tasks.Handlers
{
    public class GetAllTasksByProjectQueryHandler : BaseQueryHandler<GetAllTasksByProjectQuery, Response>
    {
        private readonly IQueryTaskRepository queryTaskRepository;

        public GetAllTasksByProjectQueryHandler(IQueryTaskRepository queryTaskRepository)
        {
            this.queryTaskRepository = queryTaskRepository;
        }

        protected override async Task<Response> TryHandle(GetAllTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            if(request == default)
            {
                return Response.FromError(Issue.CreateNew(ErrorConstants.InvalidRequestCode,ErrorConstants.InvalidRequestDesc));
            }

            IEnumerable<TaskEntity> tasks = await queryTaskRepository.GetPagedAsync(-1, -1, GetTasksByProjectSpecification.Create(request.ProjectId), cancellationToken);

            if (!tasks.Any())
            {
                return Response.Empty();
            }

            return Response.FromData(TaskQueryResponse.ToModel(tasks));
        }
    }
}
