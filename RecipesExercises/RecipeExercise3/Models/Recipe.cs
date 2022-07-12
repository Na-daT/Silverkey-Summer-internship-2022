using System.ComponentModel.DataAnnotations;
namespace RecipeExercise3.Models;

public class Recipe
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public List<string> Ingredients { get; set; }
    public List<string> Instructions { get; set; }
    [Required]
    public List<Category> Categories { get; set; }

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
