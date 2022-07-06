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
            var name = AnsiConsole.Ask<string>($"What would you like to call your [hotpink3]{type}[/]?");
            return name;
        }

        public static List<string> GetList(string type)
        {
            var list = new List<string>();
            do
            {
                AnsiConsole.Clear();
                list.Add(AnsiConsole.Prompt(
                    new TextPrompt<string>($"enter an [hotpink3]{type}[/]:"))
                );
                AnsiConsole.MarkupLine($"[grey]press any key to add another {type}, or press escape to continue[/]");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            return list;
        }

        public static List<Category> PickMulCategory(List<Category>? givenCategories)
        {
            ArgumentNullException.ThrowIfNull(givenCategories);

            AnsiConsole.Clear();
            var list = new List<Category>();
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
            AnsiConsole.MarkupLine("[grey]press any key to add another category, or press escape to continue[/]");
            return list;
        }

        public static Recipe? PickRecipe(List<Recipe>? recipes)
        {
            ArgumentNullException.ThrowIfNull(recipes);

            AnsiConsole.Clear();
            var availableRecipeNames = (from Recipe recipe in recipes
                                        select recipe.Title).ToList();
            var recipeName = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Pick a recipe:").AddChoices(availableRecipeNames)
            );
            return recipes.Find(x => x.Title == recipeName);
        }

        public static Category? PickCategory(List<Category>? categories)
        {
            ArgumentNullException.ThrowIfNull(categories);

            var availableCategoriesNames = (from Category category in categories
                                            select category.Name).ToList();
            AnsiConsole.Clear();
            var categoryName = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Pick a category:").AddChoices(availableCategoriesNames)
            );
            return categories.Find(x => x.Name == categoryName);
        }

        public static bool UpdatePrompt(Recipe? recipe, List<Recipe>? recipes, List<Category> categories)
        {
            ArgumentNullException.ThrowIfNull(recipe);
            ArgumentNullException.ThrowIfNull(recipes);
            ArgumentNullException.ThrowIfNull(categories);

            AnsiConsole.Clear();
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                 .Title("[underline bold black on white]What would you like to update?[/]")
                    .AddChoices(new[] {
                        "Name",
                        "Add ingredients",
                        "Remove all ingredients",
                        "Add instructions",
                        "Remove all instructions",
                        "Edit categories",
                        "Cancel"
                    }));
            switch (choice)
            {
                case "Name":
                    string newName = AskNewName("recipe");
                    Recipe.UpdateRecipe(recipes, recipe.Id, false, false, newName);
                    break;
                case "Add ingredients":
                    var ingredients = GetList("ingredients");
                    Recipe.UpdateRecipe(recipes, recipe.Id, false, false, null, ingredients);
                    break;
                case "Remove all ingredients":
                    Recipe.UpdateRecipe(recipes, recipe.Id, true, false);
                    break;
                case "Add instructions":
                    var instructions = GetList("instructions");
                    Recipe.UpdateRecipe(recipes, recipe.Id, false, false, null, null, instructions);
                    break;
                case "Remove all instructions":
                    Recipe.UpdateRecipe(recipes, recipe.Id, false, true);
                    break;
                case "Add categories":
                    var UpdatedCategories = PickMulCategory(categories);
                    Recipe.UpdateRecipe(recipes, recipe.Id, false, false, null, null, null, UpdatedCategories);
                    break;
                case "Cancel":
                    AnsiConsole.Clear();
                    return false;
                default:
                    return false;
            }
            return true;
        }

        public static void List(List<Recipe> recipesList)
        {
            ArgumentNullException.ThrowIfNull(recipesList);

            var table = new Table();
            table.Expand();
            table.Border(TableBorder.Heavy);
            table.AddColumns("[bold]Title[/]", "[bold]Ingredients[/]", "[bold]Instructions[/]", "[bold]Categories[/]");
            foreach (Recipe recipe in recipesList)
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

        private static void MainMenuPrompt(List<Recipe>? recipes, List<Category>? categories)
        {
            ArgumentNullException.ThrowIfNull(recipes);
            ArgumentNullException.ThrowIfNull(categories);

            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Main Menu").Centered().Color(Color.Plum1));
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                 .Title("[bold white on black]What would you like to do?[/]")
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

        private static void Choices(string choice, List<Recipe> recipes, List<Category> categories)
        {
            ArgumentNullException.ThrowIfNull(choice);
            ArgumentNullException.ThrowIfNull(recipes);
            ArgumentNullException.ThrowIfNull(categories);

            AnsiConsole.Clear();
            switch (choice)
            {
                case "List recipes":
                    AnsiConsole.Write(new FigletText("List of recipes").Centered().Color(Color.LightSlateBlue));
                    List(recipes);
                    AnsiConsole.MarkupLine("[grey]Press any key to return to main menu[/]");
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Add recipe":
                    if (categories is not null && !categories.Any())
                    {
                        AnsiConsole.Write(new Panel("[red]You need to add a category first![/] \n[grey]Press any key to return to main menu[/]"));
                    }
                    else
                    {
                        AnsiConsole.Write(new Panel("[mediumpurple]  Add new Recipe  [/]"));
                        var pickedName = AskNewName("recipe");
                        var pickedIngredients = GetList("ingredient");
                        var pickedInstructions = GetList("instruction");
                        var pickedCategories = PickMulCategory(categories);
                        if (Recipe.AddRecipe(recipes, pickedName, pickedIngredients, pickedInstructions, pickedCategories))
                            AnsiConsole.Write(new Panel("[green]Recipe added successfully![/] \n[grey]Press any key to return to main menu[/]"));
                        else
                            AnsiConsole.Write(new Panel("[red]a Recipe with the same name exists![/] \n[grey]Press any key to return to main menu[/]"));
                    }
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Remove recipe":
                    if (recipes is not null && !recipes.Any())
                    {
                        AnsiConsole.Write(new Panel("[red]You need to add a recipy first[!/] \n[grey]Press any key to return to main menu[/]"));
                        AnsiConsole.Console.Input.ReadKey(true);
                        MainMenuPrompt(recipes, categories);
                    }
                    else
                    {
                        AnsiConsole.Write(new Panel("[mediumpurple]  Remove Recipe  [/]"));
                        var pickedRecipe = PickRecipe(recipes);
                        if (pickedRecipe is not null)
                            Recipe.RemoveRecipe(recipes, pickedRecipe.Id);
                        Console.Clear();
                        AnsiConsole.MarkupLine("[bold yellow]Success![/][grey]Press any key to return to main menu[/]");
                        AnsiConsole.Console.Input.ReadKey(true);
                        MainMenuPrompt(recipes, categories);
                    }
                    break;
                case "Update recipe":
                    if (recipes is not null && !recipes.Any())
                    {
                        AnsiConsole.Write(new Panel("[red]You need to add a recipy first![/] \n[grey]Press any key to return to main menu[/]"));
                        AnsiConsole.Console.Input.ReadKey(true);
                        MainMenuPrompt(recipes, categories);
                    }
                    else
                    {
                        AnsiConsole.Write(new Panel("[mediumpurple]  Update exisitng recipe  [/]"));
                        var recipeToUpdate = PickRecipe(recipes);
                        bool updateResult = UpdatePrompt(recipeToUpdate, recipes, categories);
                        if (updateResult)
                            AnsiConsole.Write(new Panel("[green]Recipe updated successfully![/] \n[grey]Press any key to return to main menu[/]"));
                        else
                            AnsiConsole.Write(new Panel("[Red]Cancelled![/] \n[grey]Press any key to return to main menu[/]"));
                        MainMenuPrompt(recipes, categories);
                    }
                    break;
                case "Add Category":
                    AnsiConsole.Write(new Panel("[mediumpurple]Add Category[/]"));
                    var newName = AskNewName("category");
                    if (Category.AddCategory(categories, newName))
                        AnsiConsole.Write(new Panel("[green]Category Added successfully![/] \n[grey]Press any key to return to main menu[/]"));
                    else
                        AnsiConsole.Write(new Panel("[Red]Category already exists![/] \n[grey]Press any key to return to main menu[/]"));
                    AnsiConsole.Console.Input.ReadKey(true);
                    MainMenuPrompt(recipes, categories);
                    break;
                case "Update Category":
                    if (categories is not null && !categories.Any())
                    {
                        AnsiConsole.Write(new Panel("[red]You need to add a category first[/] \n[grey]Press any key to return to main menu[/]"));
                        AnsiConsole.Console.Input.ReadKey(true);
                        MainMenuPrompt(recipes, categories);
                    }
                    else
                    {
                        AnsiConsole.Write(new Panel("[mediumpurple]Update Category[/]"));
                        var categoryToUpdate = PickCategory(categories);
                        var updatedCategoryName = AskNewName("category");
                        Category.EditCategory(categoryToUpdate, updatedCategoryName);
                        Console.Clear();
                        AnsiConsole.MarkupLine("[bold yellow]Success![/][grey]Press any key to return to main menu[/]");
                        AnsiConsole.Console.Input.ReadKey(true);
                        MainMenuPrompt(recipes, categories);
                    }
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
                default:
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
            var recipes = await Recipe.Load(categories!);

            AnsiConsole.Write(new FigletText("Welcome!").Centered().Color(Color.LightCoral));
            AnsiConsole.Write(new FigletText("Nada's recipes app!").Centered().Color(Color.Plum1));
            AnsiConsole.MarkupLine("[grey]Press a key to continue[/]");
            AnsiConsole.Console.Input.ReadKey(true);
            MainMenuPrompt(recipes, categories);
        }
    }
}
