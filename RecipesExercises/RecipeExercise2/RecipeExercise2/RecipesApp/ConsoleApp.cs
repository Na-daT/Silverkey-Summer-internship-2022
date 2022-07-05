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

        public async Task<HttpResponseMessage> AddRecipe(Recipe recipe) => await httpClient.PostAsJsonAsync("add-recipe", recipe);

        public async Task<HttpResponseMessage> UpdateRecipe(Recipe recipe) => await httpClient.PutAsJsonAsync($"update-recipe", recipe);

        public async Task<HttpResponseMessage> DeleteRecipe(Guid id) => await httpClient.DeleteAsync($"delete-recipe/{id}");

        public async Task<HttpResponseMessage> AddCategory(Category category) => await httpClient.PostAsJsonAsync("add-category", category);

        public async Task<HttpResponseMessage> UpdateCategory(Category category) => await httpClient.PutAsJsonAsync($"update-category", category);

        public async Task<List<Category>?> GetCategories() => await httpClient.GetFromJsonAsync<List<Category>>("category");

        public async Task<List<Recipe>?> GetRecipes()
        {
            var categoriesList = await GetCategories();
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
                        recipesList = await GetRecipes();
                        ui.ListRecipes(recipesList);
                        break;
                    case "Add recipe":
                        categoriesList = await GetCategories();
                        if (categoriesList.Any())
                        {
                            Recipe recipeToBeAdded = ui.AddRecipe(categoriesList);
                            var response = await AddRecipe(recipeToBeAdded);
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
                        recipesList = await GetRecipes();
                        if (recipesList.Any())
                        {
                            Guid idToBeRemoved = ui.PickRecipe(recipesList).Id;
                            HttpResponseMessage response = await DeleteRecipe(idToBeRemoved);
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
                        recipesList = await GetRecipes();
                        categoriesList = await GetCategories();
                        if (recipesList.Any())
                        {
                            Recipe recipeToBeUpdated = ui.PickRecipe(recipesList);
                            recipeToBeUpdated = ui.UpdateRecipe(recipeToBeUpdated, categoriesList);
                            var response = await UpdateRecipe(recipeToBeUpdated);
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
                            var response = await AddCategory(categoryTobeAdded);
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
                        categoriesList = await GetCategories();
                        if (categoriesList.Any())
                        {
                            Category categoryToBeUpdated = ui.PickCategory(categoriesList);
                            categoryToBeUpdated = ui.UpdateCategory(categoryToBeUpdated);
                            var response = await UpdateCategory(categoryToBeUpdated);
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