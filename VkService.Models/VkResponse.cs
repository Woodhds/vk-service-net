namespace VkService.Models;

public record VkResponse<T>
{
    public VkResponseItems? Response { get; set; }

    public record VkResponseItems
    {
        public int Count { get; set; }
        public T Items { get; set; }
        public List<VkGroup> Groups { get; set; }
    }
}
