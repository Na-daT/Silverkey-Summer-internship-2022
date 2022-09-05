using FluentMigrator;
using RecipesDB.Migrations;
namespace RecipesDB.Seed
{
    public class Category
    {
        public int Id { get; set; } 
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
                Id = 1,
                Name = "Vegan",
                IsActive = true
            },
            new Category
            {
                Id = 2,
                Name = "Hot",
                IsActive = true
            },
            new Category
            {
                Id = 3,
                Name = "Quick",
                IsActive = true
            },
            new Category
            {
                Id = 4,
                Name = "Vegetarian",
                IsActive = true
            },
            new Category
            {
                Id = 5,
                Name = "Dairy",
                IsActive = true
            },
            new Category
            {
                Id = 6,
                Name = "Italian",
                IsActive = true
            },
            new Category
            {
                Id = 7,
                Name = "Egyptian",
                IsActive = true
            },
            new Category
            {
                Id = 8,
                Name = "Seafood",
                IsActive = true
            },
            new Category
            {
                Id = 9,
                Name = "Appetizer",
                IsActive = true
            },
            new Category
            {
                Id = 10,
                Name = "Beverage",
                IsActive = true
            },
            new Category
            {
                Id = 11,
                Name = "Side Dish",
                IsActive = true
            },
            new Category
            {
                Id = 12,
                Name = "Salad",
                IsActive = true
            },
            new Category
            {
                Id = 13,
                Name = "Bread",
                IsActive = true
            },
            new Category
            {
                Id = 14,
                Name = "Sauce",
                IsActive = true
            },
            new Category
            {
                Id = 15,
                Name = "Casserole",
                IsActive = true
            },
            new Category
            {
                Id = 16,
                Name = "Soup",
                IsActive = true
            },
            new Category
            {
                Id = 17,
                Name = "Cocktail",
                IsActive = true
            },
            new Category
            {
                Id = 18,
                Name = "Dinner",
                IsActive = true
            },
            new Category
            {
                Id = 19,
                Name = "Baking",
                IsActive = true
            },
            new Category
            {
                Id = 20,
                Name = "Healthy",
                IsActive = true
            },
            new Category
            {
                Id = 21,
                Name = "Dessert",
                IsActive = true
            },
            new Category
            {
                Id = 22,
                Name = "Pasta",
                IsActive = true
            },
            new Category
            {
                Id = 23,
                Name = "Poultry",
                IsActive = true
            },
            new Category
            {
                Id = 24,
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
                        id = c.Id,
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
