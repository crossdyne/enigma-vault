using Crossdyne.Toolkit.Results;

namespace Shared.Kernel.Exceptions
{
    public class DomainException : Exception
    {
        public Error Error { get; } = null!;

        public DomainException(Error error) : base(error.Message) => this.Error = error;

        public DomainException(Error error, Exception? innerException) : base(error.Message, innerException) => this.Error = error;
    }
}