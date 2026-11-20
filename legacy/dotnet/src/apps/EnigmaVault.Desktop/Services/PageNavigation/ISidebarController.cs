namespace EnigmaVault.Desktop.Services.PageNavigation
{
    public interface ISidebarController
    {
        bool IsSidebarOpen { get; set; }
        void ToggleSidebar();
    }
}