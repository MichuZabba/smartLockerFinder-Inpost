using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using MediatR;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Domain.Intefaces;

namespace smartLockerFinderApi.Application.Client.ClientWrapper;

public class InpostClientWrapper
{
    private readonly HttpClient _httpClient;
    private readonly IParcelLockerService _parcelLockerService;

    InpostClientWrapper(HttpClient httpClient, IParcelLockerService parcelLockerService)
    {
        _httpClient = httpClient;
        _parcelLockerService = parcelLockerService;
    }

    public async Task<ParcelLockerApiResponse> GetParcelLockerData(LocationData location, CancellationToken cancellationToken)
    {
        var url = _parcelLockerService.BuildPointsUrl(location);
        var responseData = await _httpClient.GetFromJsonAsync<ParcelLockerApiResponse>(url, cancellationToken);

        return responseData;
    }
}
