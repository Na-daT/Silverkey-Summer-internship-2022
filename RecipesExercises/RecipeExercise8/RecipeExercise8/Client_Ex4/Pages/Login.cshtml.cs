using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Ex4.Pages;
public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    public string MyUrl;

    public LoginModel(ILogger<LoginModel> logger)
    {
        _logger = logger;
        MyUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("api")["url"];
    }

    public void OnGet()
    {
    }
}