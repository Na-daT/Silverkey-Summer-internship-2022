using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeExercise5.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public bool Redirected { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Redirected = false;
    }
}
