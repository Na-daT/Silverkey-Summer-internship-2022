using System.ComponentModel.DataAnnotations;
using FluentValidation.AspNetCore;
using FluentValidation;


namespace RecipeExercise6.Models;
public class Category
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Category()
    {
        Id = Guid.NewGuid();
    }
}
public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(_ => _.Name).NotNull().Length(3, 50);
    }
}