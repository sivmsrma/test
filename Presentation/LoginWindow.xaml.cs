using System.Windows;
using System.Windows.Input;

namespace Terret_Billing.Presentation
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            // Add key event handlers for Enter key traversal
            UsernameTextBox.KeyDown += UsernameTextBox_KeyDown;
            PasswordTextBox.KeyDown += PasswordTextBox_KeyDown;
        }
        
        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Move focus to password field
                PasswordTextBox.Focus();
                e.Handled = true;
            }
        }
        
        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Trigger login button click
                if (LoginButton.Command != null && LoginButton.Command.CanExecute(LoginButton.CommandParameter))
                {
                    LoginButton.Command.Execute(LoginButton.CommandParameter);
                    e.Handled = true;
                }
            }
        }
    }
}
