using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Terret_Billing.Presentation.Models;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data.Repositories;
using System.Drawing;
using System.Windows.Input;
using Terret_Billing.Application.Helpers;
using Terret_Billing.Presentation.Commands;
using System.Collections.Generic;
using System.Threading;

namespace Terret_Billing.Presentation.ViewModels.Dashboard.StockEntryPersonSubMenu.ItemTagging
{
    public class GoldAndSilverViewModel : INotifyPropertyChanged

    {
        private readonly IItemRepository _itemRepository;
        private User _currentUser;
        private readonly IGoldAndSilverService _goldAndSilverService;
        private GoldAndSilverItemModel _GoldAndSilverItemModel;
       
        // Observable collections for dropdowns
        private ObservableCollection<string> _puritySuggestions = new();
        public ObservableCollection<string> PuritySuggestions
        {
            get => _puritySuggestions;
            set => SetProperty(ref _puritySuggestions, value);
        }

        public ObservableCollection<string> Categories { get; } = new();
        public ObservableCollection<string> SubCategories { get; } = new();
        public ObservableCollection<string> Designs { get; } = new();
        public ObservableCollection<GoldAndSilverTaggingEntry> Items { get; } = new();

        // Properties
        private string _stockType;
        public string Stock_Type
        {
            get => _stockType;
            set
            {
                if (_stockType != value)
                {
                    _stockType = value;
                    OnPropertyChanged();
                    _ = LoadCategoriesAsync();
                }
            }
        }


        private DateTime? _entry_Date;
        public DateTime? Entry_Date
        {
            get => _entry_Date;
            set
            {
                _entry_Date = DateTime.Now;
                OnPropertyChanged();
            }
        }

        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get => _isSelected;
        //    set
        //    {
        //        if (_isSelected != value)
        //        {
        //            _isSelected = value;
        //            OnPropertyChanged(nameof(IsSelected));
        //        }
        //    }
        //}


        private string _firmId;
        public string FirmId
        {
            get => _firmId;
            set
            {
                if (_firmId != value)
                {
                    _firmId = value;
                    OnPropertyChanged();
                    _ = LoadItemsAsync();
                }
            }
        }



        private string _selectedMetalType;
        public string SelectedMetalType
        {
            get => _selectedMetalType;
            set
            {
                if (_selectedMetalType != value)
                {
                    _selectedMetalType = value;
                    OnPropertyChanged();
                    Stock_Type = value;
                }
            }
        }

        private string _purity;
        public string Purity
        {
            get => _purity;
            set
            {
                if (_purity != value)
                {
                    _purity = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    _ = LoadSubCategoriesAsync(_selectedCategory);
                }
            }
        }

        private string _selectedSubCategory;
        public string SelectedSubCategory
        {
            get => _selectedSubCategory;
            set
            {
                if (_selectedSubCategory != value)
                {
                    _selectedSubCategory = value;
                    OnPropertyChanged();
                    _ = LoadDesignsAsync(_selectedCategory, _selectedSubCategory, _currentUser.firm_id);
                }
            }
        }

        private string _selectedDesign;
        public string SelectedDesign
        {
            get => _selectedDesign;
            set
            {
                if (_selectedDesign != value)
                {
                    _selectedDesign = value;
                    OnPropertyChanged();
                    _ = LoadItemDetailsAsync();
                    _ = LoadHSNCodeAsync(_selectedCategory, _selectedSubCategory, _selectedDesign);
                }
            }
        }

        private string _hsnNo;
        public string HSN_No
        {
            get => _hsnNo;
            set
            {
                if (_hsnNo != value)
                {
                    _hsnNo = value;
                    OnPropertyChanged(nameof(HSN_No));
                }
            }
        }

        private string _barcode;
        public string Barcode
        {
            get => _barcode;
            set
            {
                if (_barcode != value)
                {
                    _barcode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _size;
        public string Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _pcs;
        public int Pcs
        {
            get => _pcs;
            set
            {
                if (_pcs != value)
                {
                    _pcs = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _subCategory;
        public string SubCategory
        {
            get => _subCategory;
            set
            {
                if (_subCategory != value)
                {
                    _subCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _design;
        public string Design
        {
            get => _design;
            set
            {
                if (_design != value)
                {
                    _design = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedItemId;
        public string SelectedItemId
        {
            get => _selectedItemId;
            set
            {
                if (_selectedItemId != value)
                {
                    _selectedItemId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _userInfo;
        public string UserInfo
        {
            get => _userInfo;
            set
            {
                if (_userInfo != value)
                {
                    _userInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public GoldAndSilverItemModel goldAndSilver
        {
            get => _GoldAndSilverItemModel ??= new GoldAndSilverItemModel();
            set
            {
                _GoldAndSilverItemModel = value;
                OnPropertyChanged(nameof(goldAndSilver));
            }
        }

        private TaggingPagination _pagination;
        public TaggingPagination Pagination
        {
            get => _pagination ??= new TaggingPagination();
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private List<GoldAndSilverTaggingEntry> _allItems = new List<GoldAndSilverTaggingEntry>();
        private int _loadItemsCallVersion = 0;
        private CancellationTokenSource _loadItemsCts;

        // Add this property for AvailableWeight
        private decimal _availableWeight;
        public decimal AvailableWeight
        {
            get => _availableWeight;
            set
            {
                if (_availableWeight != value)
                {
                    _availableWeight = value;
                    System.Diagnostics.Debug.WriteLine($"AvailableWeight property changed: {_availableWeight}");
                    OnPropertyChanged(nameof(AvailableWeight));
                }
            }
        }

        // Add Party_Name property
        private string _partyName;
        public string Party_Name
        {
            get => _partyName;
            set
            {
                if (_partyName != value)
                {
                    _partyName = value;
                    OnPropertyChanged();
                }
            }
        }

        // Add Invoice_No property
        private string _invoiceNo;
        public string Invoice_No
        {
            get => _invoiceNo;
            set
            {
                if (_invoiceNo != value)
                {
                    _invoiceNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public long StockId { get; set; }


        public GoldAndSilverViewModel(IItemRepository itemRepository, IGoldAndSilverService goldAndSilverService, User currentUser)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _goldAndSilverService = goldAndSilverService ?? throw new ArgumentNullException(nameof(goldAndSilverService));
            _GoldAndSilverItemModel = new GoldAndSilverItemModel();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
        }

        private string NormalizeMetalType(string metalType)
        {
            if (string.IsNullOrEmpty(metalType))
                return metalType;

            return metalType.IndexOf("gold", StringComparison.OrdinalIgnoreCase) >= 0 ? "Gold" :
                   metalType.IndexOf("silver", StringComparison.OrdinalIgnoreCase) >= 0 ? "Silver" :
                   metalType.Trim();
        }

        private async Task LoadCategoriesAsync()
        {


            try
            {
                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var categories = await _itemRepository.GetCategoriesByMetalTypeAsync(metalTypeToQuery, _currentUser.firm_id);

                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Categories.Clear();
                    foreach (var category in categories.Where(c => !string.IsNullOrEmpty(c)))
                    {
                        Categories.Add(category);
                    }

                    // Clear dependent fields
                    SubCategories.Clear();
                    Designs.Clear();
                    HSN_No = string.Empty;
                    Barcode = string.Empty;
                });
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading categories", ex);
            }
        }

        private async Task LoadSubCategoriesAsync(string _selectedCategory)
        {


            try
            {
                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var subCategories = await _itemRepository.GetSubCategoriesAsync(metalTypeToQuery, _selectedCategory, _currentUser.firm_id);

                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    SubCategories.Clear();
                    foreach (var subCategory in subCategories.Where(sc => !string.IsNullOrEmpty(sc)))
                    {
                        SubCategories.Add(subCategory);
                    }
                    HSN_No = string.Empty;
                    Barcode = string.Empty;
                });
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading subcategories", ex);
            }
        }

        private async Task LoadDesignsAsync(string _selectedCategory, string _selectedSubCategory, string firm_id )
        {
            if (string.IsNullOrEmpty(Stock_Type) || string.IsNullOrEmpty(_selectedCategory) ||
                string.IsNullOrEmpty(_selectedSubCategory))
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(Designs.Clear);
                return;
            }

            try
            {
                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var designs = await _itemRepository.GetDesignsAsync(metalTypeToQuery, _selectedCategory, _selectedSubCategory, _currentUser.firm_id);

                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Designs.Clear();
                    foreach (var design in designs.Where(d => !string.IsNullOrEmpty(d)))
                    {
                        Designs.Add(design);
                    }

                    HSN_No = string.Empty;
                    Barcode = string.Empty;
                });
            }

            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading designs", ex);
            }
        }

        private async Task LoadHSNCodeAsync(string _selectedCategory, string _selectedSubCategory, string _selectedDesign)
        {
            if (string.IsNullOrEmpty(_selectedDesign))
            {
                HSN_No = string.Empty;
                return;
            }

            try
            {
                var hsnCode = await _itemRepository.GetHSNCodeAsync(Stock_Type, _selectedCategory, _selectedSubCategory, _selectedDesign);
                HSN_No = hsnCode;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading HSN code", ex);
            }

        }

        
        private async Task LoadPuritySuggestionsAsync()
        {
            if (string.IsNullOrEmpty(Stock_Type)) return;

            try
            {
                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var purities = await _itemRepository.GetPuritiesByMetalTypeAsync(metalTypeToQuery);

                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    PuritySuggestions.Clear();
                    foreach (var purity in purities.Where(p => !string.IsNullOrEmpty(p) && !PuritySuggestions.Contains(p)))
                    {
                        PuritySuggestions.Add(purity);
                    }
                });
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading purity suggestions", ex);
            }
        }

        public async Task LoadItemsAsync()
        {
            _loadItemsCts?.Cancel();
            _loadItemsCts = new CancellationTokenSource();
            var token = _loadItemsCts.Token;

            // Ensure FirmId and Stock_Type are set
            if (string.IsNullOrEmpty(FirmId))
            {
                if (_currentUser != null && !string.IsNullOrEmpty(_currentUser.firm_id))
                    FirmId = _currentUser.firm_id;
            }
            if (string.IsNullOrEmpty(Stock_Type))
            {
                if (!string.IsNullOrEmpty(SelectedMetalType))
                    Stock_Type = SelectedMetalType;
                else
                    Stock_Type = "Gold"; // Default to Gold if not set
            }

            // Debug log for parameters
            System.Diagnostics.Debug.WriteLine($"[GoldAndSilverViewModel] LoadItemsAsync: FirmId={FirmId}, Stock_Type={Stock_Type}, Page={Pagination.CurrentPage}, PageSize={Paginator.DefaultPageSize}");

            if (string.IsNullOrEmpty(Stock_Type) || string.IsNullOrEmpty(FirmId))
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(Items.Clear);
                Pagination.CurrentPage = 1;
                UpdatePaginationInfo(1, 1, 0);
                return;
            }

            int callVersion = ++_loadItemsCallVersion;

            var metalTypeToQuery = NormalizeMetalType(Stock_Type);
            int pageNumber = Pagination.CurrentPage;
            int pageSize = Paginator.DefaultPageSize;

            var (pageItems, totalCount) = await _goldAndSilverService.GetEntriesByTypePagedAsync(
                metalTypeToQuery, FirmId, null, pageNumber, pageSize);

            if (token.IsCancellationRequested) return;
            if (callVersion == _loadItemsCallVersion)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Items.Clear();
                    foreach (var item in pageItems)
                    {
                        Items.Add(item);
                    }
                });
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                UpdatePaginationInfo(pageNumber, totalPages, totalCount);
            }
        }

        private async Task LoadItemDetailsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedDesign))
                    return;


            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading item details", ex);
            }
        }

        private void UpdatePaginationInfo(int currentPage, int totalPages, int totalItems)
        {
            Pagination.CurrentPage = currentPage;
            Pagination.TotalPages = totalPages;
            Pagination.TotalItems = totalItems;
            Pagination.PageInfo = Paginator.GetPageInfo(currentPage, totalPages, totalItems, Paginator.DefaultPageSize);
            Pagination.CanGoNext = Paginator.GetNextPage(currentPage, totalPages).HasValue;
            Pagination.CanGoPrevious = Paginator.GetPreviousPage(currentPage).HasValue;
        }

        private void NextPage(object parameter = null)
        {
            var nextPage = Paginator.GetNextPage(Pagination.CurrentPage, Pagination.TotalPages);
            if (nextPage.HasValue)
            {
                Pagination.CurrentPage = nextPage.Value;
                _ = LoadItemsAsync();
            }
        }

        private void PreviousPage(object parameter = null)
        {
            var prevPage = Paginator.GetPreviousPage(Pagination.CurrentPage);
            if (prevPage.HasValue)
            {
                Pagination.CurrentPage = prevPage.Value;
                _ = LoadItemsAsync();
            }
        }

        private bool CanGoToNextPage(object parameter = null)
        {
            return Pagination.CanGoNext;
        }

        private bool CanGoToPreviousPage(object parameter = null)
        {
            return Pagination.CanGoPrevious;
        }

        public void ClearEntry()
        {
            //Stock_Type = null;
            //HSN_No = null;
            //Barcode = null;
            //SelectedItemId = null;
            //Purity = null;
            //Size = null;
        }

        public async Task<bool> SaveEntryAsync(

                string particular,
                string stock_Type,
                string partyName,
                string invoiceNo,
                DateTime entryDate,
                string purityType,
                string metalType,
                string category,
                string subCategory,
                string design,
                string hsnNo,
                string barcode,
                decimal weight,
                decimal tunchWeight,
                string tunchType,
                decimal tunchPercentage,
                string description,
                string firmIdStr,
                string size,
                int pcs = 0,
                long stockId = 0,
               long CreatedBy = 0, 
              decimal LessWt = 0 ,
               decimal NetWt=0
            
 )
        {
            try
            {
                if (string.IsNullOrEmpty(metalType) || string.IsNullOrEmpty(firmIdStr))
                {
                    await ShowMessageAsync("Error", "Please select a metal type and ensure Firm ID is valid.");
                    return false;
                }

                //if (!string.IsNullOrEmpty(SelectedItemId) && int.TryParse(SelectedItemId, out int stockId))
                //{
                //    bool weightUpdated = await _goldAndSilverService.UpdatePendingWeightAsync(stockId, weight);
                //    if (!weightUpdated)
                //    {
                //        await ShowMessageAsync("Error", "Insufficient pending weight available for this stock item.");
                //        return false;
                //    }
                //}

                var entry = new GoldAndSilverTaggingEntry
                {
                    // General Info
                    Firm_Id = firmIdStr,
                    Metal_type = metalType,
                    User_Name = _currentUser.user_name,
                    Stock_Type = stock_Type,
                    Entry_Date = entryDate,
                    IsDataPostOnServer = false,

                    // Party & Invoice
                    Particular = particular,
                    Party_Name = partyName,
                    Invoice_No = invoiceNo,
                    CommentVal = description,

                    // Item Details
                    Category = category,
                    SubCategory = subCategory,
                    Design = design,
                    Purity = purityType,
                    HSN_No = hsnNo,
                    SizeVal = size,
                    Pcs = pcs,
                    Barcode = barcode,

                    // Weight
                    Gross_Wt = weight,
                    Less_Wt = LessWt,
                    Net_Wt = weight - LessWt,

                    // Financials (defaulted to 0)
                    Net_Rate = 0,
                    Net_Amount = 0,

                    Other_Charges = 0,
                    Final_Amount = 0,
                    StockId=stockId,
                    CreatedBy=_currentUser.server_id,
                };


                await _goldAndSilverService.AddEntryAsync(entry);
                await ShowMessageAsync("Success", "Entry saved successfully and pending weight updated!");
                ClearEntry();
                await LoadItemsAsync();
                await FetchAvailableWeightAsync(); 
                return true;
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Failed to save entry: {ex.Message}");
                Debug.WriteLine($"Error in SaveEntryAsync: {ex}");
                return false;
            }
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private async Task ShowMessageAsync(string message, string title = "Error")
        {
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning));
        }

        private async Task HandleErrorAsync(string context, Exception ex)
        {
            Debug.WriteLine($"{context}: {ex}");
            await ShowMessageAsync($"{context}: {ex.Message}");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task FetchAvailableWeightAsync()
        {
           
            var result = await _goldAndSilverService.GetAvailableWeightAsync(
                SelectedMetalType, Party_Name, Invoice_No, Purity);
            AvailableWeight = result ?? 0;
        }
    }
}
