using System;
using UserTracker;
using Xunit;

public class UserReportDataTests
{
    [Fact]
    public async void Expect_UserReportDataWithTotalAndDaily_When_CreateReportForUser()
    {
        // Arrange
        var userActivities = new MockUserActivities();
        var reportManager = new ReportManager();
        var reportConfig = new ReportConfiguration
        {
            Metrics = new List<string> { "dailyAverage", "total" },
            UserNicknames = new List<string> { "Doug93" }
        };

        bool createResult = reportManager.CreateReport("SampleReport", reportConfig);
        var createdReport = reportManager.GetReport("SampleReport");

        var fromDate = DateTime.Parse("2023-10-08");
        var toDate = DateTime.Parse("2023-10-09");
        var userReportData = new UserReportData(userActivities, createdReport, fromDate, toDate);

        // Act
        var result = await userReportData.GenerateReportData("Doug93");

        // Assert
        Assert.True(createResult);
        Assert.Equal("Doug93", result.Nickname);
        Assert.NotNull(result.Metrics);
        Assert.Equal(2, result.Metrics.Count);

        var dailyAverageMetric = result.Metrics[0] as DailyAverageMetric;
        Assert.NotNull(dailyAverageMetric);
        Assert.True(dailyAverageMetric.DailyAverage >= 0);

        var totalMetric = result.Metrics[1] as TotalMetric;
        Assert.NotNull(totalMetric);
        Assert.True(totalMetric.Total >= 0);
    }
}
