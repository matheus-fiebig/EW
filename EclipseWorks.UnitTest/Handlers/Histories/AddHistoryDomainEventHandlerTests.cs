using EclipseWorks.Application.Histories.Handlers;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain.Histories.Entities;
using EclipseWorks.Domain.Histories.Events;
using EclipseWorks.Domain.Histories.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Histories
{
    public class AddHistoryDomainEventHandlerTests
    {
        private readonly Mock<ICommandHistoryRepository> _mockCommandHistoryRepository;
        private readonly AddHistoryDomainEventHandler _handler;

        public AddHistoryDomainEventHandlerTests()
        {
            _mockCommandHistoryRepository = new Mock<ICommandHistoryRepository>();
            _handler = new AddHistoryDomainEventHandler(_mockCommandHistoryRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldInsertHistory_WhenNoErrorsAreFound()
        {
            // Arrange
            var notification = new AddHistoryDomainEvent("TestTable", Guid.NewGuid(), new { Value = 1 }, EModificationType.Updated);
            var cancellationToken = CancellationToken.None;

            // Act
            await _handler.TryHandle(notification, cancellationToken);

            // Assert
            _mockCommandHistoryRepository.Verify(repo => repo.InsertAsync(It.IsAny<HistoryEntity>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRequestIsPoorlyCreated()
        {
            // Arrange
            var notification = new AddHistoryDomainEvent(null, Guid.NewGuid(), new { Value = 1 }, EModificationType.Updated);
            var cancellationToken = CancellationToken.None;

            // Assert & Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _handler.TryHandle(notification, cancellationToken));
        }
    }
}
