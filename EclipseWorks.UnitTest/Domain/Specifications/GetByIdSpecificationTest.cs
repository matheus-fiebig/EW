using EclipseWorks.Domain._Shared.Entities;
using EclipseWorks.Domain._Shared.Specifications;
using EclipseWorks.Domain.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.UnitTest.Domain.Specifications
{
    public class GetByIdSpecificationTest
    {

        [Fact]
        public void GetByIdSpecification_ShouldReturnItem_WhenIdIsPassedAndExistsInList()
        {
            //Arrange
            var id = Guid.NewGuid();
            var sut = GetByIdSpecification< TaskEntity>.Create(id);
            var itens = new List<TaskEntity>()
            {
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(id)
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Single(item);
            Assert.Equal(item.First().Id, id);
        }

        [Fact]
        public void GetByIdSpecification_ShouldReturnEmpty_WhenNotItensAreFound()
        {
            //Arrange
            var id = Guid.Empty;
            var sut = GetByIdSpecification<TaskEntity>.Create(id);
            var itens = new List<TaskEntity>()
            {
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid()),
                new TaskEntity(Guid.NewGuid())
            };

            //Act
            var item = itens.Where(sut.Predicate.Compile());

            //Assert    
            Assert.NotNull(item);
            Assert.Empty(item);
        }
    }
}
