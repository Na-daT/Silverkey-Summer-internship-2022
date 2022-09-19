using System.Net.Http.Json;
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
    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Category>> GetCategories()
    {
        try
        {
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
            return await _httpClient.PutAsJsonAsync($"categories", category);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}