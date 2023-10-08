using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UserTracker
{
    public class UserActivityManager
    {
        private readonly UserLoader _userLoader;
        private readonly Dictionary<string, UserActivity> _userActivities;

        public UserActivityManager(UserLoader userLoader)
        {
            _userLoader = userLoader;
            _userActivities = new Dictionary<string, UserActivity>();

            LoadUserActivityFromJson("D:\\C#Projects\\UserTrackerApp\\UserTrackerApp\\userActivities.json");
        }

        public async Task StartDataFetching(TimeSpan fetchInterval)
        {
            while (true)
            {
                User[] users = _userLoader.GetAllUsers();

                FetchAndUpdateUserActivities();

                SaveUserActivityToJson("D:\\C#Projects\\UserTrackerApp\\UserTrackerApp\\userActivities.json");

                await Task.Delay(fetchInterval);
            }
        }

        public int GetUserActivitiesAtDateTime(DateTime dateTime)
        {
            int onlineUserCount = _userActivities.Values
                .Count(userActivity => userActivity.IsOnlineAtDateTime(dateTime));

            return onlineUserCount;
        }

        private void FetchAndUpdateUserActivities()
        {
            User[] users = _userLoader.GetAllUsers();

            foreach (var user in users)
            {
                if (!_userActivities.ContainsKey(user.userData.nickname!))
                {
                    _userActivities[user.userData.nickname!] = new UserActivity();
                    _userActivities[user.userData.nickname!].nickname = user.userData.nickname!;
                    _userActivities[user.userData.nickname!].isOnline = false;
                    _userActivities[user.userData.nickname!].ActivityPeriods = new List<TimePeriod>();
                }

                var userActivity = _userActivities[user.userData.nickname!];

                if (user.userData.isOnline)
                {
                    userActivity.SetOnline();
                }
                else
                {
                    userActivity.SetOffline();
                }
            }

        }

        public void SaveUserActivityToJson(string filePath)
        {
            var userActivityList = _userActivities.Values.ToList();
            var json = JsonSerializer.Serialize(userActivityList, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
        }

        public void LoadUserActivityFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true, // Include non-public fields
                };

                var userActivityList = JsonSerializer.Deserialize<List<UserActivity>>(json, jsonOptions);


                _userActivities.Clear();
                foreach (var userActivity in userActivityList)
                {
                    _userActivities[userActivity.nickname] = userActivity;
                }
            }
        }

    }

}
