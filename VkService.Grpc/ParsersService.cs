using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VkService.Application.Abstractions;
using VkService.Client.Abstractions;
using VkService.Parsers.Abstractions;

namespace VkService.Grpc;

public sealed class ParsersService : ParserService.ParserServiceBase
{
    private readonly IEnumerable<IMessageParser> _parsers;
    private readonly IVkWallService _wallService;
    private readonly IMessagesSaveService _messagesService;

    public ParsersService(
        IVkWallService wallService,
        IMessagesSaveService messagesService,
        IEnumerable<IMessageParser> parsers)
    {
        _wallService = wallService;
        _parsers = parsers;
        _messagesService = messagesService;
    }

    public override Task<Empty> Parse(Empty request, ServerCallContext context)
    {
        foreach (var parser in _parsers)
        {
            Task.Run(async () =>
            {
                await foreach (var messages in parser.Parse(default))
                {
                    var response = await _wallService.GetById(messages);
                
                    if (response?.Response?.Items == null)
                        continue;

                    await _messagesService.Save(response.Response.Items, default);
                }
            });
        }

        return Task.FromResult(new Empty());
    }
}
