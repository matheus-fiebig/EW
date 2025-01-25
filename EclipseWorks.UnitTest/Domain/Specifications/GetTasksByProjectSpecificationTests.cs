using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Tasks.Entities;
using EclipseWorks.Domain.Tasks.Specifications;

namespace EclipseWorks.UnitTest.Domain.Specifications
{
    public class GetTasksByProjectSpecificationTests
    {

        [Fact]
        public void GetTasksByProjectSpecification_ShouldReturnTasks_WhenTaskIsContainedInsideProject()
        {
            //Arrange
            var projectId = Guid.NewGuid();
            var sut = GetTasksByProjectSpecification.Create(projectId);

            var taskId1 = Guid.NewGuid();
            var taskId2 = Guid.NewGuid();
          
            var itens = new List<TaskEntity>()
            {
                new(taskId1) { Project = new ProjectEntity(projectId) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(taskId2) { Project = new ProjectEntity(projectId) }
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Equal(2, item.Count());
            Assert.Equal(item.First().Id, taskId1);
            Assert.Equal(item.Last().Id, taskId2);
        }

        [Fact]
        public void GetProjectByUserSpecification_ShouldReturnEmptyProjects_WhenUserIsNotFound()
        {
            //Arrange
            var id = Guid.Empty;
            var sut = GetTasksByProjectSpecification.Create(id);

            var itens = new List<TaskEntity>()
            {
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) },
                new(Guid.NewGuid()) { Project = new ProjectEntity(Guid.NewGuid()) }
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Empty(item);
        }
    }
}
