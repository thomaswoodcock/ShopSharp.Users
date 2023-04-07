using ShopSharp.Users.Application.Commands.CreateUser;
using ShopSharp.Users.Application.Services;
using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.Repositories;
using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Application.CommandTests;

public class CreateUserCommandHandlerTests
{
    private readonly CreateUserCommandHandler _commandHandler;
    private readonly IUserQueryService _userQueryService = Substitute.For<IUserQueryService>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    public CreateUserCommandHandlerTests()
    {
        _commandHandler = new CreateUserCommandHandler(_userQueryService, _passwordHasher, _userRepository);
    }

    [Theory]
    [AutoData]
    public async Task HandleStoresUserInRepository(
        CreateUserCommand command,
        string hashedPassword,
        CancellationToken cancellationToken)
    {
        // Arrange
        var emailAddress = $"{Guid.NewGuid()}@example.com";
        var validCommand = command with { EmailAddress = emailAddress };

        _passwordHasher.HashPassword(validCommand.Password)
            .Returns(hashedPassword);

        // Act
        await _commandHandler.Handle(validCommand, cancellationToken)
            .ConfigureAwait(false);

        // Assert
        await _userRepository
            .Received(1)
            .SaveAsync(Arg.Is<User>(user =>
                user.Id != Guid.Empty
                && user.Name == validCommand.Name
                && user.EmailAddress.Value == validCommand.EmailAddress
                && user.Password == hashedPassword), cancellationToken)
            .ConfigureAwait(false);
    }

    [Theory]
    [AutoData]
    public async Task HandleReturnsCreateUserCommandResponse(CreateUserCommand command)
    {
        // Arrange
        var newUserId = Guid.NewGuid();
        var emailAddress = $"{Guid.NewGuid()}@example.com";
        var validCommand = command with { EmailAddress = emailAddress };

        _passwordHasher.HashPassword(validCommand.Password)
            .Returns(validCommand.Password);

        _userRepository
            .When(u => u.SaveAsync(Arg.Any<User>()))
            .Do(c => newUserId = c.Arg<User>().Id);

        // Act
        var result =
            await _commandHandler.Handle(validCommand)
                .ConfigureAwait(false);

        // Assert
        result
            .Should()
            .SucceedWith(
                new CreateUserCommandResponse(newUserId));
    }

    [Theory]
    [AutoData]
    public async Task HandleReturnsEmailAddressAlreadyInUseError(CreateUserCommand command)
    {
        // Arrange
        var emailAddress = $"{Guid.NewGuid()}@example.com";
        var validCommand = command with { EmailAddress = emailAddress };

        _userQueryService.ExistsByEmailAsync(
                Arg.Is<EmailAddress>(email => email.Value == validCommand.EmailAddress))
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(validCommand);

        // Assert
        result
            .Should()
            .FailWith(CreateUserCommandError.EmailAddressAlreadyInUse);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public async Task HandleReturnsEmptyOrWhitespaceNameError(string name, CreateUserCommand command)
    {
        // Arrange
        var emailAddress = $"{Guid.NewGuid()}@example.com";
        var invalidCommand = command with { Name = name, EmailAddress = emailAddress };

        // Act
        var result = await _commandHandler.Handle(invalidCommand)
            .ConfigureAwait(false);

        // Assert
        result
            .Should()
            .FailWith(CreateUserCommandError.EmptyOrWhitespaceName);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public async Task HandleReturnsEmptyOrWhitespaceEmailAddressError(string emailAddress, CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { EmailAddress = emailAddress };

        // Act
        var result = await _commandHandler.Handle(invalidCommand)
            .ConfigureAwait(false);

        // Assert
        result
            .Should()
            .FailWith(CreateUserCommandError.EmptyOrWhitespaceEmailAddress);
    }

    [Theory]
    [AutoData]
    public async Task HandleReturnsInvalidEmailAddressFormatError(CreateUserCommand command)
    {
        // Arrange

        // Act
        var result = await _commandHandler.Handle(command)
            .ConfigureAwait(false);

        // Assert
        result
            .Should()
            .FailWith(CreateUserCommandError.InvalidEmailAddressFormat);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public async Task HandleReturnsEmptyOrWhitespacePasswordError(string password, CreateUserCommand command)
    {
        // Arrange
        var emailAddress = $"{Guid.NewGuid()}@example.com";
        var invalidCommand = command with { EmailAddress = emailAddress, Password = password };

        // Act
        var result = await _commandHandler.Handle(invalidCommand)
            .ConfigureAwait(false);

        // Assert
        result
            .Should()
            .FailWith(CreateUserCommandError.EmptyOrWhitespacePassword);
    }
}
