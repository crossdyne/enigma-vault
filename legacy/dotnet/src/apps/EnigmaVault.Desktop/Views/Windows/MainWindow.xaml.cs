using System.Windows;
using System.Windows.Controls;

namespace EnigmaVault.Desktop.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StateChanged += MainWindowStateChangeRaised;
        }

        private void MainWindowStateChangeRaised(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                var mainWindowBorder = window.FindName("MainWindowBorder") as Border;
                var restoreButton = window.FindName("RestoreButton") as Button;
                var maximizeButton = window.FindName("MaximizeButton") as Button;

                if (window.WindowState == WindowState.Maximized)
                {
                    mainWindowBorder!.BorderThickness = new Thickness(8);
                    restoreButton!.Visibility = Visibility.Visible;
                    maximizeButton!.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mainWindowBorder!.BorderThickness = new Thickness(0);
                    restoreButton!.Visibility = Visibility.Collapsed;
                    maximizeButton!.Visibility = Visibility.Visible;
                }
            }
        }
    }
}