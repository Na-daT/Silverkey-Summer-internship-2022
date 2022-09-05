using FluentMigrator;
using RecipesDB.Migrations;

namespace RecipesDB.Seed
{
    public class Instruction
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; }
    }
    [Migration(16)]
    public class _106_InstructionTableSeed : Migration
    {
        public static List<Instruction> instructions = new()
        {
            new Instruction
            {
                Id = 1,
                RecipeId = 1,
                Name ="Bake in three layer pans at 350 degrees.\t"
            },
            new Instruction
            {
                Id = 2,
                RecipeId = 2,
                Name = "Place apples in the pie shell"
            },
            new Instruction
            {
                Id = 3,
                RecipeId = 1,
                Name = "Mix together sugar, flour, and salt"
            },
            new Instruction
            {
                Id = 4,
                RecipeId = 2,
                Name = "Bake at 375 degrees for 45 minutes"
            },
            new Instruction
            {
                Id = 5,
                RecipeId = 3,
                Name = "Melt butter over medium heat"
            },
            new Instruction
            {
                Id = 6,
                RecipeId = 3,
                Name = "Add shrimp, mushrooms, green pepper and garlic.",
            },
            new Instruction
            {
                Id = 7,
                RecipeId = 3,
                Name = "Saute for about 3-5 minute"
            },
            new Instruction
            {
                Id = 8,
                RecipeId = 4,
                Name = "Mix milk, onion, and parley."
            },
            new Instruction
            {
                Id = 9,
                RecipeId = 4,
                Name = "add chicken to mix"
            },
            new Instruction
            {
                Id = 10,
                RecipeId = 4,
                Name = "Bake at 400 degrees for 1 hour"
            },
            new Instruction
            {
                Id = 11,
                RecipeId = 5,
                Name = "slice the potatoes"
            },
            new Instruction
            {
                Id = 12,
                RecipeId = 5,
                Name = "fry potatoes till crisp"
            },
            new Instruction
            {
                Id = 13,
                RecipeId = 5,
                Name = "fry fish"
            },
            new Instruction
            {
                Id = 14,
                RecipeId = 5,
                Name = "add both to the plate with extra chilli sauce"
            },
            new Instruction
            {
                Id = 15,
                RecipeId = 6,
                Name = "cut chicken into small parts"
            },
            new Instruction
            {
                Id = 16,
                RecipeId = 6,
                Name = "fry chicken untill golden"
            },
            new Instruction
            {
                Id = 17,
                RecipeId = 6,
                Name = "fry potatoes"
            },
            new Instruction
            {
                Id = 18,
                RecipeId = 6,
                Name = "add garlic and onion to yogurt and mix"
            },
            new Instruction
            {
                Id = 19,
                RecipeId = 7,
                Name = "Heat oil in a large saucepan over medium-high heat. Add onion , carrot and celery and cook, stirring, for 5 minutes or until soft. Add lamb mince and cook, stirring to break up any lumps, for 5 minutes or until lamb changes colour."
            },
            new Instruction
            {
                Id = 20,
                RecipeId = 7,
                Name = "Add the flour and cook, stirring, for 2 minutes or until combined. Add stock , bay leaf , Worcestershire sauce and tomato paste . Bring to the boil. Reduce heat to low and cook, stirring occasionally, for 30 minutes or until sauce thickens. Taste and season with salt and pepper ."
            },
            new Instruction
            {
                Id = 21,
                RecipeId = 7,
                Name = "Add the peas and cook for 5 minutes or until heated through. Stir in the parsley . Serve with mashed potato and steamed green beans."
            },
            new Instruction
            {
                Id = 22,
                RecipeId = 8,
                Name =       "Place a large pot of salted water over high heat and bring to a boil while you proceed with the recipe."            },
            new Instruction
            {
                Id = 23,
                RecipeId = 8,
                Name = "To toast the Panko breadcrumbs: In a large skillet over medium-high heat, heat 2 tablespoons of the olive oil. Add the Panko, and stir constantly for 3 to 5 minutes, or until the crumbs are golden. Season with salt and pepper, transfer to a bowl, and set aside. Wipe the skillet clean."
            },
            new Instruction
            {
                Id = 24,
                RecipeId = 8,
                Name = "In another small bowl, stir the ricotta"
            },
            new Instruction
            {
                Id = 25,
                RecipeId = 8,
                Name = "Add 1 heaping tablespoon of the lemon zest, and 2 tablespoons of the lemon juice together (save the rest). Add salt and pepper to taste."
            },
            new Instruction
            {
                Id = 26,
                RecipeId = 8,
                Name = "Add the spaghetti to the boiling water and set your timer for 2 minutes less than the recommended time (this is so you can be assured that you are cooking the pasta al dente)."
            },
            new Instruction
            {
                Id = 27,
                RecipeId = 8,
                Name ="Cook the spaghetti uncovered in vigorously boiling water. When done, it should be cooked, but still firm in the middle. Scoop out 1/2 cup of the pasta water for the sauce, and drain in a colander."
            },
            new Instruction
            {
                Id = 28,
                RecipeId = 8,
                Name = "In a large skillet over medium-high heat, heat the remaining 2 tablespoons of olive oil. Add the garlic and cook for 1 minute, or until fragrant. Add the shrimp and cook for 2 minutes, or until pink and opaque. Season with salt and pepper."
            },
            new Instruction
            {
                Id = 29,
                RecipeId = 8,
                Name = "Add the spaghetti to the skillet with the shrimp, and toss to combine. Add the ricotta mixture, and toss to combine. Add the reserved pasta water, and toss to combine. Add the remaining lemon zest and juice, and toss to combine. Season with salt and pepper to taste."
            },
            new Instruction
            {
                Id = 30,
                RecipeId = 8,
                Name = "Divide the pasta among 4 bowls, and top with the toasted Panko breadcrumbs. Serve immediately."
            },
            new Instruction
            {
                Id = 31,
                RecipeId = 9,
                Name = "Preheat oven to 350 degrees F (175 degrees C)."
            },
            new Instruction
            {
                Id = 32,
                RecipeId = 9,
                Name = "Fill a large pot with lightly salted water and bring to a rolling boil; stir in vermicelli pasta and return to a boil. Cook pasta uncovered, stirring occasionally, until the pasta is tender yet firm to the bite, 4 to 5 minutes. Drain.",
            },
            new Instruction
            {
                Id = 33,
                RecipeId = 9,
                Name = "Reduce skillet heat to simmer. Add the noodles and let simmer until flavors are absorbed, about 5 minutes. Divide chicken and noodles among individual serving bowls."
            },
            new Instruction
            {
                Id = 34,
                RecipeId = 10,
                Name = "Preheat oven to 350 degrees F (175 degrees C)."
            },
            new Instruction
            {
                Id = 35,
                RecipeId = 10,
                Name = "Pat chicken thighs dry with a paper towel and brush skins with olive oil. Place chicken thighs, skin-side down, in a single layer on a plate."
            },
            new Instruction
            {
                Id = 36,
                RecipeId = 10,
                Name = "Season chicken thighs with salt and pepper. Place chicken thighs, skin-side down, in a single layer on a plate."
            },
            new Instruction
            {
                Id = 37,
                RecipeId = 10,
                Name = "Place chicken thighs, skin-side down, in a single layer on a plate."
            },
            new Instruction
            {
                Id = 38,
                RecipeId = 11,
                Name = "Start by placing the three ingredients together in a bowl"
            },
            new Instruction
            {
                Id = 39,
                RecipeId = 11,
                Name = "Mix the ingredients together until they are well combined"
            },
            new Instruction
            {
                Id = 40,
                RecipeId = 11,
                Name = "Place the mixture in the fridge for 30 minutes"
            },
            new Instruction
            {
                Id = 41,
                RecipeId = 11,
                Name = "Flatten the cookies with your hands or the bottom of a glass, then use the back of a fork to press down and create criss-cross lines"
            },
            new Instruction
            {
                Id = 42,
                RecipeId = 11,
                Name = "Place the cookies on a baking sheet and bake for 10 minutes"
            },
            new Instruction
            {
                Id = 43,
                RecipeId = 11,
                Name = "Remove the cookies from the oven and let them cool for 5 minutes before transferring them to a wire rack to cool completely"
            },
            new Instruction
            {
                Id = 44,
                RecipeId = 12,
                Name =  "Place macaroni in a medium saucepan or skillet and add just enough cold water to cover. Add a pinch of salt and bring to a boil over high heat, stirring frequently. Continue to cook, stirring, until water has been almost completely absorbed and macaroni is just shy of al dente, about 6 minutes."
            },
            new Instruction
            {
                Id = 45,
                RecipeId = 12,
                Name = "Immediately add evaporated milk and bring to a boil. Add cheese. Reduce heat to low and cook, stirring continuously, until cheese is melted and liquid has reduced to a creamy sauce, about 2 minutes longer. Season to taste with more salt and serve immediately."
            },
            new Instruction
            {
                Id = 46,
                RecipeId = 13,
                Name = "Preheat oven to 350 degrees F (175 degrees C)."
            },
            new Instruction
            {
                Id = 47,
                RecipeId = 13,
                Name = "Melt the butter with sugar, salt, and cocoa powder"
            },
            new Instruction
            {
                Id = 48,
                RecipeId = 13,
                Name = "Add the eggs and vanilla extract"
            },
            new Instruction
            {
                Id = 49,
                RecipeId = 13,
                Name = "Add the flour and baking powder"
            },
            new Instruction
            {
                Id = 50,
                RecipeId = 13,
                Name = "Add the chocolate chips"
            },
            new Instruction
            {
                Id = 51,
                RecipeId = 13,
                Name = "Bake for 15 minutes"
            }
        };
        public override void Up()
        {
            foreach (var instruction in instructions)
            {
                Insert.IntoTable(Tables.Instruction).Row(new
                {
                    id = instruction.Id,
                    recipe_id = instruction.RecipeId,
                    name = instruction.Name
                });
            }
        }
        public override void Down()
        {
        }
    }
}
