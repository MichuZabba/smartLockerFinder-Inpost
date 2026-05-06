using System.Text.Json.Serialization;

namespace Inwentaryzator_paczkomatow_Api.Domain.Entities;

public record LocationData
{
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
}

