using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Server;
public static class Utility
{
    public static List<Recipe> LoadRecipes()
    {
        var recipes = new List<Recipe>();
        using (var streamReader = File.OpenText("/Data/recipe.json"))
        {
            var jsonRecipes = JsonConvert.DeserializeObject<List<Recipe>>(streamReader.ReadToEnd());
            if (jsonRecipes == null)
            {
                jsonRecipes = new List<Recipe>();
            }
            foreach (var recipe in jsonRecipes)
            {
                recipes.Add(new Recipe { Id = recipe.Id, Title = recipe.Title, Ingredients = recipe.Ingredients, Instructions = recipe.Instructions, Categories = recipe.Categories });
            }
        }
        return recipes;
    }

    public static List<Category> LoadCategories()
    {
        var categories = new List<Category>();
        using (var streamReader = File.OpenText("Data/category.json"))
        {
            var jsonCategories = JsonConvert.DeserializeObject<List<Category>>(streamReader.ReadToEnd());
            if (jsonCategories == null)
            {
                jsonCategories = new List<Category>();
            }
            foreach (var category in jsonCategories)
            {
                categories.Add(new Category { Id = category.Id, Name = category.Name });
            }
        }
        return categories;
    }

    public static void SaveRecipes(List<Recipe> recipes)
    {
        var jsonRecipes = JsonConvert.SerializeObject(recipes);
        File.WriteAllText("Data/recipe.json", jsonRecipes);
    }

    public static void SaveCategories(List<Category> categories)
    {
        var jsonCategories = JsonConvert.SerializeObject(categories);
        File.WriteAllText("Data/category.json", jsonCategories);
    }
}