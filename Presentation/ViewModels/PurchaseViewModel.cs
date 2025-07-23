using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Commands;
using System.Collections.Generic;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Presentation.Models.Request;
using Terret_Billing.Domain.Entities;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace Terret_Billing.Presentation.ViewModels
{
    public class PurchaseViewModel : INotifyPropertyChanged
    {
        // Properties for purchase and UI selections
        private Purchase _currentPurchase;
        private Customer _selectedParty;
        private PurchaseItem _currentItem;
        private PurchaseItem _selectedPurchaseItem;
        private string _selectedMetal;
        private string _selectedTaxType;
        private decimal _taxPercentage;
        private PurchaseRequest purchaserequest;
        private bool _isUpdateMode;
        private readonly User _currentUser;
        private readonly Action<string> _log;
        // Properties with OnPropertyChanged for binding
        public Purchase CurrentPurchase
        {
            get => _currentPurchase;
            set { _currentPurchase = value; OnPropertyChanged(); }
        }

        public Customer SelectedParty
        {
            get => _selectedParty;
            set
            {
                _selectedParty = value;
                if (_selectedParty != null)
                    CurrentPurchase.SupplierId = _selectedParty.Id;
                OnPropertyChanged();
            }
        }

        public PurchaseItem CurrentItem
        {
            get => _currentItem;
            set { _currentItem = value; OnPropertyChanged(); }
        }

        public PurchaseItem SelectedPurchaseItem
        {
            get => _selectedPurchaseItem;
            set
            {
                _selectedPurchaseItem = value;
                OnPropertyChanged(nameof(SelectedPurchaseItem));

                if (_selectedPurchaseItem != null)
                {
                    // Set update mode to true when an item is selected
                    IsUpdateMode = true;

                    // Create a new item and set the basic properties
                    var newItem = new PurchaseItem
                    {
                        Id = _selectedPurchaseItem.Id,
                        Metal = _selectedPurchaseItem.Metal,
                        HSNCode = _selectedPurchaseItem.HSNCode,
                        HuidNo = _selectedPurchaseItem.HuidNo,
                        ItemName = _selectedPurchaseItem.ItemName,
                        Purity = _selectedPurchaseItem.Purity,
                        Pcs = _selectedPurchaseItem.Pcs,
                        GrossWt = _selectedPurchaseItem.GrossWt,
                        LessWt = _selectedPurchaseItem.LessWt,
                        NetWt = _selectedPurchaseItem.NetWt,
                        Rate = _selectedPurchaseItem.Rate,
                        TaxType = _selectedPurchaseItem.TaxType,
                        TaxRate = _selectedPurchaseItem.TaxRate,
                        DiaCt = _selectedPurchaseItem.DiaCt,
                        StoneCt = _selectedPurchaseItem.StoneCt,
                        StoneCharge = _selectedPurchaseItem.StoneCharge,
                        DiaCharge = _selectedPurchaseItem.DiaCharge,
                        
                    };

                    // Trigger calculations
                    newItem.CalculateAmount();
                    newItem.CalculateTaxAndNetAmount();

                    CurrentItem = newItem;
                    SelectedMetal = _selectedPurchaseItem.Metal;
                }
                else
                {
                    IsUpdateMode = false;
                    CurrentItem = new PurchaseItem { Pcs = 1 };
                    SelectedMetal = null;
                }
            }
        }

        public string SelectedMetal
        {
            get => _selectedMetal;
            set
            {
                if (_selectedMetal != value)
                {
                    _selectedMetal = value;
                    if (CurrentItem != null) // Ensure CurrentItem is not null
                    {
                        CurrentItem.Metal = value; // Update CurrentItem.Metal
                    }
                    OnPropertyChanged(nameof(SelectedMetal));
                    OnPropertyChanged(nameof(ShowDiamondFields)); // Notify UI for new property
                    OnPropertyChanged(nameof(HideDiamondFields));
                    OnPropertyChanged(nameof(ShowLooseDiamondFields));// Notify UI for new property
                   
                }
            }
        }
        public string SelectedTaxType
        {
            get => _selectedTaxType;
            set
            {
                _selectedTaxType = value;
                TaxPercentage = ParseTaxPercentage(value);
                OnPropertyChanged();
            }
        }

        public decimal TaxPercentage
        {
            get => _taxPercentage;
            set
            {
                if (_taxPercentage != value)
                {
                    _taxPercentage = value;
                    CalculateTax(); // This will update tax for all items
                    OnPropertyChanged();
                }
            }
        }

        public bool IsUpdateMode
        {
            get => _isUpdateMode;
            set
            {
                _isUpdateMode = value;
                OnPropertyChanged(nameof(IsUpdateMode));
                OnPropertyChanged(nameof(AddButtonText));
            }
        }

        public string AddButtonText => IsUpdateMode ? "Update" : "Add";

        // Properties for field visibility
        public bool ShowDiamondFields => SelectedMetal == DIAMOND_JEW;
        public bool HideDiamondFields => !ShowDiamondFields;
        public bool ShowLooseDiamondFields => SelectedMetal == LOOSE_CUT_DIAMOND;


        // Constants
        private const string LOOSE_CUT_DIAMOND = "Loose/Cut Diamond";
        private const string DIAMOND_JEW = "Diamond Jew";

        // Collections for dropdowns and lists
        public ObservableCollection<Customer> Parties { get; }
        public ObservableCollection<PurchaseItem> PurchaseItems { get; }
        public PurchaseRequest PurchaseRequest { get; }
        public ObservableCollection<string> MetalTypes { get; }
        public ObservableCollection<string> PurityTypes { get; }
        public ObservableCollection<string> TaxTypes { get; }

        // Commands for UI actions
        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand CancelUpdateCommand { get; }

        private readonly IPartyRepository _partyRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly Customer _selectedCustomer;

        public PurchaseViewModel() : this(new PartyRepository(new DatabaseHelper()), new User())
        {
        }


        public PurchaseViewModel(IPartyRepository partyRepository, User currentUser)
        {
            try
            {
                _partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
                var databaseHelper = new MySqlDatabaseHelper();
                _partyRepository = new PartyRepository(databaseHelper);
                _purchaseRepository = new PurchaseRepository(databaseHelper); // Initialize PurchaseRepository
                _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
                // Initialize collections
                _currentPurchase = new Purchase(); // Initialize _currentPurchase first
                Parties = new ObservableCollection<Customer>();
                PurchaseItems = new ObservableCollection<PurchaseItem>();
                PurchaseRequest = new PurchaseRequest
                {
                    Purchase = _currentPurchase, // Use the instance of _currentPurchase
                    PurchaseItems = new List<PurchaseItem>()
                };

                // Load parties from repository
                LoadParties();

                MetalTypes = new ObservableCollection<string>
                {
                    "Gold Jew", "Silver Jew", "Platinum Jew",
                    DIAMOND_JEW, "Stone", LOOSE_CUT_DIAMOND,
                    "Gold Bar", "Silver Bar", "Immitation" ,
                    "Lab Grown Diamond"
                };

                PurityTypes = new ObservableCollection<string>
                {
                    "24K", "22K", "18K", "14K", "999", "925", "995"
                };

                TaxTypes = new ObservableCollection<string>
                {
                    "CGST+SGST", "IGST"
                };

                CurrentItem = new PurchaseItem { Pcs = 1 };

                // Initialize commands
                AddItemCommand = new RelayCommand(AddItem);
                RemoveItemCommand = new RelayCommand(RemoveItem);
                SaveCommand = new RelayCommand(SavePurchase);
                CancelCommand = new RelayCommand(CancelPurchase);
                UpdateCommand = new RelayCommand(UpdateItem);
                CancelUpdateCommand = new RelayCommand(CancelUpdate);

                // Initial calculations
                CalculateTax();
                OnPropertyChanged();
            }
            catch (Exception ex)
            {
                // Log the error and show a user-friendly message
                MessageBox.Show($"Error initializing purchase form: {ex.Message}",
                    "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Consider re-throwing or handling the error appropriately
            }
        }

        private async void LoadParties()
        {
            try
            {
                var parties = await _partyRepository.GetAllAsync();
                Parties.Clear();
                foreach (var party in parties)
                {
                    Parties.Add(new Customer
                    {
                        Id = party.Id,
                        Name = party.Name,
                        Address = party.Address
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parties: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Command implementations
        private void AddItem(object parameter)
        {
            try
            {
                if (IsUpdateMode)
                {
                    UpdateItem(parameter);
                    return;
                }

                if (CurrentItem == null || string.IsNullOrEmpty(CurrentItem.Metal) || CurrentItem.Pcs <= 0)
                    throw new ArgumentException("Please fill in all required fields.");

                // Create a copy to avoid reference issues
                var item = new PurchaseItem
                {
                    Metal = CurrentItem.Metal,
                    HSNCode = CurrentItem.HSNCode,
                    HuidNo = CurrentItem.HuidNo,
                    ItemName = CurrentItem.ItemName,
                    Purity = CurrentItem.Purity,
                    Pcs = CurrentItem.Pcs,
                    GrossWt = CurrentItem.GrossWt,
                    LessWt = CurrentItem.LessWt,
                    NetWt = CurrentItem.NetWt,
                    Rate = CurrentItem.Rate,
                    TaxType = CurrentItem.TaxType,
                    TaxRate = CurrentItem.TaxRate,
                    DiaAmount=CurrentItem.DiaAmount,
                    DiaCt=CurrentItem.DiaCt,


                };

                // Trigger calculations
                item.CalculateAmount();
                item.CalculateTaxAndNetAmount();

                PurchaseItems.Add(item);
                CalculateTotalAmount();

                // Reset form
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelPurchase(object parameter)
        {
            var result = MessageBox.Show("Are you sure you want to cancel? All unsaved changes will be lost.",
                "Confirm Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Reset the form
                CurrentPurchase = new Purchase { PurchaseDate = DateTime.Now };
                PurchaseItems.Clear();
                ResetForm();
                SelectedParty = null;

                // Close the window if a window was passed as parameter
                if (parameter is Window window)
                {
                    window.Close();
                }
            }
        }

        private bool CanUpdateItem()
        {
            return SelectedPurchaseItem != null && !string.IsNullOrEmpty(CurrentItem?.Metal) && CurrentItem.Pcs > 0;
        }

        private void UpdateItem(object parameter)
        {
            if (SelectedPurchaseItem == null) return;

            try
            {
                // Update the selected item with edited values
                var index = PurchaseItems.IndexOf(SelectedPurchaseItem);
                if (index >= 0)
                {
                    // Create a new item with updated values to ensure all calculations are triggered
                    var updatedItem = new PurchaseItem
                    {
                        Metal = CurrentItem.Metal,
                        HSNCode = CurrentItem.HSNCode,
                        HuidNo = CurrentItem.HuidNo,
                        ItemName = CurrentItem.ItemName,
                        Purity = CurrentItem.Purity,
                        Pcs = CurrentItem.Pcs,
                        GrossWt = CurrentItem.GrossWt,
                        LessWt = CurrentItem.LessWt,
                        NetWt = CurrentItem.NetWt,
                        Rate = CurrentItem.Rate,
                        TaxType = CurrentItem.TaxType,
                        TaxRate = CurrentItem.TaxRate
                    };

                    // Trigger calculations
                    updatedItem.CalculateAmount();
                    updatedItem.CalculateTaxAndNetAmount();

                    // Preserve the original ID if it exists
                    updatedItem.Id = PurchaseItems[index].Id;

                    // Force UI update by removing and re-adding the item
                    PurchaseItems.RemoveAt(index);
                    PurchaseItems.Insert(index, updatedItem);

                    // Reset form
                    ResetForm();
                    CalculateTotalAmount();

                    MessageBox.Show("Item updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating item: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelUpdate(object parameter)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            CurrentItem = new PurchaseItem { Pcs = 1 };
            SelectedMetal = null;
            SelectedPurchaseItem = null;
            IsUpdateMode = false;
        }

        private void RemoveItem(object parameter)
        {
            var itemToRemove = parameter as PurchaseItem ?? SelectedPurchaseItem;

            if (itemToRemove == null) return;

            var result = MessageBox.Show("Are you sure you want to remove this item?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                PurchaseItems.Remove(itemToRemove);
                CalculateTotalAmount();

                // If we're in update mode and the selected item is being removed, reset the form
                if (SelectedPurchaseItem == itemToRemove)
                {
                    ResetForm();
                }
            }
        }

        private async void SavePurchase(object parameter)
        {
            var party = Parties ;
            try
            {
                if (!IsValidPurchase(CurrentPurchase))
                    throw new ArgumentException("Purchase details are incomplete or invalid.");

                if (PurchaseItems.Count == 0)
                    throw new ArgumentException("Add at least one purchase item.");

                // Create a new PurchaseRequest with current purchase and items
                var purchaseRequest = new PurchaseRequest
                {
                    Purchase = CurrentPurchase,
                    PurchaseItems = new List<PurchaseItem>(PurchaseItems)
                };

                // Call the repository to save the purchase
                if (_purchaseRepository == null)
                {
                    System.Diagnostics.Debug.WriteLine("CRITICAL: _purchaseRepository is NULL right before calling AddAsync!");
                    MessageBox.Show("Critical error: Purchase repository is not initialized. Please contact support.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Prevent NullReferenceException
                }
                //CurrentPurchase.CreatedByUserId = _currentUser.Id; // Or CreatedBy
                CurrentPurchase.firm_id = _currentUser.firm_id;     // Assuming Purchase has a FirmId prope
                CurrentPurchase.Admin_user = _currentUser.user_name;
                CurrentPurchase.Createdby = _currentUser.server_id;// Assuming Purchase has a FirmId property
                //CurrentPurchase.Party_name = 

                System.Diagnostics.Debug.WriteLine($"_purchaseRepository instance: {_purchaseRepository.GetType().FullName}");
                await _purchaseRepository.AddAsync(purchaseRequest);

                MessageBox.Show("Purchase saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ClearWindow(parameter);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving purchase: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearWindow(object parameter)
        {
            CurrentPurchase = new Purchase { PurchaseDate = DateTime.Now };
            PurchaseItems.Clear();
            ResetForm();
            SelectedParty = null;

            // Close the window if a window was passed as parameter
            if (parameter is Window window)
            {
                window.Close();
            }

        }

        private bool IsValidPurchase(Purchase purchase)
        {
            if (purchase == null)
            {
                MessageBox.Show("Purchase object is null.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (purchase.SupplierId <= 0) // Assuming SupplierId is an int and should be positive
            {
                MessageBox.Show("Please select a supplier.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(purchase.BillNo))
            {
                MessageBox.Show("Bill number cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            // Add any other essential checks, e.g., PurchaseDate
            return true;
        }

        // Helpers for validation & calculation
        private bool IsValidItem(PurchaseItem item) =>
            !string.IsNullOrWhiteSpace(item.Metal)
            && !string.IsNullOrWhiteSpace(item.ItemName)
            && item.GrossWt > 0;

        private void CalculateTotalAmount()
        {
            if (CurrentPurchase == null)
            {
                // This indicates CurrentPurchase was null when CalculateTotalAmount was called.
                // This might happen if XAML bindings set CurrentPurchase to null during initialization.
                System.Diagnostics.Debug.WriteLine("PurchaseViewModel.CalculateTotalAmount() called when CurrentPurchase was null. Aborting calculation.");
                return; // Exit the method to prevent NullReferenceException
            }

            if (PurchaseItems != null && PurchaseItems.Any())
            {
                // Calculate totals from all items
                decimal itemsTotal = PurchaseItems.Sum(i => i.Amount ?? 0m);
                decimal totalTax = PurchaseItems.Sum(i => i.TaxAmount ?? 0m);

                // Update the purchase with items total and tax
                CurrentPurchase.TotalAmount = itemsTotal;
                CurrentPurchase.Tax = (decimal)totalTax;
                CurrentPurchase.TaxAmount = totalTax;

                // Calculate net amount with discount (
                // discount is applied after tax)
                decimal totalBeforeDiscount = itemsTotal + totalTax;
                decimal discount = CurrentPurchase.Discount; // Directly use the discount value
                CurrentPurchase.NetAmount = Math.Max(0, totalBeforeDiscount - discount);

                OnPropertyChanged(nameof(CurrentPurchase));
            }
            else
            {
                // Reset totals if no items or PurchaseItems is null
                CurrentPurchase.TotalAmount = 0;
                CurrentPurchase.Tax = 0;
                CurrentPurchase.TaxAmount = 0;
                CurrentPurchase.NetAmount = 0;
                //CurrentPurchase.Discount = 0; // Reset discount when no items
                OnPropertyChanged(nameof(CurrentPurchase));
            }
        }

        // This method is now handled at the item level
        private void CalculateTax()
        {
            // Update tax for all items when tax percentage changes
            foreach (var item in PurchaseItems)
            {
                item.TaxRate = TaxPercentage;
                item.CalculateNetPrice(); // This will recalculate tax and net amount
            }

            // Recalculate totals
            CalculateTotalAmount();
        }

        // Helper methods for calculations
        private void ValidateDiamondItem(PurchaseItem item)
        {
            if (item.DiaCt <= 0 || item.Rate <= 0)
            {
                throw new ArgumentException("Please enter valid Diamond Carat and Rate for Loose/Cut Diamond.");
            }
        }

        private void ValidateMetalItem(PurchaseItem item)
        {
            if ((item.GrossWt ?? 0) <= 0 || item.Rate <= 0)
            {
                throw new ArgumentException("Please enter valid Gross Weight and Rate for the selected metal.");
            }
        }

        private void CalculateDiamondItemAmount(PurchaseItem item)
        {
         
        }

        private void CalculateMetalItemAmount(PurchaseItem item)
        {
            item.CalculateNetWt();
            item.CalculateAmount();
        }

        private decimal ParseTaxPercentage(string taxType)
        {
            if (string.IsNullOrEmpty(taxType)) return 0;

            var taxMap = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                {"3%", 3m},
                {"5%", 5m},
                {"12%", 12m},
                {"18%", 18m}
            };

            return taxMap.FirstOrDefault(x => taxType.Contains(x.Key)).Value;
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}