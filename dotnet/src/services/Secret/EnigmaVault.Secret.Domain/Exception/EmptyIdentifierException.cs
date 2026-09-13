using Crossdyne.Toolkit.Results;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.Secret.Domain.Exception
{
    public sealed class EmptyIdentifierException(Error error) : DomainException(error);
}