using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Application.DTOs;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services
{
    public class BillEntryService : IBillEntryService
    {
        private readonly ITaggingRepository _repo;
        public BillEntryService(ITaggingRepository repo) => _repo = repo;

        public async Task<TaggingItemDto?> FindItemAsync(string barcode)
        {
            var item = await _repo.GetByBarcodeAsync(barcode);
            if (item == null) return null;
            
            // Map TaggingItem to TaggingItemDto
            return new TaggingItemDto
            {
                Barcode = item.Barcode,
                Description = item.Description,
                Item = item.ItemName,
                Purity = item.purity,
                HSN = item.HSNCode,
                PCS = item.Quantity,
                GrossWt = item.GrossWeight,
                NetWt = item.NetWeight,
                DiamondCt = item.DiamondCarat,
                DiamondRate = item.DiamondRate,
                StoneCt = item.StoneCount,
                FinalWeight = item.FinalWeight,
                MetalType = item.metal_type
            };
        }
    }
}