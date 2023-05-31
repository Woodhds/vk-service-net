using Microsoft.EntityFrameworkCore;
using VkService.Data;
using VkService.Grpc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(x => x
    .UseSqlite(builder.Configuration.GetConnectionString("DataContext"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddGrpc()
    .AddJsonTranscoding();

var app = builder.Build();

app.MapGrpcService<MessageQueryService>();
app.MapGrpcService<UserQueryService>();

await app.RunAsync();