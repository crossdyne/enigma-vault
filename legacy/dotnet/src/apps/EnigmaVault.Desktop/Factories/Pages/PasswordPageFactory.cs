using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Pages;
using EnigmaVault.Desktop.Views.Pages;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Factories.Pages
{
    internal sealed class PasswordPageFactory(Func<PasswordPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<PasswordPageViewModel> _vmFactory = vmFactory;

        public PagesName PageName => PagesName.Password;

        public Page CreatePage() => new PasswordPage() { DataContext = _vmFactory() };
    }
}