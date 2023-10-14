using static UserTracker.ApiTests;
using System.Text.Json;

namespace UserTracker
{
    public class ApiUnitTests
    {
        [Theory]
        [InlineData("https://localhost:7215/api/stats/user/total?nickname=Doug93")]
        [InlineData("https://localhost:7215/api/stats/user/total?nickname=Nathaniel6")]
        [InlineData("https://localhost:7215/api/stats/user/total?nickname=Terry_Weber")]
        [InlineData("https://localhost:7215/api/stats/user/total?nickname=Willard66")]
        [InlineData("https://localhost:7215/api/stats/user/total?nickname=Nick37")]
        public void Expect_WorkingEndpoint_When_CorrectUrl(string url)
        {
            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, url));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            // Act
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<TotalTime>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;
            // Assert
            Assert.NotNull(jsonResponse);
        }
    }
}
