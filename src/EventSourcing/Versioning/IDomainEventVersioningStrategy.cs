using ShopSharp.Core.Domain.Events;

namespace ShopSharp.Users.EventSourcing.Versioning;

/// <summary>
/// Represents a strategy for managing domain event versioning.
/// </summary>
public interface IDomainEventVersioningStrategy
{
    /// <summary>
    /// Determines the version for the given domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event for which the version should be determined.</param>
    /// <returns>The version of the domain event.</returns>
    int GetVersion(DomainEvent domainEvent);
}
