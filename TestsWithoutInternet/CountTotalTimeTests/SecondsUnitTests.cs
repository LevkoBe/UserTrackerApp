using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTracker
{
    public class SecondsUnitTests
    {
        [Theory]
        [InlineData("Doug93", true)]
        [InlineData("Nathaniel6", true)]
        [InlineData("Nathan", false)]
        [InlineData("Terry_Weber", true)]
        [InlineData("Terry", false)]
        [InlineData("Willard66", true)]
        [InlineData("Nick37", true)]
        public void Expect_ManySecondsOnline_When_askAboutUser(string nickname, bool working)
        {
            // Arrange
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
            var userActivity2 = new UserActivity();
            userActivity2.nickname = "SecondNick";
            userActivity2.ActivityPeriods = new List<TimePeriod>
            {
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T22:18:27.1940432+03:00"),
                    End = DateTime.Parse("2023-10-08T22:20:49.9411621+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-10-08T23:38:17.3219311+03:00"),
                    End = DateTime.Parse("2023-10-08T23:40:09.6822659+03:00")
                }
            };
            var userActivity3 = new UserActivity();
            userActivity3.nickname = "ThirdNick";
            userActivity3.ActivityPeriods = new List<TimePeriod>
            {
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
                {"Doug93", userActivity1 },
                {"Nathaniel6", userActivity1 },
                {"Terry_Weber", userActivity1 },
                {"Willard66", userActivity1 },
                {"Nick37", userActivity1 },
                {"SecondNick", userActivity2 },
                {"ThirdNick", userActivity3 }
            });
            // Act
            long? secondsTotally = userActivities.GetTotalOnlineTimeForUser(nickname);

            // Assert
            Assert.NotNull(secondsTotally);
            Assert.Equal(secondsTotally > 0, working);
        }
    }
}
