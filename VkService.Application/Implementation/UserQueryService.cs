using Microsoft.EntityFrameworkCore;
using VkService.Application.Abstractions;
using VkService.Data;
using VkService.Data.Entities;
using VkService.Models;

namespace VkService.Application.Implementation;

public sealed class UserQueryService : IUsersQueryService
{
    private readonly IDbContextFactory<DataContext> _factory;

    public UserQueryService(IDbContextFactory<DataContext> factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<int>> GetAll(CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);
        return await context.Users.Select(f => f.Id).ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<VkUserModel>> GetFullUsers()
    {
        await using var context = await _factory.CreateDbContextAsync();
        return await context.Users
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
        await using var context = await _factory.CreateDbContextAsync();
        await context.Users.AddAsync(new VkUser { Id = id, Avatar = avatar, Name = name });
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await using var context = await _factory.CreateDbContextAsync();
        await context.Users.Where(f => f.Id == id).ExecuteDeleteAsync();
    }
}
