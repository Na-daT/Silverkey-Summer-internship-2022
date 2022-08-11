namespace Client_Razor.Utility
{
    public class Utility
    {
        public Utility()
        {  
        }
        public static List<Recipe> Load(List<Category> categories, List<Recipe> recipes)
        {
            ArgumentNullException.ThrowIfNull(categories);
            ArgumentNullException.ThrowIfNull(recipes);
            for (int i = 0; i < recipes.Count; i++)
                for (int j = 0; j < recipes[i].Categories.Count; j++)
                    recipes[i].Categories[j] = categories.First(y => y.Id == recipes[i].Categories[j].Id);

            return recipes;
        }
        public static Recipe MatchCategory(Recipe recipe, List<Category> categories, List<string> categoriesIds)
        {
            ArgumentNullException.ThrowIfNull(recipe);
            ArgumentNullException.ThrowIfNull(categories);
            ArgumentNullException.ThrowIfNull(categoriesIds);
            for (int i = 0; i < categoriesIds.Count; i++)
            {
                var category = categories.FirstOrDefault(y => y.Id == categoriesIds[i]);
                if (category is null)
                    throw new Exception("Could not find category");
                recipe.Categories.Add(category);
            }
            return recipe;
        }
    }
}
