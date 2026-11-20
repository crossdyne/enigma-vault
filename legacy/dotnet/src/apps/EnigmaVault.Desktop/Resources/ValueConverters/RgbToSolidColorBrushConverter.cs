using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    public class RgbToSolidColorBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => v == DependencyProperty.UnsetValue || v == null))
                return Brushes.White;

            if (values.Length < 3)
                return Brushes.White;

            bool rParsed = byte.TryParse(values[0]?.ToString(), out byte r);
            bool gParsed = byte.TryParse(values[1]?.ToString(), out byte g);
            bool bParsed = byte.TryParse(values[2]?.ToString(), out byte b);

            if (rParsed && gParsed && bParsed)
                return new SolidColorBrush(Color.FromRgb(r, g, b));

            return Brushes.White;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
