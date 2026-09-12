using EnigmaVault.Secret.Application.Features.Folders.Commands.CreateRoot;
using EnigmaVault.Secret.Application.Features.Folders.Commands.CreateSubFolder;
using EnigmaVault.Secret.Application.Features.Folders.Commands.Delete;
using EnigmaVault.Secret.Application.Features.Folders.Commands.Update;
using EnigmaVault.Secret.Application.Features.Folders.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SecretService.Requests;
using EnigmaVault.Secret.Api.Extensions;

namespace EnigmaVault.Secret.Api.Controllers
{
    [ApiController]
    [Route("api/folders")]
    public sealed class FolderController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost("root")]
        [Authorize]
        public async Task<IActionResult> CreateRoot([FromBody] CreateFolderRootRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new CreateRootFolderCommand(extractResult.Value.UserId, request.Name, request.Color));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPost("sub")]
        [Authorize]
        public async Task<IActionResult> CreateSubFolder([FromBody] CreateSubFolderRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new CreateSubFolderCommand(extractResult.Value.UserId, Guid.Parse(request.ParentFolderId), request.Name, request.Color));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateFolderRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new UpdateFolderCommand(Guid.Parse(request.Id), extractResult.Value.UserId, request.ParentFolderId != null ? Guid.Parse(request.ParentFolderId) : null, request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new DeleteFolderCommand(id, extractResult.Value.UserId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var result = await _mediator.Send(new GetAllFoldersQuery(extractResult.Value.UserId));

            return Ok(result);
        }
    }
}