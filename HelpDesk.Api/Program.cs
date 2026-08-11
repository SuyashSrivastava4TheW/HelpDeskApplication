using HelpDesk.Api.Data;
using HelpDesk.Api.Mapping;
using HelpDesk.Api.Middleware;
using HelpDesk.Api.Repositories;
using HelpDesk.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

// ---- Serilog: configured before the host builds so startup failures are logged too ----
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/helpdesk-api-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting HelpDesk.Api");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // ---- Database ----
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=HelpDeskManagementDb.db";
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        if (connectionString.Contains("Data Source=") && !connectionString.Contains("Server="))
        {
            options.UseSqlite(connectionString);
        }
        else
        {
            options.UseSqlServer(connectionString);
        }
    });

    // ---- Repository Pattern (DI) ----
    builder.Services.AddScoped<ITicketRepository, TicketRepository>();

    // ---- Service Layer (DI) ----
    builder.Services.AddScoped<ITicketService, TicketService>();

    // ---- AutoMapper ----
    builder.Services.AddAutoMapper(typeof(TicketMappingProfile));

    // ---- Controllers ----
    builder.Services.AddControllers();

    // ---- CORS: allow the MVC app to call this API from a different port ----
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowMvcClient", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });

    // ---- Swagger ----
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "HelpDesk API",
            Version = "v1",
            Description = "REST API for the Help Desk Ticket Management System."
        });

        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    });

    var app = builder.Build();

    // Ensure Database and tables are created automatically on startup
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    // ---- Middleware pipeline ----
    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "HelpDesk API v1");
            options.RoutePrefix = string.Empty; // Serves Swagger UI directly at root (https://localhost:59186/)
        });
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseCors("AllowMvcClient");
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "HelpDesk.Api terminated unexpectedly during startup");
}
finally
{
    Log.CloseAndFlush();
}
