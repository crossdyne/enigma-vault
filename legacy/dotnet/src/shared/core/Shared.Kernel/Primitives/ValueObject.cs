using Crossdyne.Toolkit.Results;
using Shared.Kernel.Exceptions;

namespace Shared.Kernel.Primitives
{
    public abstract record ValueObject : IValueObject
    {
        protected static void CheckRule(IBusinessRule rule)
        {
            // if (rule.IsBroken())
            //     throw new DomainException(Error.Rule(rule.Message));
        }
    }
}