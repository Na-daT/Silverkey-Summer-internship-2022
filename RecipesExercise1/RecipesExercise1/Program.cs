using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RecipesExercise1;

Console.WriteLine("yalahwy");
List<Recipe> recipesList = new List<Recipe>();
recipesList.Add(new Recipe("Recipe 1", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<string>() { "Category 1", "Category 2" }));
recipesList.Add(new Recipe("Recipe 2", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<string>() { "Category 1", "Category 2" }));
recipesList.Add(new Recipe("Recipe 3", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<string>() { "Category 1", "Category 2" }));
recipesList.Add(new Recipe("Recipe 4", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<string>() { "Category 1", "Category 2" }));
recipesList.Add(new Recipe("Recipe 5", new List<string>() { "Ingredient 1", "Ingredient 2" }, new List<string>() { "Instruction 1", "Instruction 2" }, new List<string>() { "Category 1", "Category 2" }));
Recipe.SaveRecipes(recipesList);
List<Recipe> newList = await Recipe.GetRecipes();

foreach (var i in newList)
{
    Console.WriteLine(i.Title);
}