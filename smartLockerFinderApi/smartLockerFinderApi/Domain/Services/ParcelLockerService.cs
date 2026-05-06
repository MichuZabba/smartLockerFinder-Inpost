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

    public IList<ParcelLockerItemDto> FilterLockersByFunctions(
        IEnumerable<ParcelLockerItemDto> parcelLockers,
        LockerFunctionsFilter filters)
    {

        if (filters.ReturnEnabled)
        {
            parcelLockers = parcelLockers.Where(i =>
                i.Functions.Contains("parcel_reverse_return_send", StringComparer.OrdinalIgnoreCase));
        }

        if (filters.AllegroDelivery)
        {
            parcelLockers = parcelLockers.Where(i =>
                i.Functions.Any(f => f.Contains("allegro", StringComparison.OrdinalIgnoreCase)));
        }

        return parcelLockers.ToList();
    }

    public string BuildPointsUrl(LocationData location)
    {
        var queryParams = new List<string>();

        var lat = location.Latitude.ToString(CultureInfo.InvariantCulture);
        var lon = location.Longitude.ToString(CultureInfo.InvariantCulture);

        queryParams.Add($"relative_point={lat},{lon}");

        return $"{BaseUrl}?{string.Join("&", queryParams)}";
    }
}