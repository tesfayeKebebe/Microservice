using CommandService;
using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => { option.UseInMemoryDatabase("commandMemo"); });
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MessageBusSubscriber>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapPost("api/command/platform", ([FromBody] PlatformReadDto platform) => { }).WithName("CreateCommand")
    .WithOpenApi();

app.MapGet("api/command/platforms", async (ICommandRepo repo) =>
{
    var data = await repo.GetAllPlatform();
    var read = data.Select(x => (PlatformReadDto)x);
    return Results.Ok(read);
}).WithName("getPlatform").WithOpenApi();
app.MapGet("api/command/platform/{platformId}", async (ICommandRepo repo, int platformId) =>
{
    if (!await repo.PlatformExists(platformId))
    {
        return Results.NotFound();
    }

    var data = await repo.GetCommandsPlatform(platformId);
    var read = data.Select(x => (CommandReadDto)x);
    return Results.Ok(read);
}).WithName("getCommandsForPlaform").WithOpenApi();

app.MapGet("api/command/platform/{platformId}/command/{commandId}",
    async (ICommandRepo repo, int platformId, int commandId) =>
    {
        if (!await repo.PlatformExists(platformId))
        {
            return Results.NotFound();
        }

        var data = await repo.GetCommand(platformId, commandId);
        if (data == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(data);
    }).WithName("getCommandByPlatformIdAndCommandId").WithOpenApi();

app.MapPost("api/command/platform/{platformId}/command",
    async (ICommandRepo repo, int platformId, CommandCreateDto dto) =>
    {
        if (!await repo.PlatformExists(platformId))
        {
            return Results.NotFound();
        }

        var command = dto;
        repo.CreateCommand(platformId, dto);
        await repo.SaveChangesAsync();
        return Results.Ok(command);
    }).WithName("createCommand").WithOpenApi();

app.Run();