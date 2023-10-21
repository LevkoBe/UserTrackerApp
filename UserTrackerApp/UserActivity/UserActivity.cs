using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTracker
{
    public class TimePeriod
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class UserActivity
    {
        public string nickname { get; set; }
        public bool isOnline { get; set; }
        public List<TimePeriod> ActivityPeriods { get; set; }

        public UserActivity()
        {
            // nothing
        }

        public DateTime? GetNearestOnlineTime(DateTime dateTime)
        {
            DateTime? nearestOnlineTime = null;
            TimeSpan nearestTimeDifference = TimeSpan.MaxValue;

            foreach (var timePeriod in ActivityPeriods)
            {
                if (dateTime >= timePeriod.Start && dateTime <= timePeriod.End)
                {
                    return null;
                }

                TimeSpan startDifference = dateTime - timePeriod.Start;
                TimeSpan endDifference = dateTime - timePeriod.End;

                if (Math.Abs(startDifference.TotalSeconds) < nearestTimeDifference.TotalSeconds)
                {
                    nearestTimeDifference = startDifference;
                    nearestOnlineTime = timePeriod.Start;
                }

                if (Math.Abs(endDifference.TotalSeconds) < nearestTimeDifference.TotalSeconds)
                {
                    nearestTimeDifference = endDifference;
                    nearestOnlineTime = timePeriod.End;
                }
            }

            return nearestOnlineTime;
        }


        public void SetOnline()
        {
            if (!isOnline)
            {
                isOnline = true;
                ActivityPeriods.Add(new TimePeriod { Start = DateTime.Now });
            }
        }

        public int CountWeeks(DateTime? fromDate = null, DateTime? toDate = null)
        {
            HashSet<int> weeks = new HashSet<int>();

            foreach (var timePeriod in ActivityPeriods)
            {
                if (timePeriod.End != default)
                {
                    DateTime current = timePeriod.Start;
                    while (current <= timePeriod.End)
                    {
                        if ((!fromDate.HasValue || current >= fromDate.Value) &&
                            (!toDate.HasValue || current <= toDate.Value))
                        {
                            int week = GetIso8601WeekOfYear(current);
                            weeks.Add(week);
                        }
                        current = current.AddDays(1);
                    }
                }
            }

            return weeks.Count;
        }

        public int CountDays(DateTime? fromDate = null, DateTime? toDate = null)
        {
            HashSet<DateTime> days = new HashSet<DateTime>();

            foreach (var timePeriod in ActivityPeriods)
            {
                if (timePeriod.End != default)
                {
                    DateTime current = timePeriod.Start;
                    while (current.Date <= timePeriod.End.Date)
                    {
                        if ((!fromDate.HasValue || current >= fromDate.Value) &&
                            (!toDate.HasValue || current <= toDate.Value))
                        {
                            days.Add(current.Date);
                        }
                        current = current.AddDays(1);
                    }
                }
            }

            return days.Count;
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public void SetOffline()
        {
            if (isOnline)
            {
                isOnline = false;
                var lastTimePeriod = ActivityPeriods[ActivityPeriods.Count - 1];
                lastTimePeriod.End = DateTime.Now; // .ToUniversalTime() ?
            }
        }

        public bool IsOnlineAtDateTime(DateTime dateTime)
        {
            foreach (var timePeriod in ActivityPeriods)
            {
                if (dateTime >= timePeriod.Start && dateTime <= timePeriod.End)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
