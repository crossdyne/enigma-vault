using Crossdyne.Toolkit.Results;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shared.Application.Behaviors;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using Xunit;

namespace EnigmaVault.Secret.Unit.Tests.Behaviors
{
    public class ExceptionBehaviorTests
    {
        public record FakeCommand : IRequest<Result>;

        [Fact]
        public async Task Handle_NoException_ReturnsResultFromNext()
        {
            var behavior = new ExceptionBehavior<FakeCommand, Result>(NullLogger<ExceptionBehavior<FakeCommand, Result>>.Instance);

            var next = new Mock<RequestHandlerDelegate<Result>>();
            next.Setup(x => x()).ReturnsAsync(Result.Success());

            var result = await behavior.Handle(new FakeCommand(), next.Object, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            next.Verify(x => x(), Times.Once);
        }

        [Fact]
        public async Task Handle_DomainException_ReturnsFailureWithDomainError()
        {
            var behavior = new ExceptionBehavior<FakeCommand, Result>(NullLogger<ExceptionBehavior<FakeCommand, Result>>.Instance);

            var domainError = new Error(AppErrors.Validation, "Ошибка валидации");
            var next = new Mock<RequestHandlerDelegate<Result>>();
            next.Setup(x => x()).ThrowsAsync(new DomainException(domainError));

            var result = await behavior.Handle(new FakeCommand(), next.Object, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("Ошибка валидации");
        }

        [Fact]
        public async Task Handle_UnexpectedException_ReturnsServerError()
        {
            var behavior = new ExceptionBehavior<FakeCommand, Result>(NullLogger<ExceptionBehavior<FakeCommand, Result>>.Instance);

            var next = new Mock<RequestHandlerDelegate<Result>>();
            next.Setup(x => x()).ThrowsAsync(new InvalidOperationException("База данных разорвала соединение"));

            var result = await behavior.Handle(new FakeCommand(), next.Object, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("Ошибка на стороне сервера");
        }

        public record FakeValueCommand : IRequest<Result<string>>;

        [Fact]
        public async Task Handle_GenericResult_DomainException_ReturnsFailure()
        {
            var behavior = new ExceptionBehavior<FakeValueCommand, Result<string>>(NullLogger<ExceptionBehavior<FakeValueCommand, Result<string>>>.Instance);

            var domainError = new Error(ErrorCode.NotFound, "Асет не найден");
            var next = new Mock<RequestHandlerDelegate<Result<string>>>();
            next.Setup(x => x()).ThrowsAsync(new DomainException(domainError));

            var result = await behavior.Handle(new FakeValueCommand(), next.Object, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Code.Should().Be(ErrorCode.NotFound);
        }

        [Fact]
        public async Task Handle_GenericResult_UnexpectedException_ReturnsServerError()
        {
            var behavior = new ExceptionBehavior<FakeValueCommand, Result<string>>(NullLogger<ExceptionBehavior<FakeValueCommand, Result<string>>>.Instance);

            var next = new Mock<RequestHandlerDelegate<Result<string>>>();
            next.Setup(x => x()).ThrowsAsync(new NullReferenceException());

            var result = await behavior.Handle(new FakeValueCommand(), next.Object, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("Ошибка на стороне сервера");
        }
    }
}