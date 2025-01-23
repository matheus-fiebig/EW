using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;

namespace EclipseWorks.Application.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : BaseCommandHandler<UpdateTaskCommand, Response>
    {
        private readonly IQueryTaskRepository queryTaskRepository;
        private readonly ICommandTaskRepository commandTaskRepository;

        public UpdateTaskCommandHandler(IQueryTaskRepository queryTaskRepository, ICommandTaskRepository commandTaskRepository, IUnitOfWork uow)
            : base(uow) 
        {
            this.queryTaskRepository = queryTaskRepository;
            this.commandTaskRepository = commandTaskRepository;
        }

        protected override async Task<Response> TryHandle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            TaskEntity task = await queryTaskRepository.GetAsync(GetByIdSpecification<TaskEntity>.Create(request.TaskId), cancellationToken);

            if(task is null)
            {
                return Issue.CreateNew(ErrorConstants.TaskNotFoundCode, ErrorConstants.TaskNotFoundDesc);
            }
            
            var body = request.Body;
            ValidationObject<TaskEntity> validationObject = task.TryUpdate(body.Title, body.Description, body.DueDate, body.Progress);
            if(validationObject.HasIssue)
            {
                return validationObject.Issue;
            }

            TaskEntity updatedEntity = await commandTaskRepository.UpdateAsync(validationObject.Entity, cancellationToken);
            return Response.FromData(TaskQueryResponse.ToModel(updatedEntity));
        }
    }
}
