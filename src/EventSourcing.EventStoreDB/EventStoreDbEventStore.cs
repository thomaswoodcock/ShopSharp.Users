using System.Text.Json;
using EventStore.Client;
using ShopSharp.Users.EventSourcing.EventStores;
using EventRecord = ShopSharp.Users.EventSourcing.EventRecords.EventRecord;

namespace ShopSharp.Users.EventSourcing.EventStoreDB;

internal class EventStoreDbEventStore : IEventStore, IDisposable
{
    private readonly EventStoreClient _eventStoreClient;

    public EventStoreDbEventStore(string connectionString)
    {
        var eventStoreClientSettings = EventStoreClientSettings.Create(connectionString);
        _eventStoreClient = new EventStoreClient(eventStoreClientSettings);
    }

    public void Dispose()
    {
        _eventStoreClient.Dispose();
    }

    public Task AppendToStreamAsync(string streamId, IEnumerable<EventRecord> eventRecords, CancellationToken cancellationToken = default)
    {
        var mappedEventRecords = eventRecords.Select(MapEventRecordToEventData);

        return _eventStoreClient.AppendToStreamAsync(streamId, StreamState.Any, mappedEventRecords, cancellationToken: cancellationToken);
    }

    private static EventData MapEventRecordToEventData(EventRecord eventRecord)
    {
        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(eventRecord);

        return new EventData(Uuid.FromGuid(eventRecord.EventId), eventRecord.EventType, jsonBytes);
    }
}
