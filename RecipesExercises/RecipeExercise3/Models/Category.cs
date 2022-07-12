using System.ComponentModel.DataAnnotations;

namespace RecipesExercises.RecipeExercise3.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}