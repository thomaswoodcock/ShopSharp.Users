namespace ShopSharp.Users.Application.Services;

/// <summary>
/// Represents a service that hashes passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes the given password.
    /// </summary>
    /// <param name="password">The password to be hashed.</param>
    /// <returns>A string representing the hashed password.</returns>
    string HashPassword(string password);
}
