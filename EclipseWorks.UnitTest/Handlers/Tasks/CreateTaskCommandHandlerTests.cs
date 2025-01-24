using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Handlers;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Tasks
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<IQueryProjectRepository> _mockQueryProjectRepository;
        private readonly Mock<ICommandTaskRepository> _mockCommandTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _mockQueryProjectRepository = new Mock<IQueryProjectRepository>();
            _mockCommandTaskRepository = new Mock<ICommandTaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateTaskCommandHandler(_mockQueryProjectRepository.Object, _mockCommandTaskRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedTask_WhenNoIssuesAreFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var request = new CreateTaskCommand(new CreateTaskRequest(projectId, Guid.NewGuid(), "New task", "Task Desc", DateTime.Now.AddDays(5), EPriority.Medium, Guid.NewGuid()));
            var cancellationToken = CancellationToken.None;
            var project = new ProjectEntity(projectId) { Tasks = [] };
            var task = new TaskEntity(Guid.NewGuid()) { Commentaries = [] };

            _mockQueryProjectRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(project);

            _mockCommandTaskRepository.Setup(repo => repo.InsertAsync(It.IsAny<TaskEntity>(), cancellationToken))
                                       .ReturnsAsync(task);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equivalent(TaskQueryResponse.ToModel(task), response.Data, true);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenProjectHasExceededTaskCapacity()
        {
            // Arrange
            var request = new CreateTaskCommand(new CreateTaskRequest(Guid.NewGuid(), Guid.NewGuid(), "New task", "Task Desc", DateTime.Now.AddDays(5), EPriority.Medium, Guid.NewGuid()));
            var project = new ProjectEntity(request.Body.ProjectId) { Tasks = Enumerable.Range(0,21).Select(_ => new TaskEntity(Guid.NewGuid())).ToList() };
            var cancellationToken = CancellationToken.None;

            _mockQueryProjectRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(project);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.TaskLimitExceededCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.TaskLimitExceededDesc, response.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenTaskCannotBeCreated()
        {
            // Arrange
            var request = new CreateTaskCommand(new CreateTaskRequest(Guid.NewGuid(), Guid.NewGuid(), null, "Task Desc", DateTime.Now.AddDays(5), EPriority.Medium, Guid.NewGuid()));
            var project = new ProjectEntity(request.Body.ProjectId) { Tasks = Enumerable.Range(0, 21).Select(_ => new TaskEntity(Guid.NewGuid())).ToList() };
            var cancellationToken = CancellationToken.None;

            _mockQueryProjectRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(project);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Errors);
            Assert.True(response.Errors.Count > 0);
        }
    }
}
