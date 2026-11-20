using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.ViewModels.Base;
using System.Windows;

namespace EnigmaVault.Desktop.Services.WindowNavigation
{
    public sealed class WindowNavigation(IEnumerable<IWindowFactory> windowFactories) : IWindowNavigation
    {
        private readonly Dictionary<WindowsName, Window> _windows = [];
        private readonly Dictionary<WindowsName, IWindowFactory> _windowFactories = windowFactories.ToDictionary(f => f.WindowName, f => f);

        public void Open(WindowsName name, bool isOpenDialog = false)
        {
            if (_windows.TryGetValue(name, out Window? wnd) && wnd is not null)
                (isOpenDialog ? () => { wnd.ShowDialog(); } : (Action)wnd.Show)();
            else
                OpenWindow(name, isOpenDialog);
        }

        private void OpenWindow(WindowsName windowName, bool isOpenDialog = false)
        {
            if (_windowFactories.TryGetValue(windowName, out var factory))
            {
                var window = factory.CreateWindow();

                _windows[windowName] = window;

                window.Closed += (c, e) => _windows.Remove(windowName);

                (isOpenDialog ? () => { window.ShowDialog(); } : (Action)window.Show)();
            }
            else
                throw new Exception($"Такое окно не зарегистрировано {windowName}");
        }

        public void TransmittingValue<TData>(WindowsName windowName, TData value, TransmittingParameter parameter = TransmittingParameter.None, bool isActive = false)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
            {
                if (window.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update<TData>(value, parameter);

                    if (isActive)
                        window.Activate();
                }
            }
        }

        public void Close(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                window.Close();
        }

        public void MinimizeWindow(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.MinimizeWindow(window);
        }

        public void MaximizeWindow(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.MaximizeWindow(window);
        }

        public void RestoreWindow(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.RestoreWindow(window);
        }
    }
}