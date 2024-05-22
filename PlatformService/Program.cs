
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.Data;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // if (builder.Environment.IsProduction())
        // {
        //    Console.WriteLine("Using sql server");
        //     builder.Services.AddDbContext<AppDbContext>(options =>
        //     {
        //         options.UseSqlServer(builder.Configuration.GetConnectionString("platfomConnection"));
        //     });
        // }
        // else
        // {
            Console.WriteLine("Using Inmemory db");
            builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMem");
        });
        // }

        builder.Services.AddServices();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        app.UseSwagger();
        app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();
        app.MapGet("api/platform", async (IPlatformRepo repo) =>
        {
            var data = await repo.GetAll();
            if (data != null)
            {
                IEnumerable<PlatformReadDto> target = data.Select(x => (PlatformReadDto)x);
                return Results.Ok(target);
            }
            return Results.NotFound();

        })
        .WithName("GetAllPlatform")
        .WithOpenApi();
        app.MapPost("api/platform", (IPlatformRepo repo, [FromBody] PlatformCreateDto platform) =>
        {
            var result = repo.Create(platform);
            return result;

        })
        .WithName("CreatePlatform")
        .WithOpenApi();
        app.MapGet("api/platform/{id}", async (IPlatformRepo repo, int id) =>
        {
            var data = await repo.GetById(id);
            if (data != null)
            {
                PlatformReadDto target = data;
                return Results.Ok(target);
            }
            return Results.NotFound();
        })
        .WithName("GetPlatformById")
        .WithOpenApi();
        ApplicationDbInitializer.PrepPopulation(app, app.Environment.IsProduction());
        app.Run();
    
