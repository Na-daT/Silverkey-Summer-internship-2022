using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddConsole();

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";

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

app.MapPost("api/json/add-recipe", async ([FromBody] Recipe recipeToPost) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        recipesList!.Add(recipeToPost);
        var json = await JsonHandler.SerializeAsync(recipesList!);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPut("api/json/update-recipe", async ([FromBody] Recipe recipeToUpdate) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        var recipe = recipesList!.FirstOrDefault(x => x.Id == recipeToUpdate.Id);
        if (recipe is null)
            return Results.StatusCode(404);
        recipesList![recipesList.IndexOf(recipe)] = recipeToUpdate;
        var json = await JsonHandler.SerializeAsync(recipesList!);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapDelete("api/json/delete-recipe/{id}", async (Guid id) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        var recipe = recipesList!.FirstOrDefault(x => x.Id == id);
        if (recipe is null)
            return Results.StatusCode(404);
        recipesList!.Remove(recipe);
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

app.MapPost("api/json/add-category", async ([FromBody] Category categoryToPost) =>
{
    try
    {
        var categories = await FileHandler.ReadAsync("category.json");
        var categoriesList = await JsonHandler.DeserializeAsync<List<Category>>(categories);
        categoriesList!.Add(categoryToPost);
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

app.MapPut("api/json/update-category", async ([FromBody] Category categoryToUpdate) =>
{
    try
    {
        var categories = await FileHandler.ReadAsync("category.json");
        var categoriesList = await JsonHandler.DeserializeAsync<List<Category>>(categories);
        var category = categoriesList!.FirstOrDefault(x => x.Id == categoryToUpdate.Id);
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

app.Run($"http://localhost:{port}");