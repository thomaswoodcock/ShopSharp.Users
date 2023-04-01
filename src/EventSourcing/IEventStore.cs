namespace ShopSharp.Users.EventSourcing;

/// <summary>
/// Represents an event store for managing and appending events to streams.
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Asynchronously appends a collection of events to the specified stream.
    /// </summary>
    /// <param name="streamId">The identifier of the stream to which the events will be appended.</param>
    /// <param name="events">An enumerable collection of events to be appended to the stream.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AppendToStreamAsync(
        string streamId, IEnumerable<object> events, CancellationToken cancellationToken = default);
}
