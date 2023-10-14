namespace UserTracker
{
    public class FindAverageUnitTests
    {
        [Fact]
        public void Expect_AverageTimeOfWeeks_When_UserHasThem()
        {
            // Arrange
            var nickname = "Doug93";
            var userActivity1 = new UserActivity();
            userActivity1.nickname = nickname;
            userActivity1.ActivityPeriods = new List<TimePeriod>
            {
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T22:18:27.1940432+03:00"),
                    End = DateTime.Parse("2023-10-08T22:20:49.9411621+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T22:59:17.9205683+03:00"),
                    End = DateTime.Parse("2023-10-08T22:59:52.965938+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T23:00:27.9960034+03:00"),
                    End = DateTime.Parse("2023-10-08T23:35:57.292808+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T23:38:17.3219311+03:00"),
                    End = DateTime.Parse("2023-10-08T23:40:09.6822659+03:00")
                }
            };
            UserActivityManager userActivities = new UserActivityManager(
                null,
                new Dictionary<string, UserActivity>
            {
                {"Doug93", userActivity1 }
            });
            // Act
            var weeksAverage = userActivities.GetWeeklyAverageOnlineTimeForUser(nickname);
            // Assert
            Assert.True(weeksAverage > 0);
        }
        [Fact]
        public void Expect_AverageTimeOfDays_When_UserHasThem()
        {
            // Arrange
            var nickname = "Doug93";
            var userActivity1 = new UserActivity();
            userActivity1.nickname = nickname;
            userActivity1.ActivityPeriods = new List<TimePeriod>
            {
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T22:18:27.1940432+03:00"),
                    End = DateTime.Parse("2023-10-08T22:20:49.9411621+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T22:59:17.9205683+03:00"),
                    End = DateTime.Parse("2023-10-08T22:59:52.965938+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T23:00:27.9960034+03:00"),
                    End = DateTime.Parse("2023-10-08T23:35:57.292808+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T23:38:17.3219311+03:00"),
                    End = DateTime.Parse("2023-10-08T23:40:09.6822659+03:00")
                }
            };
            UserActivityManager userActivities = new UserActivityManager(
                null,
                new Dictionary<string, UserActivity>
            {
                {"Doug93", userActivity1 }
            });
            // Act
            var dailyAverage = userActivities.GetDailyAverageOnlineTimeForUser(nickname);
            // Assert
            Assert.True(dailyAverage > 0);
        }
        [Fact]
        public void Expect_Null_When_UserHasNoHistory()
        {
            // Arrange
            var nickname = "Doug93";
            var userActivity1 = new UserActivity();
            userActivity1.nickname = nickname;
            userActivity1.ActivityPeriods = new List<TimePeriod> { };
            UserActivityManager userActivities = new UserActivityManager(
                null,
                new Dictionary<string, UserActivity>
            {
                {"Doug93", userActivity1 }
            });
            // Act
            var dailyAverage = userActivities.GetDailyAverageOnlineTimeForUser(nickname);
            // Assert
            Assert.Null(dailyAverage);
        }
    }
}
