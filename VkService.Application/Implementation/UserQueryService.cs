using Microsoft.EntityFrameworkCore;
using VkService.Application.Abstractions;
using VkService.Data;
using VkService.Data.Entities;
using VkService.Models;

namespace VkService.Application.Implementation;

public sealed class UserQueryService : IUsersQueryService
{
    private readonly DataContext _context;

    public UserQueryService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<int>> GetAll()
    {
        return await _context.Users.Select(f => f.Id).ToArrayAsync();
    }

    public async Task<IEnumerable<VkUserModel>> GetFullUsers()
    {
        return await _context.Users
            .Select(f => new VkUserModel
            {
                Id = f.Id,
                Avatar = f.Avatar,
                Name = f.Name
            })
            .ToArrayAsync();
    }

    public async Task Add(int id, string name, string avatar)
    {
        await _context.Users.AddAsync(new VkUser { Id = id, Avatar = avatar, Name = name });
    }

    public async Task Delete(int id)
    {
        await _context.Users.Where(f => f.Id == id).ExecuteDeleteAsync();
    }
}