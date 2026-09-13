using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.Tags.Commands.Update;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class UpdateTagCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UpdateTagCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UpdateTagCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var tagRepo = new TagRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new UpdateTagCommandHandler(tagRepo, unitOfWork);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatedTag()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var tagName = TagName.Create("ValidName");
            var color = Color.FromHex("#F1F3F9");
            var tag = Tag.Create(userId, tagName, color);

            await _fixture.DbContext.Set<Tag>().AddAsync(tag, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new UpdateTagCommand(userId, tag.Id, "NewName", "#F1F1F1");
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            var updatedTag = await _fixture.DbContext.Set<Tag>().FirstOrDefaultAsync(t => t.Id == tag.Id, _cancellationToken);
            updatedTag.Should().NotBeNull();
            updatedTag.Name.Value.Should().Be("NewName");
            updatedTag.Color.Value.Should().Be("#F1F1F1"); 
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnTagNotFoundError()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var tagId = TagId.Create(Guid.NewGuid());

            var command = new UpdateTagCommand(userId, tagId, "NewName", "#F1F1F1");
            Result<Unit> result = await _handler.Handle(command, _cancellationToken); 

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}