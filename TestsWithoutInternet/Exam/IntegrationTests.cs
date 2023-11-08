using System;
using System.Collections.Generic;
using UserTracker;
using Xunit;

public class IntegrationTests
{
    [Fact]
    public void GetUserList_ReturnsUserListWithFirstSeen()
    {
        // Arrange
        var userActivities = new MockUserActivities();

        // Act
        var userList = userActivities.GetUserList();

        // Assert
        Assert.NotNull(userList);
        Assert.NotEmpty(userList);

        foreach (var user in userList)
        {
            Assert.NotNull(user.Username);
            Assert.NotNull(user.FirstSeen);
        }
    }

    [Fact]
    public void GetUserList_WhenNoUserActivity_ReturnsEmptyList()
    {
        // Arrange
        var userActivityManager = new UserActivityManager(null);

        // Act
        var userList = userActivityManager.GetUserList();

        // Assert
        Assert.NotNull(userList);
        Assert.Empty(userList);
    }
}
