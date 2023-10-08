
using System.Text.Json;
using System;

namespace UserTracker
{
    public class LastSeenTest
    {
        [Fact]
        public void Expect_Online_When_UserIsOnline()
        {
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = null,
                isOnline = true
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber is online.", userState);
        }

        [Fact]
        public void Expect_JustNow_When_UpTo30Secs()
        {
            DateTime lastSeen = DateTime.Now.AddSeconds(-15);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online just now.", userState);
        }

        [Fact]
        public void Expect_LessThanAMinuteAgo_When_UpTo1Min()
        {
            DateTime lastSeen = DateTime.Now.AddSeconds(-45);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online less than a minute ago.", userState);
        }

        [Fact]
        public void Expect_LessThanAnHourAgo_When_UpTo1Hour()
        {
            DateTime lastSeen = DateTime.Now.AddMinutes(-30);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online less than an hour ago.", userState);
        }

        [Fact]
        public void Expect_AnHourAgo_When_UpTo2Hours()
        {
            DateTime lastSeen = DateTime.Now.AddHours(-1.5);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online an hour ago.", userState);
        }

        [Fact]
        public void Expect_LongTimeAgo_When_MoreThen7Days()
        {
            DateTime lastSeen = DateTime.Now.AddDays(-8);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online long time ago.", userState);
        }

        [Fact]
        public void Expect_Today_When_TodayAfter2AM()
        {
            DateTime now = DateTime.Now;
            DateTime lastSeen = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online today.", userState);
        }

        [Fact]
        public void Expect_Yesterday_When_YesterdayAfter2AM()
        {
            DateTime now = DateTime.Now;
            DateTime lastSeen = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0).AddDays(-1);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online yesterday.", userState);
        }

        [Fact]
        public void Expect_ThisWeek_When_Between2and7days()
        {
            DateTime lastSeen = DateTime.Now.AddDays(-4);
            User theUser = new User(new UserData
            {
                userId = "e13412b2-fe46-7149-6593-e47043f39c91",
                nickname = "Terry_Weber",
                firstName = "Terry",
                lastName = "Weber",
                registrationDate = "2022-10-24T17:46:53.1388008+00:00",
                lastSeenDate = lastSeen.ToString("o"), // Format DateTime to ISO 8601
                isOnline = false
            });
            var userState = theUser.ToString();
            Assert.Equal("Terry_Weber was online this week.", userState);
        }

        [Fact]
        public void Expect_AnyResult_When_FetchingData()
        {
            // Arrange
            IGetData dataProvider = new GetData();
            string apiUrl = "https://sef.podkolzin.consulting/api/users/lastSeen";
            UserLoader userLoader = new UserLoader(dataProvider, apiUrl);

            // Act
            User[] users = userLoader.GetAllUsers();

            // Assert
            Assert.NotEmpty(users);
            Assert.True(users.Length > 0);
        }

        [Fact]
        public void Expect_MoreThan200rows_When_FetchData()
        {
            // Arrange
            IGetData dataProvider = new GetData();
            string apiUrl = "https://sef.podkolzin.consulting/api/users/lastSeen";
            UserLoader userLoader = new UserLoader(dataProvider, apiUrl);

            // Act
            User[] users = userLoader.GetAllUsers();

            // Assert
            Assert.True(users.Length >= 200);
        }
    }

    public class ApiTests
    {

        [Fact]
        public void Expect_NumberofUsers_When_CorrectDateIsPassed()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/users?date=2023-10-08T22:07:06.9711678"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<UserOnlineResponse>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            int? usersOnline = jsonResponse.usersOnline;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.NotNull(usersOnline);
            Assert.NotEqual(0, usersOnline);
        }

        [Fact]
        public void Expect_Null_When_NoDataOnDate()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/users?date=2020-10-08T22:07:06.9711678"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<UserOnlineResponse>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            int? usersOnline = jsonResponse.usersOnline;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.Null(usersOnline);
            Assert.NotEqual(0, usersOnline);
        }

        public class UserOnlineResponse
        {
            public int? usersOnline { get; set; }
        }
    }

}