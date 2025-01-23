using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using MediatR;

namespace EclipseWorks.Application.Tasks.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Response>
    {
        private readonly ICommandTaskRepository commandTaskRepository;

        public DeleteTaskCommandHandler(ICommandTaskRepository commandTaskRepository)
        {
            this.commandTaskRepository = commandTaskRepository;
        }

        public async Task<Response> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
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
