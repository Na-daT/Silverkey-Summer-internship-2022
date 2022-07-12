using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeExercise3.Pages;

public class RecipesModel : PageModel
{
    private readonly ILogger<RecipesModel> _logger;

    public RecipesModel(ILogger<RecipesModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
