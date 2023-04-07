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
        // Arrange

        // Act
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
        // Arrange

        // Act
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
        // Arrange

        // Act
        var result = EmailAddress.Create(emailAddress);

        // Assert
        result
            .Should()
            .FailWith(CreateEmailAddressError.InvalidEmailAddressFormat);
    }

    [Fact]
    public void EqualsReturnsTrueForEmailAddressesWithSameCharactersButDifferentCasing()
    {
        // Arrange
        var firstResult = EmailAddress.Create("user@example.com");
        var secondResult = EmailAddress.Create("User@EXAMPLE.com");

        // Act
        var result = firstResult.Value == secondResult.Value;

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public void EqualsReturnsFalseForEmailAddressesWithDifferentCharacters()
    {
        // Arrange
        var firstResult = EmailAddress.Create("user@example.com");
        var secondResult = EmailAddress.Create("another.user@example.com");

        // Act
        var result = firstResult.Value == secondResult.Value;

        // Assert
        result
            .Should()
            .BeFalse();
    }
}
