using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Reports.Queries;
using EclipseWorks.Controllers;
using EclipseWorks.Domain._Shared.Constants;
using EclipseWorks.Domain._Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace EclipseWorks.UnitTest.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Mock<ISender> _mediatorMock;
        private readonly ReportsControlller _controller;

        public ReportsControllerTests()
        {
            _mediatorMock = new Mock<ISender>();
            _controller = new ReportsControlller(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetGenericReportData_ShouldReturnOk_WhenMediatorReturnsOkResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var from = DateTime.UtcNow.AddDays(-1);
            var to = DateTime.UtcNow;

            var expectedResponse = Response.FromData(new { Message = "Success" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetGenericReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetGenericReportData(userId, from, to);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetGenericReportData_ShouldReturnNoContent_WhenMediatorReturnsNoContentResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var from = DateTime.UtcNow.AddDays(-1);
            var to = DateTime.UtcNow;

            var expectedResponse = Response.Empty();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetGenericReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetGenericReportData(userId, from, to);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetGenericReportData_ShouldReturnBadRequest_WhenMediatorReturnsBadRequestResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var from = DateTime.UtcNow.AddDays(-1);
            var to = DateTime.UtcNow;

            var expectedResponse = Response.FromError(Issue.CreateNew("Invalid data", "Invalid data"));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetGenericReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetGenericReportData(userId, from, to);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedResponse, badRequestResult.Value);
        }

        [Fact]
        public async Task GetGenericReportData_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var from = DateTime.UtcNow.AddDays(-1);
            var to = DateTime.UtcNow;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetGenericReportQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetGenericReportData(userId, from, to);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal(ErrorConstants.GenericBadRequestErrorDesc, problemDetails.Instance);
        }
    }
}
