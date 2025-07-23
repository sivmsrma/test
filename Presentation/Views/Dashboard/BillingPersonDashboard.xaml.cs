using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Application.Services;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Views.Dashboard.BillingSubMenu;

namespace Terret_Billing.Presentation.Dashboards
{
    /// <summary>
    /// Interaction logic for BillingPersonDashboard.xaml
    /// </summary>
    
        public partial class BillingPersonDashboard : Window
        {
            private readonly User currentUser;
            private readonly IPartyService partyService;

            public BillingPersonDashboard(User user, IPartyService partyService)
            {
                InitializeComponent();
                this.currentUser = user;
                this.partyService = partyService;
                LoadRecentActivities();
            }

            private void BillEntryButton_Click(object sender, RoutedEventArgs e)
            {
                var billEntryWindow = new BillEntry(partyService, currentUser);
                billEntryWindow.Show();
            }

        private void LoadRecentActivities()
        {
            // Example of binding data to the recent activities list
            List<Activity> activities = new List<Activity>
                {
                    new Activity { Title = "Bill #1234", Description = "Created on " + DateTime.Now.AddDays(-1).ToShortDateString() },
                    new Activity { Title = "Invoice #5678", Description = "Updated on " + DateTime.Now.AddDays(-2).ToShortDateString() },
                    new Activity { Title = "Discount Applied", Description = "5% discount applied on " + DateTime.Now.AddDays(-3).ToShortDateString() }
                };

            RecentActivitiesListView.ItemsSource = activities;
        }



        private void ViewBillsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Bills functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generate Report functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PreferencesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Preferences functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SystemSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("System Settings functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Backup functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Account functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?",
                "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }

    // Activity class for recent activities list
    public class Activity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}