using ShopSharp.Core.Domain.Events;
using ShopSharp.Users.EventSourcing.Utilities;
using ShopSharp.Users.EventSourcing.Versioning;

namespace ShopSharp.Users.EventSourcing.EventRecords;

internal class EventRecordFactory : IEventRecordFactory
{
    private readonly IClock _clock;
    private readonly IDomainEventVersioningStrategy _versioningStrategy;

    public EventRecordFactory(IClock clock, IDomainEventVersioningStrategy versioningStrategy)
    {
        _clock = clock;
        _versioningStrategy = versioningStrategy;
    }

    public IEnumerable<EventRecord> CreateFromDomainEvents(IEnumerable<DomainEvent> domainEvents)
    {
        return domainEvents.Select(domainEvent => new EventRecord(
            Guid.NewGuid(), domainEvent.GetType().Name, _clock.Now, _versioningStrategy.GetVersion(domainEvent), domainEvent));
    }
}
