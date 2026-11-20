using EnigmaVault.Secret.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.Delete
{
    public sealed class DeleteFolderCommandValidator : AbstractValidator<DeleteFolderCommand>
    {
        public DeleteFolderCommandValidator()
        {
            Include(new GuidValidator());
            Include(new MustUserIdValidator());
        }
    }
}