namespace LastSeenTest_goodGitStructure
{
    public class User
    {
        private UserData userData;

        public User(UserData data)
        {
            userData = data;
        }

        public string CalcTime()
        {
            if (userData.isOnline)
            {
                return "Online";
            }

            DateTime itWas = DateTime.Parse(userData.lastSeenDate).ToUniversalTime();
            DateTime itIs = DateTime.Now.ToUniversalTime();
            TimeSpan timeDifference = itIs - itWas;

            if (timeDifference <= TimeSpan.FromSeconds(30))
            {
                return "just now";
            }
            else if (timeDifference <= TimeSpan.FromMinutes(1))
            {
                return "less than a minute ago";
            }
            else if (timeDifference < TimeSpan.FromHours(1))
            {
                return "less than an hour ago";
            }
            else if (timeDifference < TimeSpan.FromHours(2))
            {
                return "an hour ago";
            }
            else if (timeDifference > TimeSpan.FromDays(7))
            {
                return "long time ago";
            }
            else if (itWas.AddHours(-2).Day == itIs.Day)
            {
                return "today";
            }
            else if (itWas.AddHours(-2).AddDays(1).Day == itIs.Day)
            {
                return "yesterday";
            }
            else if (timeDifference < TimeSpan.FromDays(7))
            {
                return "this week";
            }
            return "?";
        }

        public override string ToString()
        {
            if (userData.isOnline)
            {
                return $"{userData.nickname} is online.";
            }
            return $"{userData.nickname} was online {CalcTime()}.";
        }
    }
}
