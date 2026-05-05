using Inwentaryzator_paczkomatow_Api.Domain.Entities;

namespace smartLockerFinderApi.Application.Responses;
public record ParcelLockerDataResponse
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public LocationData Location { get; set; }
}
