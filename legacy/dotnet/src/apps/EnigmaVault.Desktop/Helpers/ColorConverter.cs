using System.Windows.Media;
using System.Globalization;
using Crossdyne.Toolkit.Validation;

namespace EnigmaVault.Desktop.Helpers
{
    public static class ColorConverter
    {
        public static string RgbToHex(int r, int g, int b)
        {
            Guard.Against.That(r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255, () => new ArgumentOutOfRangeException("Значения RGB должны быть в диапазоне от 0 до 255."));

            string rHex = r.ToString("X2");
            string gHex = g.ToString("X2");
            string bHex = b.ToString("X2");

            return "#" + rHex + gHex + bHex;
        }

        public static Color HexToRgb(string hexColor)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(hexColor), () => new ArgumentException("HEX-строка не может быть пустой."));


            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            Guard.Against.That(hexColor.Length != 6, () => new ArgumentException("HEX-строка должна содержать 6 символов (RRGGBB)."));

            try
            {
                int r = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
                int g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
                int b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);

                _ = byte.TryParse(r.ToString(), out byte r_byte);
                _ = byte.TryParse(g.ToString(), out byte g_byte);
                _ = byte.TryParse(b.ToString(), out byte b_byte);

                return Color.FromRgb(r_byte, g_byte, b_byte);
            }
            catch (FormatException)
            {
                throw new FormatException("HEX-строка содержит недопустимые символы.");
            }
        }
    }
}