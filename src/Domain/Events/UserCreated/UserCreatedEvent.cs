using ShopSharp.Core.Domain.Aggregates;
using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Domain.Events.UserCreated;

/// <summary>
/// Represents an event that occurs when a user is created in the domain model.
/// </summary>
/// <param name="UserId">The unique identifier of the created user.</param>
/// <param name="UserName">The name of the created user.</param>
/// <param name="UserEmailAddress">The email address of the created user.</param>
/// <param name="UserPassword">The password of the created user.</param>
public record UserCreatedEvent(
    Guid UserId,
    string UserName,
    EmailAddress UserEmailAddress,
    string UserPassword) : DomainEvent;
