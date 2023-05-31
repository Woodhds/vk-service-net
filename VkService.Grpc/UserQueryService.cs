using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VkService.Application.Abstractions;

namespace VkService.Grpc;

public sealed class UserQueryService : UsersService.UsersServiceBase
{
    private readonly IUsersQueryService _userQueryService;

    public UserQueryService(IUsersQueryService userQueryService)
    {
        _userQueryService = userQueryService;
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
}