using System.Text.Json;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.ValueObjects;

namespace VirtualLab.Application
{
    public class UserHttpService : IUserHttpService
    {
        private Uri _baseAddress = new Uri("https://localhost:7182");
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
