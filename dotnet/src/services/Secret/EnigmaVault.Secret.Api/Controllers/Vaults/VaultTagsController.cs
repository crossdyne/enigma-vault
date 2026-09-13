using EnigmaVault.Secret.Application.Features.VaultItems.Commands.AddTag;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.RemoveTag;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.UpdateTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Responses;
using Shared.Contracts.SecretService.Requests;

namespace EnigmaVault.Secret.Api.Controllers.Vaults
{
    public class VaultTagsController(IMediator mediator) : VaultControllerBase
    {
        [HttpPatch("attach/{tagId}/tag/{vaultId}/")]
        public async Task<IActionResult> AddTag([FromRoute] Guid vaultId, [FromRoute] Guid tagId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new AddTagToVaultItemCommand(extractResult.Value.UserId, vaultId, tagId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("remove/{tagId}/tag/{vaultId}")]
        public async Task<IActionResult> RemoveTag([FromRoute] Guid vaultId, [FromRoute] Guid tagId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RemoveTagFromVaultItemCommand(extractResult.Value.UserId, vaultId, tagId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("tags/{vaultId}")]
        public async Task<IActionResult> AttachTags([FromRoute] Guid vaultId, [FromBody] UpdateTagsRequest request)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UpdateTagsCommand(extractResult.Value.UserId, vaultId, [.. request.TagIds.Select(t => Guid.Parse(t))]);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(new DateUpdateResponse(result.Value));
        }
    }
}