namespace RecipesExercise1
{
    [Serializable]
    internal class Category
    {
        string Id { get; set; }
        string Name { get; set; }

        public Category()
        {
            Id = Guid.NewGuid().ToString(); // Generate a unique ID for each category
            Name = string.Empty;
        }

        public Category(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Category(string name)
        {
            Id = Guid.NewGuid().ToString(); // Generate a unique ID for each category
            Name = name;
        }

        public static void EditCategory(Category category, string newName)
        {
            category.Name = newName;
        }
    }
}