using Dapper;
using VkService.Application.Abstractions;
using VkService.Data;
using VkService.Models;

namespace VkService.Application.Implementation;

public sealed class UserQueryService : IUsersQueryService
{
    private readonly IDbConnectionFactory _factory;

    public UserQueryService(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<int>> GetAll(CancellationToken cancellationToken)
    {
        await using var connection = _factory.GetConnection();
        return await connection.QueryAsync<int>(new("select Id from Users", cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<VkUserModel>> GetFullUsers()
    {
        await using var connection = _factory.GetConnection();
        return await connection.QueryAsync<VkUserModel>(new("select Id, Avatar, Name from Users"));
    }

    public async Task Add(int id, string name, string avatar)
    {
        await using var connection = _factory.GetConnection();

        await connection.ExecuteAsync(new(
            "INSERT OR IGNORE INTO Users (Id, Avatar, Name) VALUES (@id, @avatar, @name)",
            new { id, avatar, name }));
    }

    public async Task Delete(int id)
    {
        await using var connection = _factory.GetConnection();
        await connection.ExecuteAsync(new("DELETE FROM Users WHERE Id = @id", new { id }));
    }
}
