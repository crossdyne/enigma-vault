using EnigmaVault.Desktop.Enums;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Services.PageNavigation
{
    public interface IPageNavigation
    {
        void Close(PagesName pageName, FramesName frameName);
        void Navigate(PagesName pageName, FramesName frameName);
        void RegisterFrame(FramesName frameName, Frame frame);
        void TransmittingValue<TData>(PagesName pageName, FramesName frameName, TData value, TransmittingParameter typeParameter = TransmittingParameter.None, bool isNavigateAfterTransmitting = true, bool forceOpenPage = true);
        PagesName GetCurrentDisplayedPage(FramesName frameName);
        void ToggleSidebar(FramesName frameName);
        bool IsOpenSidebar(PagesName pagesName);
    }
}