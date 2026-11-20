using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Secret.Domain.Exception;

namespace EnigmaVault.Secret.Domain.ValueObjects.Password
{
    public readonly record struct VaultItemId
    {
        public readonly Guid Value { get; }

        private VaultItemId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static VaultItemId Create(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, () => new EmptyIdentifierException(new Error(ErrorCode.Null, $"Был передан пустой {typeof(Guid)} в качестве идентификатора в {nameof(VaultItemId)}")));

            return new VaultItemId(value);
        }

        public static VaultItemId New() => new(Guid.NewGuid());

        override public string ToString() => Value.ToString();

        public static implicit operator Guid(VaultItemId value) => value.Value;
    }
}