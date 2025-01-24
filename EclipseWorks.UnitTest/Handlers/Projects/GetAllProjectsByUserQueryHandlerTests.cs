using EclipseWorks.Application.Projects.Handlers;
using EclipseWorks.Application.Projects.Models;
using EclipseWorks.Application.Projects.Queries;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain.Projects.Entities;
using EclipseWorks.Domain.Projects.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Projects
{
    public class GetAllProjectsByUserQueryHandlerTests
    {
        private readonly Mock<IQueryProjectRepository> _mockQueryProjectRepository;
        private readonly GetAllProjectsByUserQueryHandler _handler;

        public GetAllProjectsByUserQueryHandlerTests()
        {
            _mockQueryProjectRepository = new Mock<IQueryProjectRepository>();
            _handler = new GetAllProjectsByUserQueryHandler(_mockQueryProjectRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProjectsFromUser_WhenIdIsPassedAndItExists()
        {
            // Arrange
            GetAllProjectsByUserQuery request = new GetAllProjectsByUserQuery(Guid.NewGuid());
            CancellationToken cancellationToken = CancellationToken.None;

            List<ProjectEntity> projectEntities = [new ProjectEntity(Guid.NewGuid()), new ProjectEntity(Guid.NewGuid())];
            _mockQueryProjectRepository.Setup(repo => repo.GetPagedAsync(-1, -1, It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(projectEntities);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);   
            Assert.Equivalent(ProjectQueryResponse.ToModel(projectEntities), response.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenIdIsNotPassed()
        {
            // Arrange
            var request = new GetAllProjectsByUserQuery(default);
            var cancellationToken = CancellationToken.None;

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.InvalidRequestCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.InvalidRequestDesc, response.Errors.First().Description);
        }

        [Fact]
        public async Task TryHandle_NoProjectsFound_ReturnsEmptyResponse()
        {
            // Arrange
            var request = new GetAllProjectsByUserQuery(Guid.NewGuid());
            var cancellationToken = CancellationToken.None;

            _mockQueryProjectRepository.Setup(repo => repo.GetPagedAsync(-1, -1, It.IsAny<ISpecification<ProjectEntity>>(), cancellationToken))
                                        .ReturnsAsync(new List<ProjectEntity>());

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.Errors);
            Assert.Null(response.Data);
        }
    }
}
