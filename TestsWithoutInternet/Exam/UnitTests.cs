using System;
using System.Collections.Generic;
using Xunit;
using UserTracker;

public class UnitTests
{
    [Fact]
    public void Expect_FirstSeenDate_When_ActivityExists()
    {
        // Arrange
        var userActivity = new UserActivity();
        userActivity.ActivityPeriods = new List<TimePeriod>
        {
            new TimePeriod
            {
                Start = DateTime.Parse("2023-10-08T12:00:00"),
                End = DateTime.Parse("2023-10-08T12:30:00")
            },
            new TimePeriod
            {
                Start = DateTime.Parse("2023-10-08T11:00:00"),
                End = DateTime.Parse("2023-10-08T11:30:00")
            },
        };

        // Act
        var firstSeen = userActivity.FirstSeen();

        // Assert
        Assert.Equal(DateTime.Parse("2023-10-08T11:00:00"), firstSeen);
    }

    [Fact]
    public void Expect_Null_When_UserHasNoActivities()
    {
        // Arrange
        var userActivity = new UserActivity();
        userActivity.ActivityPeriods = new List<TimePeriod>();

        // Act
        var firstSeen = userActivity.FirstSeen();

        // Assert
        Assert.Null(firstSeen);
    }

}
