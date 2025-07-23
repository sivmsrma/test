using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Terret_Billing.Infrastructure.Helpers;

namespace Terret_Billing.Presentation.ExceptionHandling
{
    public static class GlobalExceptionHandler
    {
        public static void Initialize()
        {
            // Handle exceptions in the UI thread
            System.Windows.Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
            
            // Handle exceptions in background threads
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            
            // Handle exceptions in tasks
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }
        
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException("UI Thread Exception", e.Exception);
            e.Handled = true; // Prevent app from crashing
        }
        
        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException("AppDomain Unhandled Exception", e.ExceptionObject as Exception);
        }
        
        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException("Unobserved Task Exception", e.Exception);
            e.SetObserved(); // Prevent app from crashing
        }
        
        private static void HandleException(string source, Exception exception)
        {
            if (exception == null) return;
            
            // Log the exception
            Logger.LogError($"Global Exception Handler caught an exception from {source}", exception);
            
            // Show a user-friendly message
            MessageBox.Show(
                $"An unexpected error occurred: {exception.Message}\n\nThe error has been logged.",
                "Application Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
