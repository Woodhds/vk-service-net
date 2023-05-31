using System.ComponentModel.DataAnnotations.Schema;

namespace VkService.Data.Entities;

public class VkUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    
    public string? Avatar { get; set; }
    
    public string? Name { get; set; }
}