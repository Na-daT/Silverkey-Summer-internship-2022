using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public class Recipe
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
    
    [Migration(12)]
    public class _102_RecipeTableSeed : Migration
    {
        public static List<Recipe> recipes = new()
        {
            new Recipe
            {
                Id = 1,
                Title = "Banana Cake\t",
                IsActive = true
            },
            new Recipe
            {
                Id = 2,
                Title = "Apple Pie",
                IsActive = true
            },
            new Recipe
            {
                Id = 3,
                Title = "Shrimp Saute\t",
                IsActive = true
            },
            new Recipe
            {
                Id = 4,
                Title = "Fried Chicken",
                IsActive = true
            },
            new Recipe
            {
                Id = 5,
                Title = "Fish and chips",
                IsActive = true
            },
            new Recipe
            {
                Id = 6,
                Title = "Egyptian Shawerma",
                IsActive = true
            },
            new Recipe
            {
                Id = 7,
                Title = "Classic Shepherd\u0027s pie",
                IsActive = true
            },
            new Recipe
            {
                Id = 8,
                Title = "Lemony spaghetti",
                IsActive = true
            },
            new Recipe
            {
                Id = 9,
                Title = "Red Curry chicken",
                IsActive = true
            },
            new Recipe
            {
                Id = 10,
                Title = "Chicken Thighs",
                IsActive = true
            },
            new Recipe
            {
                Id = 11,
                Title = "Peanut Butter Cookies",
                IsActive = true
            },
            new Recipe
            {
                Id = 12,
                Title = "Mac and Cheese",
                IsActive = true
            },
            new Recipe
            {
                Id = 13,
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
                        id = recipe.Id,
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
