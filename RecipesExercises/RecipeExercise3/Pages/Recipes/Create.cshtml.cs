using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise3.Models;

namespace RecipeExercise3.Pages;
public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public List<Category> categoriesList { get; set; } = new();

    [BindProperty]
    public Recipe recipe { get; set; }

    public CreateModel(ILogger<CreateModel> logger, IHttpClientFactory httpClientFactory)
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
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var HttpClient = _httpClientFactory.CreateClient("recipeClient");
        var result = await HttpClient.PostAsJsonAsync("recipes", recipe);
        if (!result.IsSuccessStatusCode)
            throw new Exception("Could not add recipe");
        return RedirectToPage("./Index");
    }
}
