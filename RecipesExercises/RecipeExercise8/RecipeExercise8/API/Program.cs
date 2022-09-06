using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using Npgsql;
using recipesApp;
using recipesApp.EntityClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Store.PartnerCenter.Models.Query;
using View.DtoClasses;
using View.Persistence;
//using recipesApp.Linq;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    var securityScheme = new OpenApiSecurityScheme
//    {
//        Name = "JWT Authentication",
//        Description = "Enter JWT Bearer token",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };
//    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {securityScheme, new string[] { }}
//    });
//});
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
//RuntimeConfiguration.AddConnectionString("ConnectionString.SQL Server (SqlClient)",
//                                                     builder.Configuration.GetConnectionString("ConnectionString.SQL Server (SqlClient)"));
//RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c =>
//{
//    c.AddDbProviderFactory(typeof(NpgsqlFactory));
//});
builder.Services.AddDbContext<RecipesAppDataContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString.SQL Server (SqlClient)")));

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
         //  c.Events.OnMessageReceived = context =>
         //  {

         //      if (context.Request.Cookies.ContainsKey("X-Access-Token"))
         //      {
         //          context.Token = context.Request.Cookies["X-Access-Token"];
         //      }

         //      return Task.CompletedTask;
         //  };
     });

builder.Services.AddAuthorization();
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddConsole();
builder.Services.AddTransient<ITokenService, TokenService>();
var app = builder.Build();
app.UseCors();
//app.UseSwagger();
//app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//    options.RoutePrefix = String.Empty;
//});
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
};

app.MapPost("api/json/login", [AllowAnonymous] async ([FromBody] LoginModel loginModel, HttpContext context, RecipesAppDataContext dbContext) =>
{
    using (var trransaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == loginModel.UserName.ToLower());
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
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            trransaction.Commit();
            return Results.Ok(new AuthenticatedResponse { RefreshToken = refreshToken, Token = accessToken });
        }
        catch (Exception ex)
        {
            trransaction.Rollback();
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapPost("api/json/register", [AllowAnonymous] async ([FromBody] RegisterModel newUser, RecipesAppDataContext dbContext) =>
{
    using (var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username.ToLower());
            if (user != null)
                return Results.BadRequest("User already exists");
            var hasher = new PasswordHasher<LoginModel>();
            var hashedPassword = hasher.HashPassword(new LoginModel(), newUser.Password);
            var userEntity = new User
            {
                Username = newUser.Username.ToLower(),
                Password = hashedPassword,
                RefreshToken = null,
                RefreshTokenExpiry = null
            };
            dbContext.Users.Add(userEntity);
            await dbContext.SaveChangesAsync();
            transaction.Commit();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapPost("api/json/refresh-token", [AllowAnonymous] async ([FromBody] RefreshRequest request, HttpContext context, RecipesAppDataContext dbContext) =>
{
    using(var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            if (request is null)
                return Results.BadRequest("Invalid client request");

            string accessToken = request.Token;
            string refreshToken = request.RefreshToken;
            TokenService _tokenService = new TokenService();
            var principal = _tokenService.ValidateExpiredToken(accessToken);
            if (principal == null)
                return Results.BadRequest("Invalid token");
            var username = principal.Identity.Name;

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return Results.BadRequest("Invalid client request");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            transaction.Commit();
            return Results.Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapPost("api/json/revoke-token", [Authorize] async ([FromBody] string token, RecipesAppDataContext dbContext) =>
{
    using(var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return Results.BadRequest(new { message = "Token is required" });
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == token);
            if(user is null)
                return Results.BadRequest(new { message = "Invalid token" });
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            transaction.Commit();
            return Results.Ok(new { message = "Token revoked" });
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Results.BadRequest(ex.Message);
        }
    }
});

app.MapGet("api/json/recipes", [Authorize] async Task<List<RecipeMenuView>> (RecipesAppDataContext dbContext) =>
{
    var recipes = await dbContext.Recipes.Where(x => x.IsActive).ProjectToRecipeMenuView().ToListAsync();
    foreach(var recipe in recipes)
    {
        recipe.Instructions = recipe.Instructions.Where(x => x.IsActive).ToList();
        recipe.Ingredients = recipe.Ingredients.Where(x => x.IsActive).ToList();
        recipe.RecipeCategories = recipe.RecipeCategories.Where(x => x.IsActive).ToList();
    }
    return recipes.OrderBy(x => x.Title).ToList();
});

app.MapGet("api/json/categories", [Authorize] async Task<List<recipesApp.EntityClasses.Category>> (RecipesAppDataContext dbContext) =>
{
    var categories = await dbContext.Categories.Where(x=>x.IsActive).ToListAsync();
    return categories.OrderBy(x => x.Name).ToList();
}); 

app.MapPost("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToPost, RecipesAppDataContext dbContext) =>
{
    using (var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var recipe = new recipesApp.EntityClasses.Recipe() { Title = recipeToPost.Title, IsActive = true };
            await dbContext.Recipes.AddAsync(recipe);
            
            var ingredientsList = new List<recipesApp.EntityClasses.Ingredient>();
            var instuctionsList = new List<recipesApp.EntityClasses.Instruction>();
            var recipeCategories = new List<RecipeCategory>();

            foreach (var ingredient in recipeToPost.Ingredients)
                ingredientsList.Add(new recipesApp.EntityClasses.Ingredient() { Name = ingredient.Name, Recipe = recipe });
            await dbContext.Ingredients.AddRangeAsync(ingredientsList);

            foreach (var instruction in recipeToPost.Instructions)
                instuctionsList.Add(new recipesApp.EntityClasses.Instruction { Name = instruction.Name, Recipe = recipe });
            await dbContext.Instructions.AddRangeAsync(instuctionsList);

            foreach (var category in recipeToPost.Categories)
                recipeCategories.Add(new RecipeCategory { CategoryId = category.Id, Recipe = recipe });
            await dbContext.RecipeCategories.AddRangeAsync(recipeCategories);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            app.Logger.LogError(e.Message);
            return Results.StatusCode(500);
        }
    }
});

app.MapPut("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToUpdate, RecipesAppDataContext dbContext) =>
{
    using (var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var recipe = await dbContext.Recipes.FindAsync(recipeToUpdate.Id);
            if (recipe is null)
                throw new Exception("Recipe not found");
                
            recipe.Title = recipeToUpdate.Title;
            dbContext.Update(recipe);
                
            var ingredientsList = new List<recipesApp.EntityClasses.Ingredient>();
            var instuctionsList = new List<recipesApp.EntityClasses.Instruction>();
            var recipeCategories = new List<RecipeCategory>();

            foreach (var ingredient in recipeToUpdate.Ingredients)
            {
                var ingredientToUpdate = await dbContext.Ingredients.Where(x => x.RecipeId == recipeToUpdate.Id && x.IsActive).FirstOrDefaultAsync(x => x.Name == ingredient.Name);
                if (ingredientToUpdate is null && ingredient.IsActive)
                    ingredientsList.Add(new recipesApp.EntityClasses.Ingredient() { Name = ingredient.Name, Recipe = recipe, IsActive = true });
                else
                {
                    ingredientToUpdate!.Name = ingredient.Name;
                    ingredientToUpdate.IsActive = ingredient.IsActive;
                    dbContext.Update(ingredientToUpdate);
                }
            }
            await dbContext.Ingredients.AddRangeAsync(ingredientsList);

            foreach (var instruction in recipeToUpdate.Instructions)
            {
                var instructionToUpdate = await dbContext.Instructions.Where(x => x.RecipeId == recipeToUpdate.Id && x.IsActive).FirstOrDefaultAsync(x => x.Name == instruction.Name);
                if (instructionToUpdate is null && instruction.IsActive)
                    instuctionsList.Add(new recipesApp.EntityClasses.Instruction { Name = instruction.Name, Recipe = recipe, IsActive = true });
                else
                {
                    instructionToUpdate!.Name = instruction.Name;
                    instructionToUpdate.IsActive = instruction.IsActive;
                    dbContext.Update(instructionToUpdate);
                }
            }
            await dbContext.Instructions.AddRangeAsync(instuctionsList);

            foreach (var recipeCategory in dbContext.RecipeCategories.Where(x => x.RecipeId == recipeToUpdate.Id))
                if (!recipeToUpdate.Categories.Any(x => x.Id == recipeCategory.CategoryId))
                    recipeCategory.IsActive = false;
            dbContext.UpdateRange(dbContext.RecipeCategories.Where(x => x.RecipeId == recipeToUpdate.Id));

            foreach (var category in recipeToUpdate.Categories)
            {
                var dbCategory = await dbContext.Categories.FindAsync(category.Id);
                if (dbCategory is null)
                    throw new Exception("Category not found");
                else if (!dbContext.RecipeCategories.Any(x => x.RecipeId == recipeToUpdate.Id && x.CategoryId == category.Id && x.IsActive))
                    recipeCategories.Add(new RecipeCategory { Category = dbCategory!, Recipe = recipe, IsActive = true });
            }
            
            await dbContext.RecipeCategories.AddRangeAsync(recipeCategories);
            
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            app.Logger.LogError(e.Message);
            return Results.StatusCode(500);
        }
    }
});

app.MapDelete("api/json/recipes/{id}", [Authorize] async (int id, RecipesAppDataContext dbContext) =>
{
    using (var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var recipe = await dbContext.Recipes.FindAsync(id);
            if (recipe is null)
                throw new Exception("Recipe not found");
            recipe.IsActive = false;
            dbContext.Update(recipe);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            app.Logger.LogError(e.Message);
            return Results.StatusCode(500);
        }
    }
});

app.MapPost("api/json/categories", [Authorize] async ([FromBody] Category categoryToPost, RecipesAppDataContext dbContext) =>
{
    using (var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var category = new recipesApp.EntityClasses.Category() { Name = categoryToPost.Name, IsActive = true };
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            app.Logger.LogError(e.Message);
            return Results.StatusCode(500);
        }
    }
});

app.MapPut("api/json/categories", [Authorize] async ([FromBody] Category categoryToUpdate, RecipesAppDataContext dbContext) =>
{
    using (var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            var category = await dbContext.Categories.FindAsync(categoryToUpdate.Id);
            if (category is null)
                throw new Exception("Category not found");
            category.Name = categoryToUpdate.Name;
            dbContext.Update(category);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Results.Ok();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            app.Logger.LogError(e.Message);
            return Results.StatusCode(500);
        }
    }
});

app.Run();