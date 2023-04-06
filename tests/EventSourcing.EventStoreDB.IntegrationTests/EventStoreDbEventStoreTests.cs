using System.Text.Json;
using EventStore.Client;
using ShopSharp.Users.EventSourcing.EventStoreDB.Fixtures;
using EventRecord = ShopSharp.Users.EventSourcing.EventRecords.EventRecord;

namespace ShopSharp.Users.EventSourcing.EventStoreDB;

public class EventStoreDbEventStoreTests : IClassFixture<EventStoreDbFixture>
{
    private readonly EventStoreDbEventStore _eventStore;
    private readonly EventStoreDbFixture _eventStoreDbFixture;

    public EventStoreDbEventStoreTests(EventStoreDbFixture eventStoreDbFixture)
    {
        _eventStore = new EventStoreDbEventStore(EventStoreDbFixture.ConnectionString);
        _eventStoreDbFixture = eventStoreDbFixture;
    }

    [Theory]
    [AutoData]
    public async Task AppendToStreamAsyncCreatesNewStream(EventRecord eventRecord, string streamId)
    {
        // Arrange

        // Act
        await _eventStore.AppendToStreamAsync(streamId, new[] { eventRecord })
            .ConfigureAwait(false);

        // Assert
        var events = await ReadEventsFromStreamAsync(streamId)
            .ConfigureAwait(false);

        events.Should()
            .HaveCount(1);
    }

    [Theory]
    [AutoData]
    public async Task AppendToStreamAsyncUsesEventRecordEventIdAsEventId(EventRecord eventRecord, string streamId)
    {
        // Arrange

        // Act
        await _eventStore.AppendToStreamAsync(streamId, new[] { eventRecord })
            .ConfigureAwait(false);

        // Assert
        var events = await ReadEventsFromStreamAsync(streamId)
            .ConfigureAwait(false);

        var @event = events.Single().Event;

        @event.EventId
            .Should()
            .Be(Uuid.FromGuid(eventRecord.EventId));
    }

    [Theory]
    [AutoData]
    public async Task AppendToStreamAsyncUsesEventRecordEventTypeAsEventType(EventRecord eventRecord, string streamId)
    {
        // Arrange

        // Act
        await _eventStore.AppendToStreamAsync(streamId, new[] { eventRecord })
            .ConfigureAwait(false);

        // Assert
        var events = await ReadEventsFromStreamAsync(streamId)
            .ConfigureAwait(false);

        var @event = events.Single().Event;

        @event.EventType
            .Should()
            .Be(eventRecord.EventType);
    }

    [Theory]
    [AutoData]
    public async Task AppendToStreamAsyncStoresJsonBytes(EventRecord eventRecord, string streamId)
    {
        // Arrange

        // Act
        await _eventStore.AppendToStreamAsync(streamId, new[] { eventRecord })
            .ConfigureAwait(false);

        // Assert
        var events = await ReadEventsFromStreamAsync(streamId)
            .ConfigureAwait(false);

        var @event = events.Single().Event;
        var deserializedEvent = JsonSerializer.Deserialize<EventRecord>(@event.Data.ToArray());

        deserializedEvent.Should()
            .BeEquivalentTo(eventRecord);
    }

    private async Task<IReadOnlyList<ResolvedEvent>> ReadEventsFromStreamAsync(string streamId)
    {
        var result = _eventStoreDbFixture.Client.ReadStreamAsync(Direction.Forwards, streamId, StreamPosition.Start);

        return await result.ToListAsync()
            .ConfigureAwait(false);
    }
}
