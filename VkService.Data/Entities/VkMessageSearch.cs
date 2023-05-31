using System.ComponentModel.DataAnnotations.Schema;

namespace VkService.Data.Entities;

public class VkMessageSearch
{
    public string? Text { get; set; }
    public int OwnerId { get; set; }
    public int Id { get; set; }
    public VkMessage? Message { get; set; }
}