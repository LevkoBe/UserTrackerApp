using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static UserTracker.ApiTests;

namespace UserTracker
{
    public class AverageE2ETests
    {
        [Theory]
        [InlineData("Doug93")]
        [InlineData("Nathaniel6")]
        [InlineData("Terry_Weber")]
        [InlineData("Willard66")]
        [InlineData("Nick37")]
        public void Expect_ManySecondsOnline_When_askAboutUser(string nickname)
        {
            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7215/api/stats/user/average?nickname={nickname}"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<AverageTime>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            long? weeklyAverage = jsonResponse.weeklyAverage;
            long? dailyAverage = jsonResponse.dailyAverage;

            // Assert
            Assert.True(weeklyAverage > 0);
            Assert.True(dailyAverage > 0);
        }
    }
}
