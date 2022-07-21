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
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
        if (categoriesList is null)
            throw new Exception("Could not get categories");
        var recipesList = await httpClient.GetFromJsonAsync<List<Recipe>>("recipe");
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        Recipes = Recipe.Load(categoriesList, recipesList);
        Recipes = Recipes.OrderBy(x => x.Title).ToList();

    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var HttpClient = _httpClientFactory.CreateClient("recipeClient");
        var result = await HttpClient.DeleteAsync($"recipes/{id}");
        if (result.IsSuccessStatusCode)
            return RedirectToPage("/Recipes/List");
        throw new Exception("Could not delete recipe");
    }
}
