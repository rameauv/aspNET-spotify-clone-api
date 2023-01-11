using System.ComponentModel.DataAnnotations;

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
    public IEnumerable<T> Items { get; }
    [Required]
    public int Limit { get; }
    [Required]
    public int Offset { get; }
    [Required]
    public int Total { get; }
}