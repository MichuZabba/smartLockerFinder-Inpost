using Inwentaryzator_paczkomatow_Api.Api.Dto;
using Inwentaryzator_paczkomatow_Api.Application.Handlers;
using Inwentaryzator_paczkomatow_Api.Application.Requests;
using Inwentaryzator_paczkomatow_Api.Application.Responses;
using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using Moq;
using Moq.Protected;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Domain.Entities;
using smartLockerFinderApi.Domain.Intefaces;
using System.Net;
using System.Text.Json;


namespace smartLockerFinderApiTests.Application.Handlers
{
    public class FetchParcelLockerDataRequestHandlerTests
    {
        private readonly Mock<IParcelLockerService> _serviceMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly FetchParcelLockerDataRequestHandler _handler;

        public FetchParcelLockerDataRequestHandlerTests()
        {
            _serviceMock = new Mock<IParcelLockerService>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new System.Uri("https://api-global-points.easypack24.net/")
            };

            _handler = new FetchParcelLockerDataRequestHandler(_httpClient, _serviceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenDataIsPresent()
        {
            // Arrange
            var location = new LocationData { Latitude = 50.0, Longitude = 19.0 };
            var requestDto = new ParcelLockerDataDTO
            {
                Location = location,
                FilterFunctions = new LockerFunctionsFilter(false, false)
            };
            var request = new FetchParcelLockerDataRequest(requestDto);

            _serviceMock.Setup(s => s.BuildPointsUrl(location))
                .Returns("https://api.test/points");

            var mockApiResponse = new ParcelLockerApiResponse
            {
                Items = new List<ParcelLockerItemDto>
                {
                    new ParcelLockerItemDto 
                    { 
                        Name = "Locker1", 
                        AddressDetails = new AdressDetails { City = "Krakow", Street = "Dluga" },
                        Location = new LocationData()
                    }
                }
            };

            var jsonResponse = JsonSerializer.Serialize(mockApiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            _serviceMock.Setup(s => s.FilterLockersByFunctions(It.IsAny<IEnumerable<ParcelLockerItemDto>>(), It.IsAny<LockerFunctionsFilter>()))
                .Returns(mockApiResponse.Items);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ErrorMessage);
            Assert.NotNull(result.ParcelLockerData);
            Assert.Single(result.ParcelLockerData);
            Assert.Equal("Locker1", result.ParcelLockerData.First().Name);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenNoPointsFound()
        {
            // Arrange
            var location = new LocationData { Latitude = 50.0, Longitude = 19.0 };
            var requestDto = new ParcelLockerDataDTO
            {
                Location = location,
                FilterFunctions = new LockerFunctionsFilter(false, false)
            };
            var request = new FetchParcelLockerDataRequest(requestDto);

            _serviceMock.Setup(s => s.BuildPointsUrl(location))
                .Returns("https://api.test/points");

            var mockApiResponse = new ParcelLockerApiResponse
            {
                Items = new List<ParcelLockerItemDto>() // Empty list
            };

            var jsonResponse = JsonSerializer.Serialize(mockApiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Nie znaleziono punktów w podanej lokalizacji.", result.ErrorMessage);
            Assert.Null(result.ParcelLockerData);
        }
    }
}