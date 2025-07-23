using System;
using System.Windows;

namespace Terret_Billing.Presentation.Helpers
{
    public static class UIHelper
    {
        public static void ShowMessage(string message, string title = "Information", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(message, title, button, icon);
        }
        
        public static bool Confirm(string message, string title = "Confirmation")
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
