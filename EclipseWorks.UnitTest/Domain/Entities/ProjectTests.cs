using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.UnitTest.Domain.Entities
{
    public class ProjectTests
    {
        [Fact]
        public void TryCreateNew_ShouldReturnProjectEntity_WhenValidInputIsProvided()
        {
            // Arrange
            var name = "Project Alpha";
            var description = "This is a test project.";
            var users = new List<UserEntity> { new UserEntity(Guid.NewGuid()) };

            // Act
            var result = ProjectEntity.TryCreateNew(name, description, users);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasIssue);
            Assert.Equal(name, result.Entity.Name);
            Assert.Equal(description, result.Entity.Description);
            Assert.Equal(users.Count, result.Entity.Users.Count);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenNameIsNull()
        {
            // Arrange
            string name = null;
            var description = "This is a test project.";
            var users = new List<UserEntity> { new UserEntity(Guid.NewGuid()) };

            // Act
            var result = ProjectEntity.TryCreateNew(name, description, users);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.TitleNullCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenUsersListIsNull()
        {
            // Arrange
            var name = "Project Alpha";
            var description = "This is a test project.";
            List<UserEntity> users = null;

            // Act
            var result = ProjectEntity.TryCreateNew(name, description, users);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.ProjectMinUserCode, result.Issue.Code);
        }

        [Fact]
        public void TryCreateNew_ShouldReturnIssue_WhenUsersListIsEmpty()
        {
            // Arrange
            var name = "Project Alpha";
            var description = "This is a test project.";
            var users = new List<UserEntity>();

            // Act
            var result = ProjectEntity.TryCreateNew(name, description, users);

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.ProjectMinUserCode, result.Issue.Code);
        }

        [Fact]
        public void VerifyDeletionEligibility_ShouldReturnUnit_WhenAllTasksAreDone()
        {
            // Arrange
            var tasks = new List<TaskEntity>
            {
                new TaskEntity(Guid.Empty) { Progress = EProgress.Done },
                new TaskEntity(Guid.Empty) { Progress = EProgress.Done }
            };
            var project = new ProjectEntity(Guid.Empty) { Tasks = tasks };

            // Act
            var result = project.VerifyDeletionEligibility();

            // Assert
            Assert.False(result.HasIssue);
        }

        [Fact]
        public void VerifyDeletionEligibility_ShouldReturnIssue_WhenSomeTasksAreNotDone()
        {
            // Arrange
            var tasks = new List<TaskEntity>
            {
                new TaskEntity(Guid.Empty) { Progress = EProgress.Done },
                new TaskEntity(Guid.Empty) { Progress = EProgress.InProgress }
            };
            var project = new ProjectEntity(Guid.Empty) { Tasks = tasks };

            // Act
            var result = project.VerifyDeletionEligibility();

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.ProjectDeletionCode, result.Issue.Code);
        }

        [Fact]
        public void VerifyAddTaskEligibility_ShouldReturnUnit_WhenTasksAreLessThanLimit()
        {
            // Arrange
            var tasks = Enumerable.Range(1, 10).Select(_ => new TaskEntity(Guid.Empty)).ToList();
            var project = new ProjectEntity(Guid.Empty) { Tasks = tasks };

            // Act
            var result = project.VerifyAddTaskEligibility();

            // Assert
            Assert.False(result.HasIssue);
        }

        [Fact]
        public void VerifyAddTaskEligibility_ShouldReturnIssue_WhenTasksExceedLimit()
        {
            // Arrange
            var tasks = Enumerable.Range(1, 21).Select(_ => new TaskEntity(Guid.Empty)).ToList();
            var project = new ProjectEntity(Guid.Empty) { Tasks = tasks };

            // Act
            var result = project.VerifyAddTaskEligibility();

            // Assert
            Assert.True(result.HasIssue);
            Assert.Equal(ErrorConstants.TaskLimitExceededCode, result.Issue.Code);
        }
    }
}
