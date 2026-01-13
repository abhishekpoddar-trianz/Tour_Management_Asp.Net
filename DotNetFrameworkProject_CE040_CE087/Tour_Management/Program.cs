using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Tour_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure to use environment variable for port, default to 8080
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(int.Parse(port));
            });

            // Add configuration sources
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            // Add services to the container
            builder.Services.AddSystemWebAdapters();
            builder.Services.AddHttpContextAccessor();

            // Add health checks
            builder.Services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("dbconnection") ?? "Server=localhost;Database=tourdb;",
                    name: "database",
                    timeout: TimeSpan.FromSeconds(5));

            var app = builder.Build();

            // Configure middleware pipeline
            app.UseSystemWebAdapters();

            // Add health check endpoints
            app.MapHealthChecks("/health");
            app.MapHealthChecks("/ready");

            app.Run();
        }
    }
}
