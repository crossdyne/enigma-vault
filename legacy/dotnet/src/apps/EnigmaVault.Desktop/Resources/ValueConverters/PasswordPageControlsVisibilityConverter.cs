using EnigmaVault.Desktop.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    internal class PasswordPageControlsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserControlsName mode && parameter is UserControlsName targetMode)
                return mode == targetMode ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}