using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise3.Models;


namespace RecipeExercise3.Pages.Recipes;

public class RecipesModel : PageModel
{
    private readonly ILogger<RecipesModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public List<Recipe> Recipes { get; set; } = new();

    public RecipesModel(ILogger<RecipesModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var HttpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoriesList = await HttpClient.GetFromJsonAsync<List<Category>>("category");
        if (categoriesList is null)
            throw new Exception("Could not get categories");
        var recipesList = await HttpClient.GetFromJsonAsync<List<Recipe>>("recipe");
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        Recipes = Recipe.Load(categoriesList, recipesList);
    }
}
