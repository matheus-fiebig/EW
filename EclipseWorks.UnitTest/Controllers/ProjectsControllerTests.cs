using AutoFixture;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Projects.Commands;
using EclipseWorks.Application.Projects.Models;
using EclipseWorks.Application.Tasks.Queries;
using EclipseWorks.Controllers;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EclipseWorks.UnitTest.Controllers
{
    public class ProjectsControllerTests
    {
        private readonly Mock<ISender> _mediatorMock;
        private readonly ProjectsController _sut;

        private readonly IFixture _fixture;

        public ProjectsControllerTests()
        {
            _mediatorMock = new Mock<ISender>();
            _sut = new ProjectsController(_mediatorMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllTasksByProject_ReturnsOkResult_WhenMediatorReturnsOkResponse()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var response = Response.FromData("Mocked Tasks");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllTasksByProjectQuery>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.GetAllTasksByProject(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenMediatorReturnsBadRequestResponse()
        {
            // Arrange
            var request = _fixture.Create<CreateProjectRequest>();
            var response = Response.FromError(Issue.CreateNew("Invalid Data", "Invalid Data"));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProjectCommand>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenMediatorReturnsNoContentResponse()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var response = Response.Empty();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.Delete(projectId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task HandleResponse_ReturnsProblem_WhenExceptionIsThrown()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllTasksByProjectQuery>(), default))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _sut.GetAllTasksByProject(projectId);

            // Assert
            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);
          
            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Equal(ErrorConstants.GenericBadRequestErrorDesc, problemDetails.Instance);
        }
    }
}
