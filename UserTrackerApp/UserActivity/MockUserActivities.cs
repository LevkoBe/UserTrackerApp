namespace UserTracker
{
    public class MockUserActivities : UserActivityManager
    {
        public MockUserActivities() : base(null, new Dictionary<string, UserActivity>
    {
        {
            "Doug93", new UserActivity
            {
                nickname = "Doug93",
                ActivityPeriods = new List<TimePeriod>
                {
                    new TimePeriod
                    {
                        Start = DateTime.Parse("2023-10-08T22:18:27.1940432+03:00"),
                        End = DateTime.Parse("2023-10-08T22:20:49.9411621+03:00")
                    },
                    new TimePeriod
                    {
                        Start = DateTime.Parse("2023-10-08T22:59:17.9205683+03:00"),
                        End = DateTime.Parse("2023-10-08T22:59:52.965938+03:00")
                    },
                    new TimePeriod
                    {
                        Start = DateTime.Parse("2023-10-08T23:00:27.9960034+03:00"),
                        End = DateTime.Parse("2023-10-08T23:35:57.292808+03:00")
                    },
                    new TimePeriod
                    {
                        Start = DateTime.Parse("2023-10-08T23:38:17.3219311+03:00"),
                        End = DateTime.Parse("2023-10-08T23:40:09.6822659+03:00")
                    }
                }
            }
        }
    })
        {
        }
    }

}
