using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IRateService
    {
        /// <summary>
        /// Gets the latest rate for a specific metal type
        /// </summary>
        /// <param name="metalType">The type of metal (e.g., "Gold", "Silver")</param>
        /// <returns>The latest rate information</returns>
        Task<Rate> GetLatestRateAsync(string metalType);
        
        /// <summary>
        /// Calculates the making charge based on weight and rate
        /// </summary>
        /// <param name="weight">Weight of the item</param>
        /// <param name="rate">Rate information</param>
        /// <returns>Making charge amount</returns>
        decimal CalculateMaking(decimal weight, Rate rate);
        
        /// <summary>
        /// Converts a base rate based on purity
        /// </summary>
        /// <param name="baseRate">Base rate to convert</param>
        /// <param name="purity">Purity level (e.g., "24K", "22K")</param>
        /// <returns>Converted rate</returns>
        decimal ConvertRate(decimal baseRate, string purity);
    }
}
