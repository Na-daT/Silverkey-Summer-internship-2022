using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeExercise5.Models;
using FluentValidation.Results;
using FluentValidation.AspNetCore;

namespace RecipeExercise5.Pages
{
    public class EditRecipeModel : PageModel
    {
        private readonly ILogger<EditRecipeModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private Recipe.RecipeValidator validator = new Recipe.RecipeValidator();

        [BindProperty]
        public Recipe UpdatedRecipe { get; set; } = new();

        [BindProperty]
        public List<Category> Categories { get; set; } = new();

        [BindProperty]
        public List<Guid> CategoriesIds { get; set; } = new();

        public EditRecipeModel(ILogger<EditRecipeModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("recipeClient");
            var recipesList = await httpClient.GetFromJsonAsync<List<Recipe>>("recipe");
            if (recipesList is null)
                throw new Exception("Could not get categories");
            UpdatedRecipe = recipesList.Find(c => c.Id.ToString() == id);
            if (UpdatedRecipe is null)
                throw new Exception("Could not find recipe");
            var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
            if (categoriesList is null)
                throw new Exception("Could not get categories");
            Categories = categoriesList;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();
            var httpClient = _httpClientFactory.CreateClient("recipeClient");
            var categoriesList = await httpClient.GetFromJsonAsync<List<Category>>("category");
            if (categoriesList is null)
                throw new Exception("Could not get categories");
            UpdatedRecipe = Recipe.MatchCategory(UpdatedRecipe, categoriesList, CategoriesIds);
            var recipeToUpdate = new Recipe { Id = Guid.Parse(id), Title = UpdatedRecipe.Title, Instructions = UpdatedRecipe.Instructions, Ingredients = UpdatedRecipe.Ingredients, Categories = UpdatedRecipe.Categories };
            var result = await httpClient.PutAsJsonAsync("recipes", recipeToUpdate);
            if (result.IsSuccessStatusCode)
                return RedirectToPage("/Recipes/List");
            throw new Exception("Could not update recipe");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var HttpClient = _httpClientFactory.CreateClient("recipeClient");
            var result = await HttpClient.DeleteAsync($"recipes/{id}");
            if (result.IsSuccessStatusCode)
                return RedirectToPage("/Recipes/List");
            throw new Exception("Could not delete recipe");
        }
    }
}