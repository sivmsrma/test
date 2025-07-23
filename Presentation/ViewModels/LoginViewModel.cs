using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Application.Services;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Infrastructure.Helpers;
using Terret_Billing.Presentation.Commands;

namespace Terret_Billing.Presentation.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        private string _username;
        private string _logintype = "User"; // default value
        private string _password;
        private bool _isLoading;
        private string _errorMessage;

        public string LoginType
        {
            get => _logintype;
            set => SetProperty(ref _logintype, value);
        }


        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            // In a real application, this would be injected via dependency injection
            _authService = new AuthenticationService();
            LoginCommand = new Terret_Billing.Presentation.Commands.RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !IsLoading;
        }

        private async void ExecuteLogin(object parameter)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                
                // Use the authentication service to authenticate the user
                var user = await _authService.AuthenticateAsync(Username, Password,LoginType);
                
                if (user != null)
                {
                    Logger.LogInfo($"User {user.UserName} authenticated successfully");
                    
                    // Use the partial class method to navigate to the dashboard
                    if (parameter is LoginWindow loginWindow)
                    {
                        loginWindow.NavigateToDashboard(user);
                    }
                }
                else
                {
                    ErrorMessage = "Invalid username or password";
                    Logger.LogWarning($"Failed login attempt for username: {Username}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
                Logger.LogError("Login attempt failed", ex);
                MessageBox.Show($"Login failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }


    }
}
