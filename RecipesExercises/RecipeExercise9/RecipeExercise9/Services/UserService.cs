using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RecipeExercise9;
public interface IUserService
{
    Task<bool> Login(LoginModel loginDetails);
    Task<HttpResponseMessage> Register(RegisterModel userDetails);
    Task<HttpResponseMessage> RefreshToken(RefreshRequest request);
    Task LogOut();
    Task<bool> CheckAuthentication();
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
        var checktoken = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "token");
        if (string.IsNullOrEmpty(checktoken))
        { 
            var auth = await RefreshToken();
            if (auth.IsSuccessStatusCode)
            {
                var tokens = await auth.Content.ReadAsAsync<AuthenticatedResponse>();
                checktoken = tokens.Token;
                var refreshToken = tokens.RefreshToken;
                await _jsRuntime.InvokeAsync<string>("sessionStorage.setItem", "token", checktoken);
                await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "refreshToken", refreshToken);
            }
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", checktoken);

        return !string.IsNullOrEmpty(checktoken);
    }
    
    private async Task<HttpResponseMessage> RefreshToken()
    {
        var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");
        var request = new RefreshRequest { RefreshToken = refreshToken };
        return await _httpClient.PostAsJsonAsync("refresh-token", request);
    }
    
    public async Task<bool> Login(LoginModel loginDetails)
    {
        try
        {
            var auth = await _httpClient.PostAsJsonAsync("login", loginDetails);
            if (auth.IsSuccessStatusCode)
            {
                var tokens = await auth.Content.ReadAsAsync<AuthenticatedResponse>();
                var token = tokens.Token;
                var refreshToken = tokens.RefreshToken;
                await _jsRuntime.InvokeAsync<string>("sessionStorage.setItem", "token", token);
                await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "refreshToken", refreshToken);
                return true;
            }
            else
                return false;

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

    public async Task LogOut()
    {
        await _jsRuntime.InvokeAsync<string>("sessionStorage.removeItem", "token");
        await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", "refreshToken");
    }
}