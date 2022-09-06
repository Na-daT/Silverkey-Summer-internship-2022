using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public record RecipeIngredient
    {
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
                RecipeId = 1,
                Name = "sugar"
            },
            new RecipeIngredient
            {
                RecipeId = 1,
                Name = "eggs"
            },
            new RecipeIngredient
            {
                RecipeId = 1,
                Name = "sour milk\t"
            },
            new RecipeIngredient
            {
                RecipeId = 2,
                Name = "Apple"
            },
            new RecipeIngredient
            {
                RecipeId = 2,
                Name = "Flour"
            },
            new RecipeIngredient
            {
                RecipeId = 2,
                Name = "Sugar"
            },
            new RecipeIngredient
            {
                RecipeId = 2,
                Name = "Salt"
            },
            new RecipeIngredient
            {
                RecipeId = 3,
                Name = "shrimp"
            },
            new RecipeIngredient
            {
                RecipeId = 3,
                Name = "garlic"
            },
            new RecipeIngredient
            {
                RecipeId = 3,
                Name = "mushrooms"
            },
            new RecipeIngredient
            {
                RecipeId = 3,
                Name = "green pepper"
            },
            new RecipeIngredient
            {
                RecipeId = 4,
                Name = "chicken breasts"
            },
            new RecipeIngredient
            {
                RecipeId = 4,
                Name = "milk"
            },
            new RecipeIngredient
            {
                RecipeId = 4,
                Name = "onion"
            },
            new RecipeIngredient
            {
                RecipeId = 4,
                Name = "parley"
            },
            new RecipeIngredient
            {
                RecipeId = 5,
                Name = "fish sticks"
            },
            new RecipeIngredient
            {
                RecipeId = 5,
                Name = "potatoes"
            },
            new RecipeIngredient
            {
                RecipeId = 5,
                Name = "onion"
            },
            new RecipeIngredient
            {
                RecipeId = 5,
                Name = "milk"
            },
            new RecipeIngredient
            {
                RecipeId = 6,
                Name = "chicken strips"
            },
            new RecipeIngredient
            {
                RecipeId = 6,
                Name = "garlic"
            },
            new RecipeIngredient
            {
                RecipeId = 6,
                Name = "onion"
            },
            new RecipeIngredient
            {
                RecipeId = 6,
                Name = "fries"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "1 tbsp olive oil"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "2 garlic cloves, crushed"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "2 tsp ground cumin"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "2 tsp ground coriander"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "1 tsp ground turmeric"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "1 tsp ground cinnamon"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "1 tbsp tomato paste"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "500g lamb mince"
            },
            new RecipeIngredient
            {
                RecipeId = 7,
                Name = "1 cup (250ml) chicken stock"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "4 tablespoons olive oil"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1 large onion, chopped"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "3/4 cup panko breadcrumbs"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1/2 cup grated Parmesan cheese"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1/2 cup chopped fresh parsley"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "3/4 cup good-quality fresh ricotta"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "2 heaping tablespoons finely grated lemon zest, divided"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1/2 cup freshly squeezed lemon juice"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "3/4 pound dry spaghetti"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1 large or 3 small spring onion bulbs, tough outer layers removed and the rest halved and thinly sliced (about 1/2 cup)"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1 1/2 cups fresh or frozen green peas"
            },
            new RecipeIngredient
            {
                RecipeId = 8,
                Name = "1/4 cup chicken or vegetable stock"
            },
            new RecipeIngredient
            {
                RecipeId = 9,
                Name = "2 tablespoons coconut oil"
            },
            new RecipeIngredient
            {
                RecipeId = 9,
                Name = "1 (16 ounce) package skinless, boneless chicken breast halves, cut into small cubes"
            },
            new RecipeIngredient
            {
                RecipeId = 9,
                Name = "1 (14 ounce) can cream of coconut (such as Trader Joe\u0027s\u00AE Extra Thick and Rich)"
            },
            new RecipeIngredient
            {
                RecipeId = 9,
                Name = "1 (11 ounce) bottle red Thai curry sauce (such as Trader Joe\u0027s\u00AE)"
            },
            new RecipeIngredient
            {
                RecipeId = 9,
                Name = "\u00BD (16 ounce) package dried rice stick vermicelli noodles"
            },
            new RecipeIngredient
            {
                RecipeId = 10,
                Name =       "4 skin-on, boneless chicken thighs"
            },
            new RecipeIngredient
            {
                RecipeId = 10,
                Name = "1 tablespoon olive oil"
            },
            new RecipeIngredient
            {
                RecipeId = 10,
                Name ="1 teaspoon smoked paprika"
            },
            new RecipeIngredient
            {
                RecipeId = 10,
                Name = "1 teaspoon ground cumin"
            },
            new RecipeIngredient
            {
                RecipeId = 10,
                Name = "1 teaspoon ground coriander"
            },
            new RecipeIngredient
            {
                RecipeId = 10,
                Name = "1 teaspoon ground turmeric"
            },
            new RecipeIngredient
            {
                RecipeId = 11,
                Name = "Peanut butter"
            },
            new RecipeIngredient
            {
                RecipeId = 11,
                Name = "Honey"
            },
            new RecipeIngredient
            {
                RecipeId = 11,
                Name = "Granulated sugar"
            },
            new RecipeIngredient
            {
                RecipeId = 12,
                Name = "(170g) elbow macaroni"
            },
            new RecipeIngredient
            {
                RecipeId = 12,
                Name = "1/2 cup (125ml) milk"
            },
            new RecipeIngredient
            {
                RecipeId = 12,
                Name = "1/2 cup (125ml) cream"
            },
            new RecipeIngredient
            {
                RecipeId = 12,
                Name = "1/2 cup (125ml) chicken stock"
            },
            new RecipeIngredient
            {
                RecipeId = 13,
                Name = "Butter"
            },
            new RecipeIngredient
            {
                RecipeId = 13,
                Name = "Flour"
            },
            new RecipeIngredient
            {
                RecipeId = 13,
                Name = "Milk"
            },
            new RecipeIngredient
            {
                RecipeId = 13,
                Name = "Salt"
            },
            new RecipeIngredient
            {
                RecipeId = 13,
                Name = "Cocoa Powder"
            },
            new RecipeIngredient
            {
                RecipeId = 13,
                Name = "Sugar"
            },
            new RecipeIngredient
            {
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
