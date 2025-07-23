using System;
using System.Windows;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Application.Logging;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Application.Services;
using Terret_Billing.Infrastructure.Data.Repositories;

namespace Terret_Billing.Presentation.Dashboards
{
    public partial class StockEntryPersonDashboard : Window
    {
        private readonly User _currentUser;

        public StockEntryPersonDashboard(User user)
        {
            InitializeComponent();
            _currentUser = user;
            
            // Display user information
            if (UserInfoTextBlock != null)
            {
                // Safely access firm_id using the internal field
                string firmId = (_currentUser.firm_id ?? string.Empty).ToString();
                UserInfoTextBlock.Text = $"Welcome, {_currentUser.user_name} | Firm ID: {firmId}";
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement settings logic */ }
        private void AccountButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement account logic */ }
        private void AddStockButton_Click(object sender, RoutedEventArgs e)
        {
            var databaseHelper = new MySqlDatabaseHelper();
            var taggingRepository = new TaggingRepository(databaseHelper);
            var taggingService = new TaggingService(taggingRepository);
            var taggingWindow = new Terret_Billing.Presentation.Views.Dashboard.StockEntryPersonSubMenu.Tagging(taggingService, _currentUser);
            taggingWindow.ShowDialog();
        }
        private void UpdateStockButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement update stock logic */ }
        private void ReportButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement report logic */ }
        private void PreferencesButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement preferences logic */ }
        private void SystemSettingsButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement system settings logic */ }
        private void BackupButton_Click(object sender, RoutedEventArgs e) { /* TODO: Implement backup logic */ }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            HandleLogout();
        }
        public void HandleLogout()
        {
            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to logout? Any unsaved changes will be lost.",
                    "Confirm Logout",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    Logger.LogInfo($"User {_currentUser?.user_name ?? "Unknown"} logged out");

                    var loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error during logout process: {ex.Message}", ex);
                MessageBox.Show("An error occurred during logout. Please try again or restart the application.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
