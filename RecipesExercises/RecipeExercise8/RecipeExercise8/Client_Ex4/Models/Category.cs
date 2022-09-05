using System.ComponentModel.DataAnnotations;
using FluentValidation.AspNetCore;
using FluentValidation;


namespace Client_Ex4.Models;
public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Category()
    {
    }
}
public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(_ => _.Name).NotNull().Length(3, 50);
    }
}