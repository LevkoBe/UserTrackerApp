using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace UserTracker
{
    public class ReportCreationTest
    {
        [Fact]
        public async void Expect_ReportCreatedSuccessfully()
        {
            // Arrange
            using var client = new HttpClient();
            var reportConfig = new
            {
                metrics = new[] { "dailyAverage", "total", "weeklyAverage" },
                users = new[]
                {
                "user1",
                "user2",
                "user3"
            }
            };

            var reportName = "TestReport"; // Provide a unique report name
            var reportConfigJson = JsonSerializer.Serialize(reportConfig);
            var content = new StringContent(reportConfigJson, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"https://localhost:7215/api/report/{reportName}", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }

}