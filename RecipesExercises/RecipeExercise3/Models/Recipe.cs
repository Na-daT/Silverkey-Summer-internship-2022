using System.ComponentModel.DataAnnotations;
namespace RecipesExercises.RecipeExercise3.Models
{
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
    }
}