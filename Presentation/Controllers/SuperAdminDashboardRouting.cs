using System;
using System.Windows;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Views.Dashboard;
using Terret_Billing.Presentation.Views.Dashboard.SuperAdminSubMenu;
using Terret_Billing.Application.Logging;
using Terret_Billing.Presentation.Dashboards.SuperAdminSubMenu;

namespace Terret_Billing.Presentation.Controllers
{
    public class SuperAdminDashboardRouting
    {
        private readonly User _currentUser;
        private readonly Window _parentWindow;

        public SuperAdminDashboardRouting(User currentUser, Window parentWindow)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser), "Current user cannot be null");
            _parentWindow = parentWindow ?? throw new ArgumentNullException(nameof(parentWindow), "Parent window cannot be null");
        }

        public void HandleUsersButtonClick()
        {
            try
            {
                if (_currentUser == null)
                {
                    throw new InvalidOperationException("Current user is not initialized");
                }

                var addUsersWindow = new AddUsers(_currentUser);
                addUsersWindow.Owner = _parentWindow;
                addUsersWindow.ShowDialog();
                
                Logger.LogInfo($"User management accessed by {_currentUser.user_name}");
            }
            catch (ArgumentNullException ex)
            {
                Logger.LogError("Invalid user or window reference", ex);
                MessageBox.Show("System configuration error. Please contact support.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError("User session error", ex);
                MessageBox.Show("Session error. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error in user management: {ex.Message}", ex);
                MessageBox.Show("An unexpected error occurred while accessing user management. Please try again or contact support.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleSetRateButtonClick()
        {
            try
            {
                if (_parentWindow == null)
                {
                    throw new InvalidOperationException("Parent window is not initialized");
                }

                var setRateWindow = new Terret_Billing.Presentation.Views.Dashboard.SuperAdminSubMenu.SetRate();
                setRateWindow.Owner = _parentWindow;
                setRateWindow.ShowDialog();

                Logger.LogInfo($"Rate management accessed by {_currentUser?.user_name ?? "Unknown"}");
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError("Window initialization error", ex);
                MessageBox.Show("Unable to open rate management window. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error in rate management: {ex.Message}", ex);
                MessageBox.Show("An unexpected error occurred while accessing rate management. Please try again or contact support.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleNewBranchButtonClick()
        {
            try
            {
                if (_parentWindow == null)
                {
                    throw new InvalidOperationException("Parent window is not initialized");
                }
                
                var newBranchWindow = new NewBranchForm(_currentUser);
                newBranchWindow.Owner = _parentWindow;
                newBranchWindow.ShowDialog();
                
                Logger.LogInfo($"Branch management accessed by {_currentUser?.user_name ?? "Unknown"}");
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError("Window initialization error", ex);
                MessageBox.Show("Unable to open branch management window. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error in branch management: {ex.Message}", ex);
                MessageBox.Show("An unexpected error occurred while accessing branch management. Please try again or contact support.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    _parentWindow.Close();
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
