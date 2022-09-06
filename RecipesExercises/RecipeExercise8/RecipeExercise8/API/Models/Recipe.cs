public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new ();
    public List<Instruction> Instructions { get; set; } = new ();
    public List<Category> Categories { get; set; } = new ();
    public bool IsActive { get; set; }

    public Recipe()
    {
        Title = string.Empty;
    }

    public Recipe(int id, string title, List<Ingredient> ingredients, List<Instruction> instructions, List<Category> categories, bool isActive)
    {
        Id = id;
        Title = title;
        Ingredients = ingredients;
        Instructions = instructions;
        Categories = categories;
        IsActive = isActive;
    }
}