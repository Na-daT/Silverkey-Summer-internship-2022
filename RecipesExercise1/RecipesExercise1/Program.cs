using RecipesExercise1;
using Spectre.Console;

namespace RecipesExercise1
{
    internal class Program
    {
        private static void MainMenuPrompt(List<Recipe> recipes)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Main Menu[/]"));
            string choice = AnsiConsole.Prompt<string>(
                new SelectionPrompt<string>()
                 .Title("[underline bold black on white]What would you like to do?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new[] {
                "List recipes",
                "Add recipe",
                "Remove recipe",
                "Update recipe",
                "Add Category",
                "Update Category",
                "Exit"
                        }
                    )
            );
            Choices(choice, recipes);
        }
        private static void Choices(string choice, List<Recipe> recipes)
        {
            switch (choice)
            {
                case "List recipes":
                    AnsiConsole.Write(new FigletText("List of recipes").Centered().Color(Color.Yellow));
                    Recipe.ListRecipes(recipes);
                    AnsiConsole.MarkupLine("[grey]Press a key to return[/]");
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes);
                    break;
                case "Add recipe":
                    AnsiConsole.Write(new Panel("[red]Add recipe[/]"));
                    break;
                case "Remove recipe":
                    AnsiConsole.Write(new Panel("[red]Remove recipe[/]"));
                    break;
                case "Update recipe":
                    AnsiConsole.Write(new Panel("[red]Update recipe[/]"));
                    break;
                case "Add Category":
                    AnsiConsole.Write(new Panel("[red]Add Category[/]"));
                    break;
            }
        }
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {

            List<Category> categoriesList = new List<Category>();
            categoriesList.Add(new Category("Category 1"));
            categoriesList.Add(new Category("Category 2"));
            categoriesList.Add(new Category("Category 3"));
            categoriesList.Add(new Category("Category 4"));
            categoriesList.Add(new Category("Category 5"));
            categoriesList.Add(new Category("Category 6"));
            Category.SaveCategory(categoriesList);
            List<Category> categories = await Category.GetCategory();

            List<Recipe> recipesList = new List<Recipe>();
            recipesList.Add(new Recipe(Guid.NewGuid(), "Recipe 1", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<Category>() { categoriesList[0], categoriesList[1] }));
            recipesList.Add(new Recipe(Guid.NewGuid(), "Recipe 2", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<Category>() { categoriesList[2], categoriesList[3] }));
            recipesList.Add(new Recipe(Guid.NewGuid(), "Recipe 3", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<Category>() { categoriesList[4], categoriesList[5] }));
            recipesList.Add(new Recipe(Guid.NewGuid(), "Recipe 4", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<Category>() { categoriesList[0], categoriesList[1] }));
            Recipe.SaveRecipes(recipesList);
            List<Recipe> recipes = await Recipe.GetRecipes();

            AnsiConsole.Write(new Rule("[yellow]Welcome[/]"));
            AnsiConsole.Write(new Panel("welcome to Nada's recipes app!"));
            AnsiConsole.MarkupLine("[grey]Press a key to continue[/]");
            AnsiConsole.Console.Input.ReadKey(true);
            MainMenuPrompt(recipes);


        }
    }
}
