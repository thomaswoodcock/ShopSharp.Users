using ShopSharp.Users.EventSourcing.EventRecords;
using ShopSharp.Users.EventSourcing.EventRecordTests.Fakes;
using ShopSharp.Users.EventSourcing.Utilities;
using ShopSharp.Users.EventSourcing.Versioning;

namespace ShopSharp.Users.EventSourcing.EventRecordTests;

public class EventRecordFactoryTests
{
    private readonly IClock _clock = Substitute.For<IClock>();
    private readonly EventRecordFactory _factory;
    private readonly IDomainEventVersioningStrategy _versioningStrategy = Substitute.For<IDomainEventVersioningStrategy>();

    public EventRecordFactoryTests()
    {
        _factory = new EventRecordFactory(_clock, _versioningStrategy);
    }

    [Theory]
    [AutoData]
    public void CreateFromDomainEventsReturnsEventRecords(FakeDomainEvent domainEvent, DateTimeOffset now, int version)
    {
        // Arrange
        var domainEvents = new[] { domainEvent };

        _clock.Now
            .Returns(now);

        _versioningStrategy.GetVersion(domainEvent)
            .Returns(version);

        // Act
        var result = _factory.CreateFromDomainEvents(domainEvents);

        // Assert
        var eventRecord = result.Single();

        eventRecord
            .EventId
            .Should()
            .NotBeEmpty();

        eventRecord
            .EventType
            .Should()
            .Be("FakeDomainEvent");

        eventRecord
            .Timestamp
            .Should()
            .Be(now);

        eventRecord
            .Version
            .Should()
            .Be(version);

        eventRecord
            .EventData
            .Should()
            .Be(domainEvent);
    }

    [Theory]
    [AutoData]
    public void CreateFromDomainEventsReturnsEventRecordForEachDomainEvent(FakeDomainEvent[] domainEvents)
    {
        // Arrange

        // Act
        var result = _factory.CreateFromDomainEvents(domainEvents);

        // Assert
        result.Should()
            .HaveCount(domainEvents.Length);
    }
}
