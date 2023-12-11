
using InstapotAPI.Helpers;
using InstapotAPI.Infrastructure;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InstapotAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<InstapotContext>(options =>
            options.UseSqlite(connectionString));
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        builder.Services.AddScoped<IProfileReposetory, ProfileReposetory>();
        builder.Services.AddAutoMapper(typeof(MappingProfiler));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("API starting...");

        try
        {
            var context = services.GetRequiredService<InstapotContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Migration errror...");
        }

        app.Run();
    }
}
