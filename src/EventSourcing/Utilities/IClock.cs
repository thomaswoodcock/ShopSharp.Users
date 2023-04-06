namespace ShopSharp.Users.EventSourcing.Utilities;

/// <summary>
/// Represents a clock for retrieving the current date and time.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current date and time as an instance of <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <returns>The current date and time as an instance of <see cref="DateTimeOffset"/>.</returns>
    DateTimeOffset Now { get; }
}
