using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace RecipeExercise3.Models;
public class Recipe
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
    public List<string> Instructions { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public Recipe()
    {
        Id = Guid.NewGuid();
    }

    public static List<Recipe> Load(List<Category> categories, List<Recipe> recipes)
    {
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(recipes);
        for (int i = 0; i < recipes.Count; i++)
            for (int j = 0; j < recipes[i].Categories.Count; j++)
                recipes[i].Categories[j] = categories.First(y => y.Id == recipes[i].Categories[j].Id);

        return recipes;
    }

    public static Recipe MatchCategory(Recipe recipe, List<Category> categories, List<Guid> categoriesIds)
    {
        ArgumentNullException.ThrowIfNull(recipe);
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(categoriesIds);
        for (int i = 0; i < categoriesIds.Count; i++)
        {
            var category = categories.FirstOrDefault(y => y.Id == categoriesIds[i]);
            if (category is null)
                throw new Exception("Could not find category");
            recipe.Categories.Add(category);
        }
        return recipe;
    }
}

public class RecipeValidator : AbstractValidator<Recipe>
{
    public RecipeValidator()
    {
        RuleFor(_ => _.Title).NotNull().Length(3, 50);
        RuleFor(_ => _.Categories).NotNull().WithMessage("Please pick a category");
    }
}