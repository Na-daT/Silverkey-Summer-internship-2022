using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise3.Models;

namespace RecipeExercise3.Pages;
public class CategoriesModel : PageModel
{
    private readonly ILogger<CategoriesModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public List<Category> Categories { get; set; } = new();

    public CategoriesModel(ILogger<CategoriesModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
        if (categoriesList is null)
            throw new Exception("Could not get categories");
        Categories = categoriesList;
    }
}