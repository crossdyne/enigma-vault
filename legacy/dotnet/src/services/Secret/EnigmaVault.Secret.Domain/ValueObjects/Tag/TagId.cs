using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Secret.Domain.Exception;

namespace EnigmaVault.Secret.Domain.ValueObjects.Tag
{
    public readonly record struct TagId
    {
        public readonly Guid Value { get; }

        private TagId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static TagId Create(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, () => new EmptyIdentifierException(new Error(ErrorCode.Null, $"Был передан пустой {typeof(Guid)} в качестве идентификатора в {nameof(TagId)}")));

            return new TagId(value);
        }

        public static TagId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(TagId value) => value.Value;
    }
}