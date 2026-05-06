using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using smartLockerFinderApi.Application.Dto;

namespace smartLockerFinderApi.Application.Client.Interface
{
    public interface IInpostClientWrapper
    {
        Task<ParcelLockerApiResponse> GetParcelLockerData(LocationData location, CancellationToken cancellationToken);
    }
}
