using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.Secret.Domain.ValueObjects.Folder
{
    public readonly record struct FolderName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 2;

        public readonly string Value { get; }

        private FolderName(string value) => Value = value;

        /// <exception cref="DomainException"></exception>
        public static FolderName Create(string value)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(value), () => new DomainException(new Error(AppErrors.Validation, "Название записи было пустым.")));
            Guard.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new DomainException(new Error(AppErrors.Validation, $"Максимально допустимы диапазон от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new FolderName(value);
        }

        public override string ToString() => Value;

        public static implicit operator string(FolderName value) => value.Value;
    }
}