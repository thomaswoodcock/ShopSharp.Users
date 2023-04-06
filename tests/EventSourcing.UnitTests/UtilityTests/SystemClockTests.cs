using ShopSharp.Users.EventSourcing.Utilities;

namespace ShopSharp.Users.EventSourcing.UtilityTests;

public class SystemClockTests
{
    private readonly SystemClock _systemClock = new();

    [Fact]
    public void NowReturnsCurrentDateTimeOffset()
    {
        // Arrange

        // Act
        var result = _systemClock.Now;

        // Assert
        result.Should()
            .BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(20));
    }
}
