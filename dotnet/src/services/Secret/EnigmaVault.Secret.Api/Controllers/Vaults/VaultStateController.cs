using EnigmaVault.Secret.Application.Features.VaultItems.Commands.AddToFavorites;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.Archive;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.EmptyTrash;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.MoveToTrash;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.RemoveFromFavorites;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.RestoreAllFromTrash;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.RestoreFromTrash;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.UnArchive;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.UnArchiveAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnigmaVault.Secret.Api.Controllers.Vaults
{
    public class VaultStateController(IMediator mediator) : VaultControllerBase
    {
        /*--Favorite----------------------------------------------------------------------------------------*/

        [HttpPatch("favorite/{vaultId}")]
        public async Task<IActionResult> AddToFavorites([FromRoute] Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new AddToFavoritesVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("unfavorite/{vaultId}")]
        public async Task<IActionResult> RemoveFromFavorites([FromRoute] Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RemoveFromFavoritesVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        /*--Archive----------------------------------------------------------------------------------------*/

        [HttpPatch("archive/{vaultId}")]
        public async Task<IActionResult> Archive([FromRoute] Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new ArchiveVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("unarchive/{vaultId}")]
        public async Task<IActionResult> UnArchive([FromRoute] Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UnArchiveVaultCommand(vaultId, extractResult.Value.UserId);

            var result = await mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("unarchive/all")]
        public async Task<IActionResult> UnArchiveAll([FromRoute] Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UnArchiveAllVaultCommand(extractResult.Value.UserId);

            var result = await mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        /*--Trash----------------------------------------------------------------------------------------*/

        [HttpPatch("trash/empty")]
        public async Task<IActionResult> EmptyTrash()
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new EmptyVaultsTrashCommand(extractResult.Value.UserId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("trash/{vaultId}")]
        public async Task<IActionResult> MoveToTrash(Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new MoveVaultToTrashCommand(extractResult.Value.UserId, vaultId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        [HttpPatch("restore/{vaultId}")]
        public async Task<IActionResult> RestoreFromTrash(Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RestoreVaultFromTrashCommand(extractResult.Value.UserId, vaultId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("restore/all")]
        public async Task<IActionResult> RestoreAllFromTrash()
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new RestoreAllVaultsFromTrashCommand(extractResult.Value.UserId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }
    }
}