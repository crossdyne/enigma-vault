using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.Services.WindowNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Common.Controls;

namespace EnigmaVault.Desktop.ViewModels.Windows
{
    internal sealed partial class MainWindowViewModel(IWindowNavigation windowNavigation, IPageNavigation pageNavigationService) : BaseWindowViewModel(windowNavigation, pageNavigationService)
    {
        private readonly IPageNavigation _pageNavigationService = pageNavigationService;

        public ToolTipController RightToolTipController { get; } = new(Enums.ToolTipPlacement.CenterRight);
        public ToolTipController BottomToolTipController { get; } = new(Enums.ToolTipPlacement.CenterBottom);

        [ObservableProperty]
        public bool _isSidebarOpen;

        [RelayCommand]
        private void ToggoleRightSideMenuOnPages()
        {
            _pageNavigationService.ToggleSidebar(FramesName.MainFrame);
            IsSidebarOpen = _pageNavigationService.IsOpenSidebar(CurrentOpenPage);
        }
    }
}