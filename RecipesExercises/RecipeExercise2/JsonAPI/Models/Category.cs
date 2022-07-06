public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Category()
    {
        Id = Guid.NewGuid(); // Generate a unique ID for each category
        Name = string.Empty;
    }

    public Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Category(string name)
    {
        Id = Guid.NewGuid(); // Generate a unique ID for each category
        Name = name;
    }
}
