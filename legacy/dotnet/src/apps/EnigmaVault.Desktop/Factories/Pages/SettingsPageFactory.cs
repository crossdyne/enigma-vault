using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Pages;
using EnigmaVault.Desktop.Views.Pages;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Factories.Pages
{
    internal sealed class SettingsPageFactory(Func<SettingsPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<SettingsPageViewModel> _vmFactory = vmFactory;

        public PagesName PageName => PagesName.Setting;

        public Page CreatePage() => new SettingsPage() { DataContext = _vmFactory() };
    }
}