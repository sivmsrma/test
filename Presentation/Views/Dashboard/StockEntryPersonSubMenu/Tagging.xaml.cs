using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Dapper;
using System.Linq;
using Terret_Billing.Presentation.ViewModels.StockEntryPersonSubMenu;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging;
using Terret_Billing.Application.Logging;
using System.Configuration;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Presentation.Helpers;

namespace Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu
{
    public partial class Tagging : Window
    {
        private readonly User _currentUser;
        private readonly ITaggingService _taggingService;


        public Tagging(ITaggingService taggingService, User user = null)
        {
            InitializeComponent();
            _currentUser = user;
            _taggingService = taggingService;

            try
            {
                string firmId = _currentUser?.firm_id?.ToString();
                DataContext = new TaggingViewModel(_taggingService, firmId);

                // Display user information if available
                if (_currentUser != null && UserInfoTextBlock != null)
                {
                    UserInfoTextBlock.Text = $"User: {_currentUser.user_name} | Firm ID: {firmId}";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error initializing Tagging window", ex);
                MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }


        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                var dataGrid = sender as DataGrid;
                if (dataGrid?.SelectedItem is TaggingItem selectedItem)
                {
                    // Add debug logging
                    Logger.LogInfo($"Selected item - MetalType: '{selectedItem.metal_type ?? "NULL"}', Type: {selectedItem.GetType().Name}");

                    string metalType = selectedItem.metal_type ?? string.Empty;
                    metalType = metalType.Trim().ToLower();

                    if (metalType.Contains("gold") || metalType.Contains("silver"))
                    {
                        Logger.LogInfo($"Opening GoldAndSilver window for metal type: {selectedItem.metal_type}");
                        var goldAndSilverWindow = new GoldAndSilver(
                            selectedItem,
                            _currentUser?.user_name,
                            _currentUser?.firm_id?.ToString(),
                            _currentUser

                        );
                        goldAndSilverWindow.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in DataGrid_MouseLeftButtonUp: {ex.Message}", ex);
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DiamondTaggingTable_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var dataGrid = sender as DataGrid;
                var selectedItem = dataGrid?.SelectedItem as TaggingItem;
                if (selectedItem != null && !string.IsNullOrWhiteSpace(selectedItem.metal_type))
                {
                    string metal = selectedItem.metal_type.ToLower();

                    if (metal.Contains("diamond"))
                    {
                        if (selectedItem.pending_weight != 0 || selectedItem.pending_carat != 0)
                        {
                            Logger.LogInfo($"Opening Diamond window for metal type: {selectedItem.metal_type}");
                            var diamondWindow = new Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging.Diamond(
                                selectedItem,
                                _currentUser?.firm_id?.ToString(),
                                _currentUser?.user_name,
                                selectedItem.purity,
                                _currentUser
                            );
                            diamondWindow.ShowDialog();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Weight is not available to Tag.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                    }
                    else if (metal.Contains("gold") || metal.Contains("silver"))
                    {
                        if (selectedItem.pending_weight != 0)
                        {
                            Logger.LogInfo($"Opening GoldAndSilver window for metal type: {selectedItem.metal_type}");
                            var goldAndSilverWindow = new GoldAndSilver(
                                selectedItem,
                                _currentUser?.user_name,
                                _currentUser?.firm_id?.ToString(),
                                _currentUser

                            );
                            goldAndSilverWindow.ShowDialog();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Weight is not available to Tag.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }

                // Optionally, handle edit for other types
                var vm = DataContext as TaggingViewModel;
                vm?.EditSelectedItem();
            }
            catch (Exception ex)
            {
                Logger.LogError("DiamondTaggingTable_MouseDoubleClick error", ex);
                MessageBox.Show($"Failed to edit item: {ex.Message}");
            }
        }


        private void OnExportClick(object sender, RoutedEventArgs e)
        {


            if (DiamondTaggingTable.Items.Count <= 0)
            {
                MessageBox.Show("Nothing to export", "TaggingItem", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "TaggingItem"
            };

            if (dialog.ShowDialog() == true)
            {
                var flag = GenericHelpers.ExportToExcel<PurchaseViewReport>(DiamondTaggingTable.ItemsSource.Cast<PurchaseViewReport>(), dialog.FileName);

                if (flag == true)
                {
                    MessageBox.Show("TaggingItem export data successfully!", "TaggingItem", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Problem in TaggingItem data generation!", "TaggingItem", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private void TypeOfStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only update UI selection, do not set ViewModel filter here
        }
        private void MetalType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only update UI selection, do not set ViewModel filter here
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reset all filter UI fields
                if (TypeOfStock != null) TypeOfStock.SelectedIndex = -1;
                if (MetalType != null) MetalType.SelectedIndex = -1;
                if (PartyName != null) PartyName.Text = string.Empty;
                if (InvoiceNumber != null) InvoiceNumber.Text = string.Empty;
                if (TotalWeight != null) TotalWeight.Text = string.Empty;

                // Reset ViewModel filter properties as well
                if (DataContext is TaggingViewModel viewModel)
                {
                    viewModel.SetTypeOfStockFilter(string.Empty);
                    viewModel.SetMetalTypeFilter(string.Empty);
                    viewModel.SetPartyNameFilter(string.Empty);
                    viewModel.SetInvoiceNumberFilter(string.Empty);
                    await viewModel.LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error refreshing data", ex);
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add this method to handle Search button click and apply filters
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is TaggingViewModel vm)
            {
                // Type of Stock
                string typeOfStock = string.Empty;
                if (TypeOfStock.SelectedItem is ComboBoxItem selectedType)
                {
                    var content = selectedType.Content.ToString();
                    if (content.Contains("Purchase"))
                        typeOfStock = "Purchase";
                    else if (content.Contains("Opening"))
                        typeOfStock = "Opening";
                }
                vm.SetTypeOfStockFilter(typeOfStock);

                // Metal Type
                string metalType = string.Empty;
                if (MetalType.SelectedItem is ComboBoxItem selectedMetal)
                {
                    metalType = selectedMetal.Content.ToString();
                }
                vm.SetMetalTypeFilter(metalType);

                // Party Name
                vm.SetPartyNameFilter(PartyName.Text?.Trim() ?? string.Empty);

                // Invoice Number
                vm.SetInvoiceNumberFilter(InvoiceNumber.Text?.Trim() ?? string.Empty);

                // Optionally: If you want to reload data after setting all filters
                await vm.LoadDataAsync();
            }
        }


    }
}