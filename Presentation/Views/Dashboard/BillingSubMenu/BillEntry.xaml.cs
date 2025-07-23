using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.ViewModels;
using Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu;
using ZXing.QrCode.Internal;

namespace Terret_Billing.Presentation.Views.Dashboard.BillingSubMenu
{
    public partial class BillEntry : Window
    {
        private readonly IPartyService _partyService;
        private readonly User _currentUser;
        private readonly IPartyRepository _partyRepository;
        private readonly IBillEntryService _billEntryService;
        private readonly IRateService _rateService;
        private readonly IBillRepository _billRepository;
        private bool _isUpdatingSelection;

        public BillEntry(IPartyService partyService, User currentUser, IPartyRepository partyRepository = null,
            IBillEntryService billEntryService = null, IRateService rateService = null, IBillRepository billRepository = null)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;

            _partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            // Initialize dependencies if not provided (for testing or fallback)
            var dbHelper = new MySqlDatabaseHelper();
            _partyRepository = partyRepository ?? new PartyRepository(dbHelper);
            _billEntryService = billEntryService ?? new BillEntryService(new TaggingRepository(dbHelper));
            _rateService = rateService ?? new RateService(dbHelper);
            _billRepository = billRepository ?? new BillRepository(dbHelper);
            
            SalesPersonEntry.Text = _currentUser.user_name;
            FirmNameEntry.Text = _currentUser.assigned_branch;

            DataContext = new BillingViewModel(_partyRepository, _billEntryService, _rateService, _currentUser, _billRepository);
        }

        private async void BarcodeEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            try
            {
                if (DataContext is BillingViewModel viewModel)
                {
                    await viewModel.LookupItemCommand.ExecuteAsync(null);
                    if (sender is TextBox barcodeEntry)
                        barcodeEntry.SelectAll();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BarcodeEntry_KeyDown error: {ex}");
                MessageBox.Show($"Error processing barcode: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            RoundOffEntry.Focus();

        }

        private async void BarcodeEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is BillingViewModel viewModel && !string.IsNullOrWhiteSpace(viewModel.Barcode))
                {
                    await viewModel.LookupItemCommand.ExecuteAsync(null);
                }
                RoundOffEntry.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BarcodeEntry_LostFocus error: {ex}");
                MessageBox.Show($"Error processing barcode: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Cmb_No_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is BillingViewModel viewModel)
                {
                    //await viewModel.LoadPartiesAsync();
                    System.Diagnostics.Debug.WriteLine($"Loaded {viewModel.FilteredParties?.Count ?? 0} parties in Cmb_No_Loaded");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cmb_No_Loaded error: {ex}");
                MessageBox.Show($"Error loading parties: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is ComboBox comboBox)) return;

            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    if (!comboBox.IsDropDownOpen)
                    {
                        comboBox.IsDropDownOpen = true;
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.Down && !comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape && comboBox.IsDropDownOpen)
                {
                    comboBox.IsDropDownOpen = false;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ComboBox_PreviewKeyDown error: {ex}");
                MessageBox.Show($"Error handling ComboBox input: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cmb_No_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingSelection) return;

            try
            {
                _isUpdatingSelection = true;
                if (!(sender is ComboBox comboBox) || comboBox.SelectedItem == null) return;

                if (DataContext is BillingViewModel viewModel)
                {
                    viewModel.SelectedParty = comboBox.SelectedItem as Customer;
                    comboBox.Text = viewModel.SelectedParty?.Name ?? string.Empty;
                    comboBox.IsDropDownOpen = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cmb_No_SelectionChanged error: {ex}");
                MessageBox.Show($"Error updating selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _isUpdatingSelection = false;
            }
        }

        private void Cmb_No_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is ComboBox comboBox)) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(comboBox.Text) && comboBox.SelectedItem == null)
                {
                    comboBox.Text = string.Empty;
                    if (DataContext is BillingViewModel viewModel)
                        viewModel.SelectedParty = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cmb_No_LostFocus error: {ex}");
                MessageBox.Show($"Error handling ComboBox focus: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cmb_No_DropDownOpened(object sender, EventArgs e)
        {
            if (!(sender is ComboBox comboBox)) return;

            try
            {
                if (string.IsNullOrWhiteSpace(comboBox.Text))
                    comboBox.IsDropDownOpen = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cmb_No_DropDownOpened error: {ex}");
                MessageBox.Show($"Error opening ComboBox dropdown: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Input_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt || e.SystemKey != Key.C) return;

            try
            {
                OpenAddPartyWindow();
                e.Handled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Input_PreviewKeyDown error: {ex}");
                MessageBox.Show($"Error opening Add Party window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenAddPartyWindow()
        {
            try
            {
                var addPartyWindow = new AddParty(_partyService, _currentUser);
                if (addPartyWindow.ShowDialog() == true && DataContext is BillingViewModel viewModel)
                {
                    // Reload parties after adding a new one
                    //viewModel.LoadPartiesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OpenAddPartyWindow error: {ex}");
                MessageBox.Show($"Error opening Add Party window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (DataContext is BillingViewModel viewModel)
                //    await viewModel.SaveBillCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveEntry_Click error: {ex}");
                MessageBox.Show($"Error saving bill: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is BillingViewModel viewModel)
                {
                    viewModel.ClearBillCommand.Execute(null);
                    MessageBox.Show("Bill cleared successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteEntry_Click error: {ex}");
                MessageBox.Show($"Error clearing bill: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is BillingViewModel viewModel)
            {
                // Update the binding source
                var binding = (sender as TextBox)?.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();

                if (e.Key == Key.Enter)
                {
                    // Reset Amount and focus on Amount TextBox
                    //viewModel.Amount = 0m;
                    viewModel.RecalculateTotals();
                    addItem.Focus();
                    e.Handled = true; // Optional: prevent default handling
                }
                else if (e.Key == Key.OemPlus || e.Key == Key.Add) // Add key (numpad or regular +)
                {
                    BarcodeEntry.Focus();
                    e.Handled = true;
                }
            }
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var loginWindow = new LoginWindow();
                    loginWindow.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logout_Click error: {ex}");
                MessageBox.Show($"Error logging out: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    
}