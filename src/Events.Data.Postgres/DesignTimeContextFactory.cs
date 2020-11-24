using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Events.Data.Postgres
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "../Events.Server");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{appSettingsPath}/appsettings.json")
                .AddJsonFile($"{appSettingsPath}/appsettings.{envName}.json", true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext"));
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
