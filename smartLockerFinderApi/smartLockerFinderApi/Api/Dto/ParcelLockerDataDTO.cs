using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using smartLockerFinderApi.Domain.Entities;

namespace Inwentaryzator_paczkomatow_Api.Api.Dto;
public record ParcelLockerDataDTO
{
    public LockerFunctionsFilter FilterFunctions { get; set; }
    public LocationData Location { get; set; }
}

