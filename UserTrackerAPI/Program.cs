using System.Globalization;
using UserTracker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

IGetData dataProvider = new GetData();
string apiUrl = "https://sef.podkolzin.consulting/api/users/lastSeen";
UserLoader userLoader = new UserLoader(dataProvider, apiUrl);

UserActivityManager userActivityManager = new UserActivityManager(userLoader);

Task.Run(async () => await userActivityManager.StartDataFetching(TimeSpan.FromSeconds(30)));



app.MapGet("/", () => userLoader.GetAllUsers());
app.MapGet("/formatted", () => userLoader.GetAllUsers().Select(user => user.ToString()));
app.MapGet("/api/stats/users", (HttpContext context) =>
{
    var date = context.Request.Query["date"];
    if (DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime dateTime))
    {
        int? usersOnline = userActivityManager.GetUserActivitiesAtDateTime(dateTime);
        if (usersOnline == 0)
        usersOnline = null;
        return Results.Json(new { usersOnline });
    }
    else
    {
        return Results.BadRequest("Invalid date parameter");
    }
});
app.MapGet("/api/stats/user", (HttpContext context) =>
{
    string nickname = context.Request.Query["nickname"];
    string dateParam = context.Request.Query["date"];

    if (DateTime.TryParseExact(dateParam, "yyyy-MM-dd-HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
    {
        var userOnlineResponse = userActivityManager.GetUserOnlineStatus(nickname, dateTime);

        if (userOnlineResponse.WasUserOnline.HasValue)
        {
            return Results.Json(userOnlineResponse);
        }
        else
        {
            return Results.NotFound("User not found.");
        }
    }
    else
    {
        return Results.BadRequest("Invalid date parameter.");
    }
});


app.Run();
