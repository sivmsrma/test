using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;  // Added for Keyboard clas
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Infrastructure.Data.Repositories;
using System.Configuration;
using System.Threading.Tasks;
using Terret_Billing.Presentation.ViewModels.Dashboard.StockEntryPersonSubMenu.ItemTagging;
using Terret_Billing.Application.Services;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Application.Logging;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using System.Text.RegularExpressions;
using Terret_Billing.Presentation.ViewModels.StockEntryPersonSubMenu;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using Terret_Billing.Presentation.Helpers;
using System.Collections.Generic;

namespace Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging
{
    public partial class GoldAndSilver : Window
    {
        private User _currentUser;  // Removed readonly to allow updates
        public User CurrentUser => _currentUser;  // Added property to safely access current user
        private GoldAndSilverViewModel _viewModel;
        private string _username;
        private string _firmId;
        private IItemRepository _itemRepository;
        private IGoldAndSilverService _goldAndSilverService;
        private readonly IDatabaseHelper _databaseHelper;
        private List<GoldAndSilverTaggingEntry> _allItemsCache = null;

        public GoldAndSilver(TaggingItem selectedItem = null, string username = null, string firmId = null, User user = null)
        {
            InitializeComponent();
            _username = username;
            _firmId = firmId;
            _currentUser = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null. Please ensure user is properly logged in.");
            _databaseHelper = new MySqlDatabaseHelper();
            UpdateButton.IsEnabled = false;
            try
            {
                string firm_Id = _currentUser?.firm_id?.ToString();

                // Initialize services
                try
                {
                    // Initialize repositories with database helper
                    _itemRepository = new ItemRepository(_databaseHelper);
                    var goldAndSilverRepository = new GoldAndSilverRepository(_databaseHelper);
                    _goldAndSilverService = new GoldAndSilverService(goldAndSilverRepository);

                    // Initialize ViewModel with correct parameters
                    _viewModel = new GoldAndSilverViewModel(_itemRepository, _goldAndSilverService, _currentUser);
                    this.DataContext = _viewModel;

                    // Now it's safe to set UserInfo
                    if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_firmId))
                    {
                        _viewModel.UserInfo = $"User: {_username} | Firm ID: {_firmId}";
                    }

                    // Display user information if available
                    if (_currentUser != null && UserInfoTextBlock != null)
                    {
                        CompanyName.Text = _currentUser.assigned_branch;
                        UserName.Text = _currentUser.user_name;
                    }

                    // Debug: Track Purity property changes
                    _viewModel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(_viewModel.Purity))
                        {
                            System.Diagnostics.Debug.WriteLine($"Purity changed to: {_viewModel.Purity}");
                        }
                    };

                    // Set firm ID if available
                    if (!string.IsNullOrEmpty(_firmId))
                    {
                        _viewModel.FirmId = _firmId;
                        // Set default metal type to "Gold" if not already set
                        if (string.IsNullOrEmpty(_viewModel.SelectedMetalType))
                        {
                            _viewModel.SelectedMetalType = selectedItem.metal_type; 
                        }
                    }

                    // Load items filtered by firm ID
                    _ = _viewModel.LoadItemsAsync();

                    // Set the selected item if provided
                    if (selectedItem != null)
                    {
                        EntryDatePicker.SelectedDate = DateTime.Today;
                        EntryDatePicker.IsEnabled = false;

                        // Ensure consistent case for metal type (e.g., "gold" -> "Gold")
                        string metalType = selectedItem.metal_type?.Trim() ?? string.Empty;
                        if (!string.IsNullOrEmpty(metalType))
                        {
                            metalType = char.ToUpper(metalType[0]) + (metalType.Length > 1 ? metalType.Substring(1).ToLower() : string.Empty);
                        }
                        _viewModel.SelectedMetalType = metalType;

                        if (txtParticular != null) txtParticular.Text = selectedItem.stock_type ?? string.Empty;
                        if (PartyName != null) PartyName.Text = selectedItem.party_name ?? string.Empty;
                        if (InvoiceNo != null) InvoiceNo.Text = selectedItem.invoice_number ?? string.Empty;
                        if (AvailableWt != null) AvailableWt.Text = selectedItem.pending_weight?.ToString() ?? "0";
                        //if (AvailableCarat != null) AvailableCarat.Text = selectedItem.pending_carat?.ToString() ?? "0";
                        if (PurityTextBox != null) PurityTextBox.Text = selectedItem.purity?.ToString() ?? "0";

                        _viewModel.StockId = selectedItem.stock_id;
                        _viewModel.Stock_Type = selectedItem.metal_type;




                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error initializing repositories: {ex.Message}", ex);
                    MessageBox.Show($"Failed to initialize database connection: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error initializing GoldAndSilver window: {ex.Message}", ex);
                MessageBox.Show($"An error occurred while initializing the window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
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
                string metalType = _viewModel?.SelectedMetalType ?? string.Empty;

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
                var addCategoryWindow = new Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.ItemTagging.AddCategoryView(CurrentUser);
                addCategoryWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in AddItem_MouseLeftButtonDown: {ex.Message}", ex);
                MessageBox.Show($"Failed to open Add Item window: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
                    string.IsNullOrEmpty(Size.Text) ||
                    !int.TryParse(Pcs.Text, out int pcsValue) || pcsValue <= 0)
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

                // Convert numeric values with error handling
                if (!decimal.TryParse(Weight.Text, out decimal weight) || weight <= 0)
                {
                    MessageBox.Show("Please enter a valid weight greater than 0.",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (decimal.TryParse(LessWt.Text, out decimal lesswt) && lesswt > 0)
                {
                    if (lesswt >= weight)
                    {
                        MessageBox.Show("Less Weight cannot be greater than or equal to Weight.",
                            "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    lesswt = 0;
                }
                if (!decimal.TryParse(NetWt.Text, out decimal Netwt))
                {
                    Netwt = weight - lesswt;
                }

                // Parse optional numeric fields with default values
                decimal.TryParse(Tunch.Text, out decimal tunchWeight);
                decimal.TryParse(WastePer.Text, out decimal tunchPercentage);

                // Debug information
                System.Diagnostics.Debug.WriteLine($"[SaveButton_Click] Saving entry with FirmId: {_firmId}, " +
                                                 $"Size: {Size.Text}, Pcs: {pcsValue}");

                // Update ViewModel properties
                _viewModel.Size = Size.Text.Trim();
                _viewModel.Pcs = pcsValue;

                // --- FIX: Set Party_Name, Invoice_No, and Purity in ViewModel from UI/Entry before save/fetch ---
                _viewModel.Party_Name = PartyName.Text?.Trim();
                _viewModel.Invoice_No = InvoiceNo.Text?.Trim();
                _viewModel.Purity = PurityTextBox.Text?.Trim();
                // ---------------------------------------------------------------------------------------------

                // Call the ViewModel's SaveEntryAsync with all required parameters
                bool success = await _viewModel.SaveEntryAsync(
                    particular: txtParticular.Text.Trim(),
                    partyName: PartyName.Text.Trim(),
                    invoiceNo: InvoiceNo.Text.Trim(),
                    entryDate: EntryDatePicker.SelectedDate ?? DateTime.Today,
                    purityType: PurityTextBox.Text.Trim(),
                    metalType: _viewModel.SelectedMetalType ?? string.Empty,
                    category: _viewModel.SelectedCategory ?? string.Empty,
                    subCategory: _viewModel.SelectedSubCategory ?? string.Empty,
                    design: _viewModel.SelectedDesign ?? string.Empty,
                    hsnNo: _viewModel.HSN_No ?? string.Empty,
                    barcode: _viewModel.Barcode ?? string.Empty,
                    weight: weight,
                    LessWt: lesswt,
                    NetWt: Netwt,
                    tunchWeight: tunchWeight,
                    tunchType: "Percentage",
                    tunchPercentage: tunchPercentage,
                    description: Comment.Text.Trim(),
                    firmIdStr: _firmId,  // Parameter name updated to match method signature
                    stock_Type: _viewModel.SelectedMetalType,
                    size: _viewModel.Size,
                    pcs: _viewModel.Pcs,
                    CreatedBy: _currentUser.ServerId,
                    stockId: _viewModel.StockId);

                if (success)
                {
                    MessageBox.Show("Entry saved successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await _viewModel.LoadItemsAsync();  // Refresh the data grid
                   await _viewModel.FetchAvailableWeightAsync(); // Ensure UI updates with latest available weight
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
            Size.Text = string.Empty;
            Pcs.Text = string.Empty;
            Weight.Text = string.Empty;
            LessWt.Text = string.Empty;
            NetWt.Text = string.Empty;
            Tunch.Text = string.Empty;
            TunchWt.Text = string.Empty;
            WastePer.Text = string.Empty;
            WasteAmt.Text = string.Empty;
            StoneCt.Text = string.Empty;
            FinalWt.Text = string.Empty;
            Comment.Text = string.Empty;
            _viewModel.ClearEntry();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.LoadItemsAsync();
                _allItemsCache = _viewModel.Items.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to refresh data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadData()
        {
            try
            {
                await _viewModel.LoadItemsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbPurityTypes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { }
        private void CalculateTunchWeight_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }
        private void UpdateButton_Click(object sender, RoutedEventArgs e) { }
        private void DeleteButton_Click(object sender, RoutedEventArgs e) { }
        private void SearchImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) { }
        //private void ItemDetailsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { }

        //private void PrintBarcodeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var selectedItems = _viewModel.Items.Cast<dynamic>().Where(item => item.IsSelected).ToList();
        //        if (selectedItems.Count == 0)
        //        {
        //            MessageBox.Show("Please select at least one item to print barcode.", "No Selection",
        //                MessageBoxButton.OK, MessageBoxImage.Information);
        //            return;
        //        }

        //        // Create a print dialog
        //        var printDialog = new System.Windows.Controls.PrintDialog();
        //        if (printDialog.ShowDialog() == true)
        //        {
        //            // Create a grid to hold multiple barcodes (2 columns)
        //            var grid = new System.Windows.Controls.Grid();
        //            grid.Margin = new Thickness(15);

        //            // Add 2 columns
        //            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition()
        //            {
        //                Width = new GridLength(1, GridUnitType.Star)
        //            });
        //            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition()
        //            {
        //                Width = new GridLength(1.2, GridUnitType.Star)
        //            });

        //            // Add rows (dynamically based on number of items, 1 item per row)
        //            int rowCount = selectedItems.Count;
        //            for (int i = 0; i < rowCount; i++)
        //            {
        //                grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition()
        //                {
        //                    Height = GridLength.Auto,
        //                    MinHeight = 50
        //                });
        //            }


        //            // Add barcodes to grid
        //            for (int i = 0; i < selectedItems.Count; i++)
        //            {
        //                var item = selectedItems[i];
        //                if (string.IsNullOrEmpty(item.Barcode)) continue;

        //                int row = i;  // Each item gets its own row
        //                int col = 0;   // Always start with first column for barcode

        //                // Create barcode image with dimensions for 6.5cm wide label
        //                var barcodeWriter = new ZXing.BarcodeWriterPixelData
        //                {
        //                    Format = ZXing.BarcodeFormat.CODE_128,
        //                    Options = new ZXing.Common.EncodingOptions
        //                    {
        //                        Height = 25,  // Height in pixels
        //                        Width = 170,  // Width in pixels (6.5cm ~ 245px)
        //                        Margin = 1,
        //                        PureBarcode = false
        //                    }
        //                };

        //                var pixelData = barcodeWriter.Write(item.Barcode);
        //                var barcodeBitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //                var bitmapData = barcodeBitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
        //                    System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //                try
        //                {
        //                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
        //                }
        //                finally
        //                {
        //                    barcodeBitmap.UnlockBits(bitmapData);
        //                }

        //                // Convert to BitmapImage
        //                BitmapImage barcodeImage;
        //                using (var memory = new System.IO.MemoryStream())
        //                {
        //                    barcodeBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
        //                    memory.Position = 0;
        //                    barcodeImage = new BitmapImage();
        //                    barcodeImage.BeginInit();
        //                    barcodeImage.StreamSource = memory;
        //                    barcodeImage.CacheOption = BitmapCacheOption.OnLoad;
        //                    barcodeImage.EndInit();
        //                    barcodeImage.Freeze();
        //                }

        //                // Main container for barcode (left side)

        //                // Barcode container
        //                var barcodeContainer = new System.Windows.Controls.StackPanel
        //                {
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    HorizontalAlignment = HorizontalAlignment.Center,
        //                    Margin = new Thickness(90, 0, 2, 0)
        //                };

        //                // Add barcode image
        //                var barcodeImgCtrl = new System.Windows.Controls.Image
        //                {
        //                    Source = barcodeImage,
        //                    HorizontalAlignment = HorizontalAlignment.Center,
        //                    Margin = new Thickness(0, 0, 0, 2)
        //                };

        //                // Add barcode text
        //                var barcodeText = new System.Windows.Controls.TextBlock
        //                {
        //                    Text = item.Barcode,
        //                    TextAlignment = TextAlignment.Center,
        //                    FontWeight = FontWeights.Bold,
        //                    FontSize = 7,  // Reduced from 6 to 5
        //                    FontFamily = new FontFamily("Calibri"),
        //                    Margin = new Thickness(0, 0, 0, 0)
        //                };

        //                barcodeContainer.Children.Add(barcodeImgCtrl);
        //                barcodeContainer.Children.Add(barcodeText);

        //                // Right side - Details panel
        //                var detailsPanel = new System.Windows.Controls.StackPanel
        //                {
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    Margin = new Thickness(10, 0, 0, 0)
        //                };

        //                // Create a container for category and weights
        //                var detailsStack = new System.Windows.Controls.StackPanel
        //                {
        //                    VerticalAlignment = VerticalAlignment.Center
        //                };



        //                // Add category name with 'Purity:' prefix
        //                var categoryText = new System.Windows.Controls.TextBlock
        //                {

        //                    Text = $"{item.Category?.Trim()} {item.Purity?.Trim()}",
        //                    FontWeight = FontWeights.Bold,
        //                    FontSize = 7,  // Reduced from 7 to 6
        //                    TextWrapping = TextWrapping.Wrap,
        //                    TextAlignment = TextAlignment.Left,
        //                    FontFamily = new FontFamily("Calibri"),
        //                    Margin = new Thickness(0, 0, 0, 1)  // Reduced bottom margin
        //                };
        //                detailsStack.Children.Add(categoryText);

        //                // Add weight details in a single row
        //                var weightRow = new System.Windows.Controls.StackPanel
        //                {
        //                    Orientation = System.Windows.Controls.Orientation.Horizontal,
        //                    HorizontalAlignment = HorizontalAlignment.Left,
        //                    Margin = new Thickness(0, 0, 0, 2)
        //                };

        //                // Add weight values with spacing
        //                AddWeightToRow(weightRow, "G.Wt:", item.Gross_Wt.ToString("F3"));
        //                AddWeightToRow(weightRow, "L.Wt:", item.Less_Wt.ToString("F3"));
        //                AddWeightToRow(weightRow, "N.Wt:", item.Net_Wt.ToString("F3"));

        //                detailsStack.Children.Add(weightRow);
        //                detailsPanel.Children.Add(detailsStack);

        //                // Add barcode to first column
        //                System.Windows.Controls.Grid.SetRow(barcodeContainer, row);
        //                System.Windows.Controls.Grid.SetColumn(barcodeContainer, 0);
        //                grid.Children.Add(barcodeContainer);

        //                // Add details to second column
        //                System.Windows.Controls.Grid.SetRow(detailsPanel, row);
        //                System.Windows.Controls.Grid.SetColumn(detailsPanel, 1);
        //                grid.Children.Add(detailsPanel);
        //            }


        //            // Create a document for printing
        //            var doc = new System.Windows.Documents.FlowDocument();
        //            doc.PageWidth = printDialog.PrintableAreaWidth;
        //            doc.PageHeight = printDialog.PrintableAreaHeight;
        //            doc.PagePadding = new Thickness(10);
        //            doc.Blocks.Add(new System.Windows.Documents.BlockUIContainer(grid));

        //            // Print the document
        //            printDialog.PrintDocument(
        //                ((System.Windows.Documents.IDocumentPaginatorSource)doc).DocumentPaginator,
        //                "Barcode Print"
        //            );

        //            if (selectedItems.Count > 0)
        //            {
        //                MessageBox.Show($"Successfully printed {selectedItems.Count} barcode(s).",
        //                    "Print Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError($"Error printing barcodes: {ex.Message}", ex);
        //        MessageBox.Show($"An error occurred while printing barcodes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}





        private void PrintBarcodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = _viewModel.Items.Cast<dynamic>().Where(item => item.IsSelected).ToList();
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

                    var weight = item.Gross_Wt;
                    var lessWeight = item.Less_Wt;
                    var netWeight = item.Net_Wt;

                    PrintBarcodeLabel(item.Barcode, contentTxt, weight, lessWeight, netWeight);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }


        }


        private void PrintBarcodeLabel(dynamic barcode, string contentTxt, dynamic weight, dynamic lessWeight, dynamic netWeight)
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

                if (lessWeight>0)
                {
                    using (Font font = new Font("Arial", 6, System.Drawing.FontStyle.Bold))
                    {
                        g.DrawString("G_wt :", font, System.Drawing.Brushes.Black, new PointF(265, 10));
                        g.DrawString(weight.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 10));


                        g.DrawString("L.wt :", font, System.Drawing.Brushes.Black, new PointF(265, 20));
                        g.DrawString(lessWeight.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 20));


                        g.DrawString("Net.wt:", font, System.Drawing.Brushes.Black, new PointF(265, 30));
                        g.DrawString(netWeight.ToString(), font, System.Drawing.Brushes.Black, new PointF(295, 30));

                    }
                    using (Font font = new Font("Arial", 6, System.Drawing.FontStyle.Bold))
                    {
                        g.DrawString(contentTxt, font, System.Drawing.Brushes.Black, new PointF(265, 40));
                    }
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


        //private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    Graphics g = e.Graphics;

        //    // Fonts
        //    Font fontBold = new Font("Arial", 5, System.Drawing.FontStyle.Bold);
        //    Font fontNormal = new Font("Arial", 4.5f, System.Drawing.FontStyle.Regular);
        //    Font fontBarcode = new Font("Arial", 2f, System.Drawing.FontStyle.Regular);

        //    float pageWidth = e.PageBounds.Width;
        //    float leftMargin = 50f;

        //    float x = pageWidth;  // You can adjust this to move further right/left
        //    float y= 5;

        //    g.DrawString("Metal: Gold", fontBold, System.Drawing.Brushes.Black, x, y);
        //    y += 12;
        //    g.DrawString("Item: Ring", fontNormal, System.Drawing.Brushes.Black, x, y);
        //    y += 12;
        //    g.DrawString("Purity: 22K", fontNormal, System.Drawing.Brushes.Black, x, y);
        //    y += 12;
        //    g.DrawString("Weight: 5.32g", fontNormal, System.Drawing.Brushes.Black, x, y);
        //    y += 12;
        //    g.DrawString("Barcode: 1234567890", fontNormal, System.Drawing.Brushes.Black, x, y);

        //    e.HasMorePages = false;
        //}


        private void OnExportClick(object sender, RoutedEventArgs e)
        {


            if (ItemDetailsDataGrid.Items.Count <= 0)
            {
                MessageBox.Show("Nothing to export", "GoldAndSilverTaggingEntry", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "GoldAndSilverTaggingEntry"
            };

            if (dialog.ShowDialog() == true)
            {
                var flag = GenericHelpers.ExportToExcel<GoldAndSilverTaggingEntry>(ItemDetailsDataGrid.ItemsSource.Cast<GoldAndSilverTaggingEntry>(), dialog.FileName);

                if (flag == true)
                {
                    MessageBox.Show("GoldAndSilverTaggingEntry export data successfully!", "GoldAndSilverTaggingEntry", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Problem in GoldAndSilverTaggingEntry export data generation!", "GoldAndSilverTaggingEntry", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }
        private void ShowAllButton_Click(object sender, RoutedEventArgs e) { }
        private void InStockButton_Click(object sender, RoutedEventArgs e) { }
        private void OutStockButton_Click(object sender, RoutedEventArgs e) { }

        private void AddWeightToRow(System.Windows.Controls.StackPanel panel, string label, string value)
        {
            var labelText = new System.Windows.Controls.TextBlock
            {
                Text = label,
                FontSize = 4,  // Reduced from 6 to 5
                FontWeight = FontWeights.SemiBold,
                FontFamily = new System.Windows.Media.FontFamily("Calibri"),
                Margin = new Thickness(0, 0, 3, 0)  // Reduced right margin
            };

            var valueText = new System.Windows.Controls.TextBlock
            {
                Text = value,
                FontSize = 5,  // Reduced from 6 to 5
                Margin = new Thickness(0, 0, 3, 0)  // Reduced right margin for spacing
            };

            panel.Children.Add(labelText);
            panel.Children.Add(valueText);
        }

        private void AddWeightRow(System.Windows.Controls.StackPanel panel, string label, string value)
        {
            var stackPanel = new System.Windows.Controls.StackPanel
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 1)  // Reduced bottom margin
            };

            var labelText = new System.Windows.Controls.TextBlock
            {
                Text = label,
                FontSize = 4,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new System.Windows.Media.FontFamily("Calibri"),
                Margin = new Thickness(0, 0, 2, 0)
            };

            var valueText = new System.Windows.Controls.TextBlock
            {
                Text = value,
                FontSize = 6,
                Margin = new Thickness(0, 0, 8, 0)  // Right margin for spacing
            };

            stackPanel.Children.Add(labelText);
            stackPanel.Children.Add(valueText);
            panel.Children.Add(stackPanel);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+(\\.[0-9]{0,3})?$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void PurityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                // _viewModel.IsPurityFocused = true;
                // Load purity suggestions when the control gets focus
                // _ = _viewModel.LoadPuritySuggestionsAsync();
            }
        }

        private void PurityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                // _viewModel.IsPurityFocused = false;
            }
        }

        private void PurityTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
        }



        private void ItemDetailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemDetailsDataGrid.SelectedItem is GoldAndSilverTaggingEntry selectedItem)
            {
                Category.SelectedValue = selectedItem.Category;
                SubCategory.SelectedValue = selectedItem.SubCategory;
                txtParticular.Text = selectedItem.Particular;
                Size.Text = selectedItem.SizeVal;
                Design.SelectedValue = selectedItem.Design;
                Pcs.Text = selectedItem.Pcs.ToString();
                Weight.Text = selectedItem.Gross_Wt.ToString("0.###");
                LessWt.Text = selectedItem.Less_Wt.ToString("0.###");
                NetWt.Text = selectedItem.Net_Wt.ToString("0.###");
                PurityTextBox.Text = selectedItem.Purity;
                UpdateButton.IsEnabled = true;
            }
            // Fetch available weight from DB after selection/filter change
            _ = _viewModel.FetchAvailableWeightAsync();
        }

        private void SearchBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel == null) return;
            var searchText = SearchBarcode.Text?.Trim() ?? string.Empty;

            // Cache all items if not already cached
            if (_allItemsCache == null)
            {
                _allItemsCache = _viewModel.Items.ToList();
            }

            if (searchText.Length >= 3)
            {
                var filtered = _allItemsCache.Where(x => !string.IsNullOrEmpty(x.Barcode) && x.Barcode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                _viewModel.Items.Clear();
                foreach (var item in filtered)
                    _viewModel.Items.Add(item);
            }
            else
            {
                // Show all items if less than 3 chars
                _viewModel.Items.Clear();
                foreach (var item in _allItemsCache)
                    _viewModel.Items.Add(item);
            }
        }


    }
}