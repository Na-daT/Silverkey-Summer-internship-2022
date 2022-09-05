using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public class RecipeCategory
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }
    }

    [Migration(14)]
    public class _104_RecipeCategoryTableSeed : Migration
    {
        public static List<RecipeCategory> recipeCategories = new()
        {
            new RecipeCategory
            {
                Id = 1,
                RecipeId = 1,
                CategoryId = 1
            },
            new RecipeCategory
            {
                Id = 2,
                RecipeId = 1,
                CategoryId = 2
            },
            new RecipeCategory
            {
                Id = 3,
                RecipeId = 1,
                CategoryId = 3
            },
            new RecipeCategory
            {
                Id = 4,
                RecipeId = 1,
                CategoryId = 4
            },
            new RecipeCategory
            {
                Id = 5,
                RecipeId = 1,
                CategoryId = 5
            },
            new RecipeCategory
            {
                Id = 6,
                RecipeId = 2,
                CategoryId = 1
            },
            new RecipeCategory
            {
                Id = 7,
                RecipeId = 2,
                CategoryId = 2
            },
            new RecipeCategory
            {
                Id = 8,
                RecipeId = 2,
                CategoryId = 3
            },
            new RecipeCategory
            {
                Id = 9 ,
                RecipeId =  2,
                CategoryId =  4 
            },
            new RecipeCategory
            {
                Id =  10 ,
                RecipeId =  2 ,
                CategoryId =  5 
            },
            new RecipeCategory
            {
                Id =  12 ,
                RecipeId =  3 ,
                CategoryId =  11 
            },
            new RecipeCategory
            {
                Id =  13 ,
                RecipeId =  3 ,
                CategoryId =  4 
            },
            new RecipeCategory
            {
                Id =  14 ,
                RecipeId =  3 ,
                CategoryId =  6 
            },
            new RecipeCategory
            {
                Id =  15 ,
                RecipeId =  3 ,
                CategoryId =  9 
            },
            new RecipeCategory
            {
                Id =  16 ,
                RecipeId =  4 ,
                CategoryId =  7 
            },
            new RecipeCategory
            {
                Id =  17 ,
                RecipeId =  4 ,
                CategoryId =  12 
            },
            new RecipeCategory
            {
                Id =  21 ,
                RecipeId =  5 ,
                CategoryId =  8 
            },
            new RecipeCategory
            {
                Id =  22 ,
                RecipeId =  5 ,
                CategoryId =  9 
            },
            new RecipeCategory
            {
                Id =  23 ,
                RecipeId =  5 ,
                CategoryId =  10 
            },
            new RecipeCategory
            {
                Id =  24 ,
                RecipeId =  5 ,
                CategoryId =  11 
            },
            new RecipeCategory
            {
                Id =  25 ,
                RecipeId =  6 ,
                CategoryId =  12 
            },
            new RecipeCategory
            {
                Id =  26 ,
                RecipeId =  6 ,
                CategoryId =  13 
            },
            new RecipeCategory
            {
                Id =  27 ,
                RecipeId =  6 ,
                CategoryId =  14 
            },
            new RecipeCategory
            {
                Id =  28 ,
                RecipeId =  7 ,
                CategoryId =  15 
            },
            new RecipeCategory
            {
                Id =  29 ,
                RecipeId =  7 ,
                CategoryId =  16 
            },
            new RecipeCategory
            {
                Id =  30 ,
                RecipeId =  8 ,
                CategoryId =  20 
            },
            new RecipeCategory
            {
                Id =  31 ,
                RecipeId =  8 ,
                CategoryId =  21 
            },
            new RecipeCategory
            {
                Id =  32 ,
                RecipeId =  9 ,
                CategoryId =  14 
            },
            new RecipeCategory
            {
                Id =  33 ,
                RecipeId =  9 ,
                CategoryId =  15 
            },
            new RecipeCategory
            {
                Id =  34 ,
                RecipeId =  10 ,
                CategoryId =  2 
            },
            new RecipeCategory
            {
                Id =  35 ,
                RecipeId =  10 ,
                CategoryId =  3 
            },
            new RecipeCategory
            {
                Id =  36 ,
                RecipeId =  11 ,
                CategoryId =  4 
            },
            new RecipeCategory
            {
                Id =  37 ,
                RecipeId =  11 ,
                CategoryId =  5 
            },
            new RecipeCategory
            {
                Id =  38 ,
                RecipeId =  12 ,
                CategoryId =  6 
            },
            new RecipeCategory
            {
                Id =  39 ,
                RecipeId =  12 ,
                CategoryId =  7 
            },
            new RecipeCategory
            {
                Id =  40 ,
                RecipeId =  13 ,
                CategoryId =  8 
            },
            new RecipeCategory
            {
                Id =  41 ,
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
                    id = recipeCategory.Id,
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
