using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

// SaveForgottenUsersToFile
// ForgetUserData

namespace UserTracker
{
    public class GDPRunitTests
    {
        [Theory]
        [InlineData("FirstUser")]
        [InlineData("SecondUser")]
        [InlineData("ThirdUser")]
        public void Expect_True_When_UserExists(string nickname)
        {

            // Arrange
            var userActivity1 = new UserActivity();
            userActivity1.nickname = "FirstUser";
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
            userActivity2.nickname = "SecondUser";
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
            userActivity3.nickname = "ThirdUser";
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
                {"FirstUser", userActivity1 },
                {"SecondUser", userActivity2 },
                {"Terry_Weber", userActivity1 },
                {"Willard66", userActivity1 },
                {"ThirdUser", userActivity3 },
                {"SecondNick", userActivity2 },
                {"ThirdNick", userActivity3 }
            });
            Assert.True(userActivities.UserExists(nickname));
        }

        [Theory]
        [InlineData("FirstUser")]
        [InlineData("SecondUser")]
        [InlineData("ThirdUser")]
        public void Expect_DeletedUser_When_ForgetAboutHim(string nickname)
        {

            var userActivity1 = new UserActivity();
            userActivity1.nickname = "FirstUser";
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
            userActivity2.nickname = "SecondUser";
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
            userActivity3.nickname = "ThirdUser";
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
                {"FirstUser", userActivity1 },
                {"SecondUser", userActivity2 },
                {"Terry_Weber", userActivity1 },
                {"Willard66", userActivity1 },
                {"ThirdUser", userActivity3 },
                {"SecondNick", userActivity2 },
                {"ThirdNick", userActivity3 }
            });

            Assert.True(userActivities.UserExists(nickname));
            userActivities.ForgetUserData(nickname);
            Assert.False(userActivities.UserExists(nickname));

        }

        [Theory]
        [InlineData("FirstUser")]
        [InlineData("SecondUser")]
        [InlineData("ThirdUser")]
        public void Expect_ThatFileContainsForgottenUser_When_HeBecomesForgotten(string nickname)
        {
            // Arrange
            var userActivity1 = new UserActivity();
            userActivity1.nickname = "FirstUser";
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
            userActivity2.nickname = "SecondUser";
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
            userActivity3.nickname = "ThirdUser";
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
                {"FirstUser", userActivity1 },
                {"SecondUser", userActivity2 },
                {"Terry_Weber", userActivity1 },
                {"Willard66", userActivity1 },
                {"ThirdUser", userActivity3 },
                {"SecondNick", userActivity2 },
                {"ThirdNick", userActivity3 }
            });
            userActivities.forgottenUsers = new List<string>();


            Assert.DoesNotContain(nickname, userActivities.forgottenUsers);
            userActivities.ForgetUserData(nickname);
            Assert.Contains(nickname, userActivities.forgottenUsers);
        }
    }
}
