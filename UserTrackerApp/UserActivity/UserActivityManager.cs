﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UserTracker
{
    public class UserActivityManager
    {
        private readonly UserLoader? _userLoader;
        private readonly Dictionary<string, UserActivity> _userActivities;
        public List<string> forgottenUsers = new List<string>();

        public UserActivityManager(UserLoader? userLoader = null, Dictionary<string, UserActivity>? userActivities = null)
        {
            forgottenUsers = LoadForgottenUsersFromFile("forgottenUsers.json");
            _userLoader = userLoader;
            if (userActivities == null)
            {
                _userActivities = new Dictionary<string, UserActivity>();
                LoadUserActivityFromJson("C:\\FromDD\\C#Projects\\UserTrackerApp\\UserTrackerApp\\userActivities.json");
            }
            else
            {
                _userActivities = userActivities!;
            }
        }

        public bool UserExists(string nickname)
        {
            return _userActivities.ContainsKey(nickname);
        }

        public void ForgetUserData(string nickname)
        {
            if (_userActivities.ContainsKey(nickname))
            {
                _userActivities.Remove(nickname);
                forgottenUsers.Add(nickname);
                SaveForgottenUsersToFile("forgottenUsers.json");
            }
        }



        public long? GetWeeklyAverageOnlineTimeForUser(string nickname)
        {
            long totalTime = GetTotalOnlineTimeForUser(nickname);
            long totalWeeks = CountWeeksUserOnline(nickname);

            if (totalWeeks > 0)
            {
                return totalTime / totalWeeks;
            }

            return null;
        }

        public long? GetDailyAverageOnlineTimeForUser(string nickname)
        {
            long totalTime = GetTotalOnlineTimeForUser(nickname);
            long totalDays = CountWeeksUserOnline(nickname);  

            if (totalDays > 0)
            {
                return totalTime / totalDays;
            }

            return null;
        }


        public long GetTotalOnlineTimeForUser(string nickname)
        {
            if (_userActivities.TryGetValue(nickname, out var userActivity))
            {
                long totalTime = 0;

                foreach (var timePeriod in userActivity.ActivityPeriods)
                {
                    if (timePeriod.End != default)
                    {
                        TimeSpan periodTime = timePeriod.End - timePeriod.Start;
                        totalTime += (long)periodTime.TotalSeconds;
                    }
                }

                return totalTime;
            }

            return 0; // User not found or has no online activity.
        }


        public int? PredictUsersOnline(DateTime futureDate)
        {
            DayOfWeek futureDayOfWeek = futureDate.DayOfWeek;
            Dictionary<DayOfWeek, int> usersPerDay = new Dictionary<DayOfWeek, int>();

            foreach (var userActivity in _userActivities.Values)
            {
                foreach (var timePeriod in userActivity.ActivityPeriods)
                {
                    if (timePeriod.Start.DayOfWeek == futureDayOfWeek &&
                        timePeriod.Start.TimeOfDay <= futureDate.TimeOfDay &&
                        timePeriod.End.TimeOfDay >= futureDate.TimeOfDay)
                    {
                        if (!usersPerDay.ContainsKey(futureDayOfWeek))
                        {
                            usersPerDay[futureDayOfWeek] = 1;
                        }
                        else
                        {
                            usersPerDay[futureDayOfWeek]++;
                        }
                    }
                }
            }

            int totalUsers = usersPerDay.Values.Sum();
            int numberOfDays = usersPerDay.Count;
            int averageUsers = numberOfDays > 0 ? totalUsers / numberOfDays : 0;

            return averageUsers > 0 ? averageUsers : null;
        }

        public bool PredictUserOnline(string nickname, DateTime futureDate, double tolerance, out double onlineChance)
        {
            if (_userActivities.TryGetValue(nickname, out UserActivity userActivity))
            {
                int founds = 0;
                int weeks = 0;
                int lastWeek = -1;

                foreach (var timePeriod in userActivity.ActivityPeriods)
                {
                    if (timePeriod.Start.DayOfWeek == futureDate.DayOfWeek &&
                        timePeriod.Start.TimeOfDay <= futureDate.TimeOfDay &&
                        timePeriod.End.TimeOfDay >= futureDate.TimeOfDay)
                    {
                        founds++;
                    }

                    int currentWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(timePeriod.Start, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                    if (currentWeek != lastWeek)
                    {
                        weeks++;
                        lastWeek = currentWeek;
                    }
                }

                onlineChance = weeks == 0 ? 0.0 : (double)founds / weeks;
                bool willBeOnline = onlineChance >= tolerance;

                return willBeOnline;
            }

            onlineChance = 0.0;
            return false;
        }


        public UserOnlineResponse GetUserOnlineStatus(string nickname, DateTime dateTime)
        {
            if (_userActivities.TryGetValue(nickname, out var userActivity))
            {
                bool wasUserOnline = userActivity.IsOnlineAtDateTime(dateTime);
                DateTime? nearestOnlineTime = userActivity.GetNearestOnlineTime(dateTime);

                return new UserOnlineResponse
                {
                    WasUserOnline = wasUserOnline,
                    NearestOnlineTime = nearestOnlineTime
                };
            }

            // If user not found, return null for both fields
            return new UserOnlineResponse
            {
                WasUserOnline = null,
                NearestOnlineTime = null
            };
        }

        public async Task StartDataFetching(TimeSpan fetchInterval)
        {
            while (true)
            {
                FetchAndUpdateUserActivities();

                SaveUserActivityToJson("C:\\FromDD\\C#Projects\\UserTrackerApp\\UserTrackerApp\\userActivities.json");

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
                if (forgottenUsers.Contains(user.userData.nickname!))
                {
                    continue;
                }
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

        private void SaveForgottenUsersToFile(string filePath)
        {
            var json = JsonSerializer.Serialize(forgottenUsers, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
        }

        private List<string> LoadForgottenUsersFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<string>>(json);
            }

            return new List<string>();
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
                    IncludeFields = true,
                };

                var userActivityList = JsonSerializer.Deserialize<List<UserActivity>>(json, jsonOptions);


                _userActivities.Clear();
                foreach (var userActivity in userActivityList)
                {

                    if (forgottenUsers.Contains(userActivity.nickname!))
                    {
                        continue;
                    }
                    _userActivities[userActivity.nickname] = userActivity;
                }
            }
        }

        public int CountWeeksUserOnline(string nickname)
        {
            if (_userActivities.TryGetValue(nickname, out var userActivity))
            {
                return userActivity.CountWeeks();
            }

            return 0;
        }

        public int CountDaysUserOnline(string nickname)
        {
            if (_userActivities.TryGetValue(nickname, out var userActivity))
            {
                return userActivity.CountDays();
            }

            return 0;
        }


    }




    public class UserOnlineResponse
    {
        public bool? WasUserOnline { get; set; }
        public DateTime? NearestOnlineTime { get; set; }
    }

}
