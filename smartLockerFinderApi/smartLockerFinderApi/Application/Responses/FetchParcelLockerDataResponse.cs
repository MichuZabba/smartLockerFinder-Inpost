using Inwentaryzator_paczkomatow_Api.Api.Dto;
using smartLockerFinderApi.Application.Responses;

namespace Inwentaryzator_paczkomatow_Api.Application.Responses
{
    public sealed record FetchParcelLockerDataResponse(string? ErrorMessage, IEnumerable<ParcelLockerDataResponse>? ParcelLockerData);
}