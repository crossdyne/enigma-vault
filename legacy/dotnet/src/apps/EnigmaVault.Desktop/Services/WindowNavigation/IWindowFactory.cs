using EnigmaVault.Desktop.Enums;
using System.Windows;

namespace EnigmaVault.Desktop.Services.WindowNavigation
{
    public interface IWindowFactory
    {
        WindowsName WindowName { get; }
        Window CreateWindow();
    }
}