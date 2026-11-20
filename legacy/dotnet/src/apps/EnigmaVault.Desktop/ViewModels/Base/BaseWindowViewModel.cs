using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.WindowNavigation;

namespace EnigmaVault.Desktop.ViewModels.Base
{
    public abstract partial class BaseWindowViewModel : BaseViewModel
    {
        private readonly IWindowNavigation? _windowNavigation;
        private readonly IPageNavigation? _pageNavigation;

        public BaseWindowViewModel() { }

        public BaseWindowViewModel(IWindowNavigation windowNavigation, IPageNavigation pageNavigation)
        {
            _windowNavigation = windowNavigation;
            _pageNavigation = pageNavigation;
        }

        [ObservableProperty]
        private string _windowTitle = null!;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenPageCommand))]
        private PagesName _currentOpenPage;

        /*--Команды---------------------------------------------------------------------------------------*/

        [RelayCommand(CanExecute = nameof(CanOpenPage))]
        private void OpenPage(PagesName page)
        {
            _pageNavigation?.Navigate(page, FramesName.MainFrame);
            CurrentOpenPage = _pageNavigation!.GetCurrentDisplayedPage(FramesName.MainFrame);
        }

        private bool CanOpenPage(PagesName pagesName) => CurrentOpenPage != pagesName;

        [RelayCommand]
        private void ShutDownApp() => System.Windows.Application.Current.Shutdown();

        [RelayCommand]
        private void MinimizeWindow(WindowsName windowName) => _windowNavigation!.MinimizeWindow(windowName);

        [RelayCommand]
        private void MaximizeWindow(WindowsName windowName) => _windowNavigation!.MaximizeWindow(windowName);

        [RelayCommand]
        private void RestoreWindow(WindowsName windowName) => _windowNavigation!.RestoreWindow(windowName);

        [RelayCommand]
        private void CloseWindow(WindowsName windowName) => _windowNavigation!.Close(windowName);
    }
}