using System;
using System.Windows;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Infrastructure.Helpers;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Presentation.Views.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Terret_Billing.Presentation.Dashboards;

namespace Terret_Billing.Presentation
{
    // Partial class implementation for navigation logic
    public partial class LoginWindow : Window
    {
        private readonly IPartyService _partyService;

        public LoginWindow(IPartyService partyService)
        {
            InitializeComponent();
            _partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
        }

        // Method to navigate to the appropriate dashboard based on user type
        internal void NavigateToDashboard(User user)
        {
            try
            {
                if (user == null)
                {
                    Logger.LogWarning("Attempted to navigate to dashboard with null user");
                    return;
                }

                // Close the current window
                this.Hide();
                
                // Get the service provider from Application.Current.Properties
                if (!(System.Windows.Application.Current.Properties["ServiceProvider"] is IServiceProvider serviceProvider))
                {
                    throw new InvalidOperationException("Service provider not found");
                }
                
                Window dashboardWindow = null;
                
                switch (user.user_type?.ToLower())
                {
                    case "stock entry person":
                        dashboardWindow = new Terret_Billing.Presentation.Dashboards.StockEntryPersonDashboard(user);
                        break;
                        
                    case "billing person":
                        dashboardWindow = new BillingPersonDashboard(user,_partyService);
                        break;
                        
                    case "manager":
                        // Use dependency injection to create the ManagerDashboard
                        dashboardWindow = serviceProvider.GetRequiredService<ManagerDashboard>();
                        // Set the user
                        if (dashboardWindow is ManagerDashboard managerDashboard)
                        {
                            managerDashboard.SetUser(user);
                        }
                        break;
                        
                    case "superadmin":
                        dashboardWindow = new SuperAdminDashboard(user);
                        break;
                        
                    default:
                        Logger.LogWarning($"Unknown user type: {user.user_type}");
                        MessageBox.Show($"Unknown user type: {user.user_type}", "Error", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }
                
                if (dashboardWindow != null)
                {
                    Logger.LogInfo($"User {user.user_name} logged in as {user.user_type}");
                    dashboardWindow.Show();
                    this.Close();
                }
                else
                {
                    throw new InvalidOperationException("Failed to create dashboard window");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error navigating to dashboard", ex);
                MessageBox.Show($"Error navigating to dashboard: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Show(); // Show the login window again
            }
        }
    }
}
