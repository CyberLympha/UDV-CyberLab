using System.Text.Json;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application
{
    public class UserHttpService : IUserHttpService
    {
        private static Uri GetBaseAddress()
        {
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Development")
                return new Uri("https://localhost:7182");
            else if (env == "Docker-Development")
            {
                if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS())
                    return new Uri("http://host.docker.internal:8081");
                if (OperatingSystem.IsLinux())
                    return new Uri("http://172.17.0.1:8081");
            }            
            throw new InvalidOperationException($"Unsupported environment. {env} - environment");
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
