using Inwentaryzator_paczkomatow_Api.Api.Dto;
using Inwentaryzator_paczkomatow_Api.Domain.Entities;
using System.Text.Json.Serialization;

namespace smartLockerFinderApi.Application.Dto;
public record ParcelLockerItemDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("location")]
    public LocationData Location { get; set; }

    [JsonPropertyName("address_details")]
    public AdressDetails? AddressDetails { get; set; }

    [JsonPropertyName("functions")]
    public string[] Functions { get; set; } = [];
}
