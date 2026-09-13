using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.Secret.Domain.ValueObjects.Password
{
    public readonly record struct EncryptedData
    {
        public string Value { get; }

        private EncryptedData(string value) => Value = value;

        /// <exception cref="DomainException"></exception>
        public static EncryptedData Create(string value)
        {
            Guard.Against.That(string.IsNullOrWhiteSpace(value), () => new DomainException(new Error(AppErrors.Validation, "Данные не могут быть null.")));

            return new EncryptedData(value);
        }

        public static implicit operator string(EncryptedData data) => data.Value;
    }
}