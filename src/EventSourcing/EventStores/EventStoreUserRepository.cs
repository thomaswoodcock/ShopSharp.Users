using MediatR;
using ShopSharp.Core.Domain.Aggregates;
using ShopSharp.Users.Domain.Aggregates;
using ShopSharp.Users.Domain.Repositories;
using ShopSharp.Users.EventSourcing.EventRecords;

namespace ShopSharp.Users.EventSourcing.EventStores;

internal class EventStoreUserRepository : IUserRepository
{
    private readonly IEventRecordFactory _eventRecordFactory;
    private readonly IEventStore _eventStore;
    private readonly IPublisher _publisher;

    public EventStoreUserRepository(IEventRecordFactory eventRecordFactory, IEventStore eventStore, IPublisher publisher)
    {
        _eventRecordFactory = eventRecordFactory;
        _eventStore = eventStore;
        _publisher = publisher;
    }

    public async Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        var streamId = GetStreamId(user);
        var eventRecords = _eventRecordFactory.CreateFromDomainEvents(user.UncommittedDomainEvents);

        await _eventStore.AppendToStreamAsync(streamId, eventRecords, cancellationToken)
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
