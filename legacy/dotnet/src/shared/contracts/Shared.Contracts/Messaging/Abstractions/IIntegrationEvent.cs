namespace Shared.Contracts.Messaging.Abstractions
{
    public interface IIntegrationEvent 
    {
        Guid IdEvent { get; }
        DateTime OccurredOnUtc { get; }
    }
}