using Shared.Contracts.Messaging.Abstractions;

namespace Shared.Contracts.Messaging.Events
{
    public sealed record UserAccountDeletedIntegrationEvent(Guid IdEvent, DateTime OccurredOnUtc, Guid UserId) : IIntegrationEvent;
}