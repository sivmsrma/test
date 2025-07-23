using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Terret_Billing.Application.DTOs;
using Terret_Billing.Domain.Interfaces;
using System.Windows.Input;
using Terret_Billing.Presentation.Commands;
using System.Collections.ObjectModel;
using Terret_Billing.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using Terret_Billing.Presentation.Models;
using System.Windows;

namespace Terret_Billing.Presentation.ViewModels
{
    public class VoucherNoteViewModel : INotifyPropertyChanged
    {
        private readonly IVoucherNoteRepository _repository;
        private readonly IBranchRepository _branchRepository;
        public event PropertyChangedEventHandler PropertyChanged;

        public VoucherNoteModel VoucherNote { get; set; } = new VoucherNoteModel();

        public VoucherNoteViewModel(IVoucherNoteRepository repository, IBranchRepository branchRepository, User currentUser)
        {
            _repository = repository;
            _branchRepository = branchRepository;
            LookupVoucherNoteItemCommand = new AsyncRelayCommand(async _ => await LookupVoucherNoteItemAsync());
            AddItemCommand = new RelayCommand(_ => AddItem());
            SendCommand = new RelayCommand(async _ => await SendSelectedVoucherNotes(currentUser));
            InitializeVoucherNoteNumber(currentUser);
        }

        private async void InitializeVoucherNoteNumber(User currentUser)
        {
            string firmId = currentUser?.firm_id?.ToUpper() ?? "XXXX";
            string prefix = $"VNO{firmId}-";
            int nextNumber = 1;
            var lastNumber = await _repository.GetLastVoucherNoteNumberAsync(currentUser.firm_id);
            if (!string.IsNullOrEmpty(lastNumber) && lastNumber.StartsWith(prefix))
            {
                var numPart = lastNumber.Substring(prefix.Length);
                if (int.TryParse(numPart, out int lastNum))
                {
                    nextNumber = lastNum + 1;
                }
            }
            VoucherNote.VoucherNoteNumber = $"{prefix}{nextNumber}";
            OnPropertyChanged(nameof(VoucherNote));
        }

        public ObservableCollection<VoucherNoteTaggingItemDto> Taggings { get; set; } = new ObservableCollection<VoucherNoteTaggingItemDto>();

        public IEnumerable<string> SelectedBarcodes => Taggings.Where(x => x.IsSelected).Select(x => x.Barcode);

        public IAsyncRelayCommand LookupVoucherNoteItemCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand SendCommand { get; }

        private async Task LookupVoucherNoteItemAsync()
        {
            if (string.IsNullOrWhiteSpace(VoucherNote.Barcode)) return;
            var item = await _repository.GetVoucherNoteItemByBarcodeAsync(VoucherNote.Barcode);
            if (item != null)
            {
                VoucherNote.Barcode = item.Barcode;
                VoucherNote.Category = item.Category;
                VoucherNote.SubCategory = item.SubCategory;
                VoucherNote.Design = item.Design;
                VoucherNote.Purity = item.Purity;
                VoucherNote.HSNNo = item.HSN_No;
                VoucherNote.HUIDNo = item.HUID_No;
                VoucherNote.Size = item.Size;
                VoucherNote.PCs = item.pcs.ToString();
                VoucherNote.GrossWt = item.Gross_wt.ToString();
                VoucherNote.LessWt = item.Less_wt.ToString();
                VoucherNote.NetWt = item.Net_wt.ToString();
                VoucherNote.DiamondCt = item.DiamondCt.ToString();
                VoucherNote.DiamondWtGm = item.DiamondWtGm.ToString();
                VoucherNote.TunchPercent = item.TunchPercent.ToString();
                VoucherNote.TunchWt = item.TunchWt.ToString();
                VoucherNote.WastePercent = item.WastePercent.ToString();
                VoucherNote.WasteFAmount = item.WasteFAmount.ToString();
                VoucherNote.StoneCt = item.StoneCt.ToString();
                VoucherNote.StoneWtGm = item.StoneWtGm.ToString();
                VoucherNote.FinalWt = item.FinalWt.ToString();
                VoucherNote.Comment = item.Comment;
            }
        }

        private void AddItem()
        {
            var item = new VoucherNoteTaggingItemDto
            {
                Barcode = VoucherNote.Barcode ?? string.Empty,
                MetalType = string.Empty, // Set from input if available
                StockType = string.Empty, // Set from input if available
                Description = VoucherNote.Comment ?? string.Empty, // Use Description for comment
                Item = VoucherNote.Category ?? string.Empty,
                Purity = VoucherNote.Purity ?? string.Empty,
                HSN = VoucherNote.HSNNo ?? string.Empty,
                HUID = VoucherNote.HUIDNo ?? string.Empty,
                PCS = int.TryParse(VoucherNote.PCs, out var pcsVal) ? pcsVal : 0,
                GrossWt = decimal.TryParse(VoucherNote.GrossWt, out var grossVal) ? grossVal : 0,
                LessWt = decimal.TryParse(VoucherNote.LessWt, out var lessVal) ? lessVal : 0,
                NetWt = decimal.TryParse(VoucherNote.NetWt, out var netVal) ? netVal : 0,
                DiamondCt = decimal.TryParse(VoucherNote.DiamondCt, out var diaCtVal) ? diaCtVal : 0,
                StoneCt = decimal.TryParse(VoucherNote.StoneCt, out var stoneCtVal) ? stoneCtVal : 0,
                FinalWeight = decimal.TryParse(VoucherNote.FinalWt, out var finalWtVal) ? finalWtVal : 0
            };
            Taggings.Add(item);
        }

        private async Task SendSelectedVoucherNotes(User currentUser)
        {
            var companyDetails = await _branchRepository.GetCompanyDetailsByFirmIdAsync(currentUser.firm_id);
            bool anySuccess = false;
            foreach (var item in Taggings.Where(x => x.IsSelected))
            {
                var dto = new Terret_Billing.Application.DTOs.VoucherNoteInsertDto
                {
                    // Current user details to columns without _r
                    UserName = currentUser?.user_name,
                    FirmId = currentUser?.firm_id,
                    AssignedBranch = currentUser?.assigned_branch,
                    PhoneNumber = currentUser?.phone_number,
                    ShopName = companyDetails?.ShopName,
                    State = companyDetails?.State,
                    GSTIN = companyDetails?.GSTIN,
                    Address = companyDetails?.Address,

                    // UI/TextBox values to columns with _r
                    UserNameR = VoucherNote.Mobile,
                    FirmIdR = VoucherNote.ShopName,
                    AssignedBranchR = VoucherNote.State,
                    PhoneNumberR = VoucherNote.Mobile,
                    ShopNameR = VoucherNote.ShopName,
                    StateR = VoucherNote.State,
                    GSTINR = VoucherNote.GSTIN,
                    AddressR = VoucherNote.Address,

                    VoucherNoteNumber = VoucherNote.VoucherNoteNumber,
                    Date = DateTime.Now,
                    Barcode = item.Barcode
                };
                try
                {
                    await _repository.InsertVoucherNoteAsync(dto);
                    anySuccess = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving Voucher Note: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (anySuccess)
            {
                MessageBox.Show("Voucher Note saved successfully in both Local and Server DB!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Refresh the form (clear all data, update voucher note number)
                VoucherNote = new VoucherNoteModel();
                Taggings.Clear();
                await UpdateVoucherNoteNumber(currentUser);
                OnPropertyChanged(nameof(VoucherNote));
                OnPropertyChanged(nameof(Taggings));
            }
        }

        private async Task UpdateVoucherNoteNumber(User currentUser)
        {
            string firmId = currentUser?.firm_id?.ToUpper() ?? "XXXX";
            string prefix = $"VNO{firmId}-";
            int nextNumber = 1;
            var lastNumber = await _repository.GetLastVoucherNoteNumberAsync(currentUser.firm_id);
            if (!string.IsNullOrEmpty(lastNumber) && lastNumber.StartsWith(prefix))
            {
                var numPart = lastNumber.Substring(prefix.Length);
                if (int.TryParse(numPart, out int lastNum))
                {
                    nextNumber = lastNum + 1;
                }
            }
            VoucherNote.VoucherNoteNumber = $"{prefix}{nextNumber}";
            OnPropertyChanged(nameof(VoucherNote));
        }

        public async Task FetchCompanyDetailsByMobileAsync()
        {
            var details = await _repository.GetCompanyDetailsByMobileAsync(VoucherNote.Mobile);
            if (details != null)
            {
                VoucherNote.ShopName = details.ShopName;
                VoucherNote.State = details.State;
                VoucherNote.GSTIN = details.GSTIN;
                VoucherNote.Address = details.Address;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VoucherNoteTaggingItemDto : Terret_Billing.Application.DTOs.TaggingItemDto
    {
        public bool IsSelected { get; set; }
    }
}
