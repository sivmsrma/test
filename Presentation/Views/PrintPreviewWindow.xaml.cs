using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Terret_Billing.Presentation.ViewModels;

namespace Terret_Billing.Presentation.Views
{
    public partial class PrintPreviewWindow : Window
    {
        private BillingViewModel _billingViewModel;

        public PrintPreviewWindow()
        {
            InitializeComponent();
        }

        public void SetBillData(BillingViewModel viewModel)
        {
            _billingViewModel = viewModel;
            GenerateBillDocument();
        }



        //private void GenerateBillDocument()
        //{
        //    try
        //    {
        //        var doc = new FlowDocument();
        //        doc.PagePadding = new Thickness(40);
        //        doc.ColumnWidth = double.PositiveInfinity;

        //        // Company Header
        //        var header = new Paragraph
        //        {
        //            TextAlignment = TextAlignment.Center,
        //            FontSize = 16,
        //            FontWeight = FontWeights.Bold
        //        };

        //        if (_billingViewModel.Company != null)
        //        {
        //            header.Inlines.Add(new Run($"{_billingViewModel.Company.Name}\n"));
        //            header.Inlines.Add(new Run($"{_billingViewModel.Company.Address}\n"));
        //            header.Inlines.Add(new Run($"Phone: {_billingViewModel.Company.Phone} | Mobile: {_billingViewModel.Company.Mobile}\n"));
        //            header.Inlines.Add(new Run($"GSTIN: {_billingViewModel.Company.GSTIN}\n"));
        //        }
        //        else
        //        {
        //            header.Inlines.Add(new Run("COMPANY NAME\n"));
        //            header.Inlines.Add(new Run("COMPANY ADDRESS\n"));
        //            header.Inlines.Add(new Run("Phone: XXXXXXXX | Mobile: XXXXXXXXXX\n"));
        //            header.Inlines.Add(new Run("GSTIN: XXXXXXXXXXXXXXX\n"));
        //        }

        //        doc.Blocks.Add(header);

        //        // Separator
        //        doc.Blocks.Add(new Paragraph(new Run("=".PadRight(80, '='))));

        //        // Bill Information
        //        var billInfo = new Table();
        //        billInfo.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
        //        billInfo.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
        //        var billGroup = new TableRowGroup();
        //        billInfo.RowGroups.Add(billGroup);

        //        var billRow1 = new TableRow();
        //        billRow1.Cells.Add(new TableCell(new Paragraph(new Run($"Invoice No.: {_billingViewModel.BillNo}"))));
        //        billRow1.Cells.Add(new TableCell(new Paragraph(new Run($"Date: {DateTime.Now:dd-MM-yyyy}"))));
        //        billGroup.Rows.Add(billRow1);

        //        var billRow2 = new TableRow();
        //        billRow2.Cells.Add(new TableCell(new Paragraph(new Run($"Customer: {_billingViewModel.SelectedParty?.Name ?? "N/A"}"))));
        //        billRow2.Cells.Add(new TableCell(new Paragraph(new Run($"Mobile: {_billingViewModel.SelectedParty?.MobileNumber ?? "N/A"}"))));
        //        billGroup.Rows.Add(billRow2);

        //        if (_billingViewModel.SelectedParty != null && !string.IsNullOrEmpty(_billingViewModel.SelectedParty.GSTNumber))
        //        {
        //            var billRow3 = new TableRow();
        //            billRow3.Cells.Add(new TableCell(new Paragraph(new Run($"GST: {_billingViewModel.SelectedParty.GSTNumber}"))));
        //            billRow3.Cells.Add(new TableCell(new Paragraph(new Run($"Address: {_billingViewModel.SelectedParty.Address ?? "N/A"}"))));
        //            billGroup.Rows.Add(billRow3);
        //        }

        //        doc.Blocks.Add(billInfo);

        //        // Items Table Header
        //        doc.Blocks.Add(new Paragraph(new Run("\n")));
        //        var itemsHeader = new Paragraph(new Bold(new Run("ITEMS DETAILS")));
        //        itemsHeader.TextAlignment = TextAlignment.Center;
        //        doc.Blocks.Add(itemsHeader);

        //        // Items Table
        //        var itemsTable = new Table();
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(0.5, GridUnitType.Star) }); // No
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) }); // Description
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(0.8, GridUnitType.Star) }); // PCS
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Purity
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Gross Wt
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Net Wt
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Rate
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Amount
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Making
        //        itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // Total

        //        var itemsGroup = new TableRowGroup();
        //        itemsTable.RowGroups.Add(itemsGroup);

        //        // Table Header Row
        //        var headerRow = new TableRow();
        //        string[] headers = { "No.", "Description", "PCS", "Purity", "Gross Wt", "Net Wt", "Rate", "Amount", "Making", "Total" };
        //        foreach (var h in headers)
        //        {
        //            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(h)))));
        //        }
        //        itemsGroup.Rows.Add(headerRow);

        //        // Items Rows
        //        int index = 1;
        //        foreach (var item in _billingViewModel.BillingItems)
        //        {
        //            var row = new TableRow();
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(index.ToString()))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.Item ?? item.Description ?? "N/A"))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.PCS.ToString()))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.Purity ?? "N/A"))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.GrossWt.ToString("N3")))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.NetWt.ToString("N3")))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.Rate.ToString("N2")))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.Amount.ToString("N2")))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.MakingCharge.ToString("N2")))));
        //            row.Cells.Add(new TableCell(new Paragraph(new Run(item.FinalAmount.ToString("N2")))));
        //            itemsGroup.Rows.Add(row);
        //            index++;
        //        }

        //        doc.Blocks.Add(itemsTable);

        //        // Totals Section
        //        doc.Blocks.Add(new Paragraph(new Run("\n")));
        //        var totalsTable = new Table();
        //        totalsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
        //        totalsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
        //        var totalsGroup = new TableRowGroup();
        //        totalsTable.RowGroups.Add(totalsGroup);

        //        var totalRow1 = new TableRow();
        //        totalRow1.Cells.Add(new TableCell(new Paragraph(new Run("Total Amount:"))));
        //        totalRow1.Cells.Add(new TableCell(new Paragraph(new Run($"₹ {_billingViewModel.TotalAmount:N2}"))));
        //        totalsGroup.Rows.Add(totalRow1);

        //        var totalRow2 = new TableRow();
        //        totalRow2.Cells.Add(new TableCell(new Paragraph(new Run("Tax Amount:"))));
        //        totalRow2.Cells.Add(new TableCell(new Paragraph(new Run($"₹ {_billingViewModel.TaxAmount:N2}"))));
        //        totalsGroup.Rows.Add(totalRow2);

        //        var totalRow3 = new TableRow();
        //        totalRow3.Cells.Add(new TableCell(new Paragraph(new Run("Discount:"))));
        //        totalRow3.Cells.Add(new TableCell(new Paragraph(new Run($"₹ {_billingViewModel.DiscountAmount:N2}"))));
        //        totalsGroup.Rows.Add(totalRow3);

        //        var totalRow4 = new TableRow();
        //        totalRow4.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Grand Total:")))));
        //        totalRow4.Cells.Add(new TableCell(new Paragraph(new Bold(new Run($"₹ {_billingViewModel.GrandTotal:N2}")))));
        //        totalsGroup.Rows.Add(totalRow4);

        //        doc.Blocks.Add(totalsTable);

        //        // Declaration
        //        doc.Blocks.Add(new Paragraph(new Run("\n")));
        //        doc.Blocks.Add(new Paragraph(new Bold(new Run("Declaration:"))));
        //        doc.Blocks.Add(new Paragraph(new Run("We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.")));

        //        // Terms and Conditions
        //        doc.Blocks.Add(new Paragraph(new Run("\n")));
        //        doc.Blocks.Add(new Paragraph(new Bold(new Run("Terms & Conditions:"))));
        //        doc.Blocks.Add(new Paragraph(new Run("1. Goods once sold will not be taken back or exchanged.")));
        //        doc.Blocks.Add(new Paragraph(new Run("2. Payment should be made at the time of delivery.")));
        //        doc.Blocks.Add(new Paragraph(new Run("3. All disputes are subject to local jurisdiction only.")));

        //        DocumentViewer.Document = doc;
        //        StatusText.Text = "Bill preview ready";
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusText.Text = $"Error generating preview: {ex.Message}";
        //        MessageBox.Show($"Error generating bill preview: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}




        public void GenerateBillDocument()
        {
            try
            {
                var doc = new FlowDocument
                {
                    PagePadding = new Thickness(30),
                    ColumnWidth = double.PositiveInfinity,
                    FontFamily = new FontFamily("Calibri"),
                    FontSize = 13
                };



                //---Header with Logo and Company Info-- -
                       var headerTable = new Table();
                headerTable.Columns.Add(new TableColumn());
                headerTable.Columns.Add(new TableColumn());
                var headerGroup = new TableRowGroup();
                headerTable.RowGroups.Add(headerGroup);
                var headerRow = new TableRow();

                //// Left logo
                var leftLogo = new Image
                {
                    Source = new BitmapImage(new Uri("file:///C:/Users/Dell/Downloads/logoKaira.jpg")),
                    Width = 120
                };
                headerRow.Cells.Add(new TableCell(new BlockUIContainer(leftLogo)));

                // --- TOP BOX: TAX INVOICE (left) + DATE + INVOICE NO. (right) ---
                var topBoxTable = new Table { CellSpacing = 0, Margin = new Thickness(0) };
                topBoxTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) });
                topBoxTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) });
                var topBoxGroup = new TableRowGroup();
                topBoxTable.RowGroups.Add(topBoxGroup);
                var topBoxRow = new TableRow();
                // Left: TAX INVOICE
                var taxInvoicePara = new Paragraph(new Bold(new Run("TAX INVOICE")))
                {
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Left,
                    Margin = new Thickness(0)
                };
                topBoxRow.Cells.Add(new TableCell(taxInvoicePara)
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1,1,0,1),
                    Padding = new Thickness(0), // Set to zero
                    ColumnSpan = 1
                });
                // Right: Date + Invoice No.
                var dateInvoicePara = new Paragraph();
                dateInvoicePara.Inlines.Add(new Run($"Date: {DateTime.Now:dd-MM-yyyy}   "));
                dateInvoicePara.Inlines.Add(new Run($"Invoice No.: {_billingViewModel.BillNo}"));
                dateInvoicePara.TextAlignment = TextAlignment.Right;
                dateInvoicePara.Margin = new Thickness(0);
                topBoxRow.Cells.Add(new TableCell(dateInvoicePara)
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,1,1,1),
                    Padding = new Thickness(0), // Set to zero
                    ColumnSpan = 1
                });
                topBoxGroup.Rows.Add(topBoxRow);
                doc.Blocks.Add(topBoxTable);

                // --- SECOND BOX: COMPANY (LEFT) + CUSTOMER (RIGHT) ---
                var infoBoxTable = new Table { CellSpacing = 0, Margin = new Thickness(0) };
                infoBoxTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) });
                infoBoxTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) });
                var infoBoxGroup = new TableRowGroup();
                infoBoxTable.RowGroups.Add(infoBoxGroup);
                var infoBoxRow = new TableRow();
                // Left: Company Details (with Place of Supply)
                var companyPara = new Paragraph();
                if (_billingViewModel.Company != null)
                {
                    companyPara.Inlines.Add(new Bold(new Run(_billingViewModel.Company.Name + "\n")));
                    companyPara.Inlines.Add(_billingViewModel.Company.Address + "\n");
                    companyPara.Inlines.Add("Phone: " + _billingViewModel.Company.Phone + " ");
                    companyPara.Inlines.Add("Mobile: +91 " + _billingViewModel.Company.Mobile + "\n");
                    companyPara.Inlines.Add("GSTIN: " + _billingViewModel.Company.GSTIN + "\n");
                    companyPara.Inlines.Add(new Run($"Place of Supply: {_billingViewModel.Company.Address ?? "-"}"));
                }
                else
                {
                    companyPara.Inlines.Add(new Bold(new Run("Company Name\n")));
                    companyPara.Inlines.Add("Company Address\n");
                    companyPara.Inlines.Add("Phone: XXXXXXXX Mobile: XXXXXXXXXX\n");
                    companyPara.Inlines.Add("GSTIN: XXXXXXXXXXXXXXX\n");
                    companyPara.Inlines.Add(new Run("Place of Supply: -"));
                }
                infoBoxRow.Cells.Add(new TableCell(companyPara)
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1,1,0,1),
                    Padding = new Thickness(0), // Set to zero
                    ColumnSpan = 1
                });
                // Right: Customer Details
                var customerPara = new Paragraph();
                customerPara.Inlines.Add(new Run($"Customer Name: {_billingViewModel.SelectedParty?.Name ?? "-"}\n"));
                customerPara.Inlines.Add(new Run($"Mobile: {_billingViewModel.SelectedParty?.MobileNumber ?? "-"}\n"));
                customerPara.Inlines.Add(new Run($"Address: {_billingViewModel.SelectedParty?.Address ?? "-"}"));
                infoBoxRow.Cells.Add(new TableCell(customerPara)
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,1,1,1),
                    Padding = new Thickness(0), // Set to zero
                    ColumnSpan = 1
                });
                infoBoxGroup.Rows.Add(infoBoxRow);
                doc.Blocks.Add(infoBoxTable);

                // --- Items Table ---
                var itemsTable = new Table { CellSpacing = 0, Margin = new Thickness(0) };
                // Dynamically build headers based on data
                bool showGrossWt = _billingViewModel.BillingItems.Any(i => i.LessWt > 0);
                bool showStoneWt = _billingViewModel.BillingItems.Any(i => i.DiamondCt > 0);
                string[] headers = showStoneWt && showGrossWt
                    ? new[] { "Variant No.", "Product Description", "Purity", "Gross Wt", "Stone Wt", "Net Wt", "Product Value (Rs.)" }
                    : showStoneWt
                        ? new[] { "Variant No.", "Product Description", "Purity", "Stone Wt", "Net Wt", "Product Value (Rs.)" }
                        : showGrossWt
                            ? new[] { "Variant No.", "Product Description", "Purity", "Gross Wt", "Net Wt", "Product Value (Rs.)" }
                            : new[] { "Variant No.", "Product Description", "Purity", "Net Wt", "Product Value (Rs.)" };
                foreach (var _ in headers)
                    itemsTable.Columns.Add(new TableColumn());
                var itemGroup = new TableRowGroup();
                itemsTable.RowGroups.Add(itemGroup);
                // Header row
                var itemHeaderRow = new TableRow();
                foreach (var h in headers)
                    itemHeaderRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(h))))
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0.5),
                        Padding = new Thickness(0), // Set to zero
                        TextAlignment = TextAlignment.Center
                    });
                itemGroup.Rows.Add(itemHeaderRow);
                // Data rows
                int index = 1;
                foreach (var item in _billingViewModel.BillingItems)
                {
                    var row = new TableRow();
                    row.Cells.Add(new TableCell(new Paragraph(new Run(index.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Item ?? item.Description ?? "-"))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Purity ?? "-"))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    if (showGrossWt && item.LessWt > 0)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(item.GrossWt.ToString("N3")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    if (showStoneWt && item.DiamondCt > 0)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(item.DiamondCt.ToString("N3")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.NetWt.ToString("N3")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.FinalAmount.ToString("N2")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0) });
                    itemGroup.Rows.Add(row);
                    index++;
                }
                doc.Blocks.Add(itemsTable);

                // --- SUMMARY + PAYMENT SECTION SIDE BY SIDE ---
                var summaryPaymentTable = new Table { CellSpacing = 0, Margin = new Thickness(0) };
                summaryPaymentTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) }); // Payment
                summaryPaymentTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) }); // Summary
                var summaryPaymentGroup = new TableRowGroup();
                summaryPaymentTable.RowGroups.Add(summaryPaymentGroup);
                var summaryPaymentRow = new TableRow();

                // --- Payment Table (left cell) ---
                var paymentTable = new Table { CellSpacing = 0, Margin = new Thickness(0) };
                paymentTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) }); // Payment Mode
                paymentTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) }); // Date
                paymentTable.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) }); // Amount
                var paymentGroup = new TableRowGroup();
                paymentTable.RowGroups.Add(paymentGroup);
                // Header row
                var paymentHeaderRow = new TableRow();
                paymentHeaderRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Payment Mode")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Center });
                paymentHeaderRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Date")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Center });
                paymentHeaderRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Amount ₹")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Center });
                paymentGroup.Rows.Add(paymentHeaderRow);
                // Data rows from Payments array
                decimal totalPaid = 0;
                foreach (var pay in _billingViewModel.Payments)
                {
                    paymentGroup.Rows.Add(new TableRow {
                        Cells = {
                            new TableCell(new Paragraph(new Run(pay.PaymentMode ?? ""))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Center },
                            new TableCell(new Paragraph(new Run(pay.PaymentDate.ToString("yyyy-MM-dd")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Center },
                            new TableCell(new Paragraph(new Run(pay.Amount.ToString("N2")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Right }
                        }
                    });
                    totalPaid += pay.Amount;
                }
                // Total row
                paymentGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Run("Total (Incl. of all taxes)"))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), ColumnSpan = 2 },
                        new TableCell(new Paragraph(new Run(totalPaid.ToString("N2")))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0.5), Padding = new Thickness(0), TextAlignment = TextAlignment.Right }
                    }
                });

                // --- Summary Table (right cell, single column) ---
                var summaryTable = new Table { CellSpacing = 0, Margin = new Thickness(0) };
                summaryTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
                var summaryGroup = new TableRowGroup();
                summaryTable.RowGroups.Add(summaryGroup);
                // Each row: label + value in one cell
                summaryGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Run($"Total Amount: {_billingViewModel.GrandTotal:N2}")))
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0.5,0,0.5,0.5),
                            Padding = new Thickness(0)
                        }
                    }
                });
                summaryGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Run($"TAX Amount: {_billingViewModel.TaxAmount:N2}")))
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0.5,0,0.5,0.5),
                            Padding = new Thickness(0)
                        }
                    }
                });
                summaryGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Run($"Net Invoice Value: {(_billingViewModel.GrandTotal + _billingViewModel.TaxAmount):N2}")))
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0.5,0,0.5,0.5),
                            Padding = new Thickness(0)
                        }
                    }
                });
                summaryGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Run($"Total Amount to be paid: {(_billingViewModel.GrandTotal + _billingViewModel.TaxAmount):N2}")))
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0.5,0,0.5,0.5),
                            Padding = new Thickness(0)
                        }
                    }
                });
                string amountInWords = ConvertAmountToWords(_billingViewModel.GrandTotal + _billingViewModel.TaxAmount);
                summaryGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Bold(new Run($"Value in Words – Rupees {amountInWords} Only"))))
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0.5,0,0.5,0.5),
                            Padding = new Thickness(0),
                            FontWeight = FontWeights.Bold
                        }
                    }
                });

                // Add payment and summary tables to the main row
                summaryPaymentRow.Cells.Add(new TableCell(paymentTable) { BorderThickness = new Thickness(0), Padding = new Thickness(0) });
                summaryPaymentRow.Cells.Add(new TableCell(summaryTable) { BorderThickness = new Thickness(0), Padding = new Thickness(0) });
                summaryPaymentGroup.Rows.Add(summaryPaymentRow);
                doc.Blocks.Add(summaryPaymentTable);

                // --- Declaration & Signature (unchanged) ---
                doc.Blocks.Add(new Paragraph(new Run("1. Product Value includes Gold value, Product Making charges, Wastage, Vat / Sales tax, and Stone cost (as applicable).")) { FontSize = 11 });
                doc.Blocks.Add(new Paragraph(new Run("2. Received above products in good condition.")) { FontSize = 11 });
                var signTable = new Table { Margin = new Thickness(0) };
                signTable.Columns.Add(new TableColumn());
                signTable.Columns.Add(new TableColumn());
                var signGroup = new TableRowGroup();
                signTable.RowGroups.Add(signGroup);
                var signRow = new TableRow();
                signRow.Cells.Add(new TableCell(new Paragraph(new Run("Customer Name: " + (_billingViewModel.SelectedParty?.Name ?? "")))) { Padding = new Thickness(0) });
                signRow.Cells.Add(new TableCell(new Paragraph(new Run("Authorised Signatory"))) { TextAlignment = TextAlignment.Right, Padding = new Thickness(0) });
                signGroup.Rows.Add(signRow);
                signGroup.Rows.Add(new TableRow {
                    Cells = {
                        new TableCell(new Paragraph(new Run("Customer Signature"))) { Padding = new Thickness(0) },
                        new TableCell(new Paragraph(new Run(""))) { Padding = new Thickness(0) }
                    }
                });
                doc.Blocks.Add(signTable);

                DocumentViewer.Document = doc;
                StatusText.Text = "Bill preview ready";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
                MessageBox.Show(ex.Message);
            }
        }


        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = DocumentViewer.Document;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Bill Print");
                    StatusText.Text = "Printing completed successfully";
                    MessageBox.Show("Bill printed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Print error: {ex.Message}";
                MessageBox.Show($"Error printing bill: {ex.Message}", "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private static string ConvertAmountToWords(decimal amount)
        {
            long rupees = (long)Math.Floor(amount);
            long paise = (long)Math.Round((amount - rupees) * 100);
            string rupeesInWords = NumberToWords(rupees) + " Rupees";
            string paiseInWords = paise > 0 ? " and " + NumberToWords(paise) + " Paise" : string.Empty;
            return rupeesInWords + paiseInWords + " Only";
        }

        private static string NumberToWords(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            var words = new StringBuilder();

            if ((number / 10000000) > 0)
            {
                words.Append(NumberToWords(number / 10000000) + " Crore ");
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words.Append(NumberToWords(number / 100000) + " Lakh ");
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words.Append(NumberToWords(number / 1000) + " Thousand ");
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words.Append(NumberToWords(number / 100) + " Hundred ");
                number %= 100;
            }

            if (number > 0)
            {
                if (words.Length != 0)
                    words.Append("and ");

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
            "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words.Append(unitsMap[number]);
                else
                {
                    words.Append(tensMap[number / 10]);
                    if ((number % 10) > 0)
                        words.Append("-" + unitsMap[number % 10]);
                }
            }

            return words.ToString().Trim();
        }

    }
} 