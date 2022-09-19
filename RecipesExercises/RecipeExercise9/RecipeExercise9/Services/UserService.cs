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
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
            return await _httpClient.PostAsJsonAsync("refresh-token", request);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}