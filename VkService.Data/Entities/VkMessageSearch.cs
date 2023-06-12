namespace VkService.Data.Entities;

public class VkMessageSearch
{
    public int RowId { get; set; }
    public string? Text { get; set; }
    public VkMessage? Message { get; set; }
    
    public double? Rank { get; set; }
}
