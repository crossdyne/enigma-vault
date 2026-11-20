using System.Globalization;
using System.Windows.Data;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    public class OnlyIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                if (int.TryParse(text, out int intValue))
                {
                    if (intValue > 255)
                        return "255";
                    if (intValue < 0)
                        return "0";

                    return intValue.ToString();
                }
            }

            return "255";
        }
    }
}