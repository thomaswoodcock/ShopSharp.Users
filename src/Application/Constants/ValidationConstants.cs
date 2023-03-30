namespace ShopSharp.Users.Application.Constants;

/// <summary>
/// Represents a collection of constants used for validation error messages.
/// </summary>
public static class ValidationConstants
{
    /// <summary>
    /// The message format used when a required property is not provided.
    /// </summary>
    public const string RequiredMessage = "{0} is required.";

    /// <summary>
    /// The message format used when a property value does not meet the minimum character length requirement.
    /// </summary>
    public const string MinimumCharacterLengthMessage = "{0} must be at least {1} characters in length.";

    /// <summary>
    /// The message format used when a property value exceeds the maximum character length allowed.
    /// </summary>
    public const string MaximumCharacterLengthMessage = "{0} must be at most {1} characters in length.";

    /// <summary>
    /// The message format used when an email address property value is not in a valid format.
    /// </summary>
    public const string ValidEmailMessage = "{0} must be a valid email address.";
}
