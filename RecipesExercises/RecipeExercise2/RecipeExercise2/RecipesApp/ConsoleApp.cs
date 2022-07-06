using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace RecipesApp
{
    public class ConsoleApp
    {
        UI ui;
        HttpClient httpClient;

        public ConsoleApp()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:3000/api/json/");
            ui = new UI();
        }

        public async Task<HttpResponseMessage> AddRecipeAsync(Recipe recipe) => await httpClient.PostAsJsonAsync("add-recipe", recipe);

        public async Task<HttpResponseMessage> UpdateRecipeAsync(Recipe recipe) => await httpClient.PutAsJsonAsync($"update-recipe", recipe);

        public async Task<HttpResponseMessage> DeleteRecipeAsync(Guid id) => await httpClient.DeleteAsync($"delete-recipe/{id}");

        public async Task<HttpResponseMessage> AddCategoryAsync(Category category) => await httpClient.PostAsJsonAsync("add-category", category);

        public async Task<HttpResponseMessage> UpdateCategoryAsync(Category category) => await httpClient.PutAsJsonAsync($"update-category", category);

        public async Task<List<Category>?> GetCategoriesAsync() => await httpClient.GetFromJsonAsync<List<Category>>("category");

        public async Task<List<Recipe>?> GetRecipesAsync()
        {
            var categoriesList = await GetCategoriesAsync();
            var recipesList = await httpClient.GetFromJsonAsync<List<Recipe>>("recipe");
            return Recipe.Load(categoriesList, recipesList);
        }

        public async Task RunMain()
        {
            var categoriesList = new List<Category>();
            var recipesList = new List<Recipe>();
            ui.StartPage();
            var choice = ui.MainMenuPrompt();
            while (choice != "Exit")
            {
                switch (choice)
                {
                    case "List recipes":
                        recipesList = await GetRecipesAsync();
                        ui.ListRecipes(recipesList);
                        break;
                    case "Add recipe":
                        categoriesList = await GetCategoriesAsync();
                        if (categoriesList.Any())
                        {
                            Recipe recipeToBeAdded = ui.AddRecipe(categoriesList);
                            var response = await AddRecipeAsync(recipeToBeAdded);
                            if (response.IsSuccessStatusCode)
                            {
                                ui.SuccessMessage("Recipe added successfully");
                            }
                            else
                            {
                                ui.ErrorMessage("Error adding recipe, please try again later");
                            }
                        }
                        else
                        { ui.ErrorMessage("You need to add a Category first!"); }
                        break;
                    case "Remove recipe":
                        recipesList = await GetRecipesAsync();
                        if (recipesList.Any())
                        {
                            Guid idToBeRemoved = ui.PickRecipe(recipesList).Id;
                            HttpResponseMessage response = await DeleteRecipeAsync(idToBeRemoved);
                            if (response.IsSuccessStatusCode)
                            {
                                ui.SuccessMessage("Recipe removed successfully");
                            }
                            else
                            {
                                ui.ErrorMessage("Error removing recipe, please try again later");
                            }
                        }
                        else
                        { ui.ErrorMessage("You need to add a recipe first!"); }
                        break;
                    case "Update recipe":
                        recipesList = await GetRecipesAsync();
                        categoriesList = await GetCategoriesAsync();
                        if (recipesList.Any())
                        {
                            Recipe recipeToBeUpdated = ui.PickRecipe(recipesList);
                            recipeToBeUpdated = ui.UpdateRecipe(recipeToBeUpdated, categoriesList);
                            var response = await UpdateRecipeAsync(recipeToBeUpdated);
                            if (response.IsSuccessStatusCode)
                            {
                                ui.SuccessMessage("Recipe updated successfully");
                            }
                            else
                            {
                                ui.ErrorMessage("Error updating recipe, please try again later");
                            }
                        }
                        else
                        { ui.ErrorMessage("You need to add a recipe first!"); }
                        break;
                    case "Add Category":
                        Category categoryTobeAdded = ui.AddCategory();
                        if (categoriesList.Any(x => x.Name == categoryTobeAdded.Name))
                            ui.ErrorMessage("Category already exists!");
                        else
                        {
                            var response = await AddCategoryAsync(categoryTobeAdded);
                            if (response.IsSuccessStatusCode)
                            {
                                ui.SuccessMessage("Category added successfully");
                            }
                            else
                            {
                                ui.ErrorMessage("Error adding category, please try again later");
                            }
                        }
                        break;
                    case "Update Category":
                        categoriesList = await GetCategoriesAsync();
                        if (categoriesList.Any())
                        {
                            Category categoryToBeUpdated = ui.PickCategory(categoriesList);
                            categoryToBeUpdated = ui.UpdateCategory(categoryToBeUpdated);
                            var response = await UpdateCategoryAsync(categoryToBeUpdated);
                            if (response.IsSuccessStatusCode)
                            {
                                ui.SuccessMessage("Category updated successfully");
                            }
                            else
                            {
                                ui.ErrorMessage("Error updating category, please try again later");
                            }
                        }
                        else
                        { ui.ErrorMessage("You need to add a category first!"); }
                        break;
                    case "Exit":
                        ui.EndScreen();
                        break;
                    default:
                        break;
                }
                choice = ui.MainMenuPrompt();
            }
        }
    }
}