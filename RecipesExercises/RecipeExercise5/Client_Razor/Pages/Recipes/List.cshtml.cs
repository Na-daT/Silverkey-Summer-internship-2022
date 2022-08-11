using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;

namespace Client_Razor;
public class RecipesModel : PageModel
{
    private readonly ILogger<RecipesModel> _logger;
    public List<Recipe> ListRecipes { get; set; } = new();

    public RecipesModel(ILogger<RecipesModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Recipes.RecipesClient(channel);
            var reply = client.ListRecipes(new ListRecipesRequest());
            await foreach (var recipe in reply.ResponseStream.ReadAllAsync())
            {
                ListRecipes.Add(recipe);
            }
            List<Category> ListCategories = new();
            var clientCategories = new Categories.CategoriesClient(channel);
            var replyCategories = clientCategories.ListCategories(new ListCategoriesRequest());
            await foreach (var category in replyCategories.ResponseStream.ReadAllAsync())
            {
                ListCategories.Add(category);
            }
            ListRecipes = Utility.Utility.Load(ListCategories,ListRecipes);
            ListRecipes = ListRecipes.OrderBy(x => x.Title).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling the gRPC service.");
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Recipes.RecipesClient(channel);
            var request = new DeleteRecipeRequest { Id = id };
            var reply = await client.DeleteRecipeAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error occured while calling grpc server");
        }
        return RedirectToPage("/Recipes/List");

    }
}
