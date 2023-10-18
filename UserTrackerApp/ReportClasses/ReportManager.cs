namespace UserTracker
{
    public class ReportManager
    {
        private readonly List<Report> reports;

        public ReportManager()
        {
            reports = new List<Report>();
        }

        public bool CreateReport(string reportName, ReportConfiguration reportConfig)
        {
            if (reports.Any(r => r.ReportName == reportName))
            {
                return false;
            }

            var report = new Report(reportName, reportConfig.Metrics, reportConfig.UserNicknames);
            reports.Add(report);
            return true;
        }

        public List<Report> GetReports()
        {
            return reports;
        }

        public Report GetReport(string reportName)
        {
            return reports.FirstOrDefault(r => r.ReportName == reportName);
        }
    }

}
