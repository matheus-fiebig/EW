using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Specification;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Specifications;

namespace EclipseWorks.UnitTest.Domain.Specifications
{
    public class GetByUsersIdSpecificationTests 
    {
        [Fact]
        public void GetByUsersIdSpecification_ShouldReturnUsers_WhenTheyExists()
        {
            //Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var sut = GetByUsersIdSpecification.Create([userId1, userId2]);

            var itens = new List<UserEntity>()
            {
                new (userId1),
                new (Guid.NewGuid()),
                new (Guid.NewGuid())
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Equal(1, item.Count());
            Assert.Equal(item.First().Id, userId1);
        }

        [Fact]
        public void GetByUsersIdSpecification_ShouldReturnEmpty_WhenListIsNull()
        {
            //Arrange
            var sut = GetByUsersIdSpecification.Create(null);

            var itens = new List<UserEntity>()
            {
                new (Guid.NewGuid()),
                new (Guid.NewGuid()),
                new (Guid.NewGuid())
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Equal(0, item.Count());
        }
    }
}
