namespace LastSeenTest_goodGitStructure
{
    public class UserLoader
    {
        private readonly IGetData _data;
        private readonly string _url;

        public UserLoader(IGetData data, string url)
        {
            _data = data;
            _url = url;
        }
        public User[] GetAllUsers()
        {
            List<User> users = new();
            while (true)
            {
                var result = _data.GetResponse(_url + $"?offset={users.Count}");
                if (result.data.Count == 0) { break; }

                foreach (var userData in result.data)
                {
                    users.Add(new User(userData));
                }
            }
            return users.ToArray();
        }

    }
}
