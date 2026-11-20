using EnigmaVault.Desktop.Enums;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Services.PageNavigation
{
    public interface IPageFactory
    {
        PagesName PageName { get; }
        Page CreatePage();
    }
}