using Crossdyne.Toolkit.Results;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Shared.Application.Behaviors;
using Xunit;

namespace EnigmaVault.Secret.Unit.Tests.Behaviors
{
    public class ValidationBehaviorTests
    {
        private static Mock<IValidator<TRequest>> CreateValidator<TRequest>(params ValidationFailure[] failures)
        {
            var validator = new Mock<IValidator<TRequest>>();
            validator
                .Setup(x => x.ValidateAsync(
                    It.IsAny<ValidationContext<TRequest>>(), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            return validator;
        }

        public record FakeCommand : IRequest<Result>;
        public record FakeCommandWithResult : IRequest<Result<string>>;

        [Fact]
        public async Task Handle_Result_NoValidators_CallsNext()
        {
            var behavior = new ValidationBehavior<FakeCommand, Result>(Array.Empty<IValidator<FakeCommand>>());
        
            var next = new Mock<RequestHandlerDelegate<Result>>();
            next.Setup(x => x()).ReturnsAsync(Result.Success());

            var result = await behavior.Handle(
                new FakeCommand(),
                next.Object,
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            next.Verify(x => x(), Times.Once);
        }

        [Fact]
        public async Task Handle_Result_ValidationFails_ReturnsFailure()
        {
            var failures = new[]
            {
                new ValidationFailure("Name", "Имя обязательно")
            };

            var validators = new[] { CreateValidator<FakeCommand>(failures).Object };
            var behavior = new ValidationBehavior<FakeCommand, Result>(validators);
        
            var next = new Mock<RequestHandlerDelegate<Result>>();

            var result = await behavior.Handle(
                new FakeCommand(),
                next.Object,
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("Имя обязательно");
        
            next.Verify(x => x(), Times.Never);
        }

        [Fact]
        public async Task Handle_Result_ValidationPasses_CallsNext()
        {
            var validators = new[] { CreateValidator<FakeCommand>().Object };
            var behavior = new ValidationBehavior<FakeCommand, Result>(validators);
        
            var next = new Mock<RequestHandlerDelegate<Result>>();
            next.Setup(x => x()).ReturnsAsync(Result.Success());

            var result = await behavior.Handle(
                new FakeCommand(),
                next.Object,
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            next.Verify(x => x(), Times.Once);
        }

        [Fact]
        public async Task Handle_ResultOfString_NoValidators_CallsNext()
        {
            var behavior = new ValidationBehavior<FakeCommandWithResult, Result<string>>(Array.Empty<IValidator<FakeCommandWithResult>>());
        
            var next = new Mock<RequestHandlerDelegate<Result<string>>>();
            next.Setup(x => x()).ReturnsAsync(Result<string>.Success("test-id"));

            var result = await behavior.Handle(
                new FakeCommandWithResult(),
                next.Object,
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("test-id");
            next.Verify(x => x(), Times.Once);
        }

        [Fact]
        public async Task Handle_ResultOfString_ValidationFails_ReturnsFailure()
        {
            var failures = new[]
            {
                new ValidationFailure("AssetName", "Имя не может быть пустым"),
                new ValidationFailure("FileName", "Файл обязателен")
            };

            var validators = new[] { CreateValidator<FakeCommandWithResult>(failures).Object };
            var behavior = new ValidationBehavior<FakeCommandWithResult, Result<string>>(validators);
        
            var next = new Mock<RequestHandlerDelegate<Result<string>>>();

            var result = await behavior.Handle(
                new FakeCommandWithResult(),
                next.Object,
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().HaveCount(2);
            next.Verify(x => x(), Times.Never);
        }

        [Fact]
        public async Task Handle_ResultOfString_ValidationPasses_CallsNext()
        {
            var validators = new[] { CreateValidator<FakeCommandWithResult>().Object };
            var behavior = new ValidationBehavior<FakeCommandWithResult, Result<string>>(validators);
        
            var next = new Mock<RequestHandlerDelegate<Result<string>>>();
            next.Setup(x => x()).ReturnsAsync(Result<string>.Success("created-id"));

            var result = await behavior.Handle(
                new FakeCommandWithResult(),
                next.Object,
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("created-id");
            next.Verify(x => x(), Times.Once);
        }

        [Fact]
        public async Task Handle_MultipleValidators_AggregatesErrors()
        {
            var validator1 = CreateValidator<FakeCommand>(new ValidationFailure("Field1", "Ошибка валидации 1"));
        
            var validator2 = CreateValidator<FakeCommand>(new ValidationFailure("Field2", "Ошибка валидации 2"));

            var behavior = new ValidationBehavior<FakeCommand, Result>(new[] { validator1.Object, validator2.Object });
        
            var next = new Mock<RequestHandlerDelegate<Result>>();

            var result = await behavior.Handle(
                new FakeCommand(),
                next.Object,
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().HaveCount(2);
            next.Verify(x => x(), Times.Never);
        }
    }
}