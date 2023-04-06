namespace ShopSharp.Users.EventSourcing.Utilities;

internal class SystemClock : IClock
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
