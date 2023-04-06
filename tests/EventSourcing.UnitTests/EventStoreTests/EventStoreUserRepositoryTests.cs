using MediatR;
using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.ValueObjects;
using ShopSharp.Users.EventSourcing.EventRecords;
using ShopSharp.Users.EventSourcing.EventStores;

namespace ShopSharp.Users.EventSourcing.EventStoreTests;

public class EventStoreUserRepositoryTests
{
    private readonly IEventRecordFactory _eventRecordFactory = Substitute.For<IEventRecordFactory>();
    private readonly IEventStore _eventStore = Substitute.For<IEventStore>();
    private readonly IPublisher _publisher = Substitute.For<IPublisher>();
    private readonly EventStoreUserRepository _repository;

    public EventStoreUserRepositoryTests()
    {
        _repository = new EventStoreUserRepository(_eventRecordFactory, _eventStore, _publisher);
    }

    [Theory]
    [AutoData]
    public async Task SaveAsyncStoresAndPublishesDomainEvents(IList<EventRecord> eventRecords, CancellationToken cancellationToken)
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");
        var user = User.Create(Guid.NewGuid().ToString(), emailAddress.Value, Guid.NewGuid().ToString());

        _eventRecordFactory.CreateFromDomainEvents(user.Value.UncommittedDomainEvents)
            .Returns(eventRecords);

        // Act
        await _repository.SaveAsync(user.Value, cancellationToken)
            .ConfigureAwait(false);

        // Assert
        Received.InOrder(async () =>
        {
            await _eventStore.AppendToStreamAsync(
                    $"User:{user.Value.Id}",
                    eventRecords,
                    cancellationToken)
                .ConfigureAwait(false);

            await _publisher.Publish(user.Value.UncommittedDomainEvents.Single(), cancellationToken)
                .ConfigureAwait(false);
        });
    }

    [Fact]
    public async Task SaveAsyncMarksDomainEventsAsCommitted()
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");
        var user = User.Create(Guid.NewGuid().ToString(), emailAddress.Value, Guid.NewGuid().ToString());

        // Act
        await _repository.SaveAsync(user.Value)
            .ConfigureAwait(false);

        // Assert
        user.Value.UncommittedDomainEvents
            .Should()
            .BeEmpty();
    }
}
