using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Handlers;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain.Tasks.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Tasks
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<ICommandTaskRepository> _mockCommandTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _mockCommandTaskRepository = new Mock<ICommandTaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteTaskCommandHandler(_mockCommandTaskRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenNoIdIsPassed()
        {
            // Arrange
            var command = new DeleteTaskCommand(default);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ErrorConstants.InvalidRequestCode, result.Errors.First().Code);
            Assert.Equal(ErrorConstants.InvalidRequestDesc, result.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenTaskIsDeleted()
        {
            // Arrange
            var command = new DeleteTaskCommand(Guid.NewGuid());


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result); 
            Assert.Null(result.Data); 
            Assert.Null(result.Errors); 
        }
    }
}
