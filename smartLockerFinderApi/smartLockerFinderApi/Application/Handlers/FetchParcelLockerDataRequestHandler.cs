using Inwentaryzator_paczkomatow_Api.Application.Requests;
using Inwentaryzator_paczkomatow_Api.Application.Responses;
using MediatR;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Application.Responses;
using smartLockerFinderApi.Domain.Intefaces;
using System.Globalization;

namespace Inwentaryzator_paczkomatow_Api.Application.Handlers;

public class FetchParcelLockerDataRequestHandler : IRequestHandler<FetchParcelLockerDataRequest, FetchParcelLockerDataResponse>
{
    private readonly HttpClient _httpClient;
    private readonly IParcelLockerService _parcelLockerService;

    public FetchParcelLockerDataRequestHandler(HttpClient httpClient, IParcelLockerService parcelLockerService)
    {
        _httpClient = httpClient;
        _parcelLockerService = parcelLockerService;
    }

    public async Task<FetchParcelLockerDataResponse> Handle(FetchParcelLockerDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var url = _parcelLockerService.BuildPointsUrl(request.Request.Location);
            var responseData = await _httpClient.GetFromJsonAsync<ParcelLockerApiResponse>(url, cancellationToken);

            if (responseData?.Items == null || !responseData.Items.Any())
            {
                return Failure("Nie znaleziono punktów w podanej lokalizacji.");
            }

            var allItems = responseData.Items.AsEnumerable();

            var filteredItems = _parcelLockerService.FilterLockersByFunctions(allItems, request.Request.FilterFunctions);

            var responseItems = filteredItems.Select(i => new ParcelLockerDataResponse
            {
                Name = i.Name ?? "",
                City = i.AddressDetails?.City ?? "",
                Street = i.AddressDetails?.Street ?? "",
                Location = i.Location
            });

            return Success(responseItems);
        }
        catch (Exception ex)
        {
            return Failure($"Wystąpił błąd: {ex.Message}");
        }
    }

    public FetchParcelLockerDataResponse Failure(string errorMessage) => new(errorMessage, null);
    public FetchParcelLockerDataResponse Success(IEnumerable<ParcelLockerDataResponse> data) => new(null, data);
}