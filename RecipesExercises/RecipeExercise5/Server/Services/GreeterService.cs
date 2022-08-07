using Grpc.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Server;

namespace Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    public GreeterService()
    {

    }
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }
    public override Task<RecipesList> GetRecipes(RecipesListRequest request, ServerCallContext context)
    {
        RecipesList reply = new();
        reply.Recipes.Add(new Recipe { Title = "Banana Cake", Ingredients = new[] { "sugar", "eggs" }, Instructions = new[] { "Bake in three layer pans at 350 degrees." }, Categories = new[] { new Category { Id = "1", Name = "Baking" } } });
        return Task.FromResult(reply);
    }
}
