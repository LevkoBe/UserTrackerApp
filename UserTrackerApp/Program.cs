using System;
using System.Globalization;
using System.Threading.Tasks;

namespace UserTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Select your preferred language:");
            Console.WriteLine("1. English");
            Console.WriteLine("2. French");
            Console.WriteLine("3. Ukrainian");
            Console.WriteLine("4. Spanish");
            Console.WriteLine("5. German");
            Console.WriteLine("6. Russian");

            string cultureCode;
            int choice;

            do
            {
                Console.Write("Enter the number for your preferred language: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6);

            switch (choice)
            {
                case 1:
                    cultureCode = "en-US";
                    break;
                case 2:
                    cultureCode = "fr-FR";
                    break;
                case 3:
                    cultureCode = "uk-UA";
                    break;
                case 4:
                    cultureCode = "es-ES";
                    break;
                case 5:
                    cultureCode = "de-DE";
                    break;
                case 6:
                    cultureCode = "ru-RU";
                    break;
                default:
                    cultureCode = "en-US";
                    break;
            }

            CultureInfo.CurrentCulture = new CultureInfo(cultureCode);

            IGetData dataProvider = new GetData();

            string apiUrl = "https://sef.podkolzin.consulting/api/users/lastSeen";

            UserLoader userLoader = new UserLoader(dataProvider, apiUrl);

            var userActivityManager = new UserActivityManager(userLoader);

            await userActivityManager.StartDataFetching(TimeSpan.FromSeconds(30));

            User[] users = userLoader.GetAllUsers();

            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
            Console.WriteLine(users.Length);
        }
    }
}
