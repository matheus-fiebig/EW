using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Handlers;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain._Shared.Models;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Tasks
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<IQueryTaskRepository> _mockQueryTaskRepository;
        private readonly Mock<ICommandTaskRepository> _mockCommandTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _mockQueryTaskRepository = new Mock<IQueryTaskRepository>();
            _mockCommandTaskRepository = new Mock<ICommandTaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new UpdateTaskCommandHandler(_mockQueryTaskRepository.Object, _mockCommandTaskRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenTaskNotFound()
        {
            // Arrange
            var command = new UpdateTaskCommand(Guid.NewGuid(), null);

            _mockQueryTaskRepository
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<TaskEntity>>(), CancellationToken.None))
                .ReturnsAsync((TaskEntity)null!); 

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ErrorConstants.TaskNotFoundCode, result.Errors.First().Code);
            Assert.Equal(ErrorConstants.TaskNotFoundDesc, result.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenCantUpdateTask()
        {
            // Arrange
            var command = new UpdateTaskCommand(Guid.NewGuid(), new UpdateTaskRequest("", null, default, default, default, default));
            var task = new TaskEntity(Guid.NewGuid()); 

            _mockQueryTaskRepository
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<TaskEntity>>(), CancellationToken.None))
                .ReturnsAsync(task);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ErrorConstants.TitleNullCode, result.Errors.First().Code);
            Assert.Equal(ErrorConstants.TitleNullDesc, result.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdateTask_WhenNoIssuesAreFound ()
        {
            // Arrange
            var command = new UpdateTaskCommand(Guid.NewGuid(), new UpdateTaskRequest("title", "desc", DateTime.Now.AddDays(1), EProgress.Done, Guid.NewGuid(), Guid.NewGuid()));

            var task = new TaskEntity(Guid.NewGuid()) { Title = "Old Title", Description = "Old Description", Commentaries = [] };
            var updatedTask = new TaskEntity(Guid.NewGuid()) { Title = "Updated Title", Description = "Updated Description", Commentaries = [] };

            _mockQueryTaskRepository
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<TaskEntity>>(), CancellationToken.None))
                .ReturnsAsync(task);

            _mockCommandTaskRepository
                .Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>(), CancellationToken.None))
                .ReturnsAsync(updatedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(TaskQueryResponse.ToModel(updatedTask), result.Data); 
        }
    }
}
