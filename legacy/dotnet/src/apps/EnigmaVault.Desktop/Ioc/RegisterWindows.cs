using EnigmaVault.Desktop.Factories.Windows;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterWindows
    {
        public static IServiceCollection AddWindows(this IServiceCollection services)
        {
            services.AddTransient<IWindowFactory, MainWindowFactory>();
            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton<Func<MainWindowViewModel>>(provider => () => provider.GetRequiredService<MainWindowViewModel>());
            
            services.AddTransient<IWindowFactory, AuthenticationWindowFactory>();
            services.AddTransient<AuthenticationWindowViewModel>();
            services.AddSingleton<Func<AuthenticationWindowViewModel>>(provider => () => provider.GetRequiredService<AuthenticationWindowViewModel>());

            return services;
        }
    }
}