using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;

namespace EclipseWorks.Application.Tasks.Handlers
{
    public class CreateTaskCommandHandler : BaseCommandHandler<CreateTaskCommand, Response>
    {
        private readonly IQueryProjectRepository queryProjectRepository;
        private readonly ICommandTaskRepository commandTaskRepository;

        public CreateTaskCommandHandler(IQueryProjectRepository queryProjectRepository, ICommandTaskRepository commandTaskRepository, IUnitOfWork uow)
             : base(uow) 
        {
            this.queryProjectRepository = queryProjectRepository;
            this.commandTaskRepository = commandTaskRepository;
        }

        protected override async Task<Response> TryHandle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            ProjectEntity project = await queryProjectRepository.GetAsync(GetByIdSpecification<ProjectEntity>.Create(request.Body.ProjectId),
                                                                          cancellationToken);
            
            ValidationObject<Unit> addTaskEligibility = project.VerifyAddTaskEligibility();
            if(addTaskEligibility.HasIssue)
            {
                return addTaskEligibility.Issue;
            }

            var body = request.Body;
            ValidationObject<TaskEntity> taskCreation = TaskEntity.TryCreateNew(body.Title, body.Description,
                                                                                body.DueDate, body.Priority,
                                                                                project, body.UserId, body.OwnerId);
            if(taskCreation.HasIssue)
            {
                return taskCreation.Issue;
            }

            TaskEntity task = await commandTaskRepository.InsertAsync(taskCreation.Entity, cancellationToken);
            return Response.FromData(TaskQueryResponse.ToModel(task));
        }
    }
}
