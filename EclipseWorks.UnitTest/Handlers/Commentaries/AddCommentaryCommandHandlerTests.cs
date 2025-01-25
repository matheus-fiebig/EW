using EclipseWorks.Application.Commentaries.Commands;
using EclipseWorks.Application.Commentaries.Handlers;
using EclipseWorks.Application.Commentaries.Models;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Interfaces;
using EclipseWorks.Domain.Users.Entities;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Commentaries
{
    public class AddCommentaryCommandHandlerTests 
    {
        private readonly Mock<IQueryTaskRepository> _mockQueryTaskRepository;
        private readonly Mock<ICommandTaskRepository> _mockCommandTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        private readonly AddCommentaryCommandHandler _sut;

        public AddCommentaryCommandHandlerTests()
        {
            _mockQueryTaskRepository = new Mock<IQueryTaskRepository>();
            _mockCommandTaskRepository = new Mock<ICommandTaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _sut = new AddCommentaryCommandHandler(_mockQueryTaskRepository.Object, _mockCommandTaskRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnsTaskNotFoundIssue_WhenNoTaskIsFound()
        {
            // Arrange
            var command = new AddCommentaryCommand(Guid.NewGuid(), new AddCommentaryRequest("Test Comment", Guid.NewGuid()));

            _mockQueryTaskRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<TaskEntity>>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync((TaskEntity)null!);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ErrorConstants.TaskNotFoundCode, result.Errors.First().Code);
            Assert.Equal(ErrorConstants.TaskNotFoundDesc, result.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnsValidationErrors_WhenTaskIsFoundButRequestIsWrong()
        {
            // Arrange
            var command = new AddCommentaryCommand(Guid.NewGuid(), new AddCommentaryRequest(null, Guid.NewGuid()));

            _mockQueryTaskRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<TaskEntity>>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(new TaskEntity(Guid.NewGuid()));

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ErrorConstants.CommentaryNullCode, result.Errors.First().Code);
            Assert.Equal(ErrorConstants.CommentaryNullDesc, result.Errors.First().Description);
        }


        [Fact]
        public async Task Handle_ShouldReturnsAddedCommentaries_WhenCommentaryIsInsertedSuccessfully()
        {
            // Arrange
            var command = new AddCommentaryCommand(Guid.NewGuid(), new AddCommentaryRequest("Big commentary", Guid.NewGuid()));
            var task = new TaskEntity(Guid.NewGuid()) { Commentaries = new() };

            var userUpdated = new UserEntity(Guid.NewGuid()) { Name = "Cleyton"};
            var taskUpdated = new TaskEntity(Guid.NewGuid()) { Commentaries = [new CommentaryEntity(Guid.NewGuid()) { User = userUpdated, Description = command.Body.Commentary }] };

            _mockQueryTaskRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<TaskEntity>>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(task);

            _mockCommandTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskEntity>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(taskUpdated);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Errors);
            Assert.Equivalent(result.Data, CommentaryQueryResponse.ToModel(taskUpdated.Commentaries));
        }
    }
}