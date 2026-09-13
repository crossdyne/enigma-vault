using FluentValidation;
using System.Text.RegularExpressions;

namespace EnigmaVault.Secret.Application.Features.Folders.Validators
{
    public sealed class HexColorValidator : AbstractValidator<IHasHexColor>
    {
        public HexColorValidator()
        {
            var hexColorRegex = @"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";

            RuleFor(h => h.Color)
                .NotEmpty().WithMessage("Пожалуйста, укажите цвет.")
                .Matches(hexColorRegex, RegexOptions.IgnoreCase)
                .WithMessage("'{PropertyValue}' не является валидным HEX-кодом цвета.");
        }
    }
}