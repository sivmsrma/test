using System.Windows;
using Terret_Billing.Presentation.ViewModels;
using Terret_Billing.Application.Logging;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Presentation.Dashboards.SuperAdminSubMenu
{
    public partial class NewBranchForm : Window
        
    {
        private readonly User _currentUser;
        public NewBranchForm(User currentUser )
        {
            InitializeComponent();
            _currentUser = currentUser;
            DataContext = new NewBranchFormViewModel(currentUser);
            Logger.LogInfo("NewBranchForm opened");
            this.Width = SystemParameters.PrimaryScreenWidth;
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void MyFirmsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyFirmsButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
