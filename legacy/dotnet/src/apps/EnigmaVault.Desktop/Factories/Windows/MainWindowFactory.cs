using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Windows;
using EnigmaVault.Desktop.Views.Windows;
using System.Windows;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Factories.Windows
{
    internal sealed class MainWindowFactory(Func<MainWindowViewModel> vmFactory, IPageNavigation pageNavigation) : IWindowFactory
    {
        private readonly Func<MainWindowViewModel> _vmFactory = vmFactory;
        private readonly IPageNavigation _pageNavigation = pageNavigation;

        public WindowsName WindowName => WindowsName.MainWindow;

        public Window CreateWindow()
        {
            var vm = _vmFactory();

            var window = new MainWindow() { DataContext = vm };

            window.Loaded += (s, e) =>
            {
                var frame = window.FindName("MainFrame") as Frame;
                _pageNavigation.RegisterFrame(FramesName.MainFrame, frame!);
            };

            return window;
        }
    }
}