using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise3.Models;

namespace RecipeExercise3.Pages;
public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public List<Category> Categories { get; set; } = new();

    [BindProperty]
    public Recipe NewRecipe { get; set; } = new();

    [BindProperty]
    public List<Guid> CategoriesIds { get; set; } = new();

    public CreateModel(ILogger<CreateModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Getting categories");
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
        if (categoriesList is null)
            throw new Exception("Could not get categories");
        Categories = categoriesList;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        _logger.LogInformation("Creating recipe");
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
        if (categoriesList is null)
            throw new Exception("Could not get categories");
        NewRecipe = Models.Recipe.MatchCategory(NewRecipe, categoriesList, CategoriesIds);
        var result = await httpClient.PostAsJsonAsync("recipes", NewRecipe);
        if (!result.IsSuccessStatusCode)
        {
            _logger.LogError($"Could not create recipe: {NewRecipe.Title}");
            throw new Exception("Could not add recipe");
        }
        _logger.LogInformation($"Created recipe: {NewRecipe.Title}");
        return RedirectToPage("/Index");
    }
}
