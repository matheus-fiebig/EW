using EclipseWorks.Application.Tasks.Handlers;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Application.Tasks.Queries;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Tasks
{
    public class GetAllTasksByProjectQueryHandlerTests
    {
        private readonly Mock<IQueryTaskRepository> _mockQueryTaskRepository;
        private readonly GetAllTasksByProjectQueryHandler _handler;

        public GetAllTasksByProjectQueryHandlerTests()
        {
            _mockQueryTaskRepository = new Mock<IQueryTaskRepository>();
            _handler = new GetAllTasksByProjectQueryHandler(_mockQueryTaskRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenInvaldiRequest()
        {
            // Arrange
            GetAllTasksByProjectQuery command = null!; 

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ErrorConstants.InvalidRequestCode, result.Errors.First().Code);
            Assert.Equal(ErrorConstants.InvalidRequestDesc, result.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoTaskAreFound()
        {
            // Arrange
            var command = new GetAllTasksByProjectQuery(Guid.NewGuid());
            
            _mockQueryTaskRepository
                .Setup(r => r.GetPagedAsync(-1, -1, It.IsAny<ISpecification<TaskEntity>>(), CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<TaskEntity>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task Handle_ShouldReturnTasks_WhenTaskAreFound()
        {
            // Arrange
            var command = new GetAllTasksByProjectQuery(Guid.NewGuid());
            List<TaskEntity> tasks =
            [
                new TaskEntity(Guid.NewGuid()){ ProjectId = Guid.NewGuid(), Title = "Task 1", Commentaries = [] },
                new TaskEntity(Guid.NewGuid()){ ProjectId = Guid.NewGuid(), Title = "Task 1", Commentaries = [] },
            ];

            _mockQueryTaskRepository
                .Setup(r => r.GetPagedAsync(-1, -1, It.IsAny<ISpecification<TaskEntity>>(), CancellationToken.None))
                .ReturnsAsync(tasks);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(result.Data, TaskQueryResponse.ToModel(tasks));
        }
    }
}
