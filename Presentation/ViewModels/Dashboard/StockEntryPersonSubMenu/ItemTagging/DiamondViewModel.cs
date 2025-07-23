using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Application.Helpers;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging;

namespace Terret_Billing.Presentation.ViewModels.Dashboard.StockEntryPersonSubMenu.ItemTagging
{
    public class DiamondViewModel : INotifyPropertyChanged
    {
        private readonly IItemRepository _itemRepository;
        private readonly IDiamondService _diamondService;
        private readonly User _currentUser;
        private string _firmId;
        private string _purity;
        private DiamondTaggingEntry _diamondEntry;
        private ObservableCollection<string> _puritySuggestions = new();

        // Pagination object
        private TaggingPagination _pagination;

        public ObservableCollection<string> PuritySuggestions
        {
            get => _puritySuggestions;
            set => SetProperty(ref _puritySuggestions, value);
        }

        public long StockId
        {
            get; set;
        }

        public DiamondTaggingEntry DiamondEntry
        {
            get => _diamondEntry;
            set
            {
                _diamondEntry = value;
                OnPropertyChanged(nameof(DiamondEntry));
            }
        }


        public ObservableCollection<string> Categories { get; } = new();
        public ObservableCollection<string> SubCategories { get; } = new();
        public ObservableCollection<string> Designs { get; } = new();
        public ObservableCollection<DiamondTaggingEntry> DiamondEntries { get; } = new();

        // Pagination object
        public TaggingPagination Pagination
        {
            get => _pagination ??= new TaggingPagination();
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }

        // Pagination commands
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public List<DiamondTaggingEntry> _allItems { get; set; } = new List<DiamondTaggingEntry>();
       


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

                }
            }
        }


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
                    _ = LoadCategoriesAsync();
                }
            }
        }

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



        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public DiamondViewModel(IItemRepository itemRepository, IDiamondService diamondService, string firmId, User currentUser, string purity = null)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _diamondService = diamondService ?? throw new ArgumentNullException(nameof(diamondService));
            _firmId = firmId ?? throw new ArgumentNullException(nameof(firmId));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _purity = purity;
            DiamondEntries = new ObservableCollection<DiamondTaggingEntry>();
            _diamondEntry = new DiamondTaggingEntry();

            // Initialize pagination commands
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);

            // Initialize commands
            //SaveCommand = new RelayCommand(async (param) => await SaveDiamondEntry(param));
            SaveCommand = new RelayCommand(async (param) =>
            {
                if (param is Window window)
                {
                    try
                    {
                        await SaveDiamondEntry(
                            particular: string.Empty,
                            partyName: string.Empty,
                            invoiceNo: string.Empty,
                            date: DateTime.Now,
                            purity: _purity,
                            firmId: _firmId
                        );
                    }
                    catch (Exception ex)
                    {
                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            MessageBox.Show($"Error saving entry: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                    }
                }
            });
            CancelCommand = new RelayCommand(CloseWindow);
        }

        // Removed duplicate DiamondEntry property

        //public async Task LoadCategoriesAsync()
        //{
        //    try
        //    {
        //        var categories = await _itemRepository.GetCategoriesByMetalTypeAsync("Diamond", _firmId);
        //        Category.Clear();
        //        foreach (var category in categories)
        //        {
        //            Category.Add(category);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
        //            MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        private string NormalizeMetalType(string metalType)
        {
            if (string.IsNullOrEmpty(metalType))
                return metalType;

            return metalType.IndexOf("diamond", StringComparison.OrdinalIgnoreCase) >= 0 ? "Diamond" :

                   metalType.Trim();
        }

        public async Task LoadCategoriesAsync()
        {


            try
            {
                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var categories = await _itemRepository.GetCategoriesByMetalTypeAsync("Diamond", _currentUser.firm_id);

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
                var subCategories = await _itemRepository.GetSubCategoriesAsync("Diamond", _selectedCategory, _currentUser.firm_id);

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

        private async Task LoadDesignsAsync(string _selectedCategory, string _selectedSubCategory, string firmid)
        {
            if (string.IsNullOrEmpty(Stock_Type) || string.IsNullOrEmpty(_selectedCategory) ||
                string.IsNullOrEmpty(_selectedSubCategory))
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => Designs.Clear());
                return;
            }

            try
            {
                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var designs = await _itemRepository.GetDesignsAsync("Diamond", _selectedCategory, _selectedSubCategory, _currentUser.firm_id);

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
        public async Task LoadItemsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Stock_Type) || string.IsNullOrEmpty(FirmId))
                {
                    await System.Windows.Application.Current.Dispatcher.InvokeAsync(DiamondEntries.Clear);
                    _allItems = new List<DiamondTaggingEntry>();
                    ApplyPagination();
                    return;
                }

                var metalTypeToQuery = NormalizeMetalType(Stock_Type);
                var entries = await _diamondService.GetAllDiamondEntriesAsync(metalTypeToQuery, FirmId);
                _allItems = entries.ToList();
                ApplyPagination();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading items", ex);
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

        private async Task LoadHSNCodeAsync(string _selectedCategory, string _selectedSubCategory, string _selectedDesign)
        {
            if (string.IsNullOrEmpty(_selectedDesign))
            {
                HSN_No = string.Empty;
                return;
            }

            try
            {
                var hsnCode = await _itemRepository.GetHSNCodeAsync("Diamond", _selectedCategory, _selectedSubCategory, _selectedDesign);
                HSN_No = hsnCode;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading HSN code", ex);
            }

        }

        public async Task LoadDiamondEntriesAsync(string invoiceNo = null, string stockType = null)
        {
            try
            {
                var entries = await _diamondService.GetDiamondEntriesByTypeAsync(
                    type: stockType ?? string.Empty,
                    firmId: _firmId,
                    invoiceNumber: invoiceNo
                );

                _allItems = entries?.ToList() ?? new List<DiamondTaggingEntry>();
                ApplyPagination();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync("Error loading diamond entries", ex);
            }
        }
        public async Task<bool> SaveDiamondEntry(
                 string particular,
                 string partyName,
                 string invoiceNo,
                 DateTime? date = null,
                 string image = null,
                 string category = null,
                 string subCategory = null,
                 string design = null,
                 string purity = null,
                 string hsnNo = null,
                 string huidNo = null,
                 string size = null,
                 string metaltype = null,
                 int? pcs = null,
                 decimal? grossWt = null,
                 int? lessWt = null,
                 decimal? diamondCt = null,
                 decimal? diamondWtGm = null,
                 decimal? netWt = null,
                 decimal? diamondRate = null,
                 decimal? diamondAmt = null,
                 decimal? netRate = null,
                 decimal? netAmt = null,
                 decimal? stoneCt = null,
                 decimal? stoneAmt = null,
                 string description = null,
                 string dropDown = null,
                 decimal? otherCharges = null,
                 decimal? value = null,
                 decimal? finalAmount = null,
                 decimal? grossAmount = null,
                 string remark = null,
                 string srNo = null,
                 string barcode = null,
                 int? itemId = null,
                 string firmId = null,
                 string comment = null,
                 bool? isDataPostOnServer = null,
                 long? localId = null,
                 long? createdBy = null,
                 long? stockId = null)
        {
            try
            {
                var entry = DiamondTaggingEntry.Create(
                    firmId: _firmId,
                    userName: _currentUser.user_name,
                    stockType: "Diamond",
                    particular: particular,
                    partyName: partyName,
                    invoiceNo: invoiceNo,
                    date: date,
                    image: image,
                    category: category,
                    subCategory: subCategory,
                    design: design,
                    purity: purity,
                    hsnNo: hsnNo,
                    huidNo: huidNo,
                    size: size,
                    pcs: pcs,
                    grossWt: grossWt,
                    lessWt: lessWt,
                    diamondCt: diamondCt,
                    diamondWtGm: diamondCt * 0.2m,
                    netWt: netWt,
                    diamondRate: diamondRate,
                    diamondAmt: diamondAmt,
                    netRate: netRate,
                    netAmt: netAmt,
                    stoneCt: stoneCt,
                    stoneAmt: stoneAmt,
                    description: description,
                    dropDown: dropDown,
                    otherCharges: otherCharges,
                    value: value,
                    finalAmount: finalAmount,
                    grossAmount: grossAmount,
                    remark: remark,
                    srNo: srNo,
                    barcode: barcode,
                    itemId: itemId,
                    comment: comment,
                    isDataPostOnServer: isDataPostOnServer,
                    localId: localId,
                    createdby: _currentUser.server_id,
                    stock_id: stockId
                );

                // Save the entry
                await _diamondService.CreateDiamondEntryAsync(entry);

                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show("Diamond entry saved successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                });

                // Reload entries
                await LoadDiamondEntriesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Error saving diamond entry: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                });
                return false;
            }
        }
        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Pagination methods
        private void ApplyPagination()
        {
            try
            {
                var itemsToProcess = _allItems;
                if (itemsToProcess == null) return;

                // Use Paginator helper
                var (pageItems, currentPage, totalPages, totalItems) = Paginator.GetPageFromStoredProc(
                    itemsToProcess, Pagination.CurrentPage, Paginator.DefaultPageSize);

                // Update pagination object
                Pagination.CurrentPage = currentPage;
                Pagination.TotalPages = totalPages;
                Pagination.TotalItems = totalItems;

                // Update UI
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    DiamondEntries.Clear();
                    foreach (var item in pageItems)
                    {
                        DiamondEntries.Add(item);
                    }
                });

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                // Handle error silently for now
                System.Diagnostics.Debug.WriteLine($"Error applying pagination: {ex.Message}");
            }
        }

        private void UpdatePaginationInfo()
        {
            Pagination.PageInfo = Paginator.GetPageInfo(Pagination.CurrentPage, Pagination.TotalPages, Pagination.TotalItems, Paginator.DefaultPageSize);
            Pagination.CanGoNext = Paginator.GetNextPage(Pagination.CurrentPage, Pagination.TotalPages).HasValue;
            Pagination.CanGoPrevious = Paginator.GetPreviousPage(Pagination.CurrentPage).HasValue;
        }

        private void NextPage(object parameter = null)
        {
            var nextPage = Paginator.GetNextPage(Pagination.CurrentPage, Pagination.TotalPages);
            if (nextPage.HasValue)
            {
                Pagination.CurrentPage = nextPage.Value;
                ApplyPagination();
            }
        }

        private void PreviousPage(object parameter = null)
        {
            var prevPage = Paginator.GetPreviousPage(Pagination.CurrentPage);
            if (prevPage.HasValue)
            {
                Pagination.CurrentPage = prevPage.Value;
                ApplyPagination();
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
    }
}
