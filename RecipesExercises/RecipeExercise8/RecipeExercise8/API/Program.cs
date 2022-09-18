using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using recipeDatabase;
using recipeDatabase.EntityClasses;
using recipeDatabase.Linq;
using recipeDatabase.HelperClasses;
using recipeDatabase.FactoryClasses;
using recipeDatabase.DatabaseSpecific;
using Microsoft.EntityFrameworkCore;
using View.DtoClasses;
using View.Persistence;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using Npgsql;
using SD.LLBLGen.Pro.LinqSupportClasses;
using System.Xml.Linq;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});
RuntimeConfiguration.AddConnectionString("ConnectionString.Postgres Server", builder.Configuration.GetConnectionString("ConnectionString.Postgres Server"));
RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c =>
{
    c.AddDbProviderFactory(typeof(NpgsqlFactory));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(c =>
     {
         c.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidAudience = builder.Configuration["JWT:Audience"],
             ValidIssuer = $"{builder.Configuration["JWT:Issuer"]}",
             IssuerSigningKey = new SymmetricSecurityKey
               (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
         };
     });

builder.Services.AddAuthorization();
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddConsole();
builder.Services.AddTransient<ITokenService, TokenService>();
var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
};
var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["ConnectionString.Postgres Server"];

app.MapPost("api/json/login", [AllowAnonymous] async ([FromBody] LoginModel loginModel) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            var user = await metaData.User.FirstOrDefaultAsync(u => u.Username == loginModel.UserName.ToLower());
            if (user == null)
                return Results.Unauthorized();
            var hasher = new PasswordHasher<LoginModel>();
            TokenService _tokenService = new TokenService();
            var isPasswordMatch = hasher.VerifyHashedPassword(new LoginModel(), user.Password, loginModel.Password);
            if (isPasswordMatch == PasswordVerificationResult.Failed)
                return Results.Unauthorized();
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginModel.UserName.ToLower())
                    };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await adapter.SaveEntityAsync(user);
            return Results.Ok(new AuthenticatedResponse { RefreshToken = refreshToken, Token = accessToken });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapPost("api/json/register", [AllowAnonymous] async ([FromBody] RegisterModel newUser) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            var user = await metaData.User.FirstOrDefaultAsync(u => u.Username == newUser.Username.ToLower());
            if (user != null)
                return Results.BadRequest("Username already exists");
            var hasher = new PasswordHasher<RegisterModel>();
            var hashedPassword = hasher.HashPassword(newUser, newUser.Password);
            var newUserEntity = new UserEntity
            {
                Username = newUser.Username.ToLower(),
                Password = hashedPassword,
                RefreshToken = null,
                RefreshTokenExpiry = null
            };
            await adapter.SaveEntityAsync(newUserEntity);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapPost("api/json/refresh-token", [AllowAnonymous] async ([FromBody] RefreshRequest request, HttpContext context) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            if (request is null)
                return Results.BadRequest("Invalid client request");
            string accessToken = request.Token;
            string refreshToken = request.RefreshToken;
            TokenService _tokenService = new TokenService();
            var principal = _tokenService.ValidateExpiredToken(accessToken);
            if (principal == null)
                return Results.BadRequest("Invalid token");
            var username = principal.Identity.Name;
            var user = await metaData.User.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                return Results.BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await adapter.SaveEntityAsync(user);
            return Results.Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapPost("api/json/revoke-token", [Authorize] async ([FromBody] string token) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            if (string.IsNullOrEmpty(token))
                return Results.BadRequest(new { message = "Token is required" });
            var user = await metaData.User.FirstOrDefaultAsync(x => x.RefreshToken == token);
            if (user is null)
                return Results.BadRequest(new { message = "Invalid token" });
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await adapter.SaveEntityAsync(user);
            return Results.Ok(new { message = "Token revoked" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapGet("api/json/recipes", [Authorize] async Task<List<RecipeMenu>> () =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        var metaData = new LinqMetaData(adapter);
        var recipes = await metaData.Recipe.Where(x => x.IsActive).ProjectToRecipeMenu().ToListAsync();
        foreach (var recipe in recipes)
        {
            recipe.Instructions = recipe.Instructions.Where(x => x.IsActive).OrderBy(x=>x.Id).ToList();
            recipe.Ingredients = recipe.Ingredients.Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
            recipe.RecipeCategories = recipe.RecipeCategories.Where(x => x.IsActive).ToList();
        }
        return recipes.OrderBy(x => x.Title).ToList();
    }
});

app.MapGet("api/json/categories", [Authorize] async Task<List<CategoryEntity>> () =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        var metaData = new LinqMetaData(adapter);
        var categories = await metaData.Category.Where(x => x.IsActive).ToListAsync();
        return categories.OrderBy(x => x.Name).ToList();
    }
}); 

app.MapPost("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToPost) =>
{
    
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var recipe = new RecipeEntity() { Title = recipeToPost.Title, IsActive = true };
            adapter.SaveEntity(recipe);

            var ingredientsList = new EntityCollection<IngredientEntity>();
            var instuctionsList = new EntityCollection<InstructionEntity>();
            var recipeCategories = new EntityCollection<RecipeCategoryEntity>();

            foreach (var ingredient in recipeToPost.Ingredients)
                ingredientsList.Add(new IngredientEntity() { Name = ingredient.Name, Recipe = recipe });
            await adapter.SaveEntityCollectionAsync(ingredientsList);

            foreach (var instruction in recipeToPost.Instructions)
                instuctionsList.Add(new InstructionEntity { Name = instruction.Name, Recipe = recipe });
            await adapter.SaveEntityCollectionAsync(instuctionsList);

            foreach (var category in recipeToPost.Categories)
                recipeCategories.Add(new RecipeCategoryEntity { CategoryId = category.Id, Recipe = recipe });
            await adapter.SaveEntityCollectionAsync(recipeCategories);

            return Results.Ok();
        }
        catch(Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
});

app.MapPut("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToUpdate) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            RecipeEntity recipeFetched = await metaData.Recipe.FirstOrDefaultAsync(x => x.Id == recipeToUpdate.Id);
            if (recipeFetched is null)
                return Results.BadRequest("Recipe not found");
            recipeFetched.Title = recipeToUpdate.Title;
            await adapter.SaveEntityAsync(recipeFetched);
            
            var ingredientsList = new List<IngredientEntity>();
            var instuctionsList = new List<InstructionEntity>();
            var recipeCategories = new List<RecipeCategoryEntity>();

            foreach (var ingredient in recipeToUpdate.Ingredients)
            {
                var ingredientToUpdate = await metaData.Ingredient.FirstOrDefaultAsync(x => x.Id == ingredient.Id && x.IsActive);
                if (ingredientToUpdate is null && ingredient.IsActive)
                    ingredientsList.Add(new IngredientEntity() { Name = ingredient.Name, Recipe = recipeFetched, IsActive = true });
                else
                {
                    ingredientToUpdate!.Name = ingredient.Name;
                    ingredientToUpdate.IsActive = ingredient.IsActive;
                    await adapter.SaveEntityAsync(ingredientToUpdate);
                }
            }
            foreach (var i in ingredientsList)
                await adapter.SaveEntityAsync(i);

            foreach (var instruction in recipeToUpdate.Instructions)
            {
                var instructionToUpdate = await metaData.Instruction.FirstOrDefaultAsync(x => x.Id == instruction.Id && x.IsActive);
                if (instructionToUpdate is null && instruction.IsActive)
                    instuctionsList.Add(new InstructionEntity { Name = instruction.Name, Recipe = recipeFetched, IsActive = true });
                else
                {
                    instructionToUpdate!.Name = instruction.Name;
                    instructionToUpdate.IsActive = instruction.IsActive;
                    await adapter.SaveEntityAsync(instructionToUpdate);
                }
            }
            foreach (var i in instuctionsList)
                await adapter.SaveEntityAsync(i);

            foreach (var recipeCategory in metaData.RecipeCategory.Where(x => x.RecipeId == recipeToUpdate.Id))
                if (!recipeToUpdate.Categories.Any(x => x.Id == recipeCategory.CategoryId))
                    recipeCategory.IsActive = false;
            var newCategories = metaData.RecipeCategory.Where(x => x.RecipeId == recipeToUpdate.Id).ToList();
            foreach (var c in newCategories)
               await adapter.SaveEntityAsync(c);
            
            foreach (var category in recipeToUpdate.Categories)
            {
                var dbCategory = await metaData.Category.FirstOrDefaultAsync(c=> c.Id == category.Id);
                if (dbCategory is null)
                    throw new Exception("Category not found");
                else if (!metaData.RecipeCategory.Any(x => x.RecipeId == recipeToUpdate.Id && x.CategoryId == category.Id && x.IsActive))
                    recipeCategories.Add(new RecipeCategoryEntity { Category = dbCategory, Recipe = recipeFetched, IsActive = true });
            }
            foreach (var c in recipeCategories)
                await adapter.SaveEntityAsync(c);
            return Results.Ok();
        }
        catch (Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }
});

app.MapDelete("api/json/recipes/{id}", [Authorize] async (int id) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            RecipeEntity recipeFetched = await metaData.Recipe.FirstOrDefaultAsync(x => x.Id == id);
            if (recipeFetched is null)
                return Results.BadRequest("Recipe not found");
            recipeFetched.IsActive = false;
            await adapter.SaveEntityAsync(recipeFetched);
            return Results.Ok();
        }
        catch (Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.BadRequest(e.Message);
        }
    }
});

app.MapPost("api/json/categories", [Authorize] async ([FromBody] Category categoryToPost) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var category = new CategoryEntity() { Name = categoryToPost.Name, IsActive = true };
            await adapter.SaveEntityAsync(category);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
});

app.MapPut("api/json/categories", [Authorize] async ([FromBody] Category categoryToUpdate) =>
{
    using (var adapter = new DataAccessAdapter(connectionString))
    {
        try
        {
            var metaData = new LinqMetaData(adapter);
            CategoryEntity categoryFetched = await metaData.Category.FirstOrDefaultAsync(x => x.Id == categoryToUpdate.Id);
            if (categoryFetched is null)
                return Results.BadRequest("Category not found");
            categoryFetched.Name = categoryToUpdate.Name;
            await adapter.SaveEntityAsync(categoryFetched);
            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }
});

app.Run();