namespace Task5.Models;

public class GenerationParams
{
    public int Page { get; set; } = 1;
    
    public long Seed { get; set; }
    
    public string Locale { get; set; } = "en";
    
    public double Likes { get; set; }
    
    public int PageSize { get; set; } = 10;
    
    public int SafePageSize => Math.Clamp(PageSize, 1, 100);
}
