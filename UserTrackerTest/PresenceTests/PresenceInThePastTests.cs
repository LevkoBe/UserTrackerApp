using System.Text.Json;
using static UserTracker.ApiTests;

namespace UserTracker
{
    public class PresenceInThePastTests
    {

        [Fact]
        public void Expect_NumberofUsers_When_CorrectDateIsPassed()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/users?date=2023-10-08T22:07:06.9711678"));
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
        public void Expect_Null_When_NoDataOnDate()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/users?date=2020-10-08T22:07:06.9711678"));
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
            Assert.Null(usersOnline);
            Assert.NotEqual(0, usersOnline);
        }
    }
}
