using Inwentaryzator_paczkomatow_Api.Api.Dto;
using Inwentaryzator_paczkomatow_Api.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inwentaryzator_paczkomatow_Api.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParcelLockerController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParcelLockerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Fetch")]
    public async Task<IActionResult> FetchParcelLockerDataAsync([FromBody] ParcelLockerDataDTO request,  CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new FetchParcelLockerDataRequest(request), cancellationToken);
        return Ok(result);
    }
}

