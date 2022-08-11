using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;

namespace Client_Razor;

public class EditRecipeModel : PageModel
{
    private readonly ILogger<EditRecipeModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
   // private Recipe.RecipeValidator validator = new Recipe.RecipeValidator();

    [BindProperty]
    public Recipe UpdatedRecipe { get; set; } = new();

    [BindProperty]
    public List<Category> ListCategories { get; set; } = new();

    [BindProperty]
    public List<string> CategoriesIds { get; set; } = new();

    public EditRecipeModel(ILogger<EditRecipeModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync(string id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Recipes.RecipesClient(channel);
            var reply = client.GetRecipe(new GetRecipeRequest { Id = id});
            UpdatedRecipe = reply.Recipe;
            var categoriesClient = new Categories.CategoriesClient(channel);
            var categoriesReply = categoriesClient.ListCategories(new ListCategoriesRequest());
            await foreach (var category in categoriesReply.ResponseStream.ReadAllAsync())
            {
                ListCategories.Add(category);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling the gRPC service.");
        }
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        try
        {
            UpdatedRecipe.Id = id;
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Recipes.RecipesClient(channel);
            var categoriesClient = new Categories.CategoriesClient(channel);
            var categoriesReply = categoriesClient.ListCategories(new ListCategoriesRequest());
            await foreach (var category in categoriesReply.ResponseStream.ReadAllAsync())
            {
                ListCategories.Add(category);
            }
            UpdatedRecipe = Utility.Utility.MatchCategory(UpdatedRecipe, ListCategories, CategoriesIds);
            var request = new UpdateRecipeRequest { Id = id, Recipe = UpdatedRecipe };
            var reply = await client.UpdateRecipeAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error occured while calling grpc server");
        }
        return RedirectToPage("/Recipes/List");
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