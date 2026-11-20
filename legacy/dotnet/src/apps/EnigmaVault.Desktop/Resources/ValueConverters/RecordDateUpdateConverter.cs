using System.Globalization;
using System.Windows.Data;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    internal class RecordDateUpdateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                var ruCulture = new CultureInfo("ru-RU");

                string monthInGenitiveCase = ruCulture.DateTimeFormat.MonthGenitiveNames[date.Month - 1];

                return $"{date.Day} {monthInGenitiveCase} {date.Year} {date.ToString("HH:mm")}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}