namespace UserTracker
{
    public class ReportIntegrationTests
    {
        [Fact]
        public void Expect_ProperReport_When_ReportManagerCreates()
        {
            // Arrange
            var reportManager = new ReportManager();
            var reportConfig = new ReportConfiguration
            {
                Metrics = new List<string> { "dailyAverage", "total" },
                UserNicknames = new List<string> { "user1", "user2" }
            };

            // Act
            bool createResult = reportManager.CreateReport("SampleReport", reportConfig);
            var createdReport = reportManager.GetReport("SampleReport");

            // Assert
            Assert.True(createResult);
            Assert.NotNull(createdReport);
            Assert.Equal("SampleReport", createdReport.ReportName);
            Assert.Equal(reportConfig.Metrics, createdReport.Metrics);
            Assert.Equal(reportConfig.UserNicknames, createdReport.UserNicknames);
        }

        [Fact]
        public void Expect_GoodInteractionBetweenConfigurationsAndManager_When_TwoReportsAreCreated()
        {
            // Arrange
            var reportManager = new ReportManager();
            var reportConfig1 = new ReportConfiguration
            {
                Metrics = new List<string> { "dailyAverage", "total" },
                UserNicknames = new List<string> { "user1", "user2" }
            };

            var reportConfig2 = new ReportConfiguration
            {
                Metrics = new List<string> { "weeklyAverage", "total" },
                UserNicknames = new List<string> { "user3", "user4" }
            };

            // Act
            bool createResult1 = reportManager.CreateReport("SampleReport", reportConfig1);
            bool createResult2 = reportManager.CreateReport("SampleReport", reportConfig2);

            var reports = reportManager.GetReports();

            // Assert
            Assert.True(createResult1);
            Assert.False(createResult2);
            Assert.Single(reports);
            Assert.Equal("SampleReport", reports[0].ReportName);
        }
    }
}
