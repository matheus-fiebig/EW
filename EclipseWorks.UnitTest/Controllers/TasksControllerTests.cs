using AutoFixture;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Commentaries.Commands;
using EclipseWorks.Application.Commentaries.Models;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Controllers;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EclipseWorks.UnitTest.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ISender> _mediatorMock;
        private readonly TasksController _sut;
        private readonly IFixture _fixture;

        public TasksControllerTests()
        {
            _mediatorMock = new Mock<ISender>();
            _fixture = new Fixture();
            _sut = new TasksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenMediatorReturnsOkResponse()
        {
            // Arrange
            var request = _fixture.Create<CreateTaskRequest>();
            var expectedResponse = Response.FromData(new { TaskId = Guid.NewGuid() });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task AddCommentary_ShouldReturnOk_WhenMediatorReturnsOkResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var request = _fixture.Create<AddCommentaryRequest>();
            var expectedResponse = Response.FromData(new { CommentaryId = Guid.NewGuid() });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AddCommentaryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.AddCommentary(taskId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenMediatorReturnsNoContentResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var request = _fixture.Create<UpdateTaskRequest>();
            var expectedResponse = Response.Empty();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.Update(taskId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenMediatorReturnsNoContentResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var expectedResponse = Response.Empty();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.Delete(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenMediatorReturnsBadRequestResponse()
        {
            // Arrange
            var request = _fixture.Create<CreateTaskRequest>();
            var error = Issue.CreateNew("400", "Invalid request");
            var expectedResponse = Response.FromError(error);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedResponse, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _sut.Delete(taskId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal(ErrorConstants.GenericBadRequestErrorDesc, problemDetails.Instance);
        }
    }
}
