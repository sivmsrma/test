using System;

namespace Terret_Billing.Domain.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public string MetalType { get; set; } // e.g., "Gold", "Silver"
        public decimal RateValue { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Backward compatibility property
        public decimal RatePerGram 
        {
            get => RateValue;
            set => RateValue = value;
        }
    }
}
