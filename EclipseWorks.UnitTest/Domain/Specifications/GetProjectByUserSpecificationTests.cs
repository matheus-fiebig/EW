using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Specification;
using EclipseWorks.Domain.Users.Entities;

namespace EclipseWorks.UnitTest.Domain.Specifications
{
    public class GetProjectByUserSpecificationTest
    {

        [Fact]
        public void GetProjectByUserSpecification_ShouldReturnProject_WhenProjectContainsUser()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var sut = GetProjectByUserSpecification.Create(userId);

            var projectId1 = Guid.NewGuid();
            var projectId2 = Guid.NewGuid();
            var itens = new List<ProjectEntity>()
            {
                new ProjectEntity(projectId1) { Users = [new UserEntity(userId), new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid())] },
                new ProjectEntity(Guid.NewGuid()) { Users = [new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid())] },
                new ProjectEntity(projectId2) { Users = [new UserEntity(userId)] }
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Equal(2, item.Count());
            Assert.Equal(item.First().Id, projectId1);
            Assert.Equal(item.Last().Id, projectId2);
        }

        [Fact]
        public void GetProjectByUserSpecification_ShouldReturnEmptyProjects_WhenUserIsNotFound()
        {
            //Arrange
            var id = Guid.Empty;
            var sut = GetByIdSpecification<ProjectEntity>.Create(id);

            var itens = new List<ProjectEntity>()
            {
                new ProjectEntity(Guid.NewGuid()) { Users = new List<UserEntity>{ new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid()) } },
                new ProjectEntity(Guid.NewGuid()) { Users = new List<UserEntity>{ new UserEntity(Guid.NewGuid()), new UserEntity(Guid.NewGuid()) } },
                new ProjectEntity(Guid.NewGuid()) { Users = new List<UserEntity>{ new UserEntity(Guid.NewGuid()) } }
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Empty(item);
        }
    }
}
