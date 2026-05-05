using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Domain.Entities;
using smartLockerFinderApi.Domain.Services;

namespace smartLockerFinderApiTests.Domain.Services
{
    public class ParcelLockerServiceTests
    {
        private readonly ParcelLockerService _service;

        public ParcelLockerServiceTests()
        {
            _service = new ParcelLockerService();
        }

        [Fact]
        public void FilterLockersByFunctions_ReturnEnabled_FiltersCorrectly()
        {
            // Arrange
            var lockers = new List<ParcelLockerItemDto>
            {
                new ParcelLockerItemDto { Name = "Locker1", Functions = new[] { "parcel_reverse_return_send" }, Location = new LocationData() },
                new ParcelLockerItemDto { Name = "Locker2", Functions = new[] { "other_function" }, Location = new LocationData() }
            };
            var filter = new LockerFunctionsFilter(true, false);

            // Act
            var result = _service.FilterLockersByFunctions(lockers, filter).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Locker1", result[0].Name);
        }

        [Fact]
        public void FilterLockersByFunctions_AllegroDelivery_FiltersCorrectly()
        {
            // Arrange
            var lockers = new List<ParcelLockerItemDto>
            {
                new ParcelLockerItemDto { Name = "Locker1", Functions = new[] { "allegro" }, Location = new LocationData() },
                new ParcelLockerItemDto { Name = "Locker2", Functions = new[] { "other_function" }, Location = new LocationData() }
            };
            var filter = new LockerFunctionsFilter(false, true);

            // Act
            var result = _service.FilterLockersByFunctions(lockers, filter).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Locker1", result[0].Name);
        }

        [Fact]
        public void FilterLockersByFunctions_BothFilters_FiltersCorrectly()
        {
            // Arrange
            var lockers = new List<ParcelLockerItemDto>
            {
                new ParcelLockerItemDto { Name = "Locker1", Functions = new[] { "allegro", "parcel_reverse_return_send" }, Location = new LocationData() },
                new ParcelLockerItemDto { Name = "Locker2", Functions = new[] { "allegro" }, Location = new LocationData() },
                new ParcelLockerItemDto { Name = "Locker3", Functions = new[] { "other_function" }, Location = new LocationData() }
            };
            var filter = new LockerFunctionsFilter(true, true);

            // Act
            var result = _service.FilterLockersByFunctions(lockers, filter).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Locker1", result[0].Name);
        }

        [Fact]
        public void BuildPointsUrl_BuildsCorrectUrl_WithLocation()
        {
            // Arrange
            var location = new LocationData { Latitude = 52.2296756, Longitude = 21.0122287, Limit = 50 };

            // Act
            var result = _service.BuildPointsUrl(location);

            // Assert
            Assert.Contains("relative_point=52.2296756,21.0122287", result);
            Assert.Contains("page=1", result);
            Assert.Contains("per_page=50", result);
        }
    }
}

