namespace EnigmaVault.Secret.Domain.ValueObjects.Password
{
    public readonly record struct CryptoVersion
    { 
       public int Value { get; }

        private CryptoVersion(int value)
        {
            Value = value;
        }

        public static CryptoVersion Create(int value)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);

            return new CryptoVersion(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(CryptoVersion value) => value.ToString();
        public static implicit operator int(CryptoVersion value) => value.Value;
    }
}