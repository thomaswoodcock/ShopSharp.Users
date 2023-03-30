using CSharpFunctionalExtensions;
using ShopSharp.Core.Domain.Aggregates;
using ShopSharp.Core.Domain.Events;
using ShopSharp.Users.Domain.Events.UserCreated;
using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Domain.Aggregates;

/// <summary>
/// Represents a user aggregate root in the domain model.
/// </summary>
public class User : AggregateRoot
{
    private User(Guid id, string name, EmailAddress emailAddress, string password)
    {
        Id = id;
        Name = name;
        EmailAddress = emailAddress;
        Password = password;
    }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public EmailAddress EmailAddress { get; private set; }

    /// <summary>
    /// Gets the password of the user.
    /// </summary>
    public string Password { get; private set; }

    /// <inheritdoc />
    protected override void ApplyDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }

        switch (domainEvent)
        {
            case UserCreatedEvent userCreated:
                Id = userCreated.UserId;
                Name = userCreated.UserName;
                EmailAddress = userCreated.UserEmailAddress;
                Password = userCreated.UserPassword;
                break;

            default:
                throw new NotSupportedException(
                    $"The domain event type '{domainEvent.GetType().Name}' is not supported by this aggregate root.");
        }
    }

    /// <summary>
    /// Attempts to create a new <see cref="User" /> instance with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the new user.</param>
    /// <param name="emailAddress">The email address of the new user.</param>
    /// <param name="password">The password of the new user.</param>
    /// <returns>
    /// A <see cref="Result{T,E}" /> object containing either a new <see cref="User" /> instance with the specified parameters,
    /// or a <see cref="CreateUserError" /> enum value indicating the reason for the failure.
    /// </returns>
    public static Result<User, CreateUserError> Create(string name, EmailAddress emailAddress, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CreateUserError.EmptyOrWhitespaceName;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return CreateUserError.EmptyOrWhitespacePassword;
        }

        var id = Guid.NewGuid();
        User user = new(id, name, emailAddress, password);

        user.AddAndApplyDomainEvent(
            new UserCreatedEvent(
                user.Id,
                user.Name,
                user.EmailAddress,
                user.Password));

        return user;
    }
}

/// <summary>
/// Represents the possible errors that can occur when creating a new user aggregate using the <see cref="User.Create" /> factory method.
/// </summary>
public enum CreateUserError
{
    /// <summary>
    /// There is no error.
    /// </summary>
    None = 0,

    /// <summary>
    /// The provided name for the user is empty or contains only whitespace characters.
    /// </summary>
    EmptyOrWhitespaceName,

    /// <summary>
    /// The provided password for the user is empty or contains only whitespace characters.
    /// </summary>
    EmptyOrWhitespacePassword
}
