using System;
using System.Windows;

namespace Terret_Billing.Presentation.Dashboards.ManagerSubMenu
{
    /// <summary>
    /// Interaction logic for PaymentView.xaml
    /// </summary>
    public partial class PaymentView : Window
    {
        public PaymentView()
        {
            try
            {
                InitializeComponent();
                DataContext = new Terret_Billing.Presentation.ViewModels.PaymentViewModel();
                this.Width =SystemParameters.PrimaryScreenWidth;

                // Subscribe to the Loaded event
                this.Loaded += PaymentView_Loaded;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error initializing PaymentView: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Consider logging the exception
                throw;
            }
        }

        private void PaymentView_Loaded(object sender, RoutedEventArgs e)
        {
            // Load parties when the view is loaded
            if (DataContext is Terret_Billing.Presentation.ViewModels.PaymentViewModel viewModel)
            {
                viewModel.LoadParties();
            }
        }
    }
}
