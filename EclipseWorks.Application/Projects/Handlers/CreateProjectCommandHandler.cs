using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Projects.Commands;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using EclipseWorks.Domain.Users.Specifications;

namespace EclipseWorks.Application.Projects.Handlers
{
    public class CreateProjectCommandHandler : BaseCommandHandler<CreateProjectCommand, Response>
    {
        private readonly ICommandProjectRepository commandProjectRepository;

        private readonly IQueryUserRepository queryUserRepository;

        public CreateProjectCommandHandler(ICommandProjectRepository commandProjectRepository, IQueryUserRepository queryUserRepository, IUnitOfWork uow) :
            base (uow)
        {
            this.commandProjectRepository = commandProjectRepository;
            this.queryUserRepository = queryUserRepository;
        }

        protected override async Task<Response> TryHandle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<UserEntity> users = await queryUserRepository.GetPagedAsync(-1, -1, GetByUsersIdSpecification.Create(request.Body.Users), cancellationToken);
            ValidationObject<ProjectEntity> eitherIssueOrProject = ProjectEntity.TryCreateNew(request.Body.Name, request.Body.Description, users);

            if (eitherIssueOrProject.HasIssue)
            {
                await unitOfWork.RollbackTrasactionAsync();
                return Response.FromError(eitherIssueOrProject.Issue);
            }

            ProjectEntity project = await commandProjectRepository.InsertAsync(eitherIssueOrProject.Entity, cancellationToken);
            await unitOfWork.CommitTransactionAsync();
            
            return Response.FromData(project.Id);
        }
    }
}
