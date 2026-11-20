using EnigmaVault.Desktop.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    internal class ActionOnDataToIsReadOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ActionOnData mode)
            {
                if (mode == ActionOnData.Create)
                    return true;

                return false;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}