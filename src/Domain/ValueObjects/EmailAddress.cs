using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace ShopSharp.Users.Domain.ValueObjects;

/// <summary>
/// Represents a valid email address in the domain model.
/// </summary>
public record EmailAddress
{
    private static readonly Regex EmailAddressRegex = new(@"^(.+)@(.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private EmailAddress(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the email address value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Attempts to create a new <see cref="EmailAddress" /> instance with the specified value.
    /// </summary>
    /// <param name="emailAddress">The email address to validate and create a new instance from.</param>
    /// <returns>
    /// A <see cref="Result{T,E}" /> object containing either a new <see cref="EmailAddress" /> instance with the specified value,
    /// or a <see cref="CreateEmailAddressError" /> enum value indicating the reason for the failure.
    /// </returns>
    public static Result<EmailAddress, CreateEmailAddressError> Create(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            return CreateEmailAddressError.EmptyOrWhitespaceEmailAddress;
        }

        if (!IsValidEmailAddressFormat(emailAddress))
        {
            return CreateEmailAddressError.InvalidEmailAddressFormat;
        }

        return new EmailAddress(emailAddress);
    }

    private static bool IsValidEmailAddressFormat(string emailAddress)
    {
        return EmailAddressRegex.IsMatch(emailAddress);
    }
}

/// <summary>
/// Represents the possible errors that can occur when creating a new email address value object using the <see cref="EmailAddress.Create" />
/// factory method.
/// </summary>
public enum CreateEmailAddressError
{
    /// <summary>
    /// There is no error.
    /// </summary>
    None = 0,

    /// <summary>
    /// The provided email address is empty or contains only whitespace characters.
    /// </summary>
    EmptyOrWhitespaceEmailAddress,

    /// <summary>
    /// The provided email address does not conform to the standard email address format.
    /// </summary>
    InvalidEmailAddressFormat
}
