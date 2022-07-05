using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
//using System.Web.Http.Results;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.UseSwagger();
app.UseSwaggerUI();

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
};

app.MapGet("api/json/{fileName}", async Task<string> (string fileName) =>
{
    var jsonFile = fileName + ".json";
    return await File.ReadAllTextAsync(jsonFile);
});

app.MapPost("api/json/add-recipe", async ([FromBody] Recipe recipeToPost) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        recipesList.Add(recipeToPost);
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch
    {
        return Results.NotFound();
    }
});

app.MapPut("api/json/update-recipe", async ([FromBody] Recipe recipeToUpdate) =>
{
    try
    {
        var recipes = await File.ReadAllTextAsync("recipe.json");
        var recipesList = JsonSerializer.Deserialize<List<Recipe>>(recipes, options);
        var recipe = recipesList.FirstOrDefault(x => x.Id == recipeToUpdate.Id);
        recipesList[recipesList.IndexOf(recipe)] = recipeToUpdate;
        var json = JsonSerializer.Serialize(recipesList, options);
        await File.WriteAllTextAsync("recipe.json", json);
        return Results.Ok();
    }
    catch
    {
        return Results.NotFound();
    }
});

app.MapDelete("api/json/delete-recipe/{id}", async ([FromBody] Guid id) =>
{
    try
    {
        var recipes = await File.ReadAllTextAsync("recipe.json");
        var recipesList = JsonSerializer.Deserialize<List<Recipe>>(recipes, options);
        var recipe = recipesList.FirstOrDefault(x => x.Id == id);
        recipesList.Remove(recipe);
        var json = JsonSerializer.Serialize(recipesList, options);
        await File.WriteAllTextAsync("recipe.json", json);
        return Results.Ok();
    }
    catch
    {
        return Results.NotFound();
    }
});

app.MapPost("api/json/add-category", async ([FromBody] Category categoryToPost) =>
{
    try
    {
        var categories = await File.ReadAllTextAsync("category.json");
        var categoriesList = JsonSerializer.Deserialize<List<Category>>(categories, options);
        categoriesList.Add(categoryToPost);
        var json = JsonSerializer.Serialize(categoriesList, options);
        await File.WriteAllTextAsync("category.json", json);
        return Results.Ok();
    }
    catch
    {
        return Results.NotFound();
    }
});

app.MapPut("api/json/update-category", async ([FromBody] Category categoryToUpdate) =>
{
    try
    {
        var categories = await File.ReadAllTextAsync("category.json");
        var categoriesList = JsonSerializer.Deserialize<List<Category>>(categories, options);
        var category = categoriesList.FirstOrDefault(x => x.Id == categoryToUpdate.Id);
        category.Name = categoryToUpdate.Name;
        var json = JsonSerializer.Serialize(categoriesList, options);
        await File.WriteAllTextAsync("category.json", json);
        return Results.Ok();
    }
    catch
    {
        return Results.NotFound();
    }
});

app.Run($"http://localhost:{port}");

// public record Category(Guid id, string name);
// public record Recipe(Guid id, string title, List<string> ingredients, List<string> instructions, List<Category> categories);