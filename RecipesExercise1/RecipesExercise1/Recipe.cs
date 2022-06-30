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

        public static async Task<List<Recipe>?> Load(List<Category>? categories)
        {
            try
            {
                string text = await FileHandler.ReadAsync(_fileName);
                var recipes = await JsonHandler.DeserializeAsync<List<Recipe>?>(text);

                if (recipes is not null && categories is not null)
                {
                    for (int i = 0; i < recipes.Count; i++)
                        for (int j = 0; j < recipes[i].Categories.Count; j++)
                            recipes[i].Categories[j] = categories.First(y => y.Id == recipes[i].Categories[j].Id);
                }

                return recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static void AddRecipe(List<Recipe>? recipesList, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            if (recipesList is not null)
                recipesList.Add(new Recipe(Guid.NewGuid(), title, ingredients, instructions, categories));
        }

        public static void RemoveRecipe(List<Recipe>? recipesList, Guid id)
        {
            if (recipesList is not null)
                recipesList.RemoveAll(x => x.Id == id);
        }

        public static void UpdateRecipe(List<Recipe> recipesList, Guid id, string title, List<string> ingredients, List<string> instructions, List<Category> categories)
        {
            recipesList.RemoveAll(x => x.Id == id);
            recipesList.Add(new Recipe(id, title, ingredients, instructions, categories));
        }

        public static void List(List<Recipe>? recipesList)
        {
            var table = new Table();
            table.Expand();
            table.Border(TableBorder.Heavy);
            table.AddColumns("[bold]Title[/]", "[bold]Ingredients[/]", "[bold]Instructions[/]", "[bold]Categories[/]");
            if (recipesList is not null)
            {
                foreach (Recipe recipe in recipesList)
                {
                    List<string> CategoriesList = new List<string>();
                    foreach (Category category in recipe.Categories)
                    {
                        CategoriesList.Add(category.Name);
                    }
                    table.AddRow("[blue]" + recipe.Title.ToString() + "[/]", string.Join("\n", recipe.Ingredients) + "\n", string.Join("\n", recipe.Instructions) + "\n", string.Join("\n", CategoriesList) + "\n");
                }
            }
            AnsiConsole.Write(table);
        }
    }
}
