using Microsoft.Extensions.Configuration;
using System.IO;

namespace Terret_Billing.Application.Configuration
{
    public static class ConfigurationService
    {
        private static IConfigurationRoot _configuration;
        static ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return _configuration.GetConnectionString(name);
        }

        public static string GetValue(string key)
        {
            return _configuration[key];
        }
    }
}
