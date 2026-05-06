using Inwentaryzator_paczkomatow_Api.Application.Requests;
using Inwentaryzator_paczkomatow_Api.Application.Responses;
using MediatR;
using smartLockerFinderApi.Application.Client.Interface;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Application.Responses;
using smartLockerFinderApi.Domain.Intefaces;
using System.Globalization;

namespace Inwentaryzator_paczkomatow_Api.Application.Handlers;

public class FetchParcelLockerDataRequestHandler : IRequestHandler<FetchParcelLockerDataRequest, FetchParcelLockerDataResponse>
{
    private readonly HttpClient _httpClient;
    private readonly IParcelLockerService _parcelLockerService;
    private readonly IInpostClientWrapper _inpostClientWrapper;

    public FetchParcelLockerDataRequestHandler(HttpClient httpClient, IParcelLockerService parcelLockerService, IInpostClientWrapper inpostClientWrapper)
    {
        _httpClient = httpClient;
        _parcelLockerService = parcelLockerService;
        _inpostClientWrapper = inpostClientWrapper;
    }

    public async Task<FetchParcelLockerDataResponse> Handle(FetchParcelLockerDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var unfilteredParcelLockers = await _inpostClientWrapper.GetParcelLockerData(request.Request.Location, cancellationToken);

            if (unfilteredParcelLockers?.Items == null || !unfilteredParcelLockers.Items.Any())
            {
                return Failure("Nie znaleziono punktów w podanej lokalizacji.");
            }

            var filteredParcelLockers = _parcelLockerService.FilterLockersByFunctions(unfilteredParcelLockers.Items, request.Request.FilterFunctions);

            var responseItems = filteredParcelLockers.Select(i => new ParcelLockerDataResponse
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