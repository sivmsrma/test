using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Application.Services
{
    public class TaggingService : ITaggingService
    {
        private readonly ITaggingRepository _taggingRepository;

        public TaggingService(ITaggingRepository taggingRepository)
        {
            _taggingRepository = taggingRepository ?? throw new ArgumentNullException(nameof(taggingRepository));
        }

        public async Task<IEnumerable<TaggingItem>> GetAllTaggingsAsync(string firmId = null)
        {
            try
            {
                return await _taggingRepository.GetAllAsync(firmId);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error fetching stock_tagging data", ex);
                throw;
            }
        }

        public async Task<(IEnumerable<TaggingItem> Items, int TotalCount)> GetAllTaggingsPagedAsync(
            string firmId,
            string typeOfStock,
            string metalType,
            string partyName,
            string invoiceNumber,
            int pageNumber,
            int pageSize)
        {
            return await _taggingRepository.GetAllPagedAsync(firmId, typeOfStock, metalType, partyName, invoiceNumber, pageNumber, pageSize);
        }

        public async Task<TaggingItem> GetTaggingByIdAsync(long id)
        {
            try
            {
                return await _taggingRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error fetching tagging with ID {id}", ex);
                throw;
            }
        }

        public async Task CreateTaggingAsync(TaggingItem item)
        {
            try
            {
                await _taggingRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error adding tagging", ex);
                throw;
            }
        }

        public async Task UpdateTaggingAsync(TaggingItem item)
        {
            try
            {
                await _taggingRepository.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error updating tagging with ID {item.stock_id}", ex);
                throw;
            }
        }

        public async Task DeleteTaggingAsync(long id)
        {
            try
            {
                await _taggingRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error deleting tagging with ID {id}", ex);
                throw;
            }
        }

        public async Task<bool> IsInvoiceNumberDuplicateAsync(string invoiceNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceNumber))
                    return false;

                return await _taggingRepository.ExistsByInvoiceNumberAsync(invoiceNumber);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error checking duplicate invoice number: {invoiceNumber}", ex);
                throw;
            }
        }

        public async Task<TaggingItem> GetTaggingByInvoiceNumberAsync(string invoiceNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceNumber))
                    return null;

                return await _taggingRepository.GetByInvoiceNumberAsync(invoiceNumber);
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error fetching tagging by invoice number: {invoiceNumber}", ex);
                throw;
            }
        }
    }
}
