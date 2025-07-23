using Microsoft.Extensions.DependencyInjection;
using Terret_Billing.Application.Services.Base;
using Terret_Billing.Application.Validation;

namespace Terret_Billing.Application.DependencyInjection
{
    public static class ServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Register all services and repositories here
            services.AddSingleton<ValidationService>();
            // AddSingleton/AddScoped/AddTransient as per service lifetime
            // Example: services.AddScoped<IMyService, MyService>();
        }
    }
}
