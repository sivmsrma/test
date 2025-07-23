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
            if (SelectedHSN == null)
            {
                System.Windows.MessageBox.Show("Please select an HSN row from the list.", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // Check if category exists
            bool exists = await _itemInfoService.CategoryExistsAsync(ItemInfo.metal, ItemInfo.category, ItemInfo.firm_id);
            if (exists)
            {
                var result = System.Windows.MessageBox.Show(
                    "Category already exists. Do you want to update it?",
                    "Duplicate Category",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    ItemInfo.hsn_id = SelectedHSN.hsn_id;
                    ItemInfo.hsn_code = SelectedHSN.hsn_code;
                    ItemInfo.firm_id = SelectedHSN.firm_id;
                    await _itemInfoService.UpdateItemAsync(ItemInfo);
                    System.Windows.MessageBox.Show("Category updated successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    return;
                }
                else
                {
                    return;
                }
            }

            ItemInfo.hsn_id = SelectedHSN.hsn_id;
            ItemInfo.hsn_code = SelectedHSN.hsn_code;
            ItemInfo.firm_id = SelectedHSN.firm_id;
            await _itemInfoService.InsertItemAsync(ItemInfo);
            System.Windows.MessageBox.Show("Category saved successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
