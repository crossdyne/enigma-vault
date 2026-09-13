using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Secret.Domain.Exception;

namespace EnigmaVault.Secret.Domain.ValueObjects.Folder
{
    public readonly record struct FolderId
    {
        public readonly Guid Value { get; }

        private FolderId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static FolderId Create(Guid value)
        {
            Guard.Against.That(value == Guid.Empty, () => new EmptyIdentifierException(new Error(ErrorCode.Null, $"Был передан пустой {typeof(Guid)} в качестве идентификатора в {nameof(FolderId)}")));

            return new FolderId(value);
        }

        public static FolderId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(FolderId value) => value.Value;
    }
}