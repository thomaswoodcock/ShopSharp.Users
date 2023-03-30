using ShopSharp.Users.Domain.Aggregates;

namespace ShopSharp.Users.Domain.Repositories;

/// <summary>
/// Represents a repository for managing user aggregate roots.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Asynchronously saves the given user aggregate root.
    /// </summary>
    /// <param name="user">The user aggregate root to save.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task SaveAsync(User user, CancellationToken cancellationToken = default);
}
