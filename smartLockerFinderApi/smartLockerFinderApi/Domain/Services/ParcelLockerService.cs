using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using MediatR;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Domain.Entities;
using smartLockerFinderApi.Domain.Intefaces;
using System.Globalization;

namespace smartLockerFinderApi.Domain.Services;

public class ParcelLockerService : IParcelLockerService
{
    private const string BaseUrl = "https://api-global-points.easypack24.net/v1/points";

    public IEnumerable<ParcelLockerItemDto> FilterLockersByFunctions(IEnumerable<ParcelLockerItemDto> parcelLockers, LockerFunctionsFilter filters)
    {
        var filteredItems = parcelLockers;

        if (filters.ReturnEnabled)
        {
            filteredItems = filteredItems.Where(i => i.Functions != null
                         && i.Functions.Contains("parcel_reverse_return_send", StringComparer.OrdinalIgnoreCase));
        }

        if (filters.AllegroDelivery)
        {
            filteredItems = filteredItems.Where(i => i.Functions != null
                         && i.Functions.Any(f => f.Contains("allegro", StringComparison.OrdinalIgnoreCase)));
        }

        return filteredItems;
    }
    public string BuildPointsUrl(LocationData location)
    {
        var queryParams = new List<string>();

        int safeLimit = Math.Clamp(location.Limit ?? 100, 1, 300);

        if (location != null)
        {
            var lat = location.Latitude.ToString(CultureInfo.InvariantCulture);
            var lon = location.Longitude.ToString(CultureInfo.InvariantCulture);
            queryParams.Add($"relative_point={lat},{lon}");
        }

        queryParams.Add("page=1");
        queryParams.Add($"per_page={safeLimit}");

        return $"{BaseUrl}?{string.Join("&", queryParams)}";
    }
}

