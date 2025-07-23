using System;
using System.Windows;
using System.Windows.Controls;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Presentation.Dashboards.ManagerSubMenu;
using Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu;
using Terret_Billing.Application.Services;
using Terret_Billing.Presentation.Views.Dashboard.BillingSubMenu;
using Terret_Billing.Application.Services.Interfaces;
using System.Windows.Input;


namespace Terret_Billing.Presentation.Views.Dashboard
 
{
    public partial class ManagerDashboard : Window
    {
        private readonly Action<string> _log;

        private User _currentUser;

        private readonly IPartyService _partyService;

        private static Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu.PurchaseReport _purchaseViewReportInstance;


        private readonly IUserRepository _userRepository;

        //private static PurchaseView _purchaseViewInstance;

        //private static AddParty _addPartyInstance;

        //private static Tagging _addTaggingInstance;

        //private static BillEntry _addBillingInstance;

        private readonly IDatabaseHelper _dbhelper;
        private readonly IDatabaseHelper _databaseHelper;
        private readonly IPartyRepository _partyRepository;
        private IPurchaseReportRepository _purchaseReportRepository;


        public ManagerDashboard(IPartyService partyService, IDatabaseHelper databaseHelper, IPurchaseReportRepository purchaseReportRepository)
        {
            try
            {
                InitializeComponent();
                _partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
                _log = Logger.LogInfo;
                _dbhelper = databaseHelper;
                _databaseHelper = databaseHelper;
                _partyRepository = new PartyRepository(databaseHelper);
                _userRepository = new UserRepository(databaseHelper);
                _purchaseReportRepository = purchaseReportRepository;


            }
            catch (Exception ex)
            {
                Logger.LogError("Error initializing ManagerDashboard", ex);
                throw;
            }

         
        }

        public  void SetUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                _currentUser = user;
               this.Title = $"MANAGER DASHBOARD - {_currentUser.user_name},{_currentUser.id}";
                txtUserName.Text = _currentUser.user_name;

                // Get firm name using user_id from the stored procedure
                string firmName = _currentUser.assigned_branch;
                txtFirmName.Text = firmName;

            }
            catch (Exception ex)
            {
                _log($"Error in SetUser: {ex.Message}");
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }        

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _log("Refresh button clicked");
            // Add your refresh logic here
        }
        
        private void OpenPurchaseWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               

                var addPurchaseWindow = new PurchaseView(_partyService, _currentUser);

                addPurchaseWindow.ShowDialog();


            }
            catch (Exception ex)
            {
                Logger.LogError("Error opening PurchaseView", ex);
                MessageBox.Show("Could not open Purchase window. Please try again.", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenPaymentWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var paymentView = new Dashboards.ManagerSubMenu.PaymentView();

                paymentView.Show();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error opening PaymentView", ex);
                MessageBox.Show("Could not open Payment window. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Card_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Example: Open PurchaseReport window
            if (_purchaseViewReportInstance == null)
            {
                _purchaseViewReportInstance = new PurchaseReport(_databaseHelper, _partyRepository, _currentUser, _purchaseReportRepository);

                _purchaseViewReportInstance.Show();

            }
        }


        private void OpenAddPartyWindow_Click(object sender, RoutedEventArgs e)
        {

            var addPartyWindow = new AddParty(_partyService, _currentUser);

            addPartyWindow.ShowDialog();
        }

        private void ItemTagging_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _log("Opening Add Party Window");

               
                
                    
                    var taggingRepository = new TaggingRepository(_dbhelper);
                    var taggingService = new TaggingService(taggingRepository);

                    var addTaggingWindow = new Tagging(taggingService, _currentUser);

                    addTaggingWindow.ShowDialog();

                
               
            }
            catch (Exception ex)
            {
                Logger.LogError("Error opening AddParty", ex);
                MessageBox.Show("Could not open Add Party window. Please try again.", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //{
            //    var taggingWindow = new Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.Tagging();
            //    taggingWindow.ShowDialog();
            //}
        }

       
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                _log($"Menu item clicked: {button.Content}");
                // Handle menu item click
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _log("Logout button clicked");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error during logout", ex);
                MessageBox.Show("Could not complete logout. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DayBookButton_Click(object sender, RoutedEventArgs e)
        {
            _log("Day Book button clicked");
            // Add day book logic here
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            _log("Dashboard button clicked");
            // Add dashboard logic here
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbHelper = new MySqlDatabaseHelper();
            var billRepository = new BillRepository(dbHelper);
            var billEntryWindow = new BillEntry(_partyService, _currentUser, null, null, null, billRepository);
            billEntryWindow.Show();
        }

        private void OpenVoucherWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var voucherWindow = new SenderVoucherNote(_currentUser);
                voucherWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error opening SenderVoucherNote window", ex);
                MessageBox.Show("Could not open Voucher window. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}