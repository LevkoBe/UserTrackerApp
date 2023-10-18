namespace UserTracker
{
    public class ReportTests
    {
        [Fact]
        public void Expect_ProperProperties_When_ReportIsCreated()
        {
            // Arrange
            string reportName = "SampleReport";
            var metrics = new List<string> { "dailyAverage", "total" };
            var userNicknames = new List<string> { "user1", "user2" };

            // Act
            var report = new Report(reportName, metrics, userNicknames);

            // Assert
            Assert.Equal(reportName, report.ReportName);
            Assert.Equal(metrics, report.Metrics);
            Assert.Equal(userNicknames, report.UserNicknames);
        }
    }
}

