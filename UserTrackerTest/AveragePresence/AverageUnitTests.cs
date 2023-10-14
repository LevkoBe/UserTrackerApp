using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTracker;

namespace UserTracker
{
    public class CountUnitTests
    {
        [Fact]
        public void Expect_SomeNumberOfWeeks_When_UserHasThem()
        {
            // Arrange
            var userActivity1 = new UserActivity();
            userActivity1.ActivityPeriods = new List<TimePeriod>
            {
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-06-08T22:18:27.1940432+03:00"),
                    End = DateTime.Parse("2023-07-08T22:20:49.9411621+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-08-08T22:59:17.9205683+03:00"),
                    End = DateTime.Parse("2023-08-08T22:59:52.965938+03:00")
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
            // Act
            var weeksNumber = userActivity1.CountWeeks();
            // Assert
            Assert.True(weeksNumber > 0);
        }
        [Fact]
        public void Expect_SomeNumberOfDays_When_UserHasThem()
        {
            // Arrange
            var userActivity1 = new UserActivity();
            userActivity1.ActivityPeriods = new List<TimePeriod>
            {
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-06-08T22:18:27.1940432+03:00"),
                    End = DateTime.Parse("2023-07-08T22:20:49.9411621+03:00")
                },
                new TimePeriod
                {
                    Start = DateTime.Parse("2023-08-08T22:59:17.9205683+03:00"),
                    End = DateTime.Parse("2023-08-08T22:59:52.965938+03:00")
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
            // Act
            var daysNumber = userActivity1.CountDays();
            // Assert
            Assert.True(daysNumber > 0);
        }
        [Fact]
        public void Expect_Null_When_UserHasNoHistory()
        {
            // Arrange
            var userActivity1 = new UserActivity();
            userActivity1.ActivityPeriods = new List<TimePeriod> { };
            // Act
            var daysNumber = userActivity1.CountDays();
            // Assert
            Assert.Equal(daysNumber, 0);
        }
    }
}
