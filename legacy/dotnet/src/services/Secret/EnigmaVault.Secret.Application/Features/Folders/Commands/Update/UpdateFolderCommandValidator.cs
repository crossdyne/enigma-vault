using EnigmaVault.Secret.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.Update
{
    public sealed class UpdateFolderCommandValidator : AbstractValidator<UpdateFolderCommand>
    {
        public UpdateFolderCommandValidator()
        {
            Include(new GuidValidator());
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
        }
    }
}