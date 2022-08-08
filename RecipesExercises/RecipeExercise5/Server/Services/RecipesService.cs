using Grpc.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Server;

namespace Server.Services;

public class RecipesService : Recipes.RecipesBase
{
    private readonly ILogger<RecipesService> _logger;
    public RecipesService(ILogger<RecipesService> logger)
    {
        _logger = logger;
    }
}