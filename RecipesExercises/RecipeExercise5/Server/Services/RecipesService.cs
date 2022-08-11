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

    public override async Task<GetRecipeResponse> GetRecipe(GetRecipeRequest request, ServerCallContext context)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == request.Id);
        return new GetRecipeResponse { Recipe = recipe };
    }

    public override async Task<CreateRecipeResponse> CreateRecipe(CreateRecipeRequest request, ServerCallContext context)
    {
        var newRecipe = request.Recipe;
        newRecipe.Id = Guid.NewGuid().ToString();
        _recipes.Add(newRecipe);
        Utility.SaveRecipes(_recipes);
        return new CreateRecipeResponse { Id = newRecipe.Id };
    }
    
    public override async Task<UpdateRecipeResponse> UpdateRecipe(UpdateRecipeRequest request, ServerCallContext context)
    {
        var recipeToUpdate = _recipes.FirstOrDefault(recipe => recipe.Id == request.Id);
        if(recipeToUpdate is null)
        {
            return new UpdateRecipeResponse { Response = "fail" };
        }
        recipeToUpdate.Categories.Clear();
        recipeToUpdate.Categories.AddRange(request.Recipe.Categories);
        recipeToUpdate.Ingredients.Clear();
        recipeToUpdate.Ingredients.AddRange(request.Recipe.Ingredients);
        recipeToUpdate.Title = request.Recipe.Title;
        recipeToUpdate.Instructions.Clear();
        recipeToUpdate.Instructions.AddRange(request.Recipe.Instructions);
        Utility.SaveRecipes(_recipes);
        return new UpdateRecipeResponse { Response = "success" };
    }

    public override async Task<DeleteRecipeResponse> DeleteRecipe(DeleteRecipeRequest request, ServerCallContext context)
    {
        var recipeToDelete = _recipes.FirstOrDefault(r => r.Id == request.Id);
        if(recipeToDelete is null)
        {
            return new DeleteRecipeResponse { Response = "fail" };
        }
        _recipes.Remove(recipeToDelete);
        Utility.SaveRecipes(_recipes);
        return new DeleteRecipeResponse { Response = "success" };
    }
}