using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserTracker
{
    public class UserReportData
    {
        private readonly UserActivityManager userActivityManager;
        private readonly Report report;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public UserReportData(UserActivityManager userActivityManager, Report report, DateTime fromDate, DateTime toDate)
        {
            this.userActivityManager = userActivityManager;
            this.report = report;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        public async Task<UserMetrics> GenerateReportData(string userNickname)
        {
            var userReportData = new UserMetrics
            {
                Nickname = userNickname,
                Metrics = new List<MetricBase>()
            };

            foreach (var metric in report.Metrics)
            {
                switch (metric)
                {
                    case "dailyAverage":
                        long? dailyAverage = userActivityManager.GetDailyAverageOnlineTimeForUser(userNickname, fromDate, toDate);
                        userReportData.Metrics.Add(new DailyAverageMetric { DailyAverage = dailyAverage });
                        break;
                    case "weeklyAverage":
                        long? weeklyAverage = userActivityManager.GetWeeklyAverageOnlineTimeForUser(userNickname, fromDate, toDate);
                        userReportData.Metrics.Add(new WeeklyAverageMetric { WeeklyAverage = weeklyAverage });
                        break;
                    case "total":
                        long? total = userActivityManager.GetTotalOnlineTimeForUser(userNickname, fromDate, toDate);
                        userReportData.Metrics.Add(new TotalMetric { Total = total });
                        break;
                    case "min":
                        long? min = userActivityManager.GetMinimumDailyOnlineTimeForUser(userNickname, fromDate, toDate);
                        userReportData.Metrics.Add(new MinMetric { Min = min });
                        break;
                    case "max":
                        long? max = userActivityManager.GetMaximumDailyOnlineTimeForUser(userNickname, fromDate, toDate);
                        userReportData.Metrics.Add(new MaxMetric { Max = max });
                        break;
                    default:
                        break;
                }
            }

            return userReportData;
        }
    }

    public class UserMetrics
    {
        public string Nickname { get; set; }
        public List<MetricBase> Metrics { get; set; }
    }

    public class MetricBase { }

    public class DailyAverageMetric : MetricBase
    {
        public long? DailyAverage { get; set; }
    }

    public class WeeklyAverageMetric : MetricBase
    {
        public long? WeeklyAverage { get; set; }
    }

    public class TotalMetric : MetricBase
    {
        public long? Total { get; set; }
    }

    public class MinMetric : MetricBase
    {
        public long? Min { get; set; }
    }

    public class MaxMetric : MetricBase
    {
        public long? Max { get; set; }
    }
}
