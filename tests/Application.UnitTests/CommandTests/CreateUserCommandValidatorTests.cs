using FluentValidation.TestHelper;
using ShopSharp.Users.Application.Commands.CreateUser;
using ShopSharp.Users.Application.Constants;
using ShopSharp.Users.Domain.CommandTests.Stubs;

namespace ShopSharp.Users.Domain.CommandTests;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        StubStringLocalizer<CreateUserCommandValidator> stringLocalizer = new();
        _validator = new CreateUserCommandValidator(stringLocalizer);
    }

    [Theory]
    [InlineData(UserConstants.NameMinimumLength, UserConstants.PasswordMinimumLength)]
    [InlineData(UserConstants.PasswordMinimumLength, UserConstants.PasswordMaximumLength)]
    public void ValidateDoesNotReturnValidationErrorWhenCommandIsValid(byte nameLength, byte passwordLength)
    {
        // Arrange
        CreateUserCommand command = new(
            new string('a', nameLength),
            $"{Guid.NewGuid()}@example.com",
            new string('a', passwordLength));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public void ValidateReturnsValidationErrorWhenNameIsEmptyOrWhitespace(string name, CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { Name = name };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.Name);
    }

    [Theory]
    [AutoData]
    public void ValidateReturnsValidationErrorWhenNameIsTooShort(CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { Name = new string('a', UserConstants.NameMinimumLength - 1) };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.Name);
    }

    [Theory]
    [AutoData]
    public void ValidateReturnsValidationErrorWhenNameIsTooLong(CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { Name = new string('a', UserConstants.NameMaximumLength + 1) };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.Name);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public void ValidateReturnsValidationErrorWhenEmailAddressIsEmptyOrWhitespace(string emailAddress, CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { EmailAddress = emailAddress };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.EmailAddress);
    }

    [Theory]
    [InlineAutoData("@example.com")]
    [InlineAutoData("user@")]
    [InlineAutoData("user.example.com")]
    public void ValidateReturnsValidationErrorWhenEmailAddressIsInvalid(string emailAddress, CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { EmailAddress = emailAddress };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.EmailAddress);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public void ValidateReturnsValidationErrorWhenPasswordIsEmptyOrWhitespace(string password, CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { Password = password };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.Password);
    }

    [Theory]
    [AutoData]
    public void ValidateReturnsValidationErrorWhenPasswordIsTooShort(CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { Password = new string('a', UserConstants.PasswordMinimumLength - 1) };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.Password);
    }

    [Theory]
    [AutoData]
    public void ValidateReturnsValidationErrorWhenPasswordIsTooLong(CreateUserCommand command)
    {
        // Arrange
        var invalidCommand = command with { Password = new string('a', UserConstants.PasswordMaximumLength + 1) };

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(createUserCommand => createUserCommand.Password);
    }
}
