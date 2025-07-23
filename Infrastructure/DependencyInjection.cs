using Microsoft.Extensions.DependencyInjection;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Infrastructure.Repositories;

namespace Terret_Billing.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Database
            services.AddScoped<IDatabaseHelper, MySqlDatabaseHelper>();

            // Repositories
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ITaggingRepository, TaggingRepository>();
            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IPurchaseReportRepository, PurchaseReportRepository>();
            // Services
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ITaggingService, TaggingService>();
            services.AddScoped<IPartyService, PartyService>();

            return services;
        }
    }
}