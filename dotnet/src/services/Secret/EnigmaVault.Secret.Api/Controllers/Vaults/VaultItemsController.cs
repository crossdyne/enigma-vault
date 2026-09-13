using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.ChangeIcon;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.Create;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.Delete;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.Update;
using EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetAll;
using EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetById;
using EnigmaVault.Secret.Application.Features.VaultItems.Queries.GetCountRecords;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Responses;
using Shared.Contracts.SecretService.Requests;

namespace EnigmaVault.Secret.Api.Controllers.Vaults
{
    public class VaultItemsController(IMediator mediator) : VaultControllerBase
    {
        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVaultItemRequest request)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new CreateVaultItemCommand(
                     extractResult.Value.UserId,
                     request.VaultType,
                     Guid.Parse(request.IconId),
                     request.EncryptedOverview,
                     request.EncryptedDetails,
                     request.CryptoVersion);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }
        
        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVaultItemRequest request)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UpdateVaultItemCommand(
                extractResult.Value.UserId,
                Guid.Parse(request.VaultItemId),
                Guid.Parse(request.IconId),
                request.EncryptedOverview,
                request.EncryptedDetails,
                request.CryptoVersion);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{vaultId}")]
        public async Task<IActionResult> Delete(Guid vaultId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new DeleteVaultItemCommand(extractResult.Value.UserId, vaultId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        /*--Get----------------------------------------------------------------------------------------*/

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await mediator.Send(new GetAllVaultsQuery(extractResult.Value.UserId));

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetVaultByIdQuery(id, extractResult.Value.UserId);
            var result = await mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        [HttpGet("records/count")]
        public async Task<IActionResult> GetCount()
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetCountVaultsQuery(extractResult.Value.UserId);
            Result<int> result = await mediator.Send(query);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(new PasswordsCountRecordsResponse(result.Value));
        }

        /*--Icon----------------------------------------------------------------------------------------*/

        [HttpPatch("change/{vaultId:guid}/icon/{iconId:guid}")]
        public async Task<IActionResult> ChangeIcon([FromRoute] Guid vaultId, [FromRoute] Guid iconId)
        {
            var extractResult = ExtractCredentials();

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new ChangeIconCommand(extractResult.Value.UserId, vaultId, iconId);
            var result = await mediator.Send(command);

            if (result.IsFailure)
               return BadRequest(result.StringMessage);

            return Ok(new DateUpdateResponse(result.Value));
        }
    }
}