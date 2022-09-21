using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RecipeExercise9;
public interface IUserService
{
    Task<HttpResponseMessage> Login(LoginModel loginDetails);
    Task<HttpResponseMessage> Register(RegisterModel userDetails);
    Task<HttpResponseMessage> RefreshToken(RefreshRequest request);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public UserService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> CheckAuthentication()
    {
        var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "token");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", token);

        return !string.IsNullOrEmpty(token);
    }

    public async Task<HttpResponseMessage> Login(LoginModel loginDetails)
    {
        try
        {
            return await _httpClient.PostAsJsonAsync("login", loginDetails);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> Register(RegisterModel userDetails)
    {
        try
        {
            return await _httpClient.PostAsJsonAsync("register", userDetails);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> RefreshToken(RefreshRequest request)
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));
            return await _httpClient.PostAsJsonAsync("refresh-token", request);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}