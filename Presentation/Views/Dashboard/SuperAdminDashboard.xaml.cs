using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.ViewModels.Dashboard;
using Terret_Billing.Application.Logging;
using Terret_Billing.Presentation.Controllers;

namespace Terret_Billing.Presentation.Views.Dashboard
{
    public partial class SuperAdminDashboard : Window
    {
        private User _currentUser;
        private readonly SuperAdminDashboardRouting _routing;

        public SuperAdminDashboard(User user )
        {
            InitializeComponent();
            _currentUser = user;
            _routing = new SuperAdminDashboardRouting(_currentUser, this);
            txtUserName.Text = _currentUser.UserName;
            DataContext = new SuperAdminDashboardViewModel(_currentUser);
            this.Width = SystemParameters.PrimaryScreenWidth;
            
            SettingsButton.Click += (s, e) => SettingsButton.ContextMenu.IsOpen = true;
            this.MouseDown += (s, e) => UserProfilePopup.IsOpen = false;
            
            Logger.LogInfo($"Super Admin Dashboard accessed by user: {_currentUser?.user_name ?? "Unknown"}");
        }

              private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            _routing.HandleUsersButtonClick();
        }

        private void SetRateButton_Click(object sender, RoutedEventArgs e)
        {
            _routing.HandleSetRateButtonClick();
        }

        private void NewBranchButton_Click(object sender, RoutedEventArgs e)
        {
            _routing.HandleNewBranchButtonClick();
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            UserProfilePopup.IsOpen = !UserProfilePopup.IsOpen;
            Logger.LogInfo("User profile button clicked");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _routing.HandleLogout();
        }
    }
}
