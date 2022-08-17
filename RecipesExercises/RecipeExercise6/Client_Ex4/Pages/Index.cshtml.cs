﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeExercise6;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string MyUrl;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        MyUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("api")["url"];
    }

    public string GetID()
    {
        return Guid.NewGuid().ToString();
    }

    public void OnGet()
    {
    }
}
