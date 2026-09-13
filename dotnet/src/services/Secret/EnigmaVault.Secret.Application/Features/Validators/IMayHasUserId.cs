namespace EnigmaVault.Secret.Application.Features.Validators
{
    public interface IMayHasUserId
    {
        public Guid? UserId { get; }
    }
}