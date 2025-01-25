using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Histories.Events;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;

namespace EclipseWorks.UnitTest.Domain.Entities
{
    public class TaskTests
    {
        [Fact]
        public void TryCreateNew_ShouldReturnTaskEntity_WhenAllInputsAreValid()
        {
            // Arrange
            string title = "New Task";
            string description = "Task description";
            DateTime dueDate = DateTime.Now.AddDays(5);
            EPriority priority = EPriority.Medium;
            var project = new ProjectEntity(Guid.NewGuid());
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, project, ownerId, userId);

            // Assert
            Assert.False(result.HasIssue);
            Assert.Equal(title, result.Entity.Title);
            Assert.Equal(description, result.Entity.Description);
            Assert.Equal(dueDate, result.Entity.DueDate);
            Assert.Equal(priority, result.Entity.Priority);
            Assert.Equal(EProgress.Todo, result.Entity.Progress);
            Assert.Equal(project.Id, result.Entity.ProjectId);
            Assert.Equal(ownerId, result.Entity.OwnerId);
        }

        [Theory]
        [InlineData(null, ErrorConstants.TitleNullCode)]
        [InlineData("", ErrorConstants.TitleNullCode)]
        public void TryCreateNew_ShouldReturnIssue_WhenTitleIsInvalid(string title, string expectedCode)
        {
            // Arrange
            string description = "Task description";
            DateTime dueDate = DateTime.Now.AddDays(5);
            EPriority priority = EPriority.Medium;
            var project = new ProjectEntity(Guid.NewGuid());
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, project, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(expectedCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenDueDateIsNull()
        {
            // Arrange
            string title = "New Task";
            string description = "Task description";
            DateTime? dueDate = null;
            EPriority priority = EPriority.Medium;
            var project = new ProjectEntity(Guid.NewGuid());
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, project, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.InvalidDueDateCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenPriorityIsNull()
        {
            // Arrange
            string title = "New Task";
            string description = "Task description";
            DateTime dueDate = DateTime.Now.AddDays(5);
            EPriority? priority = null;
            var project = new ProjectEntity(Guid.NewGuid());
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, project, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.PriorityNullCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenProjectIsNull()
        {
            // Arrange
            string title = "New Task";
            string description = "Task description";
            DateTime dueDate = DateTime.Now.AddDays(5);
            EPriority? priority = EPriority.Low;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, null, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.ProjectNotFoundCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenDueDateIsInvalid()
        {
            // Arrange
            string title = "New Task";
            string description = "Task description";
            DateTime dueDate = DateTime.Now.AddDays(-5);
            EPriority? priority = EPriority.Low;
            var project = new ProjectEntity(Guid.NewGuid());
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, project, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.DueDateNotAllowedCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenPriorityIsInvalid()
        {
            // Arrange
            string title = "New Task";
            string description = "Task description";
            DateTime dueDate = DateTime.Now.AddDays(5);
            EPriority? priority = (EPriority)4;
            var project = new ProjectEntity(Guid.NewGuid());
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = TaskEntity.TryCreateNew(title, description, dueDate, priority, project, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.PriorityNotFoundCode, result.Issue.Code);
        }

        [Fact]
        public void TryUpdate_ShouldUpdateTask_WhenAllInputsAreValid()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty)
            {
                Title = "Old Title",
                Description = "Old Description",
                DueDate = DateTime.Now.AddDays(5),
                Progress = EProgress.Todo,
                OwnerId = Guid.NewGuid(),
            };

            string newTitle = "Updated Title";
            string newDescription = "Updated Description";
            DateTime? newDueDate = task.DueDate;
            EProgress newProgress = EProgress.InProgress;
            Guid newOwnerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(newTitle, newDescription, newDueDate, newProgress, newOwnerId, userId);

            // Assert
            Assert.False(result.HasIssue);
            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
            Assert.Equal(newProgress, task.Progress);
            Assert.Equal(newOwnerId, task.OwnerId);
        }

        [Theory]
        [InlineData(null, ErrorConstants.TitleNullCode)]
        [InlineData("", ErrorConstants.TitleNullCode)]
        public void TryUpdate_ShouldReturnIssue_WhenTitleIsNullOrEmpty(string invalidTitle, string expectedErrorCode)
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty);
            string description = "Description";
            DateTime? dueDate = DateTime.Now.AddDays(5);
            EProgress progress = EProgress.Todo;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(invalidTitle, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(expectedErrorCode, result.Issue.Code);
        }

        [Fact]
        public void TryUpdate_ShouldReturnIssue_WhenDueDateIsNull()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty);
            string title = "Title";
            string description = "Description";
            DateTime? dueDate = null;
            EProgress progress = EProgress.Todo;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(title, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.InvalidDueDateCode, result.Issue.Code);
        }

        [Fact]
        public void TryUpdate_ShouldReturnIssue_WhenDueDateIsInThePast()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty);
            string title = "Title";
            string description = "Description";
            DateTime? dueDate = DateTime.Now.AddDays(-1);
            EProgress progress = EProgress.Todo;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(title, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.DueDateNotAllowedCode, result.Issue.Code);
        }

        [Fact]
        public void TryUpdate_ShouldReturnIssue_WhenProgressIsNull()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty);
            string title = "Title";
            string description = "Description";
            DateTime? dueDate = DateTime.Now.AddDays(5);
            EProgress? progress = null;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(title, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.ProgressNullCode, result.Issue.Code);
        }

        [Fact]
        public void TryUpdate_ShouldReturnIssue_WhenProgressIsInvalid()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty);
            string title = "Title";
            string description = "Description";
            DateTime? dueDate = DateTime.Now.AddDays(5);
            EProgress? progress = (EProgress)999;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(title, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.ProgressNotFoundCode, result.Issue.Code);
        }

        [Fact]
        public void TryUpdate_ShouldSetDoneDate_WhenProgressIsDone()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty)
            {
                Progress = EProgress.Todo
            };

            string title = "Title";
            string description = "Description";
            DateTime? dueDate = DateTime.Now.AddDays(5);
            EProgress progress = EProgress.Done;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(title, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.False(result.HasIssue);
            Assert.NotNull(task.DoneDate);
            Assert.Equal(EProgress.Done, task.Progress);
        }

        [Fact]
        public void TryUpdate_ShouldTriggerEvent_WhenTaskIsUpdated()
        {
            // Arrange
            var task = new TaskEntity(Guid.Empty);
            string title = "Title";
            string description = "Description";
            DateTime? dueDate = DateTime.Now.AddDays(5);
            EProgress progress = EProgress.InProgress;
            Guid ownerId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.TryUpdate(title, description, dueDate, progress, ownerId, userId);

            // Assert
            Assert.False(result.HasIssue);
            Assert.NotEmpty(task.Events);
            var domainEvent = task.Events[0] as AddHistoryDomainEvent;
            Assert.NotNull(domainEvent);
            Assert.Equal("Task", domainEvent.OriginTableName);
            Assert.Equal(userId, domainEvent.CreatedBy);
            Assert.Equal(EModificationType.Updated, domainEvent.Type);
        }

        [Fact]
        public void AddCommentary_ShouldAddCommentary_WhenInputsAreValid()
        {
            // Arrange
            var task = new TaskEntity(Guid.NewGuid())
            {
                Commentaries = new List<CommentaryEntity>(),
            };

            string commentary = "New Commentary";
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.AddCommentary(commentary, userId);

            // Assert
            Assert.False(result.HasIssue);
            Assert.Single(task.Commentaries);
            Assert.Equal(commentary, task.Commentaries[0].Description);
            Assert.Equal(userId, task.Commentaries[0].UserId);
        }

        [Fact]
        public void AddCommentary_ShouldReturnIssue_WhenCommentaryIsInvalid()
        {
            // Arrange
            var task = new TaskEntity(Guid.NewGuid())
            {
                Commentaries = new List<CommentaryEntity>(),
            };

            string commentary = "";
            Guid userId = Guid.NewGuid();

            // Act
            var result = task.AddCommentary(commentary, userId);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.CommentaryNullCode, result.Issue.Code);
        }
    }
}
