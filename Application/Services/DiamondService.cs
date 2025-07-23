using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Presentation.Models;
using System.Linq;

namespace Terret_Billing.Application.Services
{
    public class DiamondService : IDiamondService
    {
        private readonly IDiamondRepository _diamondRepository;

        public DiamondService(IDiamondRepository diamondRepository)
        {
            _diamondRepository = diamondRepository ?? throw new ArgumentNullException(nameof(diamondRepository));
        }

        public async Task CreateDiamondEntryAsync(DiamondTaggingEntry entry)
        {
           
            try
            {
                await _diamondRepository.AddAsync(entry);
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    MessageBox.Show($"Error saving diamond entry: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                throw;
            }
        }

        public async Task<IEnumerable<DiamondTaggingEntry>> GetDiamondEntriesByTypeAsync(string type, string firmId, string invoiceNumber = null)
        {
            try
            {
                return await _diamondRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDiamondEntriesByTypeAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DiamondTaggingEntry>> GetAllDiamondEntriesAsync(string metalType, string firmId)
        {
            try
            {
                var allEntries = await _diamondRepository.GetAllAsync();
                return allEntries.Where(e => 
                    (string.IsNullOrEmpty(metalType) || e.Metal_Type == metalType) &&
                    (string.IsNullOrEmpty(firmId) || e.Firm_id == firmId)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllDiamondEntriesAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<DiamondTaggingEntry> GetDiamondEntryByIdAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID", nameof(id));

            try
            {
                return await _diamondRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDiamondEntryByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateDiamondEntryAsync(DiamondTaggingEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            try
            {
                await _diamondRepository.UpdateAsync(entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateDiamondEntryAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteDiamondEntryAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID", nameof(id));

            try
            {
                await _diamondRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteDiamondEntryAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsInvoiceNumberDuplicateAsync(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                return false;

            return await _diamondRepository.ExistsByInvoiceNoAsync(invoiceNumber);
        }

        public async Task<DiamondTaggingEntry> GetDiamondEntryByInvoiceNoAsync(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                throw new ArgumentException("Invoice number cannot be empty", nameof(invoiceNumber));

            try
            {
                return await _diamondRepository.GetByInvoiceNoAsync(invoiceNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDiamondEntryByInvoiceNoAsync: {ex.Message}");
                throw;
            }
        }
    }
}

