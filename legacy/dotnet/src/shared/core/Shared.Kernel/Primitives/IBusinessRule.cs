namespace Shared.Kernel.Primitives
{
    public interface IBusinessRule
    {
        string Message { get; }
        bool IsBroken();
    }
}