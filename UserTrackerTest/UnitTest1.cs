
using System.Text.Json;
using System;

namespace UserTracker
{

    public class ApiTests
    {

        [Fact]
        public void Expect_MoreThanOneUserOnline_When_predictAboutFuture()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/predictions/users?date=2025-12-07-22:07"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<UserOnline>(stringContent, new JsonSerializerOptions()
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
        public void Expect_wontBeOnline_When_ProbabilityIsLow()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/predictions/user?date=2023-10-08-22:18&tolerance=0,85&nickname=Doug93"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<WillBeOnline>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            bool? willBeOnline = jsonResponse.willBeOnline;
            double? chance = jsonResponse.chance;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.True(!willBeOnline);
        }

        public class UserOnline
        {
            public int? usersOnline { get; set; }
        }

        public class WasUserOnline
        {
            public bool? wasUserOnline { get; set; }
            public DateTime? nearestOnlineTime { get; set; }
        }

        public class WillBeOnline
        {
            public bool? willBeOnline { get; set; }
            public double? chance { get; set; }
        }
    }

}