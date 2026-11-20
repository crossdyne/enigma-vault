using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Windows;
using EnigmaVault.Desktop.Views.Windows;
using System.Windows;

namespace EnigmaVault.Desktop.Factories.Windows
{
    internal sealed class AuthenticationWindowFactory(Func<AuthenticationWindowViewModel> vmFactory) : IWindowFactory
    {
        private readonly Func<AuthenticationWindowViewModel> _vmFactory = vmFactory;

        public WindowsName WindowName => WindowsName.AuthenticationWindow;

        public Window CreateWindow()
            => new AuthenticationWindow() { DataContext = _vmFactory() };
    }
}