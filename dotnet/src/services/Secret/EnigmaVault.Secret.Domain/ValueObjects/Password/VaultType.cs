namespace EnigmaVault.Secret.Domain.ValueObjects.Password
{
    public readonly record struct VaultType
    { 
       public int Value { get; }

        private VaultType(int value)
        {
            Value = value;
        }

        public static VaultType Create(int value)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);

            return new VaultType(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(VaultType value) => value.ToString();
        public static implicit operator int(VaultType value) => value.Value;
    }
}