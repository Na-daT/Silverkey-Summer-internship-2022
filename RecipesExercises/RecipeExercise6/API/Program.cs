using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
});
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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.SaveToken = true;
         c.RequireHttpsMetadata = false;
         //c.Authority = $"https://{builder.Configuration["JWT:Issuer"]}";
         c.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidAudience = builder.Configuration["JWT:Audience"],
             ValidIssuer = $"{builder.Configuration["JWT:Issuer"]}",
             IssuerSigningKey = new SymmetricSecurityKey
               (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
         };
     });
builder.Services.AddAuthorization();
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddConsole();
builder.Services.AddSingleton(new TokenService());
builder.Services.AddSingleton<IUserRepositoryService>(new UserRepositoryService());

var app = builder.Build();
app.UseCors();
//var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = String.Empty;
});
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
};
app.MapPost("api/json/login", [AllowAnonymous] async ([FromBodyAttribute] UserModel userModel, TokenService tokenService, IUserRepositoryService userRepositoryService, HttpResponse response) => {
    var userDto = userRepositoryService.GetUser(userModel);
    if (userDto == null)
    {
        response.StatusCode = 401;
        return;
    }
    var token = tokenService.BuildToken(builder.Configuration["JWT:Key"], builder.Configuration["JWT:Issuer"], builder.Configuration["JWT:Audience"], userDto);
    await response.WriteAsJsonAsync(new { token = token });
    return;
}).Produces(StatusCodes.Status200OK)
.WithName("Login").WithTags("Accounts");


app.MapGet("api/json/{fileName}", async Task<string> (string fileName) =>
{
    var jsonFile = fileName + ".json";
    return await FileHandler.ReadAsync(jsonFile);
}).RequireAuthorization();

app.MapPost("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToPost) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        recipesList.Add(recipeToPost);
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPut("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToUpdate) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        var recipe = recipesList.FirstOrDefault(x => x.Id == recipeToUpdate.Id);
        if (recipe is null)
            return Results.StatusCode(404);
        recipesList[recipesList.IndexOf(recipe)] = recipeToUpdate;
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapDelete("api/json/recipes/{id}", [Authorize] async (Guid id) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        var recipe = recipesList.FirstOrDefault(x => x.Id == id);
        if (recipe is null)
            return Results.StatusCode(404);
        recipesList.Remove(recipe);
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPost("api/json/categories", [Authorize] async ([FromBody] Category categoryToPost) =>
{
    try
    {
        var categories = await FileHandler.ReadAsync("category.json");
        var categoriesList = await JsonHandler.DeserializeAsync<List<Category>>(categories);
        if (categoriesList is null)
            throw new Exception("Could not deserialize categories list");
        categoriesList.Add(categoryToPost);
        var json = await JsonHandler.SerializeAsync(categoriesList);
        await FileHandler.WriteAsync("category.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPut("api/json/categories", [Authorize] async ([FromBody] Category categoryToUpdate) =>
{
    try
    {
        var categories = await FileHandler.ReadAsync("category.json");
        var categoriesList = await JsonHandler.DeserializeAsync<List<Category>>(categories);
        if (categoriesList is null)
            throw new Exception("Could not deserialize categories list");
        var category = categoriesList.FirstOrDefault(x => x.Id == categoryToUpdate.Id);
        if (category is null)
            return Results.StatusCode(404);
        category.Name = categoryToUpdate.Name;
        var json = await JsonHandler.SerializeAsync(categoriesList);
        await FileHandler.WriteAsync("category.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.Run();