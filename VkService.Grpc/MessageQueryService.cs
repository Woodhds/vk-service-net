using System.Threading.Tasks;
using Grpc.Core;

namespace VkService.Grpc;

public sealed class MessageQueryService : MessagesService.MessagesServiceBase
{
    public override Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetMessagesResponse());
    }
}