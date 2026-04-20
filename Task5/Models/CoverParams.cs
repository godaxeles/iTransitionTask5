using System.ComponentModel.DataAnnotations;

namespace Task5.Models;

public class CoverParams
{
    public long Seed { get; set; }

    [Range(1, int.MaxValue)]
    public int Index { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;

    [Range(0, 13)]
    public int Genre { get; set; }
}
