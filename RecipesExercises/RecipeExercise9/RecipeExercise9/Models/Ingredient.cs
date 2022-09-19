public class Ingredient
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public Ingredient()
    {
    }

    public Ingredient(int id, string name, int recipeId, bool isActive)
    {
        Id = id;
        Name = name;
        RecipeId = recipeId;
        IsActive = isActive;
    }
}

