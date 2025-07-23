using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Application.Helpers;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Commands;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Presentation.ViewModels.StockEntryPersonSubMenu
{
    public class TaggingViewModel : INotifyPropertyChanged
    {
        private readonly ITaggingService _taggingService;
        private readonly string _firmId;
        private List<TaggingItem> _allItems = new List<TaggingItem>();
        private string _filterTypeOfStock = string.Empty;
        private string _filterMetalType = string.Empty;
        private string _filterPartyName = string.Empty;
        private string _filterInvoiceNumber = string.Empty;
        private ObservableCollection<TaggingItem> _items;
        private TaggingItem _selectedItem;
        private bool _isLoading;

        // Pagination object
        private TaggingPagination _pagination;

        public ObservableCollection<TaggingItem> Items
        {
            get => _items ??= new ObservableCollection<TaggingItem>();
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public TaggingItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public string FilterTypeOfStock
        {
            get => _filterTypeOfStock;
            set
            {
                _filterTypeOfStock = value;
                OnPropertyChanged(nameof(FilterTypeOfStock));
                _ = LoadDataAsync();
            }
        }

        public string FilterMetalType
        {
            get => _filterMetalType;
            set
            {
                _filterMetalType = value;
                OnPropertyChanged(nameof(FilterMetalType));
                _ = LoadDataAsync();
            }
        }

        public string FilterPartyName
        {
            get => _filterPartyName;
            set
            {
                _filterPartyName = value;
                OnPropertyChanged(nameof(FilterPartyName));
                _ = LoadDataAsync();
            }
        }

        public string FilterInvoiceNumber
        {
            get => _filterInvoiceNumber;
            set
            {
                _filterInvoiceNumber = value;
                OnPropertyChanged(nameof(FilterInvoiceNumber));
                _ = LoadDataAsync();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

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

        public ICommand CancelCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public TaggingViewModel(ITaggingService taggingService, string firmId = null)
        {
            _taggingService = taggingService ?? throw new ArgumentNullException(nameof(taggingService));
            _firmId = firmId;
            CancelCommand = new RelayCommand(CloseWindow);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                var (items, totalCount) = await _taggingService.GetAllTaggingsPagedAsync(
                    _firmId,
                    FilterTypeOfStock,
                    FilterMetalType,
                    FilterPartyName,
                    FilterInvoiceNumber,
                    Pagination.CurrentPage,
                    Paginator.DefaultPageSize);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                });
                Pagination.TotalItems = totalCount;
                Pagination.TotalPages = (int)Math.Ceiling((double)totalCount / Paginator.DefaultPageSize);
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading tagging data", ex);
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
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
                _ = LoadDataAsync();
            }
        }

        private void PreviousPage(object parameter = null)
        {
            var prevPage = Paginator.GetPreviousPage(Pagination.CurrentPage);
            if (prevPage.HasValue)
            {
                Pagination.CurrentPage = prevPage.Value;
                _ = LoadDataAsync();
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



        public void EditSelectedItem()
        {
            try
            {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Please select an item to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // TODO: Implement edit logic
                MessageBox.Show($"Editing item with ID: {SelectedItem.stock_id}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error in EditSelectedItem", ex);
                MessageBox.Show($"Error editing item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportToExcel()
        {
            try
            {
                // TODO: Implement export to Excel logic
                var count = Items?.Count ?? 0;
                MessageBox.Show($"Exporting {count} items to Excel", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error exporting to Excel", ex);
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SetTypeOfStockFilter(string typeOfStock)
        {
            FilterTypeOfStock = typeOfStock ?? string.Empty;
        }

        public void SetMetalTypeFilter(string metalType)
        {
            FilterMetalType = metalType ?? string.Empty;
        }

        public void SetPartyNameFilter(string partyName)
        {
            FilterPartyName = partyName ?? string.Empty;
        }

        public void SetInvoiceNumberFilter(string invoiceNumber)
        {
            FilterInvoiceNumber = invoiceNumber ?? string.Empty;
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
