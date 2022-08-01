using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddConsole();

var app = builder.Build();
app.UseCors();
//var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.UseSwagger();
app.UseSwaggerUI();

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

app.MapGet("api/json/{fileName}", async Task<string> (string fileName) =>
{
    var jsonFile = fileName + ".json";
    return await FileHandler.ReadAsync(jsonFile);
});

app.MapPost("api/json/recipes", async ([FromBody] Recipe recipeToPost) =>
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

app.MapPut("api/json/recipes", async ([FromBody] Recipe recipeToUpdate) =>
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

app.MapDelete("api/json/recipes/{id}", async (Guid id) =>
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

app.MapPost("api/json/categories", async ([FromBody] Category categoryToPost) =>
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

app.MapPut("api/json/categories", async ([FromBody] Category categoryToUpdate) =>
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