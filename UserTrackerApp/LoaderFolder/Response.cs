using System.Text.Json;

namespace UserTracker
{
    public class Response
    {
        public int total { get; set; }
        public List<UserData> data { get; set; }
    }

    public interface IGetData
    {
        Response GetResponse(string url);
    }

    public class GetData : IGetData
    {
        public Response GetResponse(string url)
        {
            using var client = new HttpClient();
            using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, url));
            using var reader = new StreamReader(result.Content.ReadAsStream());
            var stringContent = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Response>(stringContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
