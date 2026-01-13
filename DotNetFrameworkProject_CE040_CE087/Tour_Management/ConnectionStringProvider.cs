using System;
using System.Configuration;

namespace Tour_Management
{
    public static class ConnectionStringProvider
    {
        public static string GetConnectionString(string name)
        {
            // First check environment variables
            var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrEmpty(envConnectionString))
            {
                return envConnectionString;
            }

            // Check individual DB environment variables
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (!string.IsNullOrEmpty(dbServer) && !string.IsNullOrEmpty(dbName))
            {
                return $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword}";
            }

            // Fallback to ConfigurationManager
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString
                ?? throw new InvalidOperationException($"Connection string '{name}' not found in configuration or environment variables.");
        }
    }
}
