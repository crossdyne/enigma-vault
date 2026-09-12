using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Folder;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Primitives;

namespace EnigmaVault.Secret.Domain.Models
{
    public sealed class Folder : AggregateRoot<FolderId>
    {
        public UserId UserId { get; private set; }
        public FolderId? ParentFolderId { get; private set; }
        public FolderName FolderName { get; private set; }
        public Color Color { get; private set; }

        private Folder() { }

        private Folder(FolderId id, FolderId? parentFolderId, UserId userId, FolderName folderName, Color color) : base(id)
        {
            UserId = userId;
            FolderName = folderName;
            ParentFolderId = parentFolderId;
            Color = color;
        }

        public static Folder CreateRoot(Guid userId, string folderName, string color)
            => new(FolderId.New(), null, UserId.Create(userId), FolderName.Create(folderName), Color.FromHex(color));

        public static Folder CreateSubfolder(Guid userId, Guid parentFolderId, string folderName, string color)
            => new(FolderId.New(), FolderId.Create(parentFolderId), UserId.Create(userId), FolderName.Create(folderName), Color.FromHex(color));

        public void Move(FolderId? newParentId)
        {
            Guard.Against.That(newParentId == this.Id, () => new DomainException(new Error(AppErrors.Rule, "Папку нельзя перемещать в саму себя.")));

            ParentFolderId = newParentId;
        }

        public void Rename(FolderName folderName)
        {
            if (FolderName != folderName)
                FolderName = folderName;
        }
    }
}