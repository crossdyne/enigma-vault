using System.Globalization;
using System.Windows.Data;

namespace EnigmaVault.Desktop.Resources.ValueConverters
{
    public class DateToDaysRemainingConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not DateTime deletedAt)
                return "Не в корзине";

            var timeLeft = deletedAt - DateTime.Now;

            if (timeLeft.TotalDays <= 0)
                return "Менее дня";

            int days = (int)Math.Ceiling(timeLeft.TotalDays);

            return $"{days} {GetDeclension(days)}";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string GetDeclension(int number)
        {
            int lastTwoDigits = Math.Abs(number) % 100;
            int lastDigit = number % 10;

            if (lastTwoDigits > 10 && lastTwoDigits < 20) return "дней";
            if (lastDigit > 1 && lastDigit < 5) return "дня";
            if (lastDigit == 1) return "день";

            return "дней";
        }
    }
}