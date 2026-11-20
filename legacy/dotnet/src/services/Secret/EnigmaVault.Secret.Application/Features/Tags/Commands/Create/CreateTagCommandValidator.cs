using EnigmaVault.Secret.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Create
{
    public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            Include(new MustUserIdValidator());
            Include(new TagNameValidator());
        }
    }
}