using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Application.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Application.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;

        public PartyService(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
        }

        public async Task CreatePartyAsync(Customer customer)
        {

            if (await IsGSTNumberDuplicateAsync(customer.GSTNumber))
                    throw new ArgumentException("GST number already exists.", nameof(customer.GSTNumber));

             await _partyRepository.AddAsync(customer);
        }

        public async Task<bool> IsGSTNumberDuplicateAsync(string gstNumber)
        {
            if (string.IsNullOrWhiteSpace(gstNumber))
                return false;

            return await _partyRepository.ExistsByGSTNumberAsync(gstNumber);
        }
        
    }
} 