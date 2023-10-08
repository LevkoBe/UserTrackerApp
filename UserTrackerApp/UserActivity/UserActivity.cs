using System;
using System.Collections.Generic;
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
