using EnigmaVault.Desktop.Factories.Pages;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace EnigmaVault.Desktop.Ioc
{
    public static class RegisterPages
    {
        public static IServiceCollection AddPages(this IServiceCollection services)
        {
            services.AddTransient<IPageFactory, PasswordPageFactory>();
            services.AddTransient<PasswordPageViewModel>();
            services.AddSingleton<Func<PasswordPageViewModel>>(provider => () => provider.GetRequiredService<PasswordPageViewModel>());

            services.AddTransient<IPageFactory, SecretPageFactory>();
            services.AddTransient<SecretPageViewModel>();
            services.AddSingleton<Func<SecretPageViewModel>>(provider => () => provider.GetRequiredService<SecretPageViewModel>());

            services.AddTransient<IPageFactory, NotePageFactory>();
            services.AddTransient<NotePageViewModel>();
            services.AddSingleton<Func<NotePageViewModel>>(provider => () => provider.GetRequiredService<NotePageViewModel>());

            services.AddTransient<IPageFactory, SettingsPageFactory>();
            services.AddTransient<SettingsPageViewModel>();
            services.AddSingleton<Func<SettingsPageViewModel>>(provider => () => provider.GetRequiredService<SettingsPageViewModel>());

            return services;
        }
    }
}