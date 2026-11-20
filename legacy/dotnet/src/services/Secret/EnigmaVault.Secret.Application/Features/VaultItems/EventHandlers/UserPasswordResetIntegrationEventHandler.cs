using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Messaging.Abstractions;
using Shared.Contracts.Messaging.Events;

namespace EnigmaVault.Secret.Application.Features.VaultItems.EventHandlers
{
    public sealed class UserPasswordResetIntegrationEventHandler(
        IVaultItemRepository repository, 
        IUnitOfWork unitOfWork, 
        ILogger<UserPasswordResetIntegrationEventHandler> logger) : IIntegrationEventHandler<UserPasswordResetIntegrationEvent>
    {
        public async Task HandleAsync(UserPasswordResetIntegrationEvent @event, CancellationToken cancellationToken)
        {
            logger.LogInformation("Получение событие сброса пароля для UserId: {UserId}", @event.UserId);

            int countEntitiesDeleted = await repository.RemoveAllAsync(UserId.Create(@event.UserId), @event.OccurredOnUtc);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Обработка событие сброса пароля для UserId: {UserId} выполнена успешно. Было удалено {countEntitiesDeleted} записей с паролями", @event.UserId, countEntitiesDeleted);
        }
    }
}