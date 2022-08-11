using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;

namespace Client_Razor;
public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;

    [BindProperty]
    public List<Category> ListCategories { get; set; } = new();

    [BindProperty]
    public Recipe NewRecipe { get; set; } = new();
    //Recipe.RecipeValidator validator = new Recipe.RecipeValidator();

    [BindProperty]
    public List<string> CategoriesIds { get; set; } = new();

    public CreateModel(ILogger<CreateModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Getting categories");
        using var channel = GrpcChannel.ForAddress("https://localhost:7103");
        var clientCategories = new Categories.CategoriesClient(channel);
        var replyCategories = clientCategories.ListCategories(new ListCategoriesRequest());
        await foreach (var category in replyCategories.ResponseStream.ReadAllAsync())
        {
            ListCategories.Add(category);
        }
        //var httpClient = _httpClientFactory.CreateClient("recipeClient");
        //var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
        //if (categoriesList is null)
        //    throw new Exception("Could not get categories");
        //Categories = categoriesList;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Recipes.RecipesClient(channel);
            var categoriesClient = new Categories.CategoriesClient(channel);
            var categoriesReply = categoriesClient.ListCategories(new ListCategoriesRequest());
            await foreach (var category in categoriesReply.ResponseStream.ReadAllAsync())
            {
                ListCategories.Add(category);
            }
            NewRecipe = Utility.Utility.MatchCategory(NewRecipe, ListCategories, CategoriesIds);
            var reply = await client.CreateRecipeAsync(new CreateRecipeRequest { Recipe = NewRecipe });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error occured while calling grpc server");
        }
        return RedirectToPage("/Recipes/List");
    }
}
