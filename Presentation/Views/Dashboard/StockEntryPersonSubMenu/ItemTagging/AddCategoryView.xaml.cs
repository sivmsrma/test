using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Terret_Billing.Presentation.ViewModels;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Services.Interfaces;
using MySql.Data.MySqlClient;
using Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu;
using Terret_Billing.Presentation.Views.Dashboard;
using Terret_Billing.Presentation.ViewModels.Dashboard.StockEntryPersonSubMenu;
using Dapper;

namespace Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging
{
    public partial class AddCategoryView : Window
    {
        private readonly string _localConnStr;
        private readonly string _serverConnStr;
        private readonly string _firmId;
        private ObservableCollection<HSNListItem> _hsnItems = new();
        private int _currentLength = 8;
        private AddCategoryViewModel _viewModel;

        public AddCategoryView()
        {
            InitializeComponent();
            var config = (IConfiguration)Presentation.App.Services.GetService(typeof(IConfiguration));
            _localConnStr = config.GetConnectionString("BillingDblocal");
            _serverConnStr = config.GetConnectionString("BillingDb");
            var hsnService = (IHSNService)Presentation.App.Services.GetService(typeof(IHSNService));
            var itemInfoService = (IItemInfoService)Presentation.App.Services.GetService(typeof(IItemInfoService));
            _viewModel = new AddCategoryViewModel(hsnService, itemInfoService, _firmId);
            this.DataContext = _viewModel;
            if (string.IsNullOrEmpty(_firmId))
            {
                MessageBox.Show("Firm ID not found for current user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AttachButtonHandlers();
            // Default: 8 digits (load after view is loaded)
            this.Loaded += async (s, e) => await LoadHSNList(8);
        }

        public AddCategoryView(User currentUser)
        {
            InitializeComponent();
            var config = (IConfiguration)Presentation.App.Services.GetService(typeof(IConfiguration));
            _localConnStr = config.GetConnectionString("BillingDbLocal");
            _serverConnStr = config.GetConnectionString("BillingDbServer");
            _firmId = currentUser?.firm_id ?? string.Empty;
            var hsnService = (IHSNService)Presentation.App.Services.GetService(typeof(IHSNService));
            var itemInfoService = (IItemInfoService)Presentation.App.Services.GetService(typeof(IItemInfoService));
            _viewModel = new AddCategoryViewModel(hsnService, itemInfoService, _firmId);
            this.DataContext = _viewModel;
            if (string.IsNullOrEmpty(_firmId))
            {
                MessageBox.Show("Firm ID not found for current user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AttachButtonHandlers();
            this.Loaded += async (s, e) => await LoadHSNList(8);
        }

        private void AttachButtonHandlers()
        {
            foreach (var child in LogicalTreeHelper.GetChildren(this))
            {
                if (child is StackPanel sp)
                {
                    foreach (var btn in sp.Children.OfType<Button>())
                    {
                        if (btn.Content?.ToString()?.Contains("4 Digits") == true)
                            btn.Click += async (s, e) => await LoadHSNList(4);
                        else if (btn.Content?.ToString()?.Contains("6 Digits") == true)
                            btn.Click += async (s, e) => await LoadHSNList(6);
                        else if (btn.Content?.ToString()?.Contains("8 Digits") == true)
                            btn.Click += async (s, e) => await LoadHSNList(8);
                    }
                }
            }
        }

        private async Task LoadHSNList(int codeLength)
        {
            _currentLength = codeLength;
            await _viewModel.LoadHSNListAsync(codeLength);
            CategoriesDataGrid.ItemsSource = _viewModel.HSNItems;
        }

        private async Task<List<HSNListItem>> GetHSNListFromDb(string connStr, int codeLength)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                var result = await conn.QueryAsync<HSNListItem>(
                    "GetHSNListByFirmIdAndLength",
                    new { firmId = _firmId, length = codeLength },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching HSN list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<HSNListItem>();
            }
        }

        private async Task InsertHSNListToLocalDb(List<HSNListItem> items)
        {
            try
            {
                using var conn = new MySqlConnection(_localConnStr);
                foreach (var item in items)
                {
                    await conn.ExecuteAsync(
                        "INSERT INTO hsn_list (firm_id, metal, hsn_code, IsDataPostOnServer) VALUES (@firm_id, @metal, @hsn_code, @IsDataPostOnServer)",
                        new { firm_id = item.firm_id, metal = item.metal, hsn_code = item.hsn_code, IsDataPostOnServer = item.IsDataPostOnServer });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting HSN list to local DB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

     

        private async void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Metal Type validation
            if (string.IsNullOrWhiteSpace(_viewModel.ItemInfo.metal))
            {
                MessageBox.Show("Please select a metal type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Category Name validation
            if (string.IsNullOrWhiteSpace(_viewModel.ItemInfo.category))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Short Name validation
            if (string.IsNullOrWhiteSpace(_viewModel.ItemInfo.short_name))
            {
                MessageBox.Show("Please enter a short name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // HSN row selection validation
            var selectedHSN = CategoriesDataGrid.SelectedItem as HSNListItem;
            if (selectedHSN == null)
            {
                MessageBox.Show("Please select an HSN row from the list.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Debug output for selectedHSN
            System.Diagnostics.Debug.WriteLine($"Selected HSN: hsn_id={selectedHSN.hsn_id}, firm_id={selectedHSN.firm_id}, hsn_code={selectedHSN.hsn_code}");

            // Set HSN-related properties on ItemInfo
            _viewModel.ItemInfo.hsn_id = selectedHSN.hsn_id;
            _viewModel.ItemInfo.hsn_code = selectedHSN.hsn_code;

            await _viewModel.SaveAsync();

            MessageBox.Show($"Category '{_viewModel.ItemInfo.category}' has been saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadHSNList(8);
        }

        // Event handler to force uppercase in Short Name textbox
        private void ShortNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
            int selStart = tb.SelectionStart;
            string upper = tb.Text.ToUpper();
            if (tb.Text != upper)
            {
                tb.Text = upper;
                tb.SelectionStart = selStart;
            }
        }
    }
}