using Grpc.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Server;
using Google.Protobuf;


namespace Server.Services;

public class RecipesService : Recipes.RecipesBase
{
    private readonly ILogger<RecipesService> _logger;
    private List<Recipe> _recipes = new List<Recipe>();
    public RecipesService(ILogger<RecipesService> logger)
    {
        _logger = logger;
        this._recipes = Utility.LoadRecipes();
    }

    public override async Task ListRecipes(ListRecipesRequest request, IServerStreamWriter<Recipe> responseStream, ServerCallContext context)
    {
        var responses = _recipes;
        foreach (var response in responses)
        {
            await responseStream.WriteAsync(response);
        }
    }
}