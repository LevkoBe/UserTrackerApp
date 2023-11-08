using System;
using System.Collections.Generic;
using Xunit;
using UserTracker;
using System.Net.Http;
using System.IO;
using System.Text.Json;

public class UserListEndpointTests
{
    [Fact]
    public void Expect_UsernameAndFirstSeenFields_When_GettingUserList()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        using var result = client.GetAsync("https://localhost:7215/api/users/list").GetAwaiter().GetResult();
        using var reader = new StreamReader(result.Content.ReadAsStream());
        var stringContent = reader.ReadToEnd();
        var jsonResponse = JsonSerializer.Deserialize<List<UserListResponse>>(stringContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        Assert.NotNull(jsonResponse);
        foreach (var user in jsonResponse)
        {
            Assert.NotNull(user.Username);
            Assert.NotNull(user.FirstSeen);
        }
    }
}
