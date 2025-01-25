using EclipseWorks.Application.Projects.Commands;
using EclipseWorks.Application.Projects.Handlers;
using EclipseWorks.Application.Projects.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Projects
{
    public class CreateProjectCommandHandlerTests
    {
        private readonly Mock<ICommandProjectRepository> _mockCommandProjectRepository;
        private readonly Mock<IQueryUserRepository> _mockQueryUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests()
        {
            _mockCommandProjectRepository = new Mock<ICommandProjectRepository>();
            _mockQueryUserRepository = new Mock<IQueryUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateProjectCommandHandler(_mockCommandProjectRepository.Object, _mockQueryUserRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateNewProject_WhenNoIssuesAreFound()
        {
            // Arrange
            var request = new CreateProjectCommand(new CreateProjectRequest("Test Project", "Test Description", [Guid.NewGuid(), Guid.NewGuid()]));
            var project = new ProjectEntity(Guid.NewGuid());

            _mockQueryUserRepository.Setup(repo => repo.GetPagedAsync(-1, -1, It.IsAny<ISpecification<UserEntity>>(), CancellationToken.None))
                                     .ReturnsAsync([new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid())]);

            _mockCommandProjectRepository.Setup(repo => repo.InsertAsync(It.IsAny<ProjectEntity>(), CancellationToken.None))
                                          .ReturnsAsync(project);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equivalent(ProjectQueryResponse.ToModel(project), response.Data, true);
            _mockCommandProjectRepository.Verify(repo => repo.InsertAsync(It.IsAny<ProjectEntity>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TryHandle_ValidationFails_ReturnsError()
        {
            // Arrange
            var request = new CreateProjectCommand(new CreateProjectRequest(null, "Test Description", [Guid.NewGuid(), Guid.NewGuid()]));
            var cancellationToken = CancellationToken.None;
         
            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.TitleNullCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.TitleNullDesc, response.Errors.First().Description);
            _mockCommandProjectRepository.Verify(repo => repo.InsertAsync(It.IsAny<ProjectEntity>(), cancellationToken), Times.Never);
        }
    }
}
