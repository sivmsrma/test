using System;
using System.IO;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Printing;
using System.Windows.Controls;

namespace Terret_Billing.Utils
{
    public static class BarcodeGenerator
    {
        private const string FontFamily = "calibri";
        private const int FontSize = 7;
        private static readonly System.Drawing.FontStyle FontStyle = System.Drawing.FontStyle.Bold;

        public static BitmapImage GenerateBarcodeImage(string barcodeText, int width = 200, int height = 50)
        {
            try
            {
                var barcodeWriter = new ZXing.BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = 0,
                        PureBarcode = false
                    }
                };
                var pixelData = barcodeWriter.Write(barcodeText);
                var barcodeBitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                var bitmapData = barcodeBitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    barcodeBitmap.UnlockBits(bitmapData);
                }

                // Convert to BitmapImage
                using (var memory = new MemoryStream())
                {
                    barcodeBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating barcode: {ex.Message}", "Barcode Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static void PrintBarcode(string barcodeText, string displayText = null)
        {
            try
            {
                var printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Generate barcode with specified size
                    var bitmapImage = GenerateBarcodeImage(barcodeText, 200, 50);

                    // Create a fixed document for printing
                    var doc = new FixedDocument();
                    var pageContent = new PageContent();
                    var fixedPage = new FixedPage();

                    // Main grid for layout
                    var grid = new System.Windows.Controls.Grid();
                    grid.Margin = new Thickness(10);

                    // Add barcode image
                    var image = new System.Windows.Controls.Image
                    {
                        Source = bitmapImage,
                        Width = 200,
                        Height = 50,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        Margin = new Thickness(0, 5, 0, 5)
                    };

                    // Add barcode text
                    var barcodeTextBlock = new System.Windows.Controls.TextBlock
                    {
                        Text = barcodeText,
                        FontFamily = new System.Windows.Media.FontFamily(FontFamily),
                        FontSize = FontSize,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    // Add display text if provided
                    if (!string.IsNullOrEmpty(displayText))
                    {
                        var displayTextBlock = new System.Windows.Controls.TextBlock
                        {
                            Text = displayText,
                            FontFamily = new System.Windows.Media.FontFamily(FontFamily),
                            FontSize = FontSize,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(0, 5, 0, 0)
                        };
                        grid.Children.Add(displayTextBlock);
                    }


                    // Add elements to grid
                    grid.Children.Add(image);
                    grid.Children.Add(barcodeTextBlock);

                    
                    // Add grid to page
                    fixedPage.Children.Add(grid);

                    
                    // Set page size
                    fixedPage.Width = printDialog.PrintableAreaWidth;
                    fixedPage.Height = printDialog.PrintableAreaHeight;

                    // Add the page to the document
                    var size = new Size(fixedPage.Width, fixedPage.Height);
                    fixedPage.Measure(size);
                    fixedPage.Arrange(new Rect(new Point(), size));
                    fixedPage.UpdateLayout();

                    pageContent.Child = fixedPage;
                    pageContent.Measure(size);
                    pageContent.Arrange(new Rect(new Point(), size));
                    pageContent.UpdateLayout();

                    doc.Pages.Add(pageContent);


                    // Print the document
                    printDialog.PrintDocument(doc.DocumentPaginator, "Barcode Print");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing barcode: {ex.Message}", "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
