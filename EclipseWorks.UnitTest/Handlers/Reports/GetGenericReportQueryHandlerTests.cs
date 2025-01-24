using EclipseWorks.Application.Reports.Handlers;
using EclipseWorks.Application.Reports.Queries;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Interfaces.Specification;
using EclipseWorks.Domain.Reports.Interfaces;
using EclipseWorks.Domain.Reports.Models;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using Moq;

namespace EclipseWorks.UnitTest.Handlers.Reports
{
    public class GetGenericReportQueryHandlerTests
    {
        private readonly Mock<IQueryReportRepository> _mockQueryReportRepository;
        private readonly Mock<IQueryUserRepository> _mockQueryUserRepository;
        private readonly GetGenericReportQueryHandler _handler;

        public GetGenericReportQueryHandlerTests()
        {
            _mockQueryReportRepository = new Mock<IQueryReportRepository>();
            _mockQueryUserRepository = new Mock<IQueryUserRepository>();
            _handler = new GetGenericReportQueryHandler(_mockQueryReportRepository.Object, _mockQueryUserRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnReport_WhenParametersAreValid()
        {
            // Arrange
            var request = new GetGenericReportQuery (Guid.NewGuid(), DateTime.Now.AddDays(-10), DateTime.Now);
            var cancellationToken = CancellationToken.None;
            var user = new UserEntity(Guid.NewGuid()) { Role = "gerente" };
            var report = new GenericReportModel();


            _mockQueryUserRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<UserEntity>>(), cancellationToken))
                                     .ReturnsAsync(user);

            _mockQueryReportRepository.Setup(repo => repo.GenerateReport(request.StartingDate!.Value, request.EndingDate!.Value, cancellationToken))
                                       .ReturnsAsync(report);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equivalent(report, response.Data, true);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenUserNotAuthorized()
        {
            // Arrange
            var request = new GetGenericReportQuery(Guid.NewGuid(), DateTime.Now.AddDays(-10), DateTime.Now);
            var cancellationToken = CancellationToken.None;
            var user = new UserEntity(Guid.NewGuid()) { Role = "developer" };

            _mockQueryUserRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<UserEntity>>(), cancellationToken))
                                     .ReturnsAsync(user);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.NotAuthorizedCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.NotAuthorizedDesc, response.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnIssue_WhenInvalidDateRange()
        {
            // Arrange
            var request = new GetGenericReportQuery(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(-10));
            var cancellationToken = CancellationToken.None;
            var user = new UserEntity(Guid.NewGuid()) { Role = "gerente" };

            _mockQueryUserRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<UserEntity>>(), cancellationToken))
                                     .ReturnsAsync(user);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(ErrorConstants.NullEndDateCode, response.Errors.First().Code);
            Assert.Equal(ErrorConstants.NullEndDateDesc, response.Errors.First().Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnResponse_WhenNoDatesArePassed()
        {
            // Arrange
            var request = new GetGenericReportQuery(Guid.NewGuid(), null, null);

            var cancellationToken = CancellationToken.None;
            var user = new UserEntity(Guid.NewGuid()) { Role = "gerente" };
            var report = new GenericReportModel();

            _mockQueryUserRepository.Setup(repo => repo.GetAsync(It.IsAny<ISpecification<UserEntity>>(), cancellationToken))
                                     .ReturnsAsync(user);

            _mockQueryReportRepository.Setup(repo => repo.GenerateReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken))
                                       .ReturnsAsync(report);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equivalent(report, response.Data, true);
            _mockQueryReportRepository.Verify(repo => repo.GenerateReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), cancellationToken), Times.Once);
        }
    }
}
