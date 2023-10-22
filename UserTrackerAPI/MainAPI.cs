using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using UserTracker;
using System.Text;

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

// await Task.Run(async () => await userActivityManager.StartDataFetching(TimeSpan.FromSeconds(30))); // file is already too big
ReportManager reportManager = new();


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
app.MapGet("/api/predictions/users", (HttpContext context) =>
{
    var dateStr = context.Request.Query["date"].ToString();

    if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd-HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime futureDate))
    {
        var predictedUsers = userActivityManager.PredictUsersOnline(futureDate);
        return Results.Json(new { usersOnline = predictedUsers });
    }
    else
    {
        return Results.BadRequest("Invalid date parameter");
    }
});
app.MapGet("/api/predictions/user", (HttpContext context) =>
{
    var dateStr = context.Request.Query["date"].ToString();
    var toleranceStr = context.Request.Query["tolerance"].ToString();
    var nickname = context.Request.Query["nickname"].ToString();

    if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd-HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime futureDate))
    {
        if (double.TryParse(toleranceStr, out double tolerance))
        {
            bool willBeOnline = userActivityManager.PredictUserOnline(nickname, futureDate, tolerance, out double onlineChance);
            return Results.Json(new { willBeOnline, onlineChance });
        }
        else
        {
            return Results.BadRequest("Invalid date or tolerance parameter");
        }
    }
    else
    {
        return Results.BadRequest("Invalid date or tolerance parameter");
    }

});
app.MapGet("/api/stats/user/total", (HttpContext context) =>
{
    var nickname = context.Request.Query["nickname"].ToString();

    if (!string.IsNullOrEmpty(nickname))
    {
        long totalTime = userActivityManager.GetTotalOnlineTimeForUser(nickname);
        return Results.Json(new { totalTime });
    }
    else
    {
        return Results.BadRequest("Invalid nickname parameter");
    }
});
app.MapGet("/api/stats/user/average", (HttpContext context) =>
{
    var nickname = context.Request.Query["nickname"].ToString();

    if (!string.IsNullOrEmpty(nickname))
    {
        long? weeklyAverage = userActivityManager.GetWeeklyAverageOnlineTimeForUser(nickname);
        long? dailyAverage = userActivityManager.GetDailyAverageOnlineTimeForUser(nickname);
        return Results.Json(new { weeklyAverage, dailyAverage });
    }
    else
    {
        return Results.BadRequest("Invalid nickname parameter");
    }
});
app.MapGet("/api/user/forget", (HttpContext context) =>
{   
    var nickname = context.Request.Query["nickname"].ToString();

    if (userActivityManager.UserExists(nickname))
    {
        userActivityManager.ForgetUserData(nickname);
        return Results.Json(new { userId = nickname });
    }
    else
    {
        return Results.Json(new { message = "User not found" });
    }
});
app.MapPost("/api/report/{reportName}", async (HttpContext context) =>
{
    var reportName = context.Request.RouteValues["reportName"].ToString();
    using var reader = new StreamReader(context.Request.Body);
    var requestBody = await reader.ReadToEndAsync();
    if (!string.IsNullOrWhiteSpace(requestBody))
    {
        try
        {
            var reportConfig = JsonSerializer.Deserialize<ReportConfiguration>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (reportConfig != null)
            {
                var success = reportManager.CreateReport(reportName, reportConfig);

                if (success)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest("Report with the same name already exists.");
                }
            }
        }
        catch (JsonException)
        {
            return Results.BadRequest("Invalid report configuration.");
        }
    }

    return Results.BadRequest("Invalid or empty report configuration.");
});


app.MapGet("/api/report/{reportName}", async (HttpContext context) =>
{
    var reportName = context.Request.RouteValues["reportName"].ToString();
    var fromStr = context.Request.Query["from"].ToString();
    var toStr = context.Request.Query["to"].ToString();

    if (DateTime.TryParseExact(fromStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate) &&
        DateTime.TryParseExact(toStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
    {
        var report = reportManager.GetReport(reportName);

        if (report != null)
        {
            var reportData = new List<object>();
            var userReportDataGenerator = new UserReportData(userActivityManager, report, fromDate, toDate);
            foreach (var userNickname in report.UserNicknames)
            {
                var userReportData = await userReportDataGenerator.GenerateReportData(userNickname);
                reportData.Add(userReportData);
            }

            return Results.Json(reportData);
        }
        else
        {
            return Results.NotFound("Report not found.");
        }
    }
    else
    {
        return Results.BadRequest("Invalid date parameters.");
    }
});


app.Run();
