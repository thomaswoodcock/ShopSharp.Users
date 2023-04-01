using MediatR;
using ShopSharp.Core.Domain.Aggregates;
using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.Repositories;

namespace ShopSharp.Users.EventSourcing;

internal class EventStoreUserRepository : IUserRepository
{
    private readonly IEventStore _eventStore;
    private readonly IPublisher _publisher;

    public EventStoreUserRepository(IEventStore eventStore, IPublisher publisher)
    {
        _eventStore = eventStore;
        _publisher = publisher;
    }

    public async Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        var streamId = GetStreamId(user);

        await _eventStore.AppendToStreamAsync(streamId, user.UncommittedDomainEvents, cancellationToken)
            .ConfigureAwait(false);

        foreach (var domainEvent in user.UncommittedDomainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken)
                .ConfigureAwait(false);
        }

        user.MarkDomainEventsAsCommitted();
    }

    private static string GetStreamId(AggregateRoot aggregateRoot)
    {
        return $"{aggregateRoot.GetType().Name}:{aggregateRoot.Id}";
    }
}
