using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace RecipesApp;
public class ConsoleApp
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly UI _ui;

    public ConsoleApp(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("api:url")!);
        _ui = new UI();
    }

    public async Task<HttpResponseMessage> AddRecipeAsync(Recipe recipe) => await _httpClient.PostAsJsonAsync("add-recipe", recipe);

    public async Task<HttpResponseMessage> UpdateRecipeAsync(Recipe recipe) => await _httpClient.PutAsJsonAsync($"update-recipe", recipe);

    public async Task<HttpResponseMessage> DeleteRecipeAsync(Guid id) => await _httpClient.DeleteAsync($"delete-recipe/{id}");

    public async Task<HttpResponseMessage> AddCategoryAsync(Category category) => await _httpClient.PostAsJsonAsync("add-category", category);

    public async Task<HttpResponseMessage> UpdateCategoryAsync(Category category) => await _httpClient.PutAsJsonAsync($"update-category", category);

    public async Task<List<Category>?> GetCategoriesAsync() => await _httpClient.GetFromJsonAsync<List<Category>>("category");

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        var categoriesList = await GetCategoriesAsync();
        var recipesList = await _httpClient.GetFromJsonAsync<List<Recipe>>("recipe");
        var recipes = Recipe.Load(categoriesList!, recipesList!);
        return recipes!;
    }

    public async Task RunMain()
    {
        var categoriesList = new List<Category>();
        var recipesList = new List<Recipe>();
        _ui.StartPage();
        var choice = _ui.MainMenuPrompt();
        while (choice != "Exit")
        {
            switch (choice)
            {
                case "List recipes":
                    recipesList = await GetRecipesAsync();
                    _ui.ListRecipes(recipesList);
                    break;
                case "Add recipe":
                    categoriesList = await GetCategoriesAsync();
                    if (categoriesList!.Any())
                    {
                        Recipe recipeToBeAdded = _ui.AddRecipe(categoriesList!);
                        var response = await AddRecipeAsync(recipeToBeAdded);
                        if (response.IsSuccessStatusCode)
                        {
                            _ui.SuccessMessage("Recipe added successfully");
                        }
                        else
                        {
                            _ui.ErrorMessage("Error adding recipe, please try again later");
                        }
                    }
                    else
                    { _ui.ErrorMessage("You need to add a Category first!"); }
                    break;
                case "Remove recipe":
                    recipesList = await GetRecipesAsync();
                    if (recipesList.Any())
                    {
                        Guid idToBeRemoved = _ui.PickRecipe(recipesList).Id;
                        HttpResponseMessage response = await DeleteRecipeAsync(idToBeRemoved);
                        if (response.IsSuccessStatusCode)
                        {
                            _ui.SuccessMessage("Recipe removed successfully");
                        }
                        else
                        {
                            _ui.ErrorMessage("Error removing recipe, please try again later");
                        }
                    }
                    else
                    { _ui.ErrorMessage("You need to add a recipe first!"); }
                    break;
                case "Update recipe":
                    recipesList = await GetRecipesAsync();
                    categoriesList = await GetCategoriesAsync();
                    if (recipesList.Any())
                    {
                        Recipe recipeToBeUpdated = _ui.PickRecipe(recipesList);
                        recipeToBeUpdated = _ui.UpdateRecipe(recipeToBeUpdated, categoriesList);
                        var response = await UpdateRecipeAsync(recipeToBeUpdated);
                        if (response.IsSuccessStatusCode)
                        {
                            _ui.SuccessMessage("Recipe updated successfully");
                        }
                        else
                        {
                            _ui.ErrorMessage("Error updating recipe, please try again later");
                        }
                    }
                    else
                    { _ui.ErrorMessage("You need to add a recipe first!"); }
                    break;
                case "Add Category":
                    Category categoryTobeAdded = _ui.AddCategory();
                    categoriesList = await GetCategoriesAsync();
                    if (categoriesList!.Any(x => x.Name == categoryTobeAdded.Name))
                        _ui.ErrorMessage("Category already exists!");
                    else
                    {
                        var response = await AddCategoryAsync(categoryTobeAdded);
                        if (response.IsSuccessStatusCode)
                        {
                            _ui.SuccessMessage("Category added successfully");
                        }
                        else
                        {
                            _ui.ErrorMessage("Error adding category, please try again later");
                        }
                    }
                    break;
                case "Update Category":
                    categoriesList = await GetCategoriesAsync();
                    if (categoriesList!.Any())
                    {
                        Category categoryToBeUpdated = _ui.PickCategory(categoriesList!);
                        categoryToBeUpdated = _ui.UpdateCategory(categoryToBeUpdated);
                        var response = await UpdateCategoryAsync(categoryToBeUpdated);
                        if (response.IsSuccessStatusCode)
                        {
                            _ui.SuccessMessage("Category updated successfully");
                        }
                        else
                        {
                            _ui.ErrorMessage("Error updating category, please try again later");
                        }
                    }
                    else
                    { _ui.ErrorMessage("You need to add a category first!"); }
                    break;
                case "Exit":
                    _ui.EndScreen();
                    break;
                default:
                    break;
            }
            choice = _ui.MainMenuPrompt();
        }
    }
}