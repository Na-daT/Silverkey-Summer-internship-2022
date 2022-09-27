public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public Category()
    {
        Name = string.Empty;
    }

    public Category(int id, string name, bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }

    public Category(string name)
    {
        Name = name;
    }

    public Category(Category category)
    {
        Id = Guid.NewGuid().GetHashCode();
        Name = category.Name;
        IsActive = category.IsActive;
    }
}

