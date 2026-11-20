using System.Text.RegularExpressions;

namespace EnigmaVault.Secret.Domain.ValueObjects.Common
{
    public readonly record struct Color
    {
        private static readonly Regex HexColorRegex = new(@"^#([A-Fa-f0-9]{6})$", RegexOptions.Compiled);

        public string Value { get; }

        private Color(string value) => Value = value;

        /// <summary>
        /// Создает экземпляр Color из строки с hex-кодом.
        /// </summary>
        /// <param name="hexCode">Строка в формате #RRGGBB.</param>
        /// <returns>Валидный объект Color.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public static Color FromHex(string hexCode)
        {
            if (string.IsNullOrWhiteSpace(hexCode))
                throw new ArgumentNullException(nameof(hexCode), "Шестнадцатеричный код не может быть пустым.");

            if (!HexColorRegex.IsMatch(hexCode))
                throw new FormatException($"Недопустимый шестнадцатеричный формат цвета: {hexCode}. Ожидаемый формат: #RRGGBB.");

            return new Color(hexCode.ToUpperInvariant());
        }

        public static readonly Color White = new("#FFFFFF");
        public static readonly Color Black = new("#000000");
        public static readonly Color Red = new("#FF0000");
        public static readonly Color Green = new("#00FF00");
        public static readonly Color Blue = new("#0000FF");

        public override string ToString() => Value;

        public static implicit operator string(Color color) => color.Value;
    }
}