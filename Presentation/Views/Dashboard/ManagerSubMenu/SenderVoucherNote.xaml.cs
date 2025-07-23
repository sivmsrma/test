using System;
using System.Collections.Generic;
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
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Infrastructure.Data.Repositories;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Services;

namespace Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu
{
    /// <summary>
    /// Interaction logic for SenderVoucherNote.xaml
    /// </summary>
    public partial class SenderVoucherNote : Window
    {
        private readonly User _currentUser;

        public SenderVoucherNote(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            var voucherNoteRepo = new VoucherNoteRepository();
            var branchRepo = new BranchRepository(new MySqlDatabaseHelper());
            DataContext = new VoucherNoteViewModel(voucherNoteRepo, branchRepo, _currentUser);
        }

        private async void BarcodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is VoucherNoteViewModel vm && vm.LookupVoucherNoteItemCommand != null)
                    await vm.LookupVoucherNoteItemCommand.ExecuteAsync(null);
            }
        }

        private async void BarcodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is VoucherNoteViewModel vm && vm.LookupVoucherNoteItemCommand != null && !string.IsNullOrWhiteSpace(vm.VoucherNote.Barcode))
                await vm.LookupVoucherNoteItemCommand.ExecuteAsync(null);
        }

        private async void MobileTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is VoucherNoteViewModel vm)
                    await vm.FetchCompanyDetailsByMobileAsync();
            }
        }
    }
}