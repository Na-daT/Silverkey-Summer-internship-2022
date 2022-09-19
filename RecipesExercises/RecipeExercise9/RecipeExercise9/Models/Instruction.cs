public class Instruction
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public Instruction()
    {
    }

    public Instruction(int id, string name, int recipeId, bool isActive)
    {
        Id = id;
        Name = name;
        RecipeId = recipeId;
        IsActive = isActive;
    }
}