using System;
using System.Windows;

namespace Terret_Billing.Presentation.Helpers
{
    public static class NavigationService
    {
        public static void Navigate(Window from, Window to)
        {
            to.Show();
            from?.Close();
        }
    }
}
