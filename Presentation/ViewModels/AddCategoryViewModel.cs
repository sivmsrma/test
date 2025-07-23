using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Presentation.ViewModels
{
    public class AddCategoryViewModel : ViewModelBase
    {
        private readonly IHSNService _hsnService;
        private readonly IItemInfoService _itemInfoService;

        private ObservableCollection<HSNListItem> _hsnItems;
        public ObservableCollection<HSNListItem> HSNItems
        {
            get
            {
                if (_hsnItems == null)
                    _hsnItems = new ObservableCollection<HSNListItem>();
                return _hsnItems;
            }
            set
            {
                if (_hsnItems != value)
                {
                    _hsnItems = value;
                    OnPropertyChanged(nameof(HSNItems));
                }
            }
        }

        private ItemInfo _itemInfo;
        public ItemInfo ItemInfo
        {
            get
            {
                if (_itemInfo == null)
                    _itemInfo = new ItemInfo { firm_id = _firmId };
                return _itemInfo;
            }
            set
            {
                if (_itemInfo != value)
                {
                    _itemInfo = value;
                    OnPropertyChanged(nameof(ItemInfo));
                }
            }
        }

        private HSNListItem _selectedHSN;
        public HSNListItem SelectedHSN
        {
            get => _selectedHSN;
            set
            {
                _selectedHSN = value;
                OnPropertyChanged(nameof(SelectedHSN));
            }
        }

        private ObservableCollection<string> _metalTypes;
        public ObservableCollection<string> MetalTypes
        {
            get
            {
                if (_metalTypes == null)
                    _metalTypes = new ObservableCollection<string> { "Gold", "Silver", "Platinum", "Diamond" };
                return _metalTypes;
            }
        }

        public string SelectedMetal
        {
            get => ItemInfo.metal;
            set
            {
                if (ItemInfo.metal != value)
                {
                    ItemInfo.metal = value;
                    OnPropertyChanged(nameof(SelectedMetal));
                    OnPropertyChanged(nameof(ItemInfo));
                }
            }
        }

        public string ShortName
        {
            get => ItemInfo.short_name;
            set
            {
                var newValue = value?.ToUpper();
                if (ItemInfo.short_name != newValue)
                {
                    ItemInfo.short_name = newValue;
                    OnPropertyChanged(nameof(ShortName));
                    OnPropertyChanged(nameof(ItemInfo));
                }
            }
        }

        public ICommand Load4DigitsCommand { get; }
        public ICommand Load6DigitsCommand { get; }
        public ICommand Load8DigitsCommand { get; }
        public ICommand SaveCommand { get; }

        private readonly string _firmId;

        public AddCategoryViewModel(IHSNService hsnService, IItemInfoService itemInfoService, string firmId)
        {
            _hsnService = hsnService;
            _itemInfoService = itemInfoService;
            _firmId = firmId;

            Load4DigitsCommand = new Presentation.Commands.RelayCommand(async _ => await LoadHSNListAsync(4));
            Load6DigitsCommand = new Presentation.Commands.RelayCommand(async _ => await LoadHSNListAsync(6));
            Load8DigitsCommand = new Presentation.Commands.RelayCommand(async _ => await LoadHSNListAsync(8));
            SaveCommand = new Presentation.Commands.RelayCommand(async _ => await SaveAsync());

            _ = LoadHSNListAsync(8);
        }

        public async Task LoadHSNListAsync(int codeLength)
        {
            if (_hsnService == null)
            {
                System.Windows.MessageBox.Show("HSN Service is not initialized.", "Error");
                return;
            }
            if (ItemInfo == null)
            {
                System.Windows.MessageBox.Show("ItemInfo is not initialized.", "Error");
                return;
            }
            if (string.IsNullOrEmpty(ItemInfo.firm_id))
            {
                System.Windows.MessageBox.Show("Firm ID is missing.", "Error");
                return;
            }
            var data = await _hsnService.GetHSNListWithSyncAsync(ItemInfo.firm_id, codeLength);
            HSNItems.Clear();
            int serial = 1;
            foreach (var item in data)
            {
                item.SerialNumber = serial++;
                HSNItems.Add(item);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                // Ensure ItemInfo is initialized
                if (ItemInfo == null)
                {
                    ItemInfo = new ItemInfo { firm_id = _firmId };
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(ItemInfo.category))
                {
                    System.Windows.MessageBox.Show("Please enter a name for the category.", "Validation Error", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedMetal))
                {
                    System.Windows.MessageBox.Show("Please select a metal type.", "Validation Error", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ShortName))
                {
                    System.Windows.MessageBox.Show("Please enter a short name.", "Validation Error", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                if (SelectedHSN == null)
                {
                    System.Windows.MessageBox.Show("Please select an HSN code from the list.", "Validation Error", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                // Set HSN details
                ItemInfo.hsn_id = SelectedHSN.hsn_id;
                ItemInfo.hsn_code = SelectedHSN.hsn_code;
                ItemInfo.firm_id = _firmId; // Use the firm ID from the constructor
                
                // Set the metal type
                ItemInfo.metal = SelectedMetal;

                // Check if category exists
                //bool exists = await _itemInfoService.CategoryExistsAsync(ItemInfo.metal, ItemInfo.category, ItemInfo.firm_id);
                
                //if (exists)
                //{
                //    var result = System.Windows.MessageBox.Show(
                //        "Category already exists. Do you want to update it?",
                //        "Duplicate Category",
                //        System.Windows.MessageBoxButton.YesNo,
                //        System.Windows.MessageBoxImage.Question);

                //    if (result == System.Windows.MessageBoxResult.Yes)
                //    {
                //        // Since we can't get the existing item, we'll just update based on the current values
                //        // This assumes that CategoryExistsAsync is checking based on metal, category, and firm_id
                //        // and that these fields form a unique key in the database
                //        await _itemInfoService.UpdateItemAsync(ItemInfo);
                //        System.Windows.MessageBox.Show("Category updated successfully!", "Success", 
                //            System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                //    }
                //    return;
                //}

                // Create a new ItemInfo object for insertion to avoid sending the item_id
                var newItem = new ItemInfo
                {
                    category = ItemInfo.category,
                    metal = ItemInfo.metal,
                    sub_category = ItemInfo.sub_category,  // Add this line
    design = ItemInfo.design, 
                    short_name = ItemInfo.short_name,
                    hsn_id = ItemInfo.hsn_id,
                    hsn_code = ItemInfo.hsn_code,
                    firm_id = ItemInfo.firm_id,
                    IsDataPostOnServer = 0 // Assuming default value for new items
                };
                
                // Insert new category
                await _itemInfoService.InsertItemAsync(newItem);
                System.Windows.MessageBox.Show("Category saved successfully!", "Success", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                
                // Reset form after successful save
                ItemInfo = new ItemInfo { firm_id = _firmId };
                OnPropertyChanged(nameof(ItemInfo));
                OnPropertyChanged(nameof(SelectedMetal));
                OnPropertyChanged(nameof(ShortName));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
