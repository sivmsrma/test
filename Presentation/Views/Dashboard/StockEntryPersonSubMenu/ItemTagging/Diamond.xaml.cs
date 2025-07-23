using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Presentation.Helpers;
using Terret_Billing.Presentation.ViewModels.Dashboard.StockEntryPersonSubMenu.ItemTagging;


namespace Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging
{
    public partial class Diamond : Window, INotifyPropertyChanged
    {
        private readonly DiamondViewModel _viewModel;
        private string _firmId;
        private string _userName;
        private readonly IDatabaseHelper _databaseHelper;
        private User _currentUser;  // Removed readonly to allow updates
        public User CurrentUser => _currentUser;
        private List<DiamondTaggingEntry> _allItemsCache = null;

        public string UserInfo { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
       

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Diamond(TaggingItem selectedItem , string firmId , string userName , string purity , User user )
        {
            InitializeComponent();

            _firmId = firmId;
            _userName = userName;
            _databaseHelper = new MySqlDatabaseHelper();
            _currentUser = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null. Please ensure user is properly logged in.");
            

            // Set user information directly in the TextBlo
            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_firmId))
            {
                UserInfoTextBlock.Text = $"User: {_userName} | Firm ID: {_firmId}";
            }
             if (_currentUser != null && UserInfoTextBlock != null)
                    {
                        CompanyName.Text = _currentUser.assigned_branch;
                        UserName.Text = _currentUser.user_name;
                    }

            // Get connection string from app.config
            //string connectionString = ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;

            // Initialize repositories with connection string
            var itemRepository = new ItemRepository(_databaseHelper);
            var diamondRepository = new DiamondRepository(new MySqlDatabaseHelper());

            // Initialize services with repositories
            IDiamondService diamondService = new DiamondService(diamondRepository);

            // Initialize the ViewModel with required services, firm ID and purity
            _viewModel = new DiamondViewModel(itemRepository, diamondService, _firmId, user, purity);

            // Set the DataContext to the ViewModel for other bindings
            DataContext = _viewModel;

            // Set the Stock_Type before loading data
            if (selectedItem != null && !string.IsNullOrEmpty(selectedItem.stock_type))
            {
                _viewModel.Stock_Type = selectedItem.stock_type;
                _viewModel.StockId = selectedItem.stock_id;
            }

            //if (_currentUser != null && UserInfoTextBlock != null)
            //{
            //    CompanyName.Text = _currentUser.assigned_branch;
            //    UserName.Text = _currentUser.user_name;
            //}


            // Load initial data with firm ID
            _ = _viewModel.LoadCategoriesAsync();
            _ = _viewModel.LoadDiamondEntriesAsync(
                invoiceNo: selectedItem?.invoice_number,
                stockType: selectedItem?.stock_type
            );

            if (selectedItem != null)
            {
                EntryDatePicker.SelectedDate = DateTime.Today;
                EntryDatePicker.IsEnabled = false;

                if (StockType != null && selectedItem.metal_type != null)
                {
                    StockType.Content = selectedItem.metal_type;
                }

                if (txtParticular != null)
                    txtParticular.Text = selectedItem.stock_type;

                if (PartyName != null)
                    PartyName.Text = selectedItem.party_name;

                if (InvoiceNo != null)
                    InvoiceNo.Text = selectedItem.invoice_number;

                if (AvailableWt != null)
                    AvailableWt.Content = selectedItem.pending_weight;

                if (AvailableCarat != null)
                    AvailableCarat.Content = selectedItem.pending_carat;
                if (PurityTextBox != null)
                    PurityTextBox.Text = selectedItem.purity;
            }
        }
       
        public void AddItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                // Get the current user
                var currentUser = CurrentUser;

                // Validate user
                if (currentUser == null)
                {
                    MessageBox.Show("User information is not available. Please log in again.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Extract just the main metal type (Gold, Silver, or Diamond) from the SelectedMetalType
                 string metalType = StockType.Content?.ToString() ?? string.Empty;

                // Check for different variations of metal types
                if (metalType.IndexOf("gold", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    metalType = "Gold";
                }
                else if (metalType.IndexOf("silver", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    metalType = "Silver";
                }
                else if (metalType.IndexOf("diamond", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    metalType = "Diamond";
                }

                // Create and show the AddItem window
                var addItemWindow = new Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.AddItem(metalType, currentUser);
                addItemWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in AddItem_MouseLeftButtonDown: {ex.Message}", ex);
                MessageBox.Show($"Failed to open Add Item window: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DNetWt_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate Firm ID
                if (string.IsNullOrEmpty(_firmId))
                {
                    MessageBox.Show("Firm ID is missing. Please log in again.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate required fields
                if (string.IsNullOrEmpty(txtParticular.Text) ||
                    string.IsNullOrEmpty(PartyName.Text) ||
                    string.IsNullOrEmpty(InvoiceNo.Text) ||
                    string.IsNullOrEmpty(DSize.Text) ||
                    !int.TryParse(DPCs.Text, out int pcsValue) || pcsValue <= 0)
                {
                    MessageBox.Show("Please fill in all required fields with valid values:\n\n" +
                                    "- Particular\n" +
                                    "- Party Name\n" +
                                    "- Invoice No\n" +
                                    "- Size (cannot be empty)\n" +
                                    "- PCS (must be a number greater than 0)",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Parse numeric values
                decimal? grossWt = decimal.TryParse(DGrossWeight.Text, out decimal weight) ? weight : (decimal?)null;
                decimal? diamondCt = decimal.TryParse(DiamondCt.Text, out decimal dct) ? dct : (decimal?)null;
                decimal? netWt = decimal.TryParse(DNetWt.Text, out decimal netwt) ? netwt : (decimal?)null;

                // Validate diamond weight
                if (diamondCt.HasValue && diamondCt.Value > 0 && grossWt.HasValue)
                {
                    if (diamondCt.Value >= grossWt.Value)
                    {
                        MessageBox.Show("Diamond weight cannot be greater than or equal to Gross Weight.",
                            "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Calculate net weight if not provided
                if (!netWt.HasValue && grossWt.HasValue)
                {
                    netWt = grossWt.Value - (diamondCt ?? 0) * 0.2m;
                }

                // Call the ViewModel's SaveDiamondEntry with all required parameters
                bool success = await _viewModel.SaveDiamondEntry(
                    particular: txtParticular.Text.Trim(),
                    partyName: PartyName.Text.Trim(),
                    invoiceNo: InvoiceNo.Text.Trim(),
                    date: EntryDatePicker.SelectedDate,
                    category: _viewModel.SelectedCategory,
                    subCategory: _viewModel.SelectedSubCategory,
                    design: _viewModel.SelectedDesign,
                    purity: PurityTextBox.Text.Trim(),
                    hsnNo: _viewModel.HSN_No,
                    huidNo: DHUID.Text.Trim(),
                    size: DSize.Text.Trim(),
                    pcs: pcsValue,
                    grossWt: grossWt,
                    lessWt: 0,
                    diamondCt: diamondCt,
                    diamondWtGm: decimal.TryParse(DWtGm.Text, out decimal dwtGm) ? dwtGm : (decimal?)null,
                    netWt: netWt,
                    diamondRate: decimal.TryParse(DRate.Text, out decimal drate) ? drate : (decimal?)null,
                    diamondAmt: decimal.TryParse(DAmount.Text, out decimal damount) ? damount : (decimal?)null,
                    netRate: decimal.TryParse(DNetRate.Text, out decimal netRate) ? netRate : (decimal?)null,
                    netAmt: decimal.TryParse(DNetAmt.Text, out decimal netAmt) ? netAmt : (decimal?)null,
                    stoneCt: decimal.TryParse(DStoneCt.Text, out decimal stoneCtValue) ? stoneCtValue : (decimal?)null,
                    stoneAmt: decimal.TryParse(DStoneWt.Text, out decimal stoneAmt) ? stoneAmt : (decimal?)null,
                    //description: DDescription.Text.Trim(),
                    dropDown: DDropDown.SelectedItem?.ToString(),
                    otherCharges: decimal.TryParse(DMakingCharge.Text, out decimal otherCharges) ? otherCharges : (decimal?)null,
                    value: decimal.TryParse(DValue.Text, out decimal value) ? value : (decimal?)null,
                    finalAmount: decimal.TryParse(DFinalAmount.Text, out decimal finalAmount) ? finalAmount : (decimal?)null,
                    grossAmount: decimal.TryParse(DPurchaseAmount.Text, out decimal grossAmount) ? grossAmount : (decimal?)null,
                    //remark: DRemarks.Text.Trim(),
                    barcode: _viewModel.Barcode,
                    comment: DComment.Text.Trim(),
                    isDataPostOnServer: false,
                    createdBy: _currentUser.server_id,
                    stockId: _viewModel.StockId
                );

                if (success)
                {
                    MessageBox.Show("Entry saved successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await _viewModel.LoadDiamondEntriesAsync();
                    ClearButton_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error saving entry: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nDetails: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.LogError($"Error in SaveButton_Click: {ex.Message}", ex);
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear all fields from Size to Comment
            DSize.Text = string.Empty;
            DPCs.Text = string.Empty;
            DGrossWeight.Text = string.Empty;
            DLessWeight.Text = string.Empty;
            DNetWt.Text = string.Empty;
            DiamondCt.Text = string.Empty;
            DWtGm.Text = string.Empty;
            //WastePer.Text = string.Empty;
            //WasteAmt.Text = string.Empty;
            //StoneCt.Text = string.Empty;
            //FinalWt.Text = string.Empty;
            //Comment.Text = string.Empty;
            //_viewModel.ClearEntry();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e) { }
        private void DeleteButton_Click(object sender, RoutedEventArgs e) { }
        private void SearchImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) { }
        private void ItemDetailsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { }

        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e) { }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && ItemDetailsDataGrid != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;

                if (isChecked)
                {
                    // Select all items in the DataGrid
                    ItemDetailsDataGrid.SelectAll();
                }
                else
                {
                    // Clear all selections
                    ItemDetailsDataGrid.UnselectAll();
                }
            }
        }
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Refresh the data
                await _viewModel.LoadDiamondEntriesAsync();
                MessageBox.Show("Data refreshed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to refresh data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear any filters and reload all entries
                await _viewModel.LoadDiamondEntriesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load all entries: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void InStockButton_Click(object sender, RoutedEventArgs e) { }
        private void OutStockButton_Click(object sender, RoutedEventArgs e) { }
        private void PrintBarcodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = _viewModel.DiamondEntries.Cast<dynamic>().Where(item => item.IsSelected).ToList();
                var assigned_branch = _currentUser.assigned_branch?.Trim().Split(' ')[0] ?? string.Empty;
                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Please select at least one item to print barcode.", "No Selection",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var item = selectedItems[i];
                    if (string.IsNullOrEmpty(item.Barcode))
                        continue;

                    var contentTxt = $"{assigned_branch} / {item.Barcode?.Trim()} / {item.Purity?.Trim()}";

                    var weight = item.Gross_wt;
                    var lessWeight = item.Diamond_Ct;
                    var netWeight = item.Net_wt;

                    PrintBarcodeLabel(item.Barcode, contentTxt, weight, lessWeight, netWeight);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }


        }

        private void PrintBarcodeLabel(dynamic barcode, string contentTxt, dynamic weight, dynamic Diact, dynamic netWeight)
        {
            PrintDocument printDoc = new PrintDocument();

            PaperSize paperSize = new PaperSize("Custom", 400, 59);
            printDoc.DefaultPageSettings.PaperSize = paperSize;
            printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Left, Right, Top, Bottom

            printDoc.PrintPage += (sender, e) =>
            {
                Graphics g = e.Graphics;

                // Draw barcode using font (or use any barcode library)
                using (Font font = new Font("Free 3 of 9", 21)) // You need to install "Free 3 of 9" font
                {
                    g.DrawString($"*{barcode}*", font, System.Drawing.Brushes.Black, new PointF(125, 7));
                }

                // Draw text (optional)
                using (Font font = new Font("Arial", 6, System.Drawing.FontStyle.Bold))
                {
                    g.DrawString(contentTxt, font, System.Drawing.Brushes.Black, new PointF(135, 40));
                }

                if (Diact > 0)
                {
                    using (Font font = new Font("Arial", 6, System.Drawing.FontStyle.Bold))
                    {
                        g.DrawString("G_wt :", font, System.Drawing.Brushes.Black, new PointF(265, 10));
                        g.DrawString(weight.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 10));


                        g.DrawString("Dia.ct :", font, System.Drawing.Brushes.Black, new PointF(265, 20));
                        g.DrawString(Diact.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 20));


                        g.DrawString("Net.wt:", font, System.Drawing.Brushes.Black, new PointF(265, 30));
                        g.DrawString(netWeight.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 30));

                    }
                    //using (Font font = new Font("Arial", 6, System.Drawing.FontStyle.Bold))
                    //{
                    //    g.DrawString(contentTxt, font, System.Drawing.Brushes.Black, new PointF(265, 40));
                    //}
                }
                else
                {

                    using (Font font = new Font("Arial", 6, System.Drawing.FontStyle.Bold))
                    {

                        g.DrawString("Net.wt:", font, System.Drawing.Brushes.Black, new PointF(265, 10));
                        g.DrawString(netWeight.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 10));

                    }

                }
            };

            try
            {
                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Print failed: " + ex.Message);
            }
        }
        //private void SearchBarcode_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (_viewModel == null) return;
        //    var searchText = SearchBarcode.Text?.Trim() ?? string.Empty;

        //    // Cache all items if not already cached
        //    if (_allItemsCache == null)
        //    {
        //        _allItemsCache = _viewModel._allItems.ToList();
        //    }

        //    if (searchText.Length >= 3)
        //    {
        //        var filtered = _allItemsCache.Where(x => !string.IsNullOrEmpty(x.Barcode) && x.Barcode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        //        _viewModel._allItems.Clear();
        //        foreach (var item in filtered)
        //            _viewModel._allItems.Add(item);
        //    }
        //    else
        //    {
        //        // Show all items if less than 3 chars
        //        _viewModel._allItems.Clear();
        //        foreach (var item in _allItemsCache)
        //            _viewModel._allItems.Add(item);
        //    }
        //}
        private void OnExportClick(object sender, RoutedEventArgs e)
        {


            if (ItemDetailsDataGrid.Items.Count <= 0)
            {
                MessageBox.Show("Nothing to export", "DiamondTaggingEntry", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "DiamondTaggingEntry"
            };

            if (dialog.ShowDialog() == true)
            {
                var flag = GenericHelpers.ExportToExcel<DiamondTaggingEntry>(ItemDetailsDataGrid.ItemsSource.Cast<DiamondTaggingEntry>(), dialog.FileName);

                if (flag == true)
                {
                    MessageBox.Show("DiamondTaggingEntry export data successfully!", "DiamondTaggingEntry", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Problem in DiamondTaggingEntry  data generation!", "DiamondTaggingEntry", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }
    }
}
