using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static UserTracker.ApiTests;

namespace UserTrackerTest.CountTotalTimeTests
{
    public class CountSecondsForUserTests
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
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/stats/user/total?nickname={nickname}"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<UserOnline>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Act
            int? secondsTotally = jsonResponse.usersOnline;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.NotNull(secondsTotally);
            Assert.NotEqual(0, secondsTotally);
        }


        [Fact]
        public void Expect_Null_When_askAboutNonExistingUser()
        {

            // Arrange
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, "https://localhost:7215/api/predictions/user?date=2023-10-08-22:18&tolerance=0,85&nickname=NonExisting"));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            var jsonResponse = JsonSerializer.Deserialize<UserOnline>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;


            // Act
            int? secondsTotally = jsonResponse.usersOnline;

            // Assert
            Assert.NotEmpty(stringContent);
            Assert.Null(secondsTotally);
        }
    }
}
