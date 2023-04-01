using MediatR;
using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.EventSourcing;

public class EventStoreUserRepositoryTests
{
    private readonly IEventStore _eventStore = Substitute.For<IEventStore>();
    private readonly IPublisher _publisher = Substitute.For<IPublisher>();
    private readonly EventStoreUserRepository _repository;

    public EventStoreUserRepositoryTests()
    {
        _repository = new EventStoreUserRepository(_eventStore, _publisher);
    }

    [Theory]
    [AutoData]
    public async Task SaveAsyncStoresAndPublishesDomainEvents(CancellationToken cancellationToken)
    {
        // Arrange
        var emailAddress = EmailAddress.Create($"{Guid.NewGuid()}@example.com");
        var user = User.Create(Guid.NewGuid().ToString(), emailAddress.Value, Guid.NewGuid().ToString());

        // Act
        await _repository.SaveAsync(user.Value, cancellationToken)
            .ConfigureAwait(false);

        // Assert
        Received.InOrder(async () =>
        {
            await _eventStore.AppendToStreamAsync(
                    $"User:{user.Value.Id}",
                    user.Value.UncommittedDomainEvents,
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
