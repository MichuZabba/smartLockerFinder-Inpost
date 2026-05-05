using Inwentaryzator_paczkomatow_Api.Api.Dto;
using Inwentaryzator_paczkomatow_Api.Application.Responses;
using MediatR;

namespace Inwentaryzator_paczkomatow_Api.Application.Requests;

public sealed record FetchParcelLockerDataRequest(ParcelLockerDataDTO Request) : IRequest<FetchParcelLockerDataResponse>;

