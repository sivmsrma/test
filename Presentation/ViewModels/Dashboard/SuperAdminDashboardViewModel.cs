using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;

namespace Terret_Billing.Presentation.ViewModels.Dashboard
{
    public class SuperAdminDashboardViewModel : INotifyPropertyChanged
    {
        private readonly IBranchRepository _branchRepository;
        private ObservableCollection<Branch> _branches;
        private Branch _selectedBranch;
        private decimal _totalSales;
        private decimal _totalPurchases;
        private decimal _totalProfit;
        private bool _isLoading;
        private DispatcherTimer refreshTimer;
        //public SuperAdminDashboardViewModel();
        private readonly User _currentUser;

        public SuperAdminDashboardViewModel(User user)
        {
            IDatabaseHelper helper = new MySqlDatabaseHelper(); // Replace with actual implementation
            _branchRepository = new BranchRepository(helper);

            NewBranchCommand = new RelayCommand(ExecuteNewBranch);
            RefreshBranchesCommand = new RelayCommand(ExecuteRefreshBranches);
            ProfileSettingsCommand = new RelayCommand(ExecuteProfileSettings);
            _currentUser = user;

            refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromSeconds(30);
            refreshTimer.Tick += RefreshPage;
            refreshTimer.Start();
            LoadDashboardDataAsync();
        }

        private void RefreshPage(object sender, EventArgs e)
        {

            LoadDashboardDataAsync();
        }


        #region Properties

        public ObservableCollection<Branch> Branches
        {
            get => _branches;
            set
            {
                _branches = value;
                OnPropertyChanged();
            }
        }

        public Branch SelectedBranch
        {
            get => _selectedBranch;
            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                // Refresh data based on selected branch
                if (_selectedBranch != null)
                {
                    LoadBranchDataAsync(_selectedBranch);
                }
            }
        }

        public decimal TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalPurchases
        {
            get => _totalPurchases;
            set
            {
                _totalPurchases = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand NewBranchCommand { get; private set; }
        public ICommand RefreshBranchesCommand { get; private set; }
        public ICommand ProfileSettingsCommand { get; private set; }

        private void ExecuteNewBranch(object parameter)
        {
            // TODO: Implement new branch creation
            Logger.LogInfo("New branch command executed");
        }

        private void ExecuteRefreshBranches(object parameter)
        {
            LoadDashboardDataAsync();
            Logger.LogInfo("Refresh branches command executed");
        }

        private void ExecuteProfileSettings(object parameter)
        {
            // TODO: Implement profile settings
            Logger.LogInfo("Profile settings command executed");
        }

        #endregion

        #region Data Loading Methods

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private async void LoadDashboardDataAsync()
        {
            try
            {
                IsLoading = true;

                // Load branches using Dapper
                var branches = await _branchRepository.GetAllCompanyDetailsAsync(_currentUser.id);
                Branches = new ObservableCollection<Branch>(branches);

                // Load branch summaries using Dapper
                // Removed: var summaries = await _branchService.GetAllBranchSummariesAsync();

                // Calculate totals from all branches
                // Removed: TotalSales = summaries.Sum(s => s.total_sales);
                // Removed: TotalPurchases = summaries.Sum(s => s.total_purchases);
                // Removed: TotalProfit = summaries.Sum(s => s.total_profit);

                // Select first branch by default
                if (Branches.Count > 0)
                {
                    SelectedBranch = Branches.FirstOrDefault(); // This will trigger LoadBranchDataAsync
                }

                Logger.LogInfo("Dashboard data loaded successfully (branches only)");
            }
            //catch (Exception ex)
            //{
            //    Logger.LogError("Error loading dashboard data", ex);
            //    MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            finally
            {
                IsLoading = false;
            }
        }

        private async void LoadBranchDataAsync(Branch branch)
        {
            if (branch == null) return;

            try
            {
                IsLoading = true;


                var summary = await _branchRepository.GetBranchSummaryAsync(branch.id);

                //// Update UI with branch data
                ////TotalSales = summary.total_sales;
                //TotalPurchases = summary.total_purchases;
                //TotalProfit = summary.total_profit;

                Logger.LogInfo($"Loaded data for branch: {branch.shop_name}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error loading data for branch {branch.shop_name}", ex);
                MessageBox.Show($"Error loading branch data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // No fallback methods - using only database data

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Simple relay command implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
