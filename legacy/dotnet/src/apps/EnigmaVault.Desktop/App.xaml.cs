using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Ioc;
using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services.Initializers;
using EnigmaVault.Desktop.Services.Secure;
using EnigmaVault.Desktop.Services.WindowNavigation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Windows;

namespace EnigmaVault.Desktop
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("apiconfig.json", optional: false, reloadOnChange: true)
                .AddJsonFile("urls.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.Configure<Urls>(options =>
            {
                options.Assets = configuration["AssetsWebSite"]!;
            });

            services.Configure<JsonSerializerOptions>(options => 
            {
                options.PropertyNameCaseInsensitive = true;
            });

            services.AddWindows();
            services.AddPages();
            services.AddServices();
            services.AddHttpServices(configuration);

            ServiceProvider = services.BuildServiceProvider();

            SetupGlobalAuthenticationHandler();

            var appInitializer = ServiceProvider.GetService<IApplicationInitializer>();

            try
            {
                appInitializer!.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла критическая ошибка при запуске: {ex.Message}");
                Shutdown();
            }

            base.OnStartup(e);
        }

        private void SetupGlobalAuthenticationHandler()
        {
            var authStateService = ServiceProvider.GetRequiredService<IAuthenticationStateService>();
            var windowsNavigationService = ServiceProvider.GetRequiredService<IWindowNavigation>();

            authStateService.AuthenticationRequired += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    windowsNavigationService.Close(WindowsName.MainWindow);
                    windowsNavigationService.Open(WindowsName.AuthenticationWindow);
                });
            };
        }
    }
}