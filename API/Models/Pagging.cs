using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models;

public class PaggingDto<T>
{
    public PaggingDto(IEnumerable<T> items, int limit, int offset, int total)
    {
        Items = items;
        Limit = limit;
        Offset = offset;
        Total = total;
    }

    [Required]
    [JsonPropertyName("id")]
    public IEnumerable<T> Items { get; }
    [Required]
    [JsonPropertyName("limit")]
    public int Limit { get; }
    [Required]
    [JsonPropertyName("offset")]
    public int Offset { get; }
    [Required]
    [JsonPropertyName("total")]
    public int Total { get; }
}