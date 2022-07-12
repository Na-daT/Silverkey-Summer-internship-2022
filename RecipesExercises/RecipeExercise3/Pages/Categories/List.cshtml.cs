using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeExercise3.Pages;

public class CategoriesModel : PageModel
{
    private readonly ILogger<CategoriesModel> _logger;

    public CategoriesModel(ILogger<CategoriesModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}