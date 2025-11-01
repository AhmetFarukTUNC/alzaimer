using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace AlzheimerApp.Services
{
    public class FlaskApiService
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl = "http://127.0.0.1:5000/predict";

        public FlaskApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<(string Class, double Confidence)> PredictAsync(byte[] imageBytes)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(imageBytes), "file", "upload.jpg");

            var response = await _client.PostAsync(_apiUrl, content);
            var json = await response.Content.ReadAsStringAsync();

            var obj = JObject.Parse(json);
            return (obj["class"].ToString(), (double)obj["confidence"]);
        }
    }
}
