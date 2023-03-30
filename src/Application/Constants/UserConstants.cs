namespace ShopSharp.Users.Application.Constants;

/// <summary>
/// Represents a collection of constants used for defining user-related constraints.
/// </summary>
public static class UserConstants
{
    /// <summary>
    /// The minimum allowed length for a user's name.
    /// </summary>
    public const byte NameMinimumLength = 2;

    /// <summary>
    /// The maximum allowed length for a user's name.
    /// </summary>
    public const byte NameMaximumLength = 100;

    /// <summary>
    /// The minimum allowed length for a user's password.
    /// </summary>
    public const byte PasswordMinimumLength = 8;

    /// <summary>
    /// The maximum allowed length for a user's password.
    /// </summary>
    public const byte PasswordMaximumLength = 128;
}
