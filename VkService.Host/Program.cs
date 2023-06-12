using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddDbContextFactory<DataContext>(x => x
    .UseSqlite(builder.Configuration.GetConnectionString("DataContext"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddGrpc()
    .AddJsonTranscoding();

builder.Services.AddVkClients(builder.Configuration);
builder.Services.AddTransient<IMessagesSaveService, MessagesSaveService>();
builder.Services.AddSingleton<IMessageParser, SiteParser>();
builder.Services.AddScoped<IMessageParser, UserParser>();
builder.Services.AddSingleton<ParsersService>(x =>
    new ParsersService(
        x.GetRequiredService<IVkWallService>(), 
        x.CreateScope().ServiceProvider.GetServices<IMessageParser>(),
        x.GetRequiredService<IMessagesSaveService>()));
builder.Services.AddScoped<IMessagesQueryService, MessageQueryService>();


var app = builder.Build();

app.MapGrpcService<VkService.Grpc.MessageQueryService>();
app.MapGrpcService<UserQueryService>();
app.MapGrpcService<ParsersService>();
app.MapGrpcService<MessageQueryService>();

await app.RunAsync();
