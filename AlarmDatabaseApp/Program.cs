using AlarmDatabaseLibrary.Context;
using AlarmDatabaseLibrary.Migrations;
using AlarmDatabaseLibrary.Models;
using AlarmDatabaseLibrary.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//Use this command to do changes in the migrations:
//Add - Migration AddNotesColumn - Project AlarmDatabaseLibrary - StartupProject AlarmDatabaseApp
//Update-Database -Project AlarmDatabaseLibrary -StartupProject AlarmDatabaseApp



var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AlarmDbContext>(options =>
            options.UseSqlServer(
                context.Configuration.GetConnectionString("AlarmDatabase")));

        services.AddScoped<AlarmDbSeeder>();
    })
    .Build();

// Apply migrations & seed
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AlarmDbContext>();
    dbContext.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<AlarmDbSeeder>();
    seeder.Seed();
}

await host.RunAsync();


//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json")
//    .Build();

//var options = new DbContextOptionsBuilder<AlarmDbContext>()
//    .UseSqlServer(configuration.GetConnectionString("AlarmDatabase"))
//    .Options;

//using var context = new AlarmDbContext(options);
//services.AddDbContext<AlarmDbContext>();

//context.Database.Migrate();

//AlarmDbSeeder.Seed(context);

