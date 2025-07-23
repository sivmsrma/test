using System;
using System;

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Application.Logging;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu;


namespace Terret_Billing.Presentation.Dashboards.ManagerSubMenu
{
    /// <summary>
    /// Interaction logic for PurchaseView.xaml
    /// </summary>
    public partial class PurchaseView : Window
    {
        private readonly Action<string> _log;
        private User _currentUser;
        private readonly IPartyService _partyService;
        private readonly IUserRepository _userRepository;

        public PurchaseView(IPartyService partyService, User user)
        {
            InitializeComponent();
            
            txtBillNo.Focus();
            dtPurchaseDate.SelectedDate = DateTime.Today;
            _log = Logger.LogInfo;
            var databaseHelper = new MySqlDatabaseHelper();
            _userRepository = new UserRepository(databaseHelper);
            this.Closed += (s, e) => {};
            _partyService = partyService;
            _currentUser = user;
            SetUser(_currentUser);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Get the row that was double-clicked
                var grid = sender as DataGrid;
                if (grid == null || grid.SelectedItem == null) return;

                // The ViewModel will handle populating the form fields
                // The button text will automatically change to "Update" due to binding
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting item: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to handle pasting of text to ensure only valid characters
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.]+");
            return !regex.IsMatch(text);
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is Control control)
            {
                control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                e.Handled = true;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.C)
            {
                OpenAddPartyWindow();
                e.Handled = true;
                return;
            }
        }

        private readonly PartyRepository _partyRepository;


        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // This method is no longer needed as we're using MVVM commands
            // The clearing is now handled by the ViewModel's ResetForm method
        }

        //public async void SetUser(User user)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException(nameof(user));

        //        _currentUser = user;
        //        _log($"Manager Dashboard started for user: {_currentUser.user_name},{_currentUser.id}");
        //        this.Title = $"CreateAccount - {_currentUser.user_name},{_currentUser.id},{_currentUser.firm_id}";
        //        txtUserName.Text = _currentUser.user_name;
        //        //string firmName =  await _userRepository.GetFirmNameByIdAsync(_currentUser.firm_id);
        //        //txtFirmName.Text = firmName;

        //    // Load any user-specific data
        //    LoadUserData();
        //}
        public async void SetUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                _currentUser = user;
                _log($"Manager Dashboard started for user: {_currentUser.user_name},{_currentUser.id}");
                this.Title = $"Purchase Entery - {_currentUser.user_name},{_currentUser.id}";
                txtUserName.Text = _currentUser.user_name;
                this.Width = SystemParameters.PrimaryScreenWidth;

                // Get firm name using user_id from the stored procedure
                string firmName = _currentUser.assigned_branch;
                txtFirmName.Text = firmName;


              
                var databaseHelper = new MySqlDatabaseHelper();
                var partyRepo = new PartyRepository(databaseHelper);

                this.DataContext = new Terret_Billing.Presentation.ViewModels.PurchaseViewModel(partyRepo, _currentUser);
                // Load any user-specific data
                LoadUserData();
            }
            catch (Exception ex)
            {
                _log($"Error in SetUser: {ex.Message}");
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadUserData()
        {

        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbPurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // The ViewModel will handle the purity change through binding
                // This method is just a placeholder for the event
                if (e.AddedItems.Count > 0 && e.AddedItems[0] is string selectedPurity)
                {
                    // Optional: Add any UI-specific logic here if needed
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling purity selection: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenAddPartyWindow()
        {
            _log("Opening Add Party Window");
            var addPartyView = new AddParty(_partyService,_currentUser);
            addPartyView.Show(); // or .Show() if not modal
        }


    }
}
