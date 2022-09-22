using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace RecipeExercise9;
public interface ICategoryService
{
    Task<List<Category>> GetCategories();
    Task<HttpResponseMessage> AddCategory(Category category);
    Task<HttpResponseMessage> UpdateCategory(Category category);
}

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public CategoryService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> CheckAuthentication()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", token);

        return !string.IsNullOrEmpty(token);
    }

    public async Task<List<Category>> GetCategories()
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));

            var categories = await _httpClient.GetFromJsonAsync<List<Category>>("categories");
            if (categories is null)
                return new List<Category>();
            return categories;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> AddCategory(Category category)
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));
            return await _httpClient.PostAsJsonAsync("categories", category);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> UpdateCategory(Category category)
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));
            return await _httpClient.PutAsJsonAsync($"categories", category);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}