using EclipseWorks.Application.Projects.Commands;
using EclipseWorks.Application.Projects.Handlers;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Tasks.Entities;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Projects
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly Mock<ICommandProjectRepository> _mockCommandProjectRepository;
        private readonly Mock<IQueryProjectRepository> _mockQueryProjectRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteProjectCommandHandler _handler;

        public DeleteProjectCommandHandlerTests()
        {
            _mockCommandProjectRepository = new Mock<ICommandProjectRepository>();
            _mockQueryProjectRepository = new Mock<IQueryProjectRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteProjectCommandHandler(_mockQueryProjectRepository.Object, _mockCommandProjectRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteProject_WhenAllTasksAreDone()
        {
            // Arrange
            var request = new DeleteProjectCommand (Guid.NewGuid());
            var cancellationToken = CancellationToken.None;
            var projectEntity = new ProjectEntity(Guid.NewGuid()) { Tasks = [] };

            _mockQueryProjectRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(projectEntity);

            _mockCommandProjectRepository.Setup(repo => repo.DeleteAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                          .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.Data);
            Assert.Null(response.Errors);

            _mockCommandProjectRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenNoProjectIsFound()
        {
            // Arrange
            var request = new DeleteProjectCommand(Guid.NewGuid());
            var cancellationToken = CancellationToken.None;

            _mockQueryProjectRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync((ProjectEntity)null!);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.ProjectNotFoundCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.ProjectNotFoundDesc, response.Errors.First().Description);

            _mockCommandProjectRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenThereIsAnyUncompletedTask()
        {
            // Arrange
            var request = new DeleteProjectCommand(Guid.NewGuid());
            var cancellationToken = CancellationToken.None;
            var projectEntity = new ProjectEntity(Guid.NewGuid()) { Tasks = [new TaskEntity(Guid.NewGuid()) { Progress = EProgress.InProgress }] };
            
            _mockQueryProjectRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(projectEntity);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.ProjectDeletionCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.ProjectDeletionDesc, response.Errors.First().Description);

            _mockCommandProjectRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken), Times.Never);
        }
    }
}
