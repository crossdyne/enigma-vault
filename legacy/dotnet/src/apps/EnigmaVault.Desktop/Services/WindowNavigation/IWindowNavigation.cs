using EnigmaVault.Desktop.Enums;

namespace EnigmaVault.Desktop.Services.WindowNavigation
{
    public interface IWindowNavigation
    {
        void Open(WindowsName name, bool IsOpenDialog = false);
        void Close(WindowsName windowName);
        void TransmittingValue<TData>(WindowsName windowName, TData value, TransmittingParameter parameter = TransmittingParameter.None, bool isActive = false);

        void MinimizeWindow(WindowsName windowName);
        void MaximizeWindow(WindowsName windowName);
        void RestoreWindow(WindowsName windowName);
    }
}