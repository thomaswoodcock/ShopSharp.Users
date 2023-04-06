using ShopSharp.Core.Domain.Events;
using ShopSharp.Users.Domain.Events.UserCreated;

namespace ShopSharp.Users.EventSourcing.Versioning;

internal class SimpleDomainEventVersioningStrategy : IDomainEventVersioningStrategy
{
    public int GetVersion(DomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserCreatedEvent => 1,
            _ => 1
        };
    }
}
