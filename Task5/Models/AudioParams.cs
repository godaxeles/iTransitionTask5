using System.ComponentModel.DataAnnotations;

namespace Task5.Models;

public class AudioParams
{
    public long Seed { get; set; }

    [Range(1, int.MaxValue)]
    public int Index { get; set; }
}
