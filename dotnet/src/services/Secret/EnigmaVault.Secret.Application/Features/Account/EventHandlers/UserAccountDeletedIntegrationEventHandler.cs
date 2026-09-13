using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Messaging.Abstractions;
using Shared.Contracts.Messaging.Events;

namespace EnigmaVault.Secret.Application.Features.Account.EventHandlers
{
    public sealed class UserAccountDeletedIntegrationEventHandler(
        IVaultItemRepository vaultItemRepository, 
        ITagRepository tagRepository,
        IFolderRepository folderRepository,
        IUnitOfWork unitOfWork, 
        ILogger<UserAccountDeletedIntegrationEventHandler> logger) : IIntegrationEventHandler<UserAccountDeletedIntegrationEvent>
    {
        public async Task HandleAsync(UserAccountDeletedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            logger.LogInformation("Получение событие удаление учетной записи UserId: {UserId}", @event.UserId);

            int removedVaults = await vaultItemRepository.RemoveAllAsync(UserId.Create(@event.UserId), eventTimeUtc: null);
            int removedTags = await tagRepository.RemoveAllAsync(UserId.Create(@event.UserId));
            int removedFolders = await folderRepository.RemoveAllAsync(UserId.Create(@event.UserId));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Обработка событие удаление учетной записи для UserId: {UserId} выполнена успешно. Было удалено {removedVaults} записей с паролями, {removedTags} записей с тэгами, {removedFolders} записей с папками", 
                @event.UserId, removedVaults, removedTags, removedFolders);
        }
    }
}