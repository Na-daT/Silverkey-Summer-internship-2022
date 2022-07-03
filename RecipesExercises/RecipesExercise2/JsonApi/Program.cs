using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    ReferenceHandler = ReferenceHandler.Preserve,
};

app.MapGet("api/json/{fileName}", async Task<string> (string fileName) =>
{
    return await File.ReadAllTextAsync(fileName + ".json");
});

app.MapPost("api/json/addrecipe", async Task<bool> (Recipe recipeToPost) =>
{
    try
    {
        var recipes = await File.ReadAllTextAsync("recipe.json");
        var recipesList = JsonSerializer.Deserialize<List<Recipe>>(recipes, options);
        recipesList.Add(recipeToPost);
        var json = JsonSerializer.Serialize(recipesList, options);
        await File.WriteAllTextAsync("recipe.json", json);
        return true;
    }
    catch
    {
        return false;
    }
});

app.MapPut("api/json/updaterecipe/{id}", async Task<bool> (Guid id, Recipe recipeToUpdate) =>
{
    try
    {
        var recipes = await File.ReadAllTextAsync("recipe.json");
        var recipesList = JsonSerializer.Deserialize<List<Recipe>>(recipes, options);
        var recipe = recipesList.FirstOrDefault(x => x.id == id);
        if (recipe != null)
        {
            recipe = recipeToUpdate;
        }
        var json = JsonSerializer.Serialize(recipesList, options);
        await File.WriteAllTextAsync("recipe.json", json);
        return true;
    }
    catch
    {
        return false;
    }
});

app.MapDelete("api/json/deleterecipe/{id}", async Task<bool> (Guid id) =>
{
    try
    {
        var recipes = await File.ReadAllTextAsync("recipe.json");
        var recipesList = JsonSerializer.Deserialize<List<Recipe>>(recipes, options);
        var recipe = recipesList.FirstOrDefault(x => x.id == id);
        if (recipe != null)
        {
            recipesList.Remove(recipe);
        }
        var json = JsonSerializer.Serialize(recipesList, options);
        await File.WriteAllTextAsync("recipe.json", json);
        return true;
    }
    catch
    {
        return false;
    }
});

app.MapPost("api/json/addcategory", async Task<bool> (Category categoryToPost) =>
{
    try
    {
        var categories = await File.ReadAllTextAsync("category.json");
        var categoriesList = JsonSerializer.Deserialize<List<Category>>(categories, options);
        categoriesList.Add(categoryToPost);
        var json = JsonSerializer.Serialize(categoriesList, options);
        await File.WriteAllTextAsync("category.json", json);
        return true;
    }
    catch
    {
        return false;
    }
});

app.MapPut("api/json/updatecategory/{id}", async Task<bool> (Guid id, Category categoryToUpdate) =>
{
    try
    {
        var categories = await File.ReadAllTextAsync("category.json");
        var categoriesList = JsonSerializer.Deserialize<List<Category>>(categories, options);
        var category = categoriesList.FirstOrDefault(x => x.id == id);
        if (category != null)
        {
            category = categoryToUpdate;
        }
        var json = JsonSerializer.Serialize(categoriesList, options);
        await File.WriteAllTextAsync("category.json", json);
        return true;
    }
    catch
    {
        return false;
    }
});

app.Run();


public record Recipe(Guid id, string title, List<string> ingredients, List<string> instructions, List<Category> categories);
public record Category(Guid id, string name);