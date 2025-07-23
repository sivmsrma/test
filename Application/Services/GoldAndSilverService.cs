using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Application.Services
{
    public class GoldAndSilverService : IGoldAndSilverService
    {
        private readonly IGoldAndSilverRepository _repository;

        public GoldAndSilverService(IGoldAndSilverRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddEntryAsync(GoldAndSilverTaggingEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            await _repository.AddAsync(entry);
        }

        public async Task<(IEnumerable<GoldAndSilverTaggingEntry> Items, int TotalCount)> GetEntriesByTypePagedAsync(string type, string firmId, string invoiceNumber, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be null or empty", nameof(type));
            if (string.IsNullOrWhiteSpace(firmId))
                throw new ArgumentException("Firm ID cannot be null or empty", nameof(firmId));

            return await _repository.GetByTypePagedAsync(type, firmId, invoiceNumber, pageNumber, pageSize);
        }

        public async Task<decimal?> GetAvailableWeightAsync(string metalType, string partyName, string invoiceNumber, string purity)
        {
            return await _repository.GetPendingWeightByFiltersAsync(metalType, partyName, invoiceNumber, purity);
        }
    }
}
