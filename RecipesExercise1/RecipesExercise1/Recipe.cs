using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipesExercise1
{
    [Serializable]
    internal class Recipe
    {
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
        public List<string> Categories { get; set; }

        public Recipe()
        {
            Title = string.Empty;
            Ingredients = new List<string>();
            Instructions = new List<string>();
            Categories = new List<string>();
        }

        public Recipe(string title, List<string> ingredients, List<string> instructions, List<string> categories)
        {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            Categories = categories;
        }

        public static async Task<List<Recipe>> GetRecipes()
        {
            try
            {
                using FileStream openStream = await FileHandler.ReadFile();
                List<Recipe>? Recipes =
                    await JsonSerializer.DeserializeAsync<List<Recipe>>(openStream);
                return Recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Recipe>();
            }
        }

        public static async Task<bool> SaveRecipes(List<Recipe> recipes)
        {
            try
            {
                string json = String.Empty;
                using (var stream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync<List<Recipe>>(stream, recipes);
                    stream.Position = 0;
                    using var reader = new StreamReader(stream);
                    json = await reader.ReadToEndAsync();
                }
                var result = await FileHandler.WriteFile(json);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
