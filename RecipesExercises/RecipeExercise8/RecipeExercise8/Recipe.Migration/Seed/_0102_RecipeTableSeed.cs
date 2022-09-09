using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public record Recipe
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }

    [Migration(12)]
    public class _0102_RecipeTableSeed : Migration
    {
        public static List<Recipe> recipes = new()
        {
            new Recipe
            {
                Title = "Banana Cake\t",
                IsActive = true
            },
            new Recipe
            {
                Title = "Apple Pie",
                IsActive = true
            },
            new Recipe
            {
                Title = "Shrimp Saute\t",
                IsActive = true
            },
            new Recipe
            {
                Title = "Fried Chicken",
                IsActive = true
            },
            new Recipe
            {
                Title = "Fish and chips",
                IsActive = true
            },
            new Recipe
            {
                Title = "Egyptian Shawerma",
                IsActive = true
            },
            new Recipe
            {
                Title = "Classic Shepherd\u0027s pie",
                IsActive = true
            },
            new Recipe
            {
                Title = "Lemony spaghetti",
                IsActive = true
            },
            new Recipe
            {
                Title = "Red Curry chicken",
                IsActive = true
            },
            new Recipe
            {
                Title = "Chicken Thighs",
                IsActive = true
            },
            new Recipe
            {
                Title = "Peanut Butter Cookies",
                IsActive = true
            },
            new Recipe
            {
                Title = "Mac and Cheese",
                IsActive = true
            },
            new Recipe
            {
                Title = "Fudge Brownies \u2764",
                IsActive = true
            }
        };

        public override void Up()
        {
            foreach (var recipe in recipes)
            {
                Insert.IntoTable(Tables.Recipe)
                    .Row(new
                    {
                        title = recipe.Title,
                        is_active = recipe.IsActive
                    });
            }
        }

        public override void Down()
        {
        }
    }
}
