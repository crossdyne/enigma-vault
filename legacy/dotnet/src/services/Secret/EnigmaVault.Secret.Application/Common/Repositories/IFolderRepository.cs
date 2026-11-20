using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.User;

namespace EnigmaVault.Secret.Application.Common.Repositories
{
    public interface IFolderRepository
    {
        Task AddAsync(Folder folder, CancellationToken token);
        Task<Maybe<Folder>> GetAsync(Guid id, Guid UserId, CancellationToken token = default);
        void Remove(Folder folder);
        Task<bool> Exist(string name, Guid userId, Guid? parentFolderId, CancellationToken token);
        Task<int> RemoveAllAsync(UserId userId);
    }
}