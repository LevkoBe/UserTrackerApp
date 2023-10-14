
using System.Text.Json;
using System;

namespace UserTracker
{

    public class ApiTests
    {

        public class UserOnline
        {
            public int? usersOnline { get; set; }
        }
        public class TotalTime
        {
            public int? totalTime { get; set; }
        }

        public class WasUserOnline
        {
            public bool? wasUserOnline { get; set; }
            public DateTime? nearestOnlineTime { get; set; }
        }

        public class WillBeOnline
        {
            public bool? willBeOnline { get; set; }
            public double? chance { get; set; }
        }

        public class AverageTime
        {
            public long? weeklyAverage { get; set; }
            public long? dailyAverage { get; set; }
        }
    }

}

// when was online should be added
// work with structures, not with urls
// check each function, each class
// for each scenario
