using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using smartLockerFinderApi.Application.Dto;
using smartLockerFinderApi.Domain.Entities;

namespace smartLockerFinderApi.Domain.Intefaces;

public interface IParcelLockerService
{
    IList<ParcelLockerItemDto> FilterLockersByFunctions(IEnumerable<ParcelLockerItemDto> parcelLockers, LockerFunctionsFilter filters);
    string BuildPointsUrl(LocationData location);
}

