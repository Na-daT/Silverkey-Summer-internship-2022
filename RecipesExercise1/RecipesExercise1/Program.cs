using RecipesExercise1;
using Spectre.Console;
using System;
using System.Linq;

namespace RecipesExercise1
{
    internal class Program
    {
        public static string AskNewName(string type)
        {
            var name = AnsiConsole.Ask<string>($"What would you like to call your [red]{type}[/]?");
            return name;
        }

        public static List<string> GetList(string type)
        {
            var list = new List<string>();
            do
            {
                AnsiConsole.Clear();
                list.Add(AnsiConsole.Prompt(
                    new TextPrompt<string>($"enter an [red]{type}[/]:"))
                );
                AnsiConsole.MarkupLine($"[grey]press any key to add another {type}, or press escape to continue[/]");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            return list;
        }

        public static List<Category> PickMulCategory(List<Category>? givenCategories)
        {
            AnsiConsole.Clear();

            var list = new List<Category>();
            if (givenCategories is not null)
            {
                var availableCategoriesNames = (from Category category in givenCategories
                                                select category.Name).ToList();

                var categoryName = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>().Title("Pick a category:").AddChoices(availableCategoriesNames)
                );
                foreach (string name in categoryName)
                {
                    var result = givenCategories.Find(x => x.Name == name);
                    if (result is not null)
                        list.Add(result);
                }
            }

            AnsiConsole.MarkupLine("[grey]press any key to add another category, or press escape to continue[/]");
            return list;
        }

        public static Recipe? PickRecipe(List<Recipe>? recipes)
        {
            AnsiConsole.Clear();

            if (recipes is not null)
            {
                var availableRecipeNames = (from Recipe recipe in recipes
                                            select recipe.Title).ToList();
                var recipeName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().Title("Pick a recipe:").AddChoices(availableRecipeNames)
                );
                return recipes.Find(x => x.Title == recipeName);
            }
            return null;
        }

        public static Category? PickCategory(List<Category>? categories)
        {
            if (categories is not null)
            {
                var availableCategoriesNames = (from Category category in categories
                                                select category.Name).ToList();
                AnsiConsole.Clear();
                var categoryName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>().Title("Pick a category:").AddChoices(availableCategoriesNames)
                );
                return categories.Find(x => x.Name == categoryName);
            }
            return null;
        }
        public static bool UpdatePrompt(Recipe? recipe)
        {
            AnsiConsole.Clear();
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                 .Title("[underline bold black on white]What would you like to update?[/]")
                    .AddChoices(new[] {
                        "Name",
                        "Add ingredients",
                        "Remove ingredients",
                        "Add instructions",
                        "Remove instructions",
                        "Add categories",
                        "Remove categories",
                        "Cancel"
                    }));

            switch (choice)
            {
                case "Name":
                    string newName = AskNewName("recipe");
                    break;
                /*
                case "Add ingredients":
                    recipe.Ingredients.AddRange(GetList("ingredient"));
                    break;
                case "Remove ingredients":
                    //recipe.Ingredients.RemoveAll(x => x == GetList("ingredient"));
                    break;
                case "Add instructions":
                    recipe.Instructions.AddRange(GetList("instruction"));
                    break;
                case "Remove instructions":
                    //recipe.Instructions.RemoveAll(x => x == GetList("instruction"));
                    break;
                case "Add categories":
                    //recipe.Categories.AddRange(PickCategory(Recipe.Categories));
                    break;
                case "Remove categories":
                    //recipe.Categories.RemoveAll(x => x == PickCategory(Recipe.Categories));
                    break;
                case "Cancel":
                    AnsiConsole.Clear();
                    return false;
                */
                default:
                    return false;
            }
            return true;

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

        private static void MainMenuPrompt(List<Recipe>? recipes, List<Category>? categories)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Main Menu[/]"));
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                 .Title("[underline bold black on white]What would you like to do?[/]")
                    .AddChoices(new[] {
                        "List recipes",
                        "Add recipe",
                        "Remove recipe",
                        "Update recipe",
                        "Add Category",
                        "Update Category",
                        "Exit"
                    }));
            Choices(choice, recipes, categories);
        }

        private static void Choices(string choice, List<Recipe>? recipes, List<Category>? categories)
        {
            AnsiConsole.Clear();
            switch (choice)
            {
                case "List recipes":
                    AnsiConsole.Write(new FigletText("List of recipes").Centered().Color(Color.Yellow));
                    List(recipes);
                    AnsiConsole.MarkupLine("[grey]Press any key to return to main menu[/]");
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Add recipe":
                    AnsiConsole.Write(new Rule("[bold blue on white]  Add new Recipe  [/]"));
                    var pickedName = AskNewName("recipe");
                    var pickedIngredients = GetList("ingredient");
                    var pickedInstructions = GetList("instruction");
                    var pickedCategories = PickMulCategory(categories);
                    Recipe.AddRecipe(recipes, pickedName, pickedIngredients, pickedInstructions, pickedCategories);
                    Console.Clear();
                    AnsiConsole.MarkupLine("[bold yellow]Success![/][grey]Press any key to return to main menu[/]");
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Remove recipe":
                    AnsiConsole.Write(new Rule("[bold blue on white]  Remove Recipe  [/]"));
                    var pickedRecipe = PickRecipe(recipes);
                    if (pickedRecipe is not null)
                        Recipe.RemoveRecipe(recipes, pickedRecipe.Id);
                    break;
                case "Update recipe":
                    AnsiConsole.Write(new Panel("[red]Update recipe[/]"));
                    var recipeToUpdate = PickRecipe(recipes);
                    bool updateResult = UpdatePrompt(recipeToUpdate);
                    if (updateResult) { AnsiConsole.MarkupLine("[bold yellow]Success![/][grey]Press any key to return to main menu[/]"); }
                    else { AnsiConsole.MarkupLine("[bold yellow]Canceled![/][grey]Press any key to return to main menu[/]"); }
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Add Category":
                    AnsiConsole.Write(new Panel("[red]Add Category[/]"));
                    var newName = AskNewName("category");
                    Category.AddCategory(categories, newName);

                    Console.Clear();
                    AnsiConsole.MarkupLine("[bold yellow]Success![/][grey]Press any key to return to main menu[/]");
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Update Category":
                    AnsiConsole.Write(new Panel("[red]Update Category[/]"));
                    var categoryToUpdate = PickCategory(categories);
                    var updatedCategoryName = AskNewName("category");
                    Category.EditCategory(categoryToUpdate, updatedCategoryName);

                    Console.Clear();
                    AnsiConsole.MarkupLine("[bold yellow]Success![/][grey]Press any key to return to main menu[/]");
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Exit":
                    AnsiConsole.Write(new Panel("[bold yellow on white]Saving...[/]"));
                    var tasks = new List<Task>
                    {
                        Task.Run(() => Recipe.Save(recipes)),
                        Task.Run(() => Category.Save(categories))
                    };
                    Task.WaitAll(tasks.ToArray());
                    break;
            }
        }
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
            var categories = await Category.Load();
            var recipes = await Recipe.Load(categories);

            AnsiConsole.Write(new Rule("[yellow]Welcome[/]"));
            AnsiConsole.Write(new Panel("welcome to Nada's recipes app!"));
            AnsiConsole.MarkupLine("[grey]Press a key to continue[/]");
            AnsiConsole.Console.Input.ReadKey(true);
            MainMenuPrompt(recipes, categories);
        }
    }
}
