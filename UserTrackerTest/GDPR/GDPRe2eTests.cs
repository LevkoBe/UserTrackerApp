using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserTracker;
using static UserTracker.ApiTests;

namespace UserTrackerTest.GDPR
{
    public class GDPRe2eTests
    {
        [Fact]
        public void Expect_DeletingUser_When_WeForgetHim()
        {
            // Arrange
            var nickname = "Nick37";
            var filename = "..\\..\\UserTrackerApp\\forgottenUsers.json";

            var originalContent = File.ReadAllText(filename);
            var existingList = JsonSerializer.Deserialize<List<string>>(originalContent);

            IGetData dataProvider = new GetData();
            string apiUrl = "https://sef.podkolzin.consulting/api/users/lastSeen";
            UserLoader userLoader = new UserLoader(dataProvider, apiUrl);
            UserActivityManager userActivityManager = new UserActivityManager(userLoader);

            // Assert 1
            Assert.True(userActivityManager.UserExists(nickname));
            Assert.DoesNotContain(nickname, existingList!);

            // Arrange 2
            using var client = new HttpClient();

            // Act 2
            client.Send(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7215/api/user/forget?nickname=Nick37"));

            // Assert 2
            Assert.False(userActivityManager.UserExists(nickname));
            Assert.Contains(nickname, existingList!);

            // Restore the original file content
            File.WriteAllText(filename, originalContent);
        }

    }
}
