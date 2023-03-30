namespace ShopSharp.Users.Application.Commands.CreateUser;

/// <summary>
/// Represents the response for a successful <see cref="CreateUserCommand" /> execution.
/// </summary>
/// <param name="UserId">The unique identifier of the created user.</param>
public record CreateUserCommandResponse(Guid UserId);
