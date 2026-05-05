using Inwentaryzator_paczkomatow_Api.Api.Controllers;
using Inwentaryzator_paczkomatow_Api.Api.Dto;
using Inwentaryzator_paczkomatow_Api.Application.Requests;
using Inwentaryzator_paczkomatow_Api.Application.Responses;
using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smartLockerFinderApi.Application.Responses;
using smartLockerFinderApi.Domain.Entities;

namespace smartLockerFinderApiTests.Api.Controllers
{
    public class ParcelLockerControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ParcelLockerController _controller;

        public ParcelLockerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ParcelLockerController(_mediatorMock.Object);
        }

        [Fact]
        public async Task FetchParcelLockerDataAsync_ReturnsOk_WhenDataIsPresent()
        {
            // Arrange
            var requestDto = new ParcelLockerDataDTO
            {
                Location = new LocationData { Latitude = 50.0, Longitude = 19.0, Limit = 10 },
                FilterFunctions = new LockerFunctionsFilter(false, false)
            };

            var responseData = new List<ParcelLockerDataResponse>
            {
                new ParcelLockerDataResponse { Name = "Locker1", City = "Krakow", Street = "Dluga", Location = new LocationData() }
            };

            var expectedResponse = new FetchParcelLockerDataResponse(null, responseData);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<FetchParcelLockerDataRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.FetchParcelLockerDataAsync(requestDto, CancellationToken.None) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var resultValue = result.Value as FetchParcelLockerDataResponse;
            Assert.NotNull(resultValue);
            Assert.Null(resultValue.ErrorMessage);
            Assert.Single(resultValue.ParcelLockerData!);
        }

        [Fact]
        public async Task FetchParcelLockerDataAsync_ReturnsOkWithResponse_WhenErrorOccurs()
        {
             // Arrange
            var requestDto = new ParcelLockerDataDTO
            {
                Location = new Inwentaryzator_paczkomatow_Api.Domain.Entities.LocationData { Latitude = 50.0, Longitude = 19.0, Limit = 10 },
                FilterFunctions = new LockerFunctionsFilter(false, false)
            };

            var expectedResponse = new FetchParcelLockerDataResponse("Error fetching", null);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<FetchParcelLockerDataRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.FetchParcelLockerDataAsync(requestDto, CancellationToken.None) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var resultValue = result.Value as FetchParcelLockerDataResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Error fetching", resultValue.ErrorMessage);
            Assert.Null(resultValue.ParcelLockerData);
        }
    }
}
