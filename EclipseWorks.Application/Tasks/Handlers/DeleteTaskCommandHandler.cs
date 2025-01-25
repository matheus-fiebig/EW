using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;

namespace EclipseWorks.Application.Tasks.Handlers
{
    public class DeleteTaskCommandHandler : BaseCommandHandler<DeleteTaskCommand, Response>
    {
        private readonly ICommandTaskRepository commandTaskRepository;

        public DeleteTaskCommandHandler(ICommandTaskRepository commandTaskRepository, IUnitOfWork uow) : base(uow) 
        {
            this.commandTaskRepository = commandTaskRepository;
        }

        protected override async Task<Response> TryHandle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            if(request.TaskId == default)
            {
                return Issue.CreateNew(ErrorConstants.InvalidRequestCode, ErrorConstants.InvalidRequestDesc);
            }

            ISpecification<TaskEntity> spec = GetByIdSpecification<TaskEntity>.Create(request.TaskId);
            await commandTaskRepository.DeleteAsync(spec, cancellationToken);
            return Response.Empty();
        }
    }
}
