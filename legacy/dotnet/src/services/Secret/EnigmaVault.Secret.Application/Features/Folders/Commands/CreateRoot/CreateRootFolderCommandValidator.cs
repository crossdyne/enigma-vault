using EnigmaVault.Secret.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Application.Features.Folders.Commands.CreateRoot
{
    public sealed class CreateRootFolderCommandValidator : AbstractValidator<CreateRootFolderCommand>
    {
        public CreateRootFolderCommandValidator()
        {
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
            Include(new HexColorValidator());
        }
    }
}