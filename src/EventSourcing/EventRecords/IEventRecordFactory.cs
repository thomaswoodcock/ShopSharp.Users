using ShopSharp.Core.Domain.Events;

namespace ShopSharp.Users.EventSourcing.EventRecords;

/// <summary>
/// Represents a factory for creating EventRecord instances from domain events.
/// </summary>
public interface IEventRecordFactory
{
    /// <summary>
    /// Creates a collection of EventRecord instances from the specified domain events.
    /// </summary>
    /// <param name="domainEvents">The domain events to create EventRecords for.</param>
    /// <returns>An enumerable collection of EventRecord instances containing the provided domain events.</returns>
    IEnumerable<EventRecord> CreateFromDomainEvents(IEnumerable<DomainEvent> domainEvents);
}
