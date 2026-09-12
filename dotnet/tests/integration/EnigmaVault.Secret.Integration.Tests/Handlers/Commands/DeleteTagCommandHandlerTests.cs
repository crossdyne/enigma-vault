using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.Tags.Commands.Delete;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class DeleteTagCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly DeleteTagCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public DeleteTagCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var tagRepo = new TagRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new DeleteTagCommandHandler(tagRepo, unitOfWork);
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletedTag()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var tag = Tag.Create(userId, TagName.Create("ValidName"), "#F1F3F9");

            await _fixture.DbContext.Set<Tag>().AddAsync(tag, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var tagInDbSaved = await _fixture.DbContext.Set<Tag>().FirstOrDefaultAsync(t => t.Id == tag.Id, _cancellationToken);
            tagInDbSaved.Should().NotBeNull(); 

            var command = new DeleteTagCommand(tag.Id, userId);
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            var deletedTag = await _fixture.DbContext.Set<Tag>().FirstOrDefaultAsync(t => t.Id == tag.Id, _cancellationToken);
            deletedTag.Should().BeNull(); 
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnTagNotFoundError()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var tagId = TagId.Create(Guid.NewGuid());

            var command = new DeleteTagCommand(tagId, userId);
            Result<Unit> result = await _handler.Handle(command, _cancellationToken); 

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}