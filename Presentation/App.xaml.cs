using System;
using System.Windows;
using Terret_Billing.Infrastructure.Helpers;
using Terret_Billing.Presentation.ExceptionHandling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Terret_Billing.Infrastructure;
using Terret_Billing.Presentation.Views.Dashboard.ManagerSubMenu;
using Terret_Billing.Presentation.Views.Dashboard;
using Microsoft.Extensions.Logging;

namespace Terret_Billing.Presentation
{

    public partial class App : System.Windows.Application
    {
        private ServiceProvider serviceProvider;
        private IConfiguration configuration;

        public App()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddXmlFile("App.config", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            try
            {
            
                services.AddLogging(configure => {
                    configure.AddDebug();
                    configure.AddConsole();
                });
                
              
                services.AddSingleton<IConfiguration>(configuration);

                services.AddInfrastructure();

                services.AddTransient<ViewModels.AddPartyViewModel>();

  
                services.AddTransient<AddParty>();
                services.AddTransient<ManagerDashboard>();
                services.AddTransient<LoginWindow>();

            }
            catch (Exception ex)
            {
                Logger.LogError("Error configuring services", ex);
                throw;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            try
            {
                System.Windows.Application.Current.Properties["ServiceProvider"] = serviceProvider;
                
                GlobalExceptionHandler.Initialize();

                
                Logger.LogInfo("Application started");
                 


                // Log application startup
                Logger.LogInfo("Application started");

            }
            catch (Exception ex)
            {
                Logger.LogError("Error during application startup", ex);
                MessageBox.Show($"Failed to start the application: {ex.Message}", "Startup Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown(1);
            }
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            Logger.LogInfo("Application exited");
            
            base.OnExit(e);
        }
    }
}
