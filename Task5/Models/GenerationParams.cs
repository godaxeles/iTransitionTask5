using System.ComponentModel.DataAnnotations;

namespace Task5.Models;

public class GenerationParams
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    public long Seed { get; set; }

    public string Locale { get; set; } = "en";

    [Range(0, 10)]
    public double Likes { get; set; }

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public int SafePageSize => Math.Clamp(PageSize, 1, 100);
}
