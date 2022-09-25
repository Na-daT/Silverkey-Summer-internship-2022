public class RecipeCategory
{
    public Category Category { get; set; } = new();
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }
    
    public RecipeCategory()
    {
    }

    public RecipeCategory(Category category, int categoryId, bool isActive)
    {
        Category = category;
        CategoryId = categoryId;
        IsActive = isActive;
    }
}
