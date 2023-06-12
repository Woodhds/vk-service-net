using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VkService.Application.Abstractions;
using VkService.Client.Abstractions;

namespace VkService.Grpc;

public sealed class UserQueryService : UsersService.UsersServiceBase
{
    private readonly IUsersQueryService _userQueryService;
    private readonly IVkUserService _userService;

    public UserQueryService(IUsersQueryService userQueryService, IVkUserService userService)
    {
        _userQueryService = userQueryService;
        _userService = userService;
    }

    public override async Task<Empty> Add(VkUserProto request, ServerCallContext context)
    {
        await _userQueryService.Add(request.Id, request.Name, request.Avatar);

        return null!;
    }

    public override async Task<Empty> Delete(DeleteUserRequest request, ServerCallContext context)
    {
        await _userQueryService.Delete(request.Id);

        return null!;
    }

    public override async Task<GetUsersResponse> GetUsers(Empty request, ServerCallContext context)
    {
        return new GetUsersResponse
        {
            Users =
            {
                (await _userQueryService.GetFullUsers()).Select(f => new VkUserProto
                {
                    Id = f.Id,
                    Avatar = f.Avatar,
                    Name = f.Name
                })
            }
        };
    }

    public override async Task<UserSearchResponse> Search(UserSearchRequest request, ServerCallContext context)
    {
        var users = await _userService.SearchUserAsync(request.Search);
        return new UserSearchResponse
        {
            Users =
            {
                users.Response?.Items.Select(f => new VkUserProto
                {
                    Id = f.Id, 
                    Avatar = f.Photo50, 
                    Name = $"{f.LastName} {f.FirstName}"
                })
            }
        };
    }
}
