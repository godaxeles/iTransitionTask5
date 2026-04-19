namespace Task5.Models;

public class CoverParams
{
    public long Seed { get; set; }
    
    public int Index { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Artist { get; set; } = string.Empty;
}
