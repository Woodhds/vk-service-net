using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using VkService.Application.Abstractions;
using VkService.Client.Abstractions;
using VkService.Models;

namespace VkService.Grpc;

public sealed class MessageQueryService : MessagesService.MessagesServiceBase
{
    private readonly IMessagesQueryService _messagesQueryService;
    private readonly IVkWallService _wallService;
    private readonly IVkGroupService _vkGroupService;
    private readonly IVkLikeService _likeService;
    private readonly ILogger<MessageQueryService> _logger;

    public MessageQueryService(
        IMessagesQueryService messagesQueryService, 
        IVkWallService wallService,
        IVkGroupService vkGroupService, 
        ILogger<MessageQueryService> logger, 
        IVkLikeService likeService)
    {
        _messagesQueryService = messagesQueryService;
        _wallService = wallService;
        _vkGroupService = vkGroupService;
        _logger = logger;
        _likeService = likeService;
    }

    public override async Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    {
        var findMessages = (await _messagesQueryService.GetMessages(request.Search))
            .Select(f => new
            {
                f.Text,
                f.OwnerId,
                f.Id
            });

        var messages = await _wallService.GetById(findMessages.Select(q => new RepostMessage(q.OwnerId, q.Id)));
        var models = messages.Response?.Items.Select(q => new VkMessageModel(q, messages.Response.Groups))
            .Select(q => new VkMessageExt
            {
                Text = q.Text,
                OwnerId = q.OwnerId,
                Id = q.Id,
                Images = { q.Images },
                LikesCount = q.LikesCount,
                RepostsCount = q.RepostsCount,
                Date = q.Date.ToTimestamp(),
                UserReposted = q.UserReposted,
                Owner = q.Owner,
                FromId = q.FromId
            }) ?? ArraySegment<VkMessageExt>.Empty;

        return new GetMessagesResponse
        {
            Messages =
            {
                models
            }
        };
    }

    public override async Task<Empty> Repost(RepostMessageRequest repost, ServerCallContext context)
    {
        if (repost?.Messages == null)
            throw new ArgumentNullException(nameof(repost));

        var posts = await _wallService.GetById(repost.Messages.Select(f => new RepostMessage(f.OwnerId, f.Id)));
        var reposts = posts.Response.Items.Where(c => c.Reposts is not null).ToArray();

            try
            {
                await Task.WhenAll(posts.Response.Groups
                    .Where(c => !c.IsMember)
                    .Select(group => _vkGroupService.JoinGroup(group.Id)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        
        
        foreach (var t in reposts)
        {
            try
            {
                await _wallService.Repost(new RepostMessage(t.OwnerId, t.Id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
        
        return new Empty();
    }

    public override async Task<Empty> Like(LikeMessageRequest request, ServerCallContext context)
    {
        await _likeService.Like(new RepostMessage(request.OwnerId, request.Id));
        return new Empty();
    }
}
