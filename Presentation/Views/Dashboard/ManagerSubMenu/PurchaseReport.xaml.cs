using System;

using System.Windows;
using System.Windows.Controls;
using Terret_Billing.Presentation.ViewModels;

using System.Windows.Input;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using ClosedXML.Excel;
using System.Linq;
using System.Windows.Data;
using Terret_Billing.Presentation.Helpers;
using System.Diagnostics;

namespace Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu
{
    /// <summary>
    /// Interaction logic for PurchaseReport.xaml
    /// </summary>
    public partial class PurchaseReport : Window
    {


        private User _currentUser;
        private IPartyService _partyService;
        private IUserRepository _userRepository;
        private IDatabaseHelper _databaseHelper;
        private IPurchaseReportRepository _purchaseReportRepository;
        private IPartyRepository _partyRepository;

        private readonly Action<string> _log;

        private static PurchaseReport _purchaseViewReportInstance;



        public PurchaseReport(IDatabaseHelper databaseHelper, IPartyRepository partyRepository, User user, IPurchaseReportRepository purchaseReportRepository)
        {
            InitializeComponent();

            _purchaseReportRepository = purchaseReportRepository;
            _databaseHelper = databaseHelper;
            _partyRepository = partyRepository;
            _currentUser = user;

            this.Title = $"Purchase Entery - {_currentUser.user_name},{_currentUser.id}";
            txtUserName.Text = _currentUser.user_name;
            this.Width = SystemParameters.PrimaryScreenWidth;
            txtFirmName.Text = _currentUser.assigned_branch;

            // Debug: Show firm_id value
            MessageBox.Show($"firm_id: {_currentUser.firm_id}");

            // Only use DI-based service resolution for the ViewModel
            var serviceProvider = (IServiceProvider)System.Windows.Application.Current.Properties["ServiceProvider"];
            //var purchaseReportService = serviceProvider.GetRequiredService<Terret_Billing.Application.Interfaces.IPurchaseReportService>();
            var viewModel = new PurchaseReportViewModel(_purchaseReportRepository, _currentUser.firm_id);
            this.DataContext = viewModel;
            //_ = viewModel.LoadDataAsync();
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

        private void OpenAddPartyWindow()
        {
            _log("Opening Add Party Window");
            var addPartyView = new AddParty(_partyService, _currentUser);
            addPartyView.Show(); // or .Show() if not modal
        }



        private void OnExportClick(object sender, RoutedEventArgs e)
        {


            if (JewelleryDataGrid.Items.Count <= 0)
            {
                MessageBox.Show("Nothing to export", "PurchaseReports", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "PurchaseReports"
            };

            if (dialog.ShowDialog() == true)
            {
                var flag = GenericHelpers.ExportToExcel<PurchaseViewReport>(JewelleryDataGrid.ItemsSource.Cast<PurchaseViewReport>(), dialog.FileName);

                if (flag == true)
                {
                    MessageBox.Show("Purchase report export data successfully!", "Purchase Report", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Problem in purchase report data generation!", "Purchase Report", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
