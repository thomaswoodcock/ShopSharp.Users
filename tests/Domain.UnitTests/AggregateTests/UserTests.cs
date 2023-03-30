using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Domain.AggregateTests;

public class UserTests
{
    [Theory]
    [AutoData]
    public void CreateReturnsNewUser(string name, string password)
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");

        // Act
        var result = User.Create(name, emailAddress.Value, password);

        // Assert
        result
            .Should()
            .Succeed();

        result.Value.Id
            .Should()
            .NotBeEmpty();

        result.Value.Name
            .Should()
            .Be(name);

        result.Value.EmailAddress
            .Should()
            .Be(emailAddress.Value);

        result.Value.Password
            .Should()
            .Be(password);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public void CreateReturnsEmptyOrWhitespaceNameError(string name, string password)
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");

        // Act
        var result = User.Create(name, emailAddress.Value, password);

        // Assert
        result
            .Should()
            .FailWith(CreateUserError.EmptyOrWhitespaceName);
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    public void CreateReturnsEmptyOrWhitespacePasswordError(string password, string name)
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");

        // Act
        var result = User.Create(name, emailAddress.Value, password);

        // Assert
        result
            .Should()
            .FailWith(CreateUserError.EmptyOrWhitespacePassword);
    }
}
