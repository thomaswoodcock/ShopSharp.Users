namespace ShopSharp.Users.EventSourcing.EventRecords;

/// <summary>
/// Represents a record of an event in an event store, along with its metadata.
/// </summary>
/// <param name="EventId">The unique identifier of the event.</param>
/// <param name="EventType">The type name of the event.</param>
/// <param name="Timestamp">The timestamp of when the event was created.</param>
/// <param name="Version">The version number of the event.</param>
/// <param name="EventData">The event data object.</param>
public record EventRecord(Guid EventId, string EventType, DateTimeOffset Timestamp, int Version, object EventData);
