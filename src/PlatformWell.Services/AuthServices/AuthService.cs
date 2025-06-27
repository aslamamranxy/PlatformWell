using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using PlatformWell.Services.Constants.Api;
using PlatformWell.Core.Models.Options;

namespace PlatformWell.Services.AuthServices;

public class AuthService(HttpClient httpClient, IOptions<ApiAuthOptions> options)
{
    public async Task<string?> LoginAsync()
    {
        var payload = new
        {
            username = options.Value.Username,
            password = options.Value.Password
        };
        
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json-patch+json");

        var response = await httpClient.PostAsync(ApiBaseUrl.Url + "/Account/Login", content);
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Login failed: " + response.StatusCode);
            return null;
        }

        var token = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Received Token: " + token);
        
        return token;
    }
}