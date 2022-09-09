using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public record RecipeCategory
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }
    }

    [Migration(14)]
    public class _0104_RecipeCategoryTableSeed : Migration
    {
        public static List<RecipeCategory> recipeCategories = new()
        {
            new RecipeCategory
            {
                RecipeId = 1,
                CategoryId = 1
            },
            new RecipeCategory
            {
                RecipeId = 1,
                CategoryId = 2
            },
            new RecipeCategory
            {
                RecipeId = 1,
                CategoryId = 3
            },
            new RecipeCategory
            {
                RecipeId = 1,
                CategoryId = 4
            },
            new RecipeCategory
            {
                RecipeId = 1,
                CategoryId = 5
            },
            new RecipeCategory
            {
                RecipeId = 2,
                CategoryId = 1
            },
            new RecipeCategory
            {
                RecipeId = 2,
                CategoryId = 2
            },
            new RecipeCategory
            {
                RecipeId = 2,
                CategoryId = 3
            },
            new RecipeCategory
            {
                RecipeId =  2,
                CategoryId =  4
            },
            new RecipeCategory
            {
                RecipeId =  2 ,
                CategoryId =  5
            },
            new RecipeCategory
            {
                RecipeId =  3 ,
                CategoryId =  11
            },
            new RecipeCategory
            {
                RecipeId =  3 ,
                CategoryId =  4
            },
            new RecipeCategory
            {
                RecipeId =  3 ,
                CategoryId =  6
            },
            new RecipeCategory
            {
                RecipeId =  3 ,
                CategoryId =  9
            },
            new RecipeCategory
            {
                RecipeId =  4 ,
                CategoryId =  7
            },
            new RecipeCategory
            {
                RecipeId =  4 ,
                CategoryId =  12
            },
            new RecipeCategory
            {
                RecipeId =  5 ,
                CategoryId =  8
            },
            new RecipeCategory
            {
                RecipeId =  5 ,
                CategoryId =  9
            },
            new RecipeCategory
            {
                RecipeId =  5 ,
                CategoryId =  10
            },
            new RecipeCategory
            {
                RecipeId =  5 ,
                CategoryId =  11
            },
            new RecipeCategory
            {
                RecipeId =  6 ,
                CategoryId =  12
            },
            new RecipeCategory
            {
                RecipeId =  6 ,
                CategoryId =  13
            },
            new RecipeCategory
            {
                RecipeId =  6 ,
                CategoryId =  14
            },
            new RecipeCategory
            {
                RecipeId =  7 ,
                CategoryId =  15
            },
            new RecipeCategory
            {
                RecipeId =  7 ,
                CategoryId =  16
            },
            new RecipeCategory
            {
                RecipeId =  8 ,
                CategoryId =  20
            },
            new RecipeCategory
            {
                RecipeId =  8 ,
                CategoryId =  21
            },
            new RecipeCategory
            {
                RecipeId =  9 ,
                CategoryId =  14
            },
            new RecipeCategory
            {
                RecipeId =  9 ,
                CategoryId =  15
            },
            new RecipeCategory
            {
                RecipeId =  10 ,
                CategoryId =  2
            },
            new RecipeCategory
            {
                RecipeId =  10 ,
                CategoryId =  3
            },
            new RecipeCategory
            {
                RecipeId =  11 ,
                CategoryId =  4
            },
            new RecipeCategory
            {
                RecipeId =  11 ,
                CategoryId =  5
            },
            new RecipeCategory
            {
                RecipeId =  12 ,
                CategoryId =  6
            },
            new RecipeCategory
            {
                RecipeId =  12 ,
                CategoryId =  7
            },
            new RecipeCategory
            {
                RecipeId =  13 ,
                CategoryId =  8
            },
            new RecipeCategory
            {
                RecipeId =  13 ,
                CategoryId =  9
            }
        };
        public override void Up()
        {
            foreach (var recipeCategory in recipeCategories)
            {
                Insert.IntoTable(Tables.RecipeCategory).Row(new
                {
                    recipe_id = recipeCategory.RecipeId,
                    category_id = recipeCategory.CategoryId
                });
            }
        }
        public override void Down()
        {
        }
    }
}
