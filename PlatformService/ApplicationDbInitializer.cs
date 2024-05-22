using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService;

public static class ApplicationDbInitializer
{
  public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
  {
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      SeedData(serviceScope.ServiceProvider.GetRequiredService<AppDbContext>(), isProduction);
    }
  }
  private static void SeedData(AppDbContext context, bool isProduction)
  {
    // if(isProduction)
    // {
    //   Console.WriteLine("Attempt to apply migration");
    //   context.Database.Migrate();
    // }
    if (!context.Platforms.Any()) 
    {
      context.Platforms.AddRange(
        new Platform { Name = "Dot net ", Publisher = "Microsoft", Costs = "Free" },
         new Platform { Name = "SQL Server ", Publisher = "Microsoft", Costs = "Free" },
          new Platform { Name = "Kubernets ", Publisher = "Cloud native computing foundation", Costs = "Free" }
      );
      context.SaveChanges();
    }
  }
}
