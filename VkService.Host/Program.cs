using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkService.Application.Abstractions;
using VkService.Application.Implementation;
using VkService.Client.Abstractions;
using VkService.Client.Extensions;
using VkService.Data;
using VkService.Grpc;
using VkService.Parsers;
using VkService.Parsers.Abstractions;
using MessageQueryService = VkService.Application.Implementation.MessageQueryService;
using UserQueryService = VkService.Grpc.UserQueryService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDbConnectionFactory>(x =>
    new DbConnectionFactory(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddGrpc()
    .AddJsonTranscoding();

builder.Services.AddVkClients(builder.Configuration);
builder.Services.AddTransient<IMessagesSaveService, MessagesSaveService>();
builder.Services.AddSingleton<IMessageParser, SiteParser>();
builder.Services.AddSingleton<IMessageParser, UserParser>();
builder.Services.AddSingleton<ParsersService>(x =>
    new ParsersService(
        x.GetRequiredService<IVkWallService>(),
        x.GetRequiredService<IMessagesSaveService>(),
        x.GetServices<IMessageParser>()));
builder.Services.AddScoped<IMessagesQueryService, MessageQueryService>();
builder.Services.AddScoped<IUsersQueryService, VkService.Application.Implementation.UserQueryService>();


var app = builder.Build();

app.MapGrpcService<VkService.Grpc.MessageQueryService>();
app.MapGrpcService<UserQueryService>();
app.MapGrpcService<ParsersService>();
app.MapGrpcService<MessageQueryService>();
app.MapGrpcService<VkService.Grpc.GroupsService>();

await app.RunAsync();
