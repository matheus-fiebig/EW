using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Histories.Events;

namespace EclipseWorks.UnitTest.Domain.Entities
{
    public class CommentaryTests
    {
        [Fact]
        public void TryCreateNew_ShouldReturnValidationObjectWithEntity_WhenValidInputIsProvided()
        {
            // Arrange
            var validCommentary = "This is a valid commentary.";
            var validUserId = Guid.NewGuid();
            var validTaskId = Guid.NewGuid();

            // Act
            var result = CommentaryEntity.TryCreateNew(validCommentary, validUserId, validTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasIssue);
            Assert.NotNull(result.Entity);
            Assert.Equal(validCommentary, result.Entity.Description);
            Assert.Equal(validUserId, result.Entity.UserId);
            Assert.Equal(validTaskId, result.Entity.TaskId);
            Assert.Single(result.Entity.Events);
            Assert.IsType<AddHistoryDomainEvent>(result.Entity.Events[0]);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenCommentaryIsNull()
        {
            // Arrange
            string invalidCommentary = null!;
            var validUserId = Guid.NewGuid();
            var validTaskId = Guid.NewGuid();

            // Act
            var result = CommentaryEntity.TryCreateNew(invalidCommentary, validUserId, validTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasIssue);
            Assert.NotNull(result.Issue);
            Assert.Equal(ErrorConstants.CommentaryNullCode, result.Issue.Code);
            Assert.Equal(ErrorConstants.CommentaryNullDesc, result.Issue.Description);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenCommentaryIsEmpty()
        {
            // Arrange
            var invalidCommentary = string.Empty;
            var validUserId = Guid.NewGuid();
            var validTaskId = Guid.NewGuid();

            // Act
            var result = CommentaryEntity.TryCreateNew(invalidCommentary, validUserId, validTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasIssue);
            Assert.NotNull(result.Issue);
            Assert.Equal(ErrorConstants.CommentaryNullCode, result.Issue.Code);
            Assert.Equal(ErrorConstants.CommentaryNullDesc, result.Issue.Description);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenUserIdIsDefault()
        {
            // Arrange
            var validCommentary = "This is a valid commentary.";
            var invalidUserId = Guid.Empty;
            var validTaskId = Guid.NewGuid();

            // Act
            var result = CommentaryEntity.TryCreateNew(validCommentary, invalidUserId, validTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasIssue);
            Assert.NotNull(result.Issue);
            Assert.Equal(ErrorConstants.UserNotFoundCode, result.Issue.Code);
            Assert.Equal(ErrorConstants.UserNotFoundDesc, result.Issue.Description);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenTaskIdIsDefault()
        {
            // Arrange
            var validCommentary = "This is a valid commentary.";
            var validUserId = Guid.NewGuid();
            var invalidTaskId = Guid.Empty;

            // Act
            var result = CommentaryEntity.TryCreateNew(validCommentary, validUserId, invalidTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasIssue);
            Assert.NotNull(result.Issue);
            Assert.Equal(ErrorConstants.TaskNotFoundCode, result.Issue.Code);
            Assert.Equal(ErrorConstants.TaskNotFoundDesc, result.Issue.Description);
        }

        [Fact]
        public void TryCreateNew_ShouldSetCorrectCreatedAtDate_WhenValidInputIsProvided()
        {
            // Arrange
            var validCommentary = "This is a valid commentary.";
            var validUserId = Guid.NewGuid();
            var validTaskId = Guid.NewGuid();
            var beforeCreationTime = DateTime.Now;

            // Act
            var result = CommentaryEntity.TryCreateNew(validCommentary, validUserId, validTaskId);
            var afterCreationTime = DateTime.Now;

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasIssue);
            Assert.NotNull(result.Entity);
            Assert.True(result.Entity.CreatedAt >= beforeCreationTime && result.Entity.CreatedAt <= afterCreationTime);
        }

        [Fact]
        public void TryCreateNew_ShouldAddDomainEvent_WhenValidInputIsProvided()
        {
            // Arrange
            var validCommentary = "This is a valid commentary.";
            var validUserId = Guid.NewGuid();
            var validTaskId = Guid.NewGuid();

            // Act
            var result = CommentaryEntity.TryCreateNew(validCommentary, validUserId, validTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasIssue);
            Assert.NotNull(result.Entity);
            Assert.Single(result.Entity.Events);

            var domainEvent = result.Entity.Events[0];
            Assert.IsType<AddHistoryDomainEvent>(domainEvent);

            var historyEvent = (AddHistoryDomainEvent)domainEvent;
            Assert.Equal("Commentary", historyEvent.OriginTableName);
            Assert.Equal(validUserId, historyEvent.CreatedBy);
            Assert.Equal(EModificationType.Created, historyEvent.Type);
            Assert.Same(result.Entity, historyEvent.Changes);
        }
    }
}
