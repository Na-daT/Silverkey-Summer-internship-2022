﻿using System.Text.Json.Serialization;
using Spectre.Console;

namespace RecipesExercise1
{
    internal class Recipe
    {
        [JsonIgnore]
        private static readonly string _fileName = "recipe.json";
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public List<string> Instructions { get; set; } = new List<string>();
        public List<Category> Categories { get; set; } = new List<Category>();

        public Recipe()
        {
            Id = Guid.NewGuid(); // Generate a unique ID for each recipe
            Title = string.Empty;
        }

        public Recipe(Guid id, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            Id = id;
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            Categories = categories;
        }

        public static async Task<List<Recipe>?> Load(List<Category> categories)
        {
            ArgumentNullException.ThrowIfNull(categories);
            try
            {
                string text = await FileHandler.ReadAsync(_fileName);
                var recipes = await JsonHandler.DeserializeAsync<List<Recipe>?>(text);

                if (recipes is not null)
                {
                    for (int i = 0; i < recipes.Count; i++)
                        for (int j = 0; j < recipes[i].Categories.Count; j++)
                            recipes[i].Categories[j] = categories.First(y => y.Id == recipes[i].Categories[j].Id);
                }
                return recipes;
            }
            catch
            {
                return new List<Recipe>();
            }
        }

        public static async Task<bool> Save(List<Recipe>? recipes)
        {
            try
            {
                var json = await JsonHandler.SerializeAsync(recipes);
                return await FileHandler.WriteAsync(_fileName, json);
            }
            catch
            {
                return false;
            }
        }

        public static bool AddRecipe(List<Recipe> recipesList, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            ArgumentNullException.ThrowIfNull(recipesList);
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(ingredients);
            ArgumentNullException.ThrowIfNull(instructions);
            ArgumentNullException.ThrowIfNull(categories);

            if (recipesList.Any(x => x.Title == title))
                return false;
            recipesList.Add(new Recipe(Guid.NewGuid(), title, ingredients, instructions, categories));
            return true;
        }

        public static void RemoveRecipe(List<Recipe>? recipesList, Guid id)
        {
            ArgumentNullException.ThrowIfNull(recipesList);
            recipesList.RemoveAll(x => x.Id == id);
        }

        public static void UpdateRecipe(List<Recipe> recipesList, Guid id, bool removeIngredients, bool removeInstructions, string? title = null, List<string>? ingredients = null, List<string>? instructions = null, List<Category>? categories = null)
        {
            ArgumentNullException.ThrowIfNull(recipesList);

            var recipe = recipesList.First(x => x.Id == id);
            if (title is not null)
                recipe.Title = title;

            if (ingredients is not null)
                recipe.Ingredients.AddRange(ingredients);

            if (instructions is not null)
                recipe.Instructions.AddRange(instructions);

            if (categories is not null)
                recipe.Categories = categories;

            if (removeIngredients)
                recipe.Ingredients.Clear();

            if (removeInstructions)
                recipe.Instructions.Clear();
        }
    }
}
