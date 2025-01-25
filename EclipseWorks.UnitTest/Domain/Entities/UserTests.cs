using EclipseWorks.Domain.Commentaries.Entities;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.UnitTest.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void Create_ShouldReturnUserEntity_WhenValidInputsAreProvided()
        {
            // Arrange
            var name = "John Doe";
            var role = "Administrator";

            // Act
            var result = UserEntity.Create(name, role);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(role, result.Role);
            Assert.NotEqual(default, result.CreatedAt);
            Assert.Empty(result.Projects ?? new List<ProjectEntity>());
            Assert.Empty(result.Tasks ?? new List<TaskEntity>());
            Assert.Empty(result.Commentaries ?? new List<CommentaryEntity>());
        }

        [Fact]
        public void Create_ShouldAllowEmptyRole()
        {
            // Arrange
            var name = "Jane Doe";
            var role = string.Empty;

            // Act
            var result = UserEntity.Create(name, role);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(role, result.Role); // Role is empty
            Assert.NotEqual(default, result.CreatedAt);
        }

        [Fact]
        public void Create_ShouldHandleNullRole()
        {
            // Arrange
            var name = "Jane Doe";
            string role = null;

            // Act
            var result = UserEntity.Create(name, role);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Null(result.Role); // Role is null
            Assert.NotEqual(default, result.CreatedAt);
        }

        [Fact]
        public void Create_ShouldSetCreatedAtToCurrentDateTime()
        {
            // Arrange
            var name = "John Smith";
            var role = "Manager";
            var before = DateTime.Now;

            // Act
            var result = UserEntity.Create(name, role);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.CreatedAt >= before && result.CreatedAt <= DateTime.Now);
        }

        [Fact]
        public void InternalConstructor_ShouldCreateEntityWithGivenId()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var user = new UserEntity(userId);

            // Assert
            Assert.Equal(userId, user.Id);
        }
    }
}
