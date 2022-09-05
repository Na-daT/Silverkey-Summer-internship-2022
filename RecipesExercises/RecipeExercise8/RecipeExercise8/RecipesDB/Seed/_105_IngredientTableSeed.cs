using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; }
    }
    [Migration(15)]
    public class _105_IngredientTableSeed : Migration
    {
        public static List<RecipeIngredient> recipeIngredients = new()
        {
            new RecipeIngredient
            {
                Id = 1,
                RecipeId = 1,
                Name = "sugar"
            },
            new RecipeIngredient
            {
                Id = 2,
                RecipeId = 1,
                Name = "eggs"
            },
            new RecipeIngredient
            {
                Id = 3,
                RecipeId = 1,
                Name = "sour milk\t"
            },
            new RecipeIngredient
            {
                Id = 4,
                RecipeId = 2,
                Name = "Apple"
            },
            new RecipeIngredient
            {
                Id = 5,
                RecipeId = 2,
                Name = "Flour"
            },
            new RecipeIngredient
            {
                Id = 6,
                RecipeId = 2,
                Name = "Sugar"
            },
            new RecipeIngredient
            {
                Id = 7,
                RecipeId = 2,
                Name = "Salt"
            },
            new RecipeIngredient
            {
                Id = 8,
                RecipeId = 3,
                Name = "shrimp"
            },
            new RecipeIngredient
            {
                Id = 9,
                RecipeId = 3,
                Name = "garlic"
            },
            new RecipeIngredient
            {
                Id = 10,
                RecipeId = 3,
                Name = "mushrooms"
            },
            new RecipeIngredient
            {
                Id = 11,
                RecipeId = 3,
                Name = "green pepper"
            },
            new RecipeIngredient
            {
                Id = 12,
                RecipeId = 4,
                Name = "chicken breasts"
            },
            new RecipeIngredient
            {
                Id = 13,
                RecipeId = 4,
                Name = "milk"
            },
            new RecipeIngredient
            {
                Id = 14,
                RecipeId = 4,
                Name = "onion"
            },
            new RecipeIngredient
            {
                Id = 15,
                RecipeId = 4,
                Name = "parley"
            },
            new RecipeIngredient
            {
                Id = 16,
                RecipeId = 5,
                Name = "fish sticks"
            },
            new RecipeIngredient
            {
                Id = 17,
                RecipeId = 5,
                Name = "potatoes"
            },
            new RecipeIngredient
            {
                Id = 18,
                RecipeId = 5,
                Name = "onion"
            },
            new RecipeIngredient
            {
                Id = 19,
                RecipeId = 5,
                Name = "milk"
            },
            new RecipeIngredient
            {
                Id = 20,
                RecipeId = 6,
                Name = "chicken strips"
            },
            new RecipeIngredient
            {
                Id = 21,
                RecipeId = 6,
                Name = "garlic"
            },
            new RecipeIngredient
            {
                Id = 22,
                RecipeId = 6,
                Name = "onion"
            },
            new RecipeIngredient
            {
                Id = 23,
                RecipeId = 6,
                Name = "fries"
            },
            new RecipeIngredient
            {
                Id = 24,
                RecipeId = 7,
                Name = "1 tbsp olive oil"
            },
            new RecipeIngredient
            {
                Id = 25,
                RecipeId = 7,
                Name = "2 garlic cloves, crushed"
            },
            new RecipeIngredient
            {
                Id = 26,
                RecipeId = 7,
                Name = "2 tsp ground cumin"
            },
            new RecipeIngredient
            {
                Id = 27,
                RecipeId = 7,
                Name = "2 tsp ground coriander"
            },
            new RecipeIngredient
            {
                Id = 28,
                RecipeId = 7,
                Name = "1 tsp ground turmeric"
            },
            new RecipeIngredient
            {
                Id = 29,
                RecipeId = 7,
                Name = "1 tsp ground cinnamon"
            },
            new RecipeIngredient
            {
                Id = 30,
                RecipeId = 7,
                Name = "1 tbsp tomato paste"
            },
            new RecipeIngredient
            {
                Id = 31,
                RecipeId = 7,
                Name = "500g lamb mince"
            },
            new RecipeIngredient
            {
                Id = 32,
                RecipeId = 7,
                Name = "1 cup (250ml) chicken stock"
            },
            new RecipeIngredient
            {
                Id = 33,
                RecipeId = 8,
                Name = "4 tablespoons olive oil"
            },
            new RecipeIngredient
            {
                Id = 34,
                RecipeId = 8,
                Name = "1 large onion, chopped"
            },
            new RecipeIngredient
            {
                Id = 35,
                RecipeId = 8,
                Name = "3/4 cup panko breadcrumbs"
            },
            new RecipeIngredient
            {
                Id = 36,
                RecipeId = 8,
                Name = "1/2 cup grated Parmesan cheese"
            },
            new RecipeIngredient
            {
                Id = 37,
                RecipeId = 8,
                Name = "1/2 cup chopped fresh parsley"
            },
            new RecipeIngredient
            {
                Id = 38,
                RecipeId = 8,
                Name = "3/4 cup good-quality fresh ricotta"
            },
            new RecipeIngredient
            {
                Id = 39,
                RecipeId = 8,
                Name = "2 heaping tablespoons finely grated lemon zest, divided"
            },
            new RecipeIngredient
            {
                Id = 40,
                RecipeId = 8,
                Name = "1/2 cup freshly squeezed lemon juice"
            },
            new RecipeIngredient
            {
                Id = 41,
                RecipeId = 8,
                Name = "3/4 pound dry spaghetti"
            },
            new RecipeIngredient
            {
                Id = 42,
                RecipeId = 8,
                Name = "1 large or 3 small spring onion bulbs, tough outer layers removed and the rest halved and thinly sliced (about 1/2 cup)"
            },
            new RecipeIngredient
            {
                Id = 43,
                RecipeId = 8,
                Name = "1 1/2 cups fresh or frozen green peas"
            },
            new RecipeIngredient
            {
                Id = 44,
                RecipeId = 8,
                Name = "1/4 cup chicken or vegetable stock"
            },
            new RecipeIngredient
            {
                Id = 45,
                RecipeId = 9,
                Name = "2 tablespoons coconut oil"
            },
            new RecipeIngredient
            {
                Id = 46,
                RecipeId = 9,
                Name = "1 (16 ounce) package skinless, boneless chicken breast halves, cut into small cubes"
            },
            new RecipeIngredient
            {
                Id = 47,
                RecipeId = 9,
                Name = "1 (14 ounce) can cream of coconut (such as Trader Joe\u0027s\u00AE Extra Thick and Rich)"
            },
            new RecipeIngredient
            {
                Id = 48,
                RecipeId = 9,
                Name = "1 (11 ounce) bottle red Thai curry sauce (such as Trader Joe\u0027s\u00AE)"
            },
            new RecipeIngredient
            {
                Id = 49,
                RecipeId = 9,
                Name = "\u00BD (16 ounce) package dried rice stick vermicelli noodles"
            },
            new RecipeIngredient
            {
                Id = 50,
                RecipeId = 10,
                Name =       "4 skin-on, boneless chicken thighs"
            },
            new RecipeIngredient
            {
                Id = 51,
                RecipeId = 10,
                Name = "1 tablespoon olive oil"
            },
            new RecipeIngredient
            {
                Id = 52,
                RecipeId = 10,
                Name ="1 teaspoon smoked paprika"
            },
            new RecipeIngredient
            {
                Id = 53,
                RecipeId = 10,
                Name = "1 teaspoon ground cumin"
            },
            new RecipeIngredient
            {
                Id = 54,
                RecipeId = 10,
                Name = "1 teaspoon ground coriander"
            },
            new RecipeIngredient
            {
                Id = 55,
                RecipeId = 10,
                Name = "1 teaspoon ground turmeric"
            },
            new RecipeIngredient
            {
                Id = 56,
                RecipeId = 11,
                Name = "Peanut butter"
            },
            new RecipeIngredient
            {
                Id = 57,
                RecipeId = 11,
                Name = "Honey"
            },
            new RecipeIngredient
            {
                Id = 58,
                RecipeId = 11,
                Name = "Granulated sugar"
            },
            new RecipeIngredient
            {
                Id = 59,
                RecipeId = 12,
                Name = "(170g) elbow macaroni"
            },
            new RecipeIngredient
            {
                Id = 60,
                RecipeId = 12,
                Name = "1/2 cup (125ml) milk"
            },
            new RecipeIngredient
            {
                Id = 61,
                RecipeId = 12,
                Name = "1/2 cup (125ml) cream"
            },
            new RecipeIngredient
            {
                Id = 62,
                RecipeId = 12,
                Name = "1/2 cup (125ml) chicken stock"
            },
            new RecipeIngredient
            {
                Id = 63,
                RecipeId = 13,
                Name = "Butter"
            },
            new RecipeIngredient
            {
                Id = 64,
                RecipeId = 13,
                Name = "Flour"
            },
            new RecipeIngredient
            {
                Id = 65,
                RecipeId = 13,
                Name = "Milk"
            },
            new RecipeIngredient
            {
                Id = 66,
                RecipeId = 13,
                Name = "Salt"
            },
            new RecipeIngredient
            {
                Id = 67,
                RecipeId = 13,
                Name = "Cocoa Powder"
            },
            new RecipeIngredient
            {
                Id = 68,
                RecipeId = 13,
                Name = "Sugar"
            },
            new RecipeIngredient
            {
                Id = 69,
                RecipeId = 13,
                Name = "Vanilla Extract"
            }
        };
        public override void Up()
        {
            foreach (var ing in recipeIngredients)
            {
                Insert.IntoTable(Tables.Ingredient)
                    .Row(new
                    {
                        id = ing.Id,
                        recipe_id = ing.RecipeId,
                        name = ing.Name
                    });
            }
        }
        public override void Down()
        {
        }
    }
}
