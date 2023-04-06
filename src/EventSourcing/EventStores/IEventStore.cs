using ShopSharp.Users.EventSourcing.EventRecords;

namespace ShopSharp.Users.EventSourcing.EventStores;

/// <summary>
/// Represents an event store for managing and appending events to streams.
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Asynchronously appends a collection of event records to the specified stream.
    /// </summary>
    /// <param name="streamId">The identifier of the stream to which the events will be appended.</param>
    /// <param name="eventRecords">An enumerable collection of event records to be appended to the stream.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AppendToStreamAsync(
        string streamId, IEnumerable<EventRecord> eventRecords, CancellationToken cancellationToken = default);
}
