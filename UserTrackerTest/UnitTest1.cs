
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
    }

}