using System.ComponentModel.DataAnnotations;

namespace RecipeExercise3.Models;
public class Category
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(60, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 60 characters")]
    [Required]
    public string Name { get; set; } = string.Empty;

    public Category()
    {
        Id = Guid.NewGuid();
    }
}