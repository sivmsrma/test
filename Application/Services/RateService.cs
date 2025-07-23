using System;
using System.Threading.Tasks;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Infrastructure.Data;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

namespace Terret_Billing.Application.Services
{
    public class RateService : IRateService
    {
        private readonly IDatabaseHelper _databaseHelper;

        public RateService(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
        }

        public async Task<Rate> GetLatestRateAsync(string metalType)
        {
            try
            {
                using (var connection = new MySqlConnection(_databaseHelper.GetConnectionString()[1]))
                {
                    await connection.OpenAsync();
                    
                    var parameters = new { MetalType = metalType };
                    
                    return await connection.QueryFirstOrDefaultAsync<Rate>(
                        "sp_GetLatestRateByMetalType",
                        parameters,
                        commandType: CommandType.StoredProcedure) ?? new Rate
                        {
                            MetalType = metalType,
                            RateValue = metalType.ToLower() switch
                            {
                                "gold" => 4500m,
                                "silver" => 75m,
                                _ => 0m
                            },
                            EffectiveDate = DateTime.UtcNow,
                            IsActive = true
                        };
                }
            }
            catch (Exception ex)
            {
                // Log error and return default rate
                Console.WriteLine($"Error getting latest rate: {ex.Message}");
                return new Rate
                {
                    MetalType = metalType,
                    RateValue = 0m,
                    EffectiveDate = DateTime.UtcNow,
                    IsActive = false
                };
            }
        }

        public decimal CalculateMaking(decimal weight, Rate rate)
        {
            if (rate == null || weight <= 0)
                return 0m;

            // Making charge calculation based on weight and rate
            // This can be customized based on business rules
            var makingChargeRate = rate.MetalType.ToLower() switch
            {
                "gold" => 0.2m,    // 10% for gold
                "silver" => 0.05m, // 5% for silver
                _ => 0.08m         // 8% for others
            };

            return weight * rate.RateValue * makingChargeRate;
         }

        public decimal ConvertRate(decimal baseRate, string purity)
        {
            if (baseRate <= 0 || string.IsNullOrEmpty(purity))
                return 0m;

            // Convert rate based on purity standards
            return purity.ToUpper() switch
            {
                "24K" => baseRate,
                "22K" => baseRate * 0.9167m,  // 22/24 = 0.9167
                "18K" => baseRate * 0.75m,
                "14K" => baseRate * 0.5833m,
                _ => baseRate // Default to base rate for unknown purities
            };
        }
    }
}
