using System.Text.Json;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application
{
    public class UserHttpService : IUserHttpService
    {
        private static Uri GetBaseAddress()
        {
            var envUrl = System.Environment.GetEnvironmentVariable("AUTH_URL");
            if (envUrl != null && envUrl != string.Empty)
            {
                var envUrlParts = envUrl.Split(':');
                if (envUrlParts[1] == "//localhost")
                    return new Uri($"http://host.docker.internal:{envUrlParts[2]}");
                return new Uri(envUrl);
            }
            return new Uri("https://localhost:7182");
        }
        private Uri _baseAddress = GetBaseAddress();

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = _baseAddress;
                response = await httpClient.GetAsync($"api/auth/users/{userId}");
            }
            response.EnsureSuccessStatusCode();
            var contentStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var userResponse = await JsonSerializer.DeserializeAsync<UserInfo>(contentStream, options);
            return userResponse;
        }
    }
}
