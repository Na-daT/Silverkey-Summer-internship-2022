using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise6.Models;

namespace RecipeExercise6;

public class LoginModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string MyUrl;

    public LoginModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        MyUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("api")["url"];
    }
    
    public void OnGet()
    {
        
    }
}