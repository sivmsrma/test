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
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private readonly IItemService _itemService;
        private bool _isLoading;
        private string _statusMessage;
        private string _errorMessage;
        private string _searchText;
        private ObservableCollection<HsnItem> _hsnItems = new ObservableCollection<HsnItem>();
        private ObservableCollection<HsnItem> _filteredHsnItems = new ObservableCollection<HsnItem>();
        private readonly User _currentUser;
        private string _username;
        private string _firmId;
        // Item properties
        private string _metalType;
        public string MetalType
        {
            get => _metalType;
            set
            {
                if (_metalType != value)
                {
                    _metalType = value;
                    OnPropertyChanged();
                    // You can add additional logic here if needed when MetalType changes
                }
            }
        }
        private string _category;
        private string _subCategory;
        private string _design;
        private string _hsnCode;
        private string _shortName;
        private string _barcode;

        private HsnItem _selectedHsnItem;


        public AddItemViewModel(IItemService itemService, User currentUser)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _firmId = currentUser?.FirmId ?? string.Empty;
            _username = currentUser?.user_name;

            // Initialize commands
            AddItemCommand = new AsyncCommand(AddItemAsync, () => !IsLoading);
            ClearFormCommand = new RelayCommands(_ => ClearForm(), () => !IsLoading);
            SearchHsnCommand = new RelayCommands(_ => FilterHsnItems(), () => !IsLoading);
            SelectHsnCommand = new RelayCommands(param => SelectHsn(param), () => !IsLoading);
            
            // Load HSN items
            LoadHsnItemsAsync();

            // Initialize metal type if not set
            if (string.IsNullOrEmpty(MetalType))
            {
                MetalType = "Gold";  // Default to Gold if no metal type is set
            }
        }

        #region Properties

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }


        // Item properties with change notification
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public string SubCategory
        {
            get => _subCategory;
            set { _subCategory = value; OnPropertyChanged(); }
        }

        public string Design
        {
            get => _design;
            set { _design = value; OnPropertyChanged(); }
        }

        public string HsnCode
        {
            get => _hsnCode;
            set { _hsnCode = value; OnPropertyChanged(); }
        }

        public string ShortName
        {
            get => _shortName;
            set { _shortName = value; OnPropertyChanged(); }
        }

        public string Barcode
        {
            get => _barcode;
            set { _barcode = value; OnPropertyChanged(); }
        }

        public ObservableCollection<HsnItem> HsnItems
        {
            get => _hsnItems;
            set
            {
                _hsnItems = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<HsnItem> FilteredHsnItems
        {
            get => _filteredHsnItems;
            set
            {
                _filteredHsnItems = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Commands

        public ICommand AddItemCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand SearchHsnCommand { get; }
        public ICommand SelectHsnCommand { get; }

        #endregion

        #region Public Methods

        public async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                await LoadHsnItemsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error initializing: {ex.Message}";
                Logger.LogError("Error in AddItemViewModel.InitializeAsync", ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Private Methods

        private async Task LoadHsnItemsAsync()
        {
            try
            {
                IsLoading = true;
                var items = await _itemService.GetHsnItemsAsync();
                HsnItems = new ObservableCollection<HsnItem>(items);
                FilteredHsnItems = new ObservableCollection<HsnItem>(items);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading HSN items: {ex.Message}";
                Logger.LogError("Error in AddItemViewModel.LoadHsnItemsAsync", ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterHsnItems()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredHsnItems = new ObservableCollection<HsnItem>(HsnItems);
            }
            else
            {
                var searchText = SearchText.ToLower();
                var filtered = HsnItems.Where(item =>
                    (item.Metal != null && item.Metal.ToLower().Contains(searchText)) ||
                    (item.HsnCode != null && item.HsnCode.ToLower().Contains(searchText))
                ).ToList();

                FilteredHsnItems = new ObservableCollection<HsnItem>(filtered);
            }
        }

        private async Task AddItemAsync()
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(MetalType) ||
                    string.IsNullOrWhiteSpace(Category) ||
                    string.IsNullOrWhiteSpace(SubCategory) ||
                    string.IsNullOrWhiteSpace(Design) ||
                    string.IsNullOrWhiteSpace(HsnCode) ||
                    string.IsNullOrWhiteSpace(ShortName))
                {
                    ErrorMessage = "Please fill in all required fields.";
                    return;
                }

                IsLoading = true;
                ErrorMessage = string.Empty;
                StatusMessage = "Adding item...";

                // Get the selected HSN item to get the HsnId
                var selectedHsn = HsnItems.FirstOrDefault(h => h.HsnCode == HsnCode);
                if (selectedHsn == null)
                {
                    ErrorMessage = "Please select a valid HSN code.";
                    return;
                }

                // Create new item
                var newItem = new Item
                {
                    MetalType = MetalType.Trim(),
                    Category = Category.Trim(),
                    SubCategory = SubCategory.Trim(),
                    Design = Design.Trim(),
                    HsnId = selectedHsn.HsnId,
                    HsnCode = HsnCode.Trim(),
                    ShortName = ShortName.Trim(),
                    FirmId=_firmId
                };

                // Call service to add item
                var itemId = await _itemService.CreateItemAsync(newItem);
                
                if (itemId > 0)
                {
                    StatusMessage = "Item added successfully!";
                    ClearForm();
                }
                else
                {
                    ErrorMessage = "Failed to add item. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error adding item: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error in AddItemAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SelectHsn(object parameter)
        {
            if (parameter is HsnItem selectedHsn)
            {
                try
                {
                    _selectedHsnItem = selectedHsn;
                    HsnCode = selectedHsn.HsnCode;
                    
                    // Auto-fill metal type if empty
                    if (string.IsNullOrEmpty(MetalType) && !string.IsNullOrEmpty(selectedHsn.Metal))
                    {
                        MetalType = selectedHsn.Metal;
                    }
                    
                    StatusMessage = $"Selected HSN: {selectedHsn.HsnCode}";
                    ErrorMessage = string.Empty; // Clear any previous error
                    
                    // Log the selection for debugging
                    System.Diagnostics.Debug.WriteLine($"Selected HSN: {selectedHsn.HsnCode}, HsnId: {selectedHsn.HsnId}");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error selecting HSN: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine($"Error in SelectHsn: {ex}");
                }
            }
            else
            {
                ErrorMessage = "Invalid HSN selection";
            }
        }

        private void ClearForm()
        {
            MetalType = string.Empty;
            Category = string.Empty;
            SubCategory = string.Empty;
            Design = string.Empty;
            HsnCode = string.Empty;
            ShortName = string.Empty;
            Barcode = string.Empty;
            SearchText = string.Empty;
            ErrorMessage = string.Empty;
            StatusMessage = string.Empty;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }

    // Custom ICommand implementation for async operations
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute();

        public async void Execute(object parameter)
        {
            await _execute();
        }
    }

    // Simple ICommand implementation for sync operations
    public class RelayCommands : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommands(Action<object> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute();

        public void Execute(object parameter) => _execute(parameter);
    }
}
#endregion