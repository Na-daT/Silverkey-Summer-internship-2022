namespace RecipesApp;
public class Recipe
{
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

    public static List<Recipe> Load(List<Category> categories, List<Recipe> recipes)
    {
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(recipes);

        for (int i = 0; i < recipes.Count; i++)
            for (int j = 0; j < recipes[i].Categories.Count; j++)
                recipes[i].Categories[j] = categories.First(y => y.Id == recipes[i].Categories[j].Id);

        return recipes;
    }
}