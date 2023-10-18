namespace UserTracker
{
    public class Report
    {
        public string ReportName { get; }
        public List<string> Metrics { get; }
        public List<string> UserNicknames { get; }

        public Report(string reportName, List<string> metrics, List<string> userNicknames)
        {
            ReportName = reportName;
            Metrics = metrics;
            UserNicknames = userNicknames;
        }
    }


}
