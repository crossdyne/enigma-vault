using EnigmaVault.Secret.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Application.Features.Tags.Commands.Update
{
    public sealed class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator() => Include(new TagNameValidator());
    }
}