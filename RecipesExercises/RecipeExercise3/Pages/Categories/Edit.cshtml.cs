using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise3.Models;

namespace RecipeExercise3.Pages.Categories;

public class CategoriesEditModel : PageModel
{
    private readonly ILogger<CategoriesEditModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public Category UpdatedCategory { get; set; } = new();

    public CategoriesEditModel(ILogger<CategoriesEditModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync(string id)
    {
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
        if (categoriesList is null)
            throw new Exception("Could not get categories");
        UpdatedCategory = categoriesList.Find(c => c.Id.ToString() == id);
        if (UpdatedCategory is null)
            throw new Exception("Could not find category");
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        var httpClient = _httpClientFactory.CreateClient("recipeClient");
        var categoryToUpdate = new Category { Id = Guid.Parse(id), Name = UpdatedCategory.Name };
        var result = await httpClient.PutAsJsonAsync("categories", categoryToUpdate);
        if (result.IsSuccessStatusCode)
            return RedirectToPage("/Categories/List");
        throw new Exception("Could not update category");
    }
}