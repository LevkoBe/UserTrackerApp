using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTracker
{
    public class AverageIntegrationTests
    {
        [Fact] public void AverageMethodsIntegration()
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
            long? weeklyAverage = userActivities.GetWeeklyAverageOnlineTimeForUser(nickname);
            long? dailyAverage = userActivities.GetDailyAverageOnlineTimeForUser(nickname);
            long weekly = weeklyAverage ?? 0;
            long daily = dailyAverage ?? 0;
            var result = new Dictionary<string, long>{ { "weeklyAverage", weekly }, { "dailyAverage", daily } };

            // Assert
            Assert.True(result["weeklyAverage"] > 0);
            Assert.True(result["dailyAverage"] > 0);
        }
    }
}
