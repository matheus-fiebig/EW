using EclipseWorks.Domain._Shared.Enums;
using EclipseWorks.Domain.Histories.Entities;
using System.Text.Json;

namespace EclipseWorks.UnitTest.Domain.Entities
{
    public class HistoryTests
    {
        [Fact]
        public void TryCreateNew_ShouldReturnHistoryEntity_WhenValidInputIsProvided()
        {
            // Arrange
            var createdBy = Guid.NewGuid();
            var originTableName = "TestTable";
            var changes = new { Field1 = "Value1", Field2 = 42 };
            var modificationType = EModificationType.Updated;

            // Act
            var result = HistoryEntity.TryCreateNew(createdBy, originTableName, changes, modificationType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdBy, result.CreatedBy);
            Assert.Equal(originTableName, result.OriginTableName);
            Assert.Equal(modificationType, result.Type);
            Assert.NotNull(result.Changes);
            Assert.True(result.CreatedAt <= DateTime.Now && result.CreatedAt > DateTime.Now.AddSeconds(-1));
            Assert.Equal(JsonSerializer.Serialize(changes), result.Changes);
        }

        [Fact]
        public void TryCreateNew_ShouldThrowArgumentNullException_WhenCreatedByIsNull()
        {
            // Arrange
            Guid? createdBy = null;
            var originTableName = "TestTable";
            var changes = new { Field1 = "Value1", Field2 = 42 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HistoryEntity.TryCreateNew(createdBy, originTableName, changes));
        }

        [Fact]
        public void TryCreateNew_ShouldThrowArgumentNullException_WhenOriginTableNameIsNull()
        {
            // Arrange
            var createdBy = Guid.NewGuid();
            string originTableName = null;
            var changes = new { Field1 = "Value1", Field2 = 42 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HistoryEntity.TryCreateNew(createdBy, originTableName, changes));
        }

        [Fact]
        public void TryCreateNew_ShouldThrowArgumentNullException_WhenOriginTableNameIsEmpty()
        {
            // Arrange
            var createdBy = Guid.NewGuid();
            var originTableName = string.Empty;
            var changes = new { Field1 = "Value1", Field2 = 42 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => HistoryEntity.TryCreateNew(createdBy, originTableName, changes));
        }

        [Fact]
        public void TryCreateNew_ShouldThrowArgumentNullException_WhenChangesIsNull()
        {
            // Arrange
            var createdBy = Guid.NewGuid();
            var originTableName = "TestTable";
            object changes = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HistoryEntity.TryCreateNew(createdBy, originTableName, changes));
        }
    }
}
