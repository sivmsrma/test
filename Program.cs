using System;
using System.Windows;

namespace Terret_Billing
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Create and run the application
                var app = new Presentation.App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Application startup error: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
} 