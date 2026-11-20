using EnigmaVault.Desktop.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    internal class ActionOnDataToPreviewIconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActionOnData mode)
            {
                if (mode == ActionOnData.View)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}