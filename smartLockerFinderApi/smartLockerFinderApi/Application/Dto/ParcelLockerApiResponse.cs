using System.Text.Json.Serialization;

namespace smartLockerFinderApi.Application.Dto;

public record ParcelLockerApiResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("items")]
    public IReadOnlyList<ParcelLockerItemDto> Items { get; set; } = Array.Empty<ParcelLockerItemDto>();
}