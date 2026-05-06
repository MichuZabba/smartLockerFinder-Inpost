using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using Microsoft.Extensions.Logging;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Domain.Entities;
using smartLockerFinderApi.Domain.Intefaces;
using System.Globalization;

namespace smartLockerFinderApi.Domain.Services;

public class ParcelLockerService : IParcelLockerService
{
    private const string BaseUrl = "https://api-global-points.easypack24.net/v1/points";
    private readonly ILogger<ParcelLockerService> _logger;

    public ParcelLockerService(ILogger<ParcelLockerService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<ParcelLockerItemDto> FilterLockersByFunctions(
        IEnumerable<ParcelLockerItemDto> parcelLockers,
        LockerFunctionsFilter filters)
    {
        if (parcelLockers is null)
            throw new ArgumentNullException(nameof(parcelLockers), "Lista paczkomatów nie może być null.");

        if (filters is null)
            throw new ArgumentNullException(nameof(filters), "Filtry nie mogą być null.");

        try
        {
            var filteredItems = parcelLockers;

            if (filters.ReturnEnabled)
            {
                filteredItems = filteredItems.Where(i =>
                    i.Functions != null &&
                    i.Functions.Contains("parcel_reverse_return_send", StringComparer.OrdinalIgnoreCase));
            }

            if (filters.AllegroDelivery)
            {
                filteredItems = filteredItems.Where(i =>
                    i.Functions != null &&
                    i.Functions.Any(f => f.Contains("allegro", StringComparison.OrdinalIgnoreCase)));
            }

            return filteredItems.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas filtrowania paczkomatów.");
            throw new InvalidOperationException("Wystąpił błąd podczas filtrowania paczkomatów.", ex);
        }
    }

    public string BuildPointsUrl(LocationData location)
    {
        if (location is null)
            throw new ArgumentNullException(nameof(location), "Dane lokalizacji nie mogą być null.");

        try
        {
            var queryParams = new List<string>();

            var lat = location.Latitude.ToString(CultureInfo.InvariantCulture);
            var lon = location.Longitude.ToString(CultureInfo.InvariantCulture);

            queryParams.Add($"relative_point={lat},{lon}");

            return $"{BaseUrl}?{string.Join("&", queryParams)}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas budowania URL punktów.");
            throw new InvalidOperationException("Wystąpił błąd podczas budowania adresu URL.", ex);
        }
    }
}