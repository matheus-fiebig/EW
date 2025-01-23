﻿using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Commentaries.Commands;
using EclipseWorks.Application.Commentaries.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using MediatR;

namespace EclipseWorks.Application.Commentaries.Handlers
{
    public class AddCommentaryCommandHandler : IRequestHandler<AddCommentaryCommand, Response>
    {
        private readonly IQueryTaskRepository queryTaskRepository;
        private readonly ICommandTaskRepository commandTaskRepository;

        public AddCommentaryCommandHandler(IQueryTaskRepository queryTaskRepository, ICommandTaskRepository commandTaskRepository)
        {
            this.queryTaskRepository = queryTaskRepository;
            this.commandTaskRepository = commandTaskRepository;
        }

        public async Task<Response> Handle(AddCommentaryCommand request, CancellationToken cancellationToken)
        {
            ISpecification<TaskEntity> spec = GetByIdSpecification<TaskEntity>.Create(request.Body.TaskId);
            TaskEntity task = await queryTaskRepository.GetAsync(spec, cancellationToken);
            
            if(task is null)
            {
                return Issue.CreateNew(ErrorConstants.TaskNotFoundCode, ErrorConstants.TaskNotFoundDesc);
            }

            ValidationObject<TaskEntity> validationObject = task.AddCommentary(request.Body.Commentary, request.Body.UserId);
            if(validationObject.HasIssue)
            {
                return validationObject.Issue;
            }

            var entity = await commandTaskRepository.UpdateAsync(validationObject.Entity, cancellationToken);
            return Response.FromData(CommentaryQueryResponse.ToModel(entity.Commentaries.LastOrDefault()));
        }
    }
}
