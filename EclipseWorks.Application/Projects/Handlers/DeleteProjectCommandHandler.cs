using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Projects.Commands;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;


namespace EclipseWorks.Application.Projects.Handlers
{
    public class DeleteProjectCommandHandler : BaseCommandHandler<DeleteProjectCommand, Response>
    {
        private readonly IQueryProjectRepository queryProjectRepository;
        private readonly ICommandProjectRepository commandProjectRepository;

        public DeleteProjectCommandHandler(IQueryProjectRepository queryProjectRepository, ICommandProjectRepository commandProjectRepository, IUnitOfWork uow)
            : base(uow)
        {
            this.queryProjectRepository = queryProjectRepository;
            this.commandProjectRepository = commandProjectRepository;
        }

        protected override async Task<Response> TryHandle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            ISpecification<ProjectEntity> spec = GetByIdSpecification<ProjectEntity>.Create(request.ProjectId);
            ProjectEntity project = await queryProjectRepository.GetAsync(spec, cancellationToken);
            if (project is null)
            {
                return Response.FromError(Issue.CreateNew(ErrorConstants.ProjectNotFoundCode, ErrorConstants.ProjectNotFoundDesc));
            }

            ValidationObject<Unit> value = project.VerifyDeletionEligibility();

            if (value.HasIssue)
            {
                return Response.FromError(value.Issue);
            }

            await commandProjectRepository.DeleteAsync(spec, cancellationToken);
            return Response.Empty();
        }
    }
}
