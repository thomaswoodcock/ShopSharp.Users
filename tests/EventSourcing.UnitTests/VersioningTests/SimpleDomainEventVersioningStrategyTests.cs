using ShopSharp.Core.Domain.Events;
using ShopSharp.Users.Domain.Events.UserCreated;
using ShopSharp.Users.Domain.ValueObjects;
using ShopSharp.Users.EventSourcing.Versioning;

namespace ShopSharp.Users.EventSourcing.VersioningTests;

public class SimpleDomainEventVersioningStrategyTests
{
    private readonly SimpleDomainEventVersioningStrategy _strategy = new();

    [Theory]
    [AutoData]
    public void GetVersionReturnsVersionForUserCreatedEvent(Guid userId, string userName, string userPassword)
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");
        UserCreatedEvent userCreatedEvent = new(userId, userName, emailAddress.Value, userPassword);

        // Act
        var result = _strategy.GetVersion(userCreatedEvent);

        // Assert
        result.Should()
            .Be(1);
    }

    [Fact]
    public void GetVersionReturnsDefaultVersionForUnhandledDomainEvents()
    {
        // Arrange
        var domainEvent = Substitute.For<DomainEvent>();

        // Act
        var result = _strategy.GetVersion(domainEvent);

        // Assert
        result.Should()
            .Be(1);
    }
}
