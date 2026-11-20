using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Secret.Domain.Exception;

namespace EnigmaVault.Secret.Domain.ValueObjects.User
{
    public readonly record struct UserId
    {
        public readonly Guid Value { get; }

        private UserId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static UserId Create(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, () => new EmptyIdentifierException(new Error(ErrorCode.Null, $"Был передан пустой {typeof(Guid)} в качестве идентификатора в {nameof(UserId)}")));

            return new UserId(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(UserId value) => value.Value;
    }
}