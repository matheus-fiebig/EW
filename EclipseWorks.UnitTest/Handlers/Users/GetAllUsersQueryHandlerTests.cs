using EclipseWorks.Application.Users.Handlers;
using EclipseWorks.Application.Users.Models;
using EclipseWorks.Application.Users.Queries;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Users
{
    public class GetAllUsersQueryHandlerTests
    {
        private readonly Mock<IQueryUserRepository> _mockQueryUserRepository;
        private readonly GetAllUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerTests()
        {
            _mockQueryUserRepository = new Mock<IQueryUserRepository>();
            _handler = new GetAllUsersQueryHandler(_mockQueryUserRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllUsers_WhenFound()
        {
            // Arrange
            var request = new GetAllUsersQuery();
            var cancellationToken = CancellationToken.None;
            var users = new List<UserEntity>
            {
                new UserEntity(Guid.NewGuid()){  Name = "User1" },
                new UserEntity(Guid.NewGuid()) { Name = "User2" }
            };

            _mockQueryUserRepository.Setup(repo => repo.GetAllAsync(null, cancellationToken))
                                     .ReturnsAsync(users);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equivalent(UsersQueryResponse.ToModel(users), response.Data);
        }
    }
}
