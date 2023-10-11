using System.Text.Json;
using static UserTracker.ApiTests;

namespace UserTracker
{
    public class UserWasTests
    {

        [Fact]
        public void Expect_wasUserOnline_When_userWasOnlineAtSpecifiedTime()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/user?date=2023-10-08-22:19&nickname=Doug93"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<WasUserOnline>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            bool? wasUserOnline = jsonResponse.wasUserOnline;
            DateTime? nearestOnlineTime = jsonResponse.nearestOnlineTime;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.Null(nearestOnlineTime);
            Assert.Equal(wasUserOnline, true);
        }

        [Fact]
        public void Expect_nearestOnlineTime_When_userWasNotOnlineAtSpecifiedTime()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/user?date=2023-10-08-22:18&nickname=Doug93"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<WasUserOnline>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            bool? wasUserOnline = jsonResponse.wasUserOnline;
            DateTime? nearestOnlineTime = jsonResponse.nearestOnlineTime;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.Equal(nearestOnlineTime, DateTime.Parse("2023-10-08T22:18:27.1940432+03:00"));
            Assert.Equal(wasUserOnline, false);
        }
    }
}
