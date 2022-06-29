using System.Text.Json.Serialization;
using Spectre.Console;

namespace RecipesExercise1
{
    internal class Recipe
    {
        [JsonIgnore]
        private static readonly string _fileName = "recipe.json";
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();

        public Recipe()
        {
            Id = Guid.NewGuid(); // Generate a unique ID for each recipe
            Title = string.Empty;
            Ingredients = new List<string>();
            Instructions = new List<string>();
            Categories = new List<Category>();
        }

        public Recipe(Guid id, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            Id = id;
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            Categories = categories;
        }

        public static async Task<List<Recipe>?> GetRecipes()
        {
            try
            {
                string text = await FileHandler.ReadAsync(_fileName);
                return await JsonHandler.DeserializeAsync<List<Recipe>?>(text);
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
                var json = await JsonHandler.SerializeAsync<List<Recipe>>(recipes);
                return await FileHandler.WriteAsync(_fileName, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void AddRecipe(List<Recipe> RecipesList, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            RecipesList.Add(new Recipe(Guid.NewGuid(), title, ingredients, instructions, categories));
        }

        public void RemoveRecipe(List<Recipe> RecipesList, Guid id)
        {
            RecipesList.RemoveAll(x => x.Id == id);
        }

        public void UpdateRecipe(List<Recipe> RecipesList, Guid id, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            RecipesList.RemoveAll(x => x.Id == id);
            RecipesList.Add(new Recipe(id, title, ingredients, instructions, categories));
        }

        public static void ListRecipes(List<Recipe> RecipesList)
        {
            var table = new Table();
            table.Expand();
            table.Border(TableBorder.Heavy);
            table.AddColumns("[bold]Title[/]", "[bold]Ingredients[/]", "[bold]Instructions[/]", "[bold]Categories[/]");
            foreach (Recipe recipe in RecipesList)
            {
                List<string> CategoriesList = new List<string>();
                foreach (Category category in recipe.Categories)
                {
                    CategoriesList.Add(category.Name);
                }
                table.AddRow("[blue]" + recipe.Title.ToString() + "[/]", string.Join("\n", recipe.Ingredients) + "\n", string.Join("\n", recipe.Instructions) + "\n", string.Join("\n", CategoriesList) + "\n");
            }
            AnsiConsole.Write(table);
        }
    }
}
