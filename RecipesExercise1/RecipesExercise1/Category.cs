using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipesExercise1
{
    public class Category
    {
        [JsonIgnore]
        private static readonly string _fileName = "categories.json";
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
        public static async Task<List<Category>?> GetCategory()
        {
            try
            {
                string text = await FileHandler.ReadAsync(_fileName);
                return await JsonHandler.DeserializeAsync<List<Category>?>(text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Category>();
            }
        }

        public static async Task<bool> SaveCategory(List<Category> categories)
        {
            try
            {
                var json = await JsonHandler.SerializeAsync<List<Category>>(categories);
                return await FileHandler.WriteAsync(_fileName, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static void EditCategory(Category category, string newName)
        {
            category.Name = newName;
        }
    }
}