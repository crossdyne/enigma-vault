using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Pages;
using EnigmaVault.Desktop.Views.Pages;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Factories.Pages
{
    internal sealed class SecretPageFactory(Func<SecretPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<SecretPageViewModel> _vmFactory = vmFactory;

        public PagesName PageName => PagesName.Secret;

        public Page CreatePage() => new SecretPage() { DataContext = _vmFactory() };
    }
}