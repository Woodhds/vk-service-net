﻿namespace VkService.Data.Entities;

public class VkMessage
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int RepostedFrom { get; set; }
    public DateTimeOffset Date { get; set; }
    public VkMessageSearch? Content { get; set; }
}