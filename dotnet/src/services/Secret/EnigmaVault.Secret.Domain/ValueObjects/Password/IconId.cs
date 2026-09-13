namespace EnigmaVault.Secret.Domain.ValueObjects.Password
{
    public readonly record struct IconId
    {
        public readonly Guid Value { get; }

        private IconId(Guid value) => Value = value;

        public static IconId Create(Guid value)
        {
            return new IconId(value);
        }

        public static IconId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(IconId value) => value.Value;
    }
}