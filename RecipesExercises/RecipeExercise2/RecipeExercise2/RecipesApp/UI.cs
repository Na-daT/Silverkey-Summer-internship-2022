using Spectre.Console;
using System;

namespace RecipesApp
{
    public class UI
    {
        public void StartPage()
        {
            AnsiConsole.Write(new FigletText("Welcome!").Centered().Color(Color.LightCoral));
            AnsiConsole.Write(new FigletText("Nada's recipes app!").Centered().Color(Color.Plum1));
            AnsiConsole.MarkupLine("[grey]Press a key to continue[/]");
            AnsiConsole.Console.Input.ReadKey(true);
            return;
        }

        public string MainMenuPrompt()
        {
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
            return choice;
        }

        public void ListRecipes(List<Recipe>? recipesList)
        {
            ArgumentNullException.ThrowIfNull(recipesList);

            AnsiConsole.Write(new FigletText("List of recipes").Centered().Color(Color.LightSlateBlue));

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
            AnsiConsole.MarkupLine("[grey]Press any key to return to main menu[/]");
            AnsiConsole.Console.Input.ReadKey(true);
            return;
        }

        public Recipe? PickRecipe(List<Recipe>? recipes)
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                 .Title("[bold white on black]Pick a recipe[/]")
                    .AddChoices(recipes.Select(r => r.Title.ToString())));
            return recipes.FirstOrDefault(r => r.Title.ToString() == choice);
        }

        public Category? PickCategory(List<Category> categories)
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

        private List<Category> PickMulCategory(List<Category> givenCategories)
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

        public string AskNewName(string type)
        {
            var name = AnsiConsole.Ask<string>($"What would you like to call your [hotpink3]{type}[/]?");
            return name;
        }
        public List<string> GetList(string type)
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

        public Recipe UpdatePrompt(Recipe? recipe, List<Category> categories)
        {
            ArgumentNullException.ThrowIfNull(recipe);
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
                    recipe.Title = newName;
                    break;
                case "Add ingredients":
                    var ingredients = GetList("ingredients");
                    recipe.Ingredients.AddRange(ingredients);
                    break;
                case "Remove all ingredients":
                    recipe.Ingredients.Clear();
                    break;
                case "Add instructions":
                    var instructions = GetList("instructions");
                    recipe.Instructions.AddRange(instructions);
                    break;
                case "Remove all instructions":
                    recipe.Instructions.Clear();
                    break;
                case "Add categories":
                    var UpdatedCategories = PickMulCategory(categories);
                    recipe.Categories.AddRange(UpdatedCategories);
                    break;
                case "Cancel":
                    AnsiConsole.Clear();
                    break;
                default:
                    return recipe;
            }
            return recipe;
        }

        public Recipe AddRecipe(List<Category> categories)
        {
            AnsiConsole.Write(new Panel("[mediumpurple]  Add new Recipe  [/]"));
            var pickedName = AskNewName("recipe");
            var pickedIngredients = GetList("ingredient");
            var pickedInstructions = GetList("instruction");
            var pickedCategories = PickMulCategory(categories);
            return new Recipe(Guid.NewGuid(), pickedName, pickedIngredients, pickedInstructions, pickedCategories);
        }

        public Recipe? UpdateRecipe(Recipe? recipe, List<Category>? categories)
        {
            ArgumentNullException.ThrowIfNull(recipe);
            ArgumentNullException.ThrowIfNull(categories);
            AnsiConsole.Write(new Panel("[mediumpurple]  Update exisitng recipe  [/]"));
            return UpdatePrompt(recipe, categories);
        }

        public Category AddCategory()
        {
            AnsiConsole.Write(new Panel("[mediumpurple]  Add new Category  [/]"));
            var pickedName = AskNewName("category");
            return new Category(pickedName);
        }

        public Category UpdateCategory(Category category)
        {
            AnsiConsole.Write(new Panel("[mediumpurple]Update Category[/]"));
            var newName = AskNewName("category");
            category.Name = newName;
            return category;
        }

        public void EndScreen()
        {
            AnsiConsole.Write(new Panel("[bold yellow on white]Goodbye![/]"));
            AnsiConsole.Console.Input.ReadKey(true);
        }

        public void SuccessMessage(string message)
        {
            AnsiConsole.Write(new Panel($"[bold green]{message}[/]"));
            AnsiConsole.Write(new Panel("[grey]Press any key to return to main menu[/]"));
            AnsiConsole.Console.Input.ReadKey(true);
        }

        public void ErrorMessage(string message)
        {
            AnsiConsole.Write(new Panel($"[bold red]{message}[/]"));
            AnsiConsole.Write(new Panel("[grey]Press any key to return to main menu[/]"));
            AnsiConsole.Console.Input.ReadKey(true);
        }
    }
}