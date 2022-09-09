using FluentMigrator;
using RecipesDB.Migrations;
namespace RecipesDB.Seed
{
    public record Category
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    [Migration(13)]
    public class _103_CategoryTableSeed : Migration
    {
        public static List<Category> categories = new()
        {
            new Category
            {
                Name = "Vegan",
                IsActive = true
            },
            new Category
            {
                Name = "Hot",
                IsActive = true
            },
            new Category
            {
                Name = "Quick",
                IsActive = true
            },
            new Category
            {
                Name = "Vegetarian",
                IsActive = true
            },
            new Category
            {
                Name = "Dairy",
                IsActive = true
            },
            new Category
            {
                Name = "Italian",
                IsActive = true
            },
            new Category
            {
                Name = "Egyptian",
                IsActive = true
            },
            new Category
            {
                Name = "Seafood",
                IsActive = true
            },
            new Category
            {
                Name = "Appetizer",
                IsActive = true
            },
            new Category
            {
                Name = "Beverage",
                IsActive = true
            },
            new Category
            {
                Name = "Side Dish",
                IsActive = true
            },
            new Category
            {
                Name = "Salad",
                IsActive = true
            },
            new Category
            {
                Name = "Bread",
                IsActive = true
            },
            new Category
            {
                Name = "Sauce",
                IsActive = true
            },
            new Category
            {
                Name = "Casserole",
                IsActive = true
            },
            new Category
            {
                Name = "Soup",
                IsActive = true
            },
            new Category
            {
                Name = "Cocktail",
                IsActive = true
            },
            new Category
            {
                Name = "Dinner",
                IsActive = true
            },
            new Category
            {
                Name = "Baking",
                IsActive = true
            },
            new Category
            {
                Name = "Healthy",
                IsActive = true
            },
            new Category
            {
                Name = "Dessert",
                IsActive = true
            },
            new Category
            {
                Name = "Pasta",
                IsActive = true
            },
            new Category
            {
                Name = "Poultry",
                IsActive = true
            },
            new Category
            {
                Name = "Breakfast",
                IsActive = true
            },
        };

        public override void Up()
        {
            foreach (var c in categories)
            {
                Insert.IntoTable(Tables.Category)
                    .Row(new
                    {
                        name = c.Name,
                        is_active = c.IsActive
                    });
            }
        }

        public override void Down()
        {
        }
    }
}
