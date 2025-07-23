using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Application.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static IDbConnection _dbConnection;
        private static IDatabaseHelper _databaseHelper;

        static ServiceLocator()
        {
            try
            {
                // Get connection string from app.config
                var connectionString = ConfigurationManager.ConnectionStrings["BillingDb"]?.ConnectionString;
                var serverConnectionString = ConfigurationManager.ConnectionStrings["BillingDblocal"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'BillingDb' not found in app.config");
                }
                var connectionStrings = new List<string> { connectionString, serverConnectionString };
                // Create database connection and helper
                _dbConnection = new MySqlConnection(connectionString);
                _databaseHelper = new DatabaseConnectionHelper(connectionString);

                // Register repositories with their required dependencies
                _services[typeof(IItemRepository)] = new ItemRepository(_databaseHelper);
                _services[typeof(ITaggingRepository)] = new TaggingRepository(_databaseHelper);
                _services[typeof(IDiamondRepository)] = new DiamondRepository(_databaseHelper);

                // Register services with their dependencies
                _services[typeof(ITaggingService)] = new TaggingService((ITaggingRepository)_services[typeof(ITaggingRepository)]);
                _services[typeof(IDiamondService)] = new DiamondService((IDiamondRepository)_services[typeof(IDiamondRepository)]);

                // Register PartyService and PartyRepository
                _services[typeof(IPartyRepository)] = new PartyRepository(_databaseHelper);
                _services[typeof(IPartyService)] = new PartyService((IPartyRepository)_services[typeof(IPartyRepository)]);
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                System.Diagnostics.Debug.WriteLine($"Error initializing ServiceLocator: {ex}");
                throw;
            }
        }

        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered.");
        }
    }
}
