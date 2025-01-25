using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Projects.Queries;
using EclipseWorks.Application.Users.Queries;
using EclipseWorks.Controllers;
using EclipseWorks.Domain._Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EclipseWorks.UnitTest.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<ISender> _mediatorMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediatorMock = new Mock<ISender>();
            _controller = new UsersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk_WhenMediatorReturnsOkResponse()
        {
            // Arrange
            var expectedResponse = Response.FromData(new[] { new { UserId = Guid.NewGuid(), Name = "John Doe" } });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnNoContent_WhenMediatorReturnsEmptyResponse()
        {
            // Arrange
            var expectedResponse = Response.Empty();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllProjects_ShouldReturnOk_WhenMediatorReturnsOkResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedResponse = Response.FromData(new[]
            {
            new { ProjectId = Guid.NewGuid(), Name = "Project A" },
            new { ProjectId = Guid.NewGuid(), Name = "Project B" }
        });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProjectsByUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllProjects(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal(ErrorConstants.GenericBadRequestErrorDesc, problemDetails.Instance);
        }
    }
}
