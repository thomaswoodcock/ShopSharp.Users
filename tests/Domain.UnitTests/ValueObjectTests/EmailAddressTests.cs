using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Domain.ValueObjectTests;

public class EmailAddressTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name@example.co.uk")]
    [InlineData("user@subdomain.example.com")]
    public void CreateReturnsNewEmailAddress(string emailAddress)
    {
        // Arrange Act
        var result = EmailAddress.Create(emailAddress);

        // Assert
        result
            .Should()
            .Succeed();

        result.Value.Value
            .Should()
            .Be(emailAddress);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateReturnsEmptyOrWhitespaceEmailAddressError(string emailAddress)
    {
        // Arrange Act
        var result = EmailAddress.Create(emailAddress);

        // Assert
        result
            .Should()
            .FailWith(CreateEmailAddressError.EmptyOrWhitespaceEmailAddress);
    }

    [Theory]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user.example.com")]
    public void CreateReturnsInvalidEmailAddressFormatError(string emailAddress)
    {
        // Arrange Act
        var result = EmailAddress.Create(emailAddress);

        // Assert
        result
            .Should()
            .FailWith(CreateEmailAddressError.InvalidEmailAddressFormat);
    }
}
