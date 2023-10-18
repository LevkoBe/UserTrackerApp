namespace UserTracker
{

    public class ReportManagerTests
    {
        [Fact]
        public void Expect_NewReport_When_CreateReport()
        {
            // Arrange
            var reportManager = new ReportManager();
            var reportConfig = new ReportConfiguration
            {
                Metrics = new List<string> { "dailyAverage", "total" },
                UserNicknames = new List<string> { "user1", "user2" }
            };

            // Act
            bool result = reportManager.CreateReport("SampleReport", reportConfig);

            // Assert
            Assert.True(result);
            var reports = reportManager.GetReports();
            Assert.Single(reports);
            Assert.Equal("SampleReport", reports[0].ReportName);
        }

        [Fact]
        public void Expect_OneReport_When_TwoIdenticalCreated()
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
            bool result1 = reportManager.CreateReport("SampleReport", reportConfig1);
            bool result2 = reportManager.CreateReport("SampleReport", reportConfig2);

            // Assert
            Assert.True(result1);
            Assert.False(result2);
            var reports = reportManager.GetReports();
            Assert.Single(reports);
            Assert.Equal("SampleReport", reports[0].ReportName);
        }

        [Fact]
        public void Expect_Report_When_GetReport()
        {
            // Arrange
            var reportManager = new ReportManager();
            var reportConfig = new ReportConfiguration
            {
                Metrics = new List<string> { "dailyAverage", "total" },
                UserNicknames = new List<string> { "user1", "user2" }
            };
            reportManager.CreateReport("SampleReport", reportConfig);

            // Act
            var report = reportManager.GetReport("SampleReport");

            // Assert
            Assert.NotNull(report);
            Assert.Equal("SampleReport", report.ReportName);
        }

        [Fact]
        public void Expect_Null_When_GetReportThatDoesntExist()
        {
            // Arrange
            var reportManager = new ReportManager();

            // Act
            var report = reportManager.GetReport("NonExistentReport");

            // Assert
            Assert.Null(report);
        }
    }
}

