using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RecipeExercise9;
public interface IRecipeService
{
    Task<List<Recipe>> GetRecipes();
    Task<HttpResponseMessage> AddRecipe(Recipe recipe);
    Task<HttpResponseMessage> UpdateRecipe(Recipe recipe);
    Task<HttpResponseMessage> DeleteRecipe(int id);
}

public class RecipeService : IRecipeService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public RecipeService(HttpClient httpClient, IJSRuntime jsRuntime)
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

    public async Task<List<Recipe>> GetRecipes()
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));

            var recipes =  await _httpClient.GetFromJsonAsync<List<Recipe>>("recipes");
            if (recipes is null)
                return new List<Recipe>();
            return recipes;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> AddRecipe(Recipe recipe)
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));
            return await _httpClient.PostAsJsonAsync("recipes", recipe);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> UpdateRecipe(Recipe recipe)
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));
            return await _httpClient.PutAsJsonAsync($"recipes", recipe);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<HttpResponseMessage> DeleteRecipe(int id)
    {
        try
        {
            if (!await CheckAuthentication())
                throw (new Exception("Not authenticated"));
            return await _httpClient.DeleteAsync($"recipes/{id}");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

