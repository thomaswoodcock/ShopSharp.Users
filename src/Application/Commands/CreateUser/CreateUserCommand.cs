using CSharpFunctionalExtensions;
using MediatR;

namespace ShopSharp.Users.Application.Commands.CreateUser;

/// <summary>
/// Represents a command to create a new user.
/// </summary>
/// <param name="Name">The user's name.</param>
/// <param name="EmailAddress">The user's email address.</param>
/// <param name="Password">The user's password.</param>
public record CreateUserCommand
    (string Name, string EmailAddress, string Password) : IRequest<Result<CreateUserCommandResponse, CreateUserCommandError>>;

/// <summary>
/// Represents the possible errors that can occur when handling a <see cref="CreateUserCommand" />.
/// </summary>
public enum CreateUserCommandError
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
    /// The provided email address for the user is empty or contains only whitespace characters.
    /// </summary>
    EmptyOrWhitespaceEmailAddress,

    /// <summary>
    /// The provided email address for the user does not conform to the standard email address format.
    /// </summary>
    InvalidEmailAddressFormat,

    /// <summary>
    /// The provided password for the user is empty or contains only whitespace characters.
    /// </summary>
    EmptyOrWhitespacePassword
}
