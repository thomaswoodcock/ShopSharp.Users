using ShopSharp.Users.Domain.ValueObjects;

namespace ShopSharp.Users.Application.Services;

/// <summary>
/// Represents a service for querying user data.
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// Checks if a user with the specified email address already exists.
    /// </summary>
    /// <param name="emailAddress">The email address to check for existing users.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a boolean value indicating whether a user with the
    /// specified email address exists.
    /// </returns>
    Task<bool> ExistsByEmailAsync(EmailAddress emailAddress, CancellationToken cancellationToken = default);
}
