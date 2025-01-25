using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Commentaries.Commands;
using EclipseWorks.Application.Commentaries.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;

namespace EclipseWorks.Application.Commentaries.Handlers
{
    public class AddCommentaryCommandHandler : BaseCommandHandler<AddCommentaryCommand, Response>
    {
        private readonly IQueryTaskRepository queryTaskRepository;
        private readonly ICommandTaskRepository commandTaskRepository;

        public AddCommentaryCommandHandler(IQueryTaskRepository queryTaskRepository, ICommandTaskRepository commandTaskRepository, IUnitOfWork uow) 
            : base(uow) 
        {
            this.queryTaskRepository = queryTaskRepository;
            this.commandTaskRepository = commandTaskRepository;
        }

        protected override async Task<Response> TryHandle(AddCommentaryCommand request, CancellationToken cancellationToken)
        {
            ISpecification<TaskEntity> spec = GetByIdSpecification<TaskEntity>.Create(request.TaskId);
            TaskEntity task = await queryTaskRepository.GetAsync(spec, cancellationToken);

            if (task is null)
            {
                return Issue.CreateNew(ErrorConstants.TaskNotFoundCode, ErrorConstants.TaskNotFoundDesc);
            }

            ValidationObject<TaskEntity> validationObject = task.AddCommentary(request.Body.Commentary, request.Body.LoggedUserId);
            if (validationObject.HasIssue)
            {
                return validationObject.Issue;
            }

            var entity = await commandTaskRepository.UpdateAsync(validationObject.Entity, cancellationToken);
            return Response.FromData(CommentaryQueryResponse.ToModel(entity.Commentaries));
        }
    }
}
