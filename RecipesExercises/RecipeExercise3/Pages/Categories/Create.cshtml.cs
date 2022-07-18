using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise3.Models;

namespace RecipeExercise3.Pages.Categories;

public class CategoriesAddModel : PageModel
{
    private readonly ILogger<CategoriesAddModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public Category NewCategory { get; set; } = new();

    public CategoriesAddModel(ILogger<CategoriesAddModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var result = await httpClient.PostAsJsonAsync("categories", NewCategory);
        if (result.IsSuccessStatusCode)
            return RedirectToPage("/Categories/List");
        throw new Exception("Could not update category");
    }
}