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
    public RecipeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Recipe>> GetRecipes()
    {
        try
        {
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
            return await _httpClient.PutAsJsonAsync($"recipes/{recipe.Id}", recipe);
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
            return await _httpClient.DeleteAsync($"recipes/{id}");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

